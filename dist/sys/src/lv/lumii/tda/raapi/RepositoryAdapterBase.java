// Automatically generated by GeneratorFromStringTemplate.java from
// the RepositoryAdapterBase.stg template.
package lv.lumii.tda.raapi;

// Class RepositoryAdapterBase --- a base class for repository adapters.
// Contains stubs for all operations of the IRepositoryAdapter interface
// (includes RAAPI).
// RepositoryAdapterBase must be extended. Derived classes must implement
// at least all required operations of RAAPI, see RAAPI specification.

abstract public class RepositoryAdapterBase implements IRepository {

	@Override	
	public long findClass(String param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "findClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String getClassName(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getClassName" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long createObject(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createObject" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean createLink(long param0, long param1, long param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createLink" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getAttributeType(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getAttributeType" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isClass(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String getAttributeName(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getAttributeName" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForObjectsByAttributeValue(long param0, String param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForObjectsByAttributeValue" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForAllOutgoingAssociationEnds(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForAllOutgoingAssociationEnds" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectOutgoingAssociationEnds(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectOutgoingAssociationEnds" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectLinguisticInstances(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectLinguisticInstances" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectObjectClasses(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectObjectClasses" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForAllIngoingAssociationEnds(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForAllIngoingAssociationEnds" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectIngoingAssociationEnds(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectIngoingAssociationEnds" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForAllLinguisticInstances(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForAllLinguisticInstances" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long createClass(String param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteClass(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isDerivedClass(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isDerivedClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteObject(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteObject" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean moveObject(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "moveObject" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isTypeOf(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isTypeOf" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isKindOf(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isKindOf" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long createAttribute(long param0, String param1, long param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createAttribute" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isDirectSubClass(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isDirectSubClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long findAttribute(long param0, String param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "findAttribute" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long resolveIterator(long param0, int param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "resolveIterator" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String getRoleName(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getRoleName" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean linkExists(long param0, long param1, long param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "linkExists" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isComposition(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isComposition" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isLinguistic(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isLinguistic" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public void freeIterator(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "freeIterator" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getSourceClass(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getSourceClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteLink(long param0, long param1, long param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteLink" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isAttribute(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isAttribute" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isAssociationEnd(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isAssociationEnd" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getTargetClass(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getTargetClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public void freeReference(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "freeReference" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteAttribute(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteAttribute" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long createAdvancedAssociation(String param0, boolean param1, boolean param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createAdvancedAssociation" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long findPrimitiveDataType(String param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "findPrimitiveDataType" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getAttributeDomain(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getAttributeDomain" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long findAssociationEnd(long param0, String param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "findAssociationEnd" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteAssociation(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteAssociation" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isAdvancedAssociation(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isAdvancedAssociation" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean createOrderedLink(long param0, long param1, long param2, int param3)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createOrderedLink" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForLinkedObjects(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForLinkedObjects" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getInverseAssociationEnd(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getInverseAssociationEnd" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteAttributeValue(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteAttributeValue" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public int getLinkedObjectPosition(long param0, long param1, long param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getLinkedObjectPosition" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long resolveIteratorFirst(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "resolveIteratorFirst" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean excludeObjectFromClass(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "excludeObjectFromClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectSubClasses(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectSubClasses" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean includeObjectInClass(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "includeObjectInClass" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForAllAttributes(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForAllAttributes" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectAttributes(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectAttributes" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean createGeneralization(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createGeneralization" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean isPrimitiveDataType(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "isPrimitiveDataType" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean deleteGeneralization(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deleteGeneralization" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectSuperClasses(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectSuperClasses" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public boolean setAttributeValue(long param0, long param1, String param2)
	{
		throw new UnsupportedOperationException("The operation "
			+ "setAttributeValue" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String getAttributeValue(long param0, long param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getAttributeValue" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForClasses()
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForClasses" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String getPrimitiveDataTypeName(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getPrimitiveDataTypeName" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForAllClassObjects(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForAllClassObjects" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForDirectClassObjects(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForDirectClassObjects" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long createAssociation(long param0, long param1, String param2, String param3, boolean param4)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createAssociation" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long createDirectedAssociation(long param0, long param1, String param2, boolean param3)
	{
		throw new UnsupportedOperationException("The operation "
			+ "createDirectedAssociation" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public int getIteratorLength(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorLength" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long resolveIteratorNext(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "resolveIteratorNext" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long deserializeReference(String param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "deserializeReference" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String callSpecificOperation(String param0, String param1)
	{
		throw new UnsupportedOperationException("The operation "
			+ "callSpecificOperation" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getLinguisticClassFor(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "getLinguisticClassFor" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public long getIteratorForLinguisticClasses()
	{
		throw new UnsupportedOperationException("The operation "
			+ "getIteratorForLinguisticClasses" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}

	@Override	
	public String serializeReference(long param0)
	{
		throw new UnsupportedOperationException("The operation "
			+ "serializeReference" + " is not implemented in the repository adapter named "+this.getClass().getCanonicalName()+".");
	}
}
