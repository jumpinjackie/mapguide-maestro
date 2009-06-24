using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;

namespace OSGeo.MapGuide.MgCooker
{
    public class RenderThreads
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
            public System.Threading.EventWaitHandle Event;
            public object Result = null;

            public EventPassing(EventType type, int row, int col, Exception ex, System.Threading.EventWaitHandle @event)
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
        private System.Threading.AutoResetEvent Event;
        private int CompleteFlag;
        private BatchSettings Parent;
        private int Scale;
        private string Group;
        private string MapDefinition;
        private BatchMap Invoker;

        public RenderThreads(BatchMap invoker, BatchSettings parent, int scale, string group, string mapdef)
        {
            TileSet = new Queue<KeyValuePair<int, int>>();
            SyncLock = new object();
            Event = new System.Threading.AutoResetEvent(false);
            CompleteFlag = parent.Config.ThreadCount;
            RaiseEvents = new Queue<EventPassing>();
            this.Scale = scale;
            this.Group = group;
            this.Parent = parent;
            this.MapDefinition = mapdef;
            this.Invoker = invoker;
        }

        public void RunAndWait()
        {
            for (int i = 0; i < Parent.Config.ThreadCount; i++)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(
                    new System.Threading.WaitCallback(ThreadRender));
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
                                throw new Exception("Bad event type");
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

        private void ThreadRender(object dummy)
        {
            try
            {
                //Create a copy of the connection for local usage
                ServerConnectionI con;
                System.Threading.AutoResetEvent ev = new System.Threading.AutoResetEvent(false);

                if (Parent.Connection is HttpServerConnection)
                    con = new HttpServerConnection(new Uri(((HttpServerConnection)Parent.Connection).ServerURI), Parent.Connection.SessionID, null, true);
                else
                    con = new LocalNativeConnection(Parent.Connection.SessionID);


                while (!Parent.Cancel)
                {
                    KeyValuePair<int, int> round;
                    lock (SyncLock)
                    {
                        if (TileSet.Count == 0)
                            return;

                        round = TileSet.Dequeue(); ;
                    }


                    if (Parent.Cancel)
                        return;

                    RenderTile(ev, con, round.Key, round.Value, Scale, Group);
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
        /// <param name="row">The row index of the tile</param>
        /// <param name="col">The column index of the tile</param>
        /// <param name="scaleindex">The scale index of the tile</param>
        /// <param name="group">The name of the baselayer group</param>
        public void RenderTile(System.Threading.EventWaitHandle ev, ServerConnectionI connection, long row, long col, int scaleindex, string group)
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
            ev.WaitOne(System.Threading.Timeout.Infinite, true);

            int c = Parent.Config.RetryCount;
            while (c > 0)
            {
                c--;
                try
                {
                    if (!Parent.Cancel)
                        if (Parent.Config.RenderMethod == null)
                            connection.GetTile(MapDefinition, group, (int)col, (int)row, scaleindex, "PNG").Dispose();
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

                        ev.WaitOne(System.Threading.Timeout.Infinite, true);

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
            ev.WaitOne(System.Threading.Timeout.Infinite, true);
        }

    }
}
