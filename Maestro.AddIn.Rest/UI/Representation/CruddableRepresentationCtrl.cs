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
using Maestro.AddIn.Rest.Model;
using System;
using System.Dynamic;
using System.Windows.Forms;

namespace Maestro.AddIn.Rest.UI.Representation
{
    internal partial class CruddableRepresentationCtrl : UserControl, IRepresentationCtrl
    {
        private string _type;

        public CruddableRepresentationCtrl(string type, RestSourceContext context)
        {
            InitializeComponent();
            getMethodCtrl1.Init(context);
            getPermsCtrl.Init(context);
            postPermsCtrl.Init(context);
            putPermsCtrl.Init(context);
            deletePermsCtrl.Init(context);
            this.CheckStates();
            switch(type.ToLower())
            {
                case "xml":
                    _type = "FeatureSetXml";
                    break;
                case "geojson":
                    _type = "FeatureSetGeoJson";
                    break;
            }
        }

        public dynamic GetOptions()
        {
            dynamic opts = new ExpandoObject();
            opts.Adapter = _type;
            opts.Methods = new ExpandoObject();
            if (chkGet.Checked)
            {
                var getOpts = getMethodCtrl1.WriteOptions();
                getPermsCtrl.UpdateConfiguration(getOpts);
                opts.Methods.GET = getOpts;
            }
            if (chkPost.Checked)
            {
                var postOpts = new ExpandoObject();
                postPermsCtrl.UpdateConfiguration(postOpts);
                opts.Methods.POST = postOpts;
            }
            if (chkPut.Checked)
            {
                var putOpts = new ExpandoObject();
                putPermsCtrl.UpdateConfiguration(putOpts);
                opts.Methods.PUT = putOpts;
            }
            if (chkDelete.Checked)
            {
                var deleteOpts = new ExpandoObject();
                deletePermsCtrl.UpdateConfiguration(deleteOpts);
                opts.Methods.DELETE = deleteOpts;
            }

            return opts;
        }

        private void CheckStates()
        {
            tabs.TabPages.Clear();

            if (chkGet.Checked)
                tabs.TabPages.Add(TAB_GET);

            if (chkPost.Checked)
                tabs.TabPages.Add(TAB_POST);

            if (chkPut.Checked)
                tabs.TabPages.Add(TAB_PUT);

            if (chkDelete.Checked)
                tabs.TabPages.Add(TAB_DELETE);
        }

        private void chkGet_CheckedChanged(object sender, EventArgs e) => this.CheckStates();

        private void chkPost_CheckedChanged(object sender, EventArgs e) => this.CheckStates();

        private void chkPut_CheckedChanged(object sender, EventArgs e) => this.CheckStates();

        private void chkDelete_CheckedChanged(object sender, EventArgs e) => this.CheckStates();
    }
}
