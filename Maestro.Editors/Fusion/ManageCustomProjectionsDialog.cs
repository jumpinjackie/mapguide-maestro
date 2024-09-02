#region Disclaimer / License

// Copyright (C) 2023, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Maestro.Editors.Fusion
{
    public partial class ManageCustomProjectionsDialog : Form
    {
        private ManageCustomProjectionsDialog()
        {
            InitializeComponent();
        }

        readonly IServerConnection _conn;
        readonly IApplicationDefinition _appDef;

        public ManageCustomProjectionsDialog(IServerConnection conn, IApplicationDefinition appDef)
            : this()
        {
            _conn = conn;
            _appDef = appDef;
        }

        public CustomProjections ProjectionSet { get; set; }

        private BindingList<CustomProjectionEntry> _items = new BindingList<CustomProjectionEntry>();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var ser = new XmlSerializer(typeof(CustomProjections));
            var el = _appDef.GetValue("CustomProjections", ExtensionElementContentKind.OuterXml);
            try
            {
                if (!string.IsNullOrEmpty(el))
                {
                    using var sr = new StringReader(el);
                    this.ProjectionSet = (CustomProjections)ser.Deserialize(sr);
                }
            }
            catch { }
            finally
            {
                if (this.ProjectionSet == null)
                {
                    this.ProjectionSet = new CustomProjections()
                    {
                        Projection = Array.Empty<CustomProjectionEntry>()
                    };
                }
            }

            foreach (var it in this.ProjectionSet.Projection)
            {
                _items.Add(it);
            }
            grdProjections.DataSource = _items;
        }

        private CustomProjectionEntry[] GetEntries(IEnumerable<string> resolveEpsgs)
        {
            // Collect epsg codes of all referenced map definitions
            var epsgToResolve = new List<string>(resolveEpsgs);
            if (epsgToResolve.Count == 0)
            {
                var maps = _appDef.MapSet?.MapGroups?.SelectMany(mg => mg.Map) ?? Enumerable.Empty<IMap>();
                foreach (var map in maps)
                {
                    if (map.Type.ToLower() == "mapguide") //NOXLATE
                    {
                        var mdfId = map.GetMapDefinition();
                        IMapDefinition mdf = (IMapDefinition)_conn.ResourceService.GetResource(mdfId);
                        if (!string.IsNullOrEmpty(mdf.CoordinateSystem))
                        {
                            var epsg = _conn.CoordinateSystemCatalog.ConvertWktToEpsgCode(mdf.CoordinateSystem);
                            if (!string.IsNullOrEmpty(epsg) && epsg.All(Char.IsDigit))
                            {
                                // 4326 and 3857 definitions are built-in, we don't need to fetch such definitions
                                if (epsg != "4326" && epsg != "3857")
                                {
                                    epsgToResolve.Add(epsg);
                                }
                            }
                        }
                    }
                }
            }
            var entries = new List<CustomProjectionEntry>();
            var http = ConnectionProviderRegistry.CreateHttpRequestor();

            foreach (var epsg in epsgToResolve)
            {
                // Hit epsg.io
                var s = http.SyncFetch(epsg);
                if (s.Results.Length > 0)
                {
                    var res = s.Results[0];
                    if (!string.IsNullOrEmpty(res.Proj4))
                        entries.Add(new CustomProjectionEntry { epsg = Convert.ToUInt16(epsg), Value = res.Proj4 });
                }
            }

            return entries.ToArray();
        }



        private void btnAddAll_Click(object sender, System.EventArgs e)
        {
            BusyWaitDialog.Run(Strings.AddingCustomProjections, () => //Worker
            {
                return GetEntries(Enumerable.Empty<string>());
            }, (res, ex) => //On completed
            {
                var entries = (CustomProjectionEntry[])res;
                foreach (var ent in entries)
                {
                    var existing = _items.FirstOrDefault(e => e.epsg == ent.epsg);
                    if (existing != null) // Update its proj4 defn
                        existing.Value = ent.Value;
                    else // Otherwise add it
                        _items.Add(ent);
                }
            });
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnApplyAndClose_Click(object sender, System.EventArgs e)
        {
            this.ProjectionSet.Projection = _items.Where(ent => !string.IsNullOrEmpty(ent.Value)).ToArray();
            this.DialogResult = DialogResult.OK;
        }

        private void grdProjections_SelectionChanged(object sender, EventArgs e)
        {
            btnGetDefn.Enabled = grdProjections.SelectedRows.Count > 0;
        }

        private void btnGetDefn_Click(object sender, EventArgs e)
        {
            grdProjections.EndEdit();
            var toUpdate = new List<string>();
            foreach (DataGridViewRow row in grdProjections.SelectedRows)
            {
                var ent = (CustomProjectionEntry)row.DataBoundItem;
                toUpdate.Add($"{ent.epsg}");
            }
            if (toUpdate.Count > 0)
            {
                BusyWaitDialog.Run(Strings.AddingCustomProjections, () => //Worker
                {
                    return GetEntries(toUpdate);
                }, (res, ex) => //On completed
                {
                    var entries = (CustomProjectionEntry[])res;
                    foreach (var ent in entries)
                    {
                        var existing = _items.FirstOrDefault(e => e.epsg == ent.epsg);
                        if (existing != null) // Update its proj4 defn
                            existing.Value = ent.Value;
                        else // Otherwise add it
                            _items.Add(ent);
                    }
                });
            }
        }
    }
}
