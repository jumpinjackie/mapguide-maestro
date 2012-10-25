using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maestro.Shared.UI;
using System.Windows.Forms;
using ICSharpCode.Core;

namespace HelloAddIn.Services
{
    public class ExampleService : ServiceBase
    {
        public void Test()
        {
            MessageService.ShowMessage("You invoked an application-service method");
        }
    }
}
