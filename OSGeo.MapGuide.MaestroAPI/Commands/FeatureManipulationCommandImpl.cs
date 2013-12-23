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
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace OSGeo.MapGuide.MaestroAPI.Commands
{
    /// <summary>
    /// A default implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.ICommand"/>. This class is reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultCommand<TConn> : ICommand where TConn : IServerConnection 
    {
        /// <summary>
        /// Gets the connection implementation.
        /// </summary>
        public TConn ConnImpl { get { return (TConn)this.Parent; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCommand&lt;TConn&gt;"/> class.
        /// </summary>
        /// <param name="conn">The conn.</param>
        protected DefaultCommand(TConn conn)
        {
            this.Parent = conn;
        }

        /// <summary>
        /// Gets the parent connection.
        /// </summary>
        /// <value>
        /// The parent connection.
        /// </value>
        public IServerConnection Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Validates the core commnad parameters.
        /// </summary>
        protected void ValidateCoreParams()
        {
            if (this.Parent == null)
                throw new InvalidOperationException("Null parent connection"); //LOCALIZEME
        }
    }

    /// <summary>
    /// A default implementation of the <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IFeatureCommand"/>. This class is reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultFeatureCommand<TConn> : DefaultCommand<TConn>, IFeatureCommand where TConn : IServerConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFeatureCommand&lt;TConn&gt;"/> class.
        /// </summary>
        /// <param name="conn">The conn.</param>
        protected DefaultFeatureCommand(TConn conn) : base(conn) { }

        /// <summary>
        /// Gets or sets the feature source id.
        /// </summary>
        /// <value>
        /// The feature source id.
        /// </value>
        public string FeatureSourceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// Performs basic validation of core command parameters
        /// </summary>
        protected void ValidateParams()
        {
            base.ValidateCoreParams();
            if (string.IsNullOrEmpty(this.FeatureSourceId))
                throw new InvalidOperationException(Strings.ErrorNoFeatureSourceIdSpecified);
            if (string.IsNullOrEmpty(this.ClassName))
                throw new InvalidOperationException(Strings.ErrorNoFeatureSourceIdSpecified);
        }
    }

    /// <summary>
    /// A default implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IInsertFeatures"/>. This class is reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultInsertCommand<TConn> : DefaultFeatureCommand<TConn>, IInsertFeatures where TConn : IServerConnection
    {
        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        /// <param name="conn">The conn.</param>
        protected DefaultInsertCommand(TConn conn) : base(conn) { }

        /// <summary>
        /// Gets or sets the record to insert.
        /// </summary>
        /// <value>
        /// The record to insert.
        /// </value>
        public IMutableRecord RecordToInsert
        {
            get;
            set;
        }

        /// <summary>
        /// Performs the actual command execution
        /// </summary>
        protected abstract void ExecuteInternal();

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        public InsertResult Execute()
        {
            var res = new InsertResult();
            try
            {
                base.ValidateParams();
                if (this.RecordToInsert == null)
                    throw new InvalidOperationException(Strings.ErrorNothingToInsert);

                this.ExecuteInternal(); 
            }
            catch (Exception ex)
            {
                res.Error = ex;
            }
            return res;
        }
    }

    /// <summary>
    /// A default implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IUpdateFeatures"/>. This class is reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultUpdateCommand<TConn> : DefaultFeatureCommand<TConn>, IUpdateFeatures where TConn : IServerConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUpdateCommand&lt;TConn&gt;"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public DefaultUpdateCommand(TConn parent) : base(parent) { }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the values to update.
        /// </summary>
        /// <value>
        /// The values to update.
        /// </value>
        public IMutableRecord ValuesToUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Performs the actual execution of the command
        /// </summary>
        /// <returns></returns>
        public abstract int ExecuteInternal();

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            base.ValidateParams();
            if (this.ValuesToUpdate == null)
                throw new InvalidOperationException(Strings.ErrorNoValuesSpecifiedForUpdating);

            return ExecuteInternal();
        }
    }

    /// <summary>
    /// A default implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IDeleteFeatures"/>. This class is reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultDeleteCommand<TConn> : DefaultFeatureCommand<TConn>, IDeleteFeatures where TConn : IServerConnection 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDeleteCommand&lt;TConn&gt;"/> class.
        /// </summary>
        /// <param name="conn">The conn.</param>
        protected DefaultDeleteCommand(TConn conn) : base(conn) { }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns></returns>
        public int Execute()
        {
            this.ValidateParams();
            return this.ExecuteInternal();
        }

        /// <summary>
        /// Performs actual execution of the command
        /// </summary>
        /// <returns></returns>
        protected abstract int ExecuteInternal();
    }

    /// <summary>
    /// A default implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IApplySchema"/>. This class is reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultApplySchemaCommand<TConn> : DefaultFeatureCommand<TConn>, IApplySchema where TConn : IServerConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApplySchemaCommand&lt;TConn&gt;"/> class.
        /// </summary>
        /// <param name="conn">The conn.</param>
        protected DefaultApplySchemaCommand(TConn conn) : base(conn) { }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public OSGeo.MapGuide.MaestroAPI.Schema.FeatureSchema Schema
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
            base.ValidateCoreParams();
            if (string.IsNullOrEmpty(this.FeatureSourceId))
                throw new InvalidOperationException(Strings.ErrorEmptyFeatureSourceId);
            if (this.Schema == null)
                throw new InvalidOperationException(Strings.ErrorNoSchemaSpecifiedToApply);

            this.ExecuteInternal();
        }

        /// <summary>
        /// Performs the actual command execution
        /// </summary>
        protected abstract void ExecuteInternal();
    }

    /// <summary>
    /// A default implementation of <see cref="T:OSGeo.MapGuide.MaestroAPI.Commands.IApplySchema"/>. Reserved for connection provider use
    /// </summary>
    /// <typeparam name="TConn">The type of the conn.</typeparam>
    public abstract class DefaultCreateDataStoreCommand<TConn> : DefaultCommand<TConn>, ICreateDataStore where TConn : IServerConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCreateDataStoreCommand&lt;TConn&gt;"/> class.
        /// </summary>
        /// <param name="conn">The conn.</param>
        protected DefaultCreateDataStoreCommand(TConn conn) : base(conn) { }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public OSGeo.MapGuide.MaestroAPI.Schema.FeatureSchema Schema
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the feature source id.
        /// </summary>
        /// <value>
        /// The feature source id.
        /// </value>
        public string FeatureSourceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public string Provider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
            this.ValidateCoreParams();
            if (string.IsNullOrEmpty(this.FeatureSourceId))
                throw new InvalidOperationException(Strings.ErrorNoFeatureSourceIdSpecified);
            if (string.IsNullOrEmpty(this.CoordinateSystemWkt))
                throw new InvalidOperationException(Strings.ErrorNoCoordinateSystemWktSpecified);
            if (this.Extent == null && this.ExtentType != OSGeo.MapGuide.ObjectModels.Common.FdoSpatialContextListSpatialContextExtentType.Dynamic)
                throw new InvalidOperationException(Strings.ErrorNoExtentSpecifiedForStaticType);
            if (string.IsNullOrEmpty(this.FileName))
                throw new InvalidOperationException(Strings.ErrorNoFileNameSpecified);
            if (string.IsNullOrEmpty(this.Name))
                throw new InvalidOperationException(Strings.ErrorNoSpatialContextNameSpecified);
            if (string.IsNullOrEmpty(this.Provider))
                throw new InvalidOperationException(Strings.ErrorNoProviderSpecified);
            if (this.Schema == null)
                throw new InvalidOperationException(Strings.ErrorNoSchemaSpecified);
            this.ExecuteInternal();
        }

        /// <summary>
        /// Performs actual execution of the command.
        /// </summary>
        protected abstract void ExecuteInternal();

        /// <summary>
        /// Gets or sets the name of the coordinate system.
        /// </summary>
        /// <value>
        /// The name of the coordinate system.
        /// </value>
        public string CoordinateSystemName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the coordinate system WKT.
        /// </summary>
        /// <value>
        /// The coordinate system WKT.
        /// </value>
        public string CoordinateSystemWkt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        /// <value>
        /// The extent.
        /// </value>
        public OSGeo.MapGuide.ObjectModels.Common.IEnvelope Extent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the extent.
        /// </summary>
        /// <value>
        /// The type of the extent.
        /// </value>
        public OSGeo.MapGuide.ObjectModels.Common.FdoSpatialContextListSpatialContextExtentType ExtentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the spatial context to create.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the XY tolerance.
        /// </summary>
        /// <value>
        /// The XY tolerance.
        /// </value>
        public double XYTolerance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Z tolerance.
        /// </summary>
        /// <value>
        /// The Z tolerance.
        /// </value>
        public double ZTolerance
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentNode"></param>
        public void WriteXml(System.Xml.XmlDocument doc, System.Xml.XmlNode currentNode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mgr"></param>
        public void ReadXml(System.Xml.XmlNode node, System.Xml.XmlNamespaceManager mgr)
        {
            throw new NotImplementedException();
        }
    }
}
