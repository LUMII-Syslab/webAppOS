package org.webappos.status;

public interface IStatus {
	public void setStatus(String key, String value);		
	public void setStatus(String key, String value, long expireSeconds);
}