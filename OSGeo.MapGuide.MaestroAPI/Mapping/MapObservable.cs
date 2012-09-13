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

namespace OSGeo.MapGuide.MaestroAPI.Mapping
{
    /// <summary>
    /// Base class implementation of the <see cref="T:System.ComponentModel.INotifyPropertyChanged"/>
    /// interface
    /// </summary>
    public abstract class MapObservable : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates whether property change events will be raised
        /// </summary>
        protected bool _disableChangeTracking = true;

        /// <summary>
        /// Set the specified field with the specified value raising
        /// the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return false;
            
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// If the specified old value is different from the new value, the specified setter is invoked
        /// and a <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event
        /// is raised
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="setter"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool ObservableSet<T>(T oldValue, T newValue, Action<T> setter, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;

            setter.Invoke(newValue);
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_disableChangeTracking)
                return;

            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Occurs when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
