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
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI;
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI.Schema;

namespace Maestro.Editors.FeatureSource.Extensions
{
    [ToolboxItem(false)]
    internal partial class JoinSettings : UserControl, IEditorBindable
    {
        private JoinSettings()
        {
            InitializeComponent();
            _propertyJoins = new BindingList<IRelateProperty>();
            grdJoinKeys.DataSource = _propertyJoins;
        }

        private IAttributeRelation _rel;

        private string _primaryFeatureSource;
        private string _primaryClass;
        private string[] _secondaryClasses;
        private string _secondaryClass;

        private BindingList<IRelateProperty> _propertyJoins;

        public JoinSettings(string primaryFeatureSource, string primaryClass, IAttributeRelation rel)
            : this()
        {
            Check.NotNull(rel, "rel"); //NOXLATE
            Check.NotNull(primaryClass, "primaryClass"); //NOXLATE
            Check.NotNull(primaryFeatureSource, "primaryFeatureSource"); //NOXLATE
            _primaryFeatureSource = primaryFeatureSource;
            _primaryClass = primaryClass;

            grdJoinKeys.AutoGenerateColumns = false;
            _rel = rel;
        }

        void OnPropertyJoinListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            { 
                case ListChangedType.ItemAdded:
                    _rel.AddRelateProperty(_propertyJoins[e.NewIndex]);
                    break;
                case ListChangedType.Reset:
                    _rel.RemoveAllRelateProperties();
                    break;
            }
        }

        private void UpdateJoinKeyList(IAttributeRelation rel)
        {
            grdJoinKeys.DataSource = new List<IRelateProperty>(rel.RelateProperty);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string resId = _edSvc.SelectResource(OSGeo.MapGuide.MaestroAPI.ResourceTypes.FeatureSource);
            if (!string.IsNullOrEmpty(resId))
            {
                txtFeatureSource.Text = resId;
                _secondaryClasses = _edSvc.FeatureService.GetClassNames(txtFeatureSource.Text, null);
                //Invalidate existing secondary class
                txtSecondaryClass.Text = string.Empty;
                _secondaryClass = null;
                //Clear existing property joins
                ClearPropertyJoins();
                CheckAddStatus();
            }
        }

        private void btnBrowseSecondaryClass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFeatureSource.Text))
            {
                MessageBox.Show(Strings.SpecifySecondaryFeatureSource);
                return;
            }

            var selClass = GenericItemSelectionDialog.SelectItem(Strings.SelectFeatureClass, Strings.SelectFeatureClass, _secondaryClasses, "QualifiedName", "QualifiedName"); //NOXLATE
            if (selClass != null)
            {
                _secondaryClass = selClass;
                txtSecondaryClass.Text = _secondaryClass;
                CheckAddStatus();
            }
        }

        private void ClearPropertyJoins()
        {
            _propertyJoins.Clear();
        }

        private IEditorService _edSvc;

        public void Bind(IEditorService service)
        {
            _edSvc = service;
            _edSvc.RegisterCustomNotifier(this);

            TextBoxBinder.BindText(txtJoinName, _rel, "Name"); //NOXLATE
            TextBoxBinder.BindText(txtFeatureSource, _rel, "ResourceId"); //NOXLATE
            TextBoxBinder.BindText(txtSecondaryClass, _rel, "AttributeClass"); //NOXLATE

            //Init selected classes
            if (!string.IsNullOrEmpty(_rel.ResourceId))
            {
                _secondaryClasses = _edSvc.FeatureService.GetClassNames(_rel.ResourceId, null);

                if (!string.IsNullOrEmpty(_rel.AttributeClass))
                {
                    foreach (var cls in _secondaryClasses)
                    {
                        if (cls.Equals(_rel.AttributeClass))
                        {
                            _secondaryClass = cls;
                            break;
                        }
                    }
                }
            }

            //CheckBoxBinder.BindChecked(chkForceOneToOne, _rel, "ForceOneToOne");
            //CheckBoxBinder is failing me here, so let's do it manually
            chkForceOneToOne.Checked = _rel.ForceOneToOne;
            chkForceOneToOne.CheckedChanged += (s, e) => { _rel.ForceOneToOne = chkForceOneToOne.Checked; };

            switch (_rel.RelateType)
            {
                case RelateTypeEnum.Association:
                    rdAssociation.Checked = true;
                    break;
                case RelateTypeEnum.Inner:
                    rdInner.Checked = true;
                    break;
                case RelateTypeEnum.LeftOuter:
                    rdLeftOuter.Checked = true;
                    break;
                case RelateTypeEnum.RightOuter:
                    rdRightOuter.Checked = true;
                    break;
            }

            _rel.PropertyChanged += (sender, e) => { OnResourceChanged(); };

            foreach (var join in _rel.RelateProperty)
            {
                _propertyJoins.Add(join);
            }
            _propertyJoins.ListChanged += new ListChangedEventHandler(OnPropertyJoinListChanged);
        }

        private void OnResourceChanged()
        {
            var handler = this.ResourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler ResourceChanged;

        public RelateTypeEnum GetJoinType()
        {
            if (rdLeftOuter.Checked)
                return RelateTypeEnum.LeftOuter;
            else if (rdInner.Checked)
                return RelateTypeEnum.Inner;
            else if (rdRightOuter.Checked)
                return RelateTypeEnum.RightOuter;
            else if (rdAssociation.Checked)
                return RelateTypeEnum.Association;

            throw new InvalidOperationException();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void rdJoinTypeChanged(object sender, EventArgs e)
        {
            _rel.RelateType = GetJoinType();
        }

        private void CheckAddStatus()
        {
            btnAddKey.Enabled = !string.IsNullOrEmpty(txtFeatureSource.Text) && !string.IsNullOrEmpty(txtSecondaryClass.Text);
        }

        private void grdJoinKeys_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                grdJoinKeys.Rows[e.RowIndex].Selected = true;
                btnDeleteKey.Enabled = true;
            }
        }

        private void btnAddKey_Click(object sender, EventArgs e)
        {
            if (_primaryClass != null && _secondaryClass != null)
            {
                var pc = _edSvc.FeatureService.GetClassDefinition(_primaryFeatureSource, _primaryClass);
                var sc = _edSvc.FeatureService.GetClassDefinition(_rel.ResourceId, _secondaryClass);
                
                var dlg = new SelectJoinKeyDialog(pc, sc);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var rel = _rel.CreatePropertyJoin(dlg.PrimaryProperty, dlg.SecondaryProperty);
                    _propertyJoins.Add(rel);
                }
            }
        }

        private void btnDeleteKey_Click(object sender, EventArgs e)
        {
            if (grdJoinKeys.SelectedRows.Count == 1)
            {
                var join = (IRelateProperty)grdJoinKeys.SelectedRows[0].DataBoundItem;
                _rel.RemoveRelateProperty(join);
                _propertyJoins.Remove(join);
            }
        }
    }
}
