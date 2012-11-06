using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.TextEditor
{
    public static class PlatformUtil
    {
        static PlatformUtil()
        {
            _mrtType = Type.GetType("Mono.Runtime"); //NOXLATE
        }

        private static Type _mrtType;

        /// <summary>
        /// Gets whether this application is running under the Mono CLR
        /// </summary>
        public static bool IsRunningOnMono
        {
            get
            {
                return _mrtType != null;
            }
        }
    }
}
