package lv.lumii.tda.webgde;

import java.util.Iterator;
import java.util.ListIterator;

import org.codehaus.jettison.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.webappos.server.API;
import org.webappos.webcaller.WebCaller;
import org.webappos.webmem.IWebMemory;
import org.webappos.webmem.WebMemoryContext;

import lv.lumii.tda.ee.mm.Option;


public class GraphDiagramEngine_webcalls {
	
	
	private static Logger logger =  LoggerFactory.getLogger(GraphDiagramEngine_webcalls.class);
	
	public static boolean onFrameActivatedEvent(IWebMemory raapi, long r)
	{
		lv.lumii.tda.ee.mm.EnvironmentEngineMetamodelFactory eeFactory = new lv.lumii.tda.ee.mm.EnvironmentEngineMetamodelFactory();
		try {
			eeFactory.setRAAPI(raapi, "", true);
		} catch (Throwable e) {
			return false;
		}
		lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory gdeFactory = new lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory();
		try {
			gdeFactory.setRAAPI(raapi, "", true);
		} catch (Throwable e) {
			eeFactory.unsetRAAPI();
			return false;
		}
		
		lv.lumii.tda.ee.mm.FrameActivatedEvent ev = (lv.lumii.tda.ee.mm.FrameActivatedEvent)eeFactory.findOrCreateRAAPIReferenceWrapper(r, false); 
		lv.lumii.tda.ee.mm.Frame frame = ev.getFrame();
		if (frame==null) {
			eeFactory.unsetRAAPI();
			return false;
		}
		lv.lumii.tda.webgde.mm.Frame frame2 = (lv.lumii.tda.webgde.mm.Frame)gdeFactory.findOrCreateRAAPIReferenceWrapper(frame.getRAAPIReference(), false);
		
		lv.lumii.tda.webgde.mm.CurrentDgrPointer ptr = lv.lumii.tda.webgde.mm.CurrentDgrPointer.firstObject(gdeFactory);
		if (ptr == null)
			ptr = gdeFactory.createCurrentDgrPointer();
		
		lv.lumii.tda.webgde.mm.GraphDiagram oldDgr = null;
		if (!ptr.getGraphDiagram().isEmpty())
			oldDgr = ptr.getGraphDiagram().get(0);
		
		// removing old toolbar...
		try {
		if (oldDgr != null) {			 
			if ((oldDgr.getToolbar()!=null) && !oldDgr.getToolbar().isEmpty()) {
				lv.lumii.tda.webgde.mm.Toolbar t = oldDgr.getToolbar().get(0);
				
				Iterator<lv.lumii.tda.ee.mm.Option> it_o = (Iterator<lv.lumii.tda.ee.mm.Option>) lv.lumii.tda.ee.mm.Option.allObjects(eeFactory).iterator();
				ListIterator<lv.lumii.tda.webgde.mm.ToolbarElement> it_te = t.getToolbarElement().listIterator();//.iterator();
				
				lv.lumii.tda.ee.mm.Option o = null;
				lv.lumii.tda.webgde.mm.ToolbarElement te = null;
				

				while (it_te.hasNext()) {
					
					te = it_te.next();
					
					while (it_o.hasNext()) {
						o = it_o.next();
						if (("GDEOPTION"+te.getRAAPIReference()).equals(o.getId())) {
							o.delete();
							break;
						}
					}
				}
			}
		}
		}
		catch(Throwable t) {
			logger.error(t.toString()+", "+t.getMessage());			
		}
		
		ptr.getGraphDiagram().clear();
		
		// linking to the current graph diagram...
		if (frame2.getGraphDiagram().size()>0) {
			ptr.getGraphDiagram().add(frame2.getGraphDiagram().get(0));
		}	
			
		
		// adding new toolbar...
		if (!frame2.getGraphDiagram().get(0).getToolbar().isEmpty()) {
			lv.lumii.tda.webgde.mm.Toolbar t = frame2.getGraphDiagram().get(0).getToolbar().get(0);
			
			Iterator<lv.lumii.tda.ee.mm.Option> it_o = (Iterator<Option>) lv.lumii.tda.ee.mm.Option.allObjects(eeFactory).iterator();
			Iterator<lv.lumii.tda.webgde.mm.ToolbarElement> it_te = t.getToolbarElement().iterator();
			
			lv.lumii.tda.ee.mm.Option o = null;
			lv.lumii.tda.webgde.mm.ToolbarElement te = null;

			try {
			while (it_te.hasNext()) {
				te = it_te.next();
				o = eeFactory.createOption();
				o.setId("GDEOPTION"+te.getRAAPIReference());
				o.setCaption(te.getCaption());
				o.setImage(te.getPicture());
				o.setLocation("TOOLBAR");
				o.setOnOptionSelectedEvent("GDE.defaultHandlerForOptionSelectedEvent");
				
				o.setFrame(frame);
				lv.lumii.tda.ee.mm.EnvironmentEngine ee = lv.lumii.tda.ee.mm.EnvironmentEngine.firstObject(eeFactory);
				ee.getOption().add(o);
				
			}
			}
			catch(Throwable tt) {
				tt.printStackTrace();
			}
		}
		
		// issuing RefreshOptionsCommand...
		lv.lumii.tda.ee.mm.RefreshOptionsCommand cmd = eeFactory.createRefreshOptionsCommand();
		cmd.setEnvironmentEngine(lv.lumii.tda.ee.mm.EnvironmentEngine.firstObject(eeFactory));
		cmd.submit();		
		eeFactory.unsetRAAPI();
		gdeFactory.unsetRAAPI();
		
		return true;
	}
	
	public static boolean onOptionSelectedEvent(IWebMemory raapi, long r) { // for converting to ToolbarElementSelectEvent
		
		lv.lumii.tda.ee.mm.EnvironmentEngineMetamodelFactory eeFactory = new lv.lumii.tda.ee.mm.EnvironmentEngineMetamodelFactory();
		try {
			eeFactory.setRAAPI(raapi, "", true);
		} catch (Throwable e) {
			return false;
		}
		lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory gdeFactory = new lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory();
		try {
			gdeFactory.setRAAPI(raapi, "", true);
		} catch (Throwable e) {
			eeFactory.unsetRAAPI();
			return false;
		}
		
		lv.lumii.tda.ee.mm.OptionSelectedEvent optEv = (lv.lumii.tda.ee.mm.OptionSelectedEvent)eeFactory.findOrCreateRAAPIReferenceWrapper(r, false);
		lv.lumii.tda.ee.mm.Option opt = optEv.getOption();
		
		long rId = 0;
		String id = null;
		if (opt != null)
			id = opt.getId();
		if ((id != null) && (id.startsWith("GDEOPTION"))) {
			rId = Long.parseLong( id.substring(9) );
		}
		
				
		lv.lumii.tda.webgde.mm.ToolbarElementSelectEvent ev = gdeFactory.createToolbarElementSelectEvent();
		ev.setToolbarElement( (lv.lumii.tda.webgde.mm.ToolbarElement) gdeFactory.findOrCreateRAAPIReferenceWrapper(rId, false) );
		ev.submit();
		
		//optEv.delete(); ??
		
		
		eeFactory.unsetRAAPI();
		gdeFactory.unsetRAAPI();
		
		return true;
	}

	
	public static boolean onCloseFrameRequestedEvent(IWebMemory webmem, long r)
	{
		lv.lumii.tda.ee.mm.EnvironmentEngineMetamodelFactory eeFactory = new lv.lumii.tda.ee.mm.EnvironmentEngineMetamodelFactory();
		try {
			eeFactory.setRAAPI(webmem, "", true);
		} catch (Throwable e) {
			e.printStackTrace();
			return false;
		}
		
		lv.lumii.tda.ee.mm.CloseFrameRequestedEvent ev = (lv.lumii.tda.ee.mm.CloseFrameRequestedEvent)eeFactory.findOrCreateRAAPIReferenceWrapper(r, false); 
		lv.lumii.tda.ee.mm.Frame frame = ev.getFrame();
		
		
		
		lv.lumii.tda.ee.mm.DetachFrameCommand dfc = eeFactory.createDetachFrameCommand();
		dfc.setFrame(frame);
		dfc.setPermanently(true);
		dfc.submit();
		
		eeFactory.unsetRAAPI();
		
		return true;
	}
	
	private static void ensureFrame(lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory factory, lv.lumii.tda.webgde.mm.GraphDiagram gd) {		
		if (gd==null)
			return;		
		lv.lumii.tda.webgde.mm.Frame f = gd.getFrame().get(0);
		if (f==null) {
			f = factory.createFrame();
			f.setCaption("Graph Diagram");
			f.setContentURI("html:GraphDiagramEngine.html?frameReference="+f.getRAAPIReference());
			f.setGraphDiagram(gd);
			f.setLocation("CENTER");
			f.setOnFrameActivatedEvent("GDE.onFrameActivatedEvent");
			f.setOnCloseFrameRequestedEvent("GDE.onCloseFrameRequestedEvent");
		}
	}
	

	public static void prepareCommand(IWebMemory webmem, long r)
	{		
		if (r==0)
			return;
		
		lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory factory = null;
		try {
	
			factory = new lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory();
			try {
				factory.setRAAPI(webmem, "", false);
			} catch (lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory.ElementReferenceException e) {
				e.printStackTrace();
				return;
			}
			
			lv.lumii.tda.webgde.mm.AsyncCommand cmd = (lv.lumii.tda.webgde.mm.AsyncCommand)factory.findOrCreateRAAPIReferenceWrapper(r, true);
			
			if (cmd instanceof lv.lumii.tda.webgde.mm.SaveDgrCmd)
				return; // do not need to execute
			
			if ((cmd instanceof lv.lumii.tda.webgde.mm.ActiveDgrCmd)
			  ||(cmd instanceof lv.lumii.tda.webgde.mm.ActiveDgrViewCmd)) {
				lv.lumii.tda.webgde.mm.GraphDiagram gd = cmd.getGraphDiagram().get(0);
				ensureFrame(factory, gd);				
			}
			else
			if ((cmd instanceof lv.lumii.tda.webgde.mm.OkCmd)
			  ||(cmd instanceof lv.lumii.tda.webgde.mm.UpdateDgrCmd)
			  ||(cmd instanceof lv.lumii.tda.webgde.mm.RefreshDgrCmd)
			  ||(cmd instanceof lv.lumii.tda.webgde.mm.ActiveElementCmd)
			){
				lv.lumii.tda.webgde.mm.GraphDiagram gd = cmd.getGraphDiagram().get(0);
				
				if (gd == null) {
					lv.lumii.tda.webgde.mm.CurrentDgrPointer ptr = lv.lumii.tda.webgde.mm.CurrentDgrPointer.firstObject(factory);
					if (ptr==null) {
						gd = lv.lumii.tda.webgde.mm.GraphDiagram.firstObject(factory);
						if (gd!=null) {
							try {
								ptr = factory.createCurrentDgrPointer();
								ptr.setGraphDiagram(gd);
								cmd.setGraphDiagram( gd );
							}
							catch (Throwable t) {}
						}
					}
					else {
						try {
							cmd.setGraphDiagram( lv.lumii.tda.webgde.mm.CurrentDgrPointer.firstObject(factory).getGraphDiagram().get(0) );
						}
						catch (Throwable t) {}
					}
									
				}
				
				if (gd != null) {
					// do not execute UpdateDgrCmd before ActiveDgrCmd
					if ((cmd instanceof lv.lumii.tda.webgde.mm.UpdateDgrCmd) && gd.getFrame().isEmpty()) {
						System.out.println("ignoring UpdateDgrCmd before activating");
						return;
					}
					
					ensureFrame(factory, gd);
				}
				
				if ((gd != null)&&(cmd instanceof lv.lumii.tda.webgde.mm.ActiveElementCmd)) {
					gd.getCollection().clear();
					lv.lumii.tda.webgde.mm.Collection c = factory.createCollection();
					c.getElement().addAll(  ((lv.lumii.tda.webgde.mm.ActiveElementCmd)cmd).getElement() );
					gd.getCollection().add(c);
				}
			}
			else
			if 	(cmd instanceof lv.lumii.tda.webgde.mm.CloseDgrCmd) {
				lv.lumii.tda.webgde.mm.GraphDiagram gd = cmd.getGraphDiagram().get(0);
				if (gd == null) {
					System.out.println("CloseDgrCmd with no diagram called");
					return;
				}
				else {
					System.out.println("CloseDgrCmd gd="+gd.getRAAPIReference());
					if (gd.getFrame().isEmpty())
						System.out.println(" no frame specified");
					else
						System.out.println(" frame="+gd.getFrame().get(0).getRAAPIReference());
				}
			}

			
			
			WebCaller.WebCallSeed seed = new WebCaller.WebCallSeed();
			
			seed.actionName = "continue"+cmd.getClass().getSimpleName();
			
			seed.callingConventions = WebCaller.CallingConventions.WEBMEMCALL;
			seed.webmemArgument = webmem.replicateObject(r);			
			
			WebMemoryContext ctx = webmem.getContext();

			if (ctx != null) {
				seed.login = ctx.login;
				seed.project_id = ctx.project_id;
			}
	  		
	  		API.webCaller.invokeNow(seed);//.enqueue(seed);
			
		} catch (Throwable e) {			
			logger.error(e.toString()+", "+e.getMessage());
			e.printStackTrace();
		}			
		finally {
			if (factory != null)
				factory.unsetRAAPI();
		}
	}
	
	public static String layoutGraphDiagram(IWebMemory webmem, String _json) {
		// reads coordinates from the repository, lays out the diagram (json = stringified diagram reference),
		// and puts the new coordinates back into the repository (AZ location encoding);
		
		// may use (TDAKernel)raapi to store cache (previous layout data structures)
	
		lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory factory = null;
		try {
			factory = new lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory();
				
			try {
				JSONObject json;
				json = new JSONObject(_json);
		
				long r;
				try {
					r = json.getLong("reference");
					factory.setRAAPI(webmem, "", false);
				} catch (lv.lumii.tda.webgde.mm.GraphDiagramEngineMetamodelFactory.ElementReferenceException e) {
					logger.error("setRAAPI exception: "+e.getMessage());
					return null;
				}

				lv.lumii.tda.webgde.mm.GraphDiagram gd = (lv.lumii.tda.webgde.mm.GraphDiagram)factory.findOrCreateRAAPIReferenceWrapper(r, true);
				
				String layoutName = json.getString("layoutName");
				
				
/*				Object cacheObj = ((TDAKernel)raapi).retrieveCache("LAYOUT"+r);
				IMCSDiagramLayout layout;
				if ((layoutName == null) && (cacheObj != null))
					layout = (IMCSDiagramLayout)cacheObj;
				else {
					layout = new IMCSDiagramLayout(layoutName);
					//layout.
					((TDAKernel)raapi).storeCache("LAYOUT"+r, layout);
				}*/
				
				// adding/re-adding boxes...
				
				// adding/re-adding lines...
				
				// adding/re-adding labels...			
				
				return "{ \"result\": true }";
			} catch (Throwable t) {
				return "{ \"result\" : false }";
			}
		}			
		finally {
			if (factory != null)
				factory.unsetRAAPI();
		}				
	}
}
