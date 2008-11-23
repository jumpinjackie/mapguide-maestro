#region Disclaimer / License
// Copyright (C) 2008, Kenneth Skovhede
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

namespace OSGeo.MapGuide.MgCooker
{
    class Program
    {
        private static DateTime beginOverall;
        private static DateTime beginMap;
        private static DateTime beginGroup;
        private static DateTime beginScale;
        private static DateTime beginTile;
        private static DateTime lastUpdate;

        private static List<Exception> exceptionList = new List<Exception>();
        private static List<TimeSpan> tileRuns;
        private static long tileCount;
        private static long totalTiles;
        private static TimeSpan prevDuration;
        
        private static long mapCount;
        private static long groupCount;

        private static bool m_logableProgress = false;

        static void Main(string[] args)
        {
            //TODO: Parameters:
            //mapagent=
            //username=
            //password=
            //mapdefinition=
            //scaleindex=0,1,2,3,4,5
            //basegroups="x","y"


            BatchSettings bx = new BatchSettings("http://localhost/mapguide", "Administrator", "admin");
            bx.BeginRenderingMap += new ProgressCallback(bx_BeginRenderingMap);
            bx.FinishRenderingMap += new ProgressCallback(bx_FinishRenderingMap);
            bx.BeginRenderingGroup += new ProgressCallback(bx_BeginRenderingGroup);
            bx.FinishRenderingGroup += new ProgressCallback(bx_FinishRenderingGroup);
            bx.BeginRenderingScale += new ProgressCallback(bx_BeginRenderingScale);
            bx.FinishRenderingScale += new ProgressCallback(bx_FinishRenderingScale);
            bx.BeginRenderingTile += new ProgressCallback(bx_BeginRenderingTile);
            bx.FinishRenderingTile += new ProgressCallback(bx_FinishRenderingTile);

            bx.FailedRenderingTile += new ErrorCallback(bx_FailedRenderingTile);

            mapCount = 0;
            lastUpdate = DateTime.Now;

            bx.RenderAll();
        }

        static void bx_FailedRenderingTile(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref Exception exception)
        {
            exceptionList.Add(exception);
            exception = null;
        }

        static void DisplayProgress(BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            Console.Clear();
            Console.WriteLine(string.Format("Update time:   \t{0}", DateTime.Now));
            Console.WriteLine(string.Format("Current Map:   \t{0} ({1} of {2})", map.ResourceId, mapCount, map.Parent.Maps.Count));
            Console.WriteLine(string.Format("Current Group: \t{0} ({1} of {2})", group, groupCount, map.Map.BaseMapDefinition.BaseMapLayerGroup.Count));
            Console.WriteLine(string.Format("Current Scale: \t1:{0} ({1} of {2})", map.Map.BaseMapDefinition.FiniteDisplayScale[scaleindex], scaleindex, map.Map.BaseMapDefinition.FiniteDisplayScale.Count));
            Console.WriteLine(string.Format("Current Tile:  \t{0} of {1}", tileCount, totalTiles));
            Console.WriteLine();
            Console.WriteLine(string.Format("Group duration:  \t{0}", DateTime.Now - beginGroup));
            Console.WriteLine(string.Format("Group estimate:  \t{0}", new TimeSpan(prevDuration.Ticks * totalTiles)));

            if (exceptionList.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error count: " + exceptionList.Count .ToString() + ", last exception:");
                Console.WriteLine(exceptionList[exceptionList.Count - 1].ToString());
            }
        }


        static void bx_FinishRenderingGroup(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            TimeSpan duration = DateTime.Now - beginGroup;
            if (m_logableProgress)
                Console.Write(string.Format("{0}: Rendered group {1} in {2}\n", DateTime.Now, group, duration));
        }

        static void bx_BeginRenderingGroup(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            groupCount++;
            beginGroup = DateTime.Now;

            if (m_logableProgress)
                Console.Write(string.Format("{0}: Rendering group {1} ({3} of {4})\n", beginGroup, group, 1, 1));

            tileRuns = new List<TimeSpan>();
            tileCount = 0;
            totalTiles = map.TotalTiles;
        }

        static void bx_FinishRenderingTile(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            tileRuns.Add(DateTime.Now - beginTile);
            tileCount++;

            //Update display, after 1000 tiles
            if (tileRuns.Count > 500 || (DateTime.Now - lastUpdate).TotalSeconds > 5)
            {
                long d = 0;
                foreach (TimeSpan ts in tileRuns)
                    d += ts.Ticks;

                d /= tileRuns.Count;

                //For all other than the first calculation, we use the previous counts too
                if (tileCount != tileRuns.Count)
                    d = (d + prevDuration.Ticks) / 2;

                prevDuration = new TimeSpan(d);
                TimeSpan duration = new TimeSpan(d * totalTiles);

                tileRuns.Clear();
                lastUpdate = DateTime.Now;


                if (m_logableProgress)
                {
                    Console.Write(string.Format("Processed {0} of {1} tiles in {2}, expected duration: {3}\n", tileCount, totalTiles, group, duration));

                }
                else
                    DisplayProgress(map, group, scaleindex, row, column, ref cancel);
            }
        }

        static void bx_BeginRenderingTile(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            beginTile = DateTime.Now;
        }

        static void bx_FinishRenderingScale(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            TimeSpan duration = DateTime.Now - beginScale;
            if (m_logableProgress)
                Console.Write(string.Format("{0}: Rendered scale 1:{1} in {2}\n", DateTime.Now, map.Map.BaseMapDefinition.FiniteDisplayScale[scaleindex], duration));
        }

        static void bx_BeginRenderingScale(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            beginScale = DateTime.Now;
            if (m_logableProgress)
                Console.Write(string.Format("{0}: Rendering scale 1:{1} ({2} of {3})\n", beginMap, map.Map.BaseMapDefinition.FiniteDisplayScale[scaleindex], scaleindex, map.Resolutions));
        }

        static void bx_FinishRenderingMap(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            groupCount = 0;
            TimeSpan duration = DateTime.Now - beginMap;
            if (m_logableProgress)
                Console.Write(string.Format("{0}: Rendered map {1} in {2}\n", DateTime.Now, map.ResourceId, duration));
        }

        static void bx_BeginRenderingMap(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            mapCount++;
            beginMap = DateTime.Now;
            if (m_logableProgress)
                Console.Write(string.Format("{0}: Rendering map {1}\n", beginMap, map.ResourceId));
        }
    }
}
