// automatically generated
package lv.lumii.tda.kernel.mm; 

import java.util.*;
import lv.lumii.tda.raapi.RAAPI;

public class Submitter
  	implements RAAPIReferenceWrapper
{
	protected TDAKernelMetamodelFactory factory;
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
				System.err.println("Unable to delete the object "+rObject+" of type TDAKernel::Submitter since the RAAPI wrapper does not take care of this reference.");
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
	Submitter(TDAKernelMetamodelFactory _factory)
	{
		factory = _factory;
		rObject = factory.raapi.createObject(factory.SUBMITTER);			
		takeReference = true;
		factory.wrappers.put(rObject, this);
	}

	public Submitter(TDAKernelMetamodelFactory _factory, long _rObject, boolean _takeReference)
	{
		factory = _factory;
		rObject = _rObject;
		takeReference = _takeReference;
		if (takeReference)
			factory.wrappers.put(rObject, this);
	}

	// iterator for instances...
	public static Iterable<? extends Submitter> allObjects()
	{
		return allObjects(TDAKernelMetamodelFactory.eINSTANCE);
	} 

	public static Iterable<? extends Submitter> allObjects(TDAKernelMetamodelFactory factory)
	{
		ArrayList<Submitter> retVal = new ArrayList<Submitter>();
		long it = factory.raapi.getIteratorForAllClassObjects(factory.SUBMITTER);
		if (it == 0)
			return retVal;
		long r = factory.raapi.resolveIteratorFirst(it);
		while (r != 0) {
 			Submitter o = (Submitter)factory.findOrCreateRAAPIReferenceWrapper(r, true);
			if (o == null)
				o = (Submitter)factory.findOrCreateRAAPIReferenceWrapper(Submitter.class, r, true);
			if (o != null)
				retVal.add(o);
			r = factory.raapi.resolveIteratorNext(it);
		}
		factory.raapi.freeIterator(it);
		return retVal;
	}

	public static boolean deleteAllObjects()
	{
		return deleteAllObjects(TDAKernelMetamodelFactory.eINSTANCE);
	}

	public static boolean deleteAllObjects(TDAKernelMetamodelFactory factory)
	{
		for (Submitter o : allObjects(factory))
			o.delete();
		return firstObject(factory) == null;
	}

	public static Submitter firstObject()
	{
		return firstObject(TDAKernelMetamodelFactory.eINSTANCE);
	} 

	public static Submitter firstObject(TDAKernelMetamodelFactory factory)
	{
		long it = factory.raapi.getIteratorForAllClassObjects(factory.SUBMITTER);
		if (it == 0)
			return null;
		long r = factory.raapi.resolveIteratorFirst(it);
		factory.raapi.freeIterator(it);
		if (r == 0)
			return null;
		else {
			Submitter  retVal = (Submitter)factory.findOrCreateRAAPIReferenceWrapper(r, true);
			if (retVal == null)
				retVal = (Submitter)factory.findOrCreateRAAPIReferenceWrapper(Submitter.class, r, true);
			return retVal;
		}
	} 
 
	public List<Event> getEvent()
	{
		return new TDAKernelMetamodel_RAAPILinkedObjectsList<Event>(factory, rObject, factory.SUBMITTER_EVENT); 
	}
	public boolean setEvent(Event value)
	{
		boolean ok = true;
		long it = factory.raapi.getIteratorForLinkedObjects(rObject, factory.SUBMITTER_EVENT);
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
				if (!factory.raapi.deleteLink(rObject, rLinked, factory.SUBMITTER_EVENT))
					ok = false;
		}
		if (value != null)
			if (!factory.raapi.createLink(rObject, value.rObject, factory.SUBMITTER_EVENT))
				ok = false;
		return ok;
	}
	public List<Command> getCommand()
	{
		return new TDAKernelMetamodel_RAAPILinkedObjectsList<Command>(factory, rObject, factory.SUBMITTER_COMMAND); 
	}
	public boolean setCommand(Command value)
	{
		boolean ok = true;
		long it = factory.raapi.getIteratorForLinkedObjects(rObject, factory.SUBMITTER_COMMAND);
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
				if (!factory.raapi.deleteLink(rObject, rLinked, factory.SUBMITTER_COMMAND))
					ok = false;
		}
		if (value != null)
			if (!factory.raapi.createLink(rObject, value.rObject, factory.SUBMITTER_COMMAND))
				ok = false;
		return ok;
	}
}
