using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Maestro.Shared.UI
{
    /// <summary>
    /// Helper class to overcome some of the bad default values assigned to the <see cref="OpenFileDialog"/> and <see cref="SaveFileDialog"/>
    /// 
    /// It is preferable to use this class instead of creating the <see cref="OpenFileDialog"/> or <see cref="SaveFileDialog"/> yourself.
    /// </summary>
    public static class DialogFactory
    {
        static string _lastOpenDir;
        static string _lastSaveDir;

        /// <summary>
        /// Creates an <see cref="OpenFileDialog"/> with sensible default values
        /// </summary>
        /// <returns></returns>
        public static OpenFileDialog OpenFile()
        {
            var dlg = new OpenFileDialog();

            // I guess someone at MSFT must've went over the wrong side of the ballmer peak 
            // because RestoreDirectory should not default to false. Defaulting to false
            // means that attempting to load files/assemblies using relative paths
            // will most likely fail because for some reason, using this dialog will
            // actually modify the current working directory!
            //
            // So we set this to true, but to preserve the existing behaviour, we store
            // the directory of the selected file in a static variable on dialog close (OK), 
            // and assign this dir to the InitialDirectory property on each dialog request, 
            // effectively replicating the old behaviour (w/o the nasty side effects)
            //
            // Or maybe my usage scenario does not qualify as a common use case.

            dlg.RestoreDirectory = true;
            if (Directory.Exists(_lastOpenDir))
            {
                dlg.InitialDirectory = _lastOpenDir;
            }
            dlg.FileOk += (sender, e) =>
            {
                _lastOpenDir = Path.GetDirectoryName(dlg.FileName);
            };

            return dlg;
        }

        /// <summary>
        /// Creates a <see cref="SaveFileDialog"/> with sensible default values
        /// </summary>
        /// <returns></returns>
        public static SaveFileDialog SaveFile()
        {
            var dlg = new SaveFileDialog();

            // I guess someone at MSFT must've went over the wrong side of the ballmer peak 
            // because RestoreDirectory should not default to false. Defaulting to false
            // means that attempting to load files/assemblies using relative paths
            // will most likely fail because for some reason, using this dialog will
            // actually modify the current working directory!
            //
            // So we set this to true, but to preserve the existing behaviour, we store
            // the directory of the selected file in a static variable on dialog close (OK), 
            // and assign this dir to the InitialDirectory property on each dialog request, 
            // effectively replicating the old behaviour (w/o the nasty side effects)
            //
            // Or maybe my usage scenario does not qualify as a common use case.

            dlg.RestoreDirectory = true;
            if (Directory.Exists(_lastSaveDir))
            {
                dlg.InitialDirectory = _lastSaveDir;
            }
            dlg.FileOk += (sender, e) =>
            {
                _lastSaveDir = Path.GetDirectoryName(dlg.FileName);
            };

            return dlg;
        }
    }
}
