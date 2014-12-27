#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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

#endregion Disclaimer / License

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OSGeo.MapGuide.ObjectModels.FeatureSource.v1_0_0
{
    partial class FeatureSourceType : IFeatureSource
    {
        internal FeatureSourceType()
        {
        }

        private static readonly Version RES_VERSION = new Version(1, 0, 0);

        private string _resId;

        [XmlIgnore]
        public string ResourceID
        {
            get
            {
                return _resId;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException(Strings.ErrorInvalidResourceIdentifier);

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.FeatureSource.ToString())
                    throw new InvalidOperationException(string.Format(Strings.ErrorUnexpectedResourceType, res.ToString(), ResourceTypes.FeatureSource));

                _resId = value;
                this.OnPropertyChanged("ResourceID"); //NOXLATE
            }
        }

        [XmlIgnore]
        public string ResourceType
        {
            get
            {
                return ResourceTypes.FeatureSource.ToString();
            }
        }

        [XmlIgnore]
        public Version ResourceVersion
        {
            get
            {
                return RES_VERSION;
            }
        }

        object ICloneable.Clone()
        {
            var fs = this.Clone();
            fs.DetachChangeListeners();
            return fs;
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")] //NOXLATE
        public string ValidatingSchema
        {
            get { return "FeatureSource-1.0.0.xsd"; } //NOXLATE
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        public void ClearConnectionProperties()
        {
            this.Parameter.Clear();
        }

        public string[] ConnectionPropertyNames
        {
            get
            {
                var names = new List<string>();
                foreach (var p in this.Parameter)
                {
                    names.Add(p.Name);
                }
                return names.ToArray();
            }
        }

        public string GetConnectionProperty(string name)
        {
            foreach (var p in this.Parameter)
            {
                if (p.Name == name)
                {
                    return p.Value;
                }
            }
            return string.Empty;
        }

        public void SetConnectionProperty(string name, string value)
        {
            NameValuePairType pr = null;
            foreach (var p in this.Parameter)
            {
                if (p.Name == name)
                {
                    pr = p;
                    break;
                }
            }

            bool bRaise = false;
            if (pr != null)
            {
                if (value != null)
                    pr.Value = value;
                else
                    this.Parameter.Remove(pr);
                bRaise = true;
            }
            else
            {
                if (value != null)
                {
                    this.Parameter.Add(new NameValuePairType() { Name = name, Value = value });
                    bRaise = true;
                }
            }

            if (bRaise)
                OnPropertyChanged("Parameter"); //NOXLATE
        }

        protected void DetachChangeListeners()
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                foreach (var h in handler.GetInvocationList())
                {
                    this.PropertyChanged -= (PropertyChangedEventHandler)h;
                }
                handler = null;
            }
        }

        [XmlIgnore]
        public string ConnectionString
        {
            get
            {
                return Utils.ToConnectionString(this.GetConnectionProperties());
            }
        }

        private const string DATA_FILE_PATH_TAG = "%MG_DATA_FILE_PATH%"; //NOXLATE

        /// <summary>
        /// Gets the name of the embedded data resource. Can only be called if <see cref="UsesEmbeddedDataFiles"/> returns true.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If <see cref="UsesEmbeddedDataFiles"/> is false</exception>
        public string GetEmbeddedDataName()
        {
            if (!this.UsesEmbeddedDataFiles)
                throw new InvalidOperationException(Strings.ERR_FS_NO_EMBEDDED_DATA);

            string connStr = this.ConnectionString;
            int tagIndex = connStr.IndexOf(DATA_FILE_PATH_TAG);

            int end = connStr.IndexOf(";", tagIndex + DATA_FILE_PATH_TAG.Length); //NOXLATE
            //The "File" parameter was the last parameter
            if (end < 0)
                return connStr.Substring(tagIndex + DATA_FILE_PATH_TAG.Length);
            else
                return connStr.Substring(tagIndex + DATA_FILE_PATH_TAG.Length, end - (tagIndex + DATA_FILE_PATH_TAG.Length));
        }

        private const string ALIAS_PREFIX = "%MG_DATA_PATH_ALIAS["; //NOXLATE

        /// <summary>
        /// Gets the name of the alias. Can only be called if <see cref="UsesAliasedDataFiles"/> returns true
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If <see cref="UsesAliasedDataFiles"/> is false </exception>
        public string GetAliasName()
        {
            if (!this.UsesAliasedDataFiles)
                throw new InvalidOperationException(Strings.ERR_FS_NO_ALIAS);

            string connStr = this.ConnectionString;

            int braceStart = connStr.IndexOf(ALIAS_PREFIX) + ALIAS_PREFIX.Length;
            int braceEnd = connStr.IndexOf(']', braceStart + 1); //NOXLATE
            int length = braceEnd - braceStart;

            return connStr.Substring(braceStart, length);
        }

        /// <summary>
        /// Gets the name of the aliased file. Can only be called if <see cref="UsesAliasedDataFiles"/> returns true. An
        /// empty string is returned if it is a directory (ie. no file name was found)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If <see cref="UsesAliasedDataFiles"/> is false</exception>
        public string GetAliasedFileName()
        {
            if (!this.UsesAliasedDataFiles)
                throw new InvalidOperationException(Strings.ERR_FS_NO_ALIAS);

            string connStr = this.ConnectionString;
            int braceStart = connStr.IndexOf(ALIAS_PREFIX) + ALIAS_PREFIX.Length;
            int braceEnd = connStr.IndexOf(']', braceStart + 1); //NOXLATE
            int aliasEnd = braceEnd + 2;

            int end = connStr.IndexOf(";", aliasEnd); //NOXLATE
            //The "File" parameter was the last parameter
            if (end < 0)
                return connStr.Substring(aliasEnd);
            else
                return connStr.Substring(aliasEnd, end - aliasEnd);
        }

        [XmlIgnore]
        public bool UsesEmbeddedDataFiles
        {
            get
            {
                return this.ConnectionString.Contains(DATA_FILE_PATH_TAG);
            }
        }

        [XmlIgnore]
        public bool UsesAliasedDataFiles
        {
            get
            {
                return this.ConnectionString.Contains(ALIAS_PREFIX);
            }
        }

        [XmlIgnore]
        IEnumerable<ISpatialContextInfo> IFeatureSource.SupplementalSpatialContextInfo
        {
            get
            {
                if (this.SupplementalSpatialContextInfo != null)
                {
                    foreach (var sc in this.SupplementalSpatialContextInfo)
                    {
                        yield return sc;
                    }
                }
            }
        }

        void IFeatureSource.AddSpatialContextOverride(ISpatialContextInfo sc)
        {
            var sp = sc as SpatialContextType;
            if (sp != null)
            {
                if (this.SupplementalSpatialContextInfo == null)
                    this.SupplementalSpatialContextInfo = new System.ComponentModel.BindingList<SpatialContextType>();

                this.SupplementalSpatialContextInfo.Add(sp);
            }
        }

        void IFeatureSource.RemoveSpatialContextOverride(ISpatialContextInfo sc)
        {
            var sp = sc as SpatialContextType;
            if (sp != null)
            {
                if (this.SupplementalSpatialContextInfo == null)
                    return;

                this.SupplementalSpatialContextInfo.Remove(sp);
            }
        }

        [XmlIgnore]
        IEnumerable<IFeatureSourceExtension> IFeatureSource.Extension
        {
            get
            {
                if (this.Extension != null)
                {
                    foreach (var ext in this.Extension)
                    {
                        yield return ext;
                    }
                }
            }
        }

        void IFeatureSource.AddExtension(IFeatureSourceExtension ext)
        {
            var e = ext as FeatureSourceTypeExtension;
            if (e != null)
            {
                this.Extension.Add(e);
            }
        }

        void IFeatureSource.RemoveExtension(IFeatureSourceExtension ext)
        {
            var e = ext as FeatureSourceTypeExtension;
            if (e != null)
            {
                this.Extension.Remove(e);
            }
        }
    }

    partial class SpatialContextType : ISpatialContextInfo { }

    partial class FeatureSourceTypeExtension : IFeatureSourceExtension
    {
        [XmlIgnore]
        IEnumerable<ICalculatedProperty> IFeatureSourceExtension.CalculatedProperty
        {
            get
            {
                foreach (var calc in this.CalculatedProperty)
                {
                    yield return calc;
                }
            }
        }

        void IFeatureSourceExtension.AddCalculatedProperty(ICalculatedProperty prop)
        {
            var calc = prop as CalculatedPropertyType;
            if (calc != null)
                this.CalculatedProperty.Add(calc);
        }

        void IFeatureSourceExtension.RemoveCalculatedProperty(ICalculatedProperty prop)
        {
            var calc = prop as CalculatedPropertyType;
            if (calc != null)
                this.CalculatedProperty.Remove(calc);
        }

        [XmlIgnore]
        IEnumerable<IAttributeRelation> IFeatureSourceExtension.AttributeRelate
        {
            get
            {
                foreach (var rel in this.AttributeRelate)
                {
                    yield return rel;
                }
            }
        }

        void IFeatureSourceExtension.AddRelation(IAttributeRelation relate)
        {
            var rel = relate as AttributeRelateType;
            if (rel != null)
                this.AttributeRelate.Add(rel);
        }

        void IFeatureSourceExtension.RemoveRelation(IAttributeRelation relate)
        {
            var rel = relate as AttributeRelateType;
            if (rel != null)
                this.AttributeRelate.Remove(rel);
        }
    }

    partial class CalculatedPropertyType : ICalculatedProperty { }

    partial class AttributeRelateType : IAttributeRelation
    {
        IRelateProperty IAttributeRelation.CreatePropertyJoin(string primaryProperty, string secondaryProperty)
        {
            return new RelatePropertyType() { FeatureClassProperty = primaryProperty, AttributeClassProperty = secondaryProperty };
        }

        [XmlIgnore]
        bool IAttributeRelation.ForceOneToOne
        {
            get
            {
                return this.ForceOneToOne;
            }
            set
            {
                this.ForceOneToOne = value;
                this.ForceOneToOneSpecified = true;
            }
        }

        void IAttributeRelation.RemoveAllRelateProperties()
        {
            this.RelateProperty.Clear();
        }

        [XmlIgnore]
        RelateTypeEnum IAttributeRelation.RelateType
        {
            get { return this.RelateType; }
            set
            {
                this.RelateType = value;
                this.RelateTypeSpecified = true;
            }
        }

        [XmlIgnore]
        IEnumerable<IRelateProperty> IAttributeRelation.RelateProperty
        {
            get
            {
                foreach (var rel in this.RelateProperty)
                {
                    yield return rel;
                }
            }
        }

        [XmlIgnore]
        int IAttributeRelation.RelatePropertyCount
        {
            get { return this.RelateProperty.Count; }
        }

        void IAttributeRelation.AddRelateProperty(IRelateProperty prop)
        {
            var rel = prop as RelatePropertyType;
            if (rel != null)
                this.RelateProperty.Add(rel);
        }

        void IAttributeRelation.RemoveRelateProperty(IRelateProperty prop)
        {
            var rel = prop as RelatePropertyType;
            if (rel != null)
                this.RelateProperty.Remove(rel);
        }
    }

    partial class RelatePropertyType : IRelateProperty { }
}