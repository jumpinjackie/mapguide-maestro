using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
    public partial class FeatureSourceEditorSQLite : ResourceEditors.FilebasedFeatureSourceEditor
    {
        public FeatureSourceEditorSQLite(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
            : base(editor, feature, false, "File", GetFileTypes())
        {
        }

        private static System.Collections.Specialized.NameValueCollection GetFileTypes()
        {
			System.Collections.Specialized.NameValueCollection nv = new System.Collections.Specialized.NameValueCollection();
			nv.Add(".sqlite;*.sqlite3", "SQLite Files (*.sqlite;*.sqlite3)");
			nv.Add("", "All files (*.*)");
            return nv;
        }
    }
}
