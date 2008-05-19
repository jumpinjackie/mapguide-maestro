namespace globalElement
{
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://weblogs.asp.net/cazzu/globalElements.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://weblogs.asp.net/cazzu/globalElements.xsd", IsNullable=false)]
    public class pubs
    {
        
        private PublisherCollection _publisher;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Publisher")]
        public PublisherCollection Publisher
        {
            get
            {
                return this._publisher;
            }
            set
            {
                this._publisher = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://weblogs.asp.net/cazzu/globalElements.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://weblogs.asp.net/cazzu/globalElements.xsd", IsNullable=false)]
    public class Publisher
    {
        
        private string _pub_id;
        
        private string _pub_name;
        
        private string _city;
        
        private string _state;
        
        private string _country;
        
        private int _date;
        
        private bool _dateSpecified;
        
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
        
        /// <remarks/>
        public int date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateSpecified
        {
            get
            {
                return this._dateSpecified;
            }
            set
            {
                this._dateSpecified = value;
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
