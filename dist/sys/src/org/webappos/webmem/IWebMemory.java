package org.webappos.webmem;

import lv.lumii.tda.kernel.TDACopier;
import lv.lumii.tda.raapi.RAAPI;

public interface IWebMemory extends RAAPI {
	/**
	 * Obtains a stringified reference (unique identifier, UUID) to the current web memory.
	 * The UUID can be used as an inter-process web memory reference (e.g., when connecting
	 * to web memory at the Java side from the native code via the native library webmem.dll/.so/.dylib).
	 * @return a string representing the current web memory
	 */
	public String getUUID();	
	
	/**
	 * If this web memory has attached synchronizers, flushes them.
	 */
	public void flush();
	
	/**
	 * Sets (temporarily) the context for this web memory. The context
	 * is used to provide more detail for further web call seeds originated from the current web call.
	 * @param user
	 */
	public void setContext(WebMemoryContext ctx);
	
	/**
	 * Returns the current (temporal) context.
	 * @return the context last set (or null)
	 */
	public WebMemoryContext getContext();
	
	/**
	 * Checks whether the given object is a submitter.
	 * @param rObject the object to check
	 * @return the result of the check
	 */
	public boolean isSubmitter(long rObject);
	
	/**
	 * Checks whether the given object is an event (e.g., web calls actions can be registered as listeners to this event).
	 * @param rObject the object to check
	 * @return the result of the check
	 */
	public boolean isEvent(long rObject);
	
	/**
	 * Checks whether the given object is a command (e.g., a web call action with the same name exists and will be called when this object
	 * is linked to the submitter).
	 * @param rObject the object to check
	 * @return the result of the check
	 */
	public boolean isCommand(long rObject);
	
	/**
	 * Checks whether the given link to be created is a submit link (i.e., the link used to invoke web calls).
	 * @param rSourceObject the source object reference
	 * @param rTargetObject the target object reference
	 * @param rAssociationEnd the target association end reference
	 * @return
	 */
	default public boolean creatingSubmitLink(long rSourceObject, long rTargetObject, long rAssociationEnd) {
		return isSubmitter(rSourceObject) || isSubmitter(rTargetObject);
	}
	
	/**
	 * Replicates the given object (copies attribute values and sets links to the same linked objects).
	 * Useful, when the object is about to be deleted, but we wish to keep it for us.
	 * 
	 * @param rObject an object to replicate
	 * @return a reference to the new object
	 */
	default public long replicateObject(long rObject) {
		return TDACopier.copyObject(this, rObject);
	}

}