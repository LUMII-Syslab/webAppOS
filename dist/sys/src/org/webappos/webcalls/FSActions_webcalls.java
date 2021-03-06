package org.webappos.webcalls;

import java.io.BufferedWriter;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStreamWriter;
import com.google.gson.JsonObject;
import lv.lumii.tda.raapi.RAAPI;
import org.apache.commons.io.IOUtils;
import org.codehaus.jettison.json.JSONArray;
import org.codehaus.jettison.json.JSONObject;
import org.webappos.antiattack.ValidityChecker;
import org.webappos.fs.FSDriversManager;
import org.webappos.fs.HomeFS;
import org.webappos.fs.IFileSystem.PathInfo;
import org.webappos.server.API;
import org.webappos.server.ConfigEx;

import java.util.Set;

public class FSActions_webcalls {

	public static String fileExists(String fileName, String login) {
		// login will be non-null
		try {
			ValidityChecker.checkRelativePath(fileName, false);
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
		if (!fileName.startsWith(login+"/"))
			return "{\"error\":\"User login must be the first path element\"}";
		
		return  "{\"result\":"+HomeFS.ROOT_INSTANCE.pathExists(fileName)+"}";
	}
	
	public static String fileExistsInCurrentProject(String project_id, String fileName, String login, String appFullName) { //  project_id, arg, login, appFullName
		// login will be non-null
		try {
			ValidityChecker.checkRelativePath(fileName, false);
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}

		String projectDir = API.dataMemory.getProjectFolder(project_id);
		if (projectDir==null)
			return "{\"error\":\"Unknown project cache directory\"}";

		File f = new File(projectDir + File.separator+fileName.replace('\\', File.separatorChar).replace('/', File.separatorChar));		
		return  "{\"result\":"+f.exists()+"}";
	}

	public static String isDirectory(String fileName, String login) {
		// login will be non-null
		try {
			ValidityChecker.checkRelativePath(fileName, false);
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
		if (!fileName.startsWith(login+"/"))
			return "{\"error\":\"User login must be the first path element\"}";
		
		PathInfo info = HomeFS.ROOT_INSTANCE.getPathInfo(fileName);
		return  "{\"result\":"+((info!=null)&&info.isDirectory)+"}";
	}

	public static String isDirectoryInCurrentProject(String project_id, String fileName, String login, String appFullName) { //  project_id, arg, login, appFullName
		// login will be non-null
		try {
			ValidityChecker.checkRelativePath(fileName, false);
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
		String projectDir = API.dataMemory.getProjectFolder(project_id);
		if (projectDir==null)
			return "{\"error\":\"Unknown project cache directory\"}";

		File f = new File(projectDir + File.separator+fileName.replace('\\', File.separatorChar).replace('/', File.separatorChar));		
		return  "{\"result\":"+f.isDirectory()+"}";
	}

	public static String getFileContentAsUTF8String(String fileName, String login) {
		// login will be non-null
		
		try {
			ValidityChecker.checkRelativePath(fileName, false);
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
		if (!fileName.startsWith(login+"/"))
			return "{\"error\":\"User login must be the first path element\"}";
		
		java.io.InputStream is = HomeFS.ROOT_INSTANCE.downloadFile(fileName);
		if (is == null) {
			return "{\"error\":\"Resouce/file not found\"}";
		}
		
		try {
			return "{\"content\":\""+IOUtils.toString(is, "UTF-8")+"\"}";			
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		finally {
			try {
				is.close();
			} catch (IOException e) {
			}
		}	
	}
	

	public static String getFileContentFromCurrentProjectAsUTF8String(String project_id, String fileName, String login, String appFullName) { //  project_id, arg, login, appFullName
		System.out.println(fileName);
		
		String projectDir = API.dataMemory.getProjectFolder(project_id);
		
		if (projectDir==null)
			return "{\"error\":\"Unknown project cache directory\"}";
		
		
		File f = new File(projectDir + File.separator+fileName.replace('\\', File.separatorChar).replace('/', File.separatorChar));
		try {
			ValidityChecker.checkRelativePath(fileName, false);
			JsonObject o = new JsonObject();
			o.addProperty("content", IOUtils.toString(f.toURI(), "UTF-8"));
			//System.out.println("done: `"+IOUtils.toString(f.toURI(), "UTF-8")+"`");
			System.out.println("done2: `"+o.toString()+"`");
			//return "{\"content\":\""+IOUtils.toString(f.toURI(), "UTF-8")+"\"}";
			return o.toString();
		} catch (Throwable t) {
			System.out.println("{\"error\":\""+t.getMessage()+"\"}");
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
		
	}
	
	public static String deleteFile(String fileName, String login) {
		// login will be non-null
		
		try {
			ValidityChecker.checkRelativePath(fileName, false);
			if (!fileName.startsWith(login+"/"))
				return "{\"error\":\"User login must be the first path element\"}";
			return "{\"result\":"+HomeFS.ROOT_INSTANCE.deletePath(fileName)+"}";
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
	}

	public static String deleteFileFromCurrentProject(String project_id, String fileName, String login, String appFullName) { //  project_id, arg, login, appFullName
		// raapi and login will be non-null
		
		String projectDir = API.dataMemory.getProjectFolder(project_id);
		
		if (projectDir==null)
			return "{\"error\":\"Unknown project cache directory\"}";
		
		File f = new File(projectDir + File.separator+fileName.replace('\\', File.separatorChar).replace('/', File.separatorChar));
		try {
			ValidityChecker.checkRelativePath(fileName, false);
			return "{\"result\":"+f.delete()+"}";
		} catch (Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
		
	}
	
	public static String uploadFile(String json, String login) {
		// login will be non-null

		try {
			JSONObject obj = new JSONObject(json);
			String fileName = obj.getString("fileName");
			String content = obj.getString("content");
			if ((fileName==null) || (content==null))
				return "{\"error\":\"No fileName or content specified\"}";
			
			if (!fileName.startsWith(login+"/"))
				return "{\"error\":\"User login must be the first path element\"}";
			ValidityChecker.checkRelativePath(fileName, false);
						
			BufferedWriter writer = null;
			try
			{
				byte[] bytes = content.getBytes( "UTF-8" );
				InputStream is = new ByteArrayInputStream(bytes);
				boolean b = HomeFS.ROOT_INSTANCE.uploadFile(fileName, is, bytes.length, true);
			    return "{\"result\":"+b+"}";
			}
			catch ( IOException e)
			{
			    return "{\"result\":false}";
			}
			finally
			{
			    try
			    {
			        if ( writer != null)
			        	writer.close( );
			    }
			    catch ( IOException e)
			    {
			    }
			}
						
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
	}
	
	public static String uploadFileToCurrentProject(String project_id, String json, String login, String appFullName) { //  project_id, arg, login, appFullName
		
		try {
			JSONObject obj = new JSONObject(json);
			String fileName = obj.getString("fileName");
			String content = obj.getString("content");
			if ((fileName==null) || (content==null))
				return "{\"error\":\"No fileName or content specified\"}";
			
			ValidityChecker.checkRelativePath(fileName, false);
			
			String projectDir = API.dataMemory.getProjectFolder(project_id);
			
			if (projectDir==null)
				return "{\"error\":\"Unknown project cache directory\"}";
			
			
			BufferedWriter writer = null;
			try
			{
			    writer = new BufferedWriter(			    
			    		new OutputStreamWriter(new FileOutputStream(new File(projectDir+File.separator+fileName)), "UTF-8"));
			    
			    
			    writer.write(content);
			    return "{\"result\":true}";
			}
			catch ( IOException e)
			{
			    return "{\"result\":false}";
			}
			finally
			{
			    try
			    {
			        if ( writer != null)
			        	writer.close( );
			    }
			    catch ( IOException e)
			    {
			    }
			}
						
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
	}
	
	public static String renameActiveProject(RAAPI raapi, String json, String login) {
		try {
			JSONObject obj = new JSONObject(json);
			String project_id = obj.getString("project_id");
			String new_project_id = obj.getString("new_project_id");
			if ((project_id==null) || (new_project_id==null))
				return "{\"error\":\"No project_id or new_project_id\"}";
			
			ValidityChecker.checkRelativePath(project_id, false);
			ValidityChecker.checkRelativePath(new_project_id, false);
			
			if (!project_id.startsWith(login+"/"))
				return "{\"error\":\"You cannot rename projects owned by other users.\"}";
			
			if (!new_project_id.startsWith(login+"/"))
				return "{\"error\":\"The new project name must start with your login followed by '/'.\"}";
						
		    return "{\"result\": "+API.dataMemory.renameActiveProject(project_id, new_project_id)+" }";
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
	}
	
	public static String getSupportedFileSystems(String json) {
		// must be called inline (from the webAppOS server process, not from a web processor)
		try {
			if (!(API.config instanceof ConfigEx))
				throw new RuntimeException("Error accessing the driver list. Perhaps, not an inline web call.");
			
			JSONObject obj = new JSONObject();
			JSONArray arr = new JSONArray();
			Set<String> ks = FSDriversManager.getSupportedPrefixes();
			for (String k : ks) {
				arr.put(k);
			}
			
			obj.put("result", arr);
			
			return obj.toString();
		}
		catch(Throwable t) {
			return "{\"error\":\""+t.getMessage()+"\"}";
		}
	}
}
