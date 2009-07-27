using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    public partial class FeatureSourceEditorSDF : ResourceEditors.FilebasedFeatureSourceEditor
    {
        public FeatureSourceEditorSDF(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
            : base(editor, feature, true, "File", GetFileTypes())
        {
        }

        private static System.Collections.Specialized.NameValueCollection GetFileTypes()
        {
			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".sdf", "SDF Files (*.sdf)");
			nv.Add("", "All files (*.*)");
            return nv;
        }
    }
}
