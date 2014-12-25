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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Capabilities;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// The expression editor interface
    /// </summary>
    internal interface IExpressionEditor
    {
        /// <summary>
        /// Initializes the expression editor
        /// </summary>
        /// <param name="edSvc"></param>
        /// <param name="caps"></param>
        /// <param name="cls"></param>
        /// <param name="featuresSourceId"></param>
        /// <param name="attachStylizationFunctions"></param>
        void Initialize(IEditorService edSvc, IFdoProviderCapabilities caps, ClassDefinition cls, string featuresSourceId, bool attachStylizationFunctions);

        /// <summary>
        /// Gets or sets the FDO expression
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// Shows the form as a modal dialog box
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.DialogResult ShowDialog();
    }

    /// <summary>
    /// An interface for inserting text
    /// </summary>
    public interface ITextInserter
    {
        /// <summary>
        /// Inserts the specified text. The implementation determines the position/cursor where the
        /// given text will be inserted at
        /// </summary>
        /// <param name="text"></param>
        void InsertText(string text);
    }

    internal static class FdoExpressionEditorFactory
    {
        public static IExpressionEditor Create()
        {
            if (Platform.IsRunningOnMono)
                return new MonoCompatibleExpressionEditor();
            else
                return new ExpressionEditor();
        }
    }
}