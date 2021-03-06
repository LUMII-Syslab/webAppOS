package lv.lumii.tda.util;

import java.io.*;

public class DOS {

	public static String getMSDOSName(String fileName)
		    throws IOException, InterruptedException {

		    String path = getAbsolutePath(fileName);

		    // changed "+ fileName.toUpperCase() +" to "path"
		    Process process =
		        Runtime.getRuntime().exec(
		            "cmd /c for %I in (\"" + path + "\") do @echo %~fsI");

		    process.waitFor();

		    byte[] data = new byte[65536];
		    int size = process.getInputStream().read(data);

		    if (size <= 0)
		        return null;

		    return new String(data, 0, size).replaceAll("\\r\\n", "");
		}

		public static String getAbsolutePath(String fileName)
		    throws IOException {
		    File file = new File(fileName);
		    String path = file.getAbsolutePath();

		    if (file.exists() == false)
		        file = new File(path);

		    path = file.getCanonicalPath();

		    if (file.isDirectory() && (path.endsWith(File.separator) == false))
		        path += File.separator;

		    return path;
		}
}
