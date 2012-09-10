#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
#pragma warning disable 1591, 0114, 0108, 0114, 0108
using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using OSGeo.MapGuide.MaestroAPI.Resource;
using System.IO;
using System.Xml;
using OSGeo.MapGuide.MaestroAPI.Exceptions;

#if SYM_DEF_240
namespace OSGeo.MapGuide.ObjectModels.SymbolDefinition_2_4_0
#elif SYM_DEF_110
namespace OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_1_0
#else
namespace OSGeo.MapGuide.ObjectModels.SymbolDefinition_1_0_0
#endif
{
    public static class SymbolDefEntryPoint
    {
        public static ICompoundSymbolDefinition CreateDefaultCompound()
        {
            return CompoundSymbolDefinition.CreateDefault();
        }

        public static ISimpleSymbolDefinition CreateDefaultSimple()
        {
            return SimpleSymbolDefinition.CreateDefault();
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }

        public static IResource Deserialize(string xml)
        {
            //HACK: We have to peek at the XML to determine if this is simple or compound.
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            if (doc.DocumentElement.Name == "SimpleSymbolDefinition") //NOXLATE
            {
                return SimpleSymbolDefinition.Deserialize(xml);
            }
            else
            {
                if (doc.DocumentElement.Name == "CompoundSymbolDefinition") //NOXLATE
                    return CompoundSymbolDefinition.Deserialize(xml);
                else //WTF?
                    throw new SerializationException();
            }
        }
    }
}
