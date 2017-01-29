#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License

using OSGeo.MapGuide.ObjectModels;
using System;
using System.Reflection;

namespace OSGeo.MapGuide.MaestroAPI
{
    // Original Source: http://www.wintellect.com/blogs/jeffreyr/weak-event-handlers

    /// <summary>
    /// A weak event handler factory
    /// </summary>
    public static class WeakEventHandler
    {
        private class WeakEventHandlerImpl
        {
            protected readonly WeakReference m_wrTarget; // WeakReference to original delegate's target object
            protected Delegate m_openEventHandler;       // "Open" delegate to invoke original target's delegate method

            public WeakEventHandlerImpl(Delegate d)
            {
                m_wrTarget = new WeakReference(d.Target);
            }

            // Match is used to compare a WeakEventHandlerImpl object with an actual delegate.
            // Typically used to remove a WeakEventHandlerImpl from an event collection.
            public Boolean Match(Delegate strongEventHandler)
            {
                // Returns true if original target & method match the WeakEventHandlerImpl's Target & method
                return (m_wrTarget.Target == strongEventHandler.Target) && (m_openEventHandler.Method == strongEventHandler.Method);
            }
        }

        // "Open" delegate definition to quickly invoke original delegate's callback
        private delegate void OpenEventHandler<TTarget, TEventArgs>(TTarget target, Object sender, TEventArgs eventArgs)
            where TTarget : class
            where TEventArgs : EventArgs;

        // A proxy object that knows how to invoke a callback on an object if it hasn't been GC'd
        private sealed class WeakEventHandlerImpl<TEventHandler> : WeakEventHandlerImpl where TEventHandler : class
        {
            // Refers to a method that removes a delegate to this proxy object once we know the original target has been GC'd
            private readonly Action<TEventHandler> m_cleanup;

            // This is the delegate passed to m_cleanup that needs to be removed from an event
            private readonly TEventHandler m_proxyHandler;

            public static TEventHandler Create(TEventHandler eh, Action<TEventHandler> cleanup)
            {
                Check.ThatPreconditionIsMet(eh != null && cleanup != null, $"{nameof(eh)} != null && {nameof(cleanup)} != null");
                // We don't create weak events for static methods since types don't get GC'd
                Delegate d = (Delegate)(Object)eh;  // We know that all event handlers are derived from Delegate
                if (d.Target == null) return eh;

                var weh = new WeakEventHandlerImpl<TEventHandler>(d, cleanup);
                return weh.m_proxyHandler; // Return the delegate to add to the event
            }

            private WeakEventHandlerImpl(Delegate d, Action<TEventHandler> cleanup)
                : base(d)
            {
                m_cleanup = cleanup;

                Type targetType = d.Target.GetType();
                Type eventHandlerType = typeof(TEventHandler);
                Type eventArgsType = eventHandlerType.IsGenericType
                   ? eventHandlerType.GetGenericArguments()[0]
                   : eventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;

                // Create a delegate to the ProxyInvoke method; this delegate is registered with the event
                var miProxy = typeof(WeakEventHandlerImpl<TEventHandler>)
                   .GetMethod("ProxyInvoke", BindingFlags.Instance | BindingFlags.NonPublic)
                   .MakeGenericMethod(targetType, eventArgsType);
                m_proxyHandler = (TEventHandler)(Object)Delegate.CreateDelegate(eventHandlerType, this, miProxy);

                // Create an "open" delegate to the original delegate's method; ProxyInvoke calls this
                Type openEventHandlerType = typeof(OpenEventHandler<,>).MakeGenericType(d.Target.GetType(), eventArgsType);
                m_openEventHandler = Delegate.CreateDelegate(openEventHandlerType, null, d.Method);
            }

            private void ProxyInvoke<TTarget, TEventArgs>(Object sender, TEventArgs e)
                where TTarget : class
                where TEventArgs : EventArgs
            {
                // If the original target object still exists, call it; else call m_cleanup to unregister our delegate with the event
                TTarget target = (TTarget)m_wrTarget.Target;
                if (target != null)
                {
                    ((OpenEventHandler<TTarget, TEventArgs>)m_openEventHandler)(target, sender, e);
                }
                else
                {
                    m_cleanup(m_proxyHandler);
                    System.Diagnostics.Debug.WriteLine("Cleanup event handler");
                }
            }
        }

        // We offer this overload because it is so common

        /// <summary>
        /// Wraps the given event handler as a weak event
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="cleanup"></param>
        /// <returns></returns>
        public static EventHandler Wrap(EventHandler eh, Action<EventHandler> cleanup)
        {
            return WeakEventHandlerImpl<EventHandler>.Create(eh, cleanup);
        }

        /// <summary>
        /// Wraps the given event handler as a weak event
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="eh"></param>
        /// <param name="cleanup"></param>
        /// <returns></returns>
        public static TEventHandler Wrap<TEventHandler>(TEventHandler eh, Action<TEventHandler> cleanup) where TEventHandler : class
        {
            return WeakEventHandlerImpl<TEventHandler>.Create(eh, cleanup);
        }

        /// <summary>
        /// Wraps the given event handler as a weak event
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="eh"></param>
        /// <param name="cleanup"></param>
        /// <returns></returns>
        public static EventHandler<TEventArgs> Wrap<TEventArgs>(EventHandler<TEventArgs> eh, Action<EventHandler<TEventArgs>> cleanup) where TEventArgs : EventArgs
        {
            return WeakEventHandlerImpl<EventHandler<TEventArgs>>.Create(eh, cleanup);
        }

        /// <summary>
        /// Returns true if the given weak and strong event handlers match
        /// </summary>
        /// <param name="weakEventHandler"></param>
        /// <param name="strongEventHandler"></param>
        /// <returns></returns>
        public static Boolean Match(Delegate weakEventHandler, Delegate strongEventHandler)
        {
            return ((WeakEventHandlerImpl)weakEventHandler.Target).Match(strongEventHandler);
        }
    }
}