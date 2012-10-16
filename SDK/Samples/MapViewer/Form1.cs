using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ExtendedObjectModels;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using Maestro.Editors.Generic;

namespace MapViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IServerConnection _conn;

        protected override void OnLoad(EventArgs e)
        {
            //This call is a one-time only call that will instantly register all known resource 
            //version types and validators. This way you never have to manually reference a 
            //ObjectModels assembly of the desired resource type you want to work with
            ModelSetup.Initialize();

            //Anytime we work with the Maestro API, we require an IServerConnection
            //reference. The Maestro.Login.LoginDialog provides a UI to obtain such a 
            //reference.

            //If you need to obtain an IServerConnection reference programmatically and
            //without user intervention, use the ConnectionProviderRegistry class
            var login = new Maestro.Login.LoginDialog();
            if (login.ShowDialog() == DialogResult.OK)
            {
                _conn = login.Connection;
            }
            else //This sample does not work without an IServerConnection
            {
                Application.Exit();
            }
        }

        private void LoadMap(IServerConnection conn, string mapDefinitionId)
        {
            //To render a map, we need to create a runtime map instance. IMappingService gives us
            //the required services. This is not a standard service, so you need to call GetService()
            //with the correct ServiceType to get the IMappingService reference.
            IMappingService mapSvc = (IMappingService)conn.GetService((int)ServiceType.Mapping);

            //Get an IMapDefinition from the specified resource id. GetResource() returns an instance
            //of IResource, which IMapDefinition extends. As long as you pass in a valid resource id
            //of the right type, you can safely cast the returned object to the expected type.
            IMapDefinition mapDef = (IMapDefinition)conn.ResourceService.GetResource(mapDefinitionId);

            //Create a runtime map
            var rtMap = mapSvc.CreateMap(mapDef);

            //Load it into the viewer
            mapViewer.LoadMap(rtMap);
        }

        private void openMapDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //The ResourcePicker class, functions like a file dialog allowing the user
            //to easily select a given resource. In our case, we want the user to select
            //a Map Definition
            using (var picker = new ResourcePicker(_conn.ResourceService,
                                                   ResourceTypes.MapDefinition,
                                                   ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LoadMap(_conn, picker.ResourceID);
                }
            }
        }
    }
}
