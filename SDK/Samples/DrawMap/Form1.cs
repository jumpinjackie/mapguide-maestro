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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ExtendedObjectModels;

namespace DrawMap
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

            //Set map display to match the size of our picture box
            rtMap.InitialiseDisplayParameters(pictureBox1.Width, pictureBox1.Height);

            //Note: Sometimes the meters-per-unit value may be inaccurate. This has ramifications on 
            //display and measuring. If you have access to the MgCoordinateSystem API, create an instance
            //using the Map Definition's WKT and use the meters-per-unit value obtained like so:
            //
            // MgCoordinateSystemFactory csFactory = new MgCoordinateSystemFactory();
            // MgCoordinateSystem cs = csFactory.Create(srs);
            // double metersPerUnit = cs.ConvertCoordinateSystemUnitsToMeters(1.0);
            //
            //This result gives a more accurate MPU

            //Before we can do anything with this map, it must first be saved.
            //Anytime you manipulate this map, you need to save it for the changes to be
            //applied back to the MapGuide Server
            rtMap.Save();

            //Render the map with its current display parameters in PNG format. A System.IO.Stream
            //is returned.
            using (var stream = rtMap.Render("PNG"))
            {
                //Create a System.Drawing.Image from the stream and load it into our picture box.
                pictureBox1.Image = Image.FromStream(stream);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
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
