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
using Duplicati.CommandLine;

namespace OSGeo.MapGuide.MgCooker
{
    public class Program
    {
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

        private static bool hasConsole = true;

        [STAThread()]
        public static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.DoEvents();

            //Parameters:
            //mapagent=
            //username=
            //password=
            //mapdefinition=
            //scaleindex=0,1,2,3,4,5
            //basegroups="x","y"

            string mapagent = "http://localhost/mapguide";
            string username = "Anonymous";
            string password = "";
            string mapdefinitions = "";
            string scaleindex = "";
            string basegroups = "";

            string limitRows = "";
            string limitCols = "";

            string tileWidth = "";
            string tileHeight = "";

            string DPI = "";
            string metersPerUnit = "";


            List<string> largs = new List<string>(args);
            Dictionary<string, string> opts = CommandLineParser.ExtractOptions(largs);
            if (opts.ContainsKey("mapagent"))
                mapagent = opts["mapagent"];
            if (opts.ContainsKey("username"))
                username = opts["username"];
            if (opts.ContainsKey("password"))
                password = opts["password"];
            if (opts.ContainsKey("mapdefinitions"))
                mapdefinitions = opts["mapdefinitions"];
            if (opts.ContainsKey("scaleindex"))
                scaleindex = opts["scaleindex"];
            if (opts.ContainsKey("basegroups"))
                basegroups = opts["basegroups"];

            if (opts.ContainsKey("limitrows"))
                limitRows = opts["limitrows"];
            if (opts.ContainsKey("limitcols"))
                limitCols = opts["limitcols"];

            if (opts.ContainsKey("tilewidth"))
                tileWidth = opts["tilewidth"];
            if (opts.ContainsKey("tileheight"))
                tileHeight = opts["tileheight"];

            if (opts.ContainsKey("DPI"))
                DPI = opts["DPI"];
            if (opts.ContainsKey("metersperunit"))
                metersPerUnit = opts["metersperunit"];

            try
            {
                Console.Clear();
            }
            catch
            {
                hasConsole = false;
            }


            MaestroAPI.ServerConnectionI connection = null;

            if (!opts.ContainsKey("username") || (!opts.ContainsKey("mapagent")))
            {
                if (largs.IndexOf("/commandline") < 0 && largs.IndexOf("commandline") < 0)
                {
                    Maestro.FormLogin frm = new OSGeo.MapGuide.Maestro.FormLogin();
                    if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

                    connection = frm.Connection;
                    mapagent = ((MaestroAPI.HttpServerConnection)connection).ServerURI;
                }
            }

            if (connection == null)
            {
                if (!opts.ContainsKey("native-connection"))
                    connection = new OSGeo.MapGuide.MaestroAPI.HttpServerConnection(new Uri(mapagent), username, password, System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, true);
                else
                {
                    string serverconfig = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "webconfig.ini");
                    connection = new OSGeo.MapGuide.MaestroAPI.LocalNativeConnection(serverconfig, username, password, System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
                }
            }

            string[] maps = mapdefinitions.Split(',');

            if (largs.IndexOf("batch") < 0 && largs.IndexOf("/batch") < 0)
            {
                SetupRun sr = new SetupRun(connection, maps, opts);
                sr.ShowDialog();
                return;
            }
            

            BatchSettings bx = new BatchSettings(connection, maps);
            if (!string.IsNullOrEmpty(scaleindex))
            {
                List<int> scales = new List<int>();
                int tmp;
                foreach (string s in scaleindex.Split(','))
                    if (int.TryParse(s, out tmp))
                        scales.Add(tmp);
                bx.SetScales(scales.ToArray());
            }

            if (!string.IsNullOrEmpty(basegroups))
            {
                List<string> groups = new List<string>();
                foreach (string s in basegroups.Split(','))
                {
                    string f = s;
                    if (f.StartsWith("\""))
                        f = f.Substring(1);
                    if (f.EndsWith("\""))
                        f = f.Substring(0, f.Length - 1);
                    groups.Add(f);
                }
                bx.SetGroups(groups.ToArray());
            }

            int x;

            if (!string.IsNullOrEmpty(limitCols) && int.TryParse(limitCols, out x))
                bx.LimitCols(x);
            if (!string.IsNullOrEmpty(limitRows) && int.TryParse(limitRows, out x))
                bx.LimitRows(x);

            if (!string.IsNullOrEmpty(tileWidth) && int.TryParse(tileWidth, out x))
                bx.Config.TileWidth = x;
            if (!string.IsNullOrEmpty(tileHeight) && int.TryParse(tileHeight, out x))
                bx.Config.TileHeight = x;

            if (!string.IsNullOrEmpty(DPI) && int.TryParse(DPI, out x))
                bx.Config.DPI = x;

            double d;
            if (!string.IsNullOrEmpty(metersPerUnit) && double.TryParse(metersPerUnit, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out d))
            {
                bx.Config.MetersPerUnit = d;
                bx.Config.UseOfficialMethod = true;
            }

            if (opts.ContainsKey("random-tile-order"))
                bx.Config.RandomizeTileSequence = true;

            if (opts.ContainsKey("threadcount") && int.TryParse(opts["threadcount"], out x) && x > 0)
                bx.Config.ThreadCount = x;

            if (largs.IndexOf("/commandline") < 0 && largs.IndexOf("commandline") < 0)
            {
                Progress pg = new Progress(bx);
                pg.ShowDialog();
            }
            else
            {
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
        }

        static void bx_FailedRenderingTile(CallbackStates state, BatchMap map, string group, int scaleindex, int row, int column, ref Exception exception)
        {
            exceptionList.Add(exception);
            exception = null;
        }

        static void DisplayProgress(BatchMap map, string group, int scaleindex, int row, int column, ref bool cancel)
        {
            if (hasConsole)
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
