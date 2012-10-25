using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using Maestro.Base.Services;
using HelloAddIn.Services;

namespace HelloAddIn.Commands
{
    public class InvokeExampleServiceCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            //Services can be accessed via the ServiceRegistry utility class
            //Any service class registered under the /Maestro/ApplicationServices
            //extension point can be accessed via this class
            var service = ServiceRegistry.GetService<ExampleService>();
            service.Test();
        }
    }
}
