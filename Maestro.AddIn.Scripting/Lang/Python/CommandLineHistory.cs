#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// Original code from SharpDevelop 3.2.1 licensed under the same terms (LGPL 2.1)
// Copyright 2002-2010 by
//
//  AlphaSierraPapa, Christoph Wille
//  Vordernberger Strasse 27/8
//  A-8700 Leoben
//  Austria
//
//  email: office@alphasierrapapa.com
//  court of jurisdiction: Landesgericht Leoben
//
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
using System.Linq;
using System.Text;

namespace Maestro.AddIn.Scripting.Lang.Python
{
    /// <summary>
    /// Stores the command line history for the PythonConsole.
    /// </summary>
    internal class CommandLineHistory
    {
        List<string> lines = new List<string>();
        int position;

        public CommandLineHistory()
        {
        }

        /// <summary>
        /// Adds the command line to the history.
        /// </summary>
        public void Add(string line)
        {
            if (!String.IsNullOrEmpty(line))
            {
                int index = lines.Count - 1;
                if (index >= 0)
                {
                    if (lines[index] != line)
                    {
                        lines.Add(line);
                    }
                }
                else
                {
                    lines.Add(line);
                }
            }
            position = lines.Count;
        }

        /// <summary>
        /// Gets the current command line. By default this will be the last command line entered.
        /// </summary>
        public string Current
        {
            get
            {
                if ((position >= 0) && (position < lines.Count))
                {
                    return lines[position];
                }
                return null;
            }
        }

        /// <summary>
        /// Moves to the next command line.
        /// </summary>
        /// <returns>False if the current position is at the end of the command line history.</returns>
        public bool MoveNext()
        {
            int nextPosition = position + 1;
            if (nextPosition < lines.Count)
            {
                ++position;
            }
            return nextPosition < lines.Count;
        }

        /// <summary>
        /// Moves to the previous command line.
        /// </summary>
        /// <returns>False if the current position is at the start of the command line history.</returns>
        public bool MovePrevious()
        {
            if (position >= 0)
            {
                if (position == 0)
                {
                    return false;
                }
                --position;
            }
            return position >= 0;
        }
    }
}
