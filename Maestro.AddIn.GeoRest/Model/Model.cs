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

namespace Maestro.AddIn.GeoRest.Model
{
    public class RepresentationPreview
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }

    public class FileSystemEntry
    {
        public string Name { get; set; }

        public bool IsFolder { get; set; }
    }

    public class Resource
    {
        public string UriPart { get; set; }

        public Source Source { get; set; }

        public List<Representation> Representations { get; set; }
    }

    public abstract class Source
    {
        public SourceType Type { get; set; }
    }

    public enum SourceType
    {
        FDO,
        MapGuide
    }

    public class FdoSource : Source 
    {
        public string Provider { get; set; }

        public string ConnectionString { get; set; }

        public string FeatureClass { get; set; }
    }

    public class MapGuideSource : Source
    { 
    }

    public abstract class Representation
    {
        public string Renderer { get; private set; }

        public List<Method> Methods { get; set; }

        public RepresentationOrdering Ordering { get; set; }
    }

    public class RepresentationOrdering
    {
        public string Fields { get; set; }

        public string Direction { get;set; }
    }

    public abstract class RepresentationWithPattern
    {
        public string Pattern { get; set; }
    }

    public class JsonRepresentation : RepresentationWithPattern
    {

    }

    public class XmlRepresentation : RepresentationWithPattern
    {
        
    }

    public abstract class Method
    {
        public RestMethod Name { get; set; }
    }

    public class GetMethod
    {
        public int? MaxCount { get; set; }

        public double? MaxBBOXWidth { get; set; }

        public double? MaxBBOXHeight { get; set; }
    }

    public class WriteMethod
    {
        
    }

    public enum RestMethod
    {
        GET,
        PUT,
        POST,
        DELETE
    }

    public class Png8Representation : RepresentationWithPattern
    {
        public string MapDefinition { get; set; }

        public string SelectionLayer { get; set; }

        public double DefaultZoomFactor { get; set; }
    }

    public class TemplateRepresentation : RepresentationWithPattern
    {
        public string MimeType { get; set; }

        public TemplateSettings Templates { get; set; }
    }

    public class TemplateSettings
    {
        public string Section { get; set; }

        public string Prefix { get; set; }

        public TemplateReference Many { get; set; }
        public TemplateReference Single { get; set; }
        public TemplateReference None { get; set; }
        public TemplateReference Error { get; set; }
    }

    public class TemplateReference
    {
        public string File { get; set; }
    }

    public class ODataRepresentation : Representation
    {
        public ODataFeedOverride FeedOverride { get; set; }

        public ODataEntryOverride EntryOverride { get; set; }
    }

    public class ODataFeedOverride : Representation
    {
        public string Title { get; set; }

        public ODataAuthor Author { get; set; }
    }

    public class ODataAuthor
    {
        public string Name { get; set; }

        public string Uri { get; set; }

        public string Email { get; set; }
    }

    public class ODataEntryOverride : Representation
    {
        public string Title { get; set; }

        public ODataAuthor Author { get; set; }
    }
}
