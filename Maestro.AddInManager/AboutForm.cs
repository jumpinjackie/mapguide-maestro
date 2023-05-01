﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using ICSharpCode.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ICSharpCode.AddInManager
{
    using Str = Maestro.AddInManager.Strings;

    public class AboutForm : System.Windows.Forms.Form
    {
        private Font boldFont;

        public AboutForm(AddIn addIn)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            boldFont = new Font(Font, FontStyle.Bold);

            List<string> titles = new List<string>();
            List<string> values = new List<string>();

            this.Text = addIn.Name;
            closeButton.Text = ResourceService.GetString("Global.CloseButtonText"); //NOXLATE

            titles.Add(Str.TitleAddInFile);
            values.Add(addIn.Name);

            if (addIn.Manifest.PrimaryVersion != null && addIn.Manifest.PrimaryVersion.ToString() != "0.0.0.0")
            {
                titles.Add(Str.TitleVersion);
                values.Add(addIn.Manifest.PrimaryVersion.ToString());
            }

            if (addIn.Properties["author"].Length > 0)
            {
                titles.Add(Str.TitleAuthor);
                values.Add(addIn.Properties["author"]);
            }

            if (addIn.Properties["copyright"].Length > 0)
            {
                if (!addIn.Properties["copyright"].StartsWith("prj:"))
                {
                    titles.Add(Str.TitleCopyright);
                    values.Add(addIn.Properties["copyright"]);
                }
            }

            if (addIn.Properties["license"].Length > 0)
            {
                titles.Add(Str.TitleLicense);
                values.Add(addIn.Properties["license"]);
            }

            if (addIn.Properties["url"].Length > 0)
            {
                titles.Add(Str.TitleWebsite);
                values.Add(addIn.Properties["url"]);
            }

            if (addIn.Properties["description"].Length > 0)
            {
                titles.Add(Str.TitleDescription);
                values.Add(addIn.Properties["description"]);
            }

            titles.Add(Str.TitleAddInFile);
            values.Add(FileUtility.NormalizePath(addIn.FileName));

            titles.Add(Str.TitleInternalName);
            values.Add(addIn.Manifest.PrimaryIdentity);

            table.RowCount = titles.Count + 1;
            table.RowStyles.Clear();
            for (int i = 0; i < titles.Count; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                AddRow(titles[i], values[i], i);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (boldFont != null)
                    boldFont.Dispose();
            }
        }

        private void AddRow(string desc, string val, int rowIndex)
        {
            Label descLabel = new Label();
            descLabel.AutoSize = true;
            descLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            descLabel.Font = boldFont;
            descLabel.Text = StringParser.Parse(desc) + ":"; //NOXLATE
            table.Controls.Add(descLabel, 0, rowIndex);

            Label valLabel;
            string link = GetLink(val);
            if (link != null)
            {
                LinkLabel linkLabel = new LinkLabel();
                linkLabel.LinkClicked += delegate
                {
                    try
                    {
                        var ps = new ProcessStartInfo(link)
                        {
                            UseShellExecute = true,
                            Verb = "open"
                        };
                        Process.Start(ps);
                    }
                    catch (Exception ex)
                    {
                        MessageService.ShowMessage(ex.ToString());
                    }
                };
                valLabel = linkLabel;
            }
            else
            {
                valLabel = new Label();
            }
            valLabel.AutoSize = true;
            valLabel.Text = val;
            table.Controls.Add(valLabel, 1, rowIndex);
        }

        private string GetLink(string text)
        {
            if (text == null)
                return null;
            switch (text)
            {
                case "GNU General Public License": //NOXLATE
                case "GPL": //NOXLATE
                    return "http://www.gnu.org/licenses/gpl.html"; //NOXLATE
                case "LGPL": //NOXLATE
                case "GNU Lesser General Public License": //NOXLATE
                    return "http://www.gnu.org/licenses/lgpl.html"; //NOXLATE
                default:
                    if (text.StartsWith("http://")) //NOXLATE
                        return text;
                    if (text.StartsWith("www.")) //NOXLATE
                        return "http://" + text; //NOXLATE
                    return null;
            }
        }

        #region Windows Forms Designer generated code

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Panel bottomPanel;
            this.closeButton = new System.Windows.Forms.Button();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            bottomPanel = new System.Windows.Forms.Panel();
            bottomPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // bottomPanel
            //
            bottomPanel.Controls.Add(this.closeButton);
            bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            bottomPanel.Location = new System.Drawing.Point(0, 233);
            bottomPanel.Name = "bottomPanel";
            bottomPanel.Size = new System.Drawing.Size(351, 35);
            bottomPanel.TabIndex = 0;
            //
            // closeButton
            //
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(264, 6);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseCompatibleTextRendering = true;
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            //
            // table
            //
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 8);
            this.table.Name = "table";
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table.Size = new System.Drawing.Size(351, 225);
            this.table.TabIndex = 1;
            //
            // AboutForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(351, 268);
            this.Controls.Add(this.table);
            this.Controls.Add(bottomPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutForm";
            bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TableLayoutPanel table;

        #endregion Windows Forms Designer generated code

        private void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}