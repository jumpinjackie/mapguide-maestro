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
                OnPropertyChanged("Override"); //NOXLATE
            }
        }

        [DisplayName("Spatial Context")] //NOXLATE
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
                OnPropertyChanged("SpatialContext"); //NOXLATE
            }
        }

        [DisplayName("Key Column")] //NOXLATE
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key"); //NOXLATE
            }
        }

        private bool _geom;

        [DisplayName("Has Geometry")] //NOXLATE
        public bool Geometry
        {
            get { return _geom; }
            set
            {
                _geom = value;
                OnPropertyChanged("Geometry"); //NOXLATE
            }
        }

        [DisplayName("X Column")] //NOXLATE
        public string X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged("X"); //NOXLATE
            }
        }

        [DisplayName("Y Column")] //NOXLATE
        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y"); //NOXLATE
            }
        }

        [DisplayName("Z Column")] //NOXLATE
        public string Z
        {
            get { return _z; }
            set
            {
                _z = value;
                OnPropertyChanged("Z"); //NOXLATE
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
