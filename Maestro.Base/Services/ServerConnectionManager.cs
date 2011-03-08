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
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using ICSharpCode.Core;
using System.ComponentModel;

namespace Maestro.Base.Services
{
    public delegate void ServerConnectionEventHandler(object sender, string name);
    public delegate void ServerConnectionRemovingEventHandler(object sender, ServerConnectionRemovingEventArgs e);

    public class ServerConnectionRemovingEventArgs : CancelEventArgs
    {
        public ServerConnectionRemovingEventArgs(string name)
        {
            this.ConnectionName = name;
            base.Cancel = false;
        }

        public string ConnectionName { get; set; }
    }

    public class ServerConnectionManager : ServiceBase
    {
        public event ServerConnectionEventHandler ConnectionAdded;
        public event ServerConnectionRemovingEventHandler ConnectionRemoving;
        public event ServerConnectionEventHandler ConnectionRemoved;

        private Dictionary<string, IServerConnection> _connections = new Dictionary<string, IServerConnection>();

        public ICollection<string> GetConnectionNames()
        {
            return _connections.Keys;
        }

        public override void Initialize()
        {
            base.Initialize();
            LoggingService.Info(Properties.Resources.Service_Init_Server_Connection_Manager);
        }

        public IServerConnection GetConnection(string name)
        {
            if (_connections.ContainsKey(name))
                return _connections[name];

            return null;
        }

        public void AddConnection(string name, IServerConnection conn)
        {
            _connections.Add(name, conn);
            var handler = this.ConnectionAdded;
            if (handler != null)
                handler(this, name);
        }

        public IServerConnection RemoveConnection(string name)
        {
            if (_connections.ContainsKey(name))
            {
                var removing = this.ConnectionRemoving;
                var ce = new ServerConnectionRemovingEventArgs(name);
                if (removing != null)
                    removing(this, ce);

                if (ce.Cancel)
                    return null;

                IServerConnection conn = _connections[name];
                _connections.Remove(name);

                var removed = this.ConnectionRemoved;
                if (removed != null)
                    removed(this, name);
                return conn;
            }
            return null;
        }
    }
}
