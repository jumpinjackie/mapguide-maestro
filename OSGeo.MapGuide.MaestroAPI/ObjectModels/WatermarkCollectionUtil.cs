#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using System.Linq;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;
using System.IO;
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI.Resource;

namespace OSGeo.MapGuide.ObjectModels.WatermarkDefinition
{
    internal class WatermarkCollectionUtil
    {
        public static IWatermark AddWatermark<T>(IList<T> collection, IWatermarkDefinition watermark) where T : class, IWatermark, new()
        {
            Check.NotNull(watermark, "watermark");
            T impl = new T();
            impl.ResourceId = watermark.ResourceID;
            impl.Name = ResourceIdentifier.GetName(impl.ResourceId);
            impl.Usage = UsageType.All;
            collection.Add(impl);
            return impl;
        }

        public static void RemoveWatermark<T>(IList<T> collection, IWatermark watermark) where T : class, IWatermark
        {
            Check.NotNull(watermark, "watermark");
            T impl = watermark as T;
            if (impl != null)
                collection.Remove(impl);
        }
    }
}
