#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Editors.FeatureSource
{
    /// <summary>
    /// A dialog that displays the spatial contexts of a feature source
    /// </summary>
    public partial class SpatialContextsDialog : Form
    {
        private SpatialContextsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialContextsDialog"/> class.
        /// </summary>
        /// <param name="fs">The fs.</param>
        public SpatialContextsDialog(IFeatureSource fs)
            : this()
        {
            lblFeatureSource.Text = fs.ResourceID;
            grdSpatialContexts.DataSource = fs.GetSpatialInfo(false).SpatialContext;
            lblCount.Text = string.Format(Strings.SpatialContextsFound, grdSpatialContexts.Rows.Count);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialContextsDialog"/> class.
        /// </summary>
        /// <param name="fsId">The fs id.</param>
        /// <param name="featSvc">The feat SVC.</param>
        public SpatialContextsDialog(string fsId, IFeatureService featSvc)
            : this()
        {
            lblFeatureSource.Text = fsId;
            grdSpatialContexts.DataSource = featSvc.GetSpatialContextInfo(fsId, false).SpatialContext;
            lblCount.Text = string.Format(Strings.SpatialContextsFound, grdSpatialContexts.Rows.Count);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
