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

#endregion Disclaimer / License

using ICSharpCode.Core;
using Maestro.Shared.UI;
using System.Collections.Generic;
using System.IO;

namespace Maestro.Base.Services
{
    internal class DragDropHandlerService : ServiceBase
    {
        public override void Initialize()
        {
            base.Initialize();
            _handlers = new List<IDragDropHandler>();

            //Find and register drag and drop handlers
            List<IDragDropHandler> handlers = AddInTree.BuildItems<IDragDropHandler>("/Maestro/DragDropHandlers", this); //NOXLATE
            if (handlers != null)
            {
                foreach (IDragDropHandler h in handlers)
                {
                    RegisterHandler(h);
                }
            }
        }

        private List<IDragDropHandler> _handlers;

        /// <summary>
        /// Registers a drag and drop handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void RegisterHandler(IDragDropHandler handler)
        {
            _handlers.Add(handler);
        }

        /// <summary>
        /// Gets the registered handlers for this particular file type
        /// </summary>
        /// <param name="file">The file being dropped</param>
        /// <returns>A list of registered handlers</returns>
        public IList<IDragDropHandler> GetHandlersForFile(string file)
        {
            var matches = new List<IDragDropHandler>();
            string ext = Path.GetExtension(file).ToUpper();
            foreach (var h in _handlers)
            {
                if (h.CanHandleFileExtension(ext))
                    matches.Add(h);
            }
            return matches;
        }
    }
}