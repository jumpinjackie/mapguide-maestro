using System;
using System.Collections.Generic;
using System.Text;

namespace OSGeo.MapGuide.MgCooker.Commandline
{
    class Program
    {
        [STAThread()]
        static void Main(string[] args)
        {
            //Append the "/commandline" switch
            List<string> tmp = new List<string>(args);
            tmp.Add("/commandline");
            OSGeo.MapGuide.MgCooker.Program.Main(tmp.ToArray());
        }
    }
}
