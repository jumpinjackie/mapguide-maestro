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
using OSGeo.MapGuide.ObjectModels.WebLayout;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using Maestro.Editors.Common;

namespace Maestro.Editors.WebLayout.Commands
{
    [ToolboxItem(false)]
    internal partial class InvokeURLCtrl : EditorBase
    {
        public InvokeURLCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edsvc;
        private IInvokeUrlCommand _cmd;

        public override void Bind(IEditorService service)
        {
            _edsvc = service;
        }

        private BindingList<IParameterPair> _params = new BindingList<IParameterPair>();

        internal void Bind(IInvokeUrlCommand invokeUrlCommandType, IEditorService service)
        {
            cmbTargetFrame.DataSource = Enum.GetValues(typeof(TargetType));
            Bind(service);
            service.RegisterCustomNotifier(this);

            TextBoxBinder.BindText(txtUrl, invokeUrlCommandType, "URL");
            TextBoxBinder.BindText(txtFrame, invokeUrlCommandType, "TargetFrame");
            ComboBoxBinder.BindSelectedIndexChanged(cmbTargetFrame, "SelectedItem", invokeUrlCommandType, "Target");
            CheckBoxBinder.BindChecked(chkDisableIfEmpty, invokeUrlCommandType, "DisableIfSelectionEmpty");

            foreach (var p in invokeUrlCommandType.AdditionalParameter)
            {
                _params.Add(p);
            }
            grdParameters.DataSource = _params;
            _params.ListChanged += OnParamsListChanged;
            _params.AddingNew += OnAddingNew;
            lstLayers.DataSource = invokeUrlCommandType.LayerSet.Layer;

            invokeUrlCommandType.PropertyChanged += OnCommandPropertyChanged;
            _cmd = invokeUrlCommandType;
        }

        void OnAddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = _cmd.CreateParameter("", "");
        }

        void OnParamsListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    _cmd.AddParameter(_params[e.NewIndex]);
                    break;
            }
        }

        protected override void UnsubscribeEventHandlers()
        {
            if (_cmd != null)
                _cmd.PropertyChanged -= OnCommandPropertyChanged;

            _params.ListChanged -= OnParamsListChanged;
            _params.AddingNew -= OnAddingNew;

            base.UnsubscribeEventHandlers();
        }

        void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnResourceChanged();
        }

        private void btnBrowseLayers_Click(object sender, EventArgs e)
        {
            var wl = (IWebLayout)_edsvc.GetEditedResource();
            if (wl.Map == null || string.IsNullOrEmpty(wl.Map.ResourceId))
            {
                MessageBox.Show(Strings.InvokeUrlNoMapDefined);
                return;
            }

            var mdf = (IMapDefinition)_edsvc.ResourceService.GetResource(wl.Map.ResourceId);
            List<string> layers = new List<string>();
            foreach (var lyr in mdf.MapLayer)
            {
                layers.Add(lyr.Name);
            }
            var selLayers = GenericItemSelectionDialog.SelectItems(Strings.SelectLayer, Strings.SelectLayer, layers.ToArray());
            if (selLayers.Length > 0)
            {
                _cmd.LayerSet.Layer.Clear();
                foreach (var val in selLayers)
                {
                    _cmd.LayerSet.Layer.Add(val);
                }
                OnResourceChanged();
            }
        }
    }
}
