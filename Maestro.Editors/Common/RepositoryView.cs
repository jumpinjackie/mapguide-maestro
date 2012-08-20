#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Services;

namespace Maestro.Editors.Common
{
    public partial class RepositoryView : UserControl
    {
        public RepositoryView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RepositoryIcons.PopulateImageList(resImageList);
        }

        public void NavigateTo(string folderId)
        {
            if (_model != null)
                _model.NavigateTo(folderId);
        }

        public void Init(IResourceService resSvc, bool bFoldersOnly)
        {
            if (_model != null)
            {
                _model.ItemSelected -= OnItemSelectedInternal;
            }

            _model = new RepositoryFolderTreeModel(resSvc, trvRepository, bFoldersOnly);
            _model.ItemSelected += OnItemSelectedInternal; 
        }

        void OnItemSelectedInternal(object sender, EventArgs e)
        {
            var h = this.ItemSelected;
            if (h != null)
                h(this, EventArgs.Empty);
        }

        private RepositoryFolderTreeModel _model;

        public IRepositoryItem SelectedItem
        {
            get
            {
                if (_model != null)
                {
                    var it = _model.SelectedItem;
                    if (it != null)
                        return it.Instance;
                }
                return null;
            }
        }

        public event EventHandler ItemSelected;
    }
}
