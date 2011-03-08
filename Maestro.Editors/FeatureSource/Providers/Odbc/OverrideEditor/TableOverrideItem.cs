using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.OverrideEditor
{
    internal class TableOverrideItem : INotifyPropertyChanged
    {
        public ClassDefinition Class { get; set; }

        public string TableName { get; set; }

        private bool _override;
        private string _key;
        private string _x;
        private string _y;
        private string _z;
        private string _spatialContext;

        public bool Override
        {
            get
            {
                return _override;
            }
            set
            {
                _override = value;
                OnPropertyChanged("Override");
            }
        }

        [DisplayName("Spatial Context")]
        public string SpatialContext
        {
            get { return _spatialContext; }
            set
            {
                _spatialContext = value;
                if (!string.IsNullOrEmpty(Class.DefaultGeometryPropertyName))
                {
                    var prop = Class.FindProperty(Class.DefaultGeometryPropertyName) as GeometricPropertyDefinition;
                    if (prop != null)
                        prop.SpatialContextAssociation = value;
                }
                OnPropertyChanged("SpatialContext");
            }
        }

        [DisplayName("Key Column")]
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        private bool _geom;

        [DisplayName("Has Geometry")]
        public bool Geometry
        {
            get { return _geom; }
            set
            {
                _geom = value;
                OnPropertyChanged("Geometry");
            }
        }

        [DisplayName("X Column")]
        public string X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }

        [DisplayName("Y Column")]
        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }

        [DisplayName("Z Column")]
        public string Z
        {
            get { return _z; }
            set
            {
                _z = value;
                OnPropertyChanged("Z");
            }
        }

        private void OnPropertyChanged(string name)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
