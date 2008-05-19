namespace globalType
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class pubs
    {
        
        private PublisherCollection _publishers;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("publishers")]
        public PublisherCollection publishers
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
    public class Publisher
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
    
    public class PublisherCollection : System.Collections.CollectionBase
    {
        
        public Publisher this[int idx]
        {
            get
            {
                return ((Publisher)(base.InnerList[idx]));
            }
            set
            {
                base.InnerList[idx] = value;
            }
        }
        
        public int Add(Publisher value)
        {
            return base.InnerList.Add(value);
        }
    }
}
