package lv.lumii.tda.raapi;


/**
 * The <code>IRepositoryManagement</code> interface contains
 * technical operations on repositories such as operations
 * for opening, closing, saving, etc.
 * This interface is a complement to <code>RAAPI</code>.
 ***/
public interface IRepositoryManagement 
{

  /** Checks whether the given location is already occupied by some repository of the same type.
     * This can be used to ask for the user confirmation to drop an existing repository,
     * when creating a new one at the same location.
     * @param location a string denoting the location to check. The location string is
     *   is specific to the type of the repository, e.g.,
     *   for ECore this is the .xmi file name, for JR this is the folder name, etc.
     *   TDA Kernel requires a URI, containing the repository name followed by a colon
     *   followed by a repository-specific location, e.g., "jr:/path/to/repository".
     * @return whether the given location is already occupied by some repository of the same type.
     */
  boolean exists (String location);

  /**
     * Opens or creates (if the repository does not exist yet) the repository at the given location.
     * This can be used to ask for the user confirmation to drop an existing repository,
     * when creating a new one at the same location.
     * @param location a string denoting the location of the repository. The location string is
     *   is specific to the type of the repository, e.g.,
     *   for ECore this is the .xmi file name, for JR this is the folder name, etc.
     *   TDA Kernel requires a URI, containing the repository name followed by a colon
     *   followed by a repository-specific location, e.g., "jr:/path/to/repository".
     * @return whether the repository has been opened or created.
     */
  boolean open (String location);

  /**
     * Closes the repository without save.<BR/>&nbsp;
     * @see IRepositoryManagement#startSave
     * @see IRepositoryManagement#finishSave
     * @see IRepositoryManagement#cancelSave
     */
  void close ();

  /**
     * Starts the two-phase save process of the repository.
     * The save process can be rolled back by calling <code>cancelSave</code>
     * or commited by calling <code>finishSave</code>.<BR/>&nbsp;
     * @return whether the operation succeeded. If <code>false</code> is returned,
     *   neither <code>cancelSave</code>, nor <code>finishSave</code> must be called.
     * @see IRepositoryManagement#finishSave
     * @see IRepositoryManagement#cancelSave
     */
  boolean startSave ();

  /**
     * Finishes the two-phase save process of the repository.
     * After finishing, the save process cannot be rolled back anymore.<BR/>&nbsp;
     * @return whether the operation succeeded.
     * @see IRepositoryManagement#startSave
     * @see IRepositoryManagement#cancelSave
     */
  boolean finishSave ();

  /**
     * Rolls back the started save process.
     * The repository content on the disk (or other media) is returned to the previous state.
     * The repository content currently loaded in memory is not changed.<BR/>&nbsp;
     * @return whether the operation succeeded.
     * @see IRepositoryManagement#startSave
     * @see IRepositoryManagement#finishSave
     */
  boolean cancelSave ();

  /** 
     * Deletes the repository at the given location.
     * The repository must be closed.
     * @param location a string denoting the location of the repository. The location string
     *   is specific to the type of the repository, e.g.,
     *   for ECore this is the .xmi file name, for JR this is the folder name, etc.
     *   TDA Kernel requires a URI, containing the repository name followed by a colon
     *   followed by a repository-specific location, e.g., "jr:/path/to/repository".
     * @return whether the operation succeeded.
     */
  boolean drop (String location);
  
} // interface IRepositoryManagement
