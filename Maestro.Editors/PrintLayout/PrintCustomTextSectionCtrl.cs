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

namespace Maestro.Editors.PrintLayout
{
    //[ToolboxItem(true)]
    [ToolboxItem(false)]
    internal partial class PrintCustomTextSectionCtrl : EditorBindableCollapsiblePanel
    {
        public PrintCustomTextSectionCtrl()
        {
            InitializeComponent();
            _texts = new BindingList<IText>();
            lstCustomText.DisplayMember = "Value";
            lstCustomText.DataSource = _texts;
        }

        private IPrintLayout _layout;

        private BindingList<IText> _texts;

        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _layout = (IPrintLayout)service.GetEditedResource();
            //Init current defined text
            foreach (var txt in _layout.CustomText)
            {
                if (txt.Value == null && txt.Font == null && txt.Position == null)
                    continue;

                _texts.Add(txt);
            }
            //Now wire change listeners
            _texts.ListChanged += new ListChangedEventHandler(OnTextListChanged);
        }

        void OnTextListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    {
                        _layout.AddText(_texts[e.NewIndex]);
                    }
                    break;
            }
            OnResourceChanged();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new TextDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var pos = _layout.CreateTextPosition(dlg.PositionLeft, dlg.PositionBottom, dlg.PositionUnits);
                    var fnt = _layout.CreateFont(dlg.FontName, dlg.FontHeight, dlg.FontUnits);
                    var txt = _layout.CreateText(dlg.TextString, fnt, pos);
                    _texts.Add(txt);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var txt = lstCustomText.SelectedItem as IText;
            if (txt != null)
            {
                using (var dlg = new TextDialog())
                {
                    dlg.TextString = txt.Value;
                    dlg.FontUnits = txt.Font.Units;
                    dlg.FontName = txt.Font.Name;
                    dlg.FontHeight = txt.Font.Height;
                    dlg.PositionBottom = txt.Position.Bottom;
                    dlg.PositionLeft = txt.Position.Left;
                    dlg.PositionUnits = txt.Position.Units;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        txt.Value = dlg.TextString;
                        txt.Font.Units = dlg.FontUnits;
                        txt.Font.Name = dlg.FontName;
                        txt.Font.Height = dlg.FontHeight;
                        txt.Position.Bottom = dlg.PositionBottom;
                        txt.Position.Left = dlg.PositionLeft;
                        txt.Position.Units = dlg.PositionUnits;
                        OnResourceChanged();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var txt = lstCustomText.SelectedItem as IText;
            if (txt != null)
            {
                _texts.Remove(txt);
                //Have to remove manually due to brain-dead API design
                //of BindingList<T>
                _layout.RemoveText(txt);
            }
        }

        private void lstCustomText_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnDelete.Enabled = (lstCustomText.SelectedItem != null);
        }
    }
}
