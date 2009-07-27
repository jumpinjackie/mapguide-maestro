using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    public partial class FeatureSourceEditorSHP : ResourceEditors.FilebasedFeatureSourceEditor
    {
        public FeatureSourceEditorSHP(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
            : base(editor, feature, false, "DefaultFileLocation", GetFileTypes())
        {
        }

        private static System.Collections.Specialized.NameValueCollection GetFileTypes()
        {
			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".shp", "SHP Files (*.shp)");
			nv.Add("", "All files (*.*)");
            return nv;
        }
    }
}
