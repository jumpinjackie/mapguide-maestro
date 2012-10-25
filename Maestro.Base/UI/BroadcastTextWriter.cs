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
    /// <summary>
    /// Defines a method to log a message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="msg"></param>
    public delegate void LogBroadcastEventHandler(object sender, LogMessage msg);

    /// <summary>
    /// A TextWriter that broadcasts all messages written to it
    /// </summary>
    public class BroadcastTextWriter : TextWriter
    {
        private static BroadcastTextWriter _instance = null;

        /// <summary>
        /// Gets the current instance
        /// </summary>
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

        /// <summary>
        /// Raised when a log message is written
        /// </summary>
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

        /// <summary>
        /// Writes the specified character
        /// </summary>
        /// <param name="value"></param>
        public override void Write(char value)
        {
            Write(value.ToString());
        }

        /// <summary>
        /// Writes the specified characters
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        /// <summary>
        /// Writes the specified string
        /// </summary>
        /// <param name="value"></param>
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

        /// <summary>
        /// Writes a new line
        /// </summary>
        public override void WriteLine()
        {
            WriteLine(string.Empty);
        }

        /// <summary>
        /// Writes the specified string with a newline at the end
        /// </summary>
        /// <param name="value"></param>
        public override void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }

        /// <summary>
        /// Gets the encoding
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }

    /// <summary>
    /// An application log message
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets the log date
        /// </summary>
        public DateTime LogDate { get; set; }

        /// <summary>
        /// Gets or sets the log message
        /// </summary>
        public string Message { get; set; }
    }
}
