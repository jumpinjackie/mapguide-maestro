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
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using System.Threading;

namespace OSGeo.MapGuide.MaestroAPI.Tile
{
    internal class RenderThreads
    {
        private class EventPassing
        {
            public enum EventType
            {
                Begin,
                Finish,
                Error
            }

            public EventType Type;
            public int Col;
            public int Row;
            public Exception Exception;
            public EventWaitHandle Event;
            public object Result = null;

            public EventPassing(EventType type, int row, int col, Exception ex, EventWaitHandle @event)
            {
                this.Type = type;
                this.Event = @event;
                this.Col = col;
                this.Row = row;
                this.Exception = ex;
            }
        }

        public Queue<KeyValuePair<int, int>> TileSet;
        private Queue<EventPassing> RaiseEvents = new Queue<EventPassing>();
        private object SyncLock;
        private AutoResetEvent Event;
        private int CompleteFlag;
        private TilingRunCollection Parent;
        private int Scale;
        private string Group;
        private string MapDefinition;
        private MapTilingConfiguration Invoker;

        private bool Randomize;
        private int Rows;
        private int Cols;
        private int RowOffset;
        private int ColOffset;

        private bool FillerComplete = false;

        public RenderThreads(MapTilingConfiguration invoker, TilingRunCollection parent, int scale, string group, string mapdef, int rows, int cols, int rowOffset, int colOffset, bool randomize)
        {
            TileSet = new Queue<KeyValuePair<int, int>>();
            SyncLock = new object();
            Event = new AutoResetEvent(false);
            CompleteFlag = parent.Config.ThreadCount;
            RaiseEvents = new Queue<EventPassing>();
            this.Scale = scale;
            this.Group = group;
            this.Parent = parent;
            this.MapDefinition = mapdef;
            this.Invoker = invoker;
            Randomize = randomize;
            Rows = rows;
            Cols = cols;
            this.RowOffset = rowOffset;
            this.ColOffset = colOffset;
        }

        public void RunAndWait()
        {
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(QueueFiller));

            for (int i = 0; i < Parent.Config.ThreadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(ThreadRender));
            }

            bool completed = false;
            while (!completed)
            {
                EventPassing eventToRaise = null;
                while(true)
                {
                    lock (SyncLock)
                        if (RaiseEvents.Count > 0)
                            eventToRaise = RaiseEvents.Dequeue();

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
                                Parent.InvokeBeginRendering(
                                    Invoker,
                                    Group,
                                    Scale,
                                    eventToRaise.Row,
                                    eventToRaise.Col);
                                break;
                            case EventPassing.EventType.Finish:
                                Parent.InvokeFinishRendering(
                                    Invoker,
                                    Group,
                                    Scale,
                                    eventToRaise.Row,
                                    eventToRaise.Col);
                                break;
                            case EventPassing.EventType.Error:
                                eventToRaise.Result = Parent.InvokeError(
                                    Invoker,
                                    Group,
                                    Scale,
                                    eventToRaise.Row,
                                    eventToRaise.Col,
                                    ref eventToRaise.Exception);
                                break;
                            default:
                                //Not translated, because it is an internal error that should never happen 
                                throw new Exception("Bad event type"); //NOXLATE
                        }
                        eventToRaise.Event.Set();
                        eventToRaise = null;
                    }
                }

                lock (SyncLock)
                    if (CompleteFlag == 0 && RaiseEvents.Count == 0)
                        completed = true;

                if (!completed)
                    Event.WaitOne(5 * 1000, true);
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
                if (Randomize)
                {
                    Random ra = new Random();
                    List<int> rows = new List<int>();
                    int[] cols_full = new int[Cols];
                    for (int i = 0; i < Rows; i++)
                        rows.Add(i);

                    for (int i = 0; i < Cols; i++)
                        cols_full[i] = i;

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

                            AddPairToQueue(r + RowOffset, c + ColOffset);
                        }
                    }
                }
                else
                {
                    //Non-random is straightforward
                    for (int r = 0; r < Rows; r++)
                        for (int c = 0; c < Cols; c++)
                            AddPairToQueue(r + RowOffset, c + ColOffset);
                }
            }
            finally
            {
                lock(SyncLock)
                    FillerComplete = true;
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
                lock (SyncLock)
                    if (TileSet.Count < 500) //Keep at most 500 items in queue
                    {
                        TileSet.Enqueue(new KeyValuePair<int, int>(r, c));
                        added = true;
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
                IServerConnection con = Parent.Connection.Clone();
                var tileSvc = (ITileService)con.GetService((int)ServiceType.Tile);
                AutoResetEvent ev = new AutoResetEvent(false);


                while (!Parent.Cancel)
                {
                    KeyValuePair<int, int>? round = null;

                    lock (SyncLock)
                    {
                        if (TileSet.Count == 0 && FillerComplete)
                            return; //No more data

                        if (TileSet.Count > 0)
                            round = TileSet.Dequeue();
                    }

                    if (Parent.Cancel)
                        return;

                    if (round == null) //No data, but producer is still running
                        Thread.Sleep(500);
                    else
                        RenderTile(ev, tileSvc, round.Value.Key, round.Value.Value, Scale, Group);
                }


            }
            catch { }
            finally
            {
                CompleteFlag--;
                Event.Set();
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
            lock (SyncLock)
            {
                RaiseEvents.Enqueue(new EventPassing(
                    EventPassing.EventType.Begin,
                     (int)row, (int)col, null,
                     ev
                    ));
                Event.Set();
            }
            ev.WaitOne(Timeout.Infinite, true);

            int c = Parent.Config.RetryCount;
            while (c > 0)
            {
                c--;
                try
                {
                    if (!Parent.Cancel)
                        if (Parent.Config.RenderMethod == null)
                            tileSvc.GetTile(MapDefinition, group, (int)col, (int)row, scaleindex, "PNG").Dispose(); //NOXLATE
                        else
                            Parent.Config.RenderMethod(MapDefinition, group, (int)col, (int)row, scaleindex);

                    break;
                }
                catch (Exception ex)
                {
                    if (c == 0)
                    {
                        Exception pex = ex;
                        ev.Reset();
                        EventPassing evobj = new EventPassing(
                            EventPassing.EventType.Error ,
                                (int)row, (int)col, ex,
                                ev
                                );

                        lock (SyncLock)
                        {
                            RaiseEvents.Enqueue(evobj);
                            Event.Set();
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
            lock (SyncLock)
            {
                RaiseEvents.Enqueue(new EventPassing( 
                    EventPassing.EventType.Finish,
                    (int)row, (int)col, null,
                    ev
                ));
                Event.Set();
            }
            ev.WaitOne(Timeout.Infinite, true);
        }

    }
}
