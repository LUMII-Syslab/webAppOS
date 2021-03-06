package org.webappos.webproc;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.webappos.server.API;
import org.webappos.webcaller.IJsonWebCallsAdapter;
import org.webappos.webcaller.IWebCaller.CallingConventions;
import org.webappos.webcaller.IWebCaller.WebCallDeclaration;
import org.webappos.webcaller.IWebCaller.WebCallSeed;
import org.webappos.webcaller.IWebMemWebCallsAdapter;
import org.webappos.webmem.IWebMemory;
import org.webappos.webmem.WebMemoryContext;

import java.io.DataInputStream;
import java.io.IOException;
import java.lang.reflect.Method;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;


public class DefaultWebProcessor extends UnicastRemoteObject implements IRWebProcessor {
	private String id;
	private static Logger logger =  LoggerFactory.getLogger(DefaultWebProcessor.class);

	protected DefaultWebProcessor(String _id) throws RemoteException {
		super();
		id = _id;
	}
	
	private static final long serialVersionUID = 1L;

	@Override
	public boolean hasInstructionSet(String name) throws RemoteException {
		Class<?> adapterClass = null;
				
		try {
			adapterClass = Class.forName("org.webappos.adapters.webcalls."+name+".WebCallsAdapter");					
		}
		catch(Throwable t) {					
		}
		
		return adapterClass != null;
	}

	@Override
	public boolean perProjectCachedInstructionSet(String instructionSet) throws RemoteException {
		Class<?> adapterClass = null;
		try {
			adapterClass = Class.forName("org.webappos.adapters.webcalls."+instructionSet+".WebCallsAdapter");					
			return adapterClass.getMethod("clearCache", String.class) != null;
		}
		catch(Throwable t) {
			return false;
		}
	}
	
	@Override
	public void clearCachedInstructionSet(String project_id, String instructionSet) throws RemoteException {
		Class<?> adapterClass = null;
		try {
			adapterClass = Class.forName("org.webappos.adapters.webcalls."+instructionSet+".WebCallsAdapter");					
			Method m = adapterClass.getMethod("clearCache", String.class);
			if (m!=null)
				m.invoke(project_id);
		}
		catch(Throwable t) {
			return;
		}
	}

	private static ScheduledExecutorService scheduler = Executors.newScheduledThreadPool(1);
	
	@Override
	public void startWebCall(WebCallSeed seed, WebCallDeclaration action) throws RemoteException {
		logger.trace("Webcall "+seed.actionName+" ("+seed.hashCode()+") starting @ "+id);
		scheduler.schedule(new Runnable() {

			@Override
			public void run() {		
				logger.trace("Webcall "+seed.actionName+" ("+seed.hashCode()+") running @ "+id);
				try {
					Class<?> adapterClass = null;
					Object adapter = null;
							
					try {
						adapterClass = Class.forName("org.webappos.adapters.webcalls."+action.resolvedInstructionSet+".WebCallsAdapter");					
						adapter = adapterClass.getConstructor().newInstance();
					}
					catch(Throwable t) {					
					}
					
					if (adapter == null)
						return;
					
					IWebMemory webmem = API.dataMemory.getWebMemory(seed.project_id);
					String jsonResult = null;
													
					if ((seed.callingConventions == CallingConventions.JSONCALL) && (adapter instanceof IJsonWebCallsAdapter)) {
						boolean newOwner = false;
						if (webmem!=null && webmem.getContext()==null) {
							newOwner = true;
							WebMemoryContext ctx = new WebMemoryContext();
							ctx.login = seed.login;
							ctx.project_id = seed.project_id;
							webmem.setContext(ctx);
						}
						try {
							jsonResult = ((IJsonWebCallsAdapter)adapter).jsoncall(action.resolvedLocation, action.pwd, seed.jsonArgument, seed.project_id, API.dataMemory.getProjectFullAppName(seed.project_id), seed.login);
						}
						catch(Throwable t) {
							jsonResult = "{\"error\":\""+t.getMessage()+"\"}";
						}
						if (newOwner)
							webmem.setContext(null);
					}
					else
					if ((seed.callingConventions == CallingConventions.WEBMEMCALL) && (adapter instanceof IWebMemWebCallsAdapter)) {
						boolean newOwner = false;
						if (webmem!=null && webmem.getContext()==null) {
							newOwner = true;
							WebMemoryContext ctx = new WebMemoryContext();
							ctx.login = seed.login;
							ctx.project_id = seed.project_id;
							webmem.setContext(ctx);							
						}
						
						try {
							((IWebMemWebCallsAdapter)adapter).webmemcall(action.resolvedLocation, action.pwd, seed.webmemArgument, webmem, seed.project_id, API.dataMemory.getProjectFullAppName(seed.project_id), seed.login);
						}
						catch(Throwable t) {
							jsonResult = "{\"error\":\""+t.getMessage()+"\"}";
						}
						if (newOwner) {
							webmem.setContext(null);
						}
					}
					else {
						logger.error("Could not peform server-side web call "+seed.actionName+" within web processor `"+id+"'since calling conventions do not match. ");
					}
					
					//if (webmem!=null)
						//webmem.flush();
					
					API.wpbService.webCallFinished(id, jsonResult);
				} catch (Throwable t) {
					logger.trace("Webcall "+seed.actionName+" ("+seed.hashCode()+") finished with an exception ("+t.getMessage()+") @ "+id);
					System.exit(0);
				}
				logger.trace("Webcall "+seed.actionName+" ("+seed.hashCode()+") finished @ "+id);
			}
		}, 0, TimeUnit.MILLISECONDS);
		logger.trace("Webcall "+seed.actionName+" ("+seed.hashCode()+") started @ "+id);
	}

	@Override
	public void disconnect() {
		logger.info("Web processor `"+id+"' terminated because disconnect() called.");
		System.exit(0);
	}
	
	/**
	 * @param args args[0] is the id of the web processor; args[1] is the address of the Web Processor Bus Service (URI syntax, "rmi:" protocol);
	 *             there can be optional arguments args[2], args[3], which specify web processor parameters (options from webproctab) 
	 */
	public static void main(String[] args) {
				
		if (args.length<2) {
			logger.error("DefaultWebProcessor could not start because not all arguments were specified.");
			return;
		}

		IRWebProcessor current;
		try {
			current = new DefaultWebProcessor(args[0]);
		} catch (RemoteException e1) {
			logger.info("Initialization failed for web processor `"+args[0]+"'. Exiting...");
			return;
		}
		
		logger.info("Web processor `"+args[0]+"' started. Connecting to Web Processor Bus Service at "+args[1]+"...");
		API.initAPI(args[0], args[1], current, false); // do not register

		// considering options 
		for (int i=2; i<args.length; i++) {
			if (args[i].startsWith("load=")) {				
				try {
					Class.forName("org.webappos.adapters.webcalls."+args[i].substring(5)+".WebCallsAdapter");					
				}
				catch(Throwable t) {
					t.printStackTrace();
				}						
			}
		}

		// registering
		try {
			API.wpbService.registerWebProcessor(args[0], current);
		} catch (RemoteException t) {
			logger.error("Could not register web processor.");
			System.exit(0);
		}
		
		try {
			new DataInputStream(System.in).readUTF();
		} catch (IOException e) {
			logger.error("Web processor `"+args[0]+"' terminated because the parent died.");
			System.exit(0);
		} // waiting...
	}


}
