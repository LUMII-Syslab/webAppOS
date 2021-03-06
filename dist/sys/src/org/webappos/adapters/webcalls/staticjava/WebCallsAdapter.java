package org.webappos.adapters.webcalls.staticjava;

import java.lang.reflect.Method;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.webappos.server.API;
import org.webappos.webcaller.IJsonWebCallsAdapter;
import org.webappos.webcaller.IWebMemWebCallsAdapter;
import org.webappos.webmem.IWebMemory;

import lv.lumii.tda.raapi.RAAPI;


public class WebCallsAdapter implements IJsonWebCallsAdapter, IWebMemWebCallsAdapter {
	private static Logger logger =  LoggerFactory.getLogger(WebCallsAdapter.class);
	
	
	@Override
	public void webmemcall(String location, String pwd, long rObject, IWebMemory raapi, String project_id, String appFullName,
			String login) {
		// location must be in the form className#functionName
		if (location == null)
			return;
		
		int i = location.lastIndexOf("#");		
		if (i<0)
			return;
		
		API.classLoader.addClasspathsForPropertiesId(appFullName);
		
		Class<?> c = API.classLoader.findClassByName(location.substring(0, i));
		if (c == null)
			return;
		
		Method m = null, m1 = null, m2 = null;
		try {
			m = c.getMethod(location.substring(i+1), IWebMemory.class, Long.TYPE);
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			if (m==null)
				m = c.getMethod(location.substring(i+1), RAAPI.class, Long.TYPE);
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			m1 = c.getMethod(location.substring(i+1), IWebMemory.class, String.class, Long.TYPE);
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			if (m1==null)
				m1 = c.getMethod(location.substring(i+1), RAAPI.class, String.class, Long.TYPE);
		} catch (NoSuchMethodException | SecurityException e) {
		}
		
		try {
			m2 = c.getMethod(location.substring(i+1), IWebMemory.class, Long.TYPE, String.class);
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			if (m2==null)
				m2 = c.getMethod(location.substring(i+1), RAAPI.class, Long.TYPE, String.class);
		} catch (NoSuchMethodException | SecurityException e) {
		}
		
		if ((m!=null) && ((m.getReturnType() == Boolean.TYPE) || (m.getReturnType() == Boolean.class) || (m.getReturnType() == Void.TYPE) || (m.getReturnType() == Void.class))) {
			try {
				m.invoke(null, raapi, rObject);
			} catch (Throwable t) {
				logger.error("webmemcall webcall "+location+" invocation exception (2 args)");
				t.printStackTrace();
			}
		}
		else 
		if ((m1!=null) && ((m1.getReturnType() == Boolean.TYPE) || (m1.getReturnType() == Boolean.class) || (m1.getReturnType() == Void.TYPE) || (m1.getReturnType() == Void.class))) {
			try {
				m1.invoke(null, raapi, project_id, rObject);
			} catch (Throwable t) {
				logger.error("webmemcall webcall "+location+" invocation exception (3 args, with project_id)");
			}			
		}
		else 
		if ((m2!=null) && ((m2.getReturnType() == Boolean.TYPE) || (m2.getReturnType() == Boolean.class) || (m2.getReturnType() == Void.TYPE) || (m1.getReturnType() == Void.class))) {
			try {
				m2.invoke(null, raapi, rObject, login);
			} catch (Throwable t) {
				logger.error("webmemcall webcall "+location+" invocation exception (3 args, with login)");
			}			
		}
		else {
			logger.error("webmemcall webcall "+location+" not found");
		}
	}


	@Override
	synchronized public String jsoncall(String location, String pwd, String argument, String project_id, String appFullName, String login) {
		
		// location must be in the form className#functionName
		if (location == null) {
			return null;
		}
		int i = location.lastIndexOf("#");		
		if (i<0) {
			return null;
		}
		
		API.classLoader.addClasspathsForPropertiesId(appFullName);
		
		Class<?> c = API.classLoader.findClassByName(location.substring(0, i));
		if (c == null)
			return null;
		
		Method m00=null, m0=null, m1=null, m2=null, m3=null, m4=null;
		IWebMemory webmem = API.dataMemory.getWebMemory(project_id);
		try {
			m00 = c.getMethod(location.substring(i+1), String.class, String.class, String.class, String.class); // project_id, arg, login, appFullName
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			m0 = c.getMethod(location.substring(i+1), IWebMemory.class, String.class, String.class, String.class); // raapi, arg, login, appFullName
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			if (m0==null)
				m0 = c.getMethod(location.substring(i+1), RAAPI.class, String.class, String.class, String.class); // raapi, arg, login, appFullName
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			m1 = c.getMethod(location.substring(i+1), IWebMemory.class, String.class, String.class); // raapi, arg, login
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			if (m1==null)
				m1 = c.getMethod(location.substring(i+1), RAAPI.class, String.class, String.class); // raapi, arg, login
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			m2 = c.getMethod(location.substring(i+1), String.class, String.class); // arg, login
		} catch (NoSuchMethodException | SecurityException e) {
		}
		
		try {
			m3 = c.getMethod(location.substring(i+1), IWebMemory.class, String.class); // raapi, arg
		} catch (NoSuchMethodException | SecurityException e) {
		}
		try {
			if (m3==null)
				m3 = c.getMethod(location.substring(i+1), RAAPI.class, String.class); // raapi, arg
		} catch (NoSuchMethodException | SecurityException e) {
		}
		
		try {
			m4 = c.getMethod(location.substring(i+1), String.class); // arg
		} catch (NoSuchMethodException | SecurityException e) {
		}
		
		if ((m00!=null) && (m00.getReturnType() == String.class) && (project_id!=null) && (login!=null) && (appFullName!=null)) {			
			try {
				return (String)m00.invoke(null, project_id, argument, login, appFullName);
			} catch (Throwable t) {
				return null;
			}
		}
		else
		if ((m0!=null) && (m0.getReturnType() == String.class) && (webmem!=null) && (login!=null) && (appFullName!=null)) {			
			try {
				return (String)m0.invoke(null, webmem, argument, login, appFullName);
			} catch (Throwable t) {
				return null;
			}
		}
		else
		if ((m1!=null) && (m1.getReturnType() == String.class) && (webmem!=null) && (login!=null)) {
			try {
				return (String)m1.invoke(null, webmem, argument, login);
			} catch (Throwable t) {
				return null;
			}
		}
		else
		if ((m2!=null) && (m2.getReturnType() == String.class) && (login!=null)) {
			
			try {
				return (String)m2.invoke(null, argument, login);
			} catch (Throwable t) {
				return null;
			}
		}
		else
		if ((m3!=null) && (m3.getReturnType() == String.class) && (webmem!=null)) {
			try {
				return (String)m3.invoke(null, webmem, argument);
			} catch (Throwable t) {
				return null;
			}
		}
		else
		if ((m4!=null) && (m4.getReturnType() == String.class)) {
			try {
				return (String)m4.invoke(null, argument);
			} catch (Throwable t) {
				return null;
			}
		}
		else
			return null;	
	}

}
