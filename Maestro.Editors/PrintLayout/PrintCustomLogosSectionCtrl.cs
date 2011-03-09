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
using Maestro.Shared.UI;
using Maestro.Editors.Common;
using OSGeo.MapGuide.ObjectModels.PrintLayout;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Editors.PrintLayout
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class PrintCustomLogosSectionCtrl : EditorBindableCollapsiblePanel
    {
        public PrintCustomLogosSectionCtrl()
        {
            InitializeComponent();
            _logos = new BindingList<ILogo>();
        }

        private BindingList<ILogo> _logos;

        private IPrintLayout _layout;
        private IServerConnection _conn;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _layout = (IPrintLayout)service.GetEditedResource();
            _conn = _layout.CurrentConnection;

            foreach (var logo in _layout.CustomLogos)
            {
                _logos.Add(logo);
                AddLogoToListView(logo);
            }
            _logos.ListChanged += new ListChangedEventHandler(OnLogoListChanged);
        }

        void OnLogoListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        _layout.AddLogo(_logos[e.NewIndex]);
                        AddLogoToListView(_logos[e.NewIndex]);
                    }
                    break;
            }
            OnResourceChanged();
        }

        private void AddLogoToListView(ILogo logo)
        {
            var item = new ListViewItem(logo.Name);
            item.Tag = logo;

            //TODO: It would be really nice to show the actual
            //symbol here

            lstCustomLogos.Items.Add(item);
        }

        private void RemoveLogoFromListView(ILogo logo)
        {
            ListViewItem remove = null;
            foreach (ListViewItem item in lstCustomLogos.Items)
            {
                if (item.Tag == logo)
                {
                    remove = item;
                    break;
                }
            }
            lstCustomLogos.Items.Remove(remove);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new LogoDialog(_conn))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var pos = _layout.CreateLogoPosition(dlg.PositionLeft, dlg.PositionBottom, dlg.PositionUnits);
                    var size = _layout.CreateLogoSize(dlg.SizeWidth, dlg.SizeHeight, dlg.SizeUnits);
                    var logo = _layout.CreateLogo(dlg.SymbolLibraryID, dlg.SymbolName, size, pos);
                    logo.Rotation = dlg.Rotation;

                    _logos.Add(logo);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstCustomLogos.SelectedItems.Count > 0)
            {
                var item = lstCustomLogos.SelectedItems[0];
                var logo = item.Tag as ILogo;
                if (logo != null)
                {
                    using (var dlg = new LogoDialog(_conn))
                    {
                        dlg.SymbolLibraryID = logo.ResourceId;
                        dlg.SymbolName = logo.Name;
                        dlg.PositionBottom = logo.Position.Bottom;
                        dlg.PositionLeft = logo.Position.Left;
                        dlg.PositionUnits = logo.Position.Units;
                        dlg.SizeHeight = logo.Size.Height;
                        dlg.SizeUnits = logo.Size.Units;
                        dlg.SizeWidth = logo.Size.Width;
                        dlg.Rotation = logo.Rotation.HasValue ? logo.Rotation.Value : 0.0f;

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            logo.ResourceId = dlg.SymbolLibraryID;
                            logo.Name = dlg.SymbolName;
                            logo.Position.Bottom = dlg.PositionBottom;
                            logo.Position.Left = dlg.PositionLeft;
                            logo.Position.Units = dlg.PositionUnits;
                            logo.Size.Height = dlg.SizeHeight;
                            logo.Size.Units = dlg.SizeUnits;
                            logo.Size.Width = dlg.SizeWidth;
                            logo.Rotation = dlg.Rotation;

                            item.Text = logo.Name;
                            OnResourceChanged();
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstCustomLogos.SelectedItems.Count > 0)
            {
                var item = lstCustomLogos.SelectedItems[0];
                var logo = item.Tag as ILogo;
                if (logo != null)
                {
                    _logos.Remove(logo);
                    //Have to remove manually due to brain-dead API design
                    //of BindingList<T>
                    _layout.RemoveLogo(logo);
                    RemoveLogoFromListView(logo);
                }
            }
        }

        private void lstCustomLogos_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnDelete.Enabled = (lstCustomLogos.SelectedItems.Count > 0);
        }
    }
}
