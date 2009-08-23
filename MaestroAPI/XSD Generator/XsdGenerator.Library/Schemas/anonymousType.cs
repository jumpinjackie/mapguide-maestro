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
namespace localType
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class pubs
    {
        
        private pubsPublishersCollection _publishers;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("publishers")]
        public pubsPublishersCollection publishers
        {
            get
            {
                return this._publishers;
            }
            set
            {
                this._publishers = value;
            }
        }
    }
    
    /// <remarks/>
    public class pubsPublishers
    {
        
        private string _pub_id;
        
        private string _pub_name;
        
        private string _city;
        
        private string _state;
        
        private string _country;
        
        /// <remarks/>
        public string pub_id
        {
            get
            {
                return this._pub_id;
            }
            set
            {
                this._pub_id = value;
            }
        }
        
        /// <remarks/>
        public string pub_name
        {
            get
            {
                return this._pub_name;
            }
            set
            {
                this._pub_name = value;
            }
        }
        
        /// <remarks/>
        public string city
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
            }
        }
        
        /// <remarks/>
        public string state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }
        
        /// <remarks/>
        public string country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }
    }
    
    public class pubsPublishersCollection : System.Collections.CollectionBase
    {
        
        public pubsPublishers this[int idx]
        {
            get
            {
                return ((pubsPublishers)(base.InnerList[idx]));
            }
            set
            {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(pubsPublishers value)
        {
            return base.InnerList.Add(value);
        }
    }
}
