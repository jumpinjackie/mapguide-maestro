#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License
using Maestro.AddIn.Rest.Model;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Windows.Forms;

namespace Maestro.AddIn.Rest.UI
{
    public partial class RestLoginDialog : Form
    {
        public RestLoginDialog()
        {
            InitializeComponent();
        }

        private RestClient _client;

        public RestClient GetClient() => _client;

        private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            _client = new RestClient(txtUrl.Text)
            {
                Authenticator = new HttpBasicAuthenticator(txtUsername.Text, txtPassword.Text)
            };

            //To test the credentials, create a session
            var req = new RestRequest("session", Method.POST);
            var session = _client.Execute<PrimitiveValueResponse>(req);

            this.DialogResult = DialogResult.OK;
        }
    }
}
