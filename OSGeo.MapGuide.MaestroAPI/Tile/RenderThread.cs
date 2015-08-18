#region Disclaimer / License

// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

using OSGeo.MapGuide.MaestroAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    internal class RenderThreads : IDisposable
    {
        private class EventPassing
        {
            public enum EventType
            {
                Begin,
                Finish,
                Error
            }

            public EventType Type { get; }
            public int Col { get; }
            public int Row { get; }
            public Exception Exception { get; }
            public EventWaitHandle Event { get; }
            public object Result { get; set; }

            public EventPassing(EventType type, int row, int col, Exception ex, EventWaitHandle @event)
            {
                this.Type = type;
                this.Event = @event;
                this.Col = col;
                this.Row = row;
                this.Exception = ex;
            }
        }

        public Queue<KeyValuePair<int, int>> TileSet { get; }

        private readonly Queue<EventPassing> _raiseEvents = new Queue<EventPassing>();
        private readonly object _syncLock;
        private AutoResetEvent _event;
        private readonly TilingRunCollection _parent;
        private readonly int _scale;
        private readonly string _group;
        private readonly string _mapDefinition;
        private readonly MapTilingConfiguration _invoker;
        private readonly bool _randomize;
        private readonly int _rows;
        private readonly int _cols;
        private readonly int _rowOffset;
        private readonly int _colOffset;

        private bool _fillerComplete = false;
        private int _completeFlag;

        public RenderThreads(MapTilingConfiguration invoker, TilingRunCollection parent, int scale, string group, string mapdef, int rows, int cols, int rowOffset, int colOffset, bool randomize)
        {
            this.TileSet = new Queue<KeyValuePair<int, int>>();
            _syncLock = new object();
            _event = new AutoResetEvent(false);
            _completeFlag = parent.Config.ThreadCount;
            _raiseEvents = new Queue<EventPassing>();
            _scale = scale;
            _group = group;
            _parent = parent;
            _mapDefinition = mapdef;
            _invoker = invoker;
            _randomize = randomize;
            _rows = rows;
            _cols = cols;
            _rowOffset = rowOffset;
            _colOffset = colOffset;
        }

        ~RenderThreads()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _event?.Dispose();
                _event = null;
            }
        }

        public void RunAndWait()
        {
            ThreadPool.QueueUserWorkItem(QueueFiller);
            for (int i = 0; i < _parent.Config.ThreadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(ThreadRender);
            }

            bool completed = false;
            while (!completed)
            {
                EventPassing eventToRaise = null;
                while (true)
                {
                    lock (_syncLock)
                    {
                        if (_raiseEvents.Count > 0)
                            eventToRaise = _raiseEvents.Dequeue();
                    }

                    if (eventToRaise == null)
                    {
                        //No more events
                        break;
                    }
                    else
                    {
                        switch (eventToRaise.Type)
                        {
                            case EventPassing.EventType.Begin:
                                _parent.InvokeBeginRendering(
                                    _invoker,
                                    _group,
                                    _scale,
                                    eventToRaise.Row,
                                    eventToRaise.Col);
                                break;

                            case EventPassing.EventType.Finish:
                                _parent.InvokeFinishRendering(
                                    _invoker,
                                    _group,
                                    _scale,
                                    eventToRaise.Row,
                                    eventToRaise.Col);
                                break;

                            case EventPassing.EventType.Error:
                                eventToRaise.Result = _parent.InvokeError(
                                    _invoker,
                                    _group,
                                    _scale,
                                    eventToRaise.Row,
                                    eventToRaise.Col,
                                    eventToRaise.Exception);
                                break;

                            default:
                                //Not translated, because it is an internal error that should never happen
                                throw new Exception("Bad event type"); //NOXLATE
                        }
                        eventToRaise.Event.Set();
                        eventToRaise = null;
                    }
                }

                lock (_syncLock)
                {
                    if (_completeFlag == 0 && _raiseEvents.Count == 0)
                        completed = true;
                }

                if (!completed)
                    _event.WaitOne(5 * 1000, true);
            }
        }

        /// <summary>
        /// Helper that fills the queue from a thread
        /// </summary>
        /// <param name="dummy">Unused parameter</param>
        private void QueueFiller(object dummy)
        {
            try
            {
                if (_randomize)
                {
                    Random ra = new Random();
                    List<int> rows = new List<int>();
                    int[] cols_full = new int[_cols];
                    for (int i = 0; i < _rows; i++)
                    {
                        rows.Add(i);
                    }

                    for (int i = 0; i < _cols; i++)
                    {
                        cols_full[i] = i;
                    }

                    //TODO: This is not really random, because we select
                    //a row, and then random columns

                    //Unfortunately, I have yet to find a truly random
                    //pair generation method that is space efficient :(

                    while (rows.Count > 0)
                    {
                        int ri = ra.Next(0, rows.Count);
                        int r = rows[ri];
                        rows.RemoveAt(ri);

                        List<int> cols = new List<int>(cols_full);

                        while (cols.Count > 0)
                        {
                            int ci = ra.Next(0, cols.Count);
                            int c = cols[ci];
                            cols.RemoveAt(ci);

                            AddPairToQueue(r + _rowOffset, c + _colOffset);
                        }
                    }
                }
                else
                {
                    //Non-random is straightforward
                    for (int r = 0; r < _rows; r++)
                    {
                        for (int c = 0; c < _cols; c++)
                        {
                            AddPairToQueue(r + _rowOffset, c + _colOffset);
                        }
                    }
                }
            }
            finally
            {
                lock (_syncLock)
                {
                    _fillerComplete = true;
                }
            }
        }

        /// <summary>
        /// Helper to add a pair to the queue, but prevents huge queue lists
        /// </summary>
        /// <param name="r">The row component of the pair</param>
        /// <param name="c">The column component of the pair</param>
        private void AddPairToQueue(int r, int c)
        {
            bool added = false;
            while (!added)
            {
                lock (_syncLock)
                {
                    if (TileSet.Count < 500) //Keep at most 500 items in queue
                    {
                        TileSet.Enqueue(new KeyValuePair<int, int>(r, c));
                        added = true;
                    }
                }

                if (!added) //Prevent CPU spinning
                    Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Invokes the render callback method
        /// </summary>
        /// <param name="dummy">Unused parameter</param>
        private void ThreadRender(object dummy)
        {
            try
            {
                //Create a copy of the connection for local usage
                IServerConnection con = _parent.Connection.Clone();
                var tileSvc = (ITileService)con.GetService((int)ServiceType.Tile);
                AutoResetEvent ev = new AutoResetEvent(false);

                while (!_parent.Cancel)
                {
                    KeyValuePair<int, int>? round = null;

                    lock (_syncLock)
                    {
                        if (TileSet.Count == 0 && _fillerComplete)
                            return; //No more data

                        if (TileSet.Count > 0)
                            round = TileSet.Dequeue();
                    }

                    if (_parent.Cancel)
                        return;

                    if (round == null) //No data, but producer is still running
                        Thread.Sleep(500);
                    else
                        RenderTile(ev, tileSvc, round.Value.Key, round.Value.Value, _scale, _group);
                }
            }
            catch { }
            finally
            {
                _completeFlag--;
                _event.Set();
            }
        }

        /// <summary>
        /// Renders a single tile
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="tileSvc"></param>
        /// <param name="row">The row index of the tile</param>
        /// <param name="col">The column index of the tile</param>
        /// <param name="scaleindex">The scale index of the tile</param>
        /// <param name="group">The name of the baselayer group</param>
        private void RenderTile(EventWaitHandle ev, ITileService tileSvc, long row, long col, int scaleindex, string group)
        {
            ev.Reset();
            lock (_syncLock)
            {
                _raiseEvents.Enqueue(new EventPassing(
                    EventPassing.EventType.Begin,
                     (int)row, (int)col, null,
                     ev
                    ));
                _event.Set();
            }
            ev.WaitOne(Timeout.Infinite, true);

            int c = _parent.Config.RetryCount;
            while (c > 0)
            {
                c--;
                try
                {
                    if (!_parent.Cancel)
                    {
                        if (_parent.Config.RenderMethod == null)
                            tileSvc.GetTile(_mapDefinition, group, (int)col, (int)row, scaleindex, "PNG").Dispose(); //NOXLATE
                        else
                            _parent.Config.RenderMethod(_mapDefinition, group, (int)col, (int)row, scaleindex);
                    }

                    break;
                }
                catch (Exception ex)
                {
                    if (c == 0)
                    {
                        Exception pex = ex;
                        ev.Reset();
                        EventPassing evobj = new EventPassing(
                            EventPassing.EventType.Error,
                                (int)row, (int)col, ex,
                                ev
                                );

                        lock (_syncLock)
                        {
                            _raiseEvents.Enqueue(evobj);
                            _event.Set();
                        }

                        ev.WaitOne(Timeout.Infinite, true);

                        if (evobj.Result == null)
                            break;

                        if (pex == evobj.Result)
                            throw;
                        else
                            throw (Exception)evobj.Result;
                    }
                }
            }

            ev.Reset();
            lock (_syncLock)
            {
                _raiseEvents.Enqueue(new EventPassing(
                    EventPassing.EventType.Finish,
                    (int)row, (int)col, null,
                    ev
                ));
                _event.Set();
            }
            ev.WaitOne(Timeout.Infinite, true);
        }
    }
}