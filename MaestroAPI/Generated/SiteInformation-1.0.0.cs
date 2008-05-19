#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
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
    public class SiteInformation {
        
		public static readonly string SchemaName = "SiteInformation-1.0.0.xsd";
        
		[System.Xml.Serialization.XmlAttribute("noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
		public string XsdSchema
		{
			get { return SchemaName; }
			set { if (value != SchemaName) throw new System.Exception("Cannot set the schema name"); }
		}

        private SiteInformationSiteServer m_siteServer;
        
        private SiteInformationStatistics m_statistics;
        
        /// <remarks/>
        public SiteInformationSiteServer SiteServer {
            get {
                return this.m_siteServer;
            }
            set {
                this.m_siteServer = value;
            }
        }
        
        /// <remarks/>
        public SiteInformationStatistics Statistics {
            get {
                return this.m_statistics;
            }
            set {
                this.m_statistics = value;
            }
        }
    }
    
    /// <remarks/>
    public class SiteInformationSiteServer {
        
        private string m_displayName;
        
        private string m_status;
        
        private string m_version;
        
        private SiteInformationSiteServerOperatingSystem m_operatingSystem;
        
        /// <remarks/>
        public string DisplayName {
            get {
                return this.m_displayName;
            }
            set {
                this.m_displayName = value;
            }
        }
        
        /// <remarks/>
        public string Status {
            get {
                return this.m_status;
            }
            set {
                this.m_status = value;
            }
        }
        
        /// <remarks/>
        public string Version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }
        
        /// <remarks/>
        public SiteInformationSiteServerOperatingSystem OperatingSystem {
            get {
                return this.m_operatingSystem;
            }
            set {
                this.m_operatingSystem = value;
            }
        }
    }
    
    /// <remarks/>
    public class SiteInformationSiteServerOperatingSystem {
        
        private string m_availablePhysicalMemory;
        
        private string m_totalPhysicalMemory;
        
        private string m_availableVirtualMemory;
        
        private string m_totalVirtualMemory;
        
        private string m_version;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string AvailablePhysicalMemory {
            get {
                return this.m_availablePhysicalMemory;
            }
            set {
                this.m_availablePhysicalMemory = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TotalPhysicalMemory {
            get {
                return this.m_totalPhysicalMemory;
            }
            set {
                this.m_totalPhysicalMemory = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string AvailableVirtualMemory {
            get {
                return this.m_availableVirtualMemory;
            }
            set {
                this.m_availableVirtualMemory = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TotalVirtualMemory {
            get {
                return this.m_totalVirtualMemory;
            }
            set {
                this.m_totalVirtualMemory = value;
            }
        }
        
        /// <remarks/>
        public string Version {
            get {
                return this.m_version;
            }
            set {
                this.m_version = value;
            }
        }
    }
    
    /// <remarks/>
    public class SiteInformationStatistics {
        
        private string m_adminOperationsQueueCount;
        
        private string m_clientOperationsQueueCount;
        
        private string m_siteOperationsQueueCount;
        
        private string m_averageOperationTime;
        
        private string m_cpuUtilization;
        
        private string m_totalOperationTime;
        
        private string m_activeConnections;
        
        private string m_totalConnections;
        
        private string m_totalOperationsProcessed;
        
        private string m_totalOperationsReceived;
        
        private string m_uptime;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string AdminOperationsQueueCount {
            get {
                return this.m_adminOperationsQueueCount;
            }
            set {
                this.m_adminOperationsQueueCount = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string ClientOperationsQueueCount {
            get {
                return this.m_clientOperationsQueueCount;
            }
            set {
                this.m_clientOperationsQueueCount = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string SiteOperationsQueueCount {
            get {
                return this.m_siteOperationsQueueCount;
            }
            set {
                this.m_siteOperationsQueueCount = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string AverageOperationTime {
            get {
                return this.m_averageOperationTime;
            }
            set {
                this.m_averageOperationTime = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string CpuUtilization {
            get {
                return this.m_cpuUtilization;
            }
            set {
                this.m_cpuUtilization = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TotalOperationTime {
            get {
                return this.m_totalOperationTime;
            }
            set {
                this.m_totalOperationTime = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string ActiveConnections {
            get {
                return this.m_activeConnections;
            }
            set {
                this.m_activeConnections = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TotalConnections {
            get {
                return this.m_totalConnections;
            }
            set {
                this.m_totalConnections = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TotalOperationsProcessed {
            get {
                return this.m_totalOperationsProcessed;
            }
            set {
                this.m_totalOperationsProcessed = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string TotalOperationsReceived {
            get {
                return this.m_totalOperationsReceived;
            }
            set {
                this.m_totalOperationsReceived = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string Uptime {
            get {
                return this.m_uptime;
            }
            set {
                this.m_uptime = value;
            }
        }
    }
}
