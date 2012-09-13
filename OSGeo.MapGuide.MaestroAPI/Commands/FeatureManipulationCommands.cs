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
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.ObjectModels.Common;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// Defines a command that works against a Feature Class of a Feature Source
    /// </summary>
    public interface IFeatureCommand : ICommand 
    {
        /// <summary>
        /// Gets or sets the Feature Source ID
        /// </summary>
        string FeatureSourceId { get; set; }

        /// <summary>
        /// Gets or sets the Feature Class name
        /// </summary>
        string ClassName { get; set; }
    }

    /// <summary>
    /// Defines a command that creates a file-based data store on a given feature source
    /// </summary>
    public interface ICreateDataStore : ICommand, IFdoSpatialContext
    {
        /// <summary>
        /// Gets or sets the Feature Schema that models the structure of the data store
        /// </summary>
        FeatureSchema Schema { get; set; }

        /// <summary>
        /// Gets or sets the FDO provider name
        /// </summary>
        string Provider { get; set; }

        /// <summary>
        /// Gets or sets the file name for the data store
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Feature Source ID
        /// </summary>
        string FeatureSourceId { get; set; }

        /// <summary>
        /// Executes the command
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// Defines a command that inserts a feature into a Feature Source
    /// </summary>
    public interface IInsertFeatures : IFeatureCommand
    {
        /// <summary>
        /// The feature to insert
        /// </summary>
        IMutableRecord RecordToInsert { get; set; }
        
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>The feature insert result</returns>
        InsertResult Execute();
    }

    /// <summary>
    /// The result of a <see cref="M:OSGeo.MapGuide.MaestroAPI.Commands.IInsertFeatures.Execute" /> operation
    /// </summary>
    public class InsertResult
    {
        /// <summary>
        /// Gets the <see cref="T:System.Exception"/> object. If there is no exception, the insert operation succeeded.
        /// </summary>
        public Exception Error { get; set; }
    }

    /// <summary>
    /// Defines a command that inserts a series of features into a Feature Source
    /// </summary>
    public interface IBatchInsertFeatures : IFeatureCommand
    {
        /// <summary>
        /// Gets or sets the list of features to insert
        /// </summary>
        ICollection<IRecord> RecordsToInsert { get; set; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.InsertResult" /> instances. 
        /// Inspect the individual <see cref="P:OSGeo.MapGuide.MaestroAPI.Commands.InsertResult "/> properties to
        /// determine which features failed to be inserted.
        /// </returns>
        ICollection<InsertResult> Execute();
    }

    /// <summary>
    /// Defines a command that updates one or more features in a Feature Source based on some filtering criteria
    /// </summary>
    public interface IUpdateFeatures : IFeatureCommand
    {
        /// <summary>
        /// Gets or sets the filter that determines which features will be updated. If empty, will cause all
        /// features to be updated
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Gets or sets the collection of values to apply
        /// </summary>
        IMutableRecord ValuesToUpdate { get; set; }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>The number of features updated by this command</returns>
        int Execute();
    }

    /// <summary>
    /// Defines a command that deletes one or more features in a Feature Source based on some filtering criteria
    /// </summary>
    public interface IDeleteFeatures : IFeatureCommand
    {
        /// <summary>
        /// Gets or sets the filter that determines what features will be deleted. If empty, this will delete all features
        /// </summary>
        string Filter { get; set; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The number of features deleted</returns>
        int Execute();
    }

    /// <summary>
    /// Defines a command that applies the given Feature Schema to a Feature Source
    /// </summary>
    public interface IApplySchema : ICommand
    {
        /// <summary>
        /// Gets or sets the Feature Source ID
        /// </summary>
        string FeatureSourceId { get; set; }

        /// <summary>
        /// Gets or sets the Feature Schema
        /// </summary>
        FeatureSchema Schema { get; set; }

        /// <summary>
        /// Executes the command
        /// </summary>
        void Execute();
    }
}
