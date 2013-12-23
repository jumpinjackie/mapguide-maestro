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
using Maestro.Shared.UI;

namespace Maestro.Base.Services
{
    /// <summary>
    /// Defines a method for connection-related events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="name"></param>
    public delegate void ServerConnectionEventHandler(object sender, string name);
    /// <summary>
    /// Defines a method that handles connection pre-removal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServerConnectionRemovingEventHandler(object sender, ServerConnectionRemovingEventArgs e);

    /// <summary>
    /// Defines a cancelable event for a connection that is about to be closed
    /// </summary>
    public class ServerConnectionRemovingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the ServerConnectionRemovingEventArgs class
        /// </summary>
        /// <param name="name"></param>
        public ServerConnectionRemovingEventArgs(string name)
        {
            this.ConnectionName = name;
            base.Cancel = false;
        }

        /// <summary>
        /// Gets the name of the connection that is about to be closed
        /// </summary>
        public string ConnectionName { get; set; }
    }

    /// <summary>
    /// Manages <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/> instances
    /// </summary>
    public class ServerConnectionManager : ServiceBase
    {
        /// <summary>
        /// Raised when a connection has been added
        /// </summary>
        public event ServerConnectionEventHandler ConnectionAdded;

        /// <summary>
        /// Raised when a connection is about to be removed. Subscribers can cancel this event if required
        /// </summary>
        public event ServerConnectionRemovingEventHandler ConnectionRemoving;

        /// <summary>
        /// Raised when a connection has been removed
        /// </summary>
        public event ServerConnectionEventHandler ConnectionRemoved;

        private Dictionary<string, IServerConnection> _connections = new Dictionary<string, IServerConnection>();

        /// <summary>
        /// Gets the names of all currently open connections
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetConnectionNames()
        {
            return _connections.Keys;
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            LoggingService.Info(Strings.Service_Init_Server_Connection_Manager);
        }
        
        /// <summary>
        /// Gets the connection by its registered name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IServerConnection GetConnection(string name)
        {
            if (_connections.ContainsKey(name))
                return _connections[name];

            return null;
        }

        /// <summary>
        /// Registers a connection by a given name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conn"></param>
        public void AddConnection(string name, IServerConnection conn)
        {
            _connections.Add(name, conn);
            var handler = this.ConnectionAdded;
            if (handler != null)
                handler(this, name);
        }

        /// <summary>
        /// Removes a connection by its given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
