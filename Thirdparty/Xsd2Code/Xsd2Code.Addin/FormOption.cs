using System;
using System.Windows.Forms;
using Xsd2Code.Library;
using System.Diagnostics;

namespace Xsd2Code.Addin
{
    public partial class FormOption : Form
    {
        #region Property : GeneratorParams

        private GeneratorParams generatorParams;

        public GeneratorParams GeneratorParams
        {
            get { return this.generatorParams; }
            set { this.generatorParams = value; }
        }

        #endregion

        #region Property

        public string InputFile { get; set; }

        public string OutputFile { get; set; }


        #endregion

        #region cTor

        /// <summary>
        /// Constructor
        /// </summary>
        public FormOption()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Method

        /// <summary>
        /// Analyse file to find generation option.
        /// </summary>
        /// <param name="xsdFilePath">The XSD file path.</param>
        /// <param name="languageIdentifier">The language identifier.</param>
        /// <param name="defaultNamespace">The default namespace.</param>
        public void Init(string xsdFilePath, string languageIdentifier, string defaultNamespace, TargetFramework framework)
        {
            string outputFile;
            this.generatorParams = GeneratorParams.LoadFromFile(xsdFilePath, out outputFile);

            if (this.generatorParams == null)
            {
                this.generatorParams = new GeneratorParams();
                switch (languageIdentifier)
                {
                    case "{B5E9BD33-6D3E-4B5D-925E-8A43B79820B4}":
                        this.generatorParams.Language = GenerationLanguage.VisualBasic;
                        break;
                    case "{B5E9BD36-6D3E-4B5D-925E-8A43B79820B4}":
                        this.generatorParams.Language = GenerationLanguage.VisualCpp;
                        break;
                    default:
                        this.generatorParams.Language = GenerationLanguage.CSharp;
                        break;
                }
                this.generatorParams.TargetFramework = framework;
                this.generatorParams.NameSpace = defaultNamespace;
            }

            this.propertyGrid.SelectedObject = this.generatorParams;
            this.OutputFile = outputFile;
        }

        /// <summary>
        /// Cancel the validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Validate the generation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var result = this.generatorParams.Validate();
            if(!result.Success)
            {
                MessageBox.Show(result.Messages.ToString());
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        /// <summary>
        /// Close form if press esc.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs param</param>
        private void FormOption_KeyPress(object sender, KeyPressEventArgs e)
        {            
            int ascii = Convert.ToInt16(e.KeyChar);
            if (ascii == 27) this.Close();
        }

        private void linkToCodePlex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(linkToCodePlex.Text));
        }



   }
}