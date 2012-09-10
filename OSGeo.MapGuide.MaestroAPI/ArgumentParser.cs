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
using System.Collections.Specialized;

namespace OSGeo.MapGuide.MaestroAPI
{
    /// <summary>
    /// A helper class to parse command-line arguments
    /// </summary>
    /// <remarks>
    /// Command-line arguments use the following format:
    /// 
    /// -name1 -name2[:value2] -name3[:value3]
    /// </remarks>
    public class ArgumentParser
    {
        private Dictionary<string, string> _values;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="args"></param>
        public ArgumentParser(string[] args)
        {
            _values = new Dictionary<string, string>();

            foreach (var arg in args)
            {
                if (arg.Length > 0 && arg[0] == '-') //NOXLATE
                {
                    string name = arg.Substring(1);
                    string value = string.Empty;
                    var cidx = arg.IndexOf(':'); //NOXLATE

                    if (cidx >= 0)
                    {
                        name = arg.Substring(1, cidx - 1);
                        value = arg.Substring(cidx + 1);
                    }

                    _values[name] = value;
                }
            }
        }

        /// <summary>
        /// Gets a collection of name-value pairs of arguments with values
        /// </summary>
        /// <returns></returns>
        public NameValueCollection GetAllArgumentsWithValues()
        {
            var nvc = new NameValueCollection();

            foreach (var key in _values.Keys)
            {
                if (!string.IsNullOrEmpty(_values[key]))
                    nvc[key] = _values[key];
            }

            return nvc;
        }

        /// <summary>
        /// Gets whether the particular switch has been defined
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsDefined(string name)
        {
            return _values.ContainsKey(name);
        }

        /// <summary>
        /// Gets the value of the specified switch
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            return _values[name];
        }
    }

    /// <summary>
    /// A set of common command-line arguments
    /// </summary>
    public static class CommandLineArguments
    {
        /// <summary>
        /// The name of the API provider
        /// </summary>
        public const string Provider = "Provider"; //NOXLATE

        /// <summary>
        /// The session id
        /// </summary>
        public const string Session = "SessionId"; //NOXLATE

        /// <summary>
        /// The username
        /// </summary>
        public const string Username = "Username"; //NOXLATE

        /// <summary>
        /// The password
        /// </summary>
        public const string Password = "Password"; //NOXLATE
    }
}
