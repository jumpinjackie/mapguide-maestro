#region Disclaimer / License
// Copyright (C) 2012, Jackie Ng
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
using ICSharpCode.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Maestro.AddInManager
{
    public class InstallableAddIn
    {
        string fileName;
        bool isPackage;
        AddIn addIn;

        public AddIn AddIn
        {
            get
            {
                return addIn;
            }
        }

        public InstallableAddIn(string fileName, bool isPackage)
        {
            this.fileName = fileName;
            this.isPackage = isPackage;
            if (isPackage)
            {
                ZipFile file = new ZipFile(fileName);
                try
                {
                    LoadAddInFromZip(file);
                }
                finally
                {
                    file.Close();
                }
            }
            else
            {
                addIn = AddIn.Load(fileName);
            }
            if (addIn.Manifest.PrimaryIdentity == null)
                throw new AddInLoadException(ResourceService.GetString("AddInManager.AddInMustHaveIdentity"));
        }

        void LoadAddInFromZip(ZipFile file)
        {
            ZipEntry addInEntry = null;
            foreach (ZipEntry entry in file)
            {
                if (entry.Name.EndsWith(".addin"))
                {
                    if (addInEntry != null)
                        throw new AddInLoadException("The package may only contain one .addin file.");
                    addInEntry = entry;
                }
            }
            if (addInEntry == null)
                throw new AddInLoadException("The package must contain one .addin file.");
            using (Stream s = file.GetInputStream(addInEntry))
            {
                using (StreamReader r = new StreamReader(s))
                {
                    addIn = AddIn.Load(r);
                }
            }
        }

        public void Install(bool isUpdate)
        {
            foreach (string identity in addIn.Manifest.Identities.Keys)
            {
                ICSharpCode.Core.AddInManager.AbortRemoveUserAddInOnNextStart(identity);
            }
            if (isPackage)
            {
                string targetDir = Path.Combine(ICSharpCode.Core.AddInManager.AddInInstallTemp,
                                                addIn.Manifest.PrimaryIdentity);
                if (Directory.Exists(targetDir))
                    Directory.Delete(targetDir, true);
                Directory.CreateDirectory(targetDir);
                FastZip fastZip = new FastZip();
                fastZip.CreateEmptyDirectories = true;
                fastZip.ExtractZip(fileName, targetDir, null);

                addIn.Action = AddInAction.Install;
                if (!isUpdate)
                {
                    AddInTree.InsertAddIn(addIn);
                }
            }
            else
            {
                ICSharpCode.Core.AddInManager.AddExternalAddIns(new AddIn[] { addIn });
            }
        }

        public static void CancelUpdate(IList<AddIn> addIns)
        {
            foreach (AddIn addIn in addIns)
            {
                foreach (string identity in addIn.Manifest.Identities.Keys)
                {
                    // delete from installation temp (if installation or update is pending)
                    string targetDir = Path.Combine(ICSharpCode.Core.AddInManager.AddInInstallTemp,
                                                    identity);
                    if (Directory.Exists(targetDir))
                        Directory.Delete(targetDir, true);
                }
            }
        }

        public static void Uninstall(IList<AddIn> addIns)
        {
            CancelUpdate(addIns);
            foreach (AddIn addIn in addIns)
            {
                foreach (string identity in addIn.Manifest.Identities.Keys)
                {
                    // remove the user AddIn
                    string targetDir = Path.Combine(ICSharpCode.Core.AddInManager.UserAddInPath, identity);
                    if (Directory.Exists(targetDir))
                    {
                        if (!addIn.Enabled)
                        {
                            try
                            {
                                Directory.Delete(targetDir, true);
                                continue;
                            }
                            catch
                            {
                            }
                        }
                        ICSharpCode.Core.AddInManager.RemoveUserAddInOnNextStart(identity);
                    }
                }
            }
        }
    }
}
