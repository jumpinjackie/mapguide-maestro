#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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
namespace OSGeo.MapGuide.MaestroAPI {
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class UnmanagedDataList {
        
        private System.Collections.ArrayList m_items;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("UnmanagedDataFolder", typeof(UnmanagedDataListUnmanagedDataFolder))]
        [System.Xml.Serialization.XmlElementAttribute("UnmanagedDataFile", typeof(UnmanagedDataListUnmanagedDataFile))]
        public System.Collections.ArrayList Items {
            get {
                return this.m_items;
            }
            set {
                this.m_items = value;
            }
        }
    }
    
    /// <remarks/>
    public class UnmanagedDataListUnmanagedDataFolder {
        
        private string m_unmanagedDataId;
        
        private System.DateTime m_createdDate;
        
        private System.DateTime m_modifiedDate;
        
        private string m_numberOfFolders;
        
        private string m_numberOfFiles;
        
        /// <remarks/>
        public string UnmanagedDataId {
            get {
                return this.m_unmanagedDataId;
            }
            set {
                this.m_unmanagedDataId = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CreatedDate {
            get {
                return this.m_createdDate;
            }
            set {
                this.m_createdDate = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ModifiedDate {
            get {
                return this.m_modifiedDate;
            }
            set {
                this.m_modifiedDate = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string NumberOfFolders {
            get {
                return this.m_numberOfFolders;
            }
            set {
                this.m_numberOfFolders = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string NumberOfFiles {
            get {
                return this.m_numberOfFiles;
            }
            set {
                this.m_numberOfFiles = value;
            }
        }
    }
    
    /// <remarks/>
    public class UnmanagedDataListUnmanagedDataFile {
        
        private string m_unmanagedDataId;
        
        private System.DateTime m_createdDate;
        
        private System.DateTime m_modifiedDate;
        
        private string m_size;
        
        /// <remarks/>
        public string UnmanagedDataId {
            get {
                return this.m_unmanagedDataId;
            }
            set {
                this.m_unmanagedDataId = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CreatedDate {
            get {
                return this.m_createdDate;
            }
            set {
                this.m_createdDate = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ModifiedDate {
            get {
                return this.m_modifiedDate;
            }
            set {
                this.m_modifiedDate = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Size {
            get {
                return this.m_size;
            }
            set {
                this.m_size = value;
            }
        }
    }
   
}
