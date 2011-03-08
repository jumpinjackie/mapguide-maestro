using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Xsd2Code.Library;
using IServiceProvider=Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Xsd2Code.CustomTool
{
    /// <summary>
    /// Implementation of the Xsd2Code custom tool for VisualStudio
    /// <para>Usage:</para>
    /// <para>     Specify Xsd2CodeCustomTool in the CustomTool proprty of an XSD file within Visual Studio</para>
    /// </summary>
    /// <remarks>
    /// 
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    ///     Changed signature of the GeneratorFacade class constructor
    /// 
    /// </remarks>
    [Guid("9E6FCB59-E3EF-4bbe-966C-30AC92A44DF6")]
    public class Xsd2CodeCustomTool : IVsSingleFileGenerator, IObjectWithSite
    {
        private CodeDomProvider provider;
        private object site;

        #region IVsSingleFileGenerator Membres

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = this.provider.FileExtension;
            
            if (!string.IsNullOrEmpty(pbstrDefaultExtension))
                pbstrDefaultExtension = ".Designer." + pbstrDefaultExtension.TrimStart(".".ToCharArray());
            
            return 0;
        }

        /// <summary>
        /// Generates the specified WSZ input file path.
        /// </summary>
        /// <param name="wszInputFilePath">The WSZ input file path.</param>
        /// <param name="bstrInputFileContents">The BSTR input file contents.</param>
        /// <param name="wszDefaultNamespace">The WSZ default namespace.</param>
        /// <param name="rgbOutputFileContents">The RGB output file contents.</param>
        /// <param name="pcbOutput">The PCB output.</param>
        /// <param name="pGenerateProgress">The p generate progress.</param>
        /// <returns></returns>
        public int Generate(string wszInputFilePath,
                            string bstrInputFileContents,
                            string wszDefaultNamespace,
                            IntPtr[] rgbOutputFileContents,
                            out uint pcbOutput,
                            IVsGeneratorProgress pGenerateProgress)
        {
            if (wszInputFilePath == null)
                throw new ArgumentNullException(wszInputFilePath);

            var generatorParams = GeneratorParams.LoadFromFile(wszInputFilePath) ?? new GeneratorParams();

            generatorParams.InputFilePath = wszInputFilePath;
            generatorParams.NameSpace = wszDefaultNamespace;

            var xsdGen = new GeneratorFacade(this.provider, generatorParams);

            var result = xsdGen.GenerateBytes();
            var generatedStuff = result.Entity;

            if (generatedStuff == null)
            {
                rgbOutputFileContents[0] = IntPtr.Zero;
                pcbOutput = 0;
                return 0;
            }

            // Copie du flux en mémoire pour que Visual Studio puisse le récupérer
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(generatedStuff.Length);
            Marshal.Copy(generatedStuff, 0, rgbOutputFileContents[0], generatedStuff.Length);
            pcbOutput = (uint) generatedStuff.Length;

            return 0;
        }

        #endregion

        #region IObjectWithSite Members

        /// <summary>
        /// </summary>
        /// <param name="riid"></param>
        /// <param name="ppvSite"></param>
        public void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (this.site == null)
                throw new COMException("Aucun site", VSConstants.E_FAIL);

            // Créer un pointeur d'interface
            IntPtr pUnknownPointer = Marshal.GetIUnknownForObject(this.site);

// ReSharper disable RedundantAssignment
            IntPtr intPointer = IntPtr.Zero;
// ReSharper restore RedundantAssignment

            // Demande de pointeur de l'interface pUnknownPointer
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero)
                throw new COMException("site ne supporte pas la demande de pointeur", VSConstants.E_NOINTERFACE);

            ppvSite = intPointer;
        }

        /// <summary>
        /// </summary>
        /// <param name="pUnkSite"></param>
        public void SetSite(object pUnkSite)
        {
            this.site = pUnkSite;

            //Créer un fournisseur de service du site
            var serviceProvider = new ServiceProvider(this.site as IServiceProvider);

            //Récupéreur le service SVSMDCodeDomProvider
            var p = serviceProvider.GetService(typeof (SVSMDCodeDomProvider)) as IVSMDCodeDomProvider;

            if (p != null)
                this.provider = p.CodeDomProvider as CodeDomProvider;
            else
            {
                //Ici, aucun langage n'a pu être déterminé
                this.provider = CodeDomProvider.CreateProvider("C#");
            }
        }

        #endregion
    }
}