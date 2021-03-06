// automatically generated
package org.webappos.weblib.de.mm; 

import java.util.*;
import lv.lumii.tda.raapi.RAAPI;

public class D_SHARP_EventHandler
  	implements RAAPIReferenceWrapper
{
	protected DialogEngineMetamodelFactory factory;
	protected long rObject = 0;
	protected boolean takeReference;

	public RAAPI getRAAPI()
	{
		return factory.raapi;
	}
	public long getRAAPIReference()
	{
		return rObject;
	}

	public boolean delete()
	{
		if (rObject != 0) {
			if (!takeReference) {
				System.err.println("Unable to delete the object "+rObject+" of type D_SHARP_EventHandler since the RAAPI wrapper does not take care of this reference.");
				return false;
			}
			factory.wrappers.remove(rObject);
			boolean retVal = factory.raapi.deleteObject(rObject);
			if (retVal) {
				rObject = 0;
			}
			else
				factory.wrappers.put(rObject, this); // putting back
			return retVal;
		}
		else
			return false;
	}

	public void finalize()
	{
		if (rObject != 0) {
			if (takeReference) {
				factory.wrappers.remove(rObject);
				factory.raapi.freeReference(rObject);
			}
			rObject = 0;
		}
	}


	// package-visibility:
	D_SHARP_EventHandler(DialogEngineMetamodelFactory _factory)
	{
		factory = _factory;
		rObject = factory.raapi.createObject(factory.D_SHARP_EVENTHANDLER);			
		takeReference = true;
		factory.wrappers.put(rObject, this);
	}

	public D_SHARP_EventHandler(DialogEngineMetamodelFactory _factory, long _rObject, boolean _takeReference)
	{
		factory = _factory;
		rObject = _rObject;
		takeReference = _takeReference;
		if (takeReference)
			factory.wrappers.put(rObject, this);
	}

	// iterator for instances...
	public static Iterable<? extends D_SHARP_EventHandler> allObjects()
	{
		return allObjects(DialogEngineMetamodelFactory.eINSTANCE);
	} 

	public static Iterable<? extends D_SHARP_EventHandler> allObjects(DialogEngineMetamodelFactory factory)
	{
		ArrayList<D_SHARP_EventHandler> retVal = new ArrayList<D_SHARP_EventHandler>();
		long it = factory.raapi.getIteratorForAllClassObjects(factory.D_SHARP_EVENTHANDLER);
		if (it == 0)
			return retVal;
		long r = factory.raapi.resolveIteratorFirst(it);
		while (r != 0) {
 			D_SHARP_EventHandler o = (D_SHARP_EventHandler)factory.findOrCreateRAAPIReferenceWrapper(r, true);
			if (o == null)
				o = (D_SHARP_EventHandler)factory.findOrCreateRAAPIReferenceWrapper(D_SHARP_EventHandler.class, r, true);
			if (o != null)
				retVal.add(o);
			r = factory.raapi.resolveIteratorNext(it);
		}
		factory.raapi.freeIterator(it);
		return retVal;
	}

	public static boolean deleteAllObjects()
	{
		return deleteAllObjects(DialogEngineMetamodelFactory.eINSTANCE);
	}

	public static boolean deleteAllObjects(DialogEngineMetamodelFactory factory)
	{
		for (D_SHARP_EventHandler o : allObjects(factory))
			o.delete();
		return firstObject(factory) == null;
	}

	public static D_SHARP_EventHandler firstObject()
	{
		return firstObject(DialogEngineMetamodelFactory.eINSTANCE);
	} 

	public static D_SHARP_EventHandler firstObject(DialogEngineMetamodelFactory factory)
	{
		long it = factory.raapi.getIteratorForAllClassObjects(factory.D_SHARP_EVENTHANDLER);
		if (it == 0)
			return null;
		long r = factory.raapi.resolveIteratorFirst(it);
		factory.raapi.freeIterator(it);
		if (r == 0)
			return null;
		else {
			D_SHARP_EventHandler  retVal = (D_SHARP_EventHandler)factory.findOrCreateRAAPIReferenceWrapper(r, true);
			if (retVal == null)
				retVal = (D_SHARP_EventHandler)factory.findOrCreateRAAPIReferenceWrapper(D_SHARP_EventHandler.class, r, true);
			return retVal;
		}
	} 
 
	public String getEventName()
	{
		return factory.raapi.getAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_EVENTNAME);
	}
	public boolean setEventName(String value)
	{
		if (value == null)
			return factory.raapi.deleteAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_EVENTNAME);
		return factory.raapi.setAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_EVENTNAME, value.toString());
	}
	public String getTransformationName()
	{
		return factory.raapi.getAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_TRANSFORMATIONNAME);
	}
	public boolean setTransformationName(String value)
	{
		if (value == null)
			return factory.raapi.deleteAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_TRANSFORMATIONNAME);
		return factory.raapi.setAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_TRANSFORMATIONNAME, value.toString());
	}
	public String getProcedureName()
	{
		return factory.raapi.getAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_PROCEDURENAME);
	}
	public boolean setProcedureName(String value)
	{
		if (value == null)
			return factory.raapi.deleteAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_PROCEDURENAME);
		return factory.raapi.setAttributeValue(rObject, factory.D_SHARP_EVENTHANDLER_PROCEDURENAME, value.toString());
	}
	public D_SHARP_EventSource getEventSource()
	{
		long it = factory.raapi.getIteratorForLinkedObjects(rObject, factory.D_SHARP_EVENTHANDLER_EVENTSOURCE);
		if (it == 0)
			return null;
		long r = factory.raapi.resolveIteratorFirst(it);
		factory.raapi.freeIterator(it);
		if (r != 0) {
			D_SHARP_EventSource retVal = (D_SHARP_EventSource)factory.findOrCreateRAAPIReferenceWrapper(r, true);
			if (retVal == null)
				retVal = (D_SHARP_EventSource)factory.findOrCreateRAAPIReferenceWrapper(D_SHARP_EventSource.class, r, true);
			return retVal;
		}
		else
			return null;
	}
	public boolean setEventSource(D_SHARP_EventSource value)
	{
		boolean ok = true;
		long it = factory.raapi.getIteratorForLinkedObjects(rObject, factory.D_SHARP_EVENTHANDLER_EVENTSOURCE);
		// deleting previous links...
		if (it != 0) {
			ArrayList<Long> list = new ArrayList<Long>();
			long r = factory.raapi.resolveIteratorFirst(it);
			while (r != 0) {
				list.add(r);
				r = factory.raapi.resolveIteratorNext(it);
			}
			factory.raapi.freeIterator(it);
			for (Long rLinked : list)
				if (!factory.raapi.deleteLink(rObject, rLinked, factory.D_SHARP_EVENTHANDLER_EVENTSOURCE))
					ok = false;
		}
		if (value != null)
			if (!factory.raapi.createLink(rObject, value.rObject, factory.D_SHARP_EVENTHANDLER_EVENTSOURCE))
				ok = false;
		return ok;
	}
}
