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
    public interface IFeatureCommand : ICommand 
    {
        string FeatureSourceId { get; set; }

        string ClassName { get; set; }
    }

    public interface ICreateDataStore : ICommand, IFdoSpatialContext
    {
        FeatureSchema Schema { get; set; }

        string Provider { get; set; }

        string FileName { get; set; }

        string FeatureSourceId { get; set; }

        void Execute();
    }

    public interface IInsertFeatures : IFeatureCommand
    {
        IMutableRecord RecordToInsert { get; set; }

        InsertResult Execute();
    }

    public class InsertResult
    {
        public Exception Error { get; set; }
    }

    public interface IBatchInsertFeatures : IFeatureCommand
    {
        ICollection<IRecord> RecordsToInsert { get; set; }

        ICollection<InsertResult> Execute();
    }

    public interface IUpdateFeatures : IFeatureCommand
    {
        string Filter { get; set; }

        IMutableRecord ValuesToUpdate { get; set; }

        int Execute();
    }

    public interface IDeleteFeatures : IFeatureCommand
    {
        string Filter { get; set; }

        int Execute();
    }

    public interface IApplySchema : ICommand
    {
        string FeatureSourceId { get; set; }

        FeatureSchema Schema { get; set; }

        void Execute();
    }
}
