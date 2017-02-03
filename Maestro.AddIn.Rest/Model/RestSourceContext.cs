#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License
using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace Maestro.AddIn.Rest.Model
{
    public class RestSource
    {
        public string FeatureSource { get; set; }

        public string ClassName { get; set; }
    }

    public class RestSourceContext
    {
        private RestSource _source;
        private readonly IServerConnection _conn;
        private ClassDefinition _cls;

        public RestSourceContext(IServerConnection conn, RestSource source)
        {
            _conn = conn;
            _source = source;
        }

        private ClassDefinition GetClass()
        {
            if (_cls == null)
                _cls = _conn.FeatureService.GetClassDefinition(_source.FeatureSource, _source.ClassName);

            return _cls;
        }

        internal IList<string> GetPropertyNames()
        {
            var list = new List<string>();
            var cls = GetClass();
            list.AddRange(cls.Properties.Select(p => p.Name));
            return list;
        }

        internal string EditExpression(string expr)
        {
            var editor = FdoExpressionEditorFactory.Create();
            editor.Initialize(_conn, GetClass(), _source.FeatureSource, ExpressionEditorMode.Expression);
            if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return editor.Expression;
            }
            return null;
        }

        internal string PickCoordinateSystemCode()
        {
            CoordinateSystemPicker picker = new CoordinateSystemPicker(_conn.CoordinateSystemCatalog);
            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var cs = picker.SelectedCoordSys;
                return cs.Code;
            }
            return null;
        }

        internal IList<string> GetGroups()
        {
            var list = new List<string>();

            ISiteService site = (ISiteService)_conn.GetService((int)ServiceType.Site);
            list.AddRange(site.EnumerateGroups().Group.Select(g => g.Name));

            return list;
        }
    }
}
