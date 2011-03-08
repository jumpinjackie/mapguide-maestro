#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
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
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using Maestro.Shared.UI;
using System.Globalization;
using System.Collections.Specialized;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Diagnostics;

namespace Maestro.Editors.Fusion
{
    public partial class MapCtrl : UserControl
    {
        const string G_NORMAL_MAP = "G_NORMAL_MAP";
        const string G_SATELLITE_MAP = "G_SATELLITE_MAP";
        const string G_HYBRID_MAP = "G_HYBRID_MAP";

        const string YAHOO_MAP_REG = "YAHOO_MAP_REG";
        const string YAHOO_MAP_SAT = "YAHOO_MAP_SAT";
        const string YAHOO_MAP_HYB = "YAHOO_MAP_HYB";

        const string BING_ROAD = "Road";
        const string BING_AERIAL = "Aerial";
        const string BING_HYBRID = "Hybrid";

        const string Type_Google = "Google";
        const string Type_Yahoo = "Yahoo";
        const string Type_Bing = "VirtualEarth";

        private MapCtrl()
        {
            InitializeComponent();
        }

        private IMap _map;
        private IMapGroup _group;
        private IMapView _initialView;
        private IResourceService _resSvc;

        private Dictionary<string, CmsMap> _cmsMaps;

        class CmsMap : IMap
        {
            internal IMap WrappedInstance { get { return _map; } }

            public bool IsEnabled { get; set; }

            private IMap _map;

            public CmsMap(IMap map) { _map = map; }

            public string Type
            {
                get
                {
                    return _map.Type;
                }
                set
                {
                    _map.Type = value;
                }
            }

            public bool SingleTile
            {
                get
                {
                    return _map.SingleTile;
                }
                set
                {
                    _map.SingleTile = value;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public IExtension Extension
            {
                get { return _map.Extension; }
            }


            public ICmsMapOptions CmsMapOptions
            {
                get
                {
                    return _map.CmsMapOptions;
                }
                set
                {
                    _map.CmsMapOptions = value;
                }
            }

            public ICmsMapOptions CreateOptions(string name, string type)
            {
                return _map.CreateOptions(name, type);
            }


            public IMapGuideOverlayOptions OverlayOptions
            {
                get
                {
                    return _map.OverlayOptions;
                }
                set
                {
                    _map.OverlayOptions = value;
                }
            }

            public IMapGuideOverlayOptions CreateOverlayOptions(bool isBaseLayer, bool useOverlay, string projection)
            {
                return _map.CreateOverlayOptions(isBaseLayer, useOverlay, projection);
            }
        }

        private bool _noEvents = true;
        private IApplicationDefinition _appDef;

        public MapCtrl(IApplicationDefinition appDef, IMapGroup group, IResourceService resSvc) : this() 
        {
            _appDef = appDef;
            _group = group;

            foreach (var map in group.Map)
            {
                if (map.Type.Equals("MapGuide"))
                {
                    _map = map;
                    break;
                }
            }

            _initialView = _group.InitialView;
            _resSvc = resSvc;
            _cmsMaps = new Dictionary<string, CmsMap>();
            chkOverride.Checked = (_initialView != null);

            InitCmsMaps(group);
            Debug.Assert(_cmsMaps.Count == 9);

            if (_initialView == null)
                _initialView = group.CreateInitialView(0.0, 0.0, 0.0);

            txtViewX.Text = _initialView.CenterX.ToString(CultureInfo.InvariantCulture);
            txtViewY.Text = _initialView.CenterY.ToString(CultureInfo.InvariantCulture);
            txtViewScale.Text = _initialView.Scale.ToString(CultureInfo.InvariantCulture);

            txtViewX.TextChanged += (s, e) =>
            {
                double d;
                if (double.TryParse(txtViewX.Text, out d))
                    _initialView.CenterX = d;
            };
            txtViewY.TextChanged += (s, e) =>
            {
                double d;
                if (double.TryParse(txtViewY.Text, out d))
                    _initialView.CenterY = d;
            };
            txtViewScale.TextChanged += (s, e) =>
            {
                double d;
                if (double.TryParse(txtViewScale.Text, out d))
                    _initialView.Scale = d;
            };

            TextBoxBinder.BindText(txtMapId, group, "id");

            txtMapDefinition.Text = _map.GetMapDefinition();
            txtMapDefinition.TextChanged += (s, e) => { _map.SetMapDefinition(txtMapDefinition.Text); };

            CheckBoxBinder.BindChecked(chkSingleTiled, _map, "SingleTile");

            var selOverlay = _map.GetValue("SelectionAsOverlay");
            var selColor = _map.GetValue("SelectionColor");

            if (!string.IsNullOrEmpty(selColor))
                cmbSelectionColor.CurrentColor = Utility.ParseHTMLColor(selColor.Substring(2)); //Strip the "0x" part
            
            if (!string.IsNullOrEmpty(selOverlay))
            {
                bool b = true;
                if (bool.TryParse(selOverlay, out b))
                    chkSelectionAsOverlay.Checked = b;
            }

            cmbSelectionColor.SelectedIndexChanged += (s, e) => 
            {
                _map.SetValue("SelectionColor", "0x" + Utility.SerializeHTMLColor(cmbSelectionColor.CurrentColor, true));
            };
            chkSelectionAsOverlay.CheckedChanged += (s, e) => { _map.SetValue("SelectionAsOverlay", chkSelectionAsOverlay.Checked.ToString().ToLower()); };
            
            _noEvents = false;
        }

        private void InitCmsMaps(IMapGroup group)
        {
            foreach (var map in group.Map)
            {
                var opts = map.CmsMapOptions;
                if (opts != null && _cmsMaps.ContainsKey(opts.Type))
                {
                    _cmsMaps[opts.Type] = new CmsMap(map) { IsEnabled = true };
                }
            }

            //Check for maps unaccounted for, these will be disabled
            if (!_cmsMaps.ContainsKey(G_HYBRID_MAP))
                _cmsMaps[G_HYBRID_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Maps Hybrid", G_HYBRID_MAP)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(G_NORMAL_MAP))
                _cmsMaps[G_NORMAL_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Maps Street", G_NORMAL_MAP)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(G_SATELLITE_MAP))
                _cmsMaps[G_SATELLITE_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Maps Satellite", G_SATELLITE_MAP)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(YAHOO_MAP_HYB))
                _cmsMaps[YAHOO_MAP_HYB] = new CmsMap(group.CreateCmsMapEntry(Type_Yahoo, true, "Yahoo! Maps Hybrid", YAHOO_MAP_HYB)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(YAHOO_MAP_REG))
                _cmsMaps[YAHOO_MAP_REG] = new CmsMap(group.CreateCmsMapEntry(Type_Yahoo, true, "Yahoo! Maps Street", YAHOO_MAP_REG)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(YAHOO_MAP_SAT))
                _cmsMaps[YAHOO_MAP_SAT] = new CmsMap(group.CreateCmsMapEntry(Type_Yahoo, true, "Yahoo! Maps Satellite", YAHOO_MAP_SAT)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(BING_ROAD))
                _cmsMaps[BING_ROAD] = new CmsMap(group.CreateCmsMapEntry(Type_Bing, true, "Bing Maps Street", BING_ROAD)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(BING_AERIAL))
                _cmsMaps[BING_AERIAL] = new CmsMap(group.CreateCmsMapEntry(Type_Bing, true, "Bing Maps Satellite", BING_AERIAL)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(BING_HYBRID))
                _cmsMaps[BING_HYBRID] = new CmsMap(group.CreateCmsMapEntry(Type_Bing, true, "Bing Maps Hybrid", BING_HYBRID)) { IsEnabled = false };

        }

        private void chkOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkOverride.Checked)
                _group.InitialView = _initialView;
            else
                _group.InitialView = null;
        }

        private void btnBrowseMdf_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_resSvc, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtMapDefinition.Text = picker.ResourceID;
                }
            }
        }

        private void SetCmsAvailability(string olType, bool enabled)
        {
            Trace.TraceInformation("Setting availability of CMS provider ({0}) to {1}", olType, enabled);
            if (_cmsMaps.ContainsKey(olType))
            {
                var map = _cmsMaps[olType];
                if (map.IsEnabled != enabled)
                {
                    map.IsEnabled = enabled;
                    if (enabled) //add
                    {
                        _group.AddMap(_cmsMaps[olType].WrappedInstance);
                    }
                    else //remove
                    {
                        IMap remove = null;
                        foreach (IMap m in _group.Map)
                        {
                            var opt = m.CmsMapOptions;
                            if (opt != null && opt.Type == olType)
                            {
                                remove = m;
                                break;
                            }
                        }

                        if (remove != null)
                            _group.RemoveMap(remove);
                    }
                }
            }

            if (IsUsingCmsLayers())
                _map.OverlayOptions = _map.CreateOverlayOptions(false, true, "EPSG:900913");
            else
                _map.OverlayOptions = null;
        }

        private bool IsUsingCmsLayers()
        {
            foreach (var map in _cmsMaps.Values)
            {
                if (map.IsEnabled)
                    return true;
            }

            return false;
        }

        private void chkBingStreets_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBingStreets.Checked)
                _appDef.SetValue("VirtualEarthScript", BING_URL);
            SetCmsAvailability(BING_ROAD, chkBingStreets.Checked);
        }

        private void chkBingSatellite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBingSatellite.Checked)
                _appDef.SetValue("VirtualEarthScript", BING_URL);
            SetCmsAvailability(BING_AERIAL, chkBingSatellite.Checked);
        }

        private void chkBingHybrid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBingHybrid.Checked)
                _appDef.SetValue("VirtualEarthScript", BING_URL);
            SetCmsAvailability(BING_HYBRID, chkBingHybrid.Checked);
        }

        private void chkGoogStreets_CheckedChanged(object sender, EventArgs e)
        {
            SetCmsAvailability(G_NORMAL_MAP, chkGoogStreets.Checked);
            EvaluateCmsStates();
        }

        private void chkGoogSatellite_CheckedChanged(object sender, EventArgs e)
        {
            SetCmsAvailability(G_SATELLITE_MAP, chkGoogSatellite.Checked);
            EvaluateCmsStates();
        }

        private void chkGoogHybrid_CheckedChanged(object sender, EventArgs e)
        {
            SetCmsAvailability(G_HYBRID_MAP, chkGoogHybrid.Checked);
            EvaluateCmsStates();
        }

        private void chkYahooStreets_CheckedChanged(object sender, EventArgs e)
        {
            SetCmsAvailability(YAHOO_MAP_REG, chkYahooStreets.Checked);
            EvaluateCmsStates();
        }

        private void chkYahooSatellite_CheckedChanged(object sender, EventArgs e)
        {
            SetCmsAvailability(YAHOO_MAP_SAT, chkYahooSatellite.Checked);
            EvaluateCmsStates();
        }

        private void chkYahooHybrid_CheckedChanged(object sender, EventArgs e)
        {
            SetCmsAvailability(YAHOO_MAP_HYB, chkYahooHybrid.Checked);
            EvaluateCmsStates();
        }

        private void EvaluateCmsStates()
        {
            txtGoogApiKey.Enabled = IsGoogleMapsEnabled();
            txtYahooApiKey.Enabled = IsYahooMapsEnabled();
        }

        private bool IsGoogleMapsEnabled()
        {
            return chkGoogHybrid.Checked ||
                   chkGoogSatellite.Checked ||
                   chkGoogStreets.Checked;
        }

        private bool IsYahooMapsEnabled()
        {
            return chkYahooHybrid.Checked ||
                   chkYahooSatellite.Checked ||
                   chkYahooStreets.Checked;
        }

        private void txtGoogApiKey_TextChanged(object sender, EventArgs e)
        {
            _appDef.SetValue("GoogleScript", GOOGLE_URL + txtGoogApiKey.Text);
        }

        private void txtYahooApiKey_TextChanged(object sender, EventArgs e)
        {
            _appDef.SetValue("YahooScript", YAHOO_URL + txtGoogApiKey.Text);
        }

        const string GOOGLE_URL = "http://maps.google.com/maps?file=api&amp;v=2&amp;key=";
        const string YAHOO_URL = "http://api.maps.yahoo.com/ajaxymap?v=3.0&amp;appid=";
        const string BING_URL = "http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2";
    }
}
