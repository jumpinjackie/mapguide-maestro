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
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI.Capability;
using OSGeo.MapGuide.MaestroAPI.Commands;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public class LocalNativeCapabilities : ConnectionCapabilities
    {
        internal LocalNativeCapabilities(IServerConnection parent) : base(parent) { }

        public override int[] SupportedCommands
        {
            get
            {
                if (_parent.SiteVersion >= new Version(2, 2))
                {
                    //TODO: Work out what this can/can't do
                    return new int[] 
                    {
                        (int)CommandType.GetResourceContents,
                        (int)CommandType.ApplySchema,
                        (int)CommandType.InsertFeature,
                        (int)CommandType.UpdateFeatures,
                        (int)CommandType.DeleteFeatures
                    };
                }
                else
                {
                    //TODO: Work out what this can/can't do
                    return new int[] 
                    {
                        (int)CommandType.ApplySchema,
                        (int)CommandType.InsertFeature,
                        (int)CommandType.UpdateFeatures,
                        (int)CommandType.DeleteFeatures
                    };
                }
            }
        }

        public override bool SupportsResourcePreviews
        {
            get { return true; }
        }

        public override bool IsMultithreaded
        {
            get { return false; }
        }
    }
}
