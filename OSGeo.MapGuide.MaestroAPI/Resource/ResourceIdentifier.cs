#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MaestroAPI.Resource
{
    /// <summary>
    /// This class contains all the required code for maintaining resource identifiers.
    /// It has implicit conversions to and from a string, which makes it much easier to use.
    /// It has both static methods that operate on strings, as well as a class that can be manipulated.
    /// </summary>
    public class ResourceIdentifier
    {
        /// <summary>
        /// The actual ResourceID
        /// </summary>
        private string m_id;

        /// <summary>
        /// Constructs a new ResourceIdentifier with the given full path
        /// </summary>
        /// <param name="resourceId">The path of the resource to refence</param>
        public ResourceIdentifier(string resourceId)
        {
            m_id = resourceId;
        }

        /// <summary>
        /// Constructs a new ResourceIdentifier, based on an existing one.
        /// </summary>
        /// <param name="id">The resource identifier to copy</param>
        public ResourceIdentifier(ResourceIdentifier id)
        {
            m_id = id.m_id;
        }

        /// <summary>
        /// Constructs a new library based resource identifier
        /// </summary>
        /// <param name="name">The name of the resource, may include path information with the \"/\" character</param>
        /// <param name="type">The type of resource the identifier names</param>
        public ResourceIdentifier(string name, ResourceTypes type)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name"); //NOXLATE
            if (name.IndexOf(".") > 0 || name.IndexOf("//") > 0 || name.IndexOf(":") > 0) //NOXLATE
                throw new ArgumentException(Strings.ErrorResourceIdentifierInvalidChars, "name"); //NOXLATE
            if (!Enum.IsDefined(typeof(ResourceTypes), type))
                throw new ArgumentException(Strings.ErrorUnknownResourceType, "type"); //NOXLATE
            m_id = StringConstants.RootIdentifier + name + EnumHelper.ResourceName(type, true);
        }

        /// <summary>
        /// Constructs a new session based resource identifier
        /// </summary>
        /// <param name="name">The name of the resource, may include path information with the \"/\" character</param>
        /// <param name="type">The type of resource the identifier names</param>
        /// <param name="sessionId">The session id to use</param>
        public ResourceIdentifier(string name, ResourceTypes type, string sessionId)
            : this(name, type)
        {
            this.ConvertToSession(sessionId);
        }

        /// <summary>
        /// Gets a value indicating if the resource is blank
        /// </summary>
        public bool IsEmpty { get { return string.IsNullOrEmpty(m_id); } }

        /// <summary>
        /// Gets or sets the name of the resource
        /// </summary>
        public string Name
        {
            get { return GetName(m_id); }
            set { m_id = SetName(m_id, value); }
        }

        /// <summary>
        /// Gets or sets the name and extension of the resource
        /// </summary>
        public string Fullname
        {
            get { return GetFullname(m_id); }
            set { m_id = SetName(m_id, value); }
        }

        /// <summary>
        /// Gets or sets the extension of the resourceId
        /// </summary>
        public string Extension
        {
            get { return GetExtension(m_id); }
            set { m_id = SetExtension(m_id, value); }
        }

        /// <summary>
        /// Gets the full path of the resource, that is the path without repository information
        /// </summary>
        public string Fullpath
        {
            get { return GetFullpath(m_id); }
            set { m_id = SetPath(m_id, value); }
        }

        /// <summary>
        /// Gets the path of the resource, that is the path without repository information and no extension
        /// </summary>
        public string Path
        {
            get { return GetPath(m_id); }
            set { m_id = SetPath(m_id, value); }
        }

        /// <summary>
        /// Gets or sets the path to the resource, including the repository
        /// </summary>
        public string RepositoryPath
        {
            get { return GetRepositoryPath(m_id); }
            set { m_id = SetRepositoryPath(m_id, value); }
        }

        /// <summary>
        /// Gets a value indicating if the resource is in the library repository
        /// </summary>
        public bool IsInLibrary
        {
            get { return GetRepository(m_id) == StringConstants.RootIdentifier; }
        }

        /// <summary>
        /// Gets a value indicating if the resource is in the session repository
        /// </summary>
        public bool IsInSessionRepository
        {
            get { return !this.IsInLibrary; }
        }

        /// <summary>
        /// Converts this instance to be in the library repository
        /// </summary>
        public void ConvertToLibrary()
        {
            m_id = ResourceIdentifier.ConvertToLibrary(m_id);
        }

        /// <summary>
        /// Converts this instance to be in the session repository
        /// </summary>
        /// <param name="sessionId">The sessionid</param>
        public void ConvertToSession(string sessionId)
        {
            m_id = ResourceIdentifier.ConvertToSession(m_id, sessionId);
        }

        /// <summary>
        /// Helper operator that makes using the resource identifiers easier
        /// </summary>
        /// <param name="id">The id to convert to a string</param>
        /// <returns>The converted string</returns>
        public static implicit operator string(ResourceIdentifier id)
        {
            return id == null ? null : id.m_id;
        }

        /// <summary>
        /// Helper operator that makes using the resource identifiers easier
        /// </summary>
        /// <param name="id">The id to convert into a resource indetifier class</param>
        /// <returns>The resource identifier</returns>
        public static implicit operator ResourceIdentifier(string id)
        {
            return new ResourceIdentifier(id);
        }

        /// <summary>
        /// Returns the full resource id as a string
        /// </summary>
        /// <returns>The full resource id as a string</returns>
        public override string ToString()
        {
            return m_id;
        }

        /// <summary>
        /// Gets the resource type
        /// </summary>
        public ResourceTypes ResourceType
        {
            get { return GetResourceType(m_id); }
        }

        /// <summary>
        /// Gets the length of the resource identifier as a string
        /// </summary>
        public int Length { get { return m_id == null ? 0 : m_id.Length; } }

        /// <summary>
        /// Gets or sets the full resource identifier
        /// </summary>
        public string ResourceId
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// Gets a value indicating if the resource identifier points to a folder
        /// </summary>
        public bool IsFolder
        {
            get { return ResourceIdentifier.IsFolderResource(m_id); }
        }

        /// <summary>
        /// Gets a value indicating if the resource identifier is valid
        /// </summary>
        public bool IsValid
        {
            get { return ResourceIdentifier.Validate(m_id); }
        }

        /// <summary>
        /// Normalizes the identifier, that is prepends a slash if the identifier points to a folder
        /// </summary>
        public void Normalize()
        {
            m_id = ResourceIdentifier.Normalize(m_id);
        }

        /// <summary>
        /// Gets the containing folder path for the resource, including the repository
        /// </summary>
        public string ParentFolder
        {
            get { return ResourceIdentifier.GetParentFolder(m_id); }
        }

        #region Static handlers

        /// <summary>
        /// Gets the name of a resource, given its identifier
        /// </summary>
        /// <param name="identifier">The identifier to look for</param>
        /// <returns>The name of the resource</returns>
        public static string GetName(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier"); //NOXLATE

            string temp = GetPath(identifier);

            if (string.IsNullOrEmpty(temp))
                throw new ArgumentException(Strings.ErrorInvalidResourceIdentifier, "identifier"); //NOXLATE

            return temp.Substring(temp.LastIndexOf("/") + 1); //NOXLATE
        }

        /// <summary>
        /// Sets the name of the resource, with or without the extension
        /// </summary>
        /// <param name="identifier">The identifier to give a new name</param>
        /// <param name="newname">The new name to assign</param>
        /// <returns>The renamed identifier</returns>
        public static string SetName(string identifier, string newname)
        {
            string temp = GetPath(identifier);
            if (identifier.EndsWith("/")) //NOXLATE
            {
                if (!newname.EndsWith("/")) //NOXLATE
                    newname += "/"; //NOXLATE
            }
            else
                newname += "." + GetExtension(identifier); //NOXLATE


            if (newname.IndexOf("/") > 0) //NOXLATE
                throw new ArgumentException(Strings.ErrorResourceIdentifierNameInvalidChars, "newname");
            temp = temp.Substring(0, temp.Length - GetName(identifier).Length) + newname;

            return GetRepository(identifier) + temp;
        }

        /// <summary>
        /// Sets the path of the identifier, with or without the extension
        /// </summary>
        /// <param name="identifier">The identifier to update</param>
        /// <param name="newpath">The new path to user, with or without the extension</param>
        /// <returns>The new identifier</returns>
        public static string SetPath(string identifier, string newpath)
        {
            string temp = GetPath(identifier);
            if (!identifier.EndsWith("/")) //NOXLATE
                newpath += "." + GetExtension(identifier); //NOXLATE

            return GetRepository(identifier) + newpath + (identifier.EndsWith("/") ? "/" : ""); //NOXLATE
        }

        /// <summary>
        /// Changes the extension of the given resource
        /// </summary>
        /// <param name="identifier">The identifier to change the extension for</param>
        /// <param name="newextension">The new extension to use</param>
        /// <returns>The renmaed identifier</returns>
        public static string SetExtension(string identifier, string newextension)
        {
            if (identifier.EndsWith("/")) //NOXLATE
                throw new Exception(Strings.ErrorResourceIdCannotChangeExtensionForFolder);

            if (!newextension.StartsWith(".")) //NOXLATE
                newextension = "." + newextension; //NOXLATE

            if (newextension.LastIndexOf(".") > 0) //NOXLATE
                throw new ArgumentException(Strings.ErrorResourceIdInvalidExtension, "newextension"); //NOXLATE

            return identifier.Substring(0, identifier.Length - GetExtension(identifier).Length - 1) + newextension;
        }

        /// <summary>
        /// Gets the repository part of a resource identifier, eg.: "Library://" or "Session:xxxx//"
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string GetRepository(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier"); //NOXLATE
            int ix = identifier.IndexOf("//"); //NOXLATE
            if (ix <= 0)
                throw new ArgumentException(Strings.ErrorInvalidResourceIdentifier, "identifier"); //NOXLATE

            string repo = identifier.Substring(0, ix);
            if (repo != "Library:" && !repo.StartsWith("Session:")) //NOXLATE
                throw new ArgumentException(Strings.ErrorInvalidResourceIdentifierType, "identifier"); //NOXLATE

            return repo + "//"; //NOXLATE
        }

        /// <summary>
        /// Returns the full path of the resource, that is the resourceId without the repository information
        /// </summary>
        /// <param name="identifier">The identifier to get the path from</param>
        /// <returns>The path of the identifier</returns>
        public static string GetFullpath(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier"); //NOXLATE
            return identifier.Substring(GetRepository(identifier).Length);
        }

        /// <summary>
        /// Returns the path of the resource, that is the resourceId without the repository information and extension
        /// </summary>
        /// <param name="identifier">The identifier to get the path from</param>
        /// <returns>The path of the identifier</returns>
        public static string GetPath(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier"); //NOXLATE
            return identifier.Substring(GetRepository(identifier).Length, identifier.Length - GetExtension(identifier).Length - GetRepository(identifier).Length - 1);
        }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public static ResourceTypes GetResourceType(string identifier)
        {
            var ext = GetExtension(identifier);
            if (ext == "Map") //NOXLATE
                return ResourceTypes.RuntimeMap;
            else
                return (ResourceTypes)Enum.Parse(typeof(ResourceTypes), ext);
        }

        /// <summary>
        /// Returns the extension of a resource identifier
        /// </summary>
        /// <param name="identifier">The identifier to get the extension from</param>
        /// <returns>The extension of the identifier</returns>
        private static string GetExtension(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier"); //NOXLATE

            if (identifier.EndsWith("/")) //NOXLATE
                return string.Empty;

            int ix = identifier.LastIndexOf("."); //NOXLATE
            if (ix <= 0)
                throw new ArgumentException(Strings.ErrorInvalidResourceIdentifier, "identifier"); //NOXLATE

            return identifier.Substring(ix + 1);
        }

        /// <summary>
        /// Converts a resource id to be placed in the library
        /// </summary>
        /// <param name="identifier">The identifier to convert</param>
        /// <returns>The converted identifier</returns>
        public static string ConvertToLibrary(string identifier)
        {
            return StringConstants.RootIdentifier + identifier.Substring(GetRepository(identifier).Length);
        }

        /// <summary>
        /// Converts a resource id to be placed in the library
        /// </summary>
        /// <param name="identifier">The identifier to convert</param>
        /// <param name="sessionId">The session id of the repository it should be placed in</param>
        /// <returns>The converted identifier</returns>
        public static string ConvertToSession(string identifier, string sessionId)
        {
            return "Session:" + sessionId + "//" + identifier.Substring(GetRepository(identifier).Length); //NOXLATE
        }

        /// <summary>
        /// Gets the name and extension of the identifier
        /// </summary>
        /// <param name="identifier">The identifier to extract the information from</param>
        /// <returns>The full name of the identifier</returns>
        public static string GetFullname(string identifier)
        {
            if (identifier.EndsWith("/")) //NOXLATE
                return GetName(identifier);
            else
                return GetName(identifier) + "." + GetExtension(identifier); //NOXLATE
        }

        /// <summary>
        /// Determines if a resource identifier is valid
        /// </summary>
        /// <param name="identifier">The identifier to validate</param>
        /// <returns>A value indicating if the identifier is valid</returns>
        public static bool Validate(string identifier)
        {
            try
            {
                GetRepository(identifier);
                if (identifier.IndexOf(".") < 0 && !identifier.EndsWith("/")) //NOXLATE
                    return false;

                if (identifier == StringConstants.RootIdentifier)
                    return true;

                if (IsFolderResource(identifier))
                    return true;

                var rt = GetResourceType(identifier);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a value indicating if the resource points to a folder
        /// </summary>
        /// <param name="identifier">The identifier to evaluate</param>
        /// <returns>A value indicating if the resource points to a folder</returns>
        public static bool IsFolderResource(string identifier)
        {
            return identifier.EndsWith("/"); //NOXLATE
        }

        /// <summary>
        /// Normalizes a identifier, that is prepends a slash if it is a folder resource
        /// </summary>
        /// <param name="identifier">The identifier to normalize</param>
        /// <returns>The normalized identifier</returns>
        public static string Normalize(string identifier)
        {
            if (identifier.LastIndexOf(".") <= identifier.LastIndexOf("/") && !identifier.EndsWith("/")) //NOXLATE
                return identifier + "/"; //NOXLATE
            else
                return identifier;
        }

        /// <summary>
        /// Determines if a resource identifier is valid and of the desired type
        /// </summary>
        /// <param name="identifier">The identifier to validate</param>
        /// <param name="type">The type the resource identifer must be</param>
        /// <returns>A value indicating if the identifier is valid</returns>
        public static bool Validate(string identifier, ResourceTypes type)
        {
            if (!Validate(identifier))
                return false;

            if (type == ResourceTypes.Folder)
                return IsFolderResource(identifier);
            else
                return EnumHelper.ResourceName(type) == GetExtension(identifier);
        }

        /// <summary>
        /// Returns the path that contains the resource, including the repository
        /// </summary>
        /// <param name="identifier">The resource identifier to use</param>
        /// <returns>The folder for the identifier</returns>
        public static string GetRepositoryPath(string identifier)
        {
            if (!Validate(identifier))
                throw new Exception("Invalid resource id: " + identifier); //NOXLATE
            identifier = Normalize(identifier);

            return identifier.Substring(0, identifier.LastIndexOf("/", identifier.Length)) + "/"; //NOXLATE
        }

        /// <summary>
        /// Sets the path including the repository to the given value
        /// </summary>
        /// <param name="identifier">The identifier to change the folder for</param>
        /// <param name="folder">The new folder</param>
        /// <returns>An identifier in the new folder</returns>
        public static string SetRepositoryPath(string identifier, string folder)
        {
            if (!folder.StartsWith("Library:") && !folder.StartsWith("Session:")) //NOXLATE
            {
                string res = identifier.EndsWith("/") ? string.Empty : GetFullname(identifier); //NOXLATE
                string repo = GetRepository(identifier);
                if (!folder.EndsWith("/") && !string.IsNullOrEmpty(folder)) //NOXLATE
                    folder += "/"; //NOXLATE
                return repo + folder + res;
            }
            else if (GetExtension(identifier) == string.Empty)
            {
                if (!folder.EndsWith("/")) //NOXLATE
                    folder += "/"; //NOXLATE
                return folder;
            }
            else
            {
                if (!folder.EndsWith("/")) //NOXLATE
                    folder += "/"; //NOXLATE
                return folder + GetFullname(identifier);
            }
        }

        /// <summary>
        /// Gets the parent folder.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public static string GetParentFolder(string identifier)
        {
            if (!Validate(identifier))
                throw new Exception(Strings.ErrorInvalidResourceIdentifier);
            identifier = Normalize(identifier);

            if (identifier == GetRepository(identifier))
                return identifier;

            if (identifier.EndsWith("/")) //NOXLATE
                identifier = identifier.Remove(identifier.Length - 1);

            identifier = identifier.Remove(identifier.LastIndexOf("/") + 1); //NOXLATE

            return identifier;
        }
    

        #endregion

        /// <summary>
        /// Determines whether this resource id is session-based
        /// </summary>
        /// <param name="resourceID">The resource ID.</param>
        /// <returns>
        /// 	<c>true</c> if this resource id is session-based; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSessionBased(string resourceID)
        {
            return resourceID.StartsWith("Session:"); //NOXLATE
        }
    }
}
