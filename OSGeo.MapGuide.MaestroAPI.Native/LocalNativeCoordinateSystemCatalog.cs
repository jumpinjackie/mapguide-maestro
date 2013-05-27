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
using OSGeo.MapGuide.MaestroAPI.CoordinateSystem;

namespace OSGeo.MapGuide.MaestroAPI.Native
{
    public class LocalNativeCoordinateSystemCatalog : CoordinateSystemCatalog
    {
        private CoordinateSystemCategory[] m_categories;
        private string m_coordLib = null;
        internal OSGeo.MapGuide.MgCoordinateSystemFactory m_cf;

        public LocalNativeCoordinateSystemCatalog()
        {
            m_cf = new MgCoordinateSystemFactory();
        }

        public override CoordinateSystemCategory[] Categories
        {
            get
            {
                if (m_categories == null)
                {
                    MgStringCollection c = m_cf.EnumerateCategories();
                    CoordinateSystemCategory[] data = new CoordinateSystemCategory[c.GetCount()];

                    for (int i = 0; i < c.GetCount(); i++)
                        data[i] = new LocalNativeCoordinateSystemCategory(this, c.GetItem(i));
                    m_categories = data;
                }

                return m_categories;
            }
        }

        public override string ConvertCoordinateSystemCodeToWkt(string coordcode)
        {
            return m_cf.ConvertCoordinateSystemCodeToWkt(coordcode);
        }

        public override string ConvertEpsgCodeToWkt(string epsg)
        {
            return m_cf.ConvertEpsgCodeToWkt(int.Parse(epsg));
        }

        public override string ConvertWktToCoordinateSystemCode(string wkt)
        {
            return m_cf.ConvertWktToCoordinateSystemCode(wkt);
        }

        public override string ConvertWktToEpsgCode(string wkt)
        {
            return m_cf.ConvertWktToEpsgCode(wkt).ToString();
        }

        public override bool IsValid(string wkt)
        {
            return m_cf.IsValid(wkt);
        }

        public override string LibraryName
        {
            get
            {
                if (m_coordLib == null)
                    m_coordLib = m_cf.GetBaseLibrary();
                return m_coordLib;
            }
        }

        public override bool IsLoaded { get { return m_categories != null; } }

        public override CoordinateSystemDefinitionBase[] EnumerateCoordinateSystems(string category)
        {
            CoordinateSystemCategory cat = null;
            foreach (CoordinateSystemCategory csc in this.Categories)
            {
                if (csc.Name == category)
                {
                    cat = csc;
                    break;
                }
            }

            if (cat == null)
                return new CoordinateSystemDefinitionBase[0];

            MgBatchPropertyCollection bp = m_cf.EnumerateCoordinateSystems(category);
            List<CoordinateSystemDefinitionBase> lst = new List<CoordinateSystemDefinitionBase>();
            for (int i = 0; i < bp.GetCount(); i++)
                lst.Add(new LocalNativeCoordinateSystemDefinition(cat, bp.GetItem(i)));

            return lst.ToArray();
        }

        public override CoordinateSystemDefinitionBase CreateEmptyCoordinateSystem()
        {
            return new LocalNativeCoordinateSystemDefinition();
        }

        public override ISimpleTransform CreateTransform(string sourceWkt, string targetWkt)
        {
            return new LocalNativeSimpleTransform(sourceWkt, targetWkt);
        }
    }
}
