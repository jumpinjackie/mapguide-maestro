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
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Validation;

#if WL240
namespace OSGeo.MapGuide.ObjectModels.WebLayout_2_4_0
#else
namespace OSGeo.MapGuide.ObjectModels.WebLayout_1_1_0
#endif
{
    public class WebLayoutValidator : BaseWebLayoutValidator
    {
        public override ResourceTypeDescriptor SupportedResourceAndVersion
        {
#if WL240
            get { return new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, "2.4.0"); }
#else
            get { return new ResourceTypeDescriptor(OSGeo.MapGuide.MaestroAPI.ResourceTypes.WebLayout, "1.1.0"); }
#endif
        }
    }
}
