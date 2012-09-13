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
using OSGeo.MapGuide.MaestroAPI.Commands;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;
using OSGeo.MapGuide.MaestroAPI.Internal;
using GeoAPI.Geometries;

#if LOCAL_API
namespace OSGeo.MapGuide.MaestroAPI.Local.Commands
#else
namespace OSGeo.MapGuide.MaestroAPI.Native.Commands
#endif
{
    [global::System.Serializable]
    public class FeatureServiceException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public FeatureServiceException() { }
        public FeatureServiceException(string message) : base(message) { }
        public FeatureServiceException(string message, Exception inner) : base(message, inner) { }
        protected FeatureServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string MgErrorDetails { get; set; }

        public string MgStackTrace { get; set; }
    }

    internal static class GeomConverter
    {
        static MgAgfReaderWriter _agfRw;
        static MgWktReaderWriter _wktRw;
        static FixedWKTReader _reader;

        static GeomConverter()
        {
            _agfRw = new MgAgfReaderWriter();
            _wktRw = new MgWktReaderWriter();
            _reader = new FixedWKTReader();
        }

        public static MgByteReader GetAgf(IGeometry geom)
        {
            MgGeometry mgeom = _wktRw.Read(geom.AsText());
            return _agfRw.Write(mgeom);
        }

        public static string GetWkt(MgGeometry geom)
        {
            return _wktRw.Write(geom);
        }

        public static IGeometry GetGeometry(MgByteReader agf)
        {
            MgGeometry mgeom = _agfRw.Read(agf);
            return _reader.Read(GetWkt(mgeom));   
        }
    }

    internal static class MgFeatureCommandUtility
    {
        internal static void Update(MgPropertyCollection props, IRecord record)
        {
            if (props.Count != record.FieldCount)
                throw new InvalidOperationException("Number of values to update does not match. Ensure the MgPropertyCollection was initialized first with PropertyUtil.Populate() first and that the input IRecord comes from the same source used to initialize this MgPropertyCollection"); //LOCALIZEME

            //Flip the null bit first
            for (int i = 0; i < props.Count; i++)
            {
                var p = props.GetItem(i);
                var np = p as MgNullableProperty;
                if (np != null)
                    np.SetNull(true);

                string name = p.Name;
                if (!record.IsNull(name))
                {
                    switch (p.PropertyType)
                    {
                        case MgPropertyType.Blob:
                            {
                                var bytes = record.GetBlob(name);
                                var bs = new MgByteSource(bytes, bytes.Length);
                                ((MgBlobProperty)p).SetValue(bs.GetReader());
                            }
                            break;
                        case MgPropertyType.Boolean:
                            {
                                ((MgBooleanProperty)p).SetValue(record.GetBoolean(name));
                            }
                            break;
                        case MgPropertyType.Byte:
                            {
                                ((MgByteProperty)p).SetValue(record.GetByte(name));
                            }
                            break;
                        case MgPropertyType.Clob:
                            {
                                var bytes = record.GetBlob(name);
                                var bs = new MgByteSource(bytes, bytes.Length);
                                ((MgClobProperty)p).SetValue(bs.GetReader()); 
                            }
                            break;
                        case MgPropertyType.DateTime:
                            {
                                var dt = record.GetDateTime(i);
                                var mdt = new MgDateTime((short)dt.Year, (short)dt.Month, (short)dt.Day, (short)dt.Hour, (short)dt.Minute, (short)dt.Second, dt.Millisecond * 1000);
                                ((MgDateTimeProperty)p).SetValue(mdt);
                            }
                            break;
                        case MgPropertyType.Decimal:
                        case MgPropertyType.Double:
                            {
                                ((MgDoubleProperty)p).SetValue(record.GetDouble(name));
                            }
                            break;
                        case MgPropertyType.Geometry:
                            {
                                var agf = GeomConverter.GetAgf(record.GetGeometry(name));
                                ((MgGeometryProperty)p).SetValue(agf);
                            }
                            break;
                        case MgPropertyType.Int16:
                            {
                                ((MgInt16Property)p).SetValue(record.GetInt16(name));
                            }
                            break;
                        case MgPropertyType.Int32:
                            {
                                ((MgInt32Property)p).SetValue(record.GetInt32(name));
                            }
                            break;
                        case MgPropertyType.Int64:
                            {
                                ((MgInt64Property)p).SetValue(record.GetInt64(name));
                            }
                            break;
                        case MgPropertyType.Single:
                            {
                                ((MgSingleProperty)p).SetValue(record.GetSingle(name));
                            }
                            break;
                        case MgPropertyType.String:
                            {
                                ((MgStringProperty)p).SetValue(record.GetString(name));
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }

        internal static void Populate(MgPropertyCollection props, IMutableRecord record)
        {
            for (int i = 0; i < record.FieldCount; i++)
            {
                var pt = record.GetPropertyType(i);
                string name = record.GetName(i);
                if (record.IsNull(i))
                {
                    switch (pt)
                    {
                        case PropertyValueType.Blob:
                            {
                                var propVal = new MgBlobProperty(name, null);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Boolean:
                            {
                                var propVal = new MgBooleanProperty(name, false);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Byte:
                            {
                                var propVal = new MgByteProperty(name, 0);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Clob:
                            {
                                var propVal = new MgClobProperty(name, null);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.DateTime:
                            {
                                var propVal = new MgDateTimeProperty(name, null);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Double:
                            {
                                var propVal = new MgDoubleProperty(name, 0.0);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Geometry:
                            {
                                var propVal = new MgGeometryProperty(name, null);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Int16:
                            {
                                var propVal = new MgInt16Property(name, 0);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Int32:
                            {
                                var propVal = new MgInt32Property(name, 0);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Int64:
                            {
                                var propVal = new MgInt64Property(name, 0L);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.Single:
                            {
                                var propVal = new MgSingleProperty(name, 0.0f);
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        case PropertyValueType.String:
                            {
                                var propVal = new MgStringProperty(name, "");
                                propVal.SetNull(true);
                                props.Add(propVal);
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {
                    switch (pt)
                    {
                        case PropertyValueType.Blob:
                            {
                                var bytes = record.GetBlob(i);
                                var br = new MgByteSource(bytes, bytes.Length);
                                var bv = new MgBlobProperty(name, br.GetReader());
                                props.Add(bv);
                            }
                            break;
                        case PropertyValueType.Boolean:
                            {
                                props.Add(new MgBooleanProperty(name, record.GetBoolean(i)));
                            }
                            break;
                        case PropertyValueType.Byte:
                            {
                                props.Add(new MgByteProperty(name, record.GetByte(i)));
                            }
                            break;
                        case PropertyValueType.Clob:
                            {
                                var bytes = record.GetBlob(i);
                                var br = new MgByteSource(bytes, bytes.Length);
                                var bv = new MgClobProperty(name, br.GetReader());
                                props.Add(bv);
                            }
                            break;
                        case PropertyValueType.DateTime:
                            {
                                var dt = record.GetDateTime(i);
                                var mdt = new MgDateTime((short)dt.Year, (short)dt.Month, (short)dt.Day, (short)dt.Hour, (short)dt.Minute, (short)dt.Second, dt.Millisecond * 1000);
                                props.Add(new MgDateTimeProperty(name, mdt));
                            }
                            break;
                        case PropertyValueType.Double:
                            {
                                props.Add(new MgDoubleProperty(name, record.GetDouble(i)));
                            }
                            break;
                        case PropertyValueType.Geometry:
                            {
                                MgByteReader agf = GeomConverter.GetAgf(record.GetGeometry(i));
                                props.Add(new MgGeometryProperty(name, agf));
                            }
                            break;
                        case PropertyValueType.Int16:
                            {
                                props.Add(new MgInt16Property(name, record.GetInt16(i)));
                            }
                            break;
                        case PropertyValueType.Int32:
                            {
                                props.Add(new MgInt32Property(name, record.GetInt32(i)));
                            }
                            break;
                        case PropertyValueType.Int64:
                            {
                                props.Add(new MgInt64Property(name, record.GetInt64(i)));
                            }
                            break;
                        case PropertyValueType.Single:
                            {
                                props.Add(new MgSingleProperty(name, record.GetSingle(i)));
                            }
                            break;
                        case PropertyValueType.String:
                            {
                                props.Add(new MgStringProperty(name, record.GetString(i)));
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }

        internal static MgFeatureSchema ConvertSchema(FeatureSchema source)
        {
            MgFeatureSchema fs = new MgFeatureSchema(source.Name, source.Description);
            MgClassDefinitionCollection classes = fs.GetClasses();
            foreach (ClassDefinition cls in source.Classes)
            {
                MgClassDefinition clsDef = new MgClassDefinition();
                clsDef.SetName(cls.Name);
                clsDef.SetDescription(cls.Description);
                clsDef.SetDefaultGeometryPropertyName(cls.DefaultGeometryPropertyName);
                var clsProps = clsDef.GetProperties();
                var idProps = clsDef.GetIdentityProperties();

                foreach (PropertyDefinition prop in cls.Properties)
                {
                    switch (prop.Type)
                    {
                        case PropertyDefinitionType.Data:
                            {
                                var dp = new MgDataPropertyDefinition(prop.Name);
                                var srcDp = (DataPropertyDefinition)prop;
                                dp.SetAutoGeneration(srcDp.IsAutoGenerated);
                                dp.SetDataType((int)srcDp.DataType);
                                if (srcDp.DefaultValue != null) dp.SetDefaultValue(srcDp.DefaultValue);
                                if (srcDp.Description != null) dp.SetDescription(srcDp.Description);
                                dp.SetLength(srcDp.Length);
                                dp.SetNullable(srcDp.IsNullable);
                                dp.SetPrecision(srcDp.Precision);
                                dp.SetReadOnly(srcDp.IsReadOnly);
                                dp.SetScale(srcDp.Scale);

                                clsProps.Add(dp);

                                if (cls.IdentityProperties.Contains(srcDp))
                                    idProps.Add(dp);
                            }
                            break;
                        case PropertyDefinitionType.Geometry:
                            {
                                var gp = new MgGeometricPropertyDefinition(prop.Name);
                                var srcGp = (GeometricPropertyDefinition)prop;
                                if (srcGp.Description != null) gp.SetDescription(srcGp.Description);
                                gp.SetGeometryTypes((int)srcGp.GeometricTypes);
                                gp.SetHasElevation(srcGp.HasElevation);
                                gp.SetHasMeasure(srcGp.HasMeasure);
                                gp.SetReadOnly(srcGp.IsReadOnly);
                                if (srcGp.SpatialContextAssociation != null) gp.SetSpatialContextAssociation(srcGp.SpatialContextAssociation);

                                clsProps.Add(gp);
                            }
                            break;
                        case PropertyDefinitionType.Raster:
                            {
                                var rp = new MgRasterPropertyDefinition(prop.Name);
                                var srcRp = (RasterPropertyDefinition)prop;
                                rp.SetDefaultImageXSize(srcRp.DefaultImageYSize);
                                rp.SetDefaultImageYSize(srcRp.DefaultImageYSize);
                                if (srcRp.Description != null) rp.SetDescription(srcRp.Description);
                                rp.SetNullable(srcRp.IsNullable);
                                rp.SetReadOnly(srcRp.IsReadOnly);
                                if (srcRp.SpatialContextAssociation != null) rp.SetSpatialContextAssociation(srcRp.SpatialContextAssociation);

                                clsProps.Add(rp);
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }

                classes.Add(clsDef);
            }

            return fs;
        }
    }

#if LOCAL_API
    public class LocalNativeInsert : DefaultInsertCommand<LocalConnection>
    {
        internal LocalNativeInsert(LocalConnection conn) : base(conn) { }
#else
    public class LocalNativeInsert : DefaultInsertCommand<LocalNativeConnection>
    {
        internal LocalNativeInsert(LocalNativeConnection conn) : base(conn) { }
#endif
        protected override void ExecuteInternal()
        {
            MgPropertyCollection props = new MgPropertyCollection();
            MgFeatureCommandUtility.Populate(props, this.RecordToInsert);
            this.ConnImpl.InsertFeatures(new MgResourceIdentifier(this.FeatureSourceId), this.ClassName, props);
        }
    }

#if LOCAL_API
    public class LocalNativeUpdate : DefaultUpdateCommand<LocalConnection>
    {
        internal LocalNativeUpdate(LocalConnection conn) : base(conn) { }
#else
    public class LocalNativeUpdate : DefaultUpdateCommand<LocalNativeConnection>
    {
        internal LocalNativeUpdate(LocalNativeConnection conn) : base(conn) { }
#endif

        public override int ExecuteInternal()
        {
            MgPropertyCollection props = new MgPropertyCollection();
            MgFeatureCommandUtility.Populate(props, this.ValuesToUpdate);
            return this.ConnImpl.UpdateFeatures(new MgResourceIdentifier(this.FeatureSourceId), this.ClassName, props, this.Filter);
        }
    }

#if LOCAL_API
    public class LocalNativeDelete : DefaultDeleteCommand<LocalConnection>
    {
        internal LocalNativeDelete(LocalConnection conn) : base(conn) { }
#else
    public class LocalNativeDelete : DefaultDeleteCommand<LocalNativeConnection>
    {
        internal LocalNativeDelete(LocalNativeConnection conn) : base(conn) { }
#endif

        protected override int ExecuteInternal()
        {
            return this.ConnImpl.DeleteFeatures(new MgResourceIdentifier(this.FeatureSourceId), this.ClassName, this.Filter);
        }
    }

#if LOCAL_API
    public class LocalNativeApplySchema : DefaultApplySchemaCommand<LocalConnection>
    {
        internal LocalNativeApplySchema(LocalConnection conn) : base(conn) { }
#else
    public class LocalNativeApplySchema : DefaultApplySchemaCommand<LocalNativeConnection>
    {
        internal LocalNativeApplySchema(LocalNativeConnection conn) : base(conn) { }
#endif

        protected override void ExecuteInternal()
        {
            MgFeatureSchema schema = MgFeatureCommandUtility.ConvertSchema(this.Schema);
            this.ConnImpl.ApplySchema(new MgResourceIdentifier(this.FeatureSourceId), schema);
        }
    }

#if LOCAL_API
    public class LocalNativeCreateDataStore : DefaultCreateDataStoreCommand<LocalConnection>
    {
        internal LocalNativeCreateDataStore(LocalConnection conn) : base(conn) { }
#else
    public class LocalNativeCreateDataStore : DefaultCreateDataStoreCommand<LocalNativeConnection>
    {
        internal LocalNativeCreateDataStore(LocalNativeConnection conn) : base(conn) { }
#endif
        protected override void ExecuteInternal()
        {
            MgFileFeatureSourceParams fp = new MgFileFeatureSourceParams(this.Provider, this.Name, this.CoordinateSystemWkt, MgFeatureCommandUtility.ConvertSchema(this.Schema));
            fp.FileName = this.FileName;
            if (this.Description != null) fp.SpatialContextDescription = this.Description;
            fp.XYTolerance = this.XYTolerance;
            fp.ZTolerance = this.ZTolerance;
            this.ConnImpl.CreateDataStore(new MgResourceIdentifier(this.FeatureSourceId), fp);
        }
    }
}
