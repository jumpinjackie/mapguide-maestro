﻿#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.ObjectModels.Capabilities;

namespace Maestro.Editors.Common
{
    /// <summary>
    /// The mode in which the expression editor should function
    /// </summary>
    public enum ExpressionEditorMode
    {
        /// <summary>
        /// The editor is for editing expressions
        /// </summary>
        Expression,
        /// <summary>
        /// The editor is for editing filters
        /// </summary>
        Filter
    }

    /// <summary>
    /// The expression editor interface
    /// </summary>
    public interface IExpressionEditor
    {
        /// <summary>
        /// Initializes the expression editor
        /// </summary>
        /// <param name="edSvc"></param>
        /// <param name="caps"></param>
        /// <param name="cls"></param>
        /// <param name="featureSourceId"></param>
        /// <param name="mode"></param>
        /// <param name="attachStylizationFunctions"></param>
        void Initialize(IEditorService edSvc, IFdoProviderCapabilities caps, ClassDefinition cls, string featureSourceId, ExpressionEditorMode mode, bool attachStylizationFunctions);

        /// <summary>
        /// Gets or sets the FDO expression
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// Shows the form as a modal dialog box
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.DialogResult ShowDialog();

        /// <summary>
        /// Initializes the FDO expression editor
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="classDefinition"></param>
        /// <param name="featureSourceId"></param>
        /// <param name="mode"></param>
        void Initialize(IServerConnection conn, ClassDefinition classDefinition, string featureSourceId, ExpressionEditorMode mode);
    }

    /// <summary>
    /// An interface for inserting text
    /// </summary>
    internal interface ITextInserter
    {
        /// <summary>
        /// Inserts the specified text. The implementation determines the position/cursor where the
        /// given text will be inserted at
        /// </summary>
        /// <param name="text"></param>
        void InsertText(string text);
    }

    /// <summary>
    /// A helper factory to create the appropriate <see cref="IExpressionEditor"/> based on the underlying platform
    /// </summary>
    public static class FdoExpressionEditorFactory
    {
        /// <summary>
        /// Creates an <see cref="IExpressionEditor"/>
        /// </summary>
        /// <returns></returns>
        public static IExpressionEditor Create()
        {
            if (Platform.IsRunningOnMono)
                return new MonoCompatibleExpressionEditor();
            else
                return new ExpressionEditor();
        }
    }
}