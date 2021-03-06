package org.webappos.apps.helloworld;

import org.webappos.apps.helloworld.mm.HelloWorldMetamodelFactory; // generated
import org.webappos.webmem.IWebMemory; // the interface for accessing web memory

import org.webappos.server.API;
import org.webappos.webcaller.IWebCaller;
import org.webappos.webcaller.IWebCaller.WebCallSeed;


public class HelloWorld {
	
	public static void initial(IWebMemory webmem, String project_id, long r)
	{		
		
		try {
			
			HelloWorldMetamodelFactory factory = webmem.elevate(HelloWorldMetamodelFactory.class);
			
			org.webappos.apps.helloworld.mm.HelloWorld objectWithMessage
				= org.webappos.apps.helloworld.mm.HelloWorld.firstObject(factory);
			
			if (objectWithMessage==null) {
				objectWithMessage = factory.createHelloWorld();
				objectWithMessage.setMessage("Hello for the first time!");
			}
			else
				objectWithMessage.setMessage("Hello again!");
			
			WebCallSeed seed = new WebCallSeed();
			seed.actionName = "ShowMessageFromJSON";
			seed.project_id = project_id;
			seed.jsonArgument = "{\"message\":\""+objectWithMessage.getMessage()+"\"}";
			seed.callingConventions = IWebCaller.CallingConventions.JSONCALL;
			API.webCaller.enqueue(seed);

			WebCallSeed seed2 = new WebCallSeed();
			seed2.actionName = "ShowMessageFromWebMemory";
			seed2.project_id = project_id;
			seed2.webmemArgument = objectWithMessage.getRAAPIReference();
			seed2.callingConventions = IWebCaller.CallingConventions.WEBMEMCALL;
			API.webCaller.enqueue(seed2);

		} catch (Throwable e) {
			WebCallSeed seed = new WebCallSeed();
			seed.actionName = "ShowMessageFromJSON";
			seed.project_id = project_id;
			seed.jsonArgument = "{\"message\":\"An exception occurred - "+e.getMessage()+"\"}";
			seed.callingConventions = IWebCaller.CallingConventions.JSONCALL;
			API.webCaller.enqueue(seed);
		}

	}

	public static String addWorld(String s)
	{
		if (s==null)
			return "{\"error\":\"Null string passed.\"}";
		else
			return "{\"result\":\""+s.replace("Hello", "Hello, world,")+"\"}";
	}

}
