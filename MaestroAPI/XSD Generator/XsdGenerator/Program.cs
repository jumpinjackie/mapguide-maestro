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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace XsdGenerator
{
	/// <summary>
	/// Summary description for Program.
	/// </summary>
	public class Program
	{
		public static void Main(string[] args)
		{
			OpenFileDialog dlg1 = new OpenFileDialog();
			dlg1.AddExtension = false;
			dlg1.CheckFileExists = true;
			dlg1.CheckPathExists = true;
			dlg1.DefaultExt = ".xsd";
			dlg1.DereferenceLinks = true;
			dlg1.Filter = "XSD Files (*.xsd)|*.xsd|All files (*.*)|*.*";
			dlg1.FilterIndex = 0;
			dlg1.Multiselect = true;
			dlg1.ValidateNames = true;
			dlg1.Title = "Select XSD files to parse";
			if (dlg1.ShowDialog() == DialogResult.OK)
			{
				FolderBrowserDialog dlg2 = new FolderBrowserDialog();
				dlg2.Description = "Select output path";
				dlg2.ShowNewFolderButton = true;
				if (dlg2.ShowDialog() == DialogResult.OK)
				{

					FormNamespace dlg3 = new FormNamespace();
					if (dlg3.ShowDialog() == DialogResult.OK)
					{
						CodeDomProvider provider = dlg3.SelectedLanguage;
						foreach(string s in dlg1.FileNames)
						{
							try
							{
								string dest = System.IO.Path.Combine(dlg2.SelectedPath, System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(s), "." + provider.FileExtension));
								buildFile(s, dest, dlg3.SelectedNameSpace, provider);
							}
							catch (Exception ex)
							{
								MessageBox.Show("Failed with file: " + s + "\r\nMessage: " + ex.Message, "XSD Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}
				}
			}
		}

		private static void buildFile(string input, string output, string namespc, CodeDomProvider provider)
		{
			CodeNamespace ns = Processor.Process( input, namespc );

			// Write the code to the output file.
			using ( StreamWriter sw = new StreamWriter( output, false ) )
			{
				provider.CreateGenerator().GenerateCodeFromNamespace(
					ns, sw, new CodeGeneratorOptions() );
			}		
		}
	}
}
