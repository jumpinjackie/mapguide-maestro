# Getting Started

Before you get started with the Maestro API, it helps to have an understanding of the following:
 
 * Programming in C# and/or the .net Framework. Most of the code examples illustrated here are in C#
 * Understanding the basics of MapGuide development and/or the MapGuide API. The Maestro API supplements MapGuide as an additional development option and carries many of the same concepts used in the official MapGuide API

## Why use the Maestro API?

As mentioned above, the Maestro API is an alternative option for MapGuide application development. Depending on your needs, the Maestro API may either complement the official MapGuide API or be a wholesale replacement. The Maestro API may be of interest to you if:

 * You prefer to work with MapGuide resources as strongly-typed objects instead of raw XML content
 * You require a strongly-typed service client interface to the http mapagent
 * You would like to build MapGuide applications/libraries that can work on **both** client and web tiers
 * You would like to build MapGuide applications/libraries that can easily integrate with the respective .net web (asp.net) and desktop (WinForms/WPF) platforms
 * You would like to build MapGuide applications/libraries in .net that can work on non-Windows platforms through .net Core

## Supported Platforms

The MapGuide Maestro API targets the following platforms:

 * .net Standard 2.0

## Acquiring the Maestro API

The MapGuide Maestro API is available as a [pre-release NuGet package](https://www.nuget.org/packages/OSGeo.MapGuide.MaestroAPI) and can be installed through the NuGet package manager in Visual Studio or the ``dotnet add package`` command from the dotnet CLI.

## Enabling SourceLink

The MapGuide Maestro API package is built with SourceLink support, which if enabled in your development environment will allow you to step into the actual MapGuide Maestro API source code while debugging.

To enable source link in Visual Studio, do the following:

1. Enable the NuGet Symbol Server
2. Disable "Just My Code"
3. Enable Source Server and Source Link Support
4. Step into Maestro API sources