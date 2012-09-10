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
using System.ComponentModel;

namespace Maestro.Editors.FeatureSource.Providers.Odbc.Drivers
{
    /// <summary>
    /// SQL Server ODBC connection string builder
    /// </summary>
    public class SqlServerDriver : OdbcDriverInfo
    {
        /// <summary>
        /// The SQL Server Name
        /// </summary>
        [Description("The SQL Server Name")] //NOXLATE
        public string ServerName { get; set; }

        /// <summary>
        /// The SQL Server database
        /// </summary>
        [Description("The SQL Server database")] //NOXLATE
        public string Database { get; set; }

        /// <summary>
        /// The user name
        /// </summary>
        [Description("Username")] //NOXLATE
        public string UserName { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        [Description("Password")] //NOXLATE
        public string Password { get; set; }

        /// <summary>
        /// Gets the built connection string
        /// </summary>
        public override string OdbcConnectionString
        {
            get
            {
                var builder = new System.Data.Odbc.OdbcConnectionStringBuilder();

                return builder.ToString();
            }
            set
            {
                var builder = new System.Data.Odbc.OdbcConnectionStringBuilder(value);

            }
        }
    }
}
