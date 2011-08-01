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
    [ToolboxItem(false)]
    internal partial class MapCtrl : UserControl, INotifyResourceChanged
    {
        const string G_NORMAL_MAP = "ROADMAP";
        const string G_SATELLITE_MAP = "SATELLITE";
        const string G_HYBRID_MAP = "HYBRID";
        const string G_PHYSICAL_MAP = "TERRAIN";

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
            cmbSelectionColor.ResetColors();
            this.Disposed += new EventHandler(OnDisposed);
        }

        private IMap _map;
        private IMapGroup _group;
        private IMapView _initialView;

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
        private IEditorService _edsvc;

        public MapCtrl(IApplicationDefinition appDef, IMapGroup group, IEditorService edService) : this() 
        {
            _edsvc = edService;
            _edsvc.RegisterCustomNotifier(this);
            _appDef = appDef;
            _group = group;

            foreach (var map in group.Map)
            {
                if (map.Type.Equals("MapGuide"))
                {
                    _map = map;
                }
                else
                {
                    var cmsOpts = map.CmsMapOptions;
                    if (cmsOpts != null)
                    {
                        switch (cmsOpts.Type)
                        {
                            case G_HYBRID_MAP:
                                chkGoogHybrid.Checked = true;
                                break;
                            case G_NORMAL_MAP:
                                chkGoogStreets.Checked = true;
                                break;
                            case G_SATELLITE_MAP:
                                chkGoogSatellite.Checked = true;
                                break;
                            case G_PHYSICAL_MAP:
                                chkGoogTerrain.Checked = true;
                                break;
                            case BING_AERIAL:
                                chkBingSatellite.Checked = true;
                                break;
                            case BING_HYBRID:
                                chkBingHybrid.Checked = true;
                                break;
                            case BING_ROAD:
                                chkBingStreets.Checked = true;
                                break;
                            case YAHOO_MAP_HYB:
                                chkYahooHybrid.Checked = true;
                                break;
                            case YAHOO_MAP_REG:
                                chkYahooStreets.Checked = true;
                                break;
                            case YAHOO_MAP_SAT:
                                chkYahooSatellite.Checked = true;
                                break;
                        }
                    }
                }
            }

            string googUrl = _appDef.GetValue("GoogleScript");
            string yahooUrl = _appDef.GetValue("YahooScript");

            if (!string.IsNullOrEmpty(yahooUrl))
            {
                txtYahooApiKey.Text = yahooUrl.Substring(YAHOO_URL.Length);
            }
            EvaluateCmsStates();

            _initialView = _group.InitialView;
            _cmsMaps = new Dictionary<string, CmsMap>();
            chkOverride.Checked = (_initialView != null);

            InitCmsMaps(group);
            Debug.Assert(_cmsMaps.Count == 10);

            if (_initialView == null)
                _initialView = group.CreateInitialView(0.0, 0.0, 0.0);

            txtViewX.Text = _initialView.CenterX.ToString(CultureInfo.InvariantCulture);
            txtViewY.Text = _initialView.CenterY.ToString(CultureInfo.InvariantCulture);
            txtViewScale.Text = _initialView.Scale.ToString(CultureInfo.InvariantCulture);

            txtViewX.TextChanged += (s, e) =>
            {
                double d;
                if (double.TryParse(txtViewX.Text, out d))
                {
                    _initialView.CenterX = d;
                    OnResourceChanged();
                }
            };
            txtViewY.TextChanged += (s, e) =>
            {
                double d;
                if (double.TryParse(txtViewY.Text, out d))
                {
                    _initialView.CenterY = d;
                    OnResourceChanged();
                }
            };
            txtViewScale.TextChanged += (s, e) =>
            {
                double d;
                if (double.TryParse(txtViewScale.Text, out d))
                {
                    _initialView.Scale = d;
                    OnResourceChanged();
                }
            };

            TextBoxBinder.BindText(txtMapId, group, "id");

            txtMapDefinition.Text = _map.GetMapDefinition();
            txtMapDefinition.TextChanged += (s, e) => 
            { 
                _map.SetMapDefinition(txtMapDefinition.Text);
                OnResourceChanged();
            };

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
                OnResourceChanged();
            };
            chkSelectionAsOverlay.CheckedChanged += (s, e) => 
            { 
                _map.SetValue("SelectionAsOverlay", chkSelectionAsOverlay.Checked.ToString().ToLower());
                OnResourceChanged();
            };
            
            _noEvents = false;
        }

        void OnDisposed(object sender, EventArgs e)
        {
            var handler = this.ResourceChanged;
            if (handler != null)
            {
                foreach (var h in handler.GetInvocationList())
                {
                    this.ResourceChanged -= (EventHandler)h;
                }
                this.ResourceChanged = null;
            }
        }

        private void InitCmsMaps(IMapGroup group)
        {
            foreach (var map in group.Map)
            {
                var opts = map.CmsMapOptions;
                if (opts != null && !_cmsMaps.ContainsKey(opts.Type))
                {
                    _cmsMaps[opts.Type] = new CmsMap(map) { IsEnabled = true };
                }
            }

            //Check for maps unaccounted for, these will be disabled
            if (!_cmsMaps.ContainsKey(G_HYBRID_MAP))
                _cmsMaps[G_HYBRID_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Hybrid", G_HYBRID_MAP)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(G_NORMAL_MAP))
                _cmsMaps[G_NORMAL_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Streets", G_NORMAL_MAP)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(G_SATELLITE_MAP))
                _cmsMaps[G_SATELLITE_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Satellite", G_SATELLITE_MAP)) { IsEnabled = false };
            if (!_cmsMaps.ContainsKey(G_PHYSICAL_MAP))
                _cmsMaps[G_PHYSICAL_MAP] = new CmsMap(group.CreateCmsMapEntry(Type_Google, true, "Google Physical", G_PHYSICAL_MAP)) { IsEnabled = false };
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

            OnResourceChanged();
        }

        private void btnBrowseMdf_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edsvc.ResourceService, ResourceTypes.MapDefinition, ResourcePickerMode.OpenResource))
            {
                if (LastSelectedFolder.IsSet)
                    picker.SetStartingPoint(LastSelectedFolder.FolderId);

                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    txtMapDefinition.Text = picker.ResourceID;
                    OnResourceChanged();
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

            OnResourceChanged();
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
            if (_noEvents)
                return;

            if (chkBingStreets.Checked)
                _appDef.SetValue("VirtualEarthScript", BING_URL);
            SetCmsAvailability(BING_ROAD, chkBingStreets.Checked);
        }

        private void chkBingSatellite_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkBingSatellite.Checked)
                _appDef.SetValue("VirtualEarthScript", BING_URL);
            SetCmsAvailability(BING_AERIAL, chkBingSatellite.Checked);
        }

        private void chkBingHybrid_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkBingHybrid.Checked)
                _appDef.SetValue("VirtualEarthScript", BING_URL);
            SetCmsAvailability(BING_HYBRID, chkBingHybrid.Checked);
        }

        private void chkGoogStreets_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkGoogStreets.Checked)
                _appDef.SetValue("GoogleScript", GOOGLE_URL);
            SetCmsAvailability(G_NORMAL_MAP, chkGoogStreets.Checked);
        }

        private void chkGoogSatellite_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkGoogSatellite.Checked)
                _appDef.SetValue("GoogleScript", GOOGLE_URL);
            SetCmsAvailability(G_SATELLITE_MAP, chkGoogSatellite.Checked);
        }

        private void chkGoogHybrid_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkGoogHybrid.Checked)
                _appDef.SetValue("GoogleScript", GOOGLE_URL);
            SetCmsAvailability(G_HYBRID_MAP, chkGoogHybrid.Checked);
        }

        private void chkGoogTerrain_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            if (chkGoogTerrain.Checked)
                _appDef.SetValue("GoogleScript", GOOGLE_URL);
            SetCmsAvailability(G_PHYSICAL_MAP, chkGoogTerrain.Checked);
        }

        private void chkYahooStreets_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            SetCmsAvailability(YAHOO_MAP_REG, chkYahooStreets.Checked);
            EvaluateCmsStates();
        }

        private void chkYahooSatellite_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            SetCmsAvailability(YAHOO_MAP_SAT, chkYahooSatellite.Checked);
            EvaluateCmsStates();
        }

        private void chkYahooHybrid_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            SetCmsAvailability(YAHOO_MAP_HYB, chkYahooHybrid.Checked);
            EvaluateCmsStates();
        }

        private void EvaluateCmsStates()
        {
            txtYahooApiKey.Enabled = IsYahooMapsEnabled();
        }

        private bool IsGoogleMapsEnabled()
        {
            return chkGoogHybrid.Checked ||
                   chkGoogSatellite.Checked ||
                   chkGoogStreets.Checked ||
                   chkGoogTerrain.Checked;
        }

        private bool IsYahooMapsEnabled()
        {
            return chkYahooHybrid.Checked ||
                   chkYahooSatellite.Checked ||
                   chkYahooStreets.Checked;
        }

        private void txtYahooApiKey_TextChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            _appDef.SetValue("YahooScript", YAHOO_URL + txtYahooApiKey.Text);
        }

        const string GOOGLE_URL = "http://maps.google.com/maps/api/js?sensor=false";
        const string YAHOO_URL = "http://api.maps.yahoo.com/ajaxymap?v=3.0&amp;appid=";
        const string BING_URL = "http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2";

        private void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;
    }
}
