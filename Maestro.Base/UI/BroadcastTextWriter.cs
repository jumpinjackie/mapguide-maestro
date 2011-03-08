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
using System.IO;

namespace Maestro.Base.UI
{
    public delegate void LogBroadcastEventHandler(object sender, LogMessage msg);

    public class BroadcastTextWriter : TextWriter
    {
        private static BroadcastTextWriter _instance = null;

        public static BroadcastTextWriter Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new BroadcastTextWriter();
                }
                return _instance;
            }
        }

        private BroadcastTextWriter() : base() { } 

        public event LogBroadcastEventHandler LogMessage;

        private List<LogMessage> _buffered = new List<LogMessage>();

        /// <summary>
        /// Flushes any buffered messages
        /// </summary>
        public void FlushMessages()
        {
            var handler = this.LogMessage;
            if (handler != null)
            {
                //Flush buffered messages
                foreach (var msg in _buffered)
                {
                    handler(this, msg);
                }
                _buffered.Clear();
            }
        }

        public override void Write(char value)
        {
            Write(value.ToString());
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        public override void Write(string value)
        {
            var message = new LogMessage() { LogDate = DateTime.Now, Message = value };
            var handler = this.LogMessage;
            if (handler != null)
            {
                //Flush buffered messages
                foreach (var msg in _buffered)
                {
                    handler(this, msg);
                }
                _buffered.Clear();
                handler(this, message);
            }
            else
            {
                //Store in buffer
                _buffered.Add(message);
            }
        }

        public override void WriteLine()
        {
            WriteLine(string.Empty);
        }

        public override void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }

    public class LogMessage
    {
        public DateTime LogDate { get; set; }
        public string Message { get; set; }
    }
}
