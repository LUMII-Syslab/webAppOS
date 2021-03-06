package org.webappos.server;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.List;
import java.util.Scanner;
import java.util.Set;

import org.eclipse.jetty.http.HttpVersion;
import org.eclipse.jetty.proxy.ProxyServlet;
import org.eclipse.jetty.server.HttpConfiguration;
import org.eclipse.jetty.server.HttpConnectionFactory;
import org.eclipse.jetty.server.SecureRequestCustomizer;
import org.eclipse.jetty.server.Server;
import org.eclipse.jetty.server.ServerConnector;
import org.eclipse.jetty.server.SslConnectionFactory;
import org.eclipse.jetty.server.handler.ContextHandler;
import org.eclipse.jetty.server.handler.ContextHandlerCollection;
import org.eclipse.jetty.servlet.DefaultServlet;
import org.eclipse.jetty.servlet.ServletContextHandler;
import org.eclipse.jetty.servlet.ServletHolder;
import org.eclipse.jetty.util.resource.ResourceCollection;
import org.eclipse.jetty.util.ssl.SslContextFactory;
import org.eclipse.jetty.webapp.WebAppContext;
import org.webappos.bridge.BridgeSocket;
import org.webappos.properties.WebAppProperties;
import org.webappos.properties.PropertiesManager;
import org.webappos.properties.SomeProperties;
import org.webappos.properties.WebLibraryProperties;
import org.webappos.properties.WebServiceProperties;
import org.webappos.webcaller.WebCaller;
import org.webappos.util.PID;
import org.webappos.util.Ports;

import com.google.gson.JsonElement;
import com.google.gson.JsonPrimitive;

import org.slf4j.*;

public class Gate {
	
	private static Logger logger =  LoggerFactory.getLogger(Gate.class);
	
	private static Server webServer = null;
	private static ContextHandlerCollection handlerColl = new ContextHandlerCollection();//new HandlerCollection(true); // mutable when running

	
	
	private static SslContextFactory sslContextFactory = null;
	synchronized public static void reloadCerts() {
		try {
			File certsDir = new File(ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs");
			if (!certsDir.exists())
				certsDir.mkdirs();
			if (sslContextFactory == null) {		
				sslContextFactory = new SslContextFactory();
				logger.info("Loading certs from "+ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_KEY_FILE_NAME +" and "+ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_CHAIN_FILE_NAME);
				KeyStore keyStore = CertBot4j.PEMToKeyStore(ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_KEY_FILE_NAME, ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_CHAIN_FILE_NAME);
				sslContextFactory.setKeyStore(keyStore);
				
				sslContextFactory.setExcludeCipherSuites("SSL_RSA_WITH_DES_CBC_SHA",
		                "SSL_DHE_RSA_WITH_DES_CBC_SHA", "SSL_DHE_DSS_WITH_DES_CBC_SHA",
		                "SSL_RSA_EXPORT_WITH_RC4_40_MD5",
		                "SSL_RSA_EXPORT_WITH_DES40_CBC_SHA",
		                "SSL_DHE_RSA_EXPORT_WITH_DES40_CBC_SHA",
		                "SSL_DHE_DSS_EXPORT_WITH_DES40_CBC_SHA");
				
			}
			else {		 
				logger.info("Re-loading certs from "+ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_KEY_FILE_NAME +" and "+ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_CHAIN_FILE_NAME);
				sslContextFactory.reload(newSslContextFactory -> { 
					try {
						sslContextFactory.setKeyStore(CertBot4j.PEMToKeyStore(ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_KEY_FILE_NAME, ConfigStatic.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+CertBot4j.DOMAIN_CHAIN_FILE_NAME));
					} catch (CertificateException | KeyStoreException | NoSuchAlgorithmException | IOException e) {
						logger.error("Could not reload certificates: "+e.getMessage());
					}
				});
			}
		} catch (Exception e) {
			logger.error("Could not initialize/reload certificates: "+e.getMessage());
		}
	}
	
	private static IAppAdapter getAppAdapter(String name) {
		try {
			return (IAppAdapter) Class.forName("org.webappos.adapters.app."+name+".AppAdapter").getConstructor().newInstance();
		} catch (Throwable e) {
			logger.error("Could not load app adapter `"+name+"'. Reason: "+e.getMessage());
			return null;
		}
	}

	private static IServiceAdapter getServiceAdapter(String name) {
		try {
			return (IServiceAdapter) Class.forName("org.webappos.adapters.service."+name+".ServiceAdapter").getConstructor().newInstance();
		} catch (Throwable e) {
			logger.error("Could not load service adapter `"+name+"'. Reason: "+e.getMessage());
			return null;
		}
	}
	
	private static void serviceHalted(String name) {
		System.err.println("GATE: service "+name+" halted.");
	}
	
	private static void serviceStopped(String name) {
		System.err.println("GATE: service "+name+" stopped.");		
	}
	
	private static Runnable getOnHalted(String name) {
		return new Runnable() {

			@Override
			public void run() {
				serviceHalted(name);
			}
			
		};
	}
	
	private static Runnable getOnStopped(String name) {
		return new Runnable() {

			@Override
			public void run() {
				serviceStopped(name);
			}
			
		};
	}
	
	private static boolean depsAttached(SomeProperties props) {
		for (String depName: props.all_required) {
			if (API.status.getValue("apps/"+depName+"/error")!=null) {
				logger.error("Could not load "+props.id+" because of a dependency error.");
				API.status.setValue("apps/"+props.id+"/error", "Dependency error ("+depName+")");
				return false;
			}
		}
		return true;		
	}

	synchronized public static void attachWebAppOrService(String name) { // reads app/service config, adds sub-domain and sub-path for the given app/service

		assert API.propertiesManager instanceof PropertiesManager;
		assert API.webCaller instanceof WebCaller;
		assert API.config instanceof ConfigEx;
		
		if (name==null) {
			logger.error("Could not attach null app/service.");
			return;
		}
		boolean isApp = name.endsWith(".webapp");
		if (!isApp) {
			if (!name.endsWith(".webservice")) {
				logger.error("Could not attach "+name+" since it is neither an app, nor a service.");
				return;
			}
		}
		
		API.status.setValue("server/free_port", ((ConfigEx)API.config).free_port);
		
		
		if (isApp) {	
			// app
			WebAppProperties appProps = API.propertiesManager.getWebAppPropertiesByFullName(name);
			if (appProps == null) {
				logger.error("Could not load app "+name+".");
				API.status.setValue("apps/"+name+"/error", "Could not load app properties.");
				return;
			}
						
			if (!depsAttached(appProps)) // sets the status to an error
				return;				
			
			// adding .webcalls functions provided by this app
			((WebCaller)API.webCaller).loadWebCalls(appProps);
			
			IAppAdapter appAdapter = getAppAdapter(appProps.app_type);
			if (appAdapter == null) {
				logger.error("Could not load app "+name+": unknown app type "+appProps.app_type);
				API.status.setValue("apps/"+name+"/error", "Unknown app type "+appProps.app_type);
				return;
			}

			
			// loading web calls of required web libraries
			for (String lib : appProps.all_required_web_libraries) {
				WebLibraryProperties props = API.propertiesManager.getWebLibraryPropertiesByFullName(lib);
				if (props!=null)
					((WebCaller)API.webCaller).loadWebCalls(props);
			}
			

			if (API.config.hasOnlyIP) {
				// only IP specified
				
				if (appProps.requires_root_url_paths) {
					// we don't have a domain, only IP, thus we need to launch the app at the specific port...
					
					int port = Gate.getFreePort(((ConfigEx)API.config).free_port);
					int securePort = -1;
					if (port < 0) {
						logger.error("Could not find a suitable port "+((ConfigEx)API.config).free_port+"+i for "+name);
						API.status.setValue("apps/"+name+"/error", "Could not find free port.");
						return;
					}
					else {
						((ConfigEx)API.config).free_port = port+1;
						API.status.setValue("server/free_port", ((ConfigEx)API.config).free_port);
					}
					
					
			        Server portServer = new Server();
			        
					HttpConfiguration http_config = new HttpConfiguration();
					if (API.config.secure) {
						securePort = Gate.getFreePort(((ConfigEx)API.config).free_port);
						if (securePort < 0) {
							logger.error("Could not find a suitable secure port "+((ConfigEx)API.config).free_port+"+i for "+name);
							API.status.setValue("apps/"+name+"/error", "Could not find free port (secure).");
							return;
						}
						else {
							((ConfigEx)API.config).free_port = securePort+1;
							API.status.setValue("server/free_port", ((ConfigEx)API.config).free_port);
						}
						http_config.setSecureScheme("https");
						http_config.setSecurePort(securePort);
					}
			        http_config.setOutputBufferSize(32768);
			        http_config.setRequestHeaderSize(8192);
			        http_config.setResponseHeaderSize(8192);
			        http_config.setSendServerVersion(true);
			        http_config.setSendDateHeader(false);
			        
			        ServerConnector http = new ServerConnector(portServer,
			                new HttpConnectionFactory(http_config));
			        http.setPort(port);
			        http.setIdleTimeout(30000);
			        
			        portServer.addConnector(http);
			        
			        if (API.config.secure) {
						HttpConfiguration https_config = new HttpConfiguration(http_config);
					    https_config.addCustomizer(new SecureRequestCustomizer());
						
						ServerConnector sslConnector = new ServerConnector(portServer,
					            new SslConnectionFactory(sslContextFactory,HttpVersion.HTTP_1_1.asString()),
					            new HttpConnectionFactory(https_config));
					    sslConnector.setPort(securePort);
					    portServer.addConnector(sslConnector);			        	
			        }
					
					ContextHandler appContextHandler = new ContextHandler("/");
					// divContextHandler.setVirtualHosts - not required for port-based apps
					ContextHandler appContext;
					try {
						 appContext = appAdapter.attachApp(appProps);
					}
					catch(Throwable t) {
						logger.error("Could not attach "+appProps.app_full_name+". "+t.getMessage());
						API.status.setValue("apps/"+name+"/error", "Could not attach app context.");
						return;
					}
					appContextHandler.setHandler(appContext);

					portServer.setHandler(appContextHandler);
					try {
						portServer.start();
						logger.info("Attached "+appProps.app_full_name+" at HTTP port "+port);
						API.status.setValue("apps/"+name+"/port", port);
						if (API.config.secure) {
							logger.info("Attached "+appProps.app_full_name+" at HTTPS port "+securePort);
							API.status.setValue("apps/"+name+"/secure_port", securePort);
						}
					} catch (Exception e) {
						API.status.setValue("apps/"+name+"/error", "Could not attach port server.");
					}
	
	
					// redirect to port...
					
					ServletContextHandler portRedirectHandler =  new ServletContextHandler(handlerColl, "/apps/"+appProps.app_url_name, false, false);
					ServletHolder holder2 = new ServletHolder();
					holder2.setServlet(new RedirectServlet(API.config.simple_domain_or_ip, port, securePort)); // w/o protocol
			        portRedirectHandler.addServlet(holder2, "/*");
					try {
						portRedirectHandler.start();
						logger.info("Attached "+appProps.app_full_name+" redirect from /apps/"+appProps.app_url_name+" to "+API.config.simple_domain_or_ip+":"+port+(API.config.secure?"["+(securePort)+"]":""));
						API.status.setValue("apps/"+name+"/bindings",
								 "[\""
						             +"/apps/"+appProps.app_url_name+" -> "+API.config.simple_domain_or_ip+":"+port+(API.config.secure?"["+(securePort)+"]":"")
						        +"\"]");
					} catch (Exception e) {
						API.status.setValue("apps/"+name+"/error", "Could not establish redirect from /apps/"+appProps.app_url_name+": "+e.getMessage());
					}
										
				}			
				else {
					// only IP specified, but the app does not require root paths; thus, sub-domain is not necessary...
					ContextHandler appContext;
					try {
						 appContext = appAdapter.attachApp(appProps);
					}
					catch(Throwable t) {
						logger.error("Could not attach "+appProps.app_full_name+". "+t.getMessage());
						API.status.setValue("apps/"+name+"/error", "Could not attach app context.");
						return;
					}
					ContextHandler appContextHandler = new ContextHandler("/apps/"+appProps.app_url_name);				
					appContextHandler.setHandler(appContext);
					handlerColl.addHandler(appContextHandler);
					try {
						appContextHandler.start();
						logger.info("Attached "+appProps.app_full_name+" at /apps/"+appProps.app_url_name);
						API.status.setValue("apps/"+name+"/bindings", "[\"/apps/"+appProps.app_url_name+"\"]");
					} catch (Exception e) {
						API.status.setValue("apps/"+name+"/error", "Could not bind to /apps/"+appProps.app_url_name+": "+e.getMessage());
					}								
				}													
			}
			else {
				// domain specified, all OK
				ContextHandler appContextHandler = new ContextHandler("/");
				
				appContextHandler.setVirtualHosts(new String[]{appProps.app_url_name+"."+API.config.simple_domain_or_ip, appProps.app_url_name+".localhost", "*."+appProps.app_url_name+"."+API.config.simple_domain_or_ip, "*."+appProps.app_url_name+".localhost"});
				
				ContextHandler appContext;
				try {
					 appContext = appAdapter.attachApp(appProps);
				}
				catch(Throwable t) {
					logger.error("Could not attach "+appProps.app_full_name+". "+t.getMessage());
					API.status.setValue("apps/"+name+"/error", "Could not attach.");
					return;
				}
				appContextHandler.setHandler(appContext);
				
				ArrayList<String> bindings = new ArrayList<String>();
				
				handlerColl.addHandler(appContextHandler);
				try {
					appContextHandler.start();
					logger.info("Attached "+appProps.app_full_name+" at "+appProps.app_url_name+"."+API.config.simple_domain_or_ip+" and "+appProps.app_url_name+".localhost");
					bindings.add(appProps.app_url_name+"."+API.config.simple_domain_or_ip);
					bindings.add(appProps.app_url_name+".localhost");
				} catch (Exception e) {
					API.status.setValue("apps/"+name+"/error", "Could not bind to "+appProps.app_url_name+"."+API.config.simple_domain_or_ip+" and "+appProps.app_url_name+".localhost");
				}
		
		
				// /apps/urlName redirect
				
				ServletContextHandler appRedirectHandler = new ServletContextHandler(handlerColl, "/apps/"+appProps.app_url_name, false, false);
				ServletHolder holder = new ServletHolder();
				holder.setServlet(new RedirectServlet(appProps.app_url_name+"."+API.config.simple_domain_or_ip, -1, -1));
		        appRedirectHandler.addServlet(holder, "/*");
				try {
					appRedirectHandler.start();
					logger.info("Attached "+appProps.app_full_name+" redirect from /apps/"+appProps.app_url_name+" to "+appProps.app_url_name+"."+API.config.simple_domain_or_ip);
					bindings.add("/apps/"+appProps.app_url_name+" -> "+appProps.app_url_name+"."+API.config.simple_domain_or_ip);
				} catch (Exception e) {
					API.status.setValue("apps/"+name+"/error", "Could not establish redirect from /apps/"+appProps.app_url_name+": "+e.getMessage());
				}
				
				if (bindings.size()>0) {
					API.status.setValue("apps/"+name+"/bindings", "[\""+String.join("\", \"", bindings)+"\"]");
				}
			}
		} // if isApp
		else {			
			API.status.setValue("apps/"+name+"/status", "stopped");
			
			// service
			WebServiceProperties svcProps = API.propertiesManager.getWebServicePropertiesByFullName(name);
			if (svcProps == null) {
				logger.error("Could not load service "+name+".");
				API.status.setValue("app/"+name+"/error", "Could not load service properties.");
				return;
			}
			
			if (!depsAttached(svcProps)) // sets the status to an error
				return;				
			
			// adding .webcalls functions provided by this service
			((WebCaller)API.webCaller).loadWebCalls(svcProps);
			
			JsonElement startup_type = API.registry.getValue("apps/"+name+"/startup_type");
			if (startup_type==null) {
				API.registry.setValue("apps/"+name+"/startup_type", "auto");
				startup_type = new JsonPrimitive("auto");
			}
			
			if (!"auto".equalsIgnoreCase(startup_type.getAsString())) {
				return;
			}
			
			IServiceAdapter svcAdapter = getServiceAdapter(svcProps.service_type);
			if (svcAdapter == null) {				
				logger.error("Could not load serivice "+name+": unknown service type "+svcProps.service_type);
				API.status.setValue("apps/"+name+"/error", "Unknown service type "+svcProps.service_type);
				return;
			}
			
			// Assigning additional ports...
			for (int k=0; k<svcProps.requires_additional_ports.length; k++) {
				if (svcProps.requires_additional_ports[k]<0) {
					int addPort = Gate.getFreePort( ((ConfigEx)API.config).free_port );
					if (addPort >=0 ) {
						svcProps.requires_additional_ports[k] = addPort;
						((ConfigEx)API.config).free_port = addPort+1;
						API.status.setValue("server/free_port", ((ConfigEx)API.config).free_port);
					}
					else {
						API.status.setValue("apps/"+name+"/error", "Could not find additional free port.");
						return;
					}
				}
			}			
			
			
			if (!API.config.hasOnlyIP) {
				// domain specified
				
				// attaching subdomain
		        ContextHandler handler;
		        try {
		        	handler = svcAdapter.attachService(svcProps, "/", getOnStopped(svcProps.service_full_name), getOnHalted(svcProps.service_full_name));
			        if (handler == null)
			        	handler = new org.webappos.adapters.service.webroot.ServiceAdapter().attachService(svcProps, "/", null, null);			        
		        }
		        catch(Throwable t) {
					logger.error("Could not attach "+svcProps.service_full_name+". "+t.getMessage());
					API.status.setValue("apps/"+name+"/error", "Could not attach for /");
					return;
		        }
		        
		        API.status.setValue("apps/"+name+"/status", "running");
		        
		        ArrayList<String> bindings = new ArrayList<String>();
		        ArrayList<String> errors = new ArrayList<String>();
		        
				handler.setVirtualHosts(new String[]{"*."+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip, "*."+svcProps.service_url_name+"_service.localhost"});
				handlerColl.addHandler(handler);
				try {
					handler.start();
					logger.info("Binding OK for "+svcProps.service_full_name+" at "+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" and "+svcProps.service_url_name+"_service.localhost");
					bindings.add("*."+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip);
					bindings.add("*."+svcProps.service_url_name+"_service.localhost");
				} catch (Exception e) {
					logger.error("Could not bind "+svcProps.service_full_name+" to "+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" and "+svcProps.service_url_name+"_service.localhost");
					errors.add("Could not bind "+svcProps.service_full_name+" to "+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" and "+svcProps.service_url_name+"_service.localhost");					
				}
		
						
				// attaching /services/url_name
				boolean err=false;
		        try {
		        	handler = svcAdapter.attachService(svcProps, "/services/"+svcProps.service_url_name, getOnStopped(svcProps.service_full_name), getOnHalted(svcProps.service_full_name));
			        if (handler == null)
			        	handler = new org.webappos.adapters.service.webroot.ServiceAdapter().attachService(svcProps, "/", null, null);
		        }
		        catch(Throwable t) {
					logger.error("Could not attach "+svcProps.service_full_name+". "+t.getMessage());
					errors.add("Could not attach for /services/"+svcProps.service_url_name);
					err = true;
		        }
		        
		        if (!err) {
					handler.setVirtualHosts(new String[] { "*."+API.config.simple_domain_or_ip, "*.localhost", "*."+API.config.simple_domain_or_ip });
					handlerColl.addHandler(handler);
					try {
						handler.start();
						logger.info("Binding OK for "+svcProps.service_full_name+" at /services/"+svcProps.service_url_name);
						bindings.add(API.config.simple_domain_or_ip+"/services/"+svcProps.service_url_name);
						bindings.add("localhost/services/"+svcProps.service_url_name);
					} catch (Exception e) {
						logger.error("Could not bind"+svcProps.service_full_name+" at /services/"+svcProps.service_url_name);
						errors.add("Could not bind"+svcProps.service_full_name+" at /services/"+svcProps.service_url_name);
					}
		        }
				

				if (bindings.size()>0) {
					API.status.setValue("apps/"+name+"/bindings", "[\""+String.join("\", \"", bindings)+"\"]");
				}
				if (errors.size()>0) {
					API.status.setValue("apps/"+name+"/error", String.join("; ", errors));
				}

				return;
			}
			
						
			// only IP specified (additional HTTP/HTTPS ports MAY be required)								
							
			
			if (!svcProps.requires_root_url_paths) {
				// only IP specified, but the service does not require root paths
		        ContextHandler handler;
		        try {
		        	handler = svcAdapter.attachService(svcProps, "/services/"+svcProps.service_url_name, getOnStopped(svcProps.service_full_name), getOnHalted(svcProps.service_full_name));
			        if (handler == null)
			        	handler = new org.webappos.adapters.service.webroot.ServiceAdapter().attachService(svcProps, "/services/"+svcProps.service_url_name, null, null);
		        }
		        catch(Throwable t) {
					logger.error("Could not attach "+svcProps.service_full_name+". "+t.getMessage());
					API.status.setValue("apps/"+name+"/error", "Could not attach.");
					return;
		        }
				
		        API.status.setValue("apps/"+name+"/status", "running");
		        
				handlerColl.addHandler(handler);
				try {
					handler.start();
					logger.info("Binding OK for "+svcProps.service_full_name+" at /services/"+svcProps.service_url_name);
					API.status.setValue("apps/"+name+"/bindings", "[\"/services/"+svcProps.service_url_name+"\"]");
				} catch (Exception e) {
					logger.error("Could not start handler for "+svcProps.service_full_name+". "+e.getMessage());
					API.status.setValue("apps/"+name+"/error", "Could not bind at /services/"+svcProps.service_url_name);
				}								
				return;
			}					
			
			
			// HTTP ports will be used!
			
	        ContextHandler handler = null;
	        try {
	        	handler = svcAdapter.attachService(svcProps, "/", getOnStopped(svcProps.service_full_name), getOnHalted(svcProps.service_full_name));
		        if (handler == null)
		        	handler = new org.webappos.adapters.service.webroot.ServiceAdapter().attachService(svcProps, "/", null, null);
	        }
	        catch(Throwable t) {
				logger.error("Could not attach "+svcProps.service_full_name+". "+t.getMessage());
				API.status.setValue("apps/"+name+"/error", "Could not attach.");
				return;
	        }
	        
	        API.status.setValue("apps/"+name+"/status", "running");

	        ArrayList<String> bindings = new ArrayList<String>();
	        ArrayList<String> errors = new ArrayList<String>();
		        
	        if (svcProps.httpPort<0) {
	        	// the service adapter did not create ports; we need to provide the HTTP (and, perhaps, HTTPS) ports for the handler
	        						
	        	// attaching our ports...
        		svcProps.httpPort = Gate.getFreePort(((ConfigEx)API.config).free_port);
				if (svcProps.httpPort < 0) {
					logger.error("Could not find a suitable port "+((ConfigEx)API.config).free_port+"+i for "+name);
					API.status.setValue("apps/"+name+"/error", "Could not find free port.");
					return;
				}
				else {
					((ConfigEx)API.config).free_port = svcProps.httpPort+1;
					API.status.setValue("server/free_port", ((ConfigEx)API.config).free_port);
				}
			
				Server portServer = null;
		        portServer = new Server();
				HttpConfiguration http_config = new HttpConfiguration();
				if (API.config.secure) {
					svcProps.httpsPort = Gate.getFreePort(((ConfigEx)API.config).free_port);
					if (svcProps.httpsPort < 0) {
						logger.error("Could not find a suitable secure port "+((ConfigEx)API.config).free_port+"+i for "+name);
						API.status.setValue("apps/"+name+"/error", "Could not find free port (secure).");
						return;
					}
					else {
						((ConfigEx)API.config).free_port = svcProps.httpsPort+1;
						API.status.setValue("server/free_port", ((ConfigEx)API.config).free_port);
					}
					http_config.setSecureScheme("https");
					http_config.setSecurePort(svcProps.httpsPort);
				}
		        http_config.setOutputBufferSize(32768);
		        http_config.setRequestHeaderSize(8192);
		        http_config.setResponseHeaderSize(8192);
		        http_config.setSendServerVersion(true);
		        http_config.setSendDateHeader(false);
		        
		        ServerConnector http = new ServerConnector(portServer,
		                new HttpConnectionFactory(http_config));
		        http.setPort(svcProps.httpPort);
		        http.setIdleTimeout(30000);
		        
		        portServer.addConnector(http);
		        
		        if (API.config.secure) {
					HttpConfiguration https_config = new HttpConfiguration(http_config);
				    https_config.addCustomizer(new SecureRequestCustomizer());
					
					ServerConnector sslConnector = new ServerConnector(portServer,
				            new SslConnectionFactory(sslContextFactory,HttpVersion.HTTP_1_1.asString()),
				            new HttpConnectionFactory(https_config));
				    sslConnector.setPort(svcProps.httpsPort);
				    portServer.addConnector(sslConnector);			        	
		        }
	        	
				portServer.setHandler(handler);
				try {
					portServer.start();
					if (API.config.secure) {
						logger.info("Port server OK for "+svcProps.service_full_name+" at HTTP port "+svcProps.httpPort);
						logger.info("Port server OK for "+svcProps.service_full_name+" at HTTPS port "+svcProps.httpsPort);
						bindings.add(":"+svcProps.httpPort);
						bindings.add(":"+svcProps.httpsPort);
					}
					else {
						logger.info("Port server OK for "+svcProps.service_full_name+" at HTTP port "+svcProps.httpPort);
						bindings.add(":"+svcProps.httpPort);
					}
				} catch (Exception e) {
					String s;
					if (API.config.secure)
						s = "Could not bind automatic ports "+svcProps.httpPort+" and "+svcProps.httpsPort;
					else
						s = "Could not bind automatic port "+svcProps.httpPort;
					
					logger.error(s);
					errors.add(s);
				}
	        }		        
		        
		    // Now we either have the http/https port(s). Configuring redirects and subdomains.
		        
			// redirect to these ports...
			ServletContextHandler portRedirectHandler =  new ServletContextHandler(handlerColl, "/services/"+svcProps.service_url_name, false, false);
			ServletHolder holder2 = new ServletHolder();

			if (svcProps.httpsPort>0)
				holder2.setServlet(new RedirectServlet(API.config.simple_domain_or_ip, svcProps.httpPort, svcProps.httpsPort));
			else
				holder2.setServlet(new RedirectServlet(API.config.simple_domain_or_ip, svcProps.httpPort));
	        portRedirectHandler.addServlet(holder2, "/*");
			try {
				portRedirectHandler.start();
				logger.info("Binding OK "+svcProps.service_full_name+" simple redirect from /services/"+svcProps.service_url_name+" to "+API.config.simple_domain_or_ip+":"+svcProps.httpPort+(API.config.secure?"["+(svcProps.httpsPort)+"]":""));
				bindings.add("/services/"+svcProps.service_url_name+" -> "+API.config.simple_domain_or_ip+":"+svcProps.httpPort+(API.config.secure?"["+(svcProps.httpsPort)+"]":""));
			} catch (Exception e) {
				errors.add("Could not bind /services/"+svcProps.service_url_name+" -> "+API.config.simple_domain_or_ip+":"+svcProps.httpPort+(API.config.secure?"["+(svcProps.httpsPort)+"]":""));
			}
			
			// subdomain proxy to port...
			if (!API.config.hasOnlyIP) {
				// create subdomain with proxy servlet...
				ServletContextHandler subdomainProxyHandler =						
						new ServletContextHandler(handlerColl, "/", ServletContextHandler.SESSIONS);
				
				ServletHolder proxyServlet = new ServletHolder(ProxyServlet.Transparent.class);
				
				subdomainProxyHandler.setVirtualHosts(new String[]{svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip, svcProps.service_url_name+"_service.localhost", "*."+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip, "*."+svcProps.service_url_name+"_service.localhost"});
											
				if (svcProps.httpsPort>=0) {
					proxyServlet.setInitParameter("proxyTo", "https://localhost:"+svcProps.httpsPort+"/");
				}
				else {
					proxyServlet.setInitParameter("proxyTo", "http://localhost:"+svcProps.httpPort+"/");
				}
				proxyServlet.setInitParameter("Prefix", "/");
		        subdomainProxyHandler.addServlet(proxyServlet, "/*");
				try {
					subdomainProxyHandler.start();
					bindings.add(svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" <-> localhost:"+svcProps.httpPort+(API.config.secure?"["+svcProps.httpsPort+"]":""));
					logger.info("Binding OK for "+svcProps.service_full_name+" via proxy from "+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" to localhost:"+svcProps.httpPort+(API.config.secure?"["+svcProps.httpsPort+"]":""));
				} catch (Exception e) {
					logger.error("Could not bind "+svcProps.service_full_name+" via proxy from "+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" to localhost:"+svcProps.httpPort+(API.config.secure?"["+svcProps.httpsPort+"]":""));
					errors.add("Could not bind "+svcProps.service_full_name+" via proxy from "+svcProps.service_url_name+"_service."+API.config.simple_domain_or_ip+" to localhost:"+svcProps.httpPort+(API.config.secure?"["+svcProps.httpsPort+"]":""));
				}
				
			}
			
			if (bindings.size()>0) {
				API.status.setValue("apps/"+name+"/bindings", "[\""+String.join("\", \"", bindings)+"\"]");
			}
			if (errors.size()>0) {
				API.status.setValue("apps/"+name+"/error", String.join("; ", errors));
			}
		}
	}

	synchronized public static void detachWebAppOrService(String name) {
		
	}
	
	synchronized private static int getFreePort(int startFrom) {
		int i=0;
		while ((i<10) && (Ports.portTaken(startFrom+i)) && (Ports.portTaken(startFrom+i+1))) {
			i++;
		}
		
		if (i>=10) {
			return -1;
		}
		return startFrom+i;
	}
	
	private static String TMP_DIR; 
	static {	
		try {
			File tempFile = File.createTempFile("webappos", ".tmp");
			TMP_DIR = tempFile.getParent();
			tempFile.delete();
		} catch (IOException e) {
			TMP_DIR = ConfigStatic.ROOT_DIR+File.separator+"tmp";
		}
	}
		
	private static long[] getLastPidAndPort() {
		try {
			Scanner scanner = null;
			try {
				scanner = new Scanner(new File(TMP_DIR + File.separator + "webappos.pid"));				
			return new long[] { scanner.nextLong(), scanner.nextInt() };
			}
			finally {
				if (scanner!=null)
					scanner.close();
			}
		}
		catch(Throwable t) {
			return null;
		}
	}
	
	private static void writePidAndPort(long pid, long port) {
		FileWriter w;
		try {
			File f = new File(TMP_DIR + File.separator + "webappos.pid");
			w = new FileWriter(f);
			w.write(pid+" "+port);
			w.close();
			f.deleteOnExit();
		} catch (IOException e) {
		}
	}
	
	private static void configure(int port) {
		
		JsonElement el = API.registry.getValue("xusers/admin");
		if (el == null) {
			// creating a new admin user
			//API.registry.setValue("xusers/admin", )
		}
		//Browser.openURL("http://localhost:"+pp[1]+"/apps/configuration");
		
	}
	
	private static void DFS(SomeProperties v, Set<SomeProperties> discovered, List<SomeProperties> topologicalOrder) {
		if (discovered.contains(v))
			return;
		
		discovered.add(v);
		for (String req : v.requires) {
			SomeProperties w = API.propertiesManager.getPropertiesByFullName(req);
			if (w==null) {
				logger.error("Could not find the dependency "+req+" required by "+v.id);
				API.status.setValue("apps/"+req+"/error", "Not found (required by "+v.id+").");
			}
			else {
				if (!discovered.contains(w))
					DFS(w, discovered, topologicalOrder);
			}
		}
		
		topologicalOrder.add(v);
	}
	
	private static void topologicalSort(List<SomeProperties> list) {				
		Set<SomeProperties> discovered = new HashSet<SomeProperties>();
		List<SomeProperties> topologicalOrder = new LinkedList<SomeProperties>();
		
		for (SomeProperties v: list) {
			DFS(v, discovered, topologicalOrder);
		}
		
		list.clear();
		list.addAll(topologicalOrder);
	}

	public static void main(String[] args) {
		
		
        if ((args.length>0) && (args[0].equals("configure"))) {
        	// check if already running...
        	long[] pp = getLastPidAndPort();
        	
        	if (pp != null) {        			
    			if (PID.isRunning(pp[0]) && Ports.portTaken((int)pp[1])) {
    				// some webAppOS instance is running; opening the configuration app within the web browser...
    				
    				API.initOfflineAPI(); // we need it for accessing the registry

    				configure((int)pp[1]);
    				return;
    			}
        	}
        }
		
		API.initAPI();
		
		// start server...
		
		
		int i=0;
		while ((i<10) && (Ports.portTaken(API.config.port+i))) {
			i++;
		}
		
		if (i>=10) {
			logger.error("Could not find a suitable port "+API.config.port+"+i.");
			return;
		}
							
		API.config.port += i;
		
		// storing pid and port
		writePidAndPort(PID.getPID(), API.config.port);


		i=0;
		while ((i<10) && ((API.config.port==API.config.secure_port+i) || (Ports.portTaken(API.config.secure_port+i)))) {
			i++;
		}
		
		if (i>=10) {
			logger.error("Could not find a suitable secure port "+API.config.secure_port+"+i.");
			return;
		}
		API.config.secure_port += i;
		
		try {
			webServer = new Server();

			///// HTTP /////
			
			HttpConfiguration http_config = new HttpConfiguration();
	        http_config.setSecureScheme("https");
	        http_config.setSecurePort(API.config.secure_port);
	        http_config.setOutputBufferSize(32768);
	        http_config.setRequestHeaderSize(8192);
	        http_config.setResponseHeaderSize(8192);
	        http_config.setSendServerVersion(true);
	        http_config.setSendDateHeader(false);
	        
	        ServerConnector http = new ServerConnector(webServer,
	                new HttpConnectionFactory(http_config));
	        http.setPort(API.config.port);
	        http.setIdleTimeout(30000);
	        
	        webServer.addConnector(http);
	        logger.info("PORT is "+API.config.port);
	        API.status.setValue("server/port", API.config.port);
	        

			
			webServer.setHandler(handlerColl);			
//			webServer.start();
			
			// MAIN (FIRST-LEVEL DOMAIN) context
			
			WebAppContext mainContext = new WebAppContext();
			mainContext.setWar(API.config.WEB_ROOT_DIR);
			mainContext.setContextPath("/");		
			
			File dir = new File(ConfigStatic.WEB_ROOT_CACHE_DIR);
			if (!dir.exists())
				dir.mkdirs();
			
			File acmeDir = new File(API.config.ETC_DIR+File.separator+"acme"+File.separator+"web-root");
			if (!acmeDir.exists())
				acmeDir.mkdirs();
			
	        // web-root + acme-challenge web-root...	       	       
			ResourceCollection mainResources = new ResourceCollection(new String[] {
				API.config.WEB_ROOT_DIR, // must always present, then other folders in the search path follow
				acmeDir.getAbsolutePath(),
				dir.getAbsolutePath(),
			});
			mainContext.setBaseResource(mainResources);
						
			// NO CACHE
			ServletHolder holder = new ServletHolder(new DefaultServlet());
			holder.setInitParameter("useFileMappedBuffer", "false");
			holder.setInitParameter("cacheControl", "max-age=0, public");
			mainContext.addServlet(holder, "/");
			mainContext.getMimeTypes().addMimeMapping("mjs", "application/javascript");
			
			
			ContextHandler mainContextHandler;
			mainContextHandler = new ContextHandler("/");

			if (API.config.hasOnlyIP) { 
				mainContextHandler.setVirtualHosts(new String[]{"localhost"});
			}
			else {
				mainContextHandler.setVirtualHosts(new String[]{API.config.simple_domain_or_ip, "localhost"});
			}
			
			handlerColl.addHandler(mainContext);
			
			webServer.start();
		
			
			///// HTTPS /////
	        
			if (API.config.secure) {				
				while (!webServer.isStarted()) {
					Thread.sleep(100);
				}
		        CertBot4j.ensureCertificates(API.config.acme_url, API.config.simple_domain_or_ip, API.config.ETC_DIR+File.separator+"acme"+File.separator+"certs", API.config.ETC_DIR+File.separator+"acme"+File.separator+"web-root", API.config.acme_renew_interval, new Runnable() {
		
					@Override
					public void run() {
						// Just renewed certs...
						reloadCerts();
						
						String os = System.getProperty("os.name");
						File f;
						if ((os!=null) && (os.indexOf("Windows")>=0))
							f = new File(API.config.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+"onrenew.bat");
						else
							f = new File(API.config.ETC_DIR+File.separator+"acme"+File.separator+"certs"+File.separator+"onrenew.sh");
						if (f.exists()) {
							logger.info("Executing "+f.getAbsolutePath()+"...");
							try {
								Process p = Runtime.getRuntime().exec(f.getAbsolutePath());
								p.waitFor();
								logger.info("Finished "+f.getAbsolutePath());
							} catch (Throwable e) {
								logger.error("Error while executing "+f.getAbsolutePath());
							}
							
						}
						else
							logger.info("Script "+f.getAbsolutePath()+" not found, but that's fine unless you need to do external actions with new certificates.");
					}
		        	
		        });		

		        webServer.stop();
				while (!webServer.isStopped()) {
					Thread.sleep(100);
				}				
				reloadCerts(); // initializes sslContextFactory
								
				HttpConfiguration https_config = new HttpConfiguration(http_config);
			    https_config.addCustomizer(new SecureRequestCustomizer());
				
				ServerConnector sslConnector = new ServerConnector(webServer,
			            new SslConnectionFactory(sslContextFactory,HttpVersion.HTTP_1_1.asString()),
			            new HttpConnectionFactory(https_config));
			    sslConnector.setPort(API.config.secure_port);
			    webServer.addConnector(sslConnector);
				
		        
				logger.info("SECURE_PORT is "+API.config.secure_port);
				API.status.setValue("server/secure_port", API.config.secure_port);
		        webServer.start();
			}
			
			
			
			// Topological sort of apps&services, and attaching them... (web libraries are considered individually within each app/service requiring them)
			
			List<SomeProperties> list = new ArrayList<SomeProperties>();
			for (File f : new File(ConfigStatic.APPS_DIR).listFiles()) {
				SomeProperties props = API.propertiesManager.getPropertiesByFullName(f.getName());
				if (props != null)
					list.add(props);
			}
			
			topologicalSort(list);
			
			if (logger.isDebugEnabled()) {
				System.out.println("Topological order:");
				for (SomeProperties props : list) {
					System.out.println("  "+props.id);				
					System.out.println("     deps: "+props.requires);				
					System.out.println("     all deps: "+props.all_required);				
					System.out.println("     all required web libs: "+props.all_required_web_libraries);				
				}
			}
			for (SomeProperties props : list) {
				if ((props instanceof WebAppProperties) || (props instanceof WebServiceProperties))
					attachWebAppOrService(props.id);
			}
			

			// ws://domain:port/ws/
			
	        ServletContextHandler wsContextHandler = new ServletContextHandler(handlerColl, "/ws", true, true); // First Server!!!
	        			        
	        
	        ServletHolder holderEvents = new ServletHolder("ws-events", BridgeSocket.Servlet.class);
	        wsContextHandler.addServlet(holderEvents, "/*");  // the path will be ws://domain.org/ws/ or wss://domain.org/ws/  - the trailing slash is mandatory!	       	        	        
	        
	        wsContextHandler.start();


	        if ((args.length>0) && (args[0].equals("configure"))) {
		        int tries = 100;
				while (tries>0 && !webServer.isStarted()) {
					tries--;
					Thread.sleep(100);
				}
				
				if (!webServer.isStarted()) {
					System.err.println("webAppOS server could not be started");
					System.exit(-1);
				}
				
				configure(API.config.port);				
	        }
	        
		} catch (Throwable e) {
			System.err.println("Error initializing webAppOS");
			e.printStackTrace();
			System.exit(-1);
		}
		

	}

}
