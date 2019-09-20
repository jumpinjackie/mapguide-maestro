# About Maestro.MapPublisher

`Maestro.MapPublisher` is a standalone tool for publishing static interactive maps from an existing MapGuide Server. These published maps are completely decoupled from the MapGuide Server you are publishing from (should you so choose).

# Usage

 1. Create a publish profile (refer to `Publish Profile Reference` below for notes on creating one)
 2. Run the following command
```
Maestro.MapPublisher.exe publish --publish-profile-path <path to your publish profile>
```
# Features

 * Creates an interactive map based on either `OpenLayers` or `Leaflet` that is optionally focused on:
   * A XYZ image tileset definition of your choosing
   * A XYZ UTFGrid tileset definition of your choosing
   * Both tilesets can be downloaded locally (completely decoupling the tiles from the MapGuide Server and also ad-hoc seeding the tiles in the process), or the published map can be set up to consume tiles directly from the MapGuide Server.
 * Can include the following external base layers:
   * Bing Maps (API key required. [Sign up for one here](https://www.bingmapsportal.com))
   * OpenStreetMap
   * [Stamen Tiles](http://maps.stamen.com/)
   * An arbitrary XYZ tile set of your choice
 * Can include any number of the following overlay layers:
   * A WFS layer (currently not supported if using `Leaflet` template)
   * A WMS service
   * A GeoJSON file downloaded from a MapGuide Layer Definition of your choice:
      * The publisher will attempt basic translation of styling (see `Limitations` below)

# Publish Profile Reference

A publish profile controls what the published interactive map will look like. Below is an example profile.

```
{
    "Title": "MapGuide Published Map",
    "MaxDegreeOfParallelism": 16,
    "MapAgent": "http://localhost/mapguide/mapagent/mapagent.fcgi",
    "OutputDirectory": "D:/temp/StaticPublish",
    "Username": "Anonymous",
    "Password": "",
    "RandomizeRequests": true,
    "OutputPageFileName": "index_ol.html",
    "ViewerOptions": {
        "Type": "OpenLayers"
    },
    "Bounds": {
        "MinX": -87.764986990962839,
        "MinY": 43.691398128787782,
        "MaxX": -87.695521510899724,
        "MaxY": 43.797520000480347
    },
    "ExternalBaseLayers": [
        {
            "Name": "Bing Maps",
            "Type": "BingMaps",
            "LayerType": "AerialWithLabels",
            "ApiKey": "MY_BING_MAPS_API_KEY"
        },
        {
            "Name": "OpenStreetMap",
            "Type": "OSM"
        },
        {
            "Name": "Stamen Toner",
            "Type": "Stamen",
            "LayerType": "Toner"
        },
        {
            "Name": "Stamen Watercolor",
            "Type": "Stamen",
            "LayerType": "WaterColor"
        }
    ],
    "OverlayLayers": [
        {
            "Name": "FWS Gov WMS (Wetlands)",
            "Type": "WMS",
            "Service": "https://fwspublicservices.wim.usgs.gov/server/services/Wetlands/MapServer/WmsServer?",
            "Layer": "1"
        },
        {
            "Name": "Unified School Districts, Wisconsin, 2000",
            "Type": "WFS",
            "Service": "https://geowebservices.stanford.edu/geoserver/wfs",
            "FeatureName": "druid:sm765cr3652"
        },
        {
            "Name": "GeoJSON - Parcels",
            "Type": "GeoJSON_FromMapGuide",
            "Source": {
                "Origin": "LayerDefinition",
                "Precision": 6,
                "LoadAsVectorTiles": true,
                "LayerDefinition": "Library://Samples/Sheboygan/Layers/Parcels.LayerDefinition"
            }
        },
        {
            "Name": "GeoJSON - Parks",
            "Type": "GeoJSON_FromMapGuide",
            "Source": {
                "Origin": "LayerDefinition",
                "Precision": 6,
                "LayerDefinition": "Library://Samples/Sheboygan_OpenData/Layers/Parks.LayerDefinition"
            }
        },
        {
            "Name": "GeoJSON - Corporate Limits",
            "Type": "GeoJSON_FromMapGuide",
            "Source": {
                "Origin": "LayerDefinition",
                "Precision": 6,
                "LayerDefinition": "Library://Samples/Sheboygan_OpenData/Layers/Corporate_Limits.LayerDefinition"
            }
        }
    ],
    "ImageTileSet": {
        "ResourceID": "Library://Samples/Sheboygan/TileSets/SheboyganXYZ.TileSetDefinition",
        "Mode": "Remote",
        "SkipTileDownloading": true
    },
    "UTFGridTileSet": {
        "ResourceID": "Library://Samples/Sheboygan/TileSets/SheboyganUTFGrid.TileSetDefinition",
        "Mode": "Remote",
        "SkipTileDownloading": true
    }
}
```

All options are explained below

## Title

This is the title of the HTML page for the interactive map

## MaxDegreeOfParallelism

This is a ***Tile Download*** control value and defines how many concurrent tile requests to send in a single batch before the seeder *awaits* for all of them to complete before starting the next concurrent batch.

If you are not downloading tiles, this value has no effect.

## MapAgent

The web tier endpoint of the MapGuide Server.

If seeding tiles or downloading GeoJSON data, this is the endpoint it will contact.

## Username/Password

Credentials required for the web tier endpoint. If not seeding tiles (but consuming them directly), these credentials are baked into the tile source URLs.

## RandomizeRequests

This is a ***Tile Download*** control value and if set to ***true*** will randomize the list of tiles to fetch. This is best used when the tile set being seeded [is set up for meta-tiling](https://themapguyde.blogspot.com/2019/09/mapguide-40-showcase-supercharged-tile.html) as randomized requests means reduced lock contention on adjacent tiles when a meta-tile is sliced up to be saved to the tile cache.

Randomized tile requests means less likelihood of requesting adjacent tiles at the same time.

## OutputDirectory/OutputPageFileName

If ***Tile Download*** is enabled, the tiles will be downloaded to a folder relative to the specified `OutputDirectory` and the viewer will set up tile sources to point to these directories.

If GeoJSON data is to be downloaded, it will be downloaded to a `vector_overlays` subdirectory of `OutputDirectory` and the viewer will set up the vector sources for these files.

The publisher will also generate the viewer HTML page in the specified `OutputDirectory` with the specified `OutputPageFileName`. If `OutputPageFileName` is not given, it will default to `index.html`. Any required viewer assets are also copied to the `assets` subdirectory of the specified directory.

## ViewerOptions

Currently, this is only to specify whether you want the `OpenLayers` or `Leaflet` template. In the future, we may introduce new `OpenLayers` or `Leaflet` specific feature toggles, which you can control in this section.

## Bounds

This has 2 purposes:

 1. To define the "area of interest" of your interactive map. This will be the initial and maximum bounds of your interactive map.
 2. If ***Tile Download*** is enabled, this is used to compute the full list of tiles that need to be downloaded/seeded.

The bounds must be defined in latitude/longitude (`EPSG:4326`, MapGuide:`LL84`) coordinates.

## ImageTileSet

Specifies the tile set definition (`ResourceID`) that is to be the focal point of this interactive map.

The specified tile set must be set to serve `PNG` tiles and cannot use retina mode (ie. `RetinaScale` must not be set). This tile set may use meta-tiling.

You may specify the specific group (`GroupName`) within the tile set whose tiles you want to read from. Otherwise the publisher will default to the first base layer group found in the tile set.

The `Mode` property determines how this tile set will be consumed by the published interactive map:

 * If the `Mode` is `Local`, the publisher will enter ***Tile Download*** mode and will download tiles within the specified `Bounds` to a local directory relative to the specified `OutputDirectory`. This has the side-effect of also seeding the tile cache for this tile set for the given bounds so this also functions as an ad-hoc tile seeder. The published viewer will have its tile sources set up to read from this subdirectory instead of the MapGuide Server.
 * If the `Mode` is `Remote`, the publisher will not download any tiles and the published viewer will set up its tile sources to consume said tile set directly from the MapGuide Server based on the provided `MapAgent` setting.

***NOTE:*** Just like regular tile seeding, the time taken to download all the required tiles is a function of the (geographical) size of your map. The bigger the geographic bounds, the longer it will take.

***NOTE:*** It is assumed the published map will most likely be put on a public-facing web site. If you will be using the `Mode` of `Remote` then it is assumed your MapGuide Server also has a public-facing mapagent endpoint. Remember not to use `localhost` in your `MapAgent` but the public-facing host name of your mapagent endpoint.

## UTFGridTileSet

Everything said about `ImageTileSet` above also applies here, with the following differences:

 * The specified tile set must be set to serve `UTFGRID` tiles

## External Base Layers

You may specify zero or many external base layer definitions. These layers sit below your image/UTFGrid tile set and overlays and are mutually exclusive (ie. Only one of these external base layers can be shown at any one time)

The `Name` specifies the name of this layer when represented in the layer switcher/control in the interactive map.

The `Type` can be one of the following:
 * `OSM`
 * `BingMaps`
   * You must also provide the following additional properties:
     * `ApiKey`: Your bing maps API key
     * `LayerType`: The bing imagery set to use
 * `Stamen`
   * You must also provide the following additional properties:
     * `LayerType`: The stamen tile set to use
 * `XYZ`
   * You must also provide the following additional properties:
     * `UrlTemplate`: The URL of the XYZ tile set with `{x}`, `{y}` and `{z}` placeholder tokens.

## Overlay Layers

You may specify zero or many overlay layers. These layers sit above your external base layers and your image/UTFGrid tile sets.

The `Name` specifies the name of this layer when represented in the layer switcher/control in the interactive map.

The `Type` can be one of the following:
 * `WMS`
   * You must also provide the following additional properties:
     * `Service`: The WMS service endpoint
     * `Layer`: The name of the WMS layer
   * Inspect the `GetCapabilities` response of your WMS service to determine the correct `Service` and `Layer` values.
 * `WFS`
   * You must also provide the following additional properties:
     * `Service`: The WFS service endpoint
     * `FeatureName`: The name of the WFS layer
   * This layer is not supported for the `Leaflet` template
   * For the `OpenLayers` template, this layer will be set up with a `bbox` loading strategy.
   * Inspect the `GetCapabilities` response of your WFS service to determine the correct `Service` and `Layer` values.
 * `GeoJSON_FromMapGuide`
   * You must provide a `Source` property that contains the following properties:
      * `Origin`: Right now, this must always be `LayerDefinition`
      * `LayerDefinition`: The Layer Definition to download GeoJSON data from. The publisher will also attempt basic conversion of the styles defined for use in `OpenLayers` or `Leaflet` subject to various limitations (see below). If property mappings are defined in this Layer Definition, they will make this layer selectable and allow for display of its attributes in a popup when any feature from this layer is selected in the interactive map.
      * `Precision`: Optional. If specified, coordinates are capped at the specified number of decimal places, reducing overall file size of the GeoJSON file as a result (those extra decimals add up!). If not specified, coordinates are output as-is, which in some cases may be up to 15 decimal places, which is probably excessive.
      * `LoadAsVectorTiles`: If `true`, extra setup code will be generated so that the GeoJSON data is loaded as on-the-fly vector tiles (using the [geojson-vt library](https://github.com/mapbox/geojson-vt)). This is an option you generally want to activate for big GeoJSON data sources (size or feature count). Not supported in the `Leaflet` template.

# Limitations

 * The publisher can only work with ***MapGuide Open Source 4.0 and newer*** as it takes advantage of APIs that have been introduced with MGOS 4.0. This requirement is ***non-negotiable***.
 * The map published will always be based on Web Mercator (`EPSG:3857`). All vector data will be reprojected to this projection if required. Any GeoJSON data downloaded from MapGuide will be re-projected as part of downloading.
 * The `Leaflet` template does not support the following:
   * WFS overlay layers
   * The `LoadAsVectorTiles` option
 * For GeoJSON sources downloaded from MapGuide, basic style conversion will be attempted to preserve as much of the defined style to the published map. For best results, the layers ***must not*** be based on [Advanced/Composite Stylization](https://trac.osgeo.org/mapguide/wiki/AdvancedStylization) and:
   * Should only have one vector scale range. If there are multiple, only the first one is chosen. The visible range itself is not preserved during conversion.
   * If the layer has a `Hyperlink` set, it must be a FDO expression which is a reference to an existing property and nothing more complex. If you must have computed hyperlinks that involves a complex FDO expression, you can workaround the limitation by using the `Extended Feature Class` capability and define a computed hyperlink property with your computed FDO expression. Then update the layer to point to this extended feature class and update the `Hyperlink` to point to this new computed property.
   * If the layer has thematic rules, the thematic rules should only use comparison FDO filters (eg. `PROPERTY = 'some_value'`). Conversion of `LIKE` comparison filters are not supported.
   * For area (polygon) styles:
      * Will pick up fill/outline colors and thicknesses
      * Will assume a `Solid` fill. Any fill pattern is ignored.
   * For line styles:
      * Will pick up color and thickness
      * Will assume a `Solid` line pattern. Any line pattern is ignored.
   * For point styles:
      * Should be styled with basic shapes (Mark symbolization). Any other symbolization type is ignored.
        * Will pick up specified shape and symbol size
   * For any defined color and thickness
      * Must use *constant* values. They cannot be an FDO expression.
      * Thickness values are assumed to be in `Device Space`. If thickness values are in `Map Space`, it will still be treated as `Device Space`