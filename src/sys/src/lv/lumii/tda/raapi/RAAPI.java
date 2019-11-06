package lv.lumii.tda.raapi;


/**
 * <code>RAAPI</code> is a common abstraction layer for accessing models stored in
 * different repositories associated with different technical spaces.
 ***/
public interface RAAPI 
{

  /**
     * Obtains a reference to a primitive data type with the given name.
     * @param name the type name. Each repository must support at least four
     *             standard primitive data types: "Integer", "Real", "Boolean", and "String".
     *             <BR/>
     *             [TODO] Certain repositories may introduce additional primitive types.
     *             To denote a repository-specific additional primitive data type,
     *             prepend the mount point of that repository, e.g.,
     *             <code>MountPoint::PeculiarDataType</code>.
     * @return a reference to a primitive data type with the given name, or 0 on error.
     * <BR/><I><B>Note (TDA Kernel):</B></I>
     *   TDA Kernel returns a proxy reference, which 
     *   is usable even when there are multiple repositories mounted
     *   or non-standard primitive data types are used.
     */
  long findPrimitiveDataType (String name);

  /**
     * Returns the name of the given primitive data type.
     * @param rDataType a reference to a primitive data type,
     *        for which the name has to be obtained
     * @return the name of the given primitive data type, or <code>null</code> on error.
     * @see RAAPI#findPrimitiveDataType
     */
  String getPrimitiveDataTypeName (long rDataType);

  /**
     * Checks whether the given reference is associated with a primitive data type.
     * @param r a reference in question
     * @return whether the given reference is associated with a primitive data type.
     *         On error, <code>false</code> is returned.
     * @see RAAPI#findPrimitiveDataType
     * @see RAAPI#getPrimitiveDataTypeName
     */
  boolean isPrimitiveDataType (long r);

  /**
     * Creates a class with the given fully qualified name.
     * @param name the fully qualified name of the class
     *        (packages are delimited by double colon "::");
     *        this fully qualified name must be unique
     * @return a reference to the class just created, or 0 on error.
     */
  long createClass (String name);

  /**
     * Obtains a reference to an existing class with the given fully qualified name.
     * <BR/><I><B>Note (M3): </B></I>If the underlying repository provides access to
     * its quasi-linguistic meta-metamodel, quasi-linguistic classes can be accessed
     * by using the prefix "M3::", e.g.,
     * <code>SomePath::MountPoint::M3::SomeMetaType</code>
     * <BR/><I><B>Note (adapters): </B></I>This function is optional for
     * repository adapters. If not implemented in an adapter,
     * TDA Kernel implements it through <code>getIteratorForClasses</code>
     * [TODO] and <code>getIteratorForLinguisticClasses</code>.
     * @param name the fully qualified name of the class
     *             (for quasi-linguistic classes, use prefix "M3::")
     * @return a reference to a (quasi-ontological or quasi-linguistic) class with the given fully qualified name.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long findClass (String name);

  /**
     * Returns the fully qualified name of the given class.
     * <BR/>
     * [TODO] <I><B>Note (M3): </B></I>If the reference points to a
     * quasi-linguistic class, then
     * the prefix "M3::" is also included in the return value, e.g.,
     * <code>MountPointForTheCorrespondingRepository::M3::ClassName</code>.
     * @param rClass a reference to the class, for which the class name has to be obtained
     * @return the fully qualified name of the given class, or <code>null</code> on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  String getClassName (long rClass);

  /**
     * Deletes the class and frees the reference.
     * @param rClass a reference to the class to be deleted
     * @return whether the operation succeeded.
     */
  boolean deleteClass (long rClass);

  /**
     * Obtains an iterator for all classes (all quasi-ontological classes at all quasi-ontological meta-levels).
     * <BR/><I><B>Note (M3): </B></I>Linguistic classes are not traversed by this iterator.
     * Use <code>getIteratorForLinguisticClasses</code> instead.<BR/>&nbsp;
     * @return an iterator for all classes, or 0 on error.
     * @see RAAPI#getIteratorForLinguisticClasses
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForClasses ();

  /**
     * Creates a generalization between the two given classes.
     * <BR/>
     * The given subclass can be a derived class of the given superclass, but
     * the direct generalization between them must not exist.
     * <BR/>
     * The generalization relation being created must not introduce inheritance loops.
     * @param rSubClass a class that becomes a subclass
     * @param rSuperClass a class that becomes a superclass
     * @return whether the operation succeeded.
     */
  boolean createGeneralization (long rSubClass, long rSuperClass);

  /**
     * Deletes the generalization between the given two classes.
     * @param rSubClass a class that was a subclass
     * @param rSuperClass a class that was a superclass
     * @return whether the operation succeeded.
     */
  boolean deleteGeneralization (long rSubClass, long rSuperClass);

  /**
     * Obtains an iterator for all direct superclasses of the given subclass.
     * <BR/><I><B>Note (M3): </B></I>If the given subclass is a quasi-linguistic class,
     * then an iterator for its direct quasi-linguistic superclasses is returned.
     * @param rSubClass a subclass for which to obtain direct superclasses
     * @return an iterator for all direct superclasses of the given subclass, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectSuperClasses (long rSubClass);

  /**
     * Obtains an iterator for all direct subclasses of the given superclass.
     * <BR/><I><B>Note (M3): </B></I>If the given superclass is a quasi-linguistic class,
     * then an iterator for direct quasi-linguistic subclasses is returned.
     * @param rSuperClass a superclass for which to obtain direct subclasses
     * @return an iterator for all direct subclasses of the given superclass, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectSubClasses (long rSuperClass);

  /**
     * Checks whether the given reference is associated with a class.
     * <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
     * @param r a reference in question
     * @return whether the given reference is associated with a class.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isClass (long r);

  /**
     * Checks whether the generalization relation between the two given classes holds.
     * <BR/><I><B>Note (M3): </B></I>Both classes may be either quasi-ontological, or quasi-linguistic.
     * @param rSubClass a reference to a potential subclass
     * @param rSuperClass a reference to a potential superclass
     * @return whether the generalization relation holds.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isDirectSubClass (long rSubClass, long rSuperClass);

  /**
     * Checks whether one class is a direct or indirect subclass of another.
     * <BR/><I><B>Note (M3): </B></I>Both classes may be either quasi-ontological, or quasi-linguistic.
     * @param rDirectlyOrIndirectlyDerivedClass a reference to a potential subclass or derived class
     * @param rSuperClass a reference to a potential (direct or indirect) superclass
     * @return whether the first class derives from the second.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isDerivedClass (long rDirectlyOrIndirectlyDerivedClass, long rSuperClass);

  /**
     * Creates an instance of the given class.
     * <BR/><I><B>Note (M3): </B></I>If the given class is a quasi-linguistic class, then
     * its quasi-linguistic instance at Level M&Omega; is being created.
     * @param rClass a reference to a class (either quasi-ontological, or quasi-linguistic)
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long createObject (long rClass);

  /**
     * Creates the given object.
     * @param rObject a reference to the object to be deleted
     * @return whether the operation succeeded.
     */
  boolean deleteObject (long rObject);

  /**
     * Adds the given object to the given (quasi-ontological) class.
     * The function works, if the underlying repository supports
     * multiple classification and dynamic reclassification.
     * <BR/><I><B>Note (M3): </B></I>It is assumed that an element
     * from a quasi-ontological level can be associated with only one
     * quasi-linguistic type (quasi-linguistic class), thus, <code>includeObjectInClass</code>
     * is meaningless in this case.
     * @param rObject a reference to the object to be included in the given class
     * @param rClass a reference to the class, where to put the object (in addition to
     *        classes, where the object already belongs)
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean includeObjectInClass (long rObject, long rClass);

  /**
     * Takes out the given object from the given (quasi-ontological) class.
     * <BR/>
     * The function works, if the underlying repository supports
     * multiple classification and dynamic reclassification.
     * If the object currently is only in one class, then
     * the operation fails (it is assumed that each object must be at
     * least in one class).
     * <BR/><I><B>Note (M3): </B></I>It is assumed that an element
     * from a quasi-ontological level can be associated with only one
     * quasi-linguistic type (quasi-linguistic class), thus, <code>excludeObjectInClass</code>
     * as well as <code>includeObjectInClass</code>
     * are meaningless in this case.
     * @param rObject a reference to the object to be excluded from the given class
     * @param rClass a reference to the class, which to exclude from the
     *        classifiers of the given object
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean excludeObjectFromClass (long rObject, long rClass);

  /**
     * Moves (reclassifies) the given object into the given (quasi-ontological) class, removing it from its current class (classes).
     * <BR/>
     * The function is similar to calling
     * <BR/><code>includeObjectInClass(rObject, rToClass);</code>
     * followed by calling
     * <BR/><code>excludeObjectInClass(rObject, <i>c</i>)</code>
     * for all other current classifiers <i>c</i> of the given object.
     * <BR/>
     * The distinction is that it may be possible to implement
     * this function even when multiple classification is not supported.   
     * <BR/><I><B>Note (adapters): </B></I>This function is optional for
     * repository adapters. If not implemented in an adapter,
     * TDA Kernel implements it by
     * [TODO] recreating the object (with the new type), while
     * also recreating all attributes and links.
     * <BR/><I><B>Note (M3): </B></I>It is assumed that an element
     * from a quasi-ontological level cannot dynamically change its
     * quasi-linguistic type (quasi-linguistic class), thus, <code>moveObject</code>
     * is meaningless in this case.
     * @param rObject a reference to the object to be reclassified
     * @param rToClass a reference to the class, to which the object will belong
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean moveObject (long rObject, long rToClass);

  /**
     * Obtains an iterator for all quasi-ontological instances of the given class or advanced association.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     * may implement only one of the functions <code>getIteratorForAllClassObjects</code>
     * and <code>getIteratorForDirectClassObjects</code>.
     * The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>If the given class or advanced association is quasi-linguistic,
     * then an iterator for the quasi-linguistic elements it describes is returned, e.g.,
     * for the EMOF class "Class", an iterator for
     * all classes found in EMOF is returned; for the EMOF class "Property", an iterator
     * for all properties found in EMOF is returned, etc.
     * @param rClassOrAdvancedAssociation a reference to a class or an advanced association
     * @return an iterator for all quasi-ontological instances (objects)
     *         of the given class or advanced association. On error, 0 is returned.
     * @see RAAPI#getIteratorForDirectClassObjects
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForAllClassObjects (long rClassOrAdvancedAssociation);

  /**
     * Obtains an iterator for direct quasi-ontological instances of the given class or advanced association.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     * may implement only one of the functions <code>getIteratorForAllClassObjects</code>
     * and <code>getIteratorForDirectClassObjects</code>.
     * The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>If the given class or advanced association is quasi-linguistic,
     * then an iterator for the quasi-linguistic elements it describes is returned, e.g.,
     * for the EMOF class "Class", an iterator for
     * all classes found in EMOF is returned; for the EMOF class "Property", an iterator
     * for all properties found in EMOF is returned, etc.
     * @param rClassOrAdvancedAssociation a reference to a class or an advanced association
     * @return an iterator for direct quasi-ontological instances (objects)
     * of the given class or advanced association. On error, 0 is returned.
     * @see RAAPI#getIteratorForAllClassObjects
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectClassObjects (long rClassOrAdvancedAssociation);

  /**
     * Obtains an iterator for direct quasi-ontological classes of the given object or advanced link.
     * <BR/><I><B>Note (M3): </B></I>The function works also 
     * if the given object or advanced link is quasi-linguistic and the
     * underlying repository provides access to quasi-linguistic elements.
     * To get the quasi-linguistic class for the given element at some quasi-ontological level,
     * use <code>getLinguisticClassFor</code>.
     * @param rObjectOrAdvancedLink a reference to an object or advanced link
     * @return an iterator for direct quasi-ontological classes of the given object or advanced link.
     * On error, 0 is returned.
     * @see RAAPI#getLinguisticClassFor
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectObjectClasses (long rObjectOrAdvancedLink);

  /**
     * Checks whether the given object is a direct (quasi-ontological or quasi-linguistic) instance of the given class.
     * <BR/><I><B>Note (M3): </B></I>The function works also when
     * one or both of <code>rObject</code> and <code>rClass</code> is/are quasi-linguistic.
     * If the object is at a quasi-ontological meta-level, but the class is quasi-linguistic,
     * then the function checks whether the object is a direct quasi-linguistic instance of the given class.
     * @param rObject a reference to an object
     * @param rClass a reference to a class
     * @return whether the given object is a direct instance of the given class.
     * On error, <code>false</code> is returned.
     * @see RAAPI#isKindOf
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isTypeOf (long rObject, long rClass);

  /**
     * Checks whether the given object is a direct or indirect, quasi-ontological or quasi-linguistic, instance of the given class.
     * <BR/><I><B>Note (M3): </B></I>The function works also when
     * one or both of <code>rObject</code> and <code>rClass</code> is/are quasi-linguistic.
     * If the object is at a quasi-ontological meta-level, but the class is quasi-linguistic,
     * then the function checks whether the object is a quasi-linguistic instance of the given class
     * or one of its subclasses.
     * @param rObject a reference to an object
     * @param rClass a reference to a class
     * @return whether the given object is a (direct or indirect) instance of the given class.
     * On error, <code>false</code> is returned.
     * @see RAAPI#isTypeOf
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isKindOf (long rObject, long rClass);

  /**
     * Creates (defines) a new attribute for the given class.
     * The default cardinality is the widest cardinality supported by the repository
     * (e.g., "0..*", if multi-valued attributes are supported; or "0..1", otherwise).
     * The cardinality can be looked up and changed by using the quasi-linguistic meta-metalevel.
     * @param rClass a reference to an existing class, for which to define the attribute
     * @param name the name of the attribute being created; it must be unique
     *             within all the attributes defined for this class, including
     *             derived ones
     * @param rPrimitiveType a reference to a primitive data type for attribute values
     * @return a reference to the attribute just created, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long createAttribute (long rClass, String name, long rPrimitiveType);

  /**
     * Obtains a reference to an existing attribute with the given name of the given class.
     * <BR/><I><B>Note (M3): </B></I>The class reference may point also to a quasi-linguistic class.
     * @param rClass a reference to a class, where the attribute in question belongs;
     *               <code>rClass</code> may be also one of its subclasses, since the attribute
     *               is available for subclasses, too
     * @param name the name of the attribute
     * @return a reference to the desired attribute, or 0 on error;
     *         the reference returned is the same reference for the class,
     *         for which the attribute was defined, as well as for derived classes.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long findAttribute (long rClass, String name);

  /**
     * Deletes the given attribute.
     * @param rAttribute a reference to the attribute to be deleted
     * @return whether the operation succeeded.
     */
  boolean deleteAttribute (long rAttribute);

  /**
     * Obtains an iterator for all (including inherited) attributes of the given class.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     * may implement only one of the functions <code>getIteratorForAllAttributes</code>
     * and <code>getIteratorForDirectAttributes</code>.
     * The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>The function works also for quasi-linguistic classes.
     * @param rClass a reference to a class, whose attributes we are interested in
     * @return an iterator for all attributes (including inherited) of the given class.
     * On error, 0 is returned.
     * @see RAAPI#getIteratorForDirectAttributes
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForAllAttributes (long rClass);

  /**
     * Obtains an iterator for direct (without inherited) attributes of the given class.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     * may implement only one of the functions <code>getIteratorForAllAttributes</code>
     * and <code>getIteratorForDirectAttributes</code>.
     * The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>The function works also for quasi-linguistic classes.
     * @param rClass a reference to a class, whose attributes we are interested in
     * @return an iterator for direct attributes of the given class.
     * On error, 0 is returned.
     * @see RAAPI#getIteratorForAllAttributes
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectAttributes (long rClass);

  /**
     * Returns the name of the given attribute.
     * <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
     *
     * @param rAttribute a reference to the attribute in question
     * @return the name of the given attribute, or <code>null</code> on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  String getAttributeName (long rAttribute);

  /**
     * Obtains a class, for which the given attribute was defined.
     * <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
     *
     * @param rAttribute a reference to the attribute in question
     * @return a reference to a class, for which the given attribute belongs, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getAttributeDomain (long rAttribute);

  /**
     * Returns the (primitive) type for values of the given attribute.
     * <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
     *
     * @param rAttribute a reference to the attribute in question
     * @return a reference to a primitive data type for values of the given attribute.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getAttributeType (long rAttribute);

  /**
     * Checks whether the given reference is associated with an attribute.
     * <BR/><I><B>Note (adapters): </B></I>If a repository adapter
     * does not implement this function, TDA Kernel will implement it by means of
     * <code>getAttributeDomain</code>, <code>getAttributeName</code> and <code>findAttribute</code>.
     * <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
     * @param r a reference in question
     * @return whether the given reference is associated with an attribute.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isAttribute (long r);

  /**
     * Sets the value or the ordered collection of values (encoded as a string) of the given attribute for the given object.
     * <BR/><I><B>Note (adapters): </B></I>Repository adapters
     *   may assume that the value is not <code>null</code> and not a string encoding <code>null</code>,
     *   since for those cases TDA Kernel forwards the call to <code>deleteAttributeValue</code>.
     * <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
     *   In this case the object can be any element at the M&Omega; level.
     * @param rObject the object, for which to set the attribute value (values)
     * @param rAttribute the attribute, for which to set the value; this attribute must be associated
     *        either with a quasi-ontological class or the quasi-linguistic class of the given object
     * @param value the attribute value (values) encoded as a string (use "." for the decimal point)
     * @return whether the value(s) has (have) been set.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean setAttributeValue (long rObject, long rAttribute, String value);

  /**
     * Gets the value or the ordered collection of values (encoded as a string) of the given attribute for the given object.
     * <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
     * @param rObject the object, for which to get the attribute value (values)
     * @param rAttribute the attribute, for which to obtain the value; this attribute must be associated
     *        either with a quasi-ontological class or the quasi-linguistic class of the given object
     * @return the attribute value (values) encoded as a string
     *         (for the decimal point the dot symbol "." is used), or <code>null</code> on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  String getAttributeValue (long rObject, long rAttribute);

  /**
     * Deletes the value (all the values) of the given attribute for the given object.
     * <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
     *   In this case the object can be any element at the M&Omega; level.
     * @param rObject the object, for which to get the attribute value (values)
     * @param rAttribute the attribute, for which to obtain the value; this attribute must be associated
     *        either with a quasi-ontological class or the quasi-linguistic class of the given object
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean deleteAttributeValue (long rObject, long rAttribute);

  /**
     * Obtains an iterator for objects, for whose the value of the given attribute equals to the given value.
     * The value has to be encoded as a string (it may encode an ordered collection of multiple values).
     * <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
     *   In this case the objects traversed by the returned iterator are elements at the M&Omega; level.
     * @param rAttribute the attribute to check
     * @param value the value to check
     * @return the iterator for objects with the given attribute value, or 0 on error.
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForObjectsByAttributeValue (long rAttribute, String value);

  /**
     * Creates a bidirectional association (or two directed associations, where each is an inverse of the other).
     * The default value for the source and target cardinalities should be "*".
     * <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
     *   if the repository supports constraints and the M3 level operations. Cardinality constraints
     *   must be accessible via M3 for that.
     * @param rSourceClass the class, where the association starts
     * @param rTargetClass the class, where the association ends
     * @param sourceRoleName the name of the association end near the source class
     * @param targetRoleName the name of the association end near the target class
     * @param isComposition whether the association is a composition, i.e.,
     *        the source class objects are containers for the target class objects
     * @return a reference for the target association end of the association just created, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long createAssociation (long rSourceClass, long rTargetClass, String sourceRoleName, String targetRoleName, boolean isComposition);

  /**
     * Creates a directed association.
     * The default value for the source and target cardinalities should be "*".
     * <BR/><I><B>Note (adapters): </B></I>If a repository adapter
     *   does not implement this function, TDA kernel will simulate it by means of
     *   <code>createAssociation</code> (a stub inverse role will be generated).
     * <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
     *   if the repository supports constraints and the M3 level operations. Cardinality constraints
     *   must be accessible via M3 for that.
     * @param rSourceClass the class, where the association starts
     * @param rTargetClass the class, where the association ends
     * @param targetRoleName the name of the association end near the target class
     * @param isComposition whether the association is a composition, i.e.,
     *        the source class objects are containers for the target class objects
     * @return a reference for the target association end of the association just created, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long createDirectedAssociation (long rSourceClass, long rTargetClass, String targetRoleName, boolean isComposition);

  /**
     * Creates an n-ary association, an association class, or an n-ary association class.
     * <BR/>
     * An advanced association behaves likes a class (although it might not be a class internally)
     * with n bidirectional associations attached to it. To specify all n association ends,
     * call <code>createAssociation</code> n times, where
     * a reference to the n-ary association has to be passed instead of one of the class references.
     * N-ary association links can be created by means of <code>createObject</code>,
     * and n-ary link ends can be created by calling <code>createLink</code> n times
     * and passing a reference to the n-ary link instead of one of the object references.
     *
     * <BR/><I><B>Note (adapters): </B></I>The underlying repository
     *   is allowed to create an n-ary association class, even when nAry or associationClass is <code>false</code>.
     * <BR/><I><B>Note (adapters): </B></I>If a repository adapter
     *   does not implement this function, TDA kernel will
     *   implement this function by introducing an additional class.
     * <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
     *   if the repository supports constraints and the M3 level operations. Cardinality constraints
     *   must be accessible via M3 for that.
     * @param name the name of the advanced association (the class name in case of an association class)
     * @param nAry whether the association is an n-ary association
     * @param associationClass whether the association is an association class
     * @return a reference to the n-ary association just created (not the association end, since no association ends are created yet), or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     */
  long createAdvancedAssociation (String name, boolean nAry, boolean associationClass);

  /**
     * Obtains a reference to an association end (by its role name) starting at the given class.
     * 
     * <BR/><I><B>Note (adapters): </B></I>If not implemented in the adapter,
     *   TDA kernel will implement it by means of <code>getIteratorForAllOutgoingAssociationEnds</code>.
     * <BR/><I><B>Note (M3): </B></I>The function works also, when searching for association ends at Level M3.
     * @param rSourceClass a class that is a source class for the association, or one of its subclasses
     * @param targetRoleName a role name associated with the target association end
     * @return a reference to an association end corresponding to the given target role name.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long findAssociationEnd (long rSourceClass, String targetRoleName);

  /**
     * Deletes the given association.
     * Directed and bidirectional associations are specified by (one of) their ends.
     * Advanced associations have their own references.
     * If the association is bidirectional, the inverse association end is deleted as well.
     * For advanced associations, all association parts are deleted.
     * @param rAssociationEndOrAdvancedAssociation a reference to an association end (if
     *        the association is directed or bidirectional) or a reference to
     *        an advanced association
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     */
  boolean deleteAssociation (long rAssociationEndOrAdvancedAssociation);

  /**
     * Obtains an iterator for all (including inherited) outgoing association ends of the given class.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     *   may implement only one of the functions <code>getIteratorForAllOutgoingAssociationEnds</code>
     *   and <code>getIteratorForDirectOutgoingAssociationEnds</code>.
     *   The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
     * @param rClass a reference to a class, whose outgoing associations (including inherited) have to be traversed
     * @return an iterator for all (including inherited) outgoing association ends of the given class.
     *   On error, 0 is returned.
     * @see RAAPI#getIteratorForDirectOutgoingAssociationEnds
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForAllOutgoingAssociationEnds (long rClass);

  /**
     * Obtains an iterator for direct (without inherited) outgoing association ends of the given class.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     *   may implement only one of the functions <code>getIteratorForAllOutgoingAssociationEnds</code>
     *   and <code>getIteratorForDirectOutgoingAssociationEnds</code>.
     *   The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
     * @param rClass a reference to a class, whose direct outgoing associations have to be traversed
     * @return an iterator for direct (without inherited) outgoing association ends of the given class.
     *   On error, 0 is returned.
     * @see RAAPI#getIteratorForAllOutgoingAssociationEnds
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectOutgoingAssociationEnds (long rClass);

  /**
     * Obtains an iterator for all (including inherited) ingoing association ends of the given class.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     *   may implement only one of the functions <code>getIteratorForAllIngoingAssociationEnds</code>
     *   and <code>getIteratorForDirectIngoingAssociationEnds</code>.
     *   The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
     * @param rClass a reference to a class, whose ingoing associations (including inherited) have to be traversed
     * @return an iterator for all (including inherited) ingoing association ends of the given class.
     *   On error, 0 is returned.
     * @see RAAPI#getIteratorForDirectIngoingAssociationEnds
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForAllIngoingAssociationEnds (long rClass);

  /**
     * Obtains an iterator for direct (without inherited) ingoing association ends of the given class.
     * <BR/><I><B>Note (adapters): </B></I>A repository adapter
     *   may implement only one of the functions <code>getIteratorForAllIngoingAssociationEnds</code>
     *   and <code>getIteratorForDirectIngoingAssociationEnds</code>.
     *   The unimplemented function will be implemented via another by TDA Kernel.
     * <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
     * @param rClass a reference to a class, whose direct ingoing associations have to be traversed
     * @return an iterator for direct (without inherited) ingoing association ends of the given class.
     *   On error, 0 is returned.
     * @see RAAPI#getIteratorForAllIngoingAssociationEnds
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectIngoingAssociationEnds (long rClass);

  /**
     * Obtains a reference to the inverse association end of the given association end (if association is bidirectional or a bidirectional part of an advanced association).
     * <BR/><I><B>Note (M3): </B></I>The function works also for association ends at Level M3.
     * @param rAssociationEnd a reference to a known association end, for which the inverse end has to be obtained
     * @return a reference to the inverse association end. On error or if the association end
     *   does not have the inverse, 0 is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getInverseAssociationEnd (long rAssociationEnd);

  /**
     * Obtains a reference to the source class of the given directed or bidirectional association (or part of an advanced association) specified by its target end.
     * Any of the association ends can be considered a target end, when calling this function.
     * @param rTargetAssociationEnd an association end of some association;
     *   this association end will be considered a target end
     * @return a reference to the source class of the given association specified by its target end.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     */
  long getSourceClass (long rTargetAssociationEnd);

  /**
     * Obtains a reference to the class corresponding to the given association end of some directed, bidirectional, or advanced association.
     * For bidirectional and advanced associations, any of the two association ends can be considered a target end, when calling this function.
     * @param rTargetAssociationEnd an association end of some association;
     *   this association end will be considered a target end
     * @return a reference to the class corresponding to the given association end.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     */
  long getTargetClass (long rTargetAssociationEnd);

  /**
     * Returns the role name of the given association end.
     * @param rAssociationEnd an association end of some directed, bidirectional, or advanced association
     * @return the role name of the given association end, or <code>null</code> on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     */
  String getRoleName (long rAssociationEnd);

  /**
     * Returns, whether the directed or bidirectional association (or a part of an advanced association) specified by its target association end is a composition (i.e., whether the source class objects are containers for the target class objects).
     * <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
     * @param rTargetAssociationEnd an association end of some association;
     *   this association end will be considered a target end
     * @return whether the directed or bidirectional association (or a part of an advanced association) is a composition.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isComposition (long rTargetAssociationEnd);

  /**
     * Checks, whether the given reference corresponds to an advanced association.
     * <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
     * @param r a reference in question
     * @return whether the given reference corresponds to an advanced association.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isAdvancedAssociation (long r);

  /**
     * Checks, whether the given reference corresponds to an association end.
     * <BR/><I><B>Note (adapters): </B></I>If not implemented in a repository adapter,
     *   TDA Kernel will implement it by means of <code>getSourceClass</code>, <code>getRoleName</code> and <code>findAssociationEnd</code>.
     * <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
     * @param r a reference in question
     * @return whether the given reference corresponds to an association end.
     *         On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isAssociationEnd (long r);

  /**
     * Creates a link of the given type (specified by <code>rAssociationEnd</code>) between two objects.
     * <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
     *   In this case, at least one of the source and target objects must be an element at the M&Omega; level.
     *   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
     * @param rSourceObject a start object of the link;
     *   this object must be an instance of the source class for the given association end
     * @param rTargetObject an end object of the link;
     *   this object must be an instance of the target class for the given association end
     * @param rAssociationEnd a target association end that specifies the link type
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean createLink (long rSourceObject, long rTargetObject, long rAssociationEnd);

  /**
     * Creates a link of the given type (specified by <code>rAssociationEnd</code>) between two objects at the given position.
     * The target position normally should be from 0 to n, where n is the number of currently linked objects at positions from 0 to n-1.
     * If the target position is outside [0..n], then the link is appended to the end.
     * <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
     *   In this case, at least one of the source and target objects must be an element at the M&Omega; level.
     *   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
     * @param rSourceObject a start object of the link;
     *   this object must be an instance of the source class for the given association end
     * @param rTargetObject an end object of the link;
     *   this object must be an instance of the target class for the given association end
     * @param rAssociationEnd a target association end that specifies the link type
     * @param targetPosition the position (starting from 0) of the target object in the list
     *          of linked objects of the source object;
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean createOrderedLink (long rSourceObject, long rTargetObject, long rAssociationEnd, int targetPosition);

  /**
     * Deletes a link of the given type (specified by <code>rTargetAssociationEnd</code>) between the given two objects.
     * <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
     *   In this case, at least one of the source and target objects must be an element at the M&Omega; level.
     *   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
     * @param rSourceObject a start object of the link;
     *   this object must be an instance of the source class for the given association end
     * @param rTargetObject an end object of the link;
     *   this object must be an instance of the target class for the given association end
     * @param rAssociationEnd a target association end that specifies the link type
     * @return whether the operation succeeded.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean deleteLink (long rSourceObject, long rTargetObject, long rAssociationEnd);

  /**
     * Checks whether the link of the given type (specified by <code>rTargetAssociationEnd</code>) between the given two objects exists.
     * <BR/><I><B>Note (adapters): </B></I>If not implemented in a repository adapter,
     *   TDA Kernel will implement this function through <code>getIteratorForLinkedObjects</code>.
     * <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
     *   In this case, at least one of the source and target objects must be an element at the M&Omega; level.
     *   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
     * @param rSourceObject a start object of the link;
     *   this object must be an instance of the source class for the given association end
     * @param rTargetObject an end object of the link;
     *   this object must be an instance of the target class for the given association end
     * @param rAssociationEnd a target association end that specifies the link type
     * @return whether the link exists. On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean linkExists (long rSourceObject, long rTargetObject, long rAssociationEnd);

  /**
     * Returns an iterator for objects linked to the given start object by links of the given type.
     * <BR/><I><B>Note (M3): </B></I>The type of links may also be an association end at Level M3.
     * @param rObject a start object, for which the iterable objects are linked
     *   this object must be an instance of the source class for the given association end
     * @param rAssociationEnd a target association end that specifies the type of links
     * @return an iterator for objects, linked to the given object by links of the given type, or 0 on error.
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForLinkedObjects (long rObject, long rAssociationEnd);

  /**
     * Returns the index (numeration starts from 0) of the target object in the list of objects linked to the source object by links of the given type.
     * <BR/><I><B>Note (M3): </B></I>The type of links may also be an association end at Level M3.
     * @param rSourceObject a source object;
     *   this object must be an instance of the source class for the given association end
     * @param rTargetObject a target object;
     *   this object must be an instance of the target class for the given association end
     * @param rAssociationEnd a target association end that specifies the type of links
     * @return the index (numeration starts from 0) of the given target object in the list of objects linked to the source object.
     *   On error or when the source and target objects are not linked by the given association,
     *   -1 is returned.
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A>
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  int getLinkedObjectPosition (long rSourceObject, long rTargetObject, long rAssociationEnd);

  /**
     * Places the iterator to the position 0 and gets the element there.
     * @param it an iterator reference
     * @return the element at position 0 in the iterable list. If there are no elements or if an error occurred, 0 is returned.
     * @see RAAPI#resolveIteratorNext
     * @see RAAPI#freeIterator
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     */
  long resolveIteratorFirst (long it);

  /**
     * Moves the iterator forward and gets the element at that position.
     * @param it an iterator reference
     * @return the element the iterator points to, after the iterator has been moved one step forward.
     *   If there are no elements or if an error occurred, 0 is returned.
     * @see RAAPI#resolveIteratorFirst
     * @see RAAPI#freeIterator
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     */
  long resolveIteratorNext (long it);

  /**
     * Places the iterator to the position 0 and returns the total number of elements to iterate through.
     * Call <code>resolveIteratorFirst</code> or <code>resolveIterator</code>
     * to move the iterator.
     * <BR/><I><B>Note (adapters): </B></I> If not implemented in a repository adapter,
     * TDA Kernel traverses all the elements and stores them in a temporary list.
     * Thus, the first call will take the linear execution time, while all subsequent calls
     * will take the constant time. The same refers to the <code>resolveIterator</code>
     * function. If both <code>getIteratorLength</code> and <code>resolveIterator</code>
     * are used, the temporary list is created only once.
     * @param it an iterator reference
     * @return the total number of elements to iterate through.
     * On error returns 0 (thus, the return value still represents the number of iterations, which can be performed with this iterator).
     * @see RAAPI#resolveIterator
     * @see RAAPI#freeIterator
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     */
  int getIteratorLength (long it);

  /**
     * Returns a reference to the element at the given <code>position</code> (numeration starts from 0) and forwards the iterator to <code>position+1</code>.
     * <BR/><I><B>Note (adapters): </B></I> If not implemented in a repository adapter,
     * TDA Kernel traverses all the elements and stores them in a temporary list.
     * Thus, the first call will take the linear execution time, while all subsequent calls
     * will take the constant time. The same refers to the <code>getIteratorLength</code>
     * function. If both <code>getIteratorLength</code> and <code>resolveIterator</code>
     * are used, the temporary list is created only once.
     * @param it an iterator reference
     * @param position the position in the iterable list, where the interested element is located
     * @return a reference to the element at the given position, or 0 if the position is out of bounds, or if an error occurred.
     * @see RAAPI#getIteratorLength
     * @see RAAPI#freeIterator
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     */
  long resolveIterator (long it, int position);

  /**
     * Frees the memory associated with the given iterator reference.
     * @param it an iterator reference
     * @see <a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a>
     */
  void freeIterator (long it);

  /**
     * Frees the memory associated with the given reference (if necessary).
     */
  void freeReference (long r);

  /**
     * Creates a string representation of the given reference, which survives the current session.
     * For the next session, TDA kernel will use this string to get another reference to the same element by means of
     * <code>deserializeReference</code>.
     * This is essential for storing inter-repository relations.
     * @param r the reference to serialize
     * @return a string representation of the given reference, which survives the current session, or <code>null</code> on error.
     * @see RAAPI#deserializeReference
     */
  String serializeReference (long r);

  /**
     * Obtains a reference to a serialized element from the given serialization.
     * This is essential for loading inter-repository relations.
     * @param r the serialization of an element, for which to obtain a reference
     * @return a reference corresponding for the given serialization, or 0 on error.
     * @see RAAPI#serializeReference
     */
  long deserializeReference (String r);

  /**
     * Returns an iterator for all quasi-linguistic classes at Level M3.
     * <BR/><I><B>Note (M3): </B></I>This function works only at Level M3.<BR/>&nbsp;
     * @return an iterator for all quasi-linguistic classes at Level M3.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForLinguisticClasses ();

  /**
     * Returns an iterator for direct quasi-linguistic instances (not including instances of subclasses) at Level M&Omega; of the given class at Level M3.
     * <BR/><I><B>Note (M3): </B></I>This function takes a class at Level M3 and returns an iterator for elements at Level M&Omega;.
     * @param rClass a Level M3 class
     * @return an iterator for direct quasi-linguistic instances at Level M&Omega; of the given class at Level M3, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForDirectLinguisticInstances (long rClass);

  /**
     * Returns an iterator for all quasi-linguistic instances (including instances of subclasses) at Level M&Omega; of the given class at Level M3.
     * <BR/><I><B>Note (M3): </B></I>This function takes a class at Level M3 and returns an iterator for elements at Level M&Omega;.
     * @param rClass a Level M3 class
     * @return an iterator for direct quasi-linguistic instances at Level M&Omega; of the given class (and its subclasses) at Level M3, or 0 on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getIteratorForAllLinguisticInstances (long rClass);

  /**
     * Returns a reference to the Level M3 class of the given quasi-ontological (Level M&Omega;) element.
     * It is assumed that there may be at most one quasi-linguistic class at M3 for each quasi-ontological element at M&Omega;.
     * @param r a quasi-ontological (Level M&Omega;) element
     * @return a reference to the Level M3 class of the given quasi-ontological (Level M&Omega;) element.
     *   On error or if M3 is not supported by the underlying repository, 0 is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  long getLinguisticClassFor (long r);

  /**
     * Checks, whether the given reference is associated with a Level M3 element.
     * Can be used together with <code>isClass</code>, <code>isAssociationEnd</code>, etc. to get more details about the element.
     * @param r a reference in question
     * @return whether the given reference is associated with a Level M3 element.
     *   On error, <code>false</code> is returned.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A>
     */
  boolean isLinguistic (long r);

  /**
     * Calls a repository-specific operation (e.g., or MOF/ECore-like operation, an SQL statement, or a SPARQL statement).
     * Arguments (if any) are encoded as a string delimited by means of the
     * Unicode character U+001E (INFORMATION SEPARATOR TWO).
     * For no-argument methods <code>arguments</code> must be <code>null</code>.
     * <BR/>
     * For instance, a repository may accept the following calls:
     * <BR/><code>callSpecificOperation("SQL", "SELECT * FROM MY TABLE");</code>
     * <BR/><code>callSpecificOperation("myMethod", "[object-reference][u001E][argument1]...");</code>
     * <BR/><code>callSpecificOperation("", null);</code>
     * <BR/>For static MOF/ECore-like operations, the first argument should point to a class.
     * For non-static operations the first argument should point to an object
     * (that resembles <code>this</code> pointer in Java).
     * @param operationName a repository-specific operation name
     * @param arguments operation-specific arguments encoded as a string
     * @return the return value of the call encoded as a string, or <code>null</code> on error.
     * @see <A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A>
     */
  String callSpecificOperation (String operationName, String arguments);
} // interface RAAPI
