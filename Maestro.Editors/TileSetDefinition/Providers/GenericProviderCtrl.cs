#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.TileSetDefinition;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.Editors.TileSetDefinition.Providers
{
    public partial class GenericProviderCtrl : UserControl
    {
        private GenericProviderCtrl()
        {
            InitializeComponent();
        }

        private readonly ITileSetDefinition _tsd;
        private readonly TileProvider _provider;
        private Action _resourceChangeHandler;

        public GenericProviderCtrl(TileProvider provider, ITileSetDefinition tsd, Action resourceChangeHandler)
            : this()
        {
            Check.ArgumentNotNull(provider, nameof(provider));
            Check.ArgumentNotNull(tsd, nameof(tsd));
            _tsd = tsd;
            _provider = provider;
            _resourceChangeHandler = resourceChangeHandler;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            grdSettings.DataSource = GetParameters();
        }

        private void OnValueChanged(TileSetParameter param)
        {
            _tsd.TileStoreParameters.SetParameter(param.Name, param.Value);
            _resourceChangeHandler();
        }

        private TileSetParameter[] GetParameters()
        {
            var param = new Dictionary<string, TileSetParameter>();
            //First collect parameters already set
            foreach (var p in _tsd.TileStoreParameters.Parameters)
            {
                param.Add(p.Name, new TileSetParameter(p.Name, p.Value, OnValueChanged));
            }
            //Then fill in missing parameters as indicated by the provider
            foreach (var p in _provider.ConnectionProperties)
            {
                if (!param.ContainsKey(p.Name))
                    param.Add(p.Name, new TileSetParameter(p.Name, p.DefaultValue, OnValueChanged));
            }
            return param.Values.ToArray();
        }
    }

    class TileSetParameter
    {
        private readonly Action<TileSetParameter> _valueChangeListener;

        public TileSetParameter(string name, string value, Action<TileSetParameter> valueChangeListener)
        {
            this.Name = name;
            this.Value = value;
            _valueChangeListener = valueChangeListener;
        }

        public string Name { get; }

        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (_valueChangeListener != null)
                        _valueChangeListener(this);
                }
            }
        }
    }
}
