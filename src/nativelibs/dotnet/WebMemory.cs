using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;


namespace org.webappos
{
	/// <summary>
	/// A .NET wrapper for Java-side web memory. It can either create a new web memory slot (in different ways), or associate itself with an existing web memory slot.
	/// </summary>
	public class WebMemory
	{
		private static bool is32 = (Marshal.SizeOf(typeof(IntPtr)) == 4);
		private IntPtr tdaKernel = IntPtr.Zero;
		[DllImport("tdakernel32", EntryPoint="TDA_GetTDAKernelReference")]
		private static extern IntPtr TDA_GetTDAKernelReference32(String protocolOrURI);
		[DllImport("tdakernel64", EntryPoint="TDA_GetTDAKernelReference")]
		private static extern IntPtr TDA_GetTDAKernelReference64(String protocolOrURI);

		/// <summary>
		/// Creates a new TDA Kernel instance in the Java VM side.
		/// Initializes a Java VM, if it has not been initialized for the current process yet.
		/// </summary>
		public TDAKernel()
		{
			//AttachConsole(-1);
			tdaKernel = is32 ?
				TDA_GetTDAKernelReference32(System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes("jni"))) :
				TDA_GetTDAKernelReference64(System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes("jni")));
			if (tdaKernel == null)
				throw new Exception("Could not initialize a TDA Kernel instance in native DLL or Java code.");
		}

		private TDAKernel(IntPtr _tdaKernel)
		{
			tdaKernel = _tdaKernel;
		}

		/// <summary>
		/// Either creates a new TDA Kernel instance and returns its reference (if just the name of a communication protocol is specified), or obtains a reference to an existing TDA Kernel (if an URI is specified).
		/// </summary>
		/// <param name="protocolOrURI">a string denoting either a protocol name ("jni", "pipe", "shared_memory", "corba"),
		///  which specifies the communication mechanism for a TDA Kernel instance to be created, or
		///  an URI ("jni:UUID", "shared_memory:MEMORY_NAME", "corba:IOR") for obtaining a reference to an
		///  existing TDA Kernel.</param>
		///  <returns>a reference to a TDA Kernel, or null on error.</returns>
		public static TDAKernel getTDAKernelReference(String protocolOrURI)
		{
			IntPtr _tdaKernel = is32 ?
				TDA_GetTDAKernelReference32(System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(protocolOrURI))) :
				TDA_GetTDAKernelReference64(System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(protocolOrURI)));
			if (_tdaKernel.ToInt64() == 0)
				return null;
			else
				return new TDAKernel(_tdaKernel);
		}

		/// <summary>
		/// Either creates a new TDA Kernel instance and returns its reference (if just the name of a communication protocol is specified), or obtains a reference to an existing TDA Kernel (if an URI is specified).
		/// </summary>
		/// <param name="protocolOrURI">a string denoting either a protocol name ("jni", "pipe", "shared_memory", "corba"),
		///  which specifies the communication mechanism for a TDA Kernel instance to be created, or
		///  an URI ("jni:UUID", "shared_memory:MEMORY_NAME", "corba:IOR") for obtaining a reference to an
		///  existing TDA Kernel.</param>
		///  <returns>a reference to a TDA Kernel, or null on error.</returns>
		public static TDAKernel GetTDAKernelReference(String protocolOrURI)
		{
			IntPtr _tdaKernel = is32 ?
				TDA_GetTDAKernelReference32(System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(protocolOrURI))) :
				TDA_GetTDAKernelReference64(System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(protocolOrURI)));
			if (_tdaKernel.ToInt64() == 0)
				return null;
			else
				return new TDAKernel(_tdaKernel);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FreeTDAKernelReference")]
		private static extern bool TDA_FreeTDAKernelReference32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_FreeTDAKernelReference")]
		private static extern bool TDA_FreeTDAKernelReference64(IntPtr tdaKernel);

		/// <summary>
		/// Frees references associated with the TDA Kernel.
		/// </summary>
		~TDAKernel()
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_FreeTDAKernelReference32(tdaKernel); else TDA_FreeTDAKernelReference64(tdaKernel);
			tdaKernel = IntPtr.Zero;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_Close")]
		private static extern void TDA_Close32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_Close")]
		private static extern void TDA_Close64(IntPtr tdaKernel);
		/// <summary>
		/// Closes the repository without save.<BR/> 
		/// </summary>
		/// <seealso cref="TDAKernel.startSave" />
		/// <seealso cref="TDAKernel.finishSave" />
		/// <seealso cref="TDAKernel.cancelSave" />
		public void close()
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_Close32(tdaKernel); else
				TDA_Close64(tdaKernel);
		}

		/// <summary>
		/// Closes the repository without save.<BR/> 
		/// </summary>
		/// <seealso cref="TDAKernel.startSave" />
		/// <seealso cref="TDAKernel.finishSave" />
		/// <seealso cref="TDAKernel.cancelSave" />
		public void Close()
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_Close32(tdaKernel); else
				TDA_Close64(tdaKernel);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_Exists")]
		private static extern byte TDA_Exists32(IntPtr tdaKernel, String location);
		[DllImport("tdakernel64", EntryPoint="TDA_Exists")]
		private static extern byte TDA_Exists64(IntPtr tdaKernel, String location);
		/// <summary>
		/// Checks whether the given location is already occupied by some repository of the same type.
		/// This can be used to ask for the user confirmation to drop an existing repository,
		/// when creating a new one at the same location.
		/// </summary>
		/// <param name="location">a string denoting the location to check. The location string is
		///   is specific to the type of the repository, e.g.,
		///   for ECore this is the .xmi file name, for JR this is the folder name, etc.
		///   TDA Kernel requires a URI, containing the repository name followed by a colon
		///   followed by a repository-specific location, e.g., "jr:/path/to/repository".</param>
		/// <returns>whether the given location is already occupied by some repository of the same type.</returns>
		public bool exists(String location)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _location = (location==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(location));
			return is32?TDA_Exists32(tdaKernel, _location)!=0:
				TDA_Exists64(tdaKernel, _location)!=0;
		}

		/// <summary>
		/// Checks whether the given location is already occupied by some repository of the same type.
		/// This can be used to ask for the user confirmation to drop an existing repository,
		/// when creating a new one at the same location.
		/// </summary>
		/// <param name="location">a string denoting the location to check. The location string is
		///   is specific to the type of the repository, e.g.,
		///   for ECore this is the .xmi file name, for JR this is the folder name, etc.
		///   TDA Kernel requires a URI, containing the repository name followed by a colon
		///   followed by a repository-specific location, e.g., "jr:/path/to/repository".</param>
		/// <returns>whether the given location is already occupied by some repository of the same type.</returns>
		public bool Exists(String location)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _location = (location==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(location));
			return is32?TDA_Exists32(tdaKernel, _location)!=0:
				TDA_Exists64(tdaKernel, _location)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_Open")]
		private static extern byte TDA_Open32(IntPtr tdaKernel, String location);
		[DllImport("tdakernel64", EntryPoint="TDA_Open")]
		private static extern byte TDA_Open64(IntPtr tdaKernel, String location);
		/// <summary>
		/// Opens or creates (if the repository does not exist yet) the repository at the given location.
		/// This can be used to ask for the user confirmation to drop an existing repository,
		/// when creating a new one at the same location.
		/// </summary>
		/// <param name="location">a string denoting the location of the repository. The location string is
		///   is specific to the type of the repository, e.g.,
		///   for ECore this is the .xmi file name, for JR this is the folder name, etc.
		///   TDA Kernel requires a URI, containing the repository name followed by a colon
		///   followed by a repository-specific location, e.g., "jr:/path/to/repository".</param>
		/// <returns>whether the repository has been opened or created.</returns>
		public bool open(String location)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _location = (location==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(location));
			return is32?TDA_Open32(tdaKernel, _location)!=0:
				TDA_Open64(tdaKernel, _location)!=0;
		}

		/// <summary>
		/// Opens or creates (if the repository does not exist yet) the repository at the given location.
		/// This can be used to ask for the user confirmation to drop an existing repository,
		/// when creating a new one at the same location.
		/// </summary>
		/// <param name="location">a string denoting the location of the repository. The location string is
		///   is specific to the type of the repository, e.g.,
		///   for ECore this is the .xmi file name, for JR this is the folder name, etc.
		///   TDA Kernel requires a URI, containing the repository name followed by a colon
		///   followed by a repository-specific location, e.g., "jr:/path/to/repository".</param>
		/// <returns>whether the repository has been opened or created.</returns>
		public bool Open(String location)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _location = (location==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(location));
			return is32?TDA_Open32(tdaKernel, _location)!=0:
				TDA_Open64(tdaKernel, _location)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_Drop")]
		private static extern byte TDA_Drop32(IntPtr tdaKernel, String location);
		[DllImport("tdakernel64", EntryPoint="TDA_Drop")]
		private static extern byte TDA_Drop64(IntPtr tdaKernel, String location);
		/// <summary>
		/// Deletes the repository at the given location.
		/// The repository must be closed.
		/// </summary>
		/// <param name="location">a string denoting the location of the repository. The location string
		///   is specific to the type of the repository, e.g.,
		///   for ECore this is the .xmi file name, for JR this is the folder name, etc.
		///   TDA Kernel requires a URI, containing the repository name followed by a colon
		///   followed by a repository-specific location, e.g., "jr:/path/to/repository".</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool drop(String location)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _location = (location==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(location));
			return is32?TDA_Drop32(tdaKernel, _location)!=0:
				TDA_Drop64(tdaKernel, _location)!=0;
		}

		/// <summary>
		/// Deletes the repository at the given location.
		/// The repository must be closed.
		/// </summary>
		/// <param name="location">a string denoting the location of the repository. The location string
		///   is specific to the type of the repository, e.g.,
		///   for ECore this is the .xmi file name, for JR this is the folder name, etc.
		///   TDA Kernel requires a URI, containing the repository name followed by a colon
		///   followed by a repository-specific location, e.g., "jr:/path/to/repository".</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool Drop(String location)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _location = (location==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(location));
			return is32?TDA_Drop32(tdaKernel, _location)!=0:
				TDA_Drop64(tdaKernel, _location)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_StartSave")]
		private static extern byte TDA_StartSave32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_StartSave")]
		private static extern byte TDA_StartSave64(IntPtr tdaKernel);
		/// <summary>
		/// Starts the two-phase save process of the repository.
		/// The save process can be rolled back by calling <code>cancelSave</code>
		/// or commited by calling <code>finishSave</code>.<BR/> 
		/// </summary>
		/// <returns>whether the operation succeeded. If <code>false</code> is returned,
		///   neither <code>cancelSave</code>, nor <code>finishSave</code> must be called.</returns>
		/// <seealso cref="TDAKernel.finishSave" />
		/// <seealso cref="TDAKernel.cancelSave" />
		public bool startSave()
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_StartSave32(tdaKernel)!=0:
				TDA_StartSave64(tdaKernel)!=0;
		}

		/// <summary>
		/// Starts the two-phase save process of the repository.
		/// The save process can be rolled back by calling <code>cancelSave</code>
		/// or commited by calling <code>finishSave</code>.<BR/> 
		/// </summary>
		/// <returns>whether the operation succeeded. If <code>false</code> is returned,
		///   neither <code>cancelSave</code>, nor <code>finishSave</code> must be called.</returns>
		/// <seealso cref="TDAKernel.finishSave" />
		/// <seealso cref="TDAKernel.cancelSave" />
		public bool StartSave()
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_StartSave32(tdaKernel)!=0:
				TDA_StartSave64(tdaKernel)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CancelSave")]
		private static extern byte TDA_CancelSave32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_CancelSave")]
		private static extern byte TDA_CancelSave64(IntPtr tdaKernel);
		/// <summary>
		/// Rolls back the started save process.
		/// The repository content on the disk (or other media) is returned to the previous state.
		/// The repository content currently loaded in memory is not changed.<BR/> 
		/// </summary>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso cref="TDAKernel.startSave" />
		/// <seealso cref="TDAKernel.finishSave" />
		public bool cancelSave()
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CancelSave32(tdaKernel)!=0:
				TDA_CancelSave64(tdaKernel)!=0;
		}

		/// <summary>
		/// Rolls back the started save process.
		/// The repository content on the disk (or other media) is returned to the previous state.
		/// The repository content currently loaded in memory is not changed.<BR/> 
		/// </summary>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso cref="TDAKernel.startSave" />
		/// <seealso cref="TDAKernel.finishSave" />
		public bool CancelSave()
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CancelSave32(tdaKernel)!=0:
				TDA_CancelSave64(tdaKernel)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FinishSave")]
		private static extern byte TDA_FinishSave32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_FinishSave")]
		private static extern byte TDA_FinishSave64(IntPtr tdaKernel);
		/// <summary>
		/// Finishes the two-phase save process of the repository.
		/// After finishing, the save process cannot be rolled back anymore.<BR/> 
		/// </summary>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso cref="TDAKernel.startSave" />
		/// <seealso cref="TDAKernel.cancelSave" />
		public bool finishSave()
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_FinishSave32(tdaKernel)!=0:
				TDA_FinishSave64(tdaKernel)!=0;
		}

		/// <summary>
		/// Finishes the two-phase save process of the repository.
		/// After finishing, the save process cannot be rolled back anymore.<BR/> 
		/// </summary>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso cref="TDAKernel.startSave" />
		/// <seealso cref="TDAKernel.cancelSave" />
		public bool FinishSave()
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_FinishSave32(tdaKernel)!=0:
				TDA_FinishSave64(tdaKernel)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FindClass")]
		private static extern long TDA_FindClass32(IntPtr tdaKernel, String name);
		[DllImport("tdakernel64", EntryPoint="TDA_FindClass")]
		private static extern long TDA_FindClass64(IntPtr tdaKernel, String name);
		/// <summary>
		/// Obtains a reference to an existing class with the given fully qualified name.
		/// <BR/><I><B>Note (M3): </B></I>If the underlying repository provides access to
		/// its quasi-linguistic meta-metamodel, quasi-linguistic classes can be accessed
		/// by using the prefix "M3::", e.g.,
		/// <code>SomePath::MountPoint::M3::SomeMetaType</code>
		/// <BR/><I><B>Note (adapters): </B></I>This function is optional for
		/// repository adapters. If not implemented in an adapter,
		/// TDA Kernel implements it through <code>getIteratorForClasses</code>
		/// [TODO] and <code>getIteratorForLinguisticClasses</code>.
		/// </summary>
		/// <param name="name">the fully qualified name of the class
		///             (for quasi-linguistic classes, use prefix "M3::")</param>
		/// <returns>a reference to a (quasi-ontological or quasi-linguistic) class with the given fully qualified name.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long findClass(String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_FindClass32(tdaKernel, _name):
				TDA_FindClass64(tdaKernel, _name);
		}

		/// <summary>
		/// Obtains a reference to an existing class with the given fully qualified name.
		/// <BR/><I><B>Note (M3): </B></I>If the underlying repository provides access to
		/// its quasi-linguistic meta-metamodel, quasi-linguistic classes can be accessed
		/// by using the prefix "M3::", e.g.,
		/// <code>SomePath::MountPoint::M3::SomeMetaType</code>
		/// <BR/><I><B>Note (adapters): </B></I>This function is optional for
		/// repository adapters. If not implemented in an adapter,
		/// TDA Kernel implements it through <code>getIteratorForClasses</code>
		/// [TODO] and <code>getIteratorForLinguisticClasses</code>.
		/// </summary>
		/// <param name="name">the fully qualified name of the class
		///             (for quasi-linguistic classes, use prefix "M3::")</param>
		/// <returns>a reference to a (quasi-ontological or quasi-linguistic) class with the given fully qualified name.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long FindClass(String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_FindClass32(tdaKernel, _name):
				TDA_FindClass64(tdaKernel, _name);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetClassName")]
		private static extern IntPtr TDA_GetClassName32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetClassName")]
		private static extern IntPtr TDA_GetClassName64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Returns the fully qualified name of the given class.
		/// <BR/>
		/// [TODO] <I><B>Note (M3): </B></I>If the reference points to a
		/// quasi-linguistic class, then
		/// the prefix "M3::" is also included in the return value, e.g.,
		/// <code>MountPointForTheCorrespondingRepository::M3::ClassName</code>.
		/// </summary>
		/// <param name="rClass">a reference to the class, for which the class name has to be obtained</param>
		/// <returns>the fully qualified name of the given class, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public String getClassName(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetClassName32(tdaKernel, rClass):
				TDA_GetClassName64(tdaKernel, rClass));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Returns the fully qualified name of the given class.
		/// <BR/>
		/// [TODO] <I><B>Note (M3): </B></I>If the reference points to a
		/// quasi-linguistic class, then
		/// the prefix "M3::" is also included in the return value, e.g.,
		/// <code>MountPointForTheCorrespondingRepository::M3::ClassName</code>.
		/// </summary>
		/// <param name="rClass">a reference to the class, for which the class name has to be obtained</param>
		/// <returns>the fully qualified name of the given class, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public String GetClassName(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetClassName32(tdaKernel, rClass):
				TDA_GetClassName64(tdaKernel, rClass));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateObject")]
		private static extern long TDA_CreateObject32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateObject")]
		private static extern long TDA_CreateObject64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Creates an instance of the given class.
		/// <BR/><I><B>Note (M3): </B></I>If the given class is a quasi-linguistic class, then
		/// its quasi-linguistic instance at Level M_Omega is being created.
		/// </summary>
		/// <param name="rClass">a reference to a class (either quasi-ontological, or quasi-linguistic)</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long createObject(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_CreateObject32(tdaKernel, rClass):
				TDA_CreateObject64(tdaKernel, rClass);
		}

		/// <summary>
		/// Creates an instance of the given class.
		/// <BR/><I><B>Note (M3): </B></I>If the given class is a quasi-linguistic class, then
		/// its quasi-linguistic instance at Level M_Omega is being created.
		/// </summary>
		/// <param name="rClass">a reference to a class (either quasi-ontological, or quasi-linguistic)</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long CreateObject(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_CreateObject32(tdaKernel, rClass):
				TDA_CreateObject64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsClass")]
		private static extern byte TDA_IsClass32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_IsClass")]
		private static extern byte TDA_IsClass64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Checks whether the given reference is associated with a class.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with a class.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isClass(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsClass32(tdaKernel, r)!=0:
				TDA_IsClass64(tdaKernel, r)!=0;
		}

		/// <summary>
		/// Checks whether the given reference is associated with a class.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with a class.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsClass(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsClass32(tdaKernel, r)!=0:
				TDA_IsClass64(tdaKernel, r)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_SerializeReference")]
		private static extern IntPtr TDA_SerializeReference32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_SerializeReference")]
		private static extern IntPtr TDA_SerializeReference64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Creates a string representation of the given reference, which survives the current session.
		/// For the next session, TDA kernel will use this string to get another reference to the same element by means of
		/// <code>deserializeReference</code>.
		/// This is essential for storing inter-repository relations.
		/// </summary>
		/// <param name="r">the reference to serialize</param>
		/// <returns>a string representation of the given reference, which survives the current session, or <code>null</code> on error.</returns>
		/// <seealso cref="TDAKernel.deserializeReference" />
		public String serializeReference(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_SerializeReference32(tdaKernel, r):
				TDA_SerializeReference64(tdaKernel, r));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Creates a string representation of the given reference, which survives the current session.
		/// For the next session, TDA kernel will use this string to get another reference to the same element by means of
		/// <code>deserializeReference</code>.
		/// This is essential for storing inter-repository relations.
		/// </summary>
		/// <param name="r">the reference to serialize</param>
		/// <returns>a string representation of the given reference, which survives the current session, or <code>null</code> on error.</returns>
		/// <seealso cref="TDAKernel.deserializeReference" />
		public String SerializeReference(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_SerializeReference32(tdaKernel, r):
				TDA_SerializeReference64(tdaKernel, r));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForAllOutgoingAssociationEnds")]
		private static extern long TDA_GetIteratorForAllOutgoingAssociationEnds32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForAllOutgoingAssociationEnds")]
		private static extern long TDA_GetIteratorForAllOutgoingAssociationEnds64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Obtains an iterator for all (including inherited) outgoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllOutgoingAssociationEnds</code>
		///   and <code>getIteratorForDirectOutgoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose outgoing associations (including inherited) have to be traversed</param>
		/// <returns>an iterator for all (including inherited) outgoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectOutgoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForAllOutgoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllOutgoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForAllOutgoingAssociationEnds64(tdaKernel, rClass);
		}

		/// <summary>
		/// Obtains an iterator for all (including inherited) outgoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllOutgoingAssociationEnds</code>
		///   and <code>getIteratorForDirectOutgoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose outgoing associations (including inherited) have to be traversed</param>
		/// <returns>an iterator for all (including inherited) outgoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectOutgoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForAllOutgoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllOutgoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForAllOutgoingAssociationEnds64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForAllIngoingAssociationEnds")]
		private static extern long TDA_GetIteratorForAllIngoingAssociationEnds32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForAllIngoingAssociationEnds")]
		private static extern long TDA_GetIteratorForAllIngoingAssociationEnds64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Obtains an iterator for all (including inherited) ingoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllIngoingAssociationEnds</code>
		///   and <code>getIteratorForDirectIngoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose ingoing associations (including inherited) have to be traversed</param>
		/// <returns>an iterator for all (including inherited) ingoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectIngoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForAllIngoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllIngoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForAllIngoingAssociationEnds64(tdaKernel, rClass);
		}

		/// <summary>
		/// Obtains an iterator for all (including inherited) ingoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllIngoingAssociationEnds</code>
		///   and <code>getIteratorForDirectIngoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose ingoing associations (including inherited) have to be traversed</param>
		/// <returns>an iterator for all (including inherited) ingoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectIngoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForAllIngoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllIngoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForAllIngoingAssociationEnds64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectLinguisticInstances")]
		private static extern long TDA_GetIteratorForDirectLinguisticInstances32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectLinguisticInstances")]
		private static extern long TDA_GetIteratorForDirectLinguisticInstances64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Returns an iterator for direct quasi-linguistic instances (not including instances of subclasses) at Level M_Omega of the given class at Level M3.
		/// <BR/><I><B>Note (M3): </B></I>This function takes a class at Level M3 and returns an iterator for elements at Level M_Omega.
		/// </summary>
		/// <param name="rClass">a Level M3 class</param>
		/// <returns>an iterator for direct quasi-linguistic instances at Level M_Omega of the given class at Level M3, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectLinguisticInstances(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectLinguisticInstances32(tdaKernel, rClass):
				TDA_GetIteratorForDirectLinguisticInstances64(tdaKernel, rClass);
		}

		/// <summary>
		/// Returns an iterator for direct quasi-linguistic instances (not including instances of subclasses) at Level M_Omega of the given class at Level M3.
		/// <BR/><I><B>Note (M3): </B></I>This function takes a class at Level M3 and returns an iterator for elements at Level M_Omega.
		/// </summary>
		/// <param name="rClass">a Level M3 class</param>
		/// <returns>an iterator for direct quasi-linguistic instances at Level M_Omega of the given class at Level M3, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectLinguisticInstances(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectLinguisticInstances32(tdaKernel, rClass):
				TDA_GetIteratorForDirectLinguisticInstances64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForObjectsByAttributeValue")]
		private static extern long TDA_GetIteratorForObjectsByAttributeValue32(IntPtr tdaKernel, long rAttribute, String value);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForObjectsByAttributeValue")]
		private static extern long TDA_GetIteratorForObjectsByAttributeValue64(IntPtr tdaKernel, long rAttribute, String value);
		/// <summary>
		/// Obtains an iterator for objects, for whose the value of the given attribute equals to the given value.
		/// The value has to be encoded as a string (it may encode an ordered collection of multiple values).
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		///   In this case the objects traversed by the returned iterator are elements at the M_Omega level.
		/// </summary>
		/// <param name="rAttribute">the attribute to check</param>
		/// <param name="value">the value to check</param>
		/// <returns>the iterator for objects with the given attribute value, or 0 on error.</returns>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForObjectsByAttributeValue(long rAttribute, String value)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _value = (value==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(value));
			return is32?TDA_GetIteratorForObjectsByAttributeValue32(tdaKernel, rAttribute, _value):
				TDA_GetIteratorForObjectsByAttributeValue64(tdaKernel, rAttribute, _value);
		}

		/// <summary>
		/// Obtains an iterator for objects, for whose the value of the given attribute equals to the given value.
		/// The value has to be encoded as a string (it may encode an ordered collection of multiple values).
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		///   In this case the objects traversed by the returned iterator are elements at the M_Omega level.
		/// </summary>
		/// <param name="rAttribute">the attribute to check</param>
		/// <param name="value">the value to check</param>
		/// <returns>the iterator for objects with the given attribute value, or 0 on error.</returns>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForObjectsByAttributeValue(long rAttribute, String value)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _value = (value==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(value));
			return is32?TDA_GetIteratorForObjectsByAttributeValue32(tdaKernel, rAttribute, _value):
				TDA_GetIteratorForObjectsByAttributeValue64(tdaKernel, rAttribute, _value);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForAllLinguisticInstances")]
		private static extern long TDA_GetIteratorForAllLinguisticInstances32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForAllLinguisticInstances")]
		private static extern long TDA_GetIteratorForAllLinguisticInstances64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Returns an iterator for all quasi-linguistic instances (including instances of subclasses) at Level M_Omega of the given class at Level M3.
		/// <BR/><I><B>Note (M3): </B></I>This function takes a class at Level M3 and returns an iterator for elements at Level M_Omega.
		/// </summary>
		/// <param name="rClass">a Level M3 class</param>
		/// <returns>an iterator for direct quasi-linguistic instances at Level M_Omega of the given class (and its subclasses) at Level M3, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForAllLinguisticInstances(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllLinguisticInstances32(tdaKernel, rClass):
				TDA_GetIteratorForAllLinguisticInstances64(tdaKernel, rClass);
		}

		/// <summary>
		/// Returns an iterator for all quasi-linguistic instances (including instances of subclasses) at Level M_Omega of the given class at Level M3.
		/// <BR/><I><B>Note (M3): </B></I>This function takes a class at Level M3 and returns an iterator for elements at Level M_Omega.
		/// </summary>
		/// <param name="rClass">a Level M3 class</param>
		/// <returns>an iterator for direct quasi-linguistic instances at Level M_Omega of the given class (and its subclasses) at Level M3, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForAllLinguisticInstances(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllLinguisticInstances32(tdaKernel, rClass):
				TDA_GetIteratorForAllLinguisticInstances64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectOutgoingAssociationEnds")]
		private static extern long TDA_GetIteratorForDirectOutgoingAssociationEnds32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectOutgoingAssociationEnds")]
		private static extern long TDA_GetIteratorForDirectOutgoingAssociationEnds64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Obtains an iterator for direct (without inherited) outgoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllOutgoingAssociationEnds</code>
		///   and <code>getIteratorForDirectOutgoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose direct outgoing associations have to be traversed</param>
		/// <returns>an iterator for direct (without inherited) outgoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllOutgoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectOutgoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectOutgoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForDirectOutgoingAssociationEnds64(tdaKernel, rClass);
		}

		/// <summary>
		/// Obtains an iterator for direct (without inherited) outgoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllOutgoingAssociationEnds</code>
		///   and <code>getIteratorForDirectOutgoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose direct outgoing associations have to be traversed</param>
		/// <returns>an iterator for direct (without inherited) outgoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllOutgoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectOutgoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectOutgoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForDirectOutgoingAssociationEnds64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectObjectClasses")]
		private static extern long TDA_GetIteratorForDirectObjectClasses32(IntPtr tdaKernel, long rObjectOrAdvancedLink);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectObjectClasses")]
		private static extern long TDA_GetIteratorForDirectObjectClasses64(IntPtr tdaKernel, long rObjectOrAdvancedLink);
		/// <summary>
		/// Obtains an iterator for direct quasi-ontological classes of the given object or advanced link.
		/// <BR/><I><B>Note (M3): </B></I>The function works also 
		/// if the given object or advanced link is quasi-linguistic and the
		/// underlying repository provides access to quasi-linguistic elements.
		/// To get the quasi-linguistic class for the given element at some quasi-ontological level,
		/// use <code>getLinguisticClassFor</code>.
		/// </summary>
		/// <param name="rObjectOrAdvancedLink">a reference to an object or advanced link</param>
		/// <returns>an iterator for direct quasi-ontological classes of the given object or advanced link.
		/// On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getLinguisticClassFor" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectObjectClasses(long rObjectOrAdvancedLink)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectObjectClasses32(tdaKernel, rObjectOrAdvancedLink):
				TDA_GetIteratorForDirectObjectClasses64(tdaKernel, rObjectOrAdvancedLink);
		}

		/// <summary>
		/// Obtains an iterator for direct quasi-ontological classes of the given object or advanced link.
		/// <BR/><I><B>Note (M3): </B></I>The function works also 
		/// if the given object or advanced link is quasi-linguistic and the
		/// underlying repository provides access to quasi-linguistic elements.
		/// To get the quasi-linguistic class for the given element at some quasi-ontological level,
		/// use <code>getLinguisticClassFor</code>.
		/// </summary>
		/// <param name="rObjectOrAdvancedLink">a reference to an object or advanced link</param>
		/// <returns>an iterator for direct quasi-ontological classes of the given object or advanced link.
		/// On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getLinguisticClassFor" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectObjectClasses(long rObjectOrAdvancedLink)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectObjectClasses32(tdaKernel, rObjectOrAdvancedLink):
				TDA_GetIteratorForDirectObjectClasses64(tdaKernel, rObjectOrAdvancedLink);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectIngoingAssociationEnds")]
		private static extern long TDA_GetIteratorForDirectIngoingAssociationEnds32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectIngoingAssociationEnds")]
		private static extern long TDA_GetIteratorForDirectIngoingAssociationEnds64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Obtains an iterator for direct (without inherited) ingoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllIngoingAssociationEnds</code>
		///   and <code>getIteratorForDirectIngoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose direct ingoing associations have to be traversed</param>
		/// <returns>an iterator for direct (without inherited) ingoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllIngoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectIngoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectIngoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForDirectIngoingAssociationEnds64(tdaKernel, rClass);
		}

		/// <summary>
		/// Obtains an iterator for direct (without inherited) ingoing association ends of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		///   may implement only one of the functions <code>getIteratorForAllIngoingAssociationEnds</code>
		///   and <code>getIteratorForDirectIngoingAssociationEnds</code>.
		///   The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for associations at Level M3.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose direct ingoing associations have to be traversed</param>
		/// <returns>an iterator for direct (without inherited) ingoing association ends of the given class.
		///   On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllIngoingAssociationEnds" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectIngoingAssociationEnds(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectIngoingAssociationEnds32(tdaKernel, rClass):
				TDA_GetIteratorForDirectIngoingAssociationEnds64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetAttributeName")]
		private static extern IntPtr TDA_GetAttributeName32(IntPtr tdaKernel, long rAttribute);
		[DllImport("tdakernel64", EntryPoint="TDA_GetAttributeName")]
		private static extern IntPtr TDA_GetAttributeName64(IntPtr tdaKernel, long rAttribute);
		/// <summary>
		/// Returns the name of the given attribute.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute in question</param>
		/// <returns>the name of the given attribute, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public String getAttributeName(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetAttributeName32(tdaKernel, rAttribute):
				TDA_GetAttributeName64(tdaKernel, rAttribute));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Returns the name of the given attribute.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute in question</param>
		/// <returns>the name of the given attribute, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public String GetAttributeName(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetAttributeName32(tdaKernel, rAttribute):
				TDA_GetAttributeName64(tdaKernel, rAttribute));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorLength")]
		private static extern int TDA_GetIteratorLength32(IntPtr tdaKernel, long it);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorLength")]
		private static extern int TDA_GetIteratorLength64(IntPtr tdaKernel, long it);
		/// <summary>
		/// Places the iterator to the position 0 and returns the total number of elements to iterate through.
		/// Call <code>resolveIteratorFirst</code> or <code>resolveIterator</code>
		/// to move the iterator.
		/// <BR/><I><B>Note (adapters): </B></I> If not implemented in a repository adapter,
		/// TDA Kernel traverses all the elements and stores them in a temporary list.
		/// Thus, the first call will take the linear execution time, while all subsequent calls
		/// will take the constant time. The same refers to the <code>resolveIterator</code>
		/// function. If both <code>getIteratorLength</code> and <code>resolveIterator</code>
		/// are used, the temporary list is created only once.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <returns>the total number of elements to iterate through.
		/// On error returns 0 (thus, the return value still represents the number of iterations, which can be performed with this iterator).</returns>
		/// <seealso cref="TDAKernel.resolveIterator" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public int getIteratorLength(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorLength32(tdaKernel, it):
				TDA_GetIteratorLength64(tdaKernel, it);
		}

		/// <summary>
		/// Places the iterator to the position 0 and returns the total number of elements to iterate through.
		/// Call <code>resolveIteratorFirst</code> or <code>resolveIterator</code>
		/// to move the iterator.
		/// <BR/><I><B>Note (adapters): </B></I> If not implemented in a repository adapter,
		/// TDA Kernel traverses all the elements and stores them in a temporary list.
		/// Thus, the first call will take the linear execution time, while all subsequent calls
		/// will take the constant time. The same refers to the <code>resolveIterator</code>
		/// function. If both <code>getIteratorLength</code> and <code>resolveIterator</code>
		/// are used, the temporary list is created only once.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <returns>the total number of elements to iterate through.
		/// On error returns 0 (thus, the return value still represents the number of iterations, which can be performed with this iterator).</returns>
		/// <seealso cref="TDAKernel.resolveIterator" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public int GetIteratorLength(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorLength32(tdaKernel, it):
				TDA_GetIteratorLength64(tdaKernel, it);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsAdvancedAssociation")]
		private static extern byte TDA_IsAdvancedAssociation32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_IsAdvancedAssociation")]
		private static extern byte TDA_IsAdvancedAssociation64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Checks, whether the given reference corresponds to an advanced association.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference corresponds to an advanced association.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isAdvancedAssociation(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsAdvancedAssociation32(tdaKernel, r)!=0:
				TDA_IsAdvancedAssociation64(tdaKernel, r)!=0;
		}

		/// <summary>
		/// Checks, whether the given reference corresponds to an advanced association.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference corresponds to an advanced association.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsAdvancedAssociation(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsAdvancedAssociation32(tdaKernel, r)!=0:
				TDA_IsAdvancedAssociation64(tdaKernel, r)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForLinkedObjects")]
		private static extern long TDA_GetIteratorForLinkedObjects32(IntPtr tdaKernel, long rObject, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForLinkedObjects")]
		private static extern long TDA_GetIteratorForLinkedObjects64(IntPtr tdaKernel, long rObject, long rAssociationEnd);
		/// <summary>
		/// Returns an iterator for objects linked to the given start object by links of the given type.
		/// <BR/><I><B>Note (M3): </B></I>The type of links may also be an association end at Level M3.
		/// </summary>
		/// <param name="rObject">a start object, for which the iterable objects are linked
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the type of links</param>
		/// <returns>an iterator for objects, linked to the given object by links of the given type, or 0 on error.</returns>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForLinkedObjects(long rObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForLinkedObjects32(tdaKernel, rObject, rAssociationEnd):
				TDA_GetIteratorForLinkedObjects64(tdaKernel, rObject, rAssociationEnd);
		}

		/// <summary>
		/// Returns an iterator for objects linked to the given start object by links of the given type.
		/// <BR/><I><B>Note (M3): </B></I>The type of links may also be an association end at Level M3.
		/// </summary>
		/// <param name="rObject">a start object, for which the iterable objects are linked
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the type of links</param>
		/// <returns>an iterator for objects, linked to the given object by links of the given type, or 0 on error.</returns>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForLinkedObjects(long rObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForLinkedObjects32(tdaKernel, rObject, rAssociationEnd):
				TDA_GetIteratorForLinkedObjects64(tdaKernel, rObject, rAssociationEnd);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetLinkedObjectPosition")]
		private static extern int TDA_GetLinkedObjectPosition32(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_GetLinkedObjectPosition")]
		private static extern int TDA_GetLinkedObjectPosition64(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		/// <summary>
		/// Returns the index (numeration starts from 0) of the target object in the list of objects linked to the source object by links of the given type.
		/// <BR/><I><B>Note (M3): </B></I>The type of links may also be an association end at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a source object;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">a target object;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the type of links</param>
		/// <returns>the index (numeration starts from 0) of the given target object in the list of objects linked to the source object.
		///   On error or when the source and target objects are not linked by the given association,
		///   -1 is returned.</returns>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public int getLinkedObjectPosition(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetLinkedObjectPosition32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd):
				TDA_GetLinkedObjectPosition64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd);
		}

		/// <summary>
		/// Returns the index (numeration starts from 0) of the target object in the list of objects linked to the source object by links of the given type.
		/// <BR/><I><B>Note (M3): </B></I>The type of links may also be an association end at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a source object;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">a target object;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the type of links</param>
		/// <returns>the index (numeration starts from 0) of the given target object in the list of objects linked to the source object.
		///   On error or when the source and target objects are not linked by the given association,
		///   -1 is returned.</returns>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public int GetLinkedObjectPosition(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetLinkedObjectPosition32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd):
				TDA_GetLinkedObjectPosition64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_ResolveIteratorFirst")]
		private static extern long TDA_ResolveIteratorFirst32(IntPtr tdaKernel, long it);
		[DllImport("tdakernel64", EntryPoint="TDA_ResolveIteratorFirst")]
		private static extern long TDA_ResolveIteratorFirst64(IntPtr tdaKernel, long it);
		/// <summary>
		/// Places the iterator to the position 0 and gets the element there.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <returns>the element at position 0 in the iterable list. If there are no elements or if an error occurred, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.resolveIteratorNext" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public long resolveIteratorFirst(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_ResolveIteratorFirst32(tdaKernel, it):
				TDA_ResolveIteratorFirst64(tdaKernel, it);
		}

		/// <summary>
		/// Places the iterator to the position 0 and gets the element there.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <returns>the element at position 0 in the iterable list. If there are no elements or if an error occurred, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.resolveIteratorNext" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public long ResolveIteratorFirst(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_ResolveIteratorFirst32(tdaKernel, it):
				TDA_ResolveIteratorFirst64(tdaKernel, it);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_ResolveIteratorNext")]
		private static extern long TDA_ResolveIteratorNext32(IntPtr tdaKernel, long it);
		[DllImport("tdakernel64", EntryPoint="TDA_ResolveIteratorNext")]
		private static extern long TDA_ResolveIteratorNext64(IntPtr tdaKernel, long it);
		/// <summary>
		/// Moves the iterator forward and gets the element at that position.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <returns>the element the iterator points to, after the iterator has been moved one step forward.
		///   If there are no elements or if an error occurred, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.resolveIteratorFirst" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public long resolveIteratorNext(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_ResolveIteratorNext32(tdaKernel, it):
				TDA_ResolveIteratorNext64(tdaKernel, it);
		}

		/// <summary>
		/// Moves the iterator forward and gets the element at that position.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <returns>the element the iterator points to, after the iterator has been moved one step forward.
		///   If there are no elements or if an error occurred, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.resolveIteratorFirst" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public long ResolveIteratorNext(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_ResolveIteratorNext32(tdaKernel, it):
				TDA_ResolveIteratorNext64(tdaKernel, it);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateDirectedAssociation")]
		private static extern long TDA_CreateDirectedAssociation32(IntPtr tdaKernel, long rSourceClass, long rTargetClass, String targetRoleName, byte isComposition);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateDirectedAssociation")]
		private static extern long TDA_CreateDirectedAssociation64(IntPtr tdaKernel, long rSourceClass, long rTargetClass, String targetRoleName, byte isComposition);
		/// <summary>
		/// Creates a directed association.
		/// The default value for the source and target cardinalities should be "*".
		/// <BR/><I><B>Note (adapters): </B></I>If a repository adapter
		///   does not implement this function, TDA kernel will simulate it by means of
		///   <code>createAssociation</code> (a stub inverse role will be generated).
		/// <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
		///   if the repository supports constraints and the M3 level operations. Cardinality constraints
		///   must be accessible via M3 for that.
		/// </summary>
		/// <param name="rSourceClass">the class, where the association starts</param>
		/// <param name="rTargetClass">the class, where the association ends</param>
		/// <param name="targetRoleName">the name of the association end near the target class</param>
		/// <param name="isComposition">whether the association is a composition, i.e.,
		///        the source class objects are containers for the target class objects</param>
		/// <returns>a reference for the target association end of the association just created, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long createDirectedAssociation(long rSourceClass, long rTargetClass, String targetRoleName, bool isComposition)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _targetRoleName = (targetRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(targetRoleName));
			byte _isComposition = Convert.ToByte(isComposition);
			return is32?TDA_CreateDirectedAssociation32(tdaKernel, rSourceClass, rTargetClass, _targetRoleName, _isComposition):
				TDA_CreateDirectedAssociation64(tdaKernel, rSourceClass, rTargetClass, _targetRoleName, _isComposition);
		}

		/// <summary>
		/// Creates a directed association.
		/// The default value for the source and target cardinalities should be "*".
		/// <BR/><I><B>Note (adapters): </B></I>If a repository adapter
		///   does not implement this function, TDA kernel will simulate it by means of
		///   <code>createAssociation</code> (a stub inverse role will be generated).
		/// <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
		///   if the repository supports constraints and the M3 level operations. Cardinality constraints
		///   must be accessible via M3 for that.
		/// </summary>
		/// <param name="rSourceClass">the class, where the association starts</param>
		/// <param name="rTargetClass">the class, where the association ends</param>
		/// <param name="targetRoleName">the name of the association end near the target class</param>
		/// <param name="isComposition">whether the association is a composition, i.e.,
		///        the source class objects are containers for the target class objects</param>
		/// <returns>a reference for the target association end of the association just created, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long CreateDirectedAssociation(long rSourceClass, long rTargetClass, String targetRoleName, bool isComposition)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _targetRoleName = (targetRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(targetRoleName));
			byte _isComposition = Convert.ToByte(isComposition);
			return is32?TDA_CreateDirectedAssociation32(tdaKernel, rSourceClass, rTargetClass, _targetRoleName, _isComposition):
				TDA_CreateDirectedAssociation64(tdaKernel, rSourceClass, rTargetClass, _targetRoleName, _isComposition);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForAllAttributes")]
		private static extern long TDA_GetIteratorForAllAttributes32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForAllAttributes")]
		private static extern long TDA_GetIteratorForAllAttributes64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Obtains an iterator for all (including inherited) attributes of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllAttributes</code>
		/// and <code>getIteratorForDirectAttributes</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for quasi-linguistic classes.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose attributes we are interested in</param>
		/// <returns>an iterator for all attributes (including inherited) of the given class.
		/// On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectAttributes" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForAllAttributes(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllAttributes32(tdaKernel, rClass):
				TDA_GetIteratorForAllAttributes64(tdaKernel, rClass);
		}

		/// <summary>
		/// Obtains an iterator for all (including inherited) attributes of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllAttributes</code>
		/// and <code>getIteratorForDirectAttributes</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for quasi-linguistic classes.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose attributes we are interested in</param>
		/// <returns>an iterator for all attributes (including inherited) of the given class.
		/// On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectAttributes" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForAllAttributes(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllAttributes32(tdaKernel, rClass):
				TDA_GetIteratorForAllAttributes64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectAttributes")]
		private static extern long TDA_GetIteratorForDirectAttributes32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectAttributes")]
		private static extern long TDA_GetIteratorForDirectAttributes64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Obtains an iterator for direct (without inherited) attributes of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllAttributes</code>
		/// and <code>getIteratorForDirectAttributes</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for quasi-linguistic classes.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose attributes we are interested in</param>
		/// <returns>an iterator for direct attributes of the given class.
		/// On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllAttributes" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectAttributes(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectAttributes32(tdaKernel, rClass):
				TDA_GetIteratorForDirectAttributes64(tdaKernel, rClass);
		}

		/// <summary>
		/// Obtains an iterator for direct (without inherited) attributes of the given class.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllAttributes</code>
		/// and <code>getIteratorForDirectAttributes</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for quasi-linguistic classes.
		/// </summary>
		/// <param name="rClass">a reference to a class, whose attributes we are interested in</param>
		/// <returns>an iterator for direct attributes of the given class.
		/// On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllAttributes" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectAttributes(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectAttributes32(tdaKernel, rClass):
				TDA_GetIteratorForDirectAttributes64(tdaKernel, rClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateOrderedLink")]
		private static extern byte TDA_CreateOrderedLink32(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd, int targetPosition);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateOrderedLink")]
		private static extern byte TDA_CreateOrderedLink64(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd, int targetPosition);
		/// <summary>
		/// Creates a link of the given type (specified by <code>rAssociationEnd</code>) between two objects at the given position.
		/// The target position normally should be from 0 to n, where n is the number of currently linked objects at positions from 0 to n-1.
		/// If the target position is outside [0..n], then the link is appended to the end.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <param name="targetPosition">the position (starting from 0) of the target object in the list
		///          of linked objects of the source object;</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool createOrderedLink(long rSourceObject, long rTargetObject, long rAssociationEnd, int targetPosition)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CreateOrderedLink32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd, targetPosition)!=0:
				TDA_CreateOrderedLink64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd, targetPosition)!=0;
		}

		/// <summary>
		/// Creates a link of the given type (specified by <code>rAssociationEnd</code>) between two objects at the given position.
		/// The target position normally should be from 0 to n, where n is the number of currently linked objects at positions from 0 to n-1.
		/// If the target position is outside [0..n], then the link is appended to the end.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <param name="targetPosition">the position (starting from 0) of the target object in the list
		///          of linked objects of the source object;</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool CreateOrderedLink(long rSourceObject, long rTargetObject, long rAssociationEnd, int targetPosition)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CreateOrderedLink32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd, targetPosition)!=0:
				TDA_CreateOrderedLink64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd, targetPosition)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FindPrimitiveDataType")]
		private static extern long TDA_FindPrimitiveDataType32(IntPtr tdaKernel, String name);
		[DllImport("tdakernel64", EntryPoint="TDA_FindPrimitiveDataType")]
		private static extern long TDA_FindPrimitiveDataType64(IntPtr tdaKernel, String name);
		/// <summary>
		/// Obtains a reference to a primitive data type with the given name.
		/// </summary>
		/// <param name="name">the type name. Each repository must support at least four
		///             standard primitive data types: "Integer", "Real", "Boolean", and "String".
		///             <BR/>
		///             [TODO] Certain repositories may introduce additional primitive types.
		///             To denote a repository-specific additional primitive data type,
		///             prepend the mount point of that repository, e.g.,
		///             <code>MountPoint::PeculiarDataType</code>.</param>
		/// <returns>a reference to a primitive data type with the given name, or 0 on error.
		/// <BR/><I><B>Note (TDA Kernel):</B></I>
		///   TDA Kernel returns a proxy reference, which 
		///   is usable even when there are multiple repositories mounted
		///   or non-standard primitive data types are used.</returns>
		public long findPrimitiveDataType(String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_FindPrimitiveDataType32(tdaKernel, _name):
				TDA_FindPrimitiveDataType64(tdaKernel, _name);
		}

		/// <summary>
		/// Obtains a reference to a primitive data type with the given name.
		/// </summary>
		/// <param name="name">the type name. Each repository must support at least four
		///             standard primitive data types: "Integer", "Real", "Boolean", and "String".
		///             <BR/>
		///             [TODO] Certain repositories may introduce additional primitive types.
		///             To denote a repository-specific additional primitive data type,
		///             prepend the mount point of that repository, e.g.,
		///             <code>MountPoint::PeculiarDataType</code>.</param>
		/// <returns>a reference to a primitive data type with the given name, or 0 on error.
		/// <BR/><I><B>Note (TDA Kernel):</B></I>
		///   TDA Kernel returns a proxy reference, which 
		///   is usable even when there are multiple repositories mounted
		///   or non-standard primitive data types are used.</returns>
		public long FindPrimitiveDataType(String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_FindPrimitiveDataType32(tdaKernel, _name):
				TDA_FindPrimitiveDataType64(tdaKernel, _name);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectClassObjects")]
		private static extern long TDA_GetIteratorForDirectClassObjects32(IntPtr tdaKernel, long rClassOrAdvancedAssociation);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectClassObjects")]
		private static extern long TDA_GetIteratorForDirectClassObjects64(IntPtr tdaKernel, long rClassOrAdvancedAssociation);
		/// <summary>
		/// Obtains an iterator for direct quasi-ontological instances of the given class or advanced association.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllClassObjects</code>
		/// and <code>getIteratorForDirectClassObjects</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>If the given class or advanced association is quasi-linguistic,
		/// then an iterator for the quasi-linguistic elements it describes is returned, e.g.,
		/// for the EMOF class "Class", an iterator for
		/// all classes found in EMOF is returned; for the EMOF class "Property", an iterator
		/// for all properties found in EMOF is returned, etc.
		/// </summary>
		/// <param name="rClassOrAdvancedAssociation">a reference to a class or an advanced association</param>
		/// <returns>an iterator for direct quasi-ontological instances (objects)
		/// of the given class or advanced association. On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllClassObjects" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectClassObjects(long rClassOrAdvancedAssociation)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectClassObjects32(tdaKernel, rClassOrAdvancedAssociation):
				TDA_GetIteratorForDirectClassObjects64(tdaKernel, rClassOrAdvancedAssociation);
		}

		/// <summary>
		/// Obtains an iterator for direct quasi-ontological instances of the given class or advanced association.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllClassObjects</code>
		/// and <code>getIteratorForDirectClassObjects</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>If the given class or advanced association is quasi-linguistic,
		/// then an iterator for the quasi-linguistic elements it describes is returned, e.g.,
		/// for the EMOF class "Class", an iterator for
		/// all classes found in EMOF is returned; for the EMOF class "Property", an iterator
		/// for all properties found in EMOF is returned, etc.
		/// </summary>
		/// <param name="rClassOrAdvancedAssociation">a reference to a class or an advanced association</param>
		/// <returns>an iterator for direct quasi-ontological instances (objects)
		/// of the given class or advanced association. On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForAllClassObjects" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectClassObjects(long rClassOrAdvancedAssociation)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectClassObjects32(tdaKernel, rClassOrAdvancedAssociation):
				TDA_GetIteratorForDirectClassObjects64(tdaKernel, rClassOrAdvancedAssociation);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FindAssociationEnd")]
		private static extern long TDA_FindAssociationEnd32(IntPtr tdaKernel, long rSourceClass, String targetRoleName);
		[DllImport("tdakernel64", EntryPoint="TDA_FindAssociationEnd")]
		private static extern long TDA_FindAssociationEnd64(IntPtr tdaKernel, long rSourceClass, String targetRoleName);
		/// <summary>
		/// Obtains a reference to an association end (by its role name) starting at the given class.
		/// 
		/// <BR/><I><B>Note (adapters): </B></I>If not implemented in the adapter,
		///   TDA kernel will implement it by means of <code>getIteratorForAllOutgoingAssociationEnds</code>.
		/// <BR/><I><B>Note (M3): </B></I>The function works also, when searching for association ends at Level M3.
		/// </summary>
		/// <param name="rSourceClass">a class that is a source class for the association, or one of its subclasses</param>
		/// <param name="targetRoleName">a role name associated with the target association end</param>
		/// <returns>a reference to an association end corresponding to the given target role name.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long findAssociationEnd(long rSourceClass, String targetRoleName)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _targetRoleName = (targetRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(targetRoleName));
			return is32?TDA_FindAssociationEnd32(tdaKernel, rSourceClass, _targetRoleName):
				TDA_FindAssociationEnd64(tdaKernel, rSourceClass, _targetRoleName);
		}

		/// <summary>
		/// Obtains a reference to an association end (by its role name) starting at the given class.
		/// 
		/// <BR/><I><B>Note (adapters): </B></I>If not implemented in the adapter,
		///   TDA kernel will implement it by means of <code>getIteratorForAllOutgoingAssociationEnds</code>.
		/// <BR/><I><B>Note (M3): </B></I>The function works also, when searching for association ends at Level M3.
		/// </summary>
		/// <param name="rSourceClass">a class that is a source class for the association, or one of its subclasses</param>
		/// <param name="targetRoleName">a role name associated with the target association end</param>
		/// <returns>a reference to an association end corresponding to the given target role name.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long FindAssociationEnd(long rSourceClass, String targetRoleName)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _targetRoleName = (targetRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(targetRoleName));
			return is32?TDA_FindAssociationEnd32(tdaKernel, rSourceClass, _targetRoleName):
				TDA_FindAssociationEnd64(tdaKernel, rSourceClass, _targetRoleName);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_ExcludeObjectFromClass")]
		private static extern byte TDA_ExcludeObjectFromClass32(IntPtr tdaKernel, long rObject, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_ExcludeObjectFromClass")]
		private static extern byte TDA_ExcludeObjectFromClass64(IntPtr tdaKernel, long rObject, long rClass);
		/// <summary>
		/// Takes out the given object from the given (quasi-ontological) class.
		/// <BR/>
		/// The function works, if the underlying repository supports
		/// multiple classification and dynamic reclassification.
		/// If the object currently is only in one class, then
		/// the operation fails (it is assumed that each object must be at
		/// least in one class).
		/// <BR/><I><B>Note (M3): </B></I>It is assumed that an element
		/// from a quasi-ontological level can be associated with only one
		/// quasi-linguistic type (quasi-linguistic class), thus, <code>excludeObjectInClass</code>
		/// as well as <code>includeObjectInClass</code>
		/// are meaningless in this case.
		/// </summary>
		/// <param name="rObject">a reference to the object to be excluded from the given class</param>
		/// <param name="rClass">a reference to the class, which to exclude from the
		///        classifiers of the given object</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool excludeObjectFromClass(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_ExcludeObjectFromClass32(tdaKernel, rObject, rClass)!=0:
				TDA_ExcludeObjectFromClass64(tdaKernel, rObject, rClass)!=0;
		}

		/// <summary>
		/// Takes out the given object from the given (quasi-ontological) class.
		/// <BR/>
		/// The function works, if the underlying repository supports
		/// multiple classification and dynamic reclassification.
		/// If the object currently is only in one class, then
		/// the operation fails (it is assumed that each object must be at
		/// least in one class).
		/// <BR/><I><B>Note (M3): </B></I>It is assumed that an element
		/// from a quasi-ontological level can be associated with only one
		/// quasi-linguistic type (quasi-linguistic class), thus, <code>excludeObjectInClass</code>
		/// as well as <code>includeObjectInClass</code>
		/// are meaningless in this case.
		/// </summary>
		/// <param name="rObject">a reference to the object to be excluded from the given class</param>
		/// <param name="rClass">a reference to the class, which to exclude from the
		///        classifiers of the given object</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool ExcludeObjectFromClass(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_ExcludeObjectFromClass32(tdaKernel, rObject, rClass)!=0:
				TDA_ExcludeObjectFromClass64(tdaKernel, rObject, rClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectSubClasses")]
		private static extern long TDA_GetIteratorForDirectSubClasses32(IntPtr tdaKernel, long rSuperClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectSubClasses")]
		private static extern long TDA_GetIteratorForDirectSubClasses64(IntPtr tdaKernel, long rSuperClass);
		/// <summary>
		/// Obtains an iterator for all direct subclasses of the given superclass.
		/// <BR/><I><B>Note (M3): </B></I>If the given superclass is a quasi-linguistic class,
		/// then an iterator for direct quasi-linguistic subclasses is returned.
		/// </summary>
		/// <param name="rSuperClass">a superclass for which to obtain direct subclasses</param>
		/// <returns>an iterator for all direct subclasses of the given superclass, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectSubClasses(long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectSubClasses32(tdaKernel, rSuperClass):
				TDA_GetIteratorForDirectSubClasses64(tdaKernel, rSuperClass);
		}

		/// <summary>
		/// Obtains an iterator for all direct subclasses of the given superclass.
		/// <BR/><I><B>Note (M3): </B></I>If the given superclass is a quasi-linguistic class,
		/// then an iterator for direct quasi-linguistic subclasses is returned.
		/// </summary>
		/// <param name="rSuperClass">a superclass for which to obtain direct subclasses</param>
		/// <returns>an iterator for all direct subclasses of the given superclass, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectSubClasses(long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectSubClasses32(tdaKernel, rSuperClass):
				TDA_GetIteratorForDirectSubClasses64(tdaKernel, rSuperClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateGeneralization")]
		private static extern byte TDA_CreateGeneralization32(IntPtr tdaKernel, long rSubClass, long rSuperClass);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateGeneralization")]
		private static extern byte TDA_CreateGeneralization64(IntPtr tdaKernel, long rSubClass, long rSuperClass);
		/// <summary>
		/// Creates a generalization between the two given classes.
		/// <BR/>
		/// The given subclass can be a derived class of the given superclass, but
		/// the direct generalization between them must not exist.
		/// <BR/>
		/// The generalization relation being created must not introduce inheritance loops.
		/// </summary>
		/// <param name="rSubClass">a class that becomes a subclass</param>
		/// <param name="rSuperClass">a class that becomes a superclass</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool createGeneralization(long rSubClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CreateGeneralization32(tdaKernel, rSubClass, rSuperClass)!=0:
				TDA_CreateGeneralization64(tdaKernel, rSubClass, rSuperClass)!=0;
		}

		/// <summary>
		/// Creates a generalization between the two given classes.
		/// <BR/>
		/// The given subclass can be a derived class of the given superclass, but
		/// the direct generalization between them must not exist.
		/// <BR/>
		/// The generalization relation being created must not introduce inheritance loops.
		/// </summary>
		/// <param name="rSubClass">a class that becomes a subclass</param>
		/// <param name="rSuperClass">a class that becomes a superclass</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool CreateGeneralization(long rSubClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CreateGeneralization32(tdaKernel, rSubClass, rSuperClass)!=0:
				TDA_CreateGeneralization64(tdaKernel, rSubClass, rSuperClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IncludeObjectInClass")]
		private static extern byte TDA_IncludeObjectInClass32(IntPtr tdaKernel, long rObject, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_IncludeObjectInClass")]
		private static extern byte TDA_IncludeObjectInClass64(IntPtr tdaKernel, long rObject, long rClass);
		/// <summary>
		/// Adds the given object to the given (quasi-ontological) class.
		/// The function works, if the underlying repository supports
		/// multiple classification and dynamic reclassification.
		/// <BR/><I><B>Note (M3): </B></I>It is assumed that an element
		/// from a quasi-ontological level can be associated with only one
		/// quasi-linguistic type (quasi-linguistic class), thus, <code>includeObjectInClass</code>
		/// is meaningless in this case.
		/// </summary>
		/// <param name="rObject">a reference to the object to be included in the given class</param>
		/// <param name="rClass">a reference to the class, where to put the object (in addition to
		///        classes, where the object already belongs)</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool includeObjectInClass(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IncludeObjectInClass32(tdaKernel, rObject, rClass)!=0:
				TDA_IncludeObjectInClass64(tdaKernel, rObject, rClass)!=0;
		}

		/// <summary>
		/// Adds the given object to the given (quasi-ontological) class.
		/// The function works, if the underlying repository supports
		/// multiple classification and dynamic reclassification.
		/// <BR/><I><B>Note (M3): </B></I>It is assumed that an element
		/// from a quasi-ontological level can be associated with only one
		/// quasi-linguistic type (quasi-linguistic class), thus, <code>includeObjectInClass</code>
		/// is meaningless in this case.
		/// </summary>
		/// <param name="rObject">a reference to the object to be included in the given class</param>
		/// <param name="rClass">a reference to the class, where to put the object (in addition to
		///        classes, where the object already belongs)</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IncludeObjectInClass(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IncludeObjectInClass32(tdaKernel, rObject, rClass)!=0:
				TDA_IncludeObjectInClass64(tdaKernel, rObject, rClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetAttributeDomain")]
		private static extern long TDA_GetAttributeDomain32(IntPtr tdaKernel, long rAttribute);
		[DllImport("tdakernel64", EntryPoint="TDA_GetAttributeDomain")]
		private static extern long TDA_GetAttributeDomain64(IntPtr tdaKernel, long rAttribute);
		/// <summary>
		/// Obtains a class, for which the given attribute was defined.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute in question</param>
		/// <returns>a reference to a class, for which the given attribute belongs, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getAttributeDomain(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetAttributeDomain32(tdaKernel, rAttribute):
				TDA_GetAttributeDomain64(tdaKernel, rAttribute);
		}

		/// <summary>
		/// Obtains a class, for which the given attribute was defined.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute in question</param>
		/// <returns>a reference to a class, for which the given attribute belongs, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetAttributeDomain(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetAttributeDomain32(tdaKernel, rAttribute):
				TDA_GetAttributeDomain64(tdaKernel, rAttribute);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateAssociation")]
		private static extern long TDA_CreateAssociation32(IntPtr tdaKernel, long rSourceClass, long rTargetClass, String sourceRoleName, String targetRoleName, byte isComposition);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateAssociation")]
		private static extern long TDA_CreateAssociation64(IntPtr tdaKernel, long rSourceClass, long rTargetClass, String sourceRoleName, String targetRoleName, byte isComposition);
		/// <summary>
		/// Creates a bidirectional association (or two directed associations, where each is an inverse of the other).
		/// The default value for the source and target cardinalities should be "*".
		/// <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
		///   if the repository supports constraints and the M3 level operations. Cardinality constraints
		///   must be accessible via M3 for that.
		/// </summary>
		/// <param name="rSourceClass">the class, where the association starts</param>
		/// <param name="rTargetClass">the class, where the association ends</param>
		/// <param name="sourceRoleName">the name of the association end near the source class</param>
		/// <param name="targetRoleName">the name of the association end near the target class</param>
		/// <param name="isComposition">whether the association is a composition, i.e.,
		///        the source class objects are containers for the target class objects</param>
		/// <returns>a reference for the target association end of the association just created, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long createAssociation(long rSourceClass, long rTargetClass, String sourceRoleName, String targetRoleName, bool isComposition)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _sourceRoleName = (sourceRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(sourceRoleName));
			String _targetRoleName = (targetRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(targetRoleName));
			byte _isComposition = Convert.ToByte(isComposition);
			return is32?TDA_CreateAssociation32(tdaKernel, rSourceClass, rTargetClass, _sourceRoleName, _targetRoleName, _isComposition):
				TDA_CreateAssociation64(tdaKernel, rSourceClass, rTargetClass, _sourceRoleName, _targetRoleName, _isComposition);
		}

		/// <summary>
		/// Creates a bidirectional association (or two directed associations, where each is an inverse of the other).
		/// The default value for the source and target cardinalities should be "*".
		/// <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
		///   if the repository supports constraints and the M3 level operations. Cardinality constraints
		///   must be accessible via M3 for that.
		/// </summary>
		/// <param name="rSourceClass">the class, where the association starts</param>
		/// <param name="rTargetClass">the class, where the association ends</param>
		/// <param name="sourceRoleName">the name of the association end near the source class</param>
		/// <param name="targetRoleName">the name of the association end near the target class</param>
		/// <param name="isComposition">whether the association is a composition, i.e.,
		///        the source class objects are containers for the target class objects</param>
		/// <returns>a reference for the target association end of the association just created, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long CreateAssociation(long rSourceClass, long rTargetClass, String sourceRoleName, String targetRoleName, bool isComposition)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _sourceRoleName = (sourceRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(sourceRoleName));
			String _targetRoleName = (targetRoleName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(targetRoleName));
			byte _isComposition = Convert.ToByte(isComposition);
			return is32?TDA_CreateAssociation32(tdaKernel, rSourceClass, rTargetClass, _sourceRoleName, _targetRoleName, _isComposition):
				TDA_CreateAssociation64(tdaKernel, rSourceClass, rTargetClass, _sourceRoleName, _targetRoleName, _isComposition);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForClasses")]
		private static extern long TDA_GetIteratorForClasses32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForClasses")]
		private static extern long TDA_GetIteratorForClasses64(IntPtr tdaKernel);
		/// <summary>
		/// Obtains an iterator for all classes (all quasi-ontological classes at all quasi-ontological meta-levels).
		/// <BR/><I><B>Note (M3): </B></I>Linguistic classes are not traversed by this iterator.
		/// Use <code>getIteratorForLinguisticClasses</code> instead.<BR/> 
		/// </summary>
		/// <returns>an iterator for all classes, or 0 on error.</returns>
		/// <seealso cref="TDAKernel.getIteratorForLinguisticClasses" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForClasses()
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForClasses32(tdaKernel):
				TDA_GetIteratorForClasses64(tdaKernel);
		}

		/// <summary>
		/// Obtains an iterator for all classes (all quasi-ontological classes at all quasi-ontological meta-levels).
		/// <BR/><I><B>Note (M3): </B></I>Linguistic classes are not traversed by this iterator.
		/// Use <code>getIteratorForLinguisticClasses</code> instead.<BR/> 
		/// </summary>
		/// <returns>an iterator for all classes, or 0 on error.</returns>
		/// <seealso cref="TDAKernel.getIteratorForLinguisticClasses" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForClasses()
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForClasses32(tdaKernel):
				TDA_GetIteratorForClasses64(tdaKernel);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteAttributeValue")]
		private static extern byte TDA_DeleteAttributeValue32(IntPtr tdaKernel, long rObject, long rAttribute);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteAttributeValue")]
		private static extern byte TDA_DeleteAttributeValue64(IntPtr tdaKernel, long rObject, long rAttribute);
		/// <summary>
		/// Deletes the value (all the values) of the given attribute for the given object.
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		///   In this case the object can be any element at the M_Omega level.
		/// </summary>
		/// <param name="rObject">the object, for which to get the attribute value (values)</param>
		/// <param name="rAttribute">the attribute, for which to obtain the value; this attribute must be associated
		///        either with a quasi-ontological class or the quasi-linguistic class of the given object</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool deleteAttributeValue(long rObject, long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteAttributeValue32(tdaKernel, rObject, rAttribute)!=0:
				TDA_DeleteAttributeValue64(tdaKernel, rObject, rAttribute)!=0;
		}

		/// <summary>
		/// Deletes the value (all the values) of the given attribute for the given object.
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		///   In this case the object can be any element at the M_Omega level.
		/// </summary>
		/// <param name="rObject">the object, for which to get the attribute value (values)</param>
		/// <param name="rAttribute">the attribute, for which to obtain the value; this attribute must be associated
		///        either with a quasi-ontological class or the quasi-linguistic class of the given object</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool DeleteAttributeValue(long rObject, long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteAttributeValue32(tdaKernel, rObject, rAttribute)!=0:
				TDA_DeleteAttributeValue64(tdaKernel, rObject, rAttribute)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForAllClassObjects")]
		private static extern long TDA_GetIteratorForAllClassObjects32(IntPtr tdaKernel, long rClassOrAdvancedAssociation);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForAllClassObjects")]
		private static extern long TDA_GetIteratorForAllClassObjects64(IntPtr tdaKernel, long rClassOrAdvancedAssociation);
		/// <summary>
		/// Obtains an iterator for all quasi-ontological instances of the given class or advanced association.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllClassObjects</code>
		/// and <code>getIteratorForDirectClassObjects</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>If the given class or advanced association is quasi-linguistic,
		/// then an iterator for the quasi-linguistic elements it describes is returned, e.g.,
		/// for the EMOF class "Class", an iterator for
		/// all classes found in EMOF is returned; for the EMOF class "Property", an iterator
		/// for all properties found in EMOF is returned, etc.
		/// </summary>
		/// <param name="rClassOrAdvancedAssociation">a reference to a class or an advanced association</param>
		/// <returns>an iterator for all quasi-ontological instances (objects)
		///         of the given class or advanced association. On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectClassObjects" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForAllClassObjects(long rClassOrAdvancedAssociation)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllClassObjects32(tdaKernel, rClassOrAdvancedAssociation):
				TDA_GetIteratorForAllClassObjects64(tdaKernel, rClassOrAdvancedAssociation);
		}

		/// <summary>
		/// Obtains an iterator for all quasi-ontological instances of the given class or advanced association.
		/// <BR/><I><B>Note (adapters): </B></I>A repository adapter
		/// may implement only one of the functions <code>getIteratorForAllClassObjects</code>
		/// and <code>getIteratorForDirectClassObjects</code>.
		/// The unimplemented function will be implemented via another by TDA Kernel.
		/// <BR/><I><B>Note (M3): </B></I>If the given class or advanced association is quasi-linguistic,
		/// then an iterator for the quasi-linguistic elements it describes is returned, e.g.,
		/// for the EMOF class "Class", an iterator for
		/// all classes found in EMOF is returned; for the EMOF class "Property", an iterator
		/// for all properties found in EMOF is returned, etc.
		/// </summary>
		/// <param name="rClassOrAdvancedAssociation">a reference to a class or an advanced association</param>
		/// <returns>an iterator for all quasi-ontological instances (objects)
		///         of the given class or advanced association. On error, 0 is returned.</returns>
		/// <seealso cref="TDAKernel.getIteratorForDirectClassObjects" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForAllClassObjects(long rClassOrAdvancedAssociation)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForAllClassObjects32(tdaKernel, rClassOrAdvancedAssociation):
				TDA_GetIteratorForAllClassObjects64(tdaKernel, rClassOrAdvancedAssociation);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetAttributeValue")]
		private static extern IntPtr TDA_GetAttributeValue32(IntPtr tdaKernel, long rObject, long rAttribute);
		[DllImport("tdakernel64", EntryPoint="TDA_GetAttributeValue")]
		private static extern IntPtr TDA_GetAttributeValue64(IntPtr tdaKernel, long rObject, long rAttribute);
		/// <summary>
		/// Gets the value or the ordered collection of values (encoded as a string) of the given attribute for the given object.
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		/// </summary>
		/// <param name="rObject">the object, for which to get the attribute value (values)</param>
		/// <param name="rAttribute">the attribute, for which to obtain the value; this attribute must be associated
		///        either with a quasi-ontological class or the quasi-linguistic class of the given object</param>
		/// <returns>the attribute value (values) encoded as a string
		///         (for the decimal point the dot symbol "." is used), or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public String getAttributeValue(long rObject, long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetAttributeValue32(tdaKernel, rObject, rAttribute):
				TDA_GetAttributeValue64(tdaKernel, rObject, rAttribute));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Gets the value or the ordered collection of values (encoded as a string) of the given attribute for the given object.
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		/// </summary>
		/// <param name="rObject">the object, for which to get the attribute value (values)</param>
		/// <param name="rAttribute">the attribute, for which to obtain the value; this attribute must be associated
		///        either with a quasi-ontological class or the quasi-linguistic class of the given object</param>
		/// <returns>the attribute value (values) encoded as a string
		///         (for the decimal point the dot symbol "." is used), or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public String GetAttributeValue(long rObject, long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetAttributeValue32(tdaKernel, rObject, rAttribute):
				TDA_GetAttributeValue64(tdaKernel, rObject, rAttribute));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateAdvancedAssociation")]
		private static extern long TDA_CreateAdvancedAssociation32(IntPtr tdaKernel, String name, byte nAry, byte associationClass);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateAdvancedAssociation")]
		private static extern long TDA_CreateAdvancedAssociation64(IntPtr tdaKernel, String name, byte nAry, byte associationClass);
		/// <summary>
		/// Creates an n-ary association, an association class, or an n-ary association class.
		/// <BR/>
		/// An advanced association behaves likes a class (although it might not be a class internally)
		/// with n bidirectional associations attached to it. To specify all n association ends,
		/// call <code>createAssociation</code> n times, where
		/// a reference to the n-ary association has to be passed instead of one of the class references.
		/// N-ary association links can be created by means of <code>createObject</code>,
		/// and n-ary link ends can be created by calling <code>createLink</code> n times
		/// and passing a reference to the n-ary link instead of one of the object references.
		///
		/// <BR/><I><B>Note (adapters): </B></I>The underlying repository
		///   is allowed to create an n-ary association class, even when nAry or associationClass is <code>false</code>.
		/// <BR/><I><B>Note (adapters): </B></I>If a repository adapter
		///   does not implement this function, TDA kernel will
		///   implement this function by introducing an additional class.
		/// <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
		///   if the repository supports constraints and the M3 level operations. Cardinality constraints
		///   must be accessible via M3 for that.
		/// </summary>
		/// <param name="name">the name of the advanced association (the class name in case of an association class)</param>
		/// <param name="nAry">whether the association is an n-ary association</param>
		/// <param name="associationClass">whether the association is an association class</param>
		/// <returns>a reference to the n-ary association just created (not the association end, since no association ends are created yet), or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public long createAdvancedAssociation(String name, bool nAry, bool associationClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			byte _nAry = Convert.ToByte(nAry);
			byte _associationClass = Convert.ToByte(associationClass);
			return is32?TDA_CreateAdvancedAssociation32(tdaKernel, _name, _nAry, _associationClass):
				TDA_CreateAdvancedAssociation64(tdaKernel, _name, _nAry, _associationClass);
		}

		/// <summary>
		/// Creates an n-ary association, an association class, or an n-ary association class.
		/// <BR/>
		/// An advanced association behaves likes a class (although it might not be a class internally)
		/// with n bidirectional associations attached to it. To specify all n association ends,
		/// call <code>createAssociation</code> n times, where
		/// a reference to the n-ary association has to be passed instead of one of the class references.
		/// N-ary association links can be created by means of <code>createObject</code>,
		/// and n-ary link ends can be created by calling <code>createLink</code> n times
		/// and passing a reference to the n-ary link instead of one of the object references.
		///
		/// <BR/><I><B>Note (adapters): </B></I>The underlying repository
		///   is allowed to create an n-ary association class, even when nAry or associationClass is <code>false</code>.
		/// <BR/><I><B>Note (adapters): </B></I>If a repository adapter
		///   does not implement this function, TDA kernel will
		///   implement this function by introducing an additional class.
		/// <BR/><I><B>Note (M3): </B></I> The M3 level can be used to get/set the cardinality,
		///   if the repository supports constraints and the M3 level operations. Cardinality constraints
		///   must be accessible via M3 for that.
		/// </summary>
		/// <param name="name">the name of the advanced association (the class name in case of an association class)</param>
		/// <param name="nAry">whether the association is an n-ary association</param>
		/// <param name="associationClass">whether the association is an association class</param>
		/// <returns>a reference to the n-ary association just created (not the association end, since no association ends are created yet), or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public long CreateAdvancedAssociation(String name, bool nAry, bool associationClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			byte _nAry = Convert.ToByte(nAry);
			byte _associationClass = Convert.ToByte(associationClass);
			return is32?TDA_CreateAdvancedAssociation32(tdaKernel, _name, _nAry, _associationClass):
				TDA_CreateAdvancedAssociation64(tdaKernel, _name, _nAry, _associationClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetPrimitiveDataTypeName")]
		private static extern IntPtr TDA_GetPrimitiveDataTypeName32(IntPtr tdaKernel, long rDataType);
		[DllImport("tdakernel64", EntryPoint="TDA_GetPrimitiveDataTypeName")]
		private static extern IntPtr TDA_GetPrimitiveDataTypeName64(IntPtr tdaKernel, long rDataType);
		/// <summary>
		/// Returns the name of the given primitive data type.
		/// </summary>
		/// <param name="rDataType">a reference to a primitive data type,
		///        for which the name has to be obtained</param>
		/// <returns>the name of the given primitive data type, or <code>null</code> on error.</returns>
		/// <seealso cref="TDAKernel.findPrimitiveDataType" />
		public String getPrimitiveDataTypeName(long rDataType)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetPrimitiveDataTypeName32(tdaKernel, rDataType):
				TDA_GetPrimitiveDataTypeName64(tdaKernel, rDataType));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Returns the name of the given primitive data type.
		/// </summary>
		/// <param name="rDataType">a reference to a primitive data type,
		///        for which the name has to be obtained</param>
		/// <returns>the name of the given primitive data type, or <code>null</code> on error.</returns>
		/// <seealso cref="TDAKernel.findPrimitiveDataType" />
		public String GetPrimitiveDataTypeName(long rDataType)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetPrimitiveDataTypeName32(tdaKernel, rDataType):
				TDA_GetPrimitiveDataTypeName64(tdaKernel, rDataType));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteGeneralization")]
		private static extern byte TDA_DeleteGeneralization32(IntPtr tdaKernel, long rSubClass, long rSuperClass);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteGeneralization")]
		private static extern byte TDA_DeleteGeneralization64(IntPtr tdaKernel, long rSubClass, long rSuperClass);
		/// <summary>
		/// Deletes the generalization between the given two classes.
		/// </summary>
		/// <param name="rSubClass">a class that was a subclass</param>
		/// <param name="rSuperClass">a class that was a superclass</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool deleteGeneralization(long rSubClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteGeneralization32(tdaKernel, rSubClass, rSuperClass)!=0:
				TDA_DeleteGeneralization64(tdaKernel, rSubClass, rSuperClass)!=0;
		}

		/// <summary>
		/// Deletes the generalization between the given two classes.
		/// </summary>
		/// <param name="rSubClass">a class that was a subclass</param>
		/// <param name="rSuperClass">a class that was a superclass</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool DeleteGeneralization(long rSubClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteGeneralization32(tdaKernel, rSubClass, rSuperClass)!=0:
				TDA_DeleteGeneralization64(tdaKernel, rSubClass, rSuperClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsPrimitiveDataType")]
		private static extern byte TDA_IsPrimitiveDataType32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_IsPrimitiveDataType")]
		private static extern byte TDA_IsPrimitiveDataType64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Checks whether the given reference is associated with a primitive data type.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with a primitive data type.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso cref="TDAKernel.findPrimitiveDataType" />
		/// <seealso cref="TDAKernel.getPrimitiveDataTypeName" />
		public bool isPrimitiveDataType(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsPrimitiveDataType32(tdaKernel, r)!=0:
				TDA_IsPrimitiveDataType64(tdaKernel, r)!=0;
		}

		/// <summary>
		/// Checks whether the given reference is associated with a primitive data type.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with a primitive data type.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso cref="TDAKernel.findPrimitiveDataType" />
		/// <seealso cref="TDAKernel.getPrimitiveDataTypeName" />
		public bool IsPrimitiveDataType(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsPrimitiveDataType32(tdaKernel, r)!=0:
				TDA_IsPrimitiveDataType64(tdaKernel, r)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForDirectSuperClasses")]
		private static extern long TDA_GetIteratorForDirectSuperClasses32(IntPtr tdaKernel, long rSubClass);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForDirectSuperClasses")]
		private static extern long TDA_GetIteratorForDirectSuperClasses64(IntPtr tdaKernel, long rSubClass);
		/// <summary>
		/// Obtains an iterator for all direct superclasses of the given subclass.
		/// <BR/><I><B>Note (M3): </B></I>If the given subclass is a quasi-linguistic class,
		/// then an iterator for its direct quasi-linguistic superclasses is returned.
		/// </summary>
		/// <param name="rSubClass">a subclass for which to obtain direct superclasses</param>
		/// <returns>an iterator for all direct superclasses of the given subclass, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForDirectSuperClasses(long rSubClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectSuperClasses32(tdaKernel, rSubClass):
				TDA_GetIteratorForDirectSuperClasses64(tdaKernel, rSubClass);
		}

		/// <summary>
		/// Obtains an iterator for all direct superclasses of the given subclass.
		/// <BR/><I><B>Note (M3): </B></I>If the given subclass is a quasi-linguistic class,
		/// then an iterator for its direct quasi-linguistic superclasses is returned.
		/// </summary>
		/// <param name="rSubClass">a subclass for which to obtain direct superclasses</param>
		/// <returns>an iterator for all direct superclasses of the given subclass, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForDirectSuperClasses(long rSubClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForDirectSuperClasses32(tdaKernel, rSubClass):
				TDA_GetIteratorForDirectSuperClasses64(tdaKernel, rSubClass);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteAssociation")]
		private static extern byte TDA_DeleteAssociation32(IntPtr tdaKernel, long rAssociationEndOrAdvancedAssociation);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteAssociation")]
		private static extern byte TDA_DeleteAssociation64(IntPtr tdaKernel, long rAssociationEndOrAdvancedAssociation);
		/// <summary>
		/// Deletes the given association.
		/// Directed and bidirectional associations are specified by (one of) their ends.
		/// Advanced associations have their own references.
		/// If the association is bidirectional, the inverse association end is deleted as well.
		/// For advanced associations, all association parts are deleted.
		/// </summary>
		/// <param name="rAssociationEndOrAdvancedAssociation">a reference to an association end (if
		///        the association is directed or bidirectional) or a reference to
		///        an advanced association</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public bool deleteAssociation(long rAssociationEndOrAdvancedAssociation)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteAssociation32(tdaKernel, rAssociationEndOrAdvancedAssociation)!=0:
				TDA_DeleteAssociation64(tdaKernel, rAssociationEndOrAdvancedAssociation)!=0;
		}

		/// <summary>
		/// Deletes the given association.
		/// Directed and bidirectional associations are specified by (one of) their ends.
		/// Advanced associations have their own references.
		/// If the association is bidirectional, the inverse association end is deleted as well.
		/// For advanced associations, all association parts are deleted.
		/// </summary>
		/// <param name="rAssociationEndOrAdvancedAssociation">a reference to an association end (if
		///        the association is directed or bidirectional) or a reference to
		///        an advanced association</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public bool DeleteAssociation(long rAssociationEndOrAdvancedAssociation)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteAssociation32(tdaKernel, rAssociationEndOrAdvancedAssociation)!=0:
				TDA_DeleteAssociation64(tdaKernel, rAssociationEndOrAdvancedAssociation)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetInverseAssociationEnd")]
		private static extern long TDA_GetInverseAssociationEnd32(IntPtr tdaKernel, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_GetInverseAssociationEnd")]
		private static extern long TDA_GetInverseAssociationEnd64(IntPtr tdaKernel, long rAssociationEnd);
		/// <summary>
		/// Obtains a reference to the inverse association end of the given association end (if association is bidirectional or a bidirectional part of an advanced association).
		/// <BR/><I><B>Note (M3): </B></I>The function works also for association ends at Level M3.
		/// </summary>
		/// <param name="rAssociationEnd">a reference to a known association end, for which the inverse end has to be obtained</param>
		/// <returns>a reference to the inverse association end. On error or if the association end
		///   does not have the inverse, 0 is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getInverseAssociationEnd(long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetInverseAssociationEnd32(tdaKernel, rAssociationEnd):
				TDA_GetInverseAssociationEnd64(tdaKernel, rAssociationEnd);
		}

		/// <summary>
		/// Obtains a reference to the inverse association end of the given association end (if association is bidirectional or a bidirectional part of an advanced association).
		/// <BR/><I><B>Note (M3): </B></I>The function works also for association ends at Level M3.
		/// </summary>
		/// <param name="rAssociationEnd">a reference to a known association end, for which the inverse end has to be obtained</param>
		/// <returns>a reference to the inverse association end. On error or if the association end
		///   does not have the inverse, 0 is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetInverseAssociationEnd(long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetInverseAssociationEnd32(tdaKernel, rAssociationEnd):
				TDA_GetInverseAssociationEnd64(tdaKernel, rAssociationEnd);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_SetAttributeValue")]
		private static extern byte TDA_SetAttributeValue32(IntPtr tdaKernel, long rObject, long rAttribute, String value);
		[DllImport("tdakernel64", EntryPoint="TDA_SetAttributeValue")]
		private static extern byte TDA_SetAttributeValue64(IntPtr tdaKernel, long rObject, long rAttribute, String value);
		/// <summary>
		/// Sets the value or the ordered collection of values (encoded as a string) of the given attribute for the given object.
		/// <BR/><I><B>Note (adapters): </B></I>Repository adapters
		///   may assume that the value is not <code>null</code> and not a string encoding <code>null</code>,
		///   since for those cases TDA Kernel forwards the call to <code>deleteAttributeValue</code>.
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		///   In this case the object can be any element at the M_Omega level.
		/// </summary>
		/// <param name="rObject">the object, for which to set the attribute value (values)</param>
		/// <param name="rAttribute">the attribute, for which to set the value; this attribute must be associated
		///        either with a quasi-ontological class or the quasi-linguistic class of the given object</param>
		/// <param name="value">the attribute value (values) encoded as a string (use "." for the decimal point)</param>
		/// <returns>whether the value(s) has (have) been set.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool setAttributeValue(long rObject, long rAttribute, String value)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _value = (value==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(value));
			return is32?TDA_SetAttributeValue32(tdaKernel, rObject, rAttribute, _value)!=0:
				TDA_SetAttributeValue64(tdaKernel, rObject, rAttribute, _value)!=0;
		}

		/// <summary>
		/// Sets the value or the ordered collection of values (encoded as a string) of the given attribute for the given object.
		/// <BR/><I><B>Note (adapters): </B></I>Repository adapters
		///   may assume that the value is not <code>null</code> and not a string encoding <code>null</code>,
		///   since for those cases TDA Kernel forwards the call to <code>deleteAttributeValue</code>.
		/// <BR/><I><B>Note (M3): </B></I>The attribute reference can be a reference at the M3 level.
		///   In this case the object can be any element at the M_Omega level.
		/// </summary>
		/// <param name="rObject">the object, for which to set the attribute value (values)</param>
		/// <param name="rAttribute">the attribute, for which to set the value; this attribute must be associated
		///        either with a quasi-ontological class or the quasi-linguistic class of the given object</param>
		/// <param name="value">the attribute value (values) encoded as a string (use "." for the decimal point)</param>
		/// <returns>whether the value(s) has (have) been set.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool SetAttributeValue(long rObject, long rAttribute, String value)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			String _value = (value==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(value));
			return is32?TDA_SetAttributeValue32(tdaKernel, rObject, rAttribute, _value)!=0:
				TDA_SetAttributeValue64(tdaKernel, rObject, rAttribute, _value)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateClass")]
		private static extern long TDA_CreateClass32(IntPtr tdaKernel, String name);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateClass")]
		private static extern long TDA_CreateClass64(IntPtr tdaKernel, String name);
		/// <summary>
		/// Creates a class with the given fully qualified name.
		/// </summary>
		/// <param name="name">the fully qualified name of the class
		///        (packages are delimited by double colon "::");
		///        this fully qualified name must be unique</param>
		/// <returns>a reference to the class just created, or 0 on error.</returns>
		public long createClass(String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_CreateClass32(tdaKernel, _name):
				TDA_CreateClass64(tdaKernel, _name);
		}

		/// <summary>
		/// Creates a class with the given fully qualified name.
		/// </summary>
		/// <param name="name">the fully qualified name of the class
		///        (packages are delimited by double colon "::");
		///        this fully qualified name must be unique</param>
		/// <returns>a reference to the class just created, or 0 on error.</returns>
		public long CreateClass(String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_CreateClass32(tdaKernel, _name):
				TDA_CreateClass64(tdaKernel, _name);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteLink")]
		private static extern byte TDA_DeleteLink32(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteLink")]
		private static extern byte TDA_DeleteLink64(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		/// <summary>
		/// Deletes a link of the given type (specified by <code>rTargetAssociationEnd</code>) between the given two objects.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool deleteLink(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteLink32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0:
				TDA_DeleteLink64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0;
		}

		/// <summary>
		/// Deletes a link of the given type (specified by <code>rTargetAssociationEnd</code>) between the given two objects.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool DeleteLink(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteLink32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0:
				TDA_DeleteLink64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_LinkExists")]
		private static extern byte TDA_LinkExists32(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_LinkExists")]
		private static extern byte TDA_LinkExists64(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		/// <summary>
		/// Checks whether the link of the given type (specified by <code>rTargetAssociationEnd</code>) between the given two objects exists.
		/// <BR/><I><B>Note (adapters): </B></I>If not implemented in a repository adapter,
		///   TDA Kernel will implement this function through <code>getIteratorForLinkedObjects</code>.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <returns>whether the link exists. On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool linkExists(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_LinkExists32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0:
				TDA_LinkExists64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0;
		}

		/// <summary>
		/// Checks whether the link of the given type (specified by <code>rTargetAssociationEnd</code>) between the given two objects exists.
		/// <BR/><I><B>Note (adapters): </B></I>If not implemented in a repository adapter,
		///   TDA Kernel will implement this function through <code>getIteratorForLinkedObjects</code>.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <returns>whether the link exists. On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool LinkExists(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_LinkExists32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0:
				TDA_LinkExists64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteAttribute")]
		private static extern byte TDA_DeleteAttribute32(IntPtr tdaKernel, long rAttribute);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteAttribute")]
		private static extern byte TDA_DeleteAttribute64(IntPtr tdaKernel, long rAttribute);
		/// <summary>
		/// Deletes the given attribute.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute to be deleted</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool deleteAttribute(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteAttribute32(tdaKernel, rAttribute)!=0:
				TDA_DeleteAttribute64(tdaKernel, rAttribute)!=0;
		}

		/// <summary>
		/// Deletes the given attribute.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute to be deleted</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool DeleteAttribute(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteAttribute32(tdaKernel, rAttribute)!=0:
				TDA_DeleteAttribute64(tdaKernel, rAttribute)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateAttribute")]
		private static extern long TDA_CreateAttribute32(IntPtr tdaKernel, long rClass, String name, long rPrimitiveType);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateAttribute")]
		private static extern long TDA_CreateAttribute64(IntPtr tdaKernel, long rClass, String name, long rPrimitiveType);
		/// <summary>
		/// Creates (defines) a new attribute for the given class.
		/// The default cardinality is the widest cardinality supported by the repository
		/// (e.g., "0..*", if multi-valued attributes are supported; or "0..1", otherwise).
		/// The cardinality can be looked up and changed by using the quasi-linguistic meta-metalevel.
		/// </summary>
		/// <param name="rClass">a reference to an existing class, for which to define the attribute</param>
		/// <param name="name">the name of the attribute being created; it must be unique
		///             within all the attributes defined for this class, including
		///             derived ones</param>
		/// <param name="rPrimitiveType">a reference to a primitive data type for attribute values</param>
		/// <returns>a reference to the attribute just created, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long createAttribute(long rClass, String name, long rPrimitiveType)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_CreateAttribute32(tdaKernel, rClass, _name, rPrimitiveType):
				TDA_CreateAttribute64(tdaKernel, rClass, _name, rPrimitiveType);
		}

		/// <summary>
		/// Creates (defines) a new attribute for the given class.
		/// The default cardinality is the widest cardinality supported by the repository
		/// (e.g., "0..*", if multi-valued attributes are supported; or "0..1", otherwise).
		/// The cardinality can be looked up and changed by using the quasi-linguistic meta-metalevel.
		/// </summary>
		/// <param name="rClass">a reference to an existing class, for which to define the attribute</param>
		/// <param name="name">the name of the attribute being created; it must be unique
		///             within all the attributes defined for this class, including
		///             derived ones</param>
		/// <param name="rPrimitiveType">a reference to a primitive data type for attribute values</param>
		/// <returns>a reference to the attribute just created, or 0 on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long CreateAttribute(long rClass, String name, long rPrimitiveType)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_CreateAttribute32(tdaKernel, rClass, _name, rPrimitiveType):
				TDA_CreateAttribute64(tdaKernel, rClass, _name, rPrimitiveType);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteObject")]
		private static extern byte TDA_DeleteObject32(IntPtr tdaKernel, long rObject);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteObject")]
		private static extern byte TDA_DeleteObject64(IntPtr tdaKernel, long rObject);
		/// <summary>
		/// Creates the given object.
		/// </summary>
		/// <param name="rObject">a reference to the object to be deleted</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool deleteObject(long rObject)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteObject32(tdaKernel, rObject)!=0:
				TDA_DeleteObject64(tdaKernel, rObject)!=0;
		}

		/// <summary>
		/// Creates the given object.
		/// </summary>
		/// <param name="rObject">a reference to the object to be deleted</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool DeleteObject(long rObject)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteObject32(tdaKernel, rObject)!=0:
				TDA_DeleteObject64(tdaKernel, rObject)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FreeIterator")]
		private static extern void TDA_FreeIterator32(IntPtr tdaKernel, long it);
		[DllImport("tdakernel64", EntryPoint="TDA_FreeIterator")]
		private static extern void TDA_FreeIterator64(IntPtr tdaKernel, long it);
		/// <summary>
		/// Frees the memory associated with the given iterator reference.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public void freeIterator(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_FreeIterator32(tdaKernel, it); else
				TDA_FreeIterator64(tdaKernel, it);
		}

		/// <summary>
		/// Frees the memory associated with the given iterator reference.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public void FreeIterator(long it)
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_FreeIterator32(tdaKernel, it); else
				TDA_FreeIterator64(tdaKernel, it);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsAssociationEnd")]
		private static extern byte TDA_IsAssociationEnd32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_IsAssociationEnd")]
		private static extern byte TDA_IsAssociationEnd64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Checks, whether the given reference corresponds to an association end.
		/// <BR/><I><B>Note (adapters): </B></I>If not implemented in a repository adapter,
		///   TDA Kernel will implement it by means of <code>getSourceClass</code>, <code>getRoleName</code> and <code>findAssociationEnd</code>.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference corresponds to an association end.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isAssociationEnd(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsAssociationEnd32(tdaKernel, r)!=0:
				TDA_IsAssociationEnd64(tdaKernel, r)!=0;
		}

		/// <summary>
		/// Checks, whether the given reference corresponds to an association end.
		/// <BR/><I><B>Note (adapters): </B></I>If not implemented in a repository adapter,
		///   TDA Kernel will implement it by means of <code>getSourceClass</code>, <code>getRoleName</code> and <code>findAssociationEnd</code>.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference corresponds to an association end.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsAssociationEnd(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsAssociationEnd32(tdaKernel, r)!=0:
				TDA_IsAssociationEnd64(tdaKernel, r)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetSourceClass")]
		private static extern long TDA_GetSourceClass32(IntPtr tdaKernel, long rTargetAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_GetSourceClass")]
		private static extern long TDA_GetSourceClass64(IntPtr tdaKernel, long rTargetAssociationEnd);
		/// <summary>
		/// Obtains a reference to the source class of the given directed or bidirectional association (or part of an advanced association) specified by its target end.
		/// Any of the association ends can be considered a target end, when calling this function.
		/// </summary>
		/// <param name="rTargetAssociationEnd">an association end of some association;
		///   this association end will be considered a target end</param>
		/// <returns>a reference to the source class of the given association specified by its target end.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public long getSourceClass(long rTargetAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetSourceClass32(tdaKernel, rTargetAssociationEnd):
				TDA_GetSourceClass64(tdaKernel, rTargetAssociationEnd);
		}

		/// <summary>
		/// Obtains a reference to the source class of the given directed or bidirectional association (or part of an advanced association) specified by its target end.
		/// Any of the association ends can be considered a target end, when calling this function.
		/// </summary>
		/// <param name="rTargetAssociationEnd">an association end of some association;
		///   this association end will be considered a target end</param>
		/// <returns>a reference to the source class of the given association specified by its target end.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public long GetSourceClass(long rTargetAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetSourceClass32(tdaKernel, rTargetAssociationEnd):
				TDA_GetSourceClass64(tdaKernel, rTargetAssociationEnd);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsKindOf")]
		private static extern byte TDA_IsKindOf32(IntPtr tdaKernel, long rObject, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_IsKindOf")]
		private static extern byte TDA_IsKindOf64(IntPtr tdaKernel, long rObject, long rClass);
		/// <summary>
		/// Checks whether the given object is a direct or indirect, quasi-ontological or quasi-linguistic, instance of the given class.
		/// <BR/><I><B>Note (M3): </B></I>The function works also when
		/// one or both of <code>rObject</code> and <code>rClass</code> is/are quasi-linguistic.
		/// If the object is at a quasi-ontological meta-level, but the class is quasi-linguistic,
		/// then the function checks whether the object is a quasi-linguistic instance of the given class
		/// or one of its subclasses.
		/// </summary>
		/// <param name="rObject">a reference to an object</param>
		/// <param name="rClass">a reference to a class</param>
		/// <returns>whether the given object is a (direct or indirect) instance of the given class.
		/// On error, <code>false</code> is returned.</returns>
		/// <seealso cref="TDAKernel.isTypeOf" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isKindOf(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsKindOf32(tdaKernel, rObject, rClass)!=0:
				TDA_IsKindOf64(tdaKernel, rObject, rClass)!=0;
		}

		/// <summary>
		/// Checks whether the given object is a direct or indirect, quasi-ontological or quasi-linguistic, instance of the given class.
		/// <BR/><I><B>Note (M3): </B></I>The function works also when
		/// one or both of <code>rObject</code> and <code>rClass</code> is/are quasi-linguistic.
		/// If the object is at a quasi-ontological meta-level, but the class is quasi-linguistic,
		/// then the function checks whether the object is a quasi-linguistic instance of the given class
		/// or one of its subclasses.
		/// </summary>
		/// <param name="rObject">a reference to an object</param>
		/// <param name="rClass">a reference to a class</param>
		/// <returns>whether the given object is a (direct or indirect) instance of the given class.
		/// On error, <code>false</code> is returned.</returns>
		/// <seealso cref="TDAKernel.isTypeOf" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsKindOf(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsKindOf32(tdaKernel, rObject, rClass)!=0:
				TDA_IsKindOf64(tdaKernel, rObject, rClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsLinguistic")]
		private static extern byte TDA_IsLinguistic32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_IsLinguistic")]
		private static extern byte TDA_IsLinguistic64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Checks, whether the given reference is associated with a Level M3 element.
		/// Can be used together with <code>isClass</code>, <code>isAssociationEnd</code>, etc. to get more details about the element.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with a Level M3 element.
		///   On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isLinguistic(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsLinguistic32(tdaKernel, r)!=0:
				TDA_IsLinguistic64(tdaKernel, r)!=0;
		}

		/// <summary>
		/// Checks, whether the given reference is associated with a Level M3 element.
		/// Can be used together with <code>isClass</code>, <code>isAssociationEnd</code>, etc. to get more details about the element.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with a Level M3 element.
		///   On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsLinguistic(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsLinguistic32(tdaKernel, r)!=0:
				TDA_IsLinguistic64(tdaKernel, r)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetAttributeType")]
		private static extern long TDA_GetAttributeType32(IntPtr tdaKernel, long rAttribute);
		[DllImport("tdakernel64", EntryPoint="TDA_GetAttributeType")]
		private static extern long TDA_GetAttributeType64(IntPtr tdaKernel, long rAttribute);
		/// <summary>
		/// Returns the (primitive) type for values of the given attribute.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute in question</param>
		/// <returns>a reference to a primitive data type for values of the given attribute.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getAttributeType(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetAttributeType32(tdaKernel, rAttribute):
				TDA_GetAttributeType64(tdaKernel, rAttribute);
		}

		/// <summary>
		/// Returns the (primitive) type for values of the given attribute.
		/// <BR/><I><B>Note (M3): </B></I>The function works also for attributes of quasi-linguistic classes.
		/// </summary>
		/// <param name="rAttribute">a reference to the attribute in question</param>
		/// <returns>a reference to a primitive data type for values of the given attribute.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetAttributeType(long rAttribute)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetAttributeType32(tdaKernel, rAttribute):
				TDA_GetAttributeType64(tdaKernel, rAttribute);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_MoveObject")]
		private static extern byte TDA_MoveObject32(IntPtr tdaKernel, long rObject, long rToClass);
		[DllImport("tdakernel64", EntryPoint="TDA_MoveObject")]
		private static extern byte TDA_MoveObject64(IntPtr tdaKernel, long rObject, long rToClass);
		/// <summary>
		/// Moves (reclassifies) the given object into the given (quasi-ontological) class, removing it from its current class (classes).
		/// <BR/>
		/// The function is similar to calling
		/// <BR/><code>includeObjectInClass(rObject, rToClass);</code>
		/// followed by calling
		/// <BR/><code>excludeObjectInClass(rObject, <i>c</i>)</code>
		/// for all other current classifiers <i>c</i> of the given object.
		/// <BR/>
		/// The distinction is that it may be possible to implement
		/// this function even when multiple classification is not supported.   
		/// <BR/><I><B>Note (adapters): </B></I>This function is optional for
		/// repository adapters. If not implemented in an adapter,
		/// TDA Kernel implements it by
		/// [TODO] recreating the object (with the new type), while
		/// also recreating all attributes and links.
		/// <BR/><I><B>Note (M3): </B></I>It is assumed that an element
		/// from a quasi-ontological level cannot dynamically change its
		/// quasi-linguistic type (quasi-linguistic class), thus, <code>moveObject</code>
		/// is meaningless in this case.
		/// </summary>
		/// <param name="rObject">a reference to the object to be reclassified</param>
		/// <param name="rToClass">a reference to the class, to which the object will belong</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool moveObject(long rObject, long rToClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_MoveObject32(tdaKernel, rObject, rToClass)!=0:
				TDA_MoveObject64(tdaKernel, rObject, rToClass)!=0;
		}

		/// <summary>
		/// Moves (reclassifies) the given object into the given (quasi-ontological) class, removing it from its current class (classes).
		/// <BR/>
		/// The function is similar to calling
		/// <BR/><code>includeObjectInClass(rObject, rToClass);</code>
		/// followed by calling
		/// <BR/><code>excludeObjectInClass(rObject, <i>c</i>)</code>
		/// for all other current classifiers <i>c</i> of the given object.
		/// <BR/>
		/// The distinction is that it may be possible to implement
		/// this function even when multiple classification is not supported.   
		/// <BR/><I><B>Note (adapters): </B></I>This function is optional for
		/// repository adapters. If not implemented in an adapter,
		/// TDA Kernel implements it by
		/// [TODO] recreating the object (with the new type), while
		/// also recreating all attributes and links.
		/// <BR/><I><B>Note (M3): </B></I>It is assumed that an element
		/// from a quasi-ontological level cannot dynamically change its
		/// quasi-linguistic type (quasi-linguistic class), thus, <code>moveObject</code>
		/// is meaningless in this case.
		/// </summary>
		/// <param name="rObject">a reference to the object to be reclassified</param>
		/// <param name="rToClass">a reference to the class, to which the object will belong</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool MoveObject(long rObject, long rToClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_MoveObject32(tdaKernel, rObject, rToClass)!=0:
				TDA_MoveObject64(tdaKernel, rObject, rToClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsTypeOf")]
		private static extern byte TDA_IsTypeOf32(IntPtr tdaKernel, long rObject, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_IsTypeOf")]
		private static extern byte TDA_IsTypeOf64(IntPtr tdaKernel, long rObject, long rClass);
		/// <summary>
		/// Checks whether the given object is a direct (quasi-ontological or quasi-linguistic) instance of the given class.
		/// <BR/><I><B>Note (M3): </B></I>The function works also when
		/// one or both of <code>rObject</code> and <code>rClass</code> is/are quasi-linguistic.
		/// If the object is at a quasi-ontological meta-level, but the class is quasi-linguistic,
		/// then the function checks whether the object is a direct quasi-linguistic instance of the given class.
		/// </summary>
		/// <param name="rObject">a reference to an object</param>
		/// <param name="rClass">a reference to a class</param>
		/// <returns>whether the given object is a direct instance of the given class.
		/// On error, <code>false</code> is returned.</returns>
		/// <seealso cref="TDAKernel.isKindOf" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isTypeOf(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsTypeOf32(tdaKernel, rObject, rClass)!=0:
				TDA_IsTypeOf64(tdaKernel, rObject, rClass)!=0;
		}

		/// <summary>
		/// Checks whether the given object is a direct (quasi-ontological or quasi-linguistic) instance of the given class.
		/// <BR/><I><B>Note (M3): </B></I>The function works also when
		/// one or both of <code>rObject</code> and <code>rClass</code> is/are quasi-linguistic.
		/// If the object is at a quasi-ontological meta-level, but the class is quasi-linguistic,
		/// then the function checks whether the object is a direct quasi-linguistic instance of the given class.
		/// </summary>
		/// <param name="rObject">a reference to an object</param>
		/// <param name="rClass">a reference to a class</param>
		/// <returns>whether the given object is a direct instance of the given class.
		/// On error, <code>false</code> is returned.</returns>
		/// <seealso cref="TDAKernel.isKindOf" />
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsTypeOf(long rObject, long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsTypeOf32(tdaKernel, rObject, rClass)!=0:
				TDA_IsTypeOf64(tdaKernel, rObject, rClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetRoleName")]
		private static extern IntPtr TDA_GetRoleName32(IntPtr tdaKernel, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_GetRoleName")]
		private static extern IntPtr TDA_GetRoleName64(IntPtr tdaKernel, long rAssociationEnd);
		/// <summary>
		/// Returns the role name of the given association end.
		/// </summary>
		/// <param name="rAssociationEnd">an association end of some directed, bidirectional, or advanced association</param>
		/// <returns>the role name of the given association end, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public String getRoleName(long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetRoleName32(tdaKernel, rAssociationEnd):
				TDA_GetRoleName64(tdaKernel, rAssociationEnd));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Returns the role name of the given association end.
		/// </summary>
		/// <param name="rAssociationEnd">an association end of some directed, bidirectional, or advanced association</param>
		/// <returns>the role name of the given association end, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public String GetRoleName(long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String s = Marshal.PtrToStringAnsi(is32?TDA_GetRoleName32(tdaKernel, rAssociationEnd):
				TDA_GetRoleName64(tdaKernel, rAssociationEnd));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FindAttribute")]
		private static extern long TDA_FindAttribute32(IntPtr tdaKernel, long rClass, String name);
		[DllImport("tdakernel64", EntryPoint="TDA_FindAttribute")]
		private static extern long TDA_FindAttribute64(IntPtr tdaKernel, long rClass, String name);
		/// <summary>
		/// Obtains a reference to an existing attribute with the given name of the given class.
		/// <BR/><I><B>Note (M3): </B></I>The class reference may point also to a quasi-linguistic class.
		/// </summary>
		/// <param name="rClass">a reference to a class, where the attribute in question belongs;
		///               <code>rClass</code> may be also one of its subclasses, since the attribute
		///               is available for subclasses, too</param>
		/// <param name="name">the name of the attribute</param>
		/// <returns>a reference to the desired attribute, or 0 on error;
		///         the reference returned is the same reference for the class,
		///         for which the attribute was defined, as well as for derived classes.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long findAttribute(long rClass, String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_FindAttribute32(tdaKernel, rClass, _name):
				TDA_FindAttribute64(tdaKernel, rClass, _name);
		}

		/// <summary>
		/// Obtains a reference to an existing attribute with the given name of the given class.
		/// <BR/><I><B>Note (M3): </B></I>The class reference may point also to a quasi-linguistic class.
		/// </summary>
		/// <param name="rClass">a reference to a class, where the attribute in question belongs;
		///               <code>rClass</code> may be also one of its subclasses, since the attribute
		///               is available for subclasses, too</param>
		/// <param name="name">the name of the attribute</param>
		/// <returns>a reference to the desired attribute, or 0 on error;
		///         the reference returned is the same reference for the class,
		///         for which the attribute was defined, as well as for derived classes.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long FindAttribute(long rClass, String name)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _name = (name==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(name));
			return is32?TDA_FindAttribute32(tdaKernel, rClass, _name):
				TDA_FindAttribute64(tdaKernel, rClass, _name);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsDerivedClass")]
		private static extern byte TDA_IsDerivedClass32(IntPtr tdaKernel, long rDirectlyOrIndirectlyDerivedClass, long rSuperClass);
		[DllImport("tdakernel64", EntryPoint="TDA_IsDerivedClass")]
		private static extern byte TDA_IsDerivedClass64(IntPtr tdaKernel, long rDirectlyOrIndirectlyDerivedClass, long rSuperClass);
		/// <summary>
		/// Checks whether one class is a direct or indirect subclass of another.
		/// <BR/><I><B>Note (M3): </B></I>Both classes may be either quasi-ontological, or quasi-linguistic.
		/// </summary>
		/// <param name="rDirectlyOrIndirectlyDerivedClass">a reference to a potential subclass or derived class</param>
		/// <param name="rSuperClass">a reference to a potential (direct or indirect) superclass</param>
		/// <returns>whether the first class derives from the second.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isDerivedClass(long rDirectlyOrIndirectlyDerivedClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsDerivedClass32(tdaKernel, rDirectlyOrIndirectlyDerivedClass, rSuperClass)!=0:
				TDA_IsDerivedClass64(tdaKernel, rDirectlyOrIndirectlyDerivedClass, rSuperClass)!=0;
		}

		/// <summary>
		/// Checks whether one class is a direct or indirect subclass of another.
		/// <BR/><I><B>Note (M3): </B></I>Both classes may be either quasi-ontological, or quasi-linguistic.
		/// </summary>
		/// <param name="rDirectlyOrIndirectlyDerivedClass">a reference to a potential subclass or derived class</param>
		/// <param name="rSuperClass">a reference to a potential (direct or indirect) superclass</param>
		/// <returns>whether the first class derives from the second.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsDerivedClass(long rDirectlyOrIndirectlyDerivedClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsDerivedClass32(tdaKernel, rDirectlyOrIndirectlyDerivedClass, rSuperClass)!=0:
				TDA_IsDerivedClass64(tdaKernel, rDirectlyOrIndirectlyDerivedClass, rSuperClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeleteClass")]
		private static extern byte TDA_DeleteClass32(IntPtr tdaKernel, long rClass);
		[DllImport("tdakernel64", EntryPoint="TDA_DeleteClass")]
		private static extern byte TDA_DeleteClass64(IntPtr tdaKernel, long rClass);
		/// <summary>
		/// Deletes the class and frees the reference.
		/// </summary>
		/// <param name="rClass">a reference to the class to be deleted</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool deleteClass(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteClass32(tdaKernel, rClass)!=0:
				TDA_DeleteClass64(tdaKernel, rClass)!=0;
		}

		/// <summary>
		/// Deletes the class and frees the reference.
		/// </summary>
		/// <param name="rClass">a reference to the class to be deleted</param>
		/// <returns>whether the operation succeeded.</returns>
		public bool DeleteClass(long rClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_DeleteClass32(tdaKernel, rClass)!=0:
				TDA_DeleteClass64(tdaKernel, rClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsComposition")]
		private static extern byte TDA_IsComposition32(IntPtr tdaKernel, long rTargetAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_IsComposition")]
		private static extern byte TDA_IsComposition64(IntPtr tdaKernel, long rTargetAssociationEnd);
		/// <summary>
		/// Returns, whether the directed or bidirectional association (or a part of an advanced association) specified by its target association end is a composition (i.e., whether the source class objects are containers for the target class objects).
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="rTargetAssociationEnd">an association end of some association;
		///   this association end will be considered a target end</param>
		/// <returns>whether the directed or bidirectional association (or a part of an advanced association) is a composition.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isComposition(long rTargetAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsComposition32(tdaKernel, rTargetAssociationEnd)!=0:
				TDA_IsComposition64(tdaKernel, rTargetAssociationEnd)!=0;
		}

		/// <summary>
		/// Returns, whether the directed or bidirectional association (or a part of an advanced association) specified by its target association end is a composition (i.e., whether the source class objects are containers for the target class objects).
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="rTargetAssociationEnd">an association end of some association;
		///   this association end will be considered a target end</param>
		/// <returns>whether the directed or bidirectional association (or a part of an advanced association) is a composition.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsComposition(long rTargetAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsComposition32(tdaKernel, rTargetAssociationEnd)!=0:
				TDA_IsComposition64(tdaKernel, rTargetAssociationEnd)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetTargetClass")]
		private static extern long TDA_GetTargetClass32(IntPtr tdaKernel, long rTargetAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_GetTargetClass")]
		private static extern long TDA_GetTargetClass64(IntPtr tdaKernel, long rTargetAssociationEnd);
		/// <summary>
		/// Obtains a reference to the class corresponding to the given association end of some directed, bidirectional, or advanced association.
		/// For bidirectional and advanced associations, any of the two association ends can be considered a target end, when calling this function.
		/// </summary>
		/// <param name="rTargetAssociationEnd">an association end of some association;
		///   this association end will be considered a target end</param>
		/// <returns>a reference to the class corresponding to the given association end.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public long getTargetClass(long rTargetAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetTargetClass32(tdaKernel, rTargetAssociationEnd):
				TDA_GetTargetClass64(tdaKernel, rTargetAssociationEnd);
		}

		/// <summary>
		/// Obtains a reference to the class corresponding to the given association end of some directed, bidirectional, or advanced association.
		/// For bidirectional and advanced associations, any of the two association ends can be considered a target end, when calling this function.
		/// </summary>
		/// <param name="rTargetAssociationEnd">an association end of some association;
		///   this association end will be considered a target end</param>
		/// <returns>a reference to the class corresponding to the given association end.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		public long GetTargetClass(long rTargetAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetTargetClass32(tdaKernel, rTargetAssociationEnd):
				TDA_GetTargetClass64(tdaKernel, rTargetAssociationEnd);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsDirectSubClass")]
		private static extern byte TDA_IsDirectSubClass32(IntPtr tdaKernel, long rSubClass, long rSuperClass);
		[DllImport("tdakernel64", EntryPoint="TDA_IsDirectSubClass")]
		private static extern byte TDA_IsDirectSubClass64(IntPtr tdaKernel, long rSubClass, long rSuperClass);
		/// <summary>
		/// Checks whether the generalization relation between the two given classes holds.
		/// <BR/><I><B>Note (M3): </B></I>Both classes may be either quasi-ontological, or quasi-linguistic.
		/// </summary>
		/// <param name="rSubClass">a reference to a potential subclass</param>
		/// <param name="rSuperClass">a reference to a potential superclass</param>
		/// <returns>whether the generalization relation holds.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isDirectSubClass(long rSubClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsDirectSubClass32(tdaKernel, rSubClass, rSuperClass)!=0:
				TDA_IsDirectSubClass64(tdaKernel, rSubClass, rSuperClass)!=0;
		}

		/// <summary>
		/// Checks whether the generalization relation between the two given classes holds.
		/// <BR/><I><B>Note (M3): </B></I>Both classes may be either quasi-ontological, or quasi-linguistic.
		/// </summary>
		/// <param name="rSubClass">a reference to a potential subclass</param>
		/// <param name="rSuperClass">a reference to a potential superclass</param>
		/// <returns>whether the generalization relation holds.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsDirectSubClass(long rSubClass, long rSuperClass)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsDirectSubClass32(tdaKernel, rSubClass, rSuperClass)!=0:
				TDA_IsDirectSubClass64(tdaKernel, rSubClass, rSuperClass)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CreateLink")]
		private static extern byte TDA_CreateLink32(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		[DllImport("tdakernel64", EntryPoint="TDA_CreateLink")]
		private static extern byte TDA_CreateLink64(IntPtr tdaKernel, long rSourceObject, long rTargetObject, long rAssociationEnd);
		/// <summary>
		/// Creates a link of the given type (specified by <code>rAssociationEnd</code>) between two objects.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool createLink(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CreateLink32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0:
				TDA_CreateLink64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0;
		}

		/// <summary>
		/// Creates a link of the given type (specified by <code>rAssociationEnd</code>) between two objects.
		/// <BR/><I><B>Note (M3): </B></I>An association end at Level M3 can also be passed.
		///   In this case, at least one of the source and target objects must be an element at the M_Omega level.
		///   The semantics of such link then depends on a particular quasi-linguistic metamodel at Level M3.
		/// </summary>
		/// <param name="rSourceObject">a start object of the link;
		///   this object must be an instance of the source class for the given association end</param>
		/// <param name="rTargetObject">an end object of the link;
		///   this object must be an instance of the target class for the given association end</param>
		/// <param name="rAssociationEnd">a target association end that specifies the link type</param>
		/// <returns>whether the operation succeeded.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#advancedssociations">on advanced associations</A></seealso>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool CreateLink(long rSourceObject, long rTargetObject, long rAssociationEnd)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_CreateLink32(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0:
				TDA_CreateLink64(tdaKernel, rSourceObject, rTargetObject, rAssociationEnd)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_FreeReference")]
		private static extern void TDA_FreeReference32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_FreeReference")]
		private static extern void TDA_FreeReference64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Frees the memory associated with the given reference (if necessary).
		/// </summary>
		public void freeReference(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_FreeReference32(tdaKernel, r); else
				TDA_FreeReference64(tdaKernel, r);
		}

		/// <summary>
		/// Frees the memory associated with the given reference (if necessary).
		/// </summary>
		public void FreeReference(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return;
			if (is32) TDA_FreeReference32(tdaKernel, r); else
				TDA_FreeReference64(tdaKernel, r);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_ResolveIterator")]
		private static extern long TDA_ResolveIterator32(IntPtr tdaKernel, long it, int position);
		[DllImport("tdakernel64", EntryPoint="TDA_ResolveIterator")]
		private static extern long TDA_ResolveIterator64(IntPtr tdaKernel, long it, int position);
		/// <summary>
		/// Returns a reference to the element at the given <code>position</code> (numeration starts from 0) and forwards the iterator to <code>position+1</code>.
		/// <BR/><I><B>Note (adapters): </B></I> If not implemented in a repository adapter,
		/// TDA Kernel traverses all the elements and stores them in a temporary list.
		/// Thus, the first call will take the linear execution time, while all subsequent calls
		/// will take the constant time. The same refers to the <code>getIteratorLength</code>
		/// function. If both <code>getIteratorLength</code> and <code>resolveIterator</code>
		/// are used, the temporary list is created only once.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <param name="position">the position in the iterable list, where the interested element is located</param>
		/// <returns>a reference to the element at the given position, or 0 if the position is out of bounds, or if an error occurred.</returns>
		/// <seealso cref="TDAKernel.getIteratorLength" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public long resolveIterator(long it, int position)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_ResolveIterator32(tdaKernel, it, position):
				TDA_ResolveIterator64(tdaKernel, it, position);
		}

		/// <summary>
		/// Returns a reference to the element at the given <code>position</code> (numeration starts from 0) and forwards the iterator to <code>position+1</code>.
		/// <BR/><I><B>Note (adapters): </B></I> If not implemented in a repository adapter,
		/// TDA Kernel traverses all the elements and stores them in a temporary list.
		/// Thus, the first call will take the linear execution time, while all subsequent calls
		/// will take the constant time. The same refers to the <code>getIteratorLength</code>
		/// function. If both <code>getIteratorLength</code> and <code>resolveIterator</code>
		/// are used, the temporary list is created only once.
		/// </summary>
		/// <param name="it">an iterator reference</param>
		/// <param name="position">the position in the iterable list, where the interested element is located</param>
		/// <returns>a reference to the element at the given position, or 0 if the position is out of bounds, or if an error occurred.</returns>
		/// <seealso cref="TDAKernel.getIteratorLength" />
		/// <seealso cref="TDAKernel.freeIterator" />
		/// <seealso><a href="http://webappos.org/dev/raapi/notes.html#iterators">on iterators</a></seealso>
		public long ResolveIterator(long it, int position)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_ResolveIterator32(tdaKernel, it, position):
				TDA_ResolveIterator64(tdaKernel, it, position);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_IsAttribute")]
		private static extern byte TDA_IsAttribute32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_IsAttribute")]
		private static extern byte TDA_IsAttribute64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Checks whether the given reference is associated with an attribute.
		/// <BR/><I><B>Note (adapters): </B></I>If a repository adapter
		/// does not implement this function, TDA Kernel will implement it by means of
		/// <code>getAttributeDomain</code>, <code>getAttributeName</code> and <code>findAttribute</code>.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with an attribute.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool isAttribute(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsAttribute32(tdaKernel, r)!=0:
				TDA_IsAttribute64(tdaKernel, r)!=0;
		}

		/// <summary>
		/// Checks whether the given reference is associated with an attribute.
		/// <BR/><I><B>Note (adapters): </B></I>If a repository adapter
		/// does not implement this function, TDA Kernel will implement it by means of
		/// <code>getAttributeDomain</code>, <code>getAttributeName</code> and <code>findAttribute</code>.
		/// <BR/><I><B>Note (M3): </B></I>A reference at Level M3 can also be passed.
		/// </summary>
		/// <param name="r">a reference in question</param>
		/// <returns>whether the given reference is associated with an attribute.
		///         On error, <code>false</code> is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public bool IsAttribute(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return false;
			return is32?TDA_IsAttribute32(tdaKernel, r)!=0:
				TDA_IsAttribute64(tdaKernel, r)!=0;
		}

		[DllImport("tdakernel32", EntryPoint="TDA_DeserializeReference")]
		private static extern long TDA_DeserializeReference32(IntPtr tdaKernel, String r);
		[DllImport("tdakernel64", EntryPoint="TDA_DeserializeReference")]
		private static extern long TDA_DeserializeReference64(IntPtr tdaKernel, String r);
		/// <summary>
		/// Obtains a reference to a serialized element from the given serialization.
		/// This is essential for loading inter-repository relations.
		/// </summary>
		/// <param name="r">the serialization of an element, for which to obtain a reference</param>
		/// <returns>a reference corresponding for the given serialization, or 0 on error.</returns>
		/// <seealso cref="TDAKernel.serializeReference" />
		public long deserializeReference(String r)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _r = (r==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(r));
			return is32?TDA_DeserializeReference32(tdaKernel, _r):
				TDA_DeserializeReference64(tdaKernel, _r);
		}

		/// <summary>
		/// Obtains a reference to a serialized element from the given serialization.
		/// This is essential for loading inter-repository relations.
		/// </summary>
		/// <param name="r">the serialization of an element, for which to obtain a reference</param>
		/// <returns>a reference corresponding for the given serialization, or 0 on error.</returns>
		/// <seealso cref="TDAKernel.serializeReference" />
		public long DeserializeReference(String r)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			String _r = (r==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(r));
			return is32?TDA_DeserializeReference32(tdaKernel, _r):
				TDA_DeserializeReference64(tdaKernel, _r);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetLinguisticClassFor")]
		private static extern long TDA_GetLinguisticClassFor32(IntPtr tdaKernel, long r);
		[DllImport("tdakernel64", EntryPoint="TDA_GetLinguisticClassFor")]
		private static extern long TDA_GetLinguisticClassFor64(IntPtr tdaKernel, long r);
		/// <summary>
		/// Returns a reference to the Level M3 class of the given quasi-ontological (Level M_Omega) element.
		/// It is assumed that there may be at most one quasi-linguistic class at M3 for each quasi-ontological element at M_Omega.
		/// </summary>
		/// <param name="r">a quasi-ontological (Level M_Omega) element</param>
		/// <returns>a reference to the Level M3 class of the given quasi-ontological (Level M_Omega) element.
		///   On error or if M3 is not supported by the underlying repository, 0 is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getLinguisticClassFor(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetLinguisticClassFor32(tdaKernel, r):
				TDA_GetLinguisticClassFor64(tdaKernel, r);
		}

		/// <summary>
		/// Returns a reference to the Level M3 class of the given quasi-ontological (Level M_Omega) element.
		/// It is assumed that there may be at most one quasi-linguistic class at M3 for each quasi-ontological element at M_Omega.
		/// </summary>
		/// <param name="r">a quasi-ontological (Level M_Omega) element</param>
		/// <returns>a reference to the Level M3 class of the given quasi-ontological (Level M_Omega) element.
		///   On error or if M3 is not supported by the underlying repository, 0 is returned.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetLinguisticClassFor(long r)
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetLinguisticClassFor32(tdaKernel, r):
				TDA_GetLinguisticClassFor64(tdaKernel, r);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_GetIteratorForLinguisticClasses")]
		private static extern long TDA_GetIteratorForLinguisticClasses32(IntPtr tdaKernel);
		[DllImport("tdakernel64", EntryPoint="TDA_GetIteratorForLinguisticClasses")]
		private static extern long TDA_GetIteratorForLinguisticClasses64(IntPtr tdaKernel);
		/// <summary>
		/// Returns an iterator for all quasi-linguistic classes at Level M3.
		/// <BR/><I><B>Note (M3): </B></I>This function works only at Level M3.<BR/> 
		/// </summary>
		/// <returns>an iterator for all quasi-linguistic classes at Level M3.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long getIteratorForLinguisticClasses()
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForLinguisticClasses32(tdaKernel):
				TDA_GetIteratorForLinguisticClasses64(tdaKernel);
		}

		/// <summary>
		/// Returns an iterator for all quasi-linguistic classes at Level M3.
		/// <BR/><I><B>Note (M3): </B></I>This function works only at Level M3.<BR/> 
		/// </summary>
		/// <returns>an iterator for all quasi-linguistic classes at Level M3.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#metalevels">on meta-levels</A></seealso>
		public long GetIteratorForLinguisticClasses()
		{
			if (tdaKernel.ToInt64() == 0)
				return 0;
			return is32?TDA_GetIteratorForLinguisticClasses32(tdaKernel):
				TDA_GetIteratorForLinguisticClasses64(tdaKernel);
		}

		[DllImport("tdakernel32", EntryPoint="TDA_CallSpecificOperation")]
		private static extern IntPtr TDA_CallSpecificOperation32(IntPtr tdaKernel, String operationName, String arguments);
		[DllImport("tdakernel64", EntryPoint="TDA_CallSpecificOperation")]
		private static extern IntPtr TDA_CallSpecificOperation64(IntPtr tdaKernel, String operationName, String arguments);
		/// <summary>
		/// Calls a repository-specific operation (e.g., or MOF/ECore-like operation, an SQL statement, or a SPARQL statement).
		/// Arguments (if any) are encoded as a string delimited by means of the
		/// Unicode character U+001E (INFORMATION SEPARATOR TWO).
		/// For no-argument methods <code>arguments</code> must be <code>null</code>.
		/// <BR/>
		/// For instance, a repository may accept the following calls:
		/// <BR/><code>callSpecificOperation("SQL", "SELECT * FROM MY TABLE");</code>
		/// <BR/><code>callSpecificOperation("myMethod", "[object-reference][u001E][argument1]...");</code>
		/// <BR/><code>callSpecificOperation("", null);</code>
		/// <BR/>For static MOF/ECore-like operations, the first argument should point to a class.
		/// For non-static operations the first argument should point to an object
		/// (that resembles <code>this</code> pointer in Java).
		/// </summary>
		/// <param name="operationName">a repository-specific operation name</param>
		/// <param name="arguments">operation-specific arguments encoded as a string</param>
		/// <returns>the return value of the call encoded as a string, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		public String callSpecificOperation(String operationName, String arguments)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String _operationName = (operationName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(operationName));
			String _arguments = (arguments==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(arguments));
			String s = Marshal.PtrToStringAnsi(is32?TDA_CallSpecificOperation32(tdaKernel, _operationName, _arguments):
				TDA_CallSpecificOperation64(tdaKernel, _operationName, _arguments));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

		/// <summary>
		/// Calls a repository-specific operation (e.g., or MOF/ECore-like operation, an SQL statement, or a SPARQL statement).
		/// Arguments (if any) are encoded as a string delimited by means of the
		/// Unicode character U+001E (INFORMATION SEPARATOR TWO).
		/// For no-argument methods <code>arguments</code> must be <code>null</code>.
		/// <BR/>
		/// For instance, a repository may accept the following calls:
		/// <BR/><code>callSpecificOperation("SQL", "SELECT * FROM MY TABLE");</code>
		/// <BR/><code>callSpecificOperation("myMethod", "[object-reference][u001E][argument1]...");</code>
		/// <BR/><code>callSpecificOperation("", null);</code>
		/// <BR/>For static MOF/ECore-like operations, the first argument should point to a class.
		/// For non-static operations the first argument should point to an object
		/// (that resembles <code>this</code> pointer in Java).
		/// </summary>
		/// <param name="operationName">a repository-specific operation name</param>
		/// <param name="arguments">operation-specific arguments encoded as a string</param>
		/// <returns>the return value of the call encoded as a string, or <code>null</code> on error.</returns>
		/// <seealso><A HREF="http://webappos.org/dev/raapi/notes.html#encodingvalues">on encoding values</A></seealso>
		public String CallSpecificOperation(String operationName, String arguments)
		{
			if (tdaKernel.ToInt64() == 0)
				return null;
			String _operationName = (operationName==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(operationName));
			String _arguments = (arguments==null)?null:System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(arguments));
			String s = Marshal.PtrToStringAnsi(is32?TDA_CallSpecificOperation32(tdaKernel, _operationName, _arguments):
				TDA_CallSpecificOperation64(tdaKernel, _operationName, _arguments));
			if (s == null)
				return null;
			else
				return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(s));
		}

	} // class
} // namespace
