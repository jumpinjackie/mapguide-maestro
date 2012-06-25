#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using System.Xml.Serialization;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System.IO;

#if WDF_240
namespace OSGeo.MapGuide.ObjectModels.WatermarkDefinition_2_4_0
{
    using Sdf240 = SymbolDefinition_2_4_0;
#else
namespace OSGeo.MapGuide.ObjectModels.WatermarkDefinition_2_3_0
{
    using Sdf110 = SymbolDefinition_1_1_0;
#endif

    public static class WdfEntryPoint
    {
        public static WatermarkDefinition CreateDefault(SymbolDefinitionType type)
        {
            var wdf = new WatermarkDefinition()
            {
                Appearance = new WatermarkAppearanceType(),
                Content = new WatermarkDefinitionTypeContent()
                {
#if WDF_240
                    Item = (type == SymbolDefinitionType.Simple) ? (Sdf240.SymbolDefinitionBase)Sdf240.SymbolDefEntryPoint.CreateDefaultSimple() : (Sdf240.SymbolDefinitionBase)Sdf240.SymbolDefEntryPoint.CreateDefaultCompound()
#else
                    Item = (type == SymbolDefinitionType.Simple) ? (Sdf110.SymbolDefinitionBase)Sdf110.SymbolDefEntryPoint.CreateDefaultSimple() : (Sdf110.SymbolDefinitionBase)Sdf110.SymbolDefEntryPoint.CreateDefaultCompound()
#endif
                },
                Position = new WatermarkDefinitionTypePosition()
                {
                    Item = new XYPositionType()
                }
            };
            if (wdf.Content.Item.Type == SymbolDefinitionType.Simple)
            {
                var sym = (ISimpleSymbolDefinition)wdf.Content.Item;
                sym.PointUsage = sym.CreatePointUsage();
            }
            wdf.Content.Item.Name = wdf.Content.Item.Description = "";
            wdf.Content.Item.RemoveSchemaAttributes();
            return wdf;
        }

        public static IResource Deserialize(string xml)
        {
            IWatermarkDefinition wdf = WatermarkDefinition.Deserialize(xml);
            if (wdf.Content != null)
                wdf.Content.RemoveSchemaAttributes();

            return wdf;
        }

        public static Stream Serialize(IResource res)
        {
            return res.SerializeToStream();
        }
    }

    partial class WatermarkDefinition : IWatermarkDefinition
    {
#if WDF_240
        private static readonly Version RES_VERSION = new Version(2, 4, 0);
#else
        private static readonly Version RES_VERSION = new Version(2, 3, 0);
#endif

        [XmlIgnore]
        public ResourceTypes ResourceType
        {
            get
            {
                return ResourceTypes.WatermarkDefinition;
            }
        }

        [XmlIgnore]
        public Version ResourceVersion
        {
            get
            {
                return RES_VERSION;
            }
        }

        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string ValidatingSchema
        {
#if WDF_240
            get { return "WatermarkDefinition-2.4.0.xsd"; }
#else
            get { return "WatermarkDefinition-2.3.0.xsd"; }
#endif
            set { }
        }

        [XmlIgnore]
        public bool IsStronglyTyped
        {
            get { return true; }
        }

        [XmlIgnore]
        ISymbolDefinitionBase IWatermarkDefinition.Content
        {
            get
            {
                if (this.Content != null)
                    return this.Content.Item;

                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [XmlIgnore]
        IWatermarkAppearance IWatermarkDefinition.Appearance
        {
            get
            {
                return this.Appearance;
            }
            set
            {
                this.Appearance = (WatermarkAppearanceType)value;
            }
        }

        [XmlIgnore]
        IPosition IWatermarkDefinition.Position
        {
            get
            {
                if (this.Position != null)
                    return this.Position.Item;
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (this.Position == null)
                        this.Position = new WatermarkDefinitionTypePosition();

                    this.Position.Item = (PositionType)value;
                }
            }
        }

        [XmlIgnore]
        OSGeo.MapGuide.MaestroAPI.IServerConnection OSGeo.MapGuide.MaestroAPI.Resource.IResource.CurrentConnection
        {
            get;
            set;
        }

        private string _resId;

        [XmlIgnore]
        public string ResourceID
        {
            get
            {
                return _resId;
            }
            set
            {
                if (!ResourceIdentifier.Validate(value))
                    throw new InvalidOperationException("Not a valid resource identifier"); //LOCALIZE

                var res = new ResourceIdentifier(value);
                if (res.Extension != ResourceTypes.WatermarkDefinition.ToString())
                    throw new InvalidOperationException("Invalid resource identifier for this type of object: " + res.Extension); //LOCALIZE

                _resId = value;
                this.OnPropertyChanged("ResourceID");
            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public IXYPosition CreateXYPosition()
        {
            return new XYPositionType()
            {
                XPosition = new HorizontalPositionType(),
                YPosition = new VerticalPositionType()
            };
        }

        public ITilePosition CreateTilePosition()
        {
            return new TilePositionType()
            {
                HorizontalPosition = new HorizontalPositionType(),
                VerticalPosition = new VerticalPositionType()
            };
        }

        public Version SupportedMapDefinitionVersion
        {
            get 
            { 
#if WDF_240
                return new Version(2, 4, 0);
#else
                return new Version(2, 3, 0);
#endif
            }
        }

        public Version SupportedLayerDefinitionVersion
        {
            get
            {
#if WDF_240
                return new Version(2, 4, 0);
#else
                return new Version(2, 3, 0);
#endif
            }
        }
    }

    partial class WatermarkType : IWatermark
    {
        [XmlIgnore]
        IWatermarkAppearance IWatermark.AppearanceOverride
        {
            get
            {
                return this.AppearanceOverride;
            }
            set
            {
                this.AppearanceOverride = (WatermarkAppearanceType)value;
            }
        }

        [XmlIgnore]
        IPosition IWatermark.PositionOverride
        {
            get
            {
                return this.PositionOverride == null ? null : this.PositionOverride.Item;
            }
            set
            {
                if (value == null)
                {
                    this.PositionOverride = null;
                }
                else
                {
                    if (this.PositionOverride == null)
                        this.PositionOverride = new WatermarkTypePositionOverride();

                    this.PositionOverride.Item = (PositionType)value;
                }
            }
        }

        public IWatermarkAppearance CreateDefaultAppearance()
        {
            return new WatermarkAppearanceType()
            {
                Rotation = 0,
                Transparency = 0
            };
        }

        public IXYPosition CreateDefaultXYPosition()
        {
            return new XYPositionType()
            {
                XPosition = new HorizontalPositionType()
                {
                    Alignment = HorizontalAlignmentType.Center,
                    Offset = 0.0,
                    Unit = UnitType.Pixels
                },
                YPosition = new VerticalPositionType()
                {
                    Alignment = VerticalAlignmentType.Center,
                    Offset = 0.0,
                    Unit = UnitType.Pixels
                }
            };
        }

        public ITilePosition CreateDefaultTilePosition()
        {
            return new TilePositionType()
            {
                VerticalPosition = new VerticalPositionType(),
                HorizontalPosition = new HorizontalPositionType(),
                TileHeight = 200,
                TileWidth = 200
            };
        }
    }

    partial class WatermarkAppearanceType : IWatermarkAppearance { }

    partial class WatermarkTypePositionOverride { }

    partial class XYPositionType : IXYPosition 
    {
        [XmlIgnore]
        public override OSGeo.MapGuide.ObjectModels.WatermarkDefinition.PositionType Type
        {
            get { return OSGeo.MapGuide.ObjectModels.WatermarkDefinition.PositionType.XY; }
        }

        [XmlIgnore]
        IHorizontalPosition IXYPosition.XPosition
        {
            get
            {
                return this.XPosition;
            }
            set
            {
                this.XPosition = (HorizontalPositionType)value;
            }
        }

        [XmlIgnore]
        IVerticalPosition IXYPosition.YPosition
        {
            get
            {
                return this.YPosition;
            }
            set
            {
                this.YPosition = (VerticalPositionType)value;
            }
        }
    }

    abstract partial class PositionType : IPosition
    {
        [XmlIgnore]
        public abstract OSGeo.MapGuide.ObjectModels.WatermarkDefinition.PositionType Type { get; }
    }

    partial class TilePositionType : ITilePosition
    {
        [XmlIgnore]
        public override OSGeo.MapGuide.ObjectModels.WatermarkDefinition.PositionType Type
        {
            get { return OSGeo.MapGuide.ObjectModels.WatermarkDefinition.PositionType.Tile; ; }
        }

        [XmlIgnore]
        IHorizontalPosition ITilePosition.HorizontalPosition
        {
            get
            {
                return this.HorizontalPosition;
            }
            set
            {
                this.HorizontalPosition = (HorizontalPositionType)value;
            }
        }

        [XmlIgnore]
        IVerticalPosition ITilePosition.VerticalPosition
        {
            get
            {
                return this.VerticalPosition;
            }
            set
            {
                this.VerticalPosition = (VerticalPositionType)value;
            }
        }
    }

    partial class VerticalPositionType : IVerticalPosition { }

    partial class HorizontalPositionType : IHorizontalPosition { }
}
