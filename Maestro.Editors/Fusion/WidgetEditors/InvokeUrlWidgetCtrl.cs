using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using System.Xml;

namespace Maestro.Editors.Fusion.WidgetEditors
{
    public partial class InvokeUrlWidgetCtrl : UserControl, IWidgetEditor
    {
        class Pair
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        private BindingList<Pair> _params;

        public InvokeUrlWidgetCtrl()
        {
            InitializeComponent();
            _doc = new XmlDocument();
            _params = new BindingList<Pair>();
            grdParams.DataSource = _params;
        }

        private IWidget _widget;
        private XmlDocument _doc;

        public void Setup(IWidget widget)
        {
            _widget = widget;
            baseEditor.Setup(_widget);

            txtTarget.Text = _widget.GetValue("Target");
            txtUrl.Text = _widget.GetValue("Url");
            chkDisableEmpty.Checked = Convert.ToBoolean(_widget.GetValue("DisableIfSelectionEmpty"));

            //Would be nice if this particular bit was documented
            _doc.LoadXml(_widget.GetValue("AdditionalParameter"));
        }

        public Control Content
        {
            get { return this; }
        }
    }
}
