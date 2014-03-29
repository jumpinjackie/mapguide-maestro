using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using Props = ICSharpCode.Core.PropertyService;
using System.Windows.Forms;

namespace Maestro.Base.UI.Preferences
{
    internal partial class EditorPreferencesCtrl : UserControl, IPreferenceSheet
    {
        public EditorPreferencesCtrl()
        {
            InitializeComponent();
        }

        private void btnBrowseXsdPath_Click(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                browser.ShowNewFolderButton = true;
                if (browser.ShowDialog() == DialogResult.OK)
                {
                    txtXsdPath.Text = browser.SelectedPath;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var validate = Props.Get(ConfigProperties.ValidateOnSave, ConfigProperties.DefaultValidateOnSave);
            chkValidateOnSave.Checked = validate;

            var useLocal = Props.Get(ConfigProperties.UseLocalPreview, ConfigProperties.DefaultUseLocalPreview);
            chkUseLocalPreview.Checked = useLocal;

            var addWmd = Props.Get(ConfigProperties.AddDebugWatermark, ConfigProperties.DefaultAddDebugWatermark);
            chkAddDebugWatermark.Checked = addWmd;

            var path = Props.Get(ConfigProperties.XsdSchemaPath, ConfigProperties.DefaultXsdSchemaPath);
            txtXsdPath.Text = path;

            var useGrid = Props.Get(ConfigProperties.UseGridStyleEditor, ConfigProperties.DefaultUseGridStyleEditor);
            chkUseGridBasedStyleEditor.Checked = useGrid;

            txtPreviewLocale.Text = Props.Get(ConfigProperties.PreviewLocale, ConfigProperties.DefaultPreviewLocale);
        }

        public string Title
        {
            get { return Strings.Prefs_Editor; }
        }

        public Control ContentControl
        {
            get { return this; }
        }

        public bool ApplyChanges()
        {
            bool restart = false;

            Apply(ConfigProperties.ValidateOnSave, chkValidateOnSave.Checked);
            Apply(ConfigProperties.UseLocalPreview, chkUseLocalPreview.Checked);
            Apply(ConfigProperties.AddDebugWatermark, chkAddDebugWatermark.Checked);
            Apply(ConfigProperties.PreviewLocale, txtPreviewLocale.Text);
            Apply(ConfigProperties.UseGridStyleEditor, chkUseGridBasedStyleEditor.Checked);

            //These changes require restart
            if (Apply(ConfigProperties.XsdSchemaPath, txtXsdPath.Text))
                restart = true;

            return restart;
        }

        private bool Apply<T>(string key, T newValue)
        {
            if (Props.Get(key).Equals((object)newValue))
                return false;

            Props.Set(key, newValue);
            return true;
        }

        public void ApplyDefaults()
        {
            ConfigProperties.ApplyEditorDefaults();
        }
    }
}
