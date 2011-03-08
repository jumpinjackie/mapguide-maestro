SET NS_ROOT=OSGeo.MapGuide.ObjectModels
SET XSD_OPTS=/c BindingList /is+ /cl+ /db+ /sc+ /xa+ /hp+ /if- /ll-
SET GEN_ROOT=..\..\Generated
xsd2code.exe ApplicationDefinition-1.0.0.xsd %GEN_ROOT%\ApplicationDefinition-1.0.0.cs /n %NS_ROOT%.ApplicationDefinition %XSD_OPTS%
xsd2code.exe ApplicationDefinitionInfo-1.0.0.xsd %GEN_ROOT%\ApplicationDefinitionInfo-1.0.0.cs /n %NS_ROOT%.ApplicationDefinition %XSD_OPTS%
xsd2code.exe FeatureSource-1.0.0.xsd %GEN_ROOT%\FeatureSource-1.0.0.cs /n %NS_ROOT%.FeatureSource %XSD_OPTS%
xsd2code.exe LayerDefinition-1.0.0.xsd %GEN_ROOT%\LayerDefinition-1.0.0.cs /n %NS_ROOT%.LayerDefinition %XSD_OPTS%
xsd2code.exe LayerDefinition-1.1.0.xsd %GEN_ROOT%\LayerDefinition-1.1.0.cs /n %NS_ROOT%.LayerDefinition %XSD_OPTS%
xsd2code.exe LayerDefinition-1.2.0.xsd %GEN_ROOT%\LayerDefinition-1.2.0.cs /n %NS_ROOT%.LayerDefinition %XSD_OPTS%
xsd2code.exe LayerDefinition-1.3.0.xsd %GEN_ROOT%\LayerDefinition-1.3.0.cs /n %NS_ROOT%.LayerDefinition %XSD_OPTS%
xsd2code.exe LoadProcedure-1.0.0.xsd %GEN_ROOT%\LoadProcedure-1.0.0.cs /n %NS_ROOT%.LoadProcedure %XSD_OPTS%
xsd2code.exe LoadProcedure-1.1.0.xsd %GEN_ROOT%\LoadProcedure-1.1.0.cs /n %NS_ROOT%.LoadProcedure %XSD_OPTS%
xsd2code.exe LoadProcedure-2.2.0.xsd %GEN_ROOT%\LoadProcedure-2.2.0.cs /n %NS_ROOT%.LoadProcedure %XSD_OPTS%
xsd2code.exe MapDefinition-1.0.0.xsd %GEN_ROOT%\MapDefinition-1.0.0.cs /n %NS_ROOT%.MapDefinition %XSD_OPTS%
xsd2code.exe PrintLayout-1.0.0.xsd %GEN_ROOT%\PrintLayout-1.0.0.cs /n %NS_ROOT%.PrintLayout %XSD_OPTS%
xsd2code.exe SymbolLibrary-1.0.0.xsd %GEN_ROOT%\SymbolLibrary-1.0.0.cs /n %NS_ROOT%.SymbolLibrary %XSD_OPTS%
xsd2code.exe SymbolDefinition-1.0.0.xsd %GEN_ROOT%\SymbolDefinition-1.0.0.cs /n %NS_ROOT%.SymbolDefinition %XSD_OPTS%
xsd2code.exe SymbolDefinition-1.1.0.xsd %GEN_ROOT%\SymbolDefinition-1.1.0.cs /n %NS_ROOT%.SymbolDefinition %XSD_OPTS%
xsd2code.exe WebLayout-1.0.0.xsd %GEN_ROOT%\WebLayout-1.0.0.cs /n %NS_ROOT%.WebLayout %XSD_OPTS%
xsd2code.exe WebLayout-1.1.0.xsd %GEN_ROOT%\WebLayout-1.1.0.cs /n %NS_ROOT%.WebLayout %XSD_OPTS%
xsd2code.exe FdoProviderCapabilities-1.1.0.xsd %GEN_ROOT%\FdoProviderCapabilities-1.1.0.cs /n %NS_ROOT%.Capabilities %XSD_OPTS%
xsd2code.exe FdoProviderCapabilities-1.0.0.xsd %GEN_ROOT%\FdoProviderCapabilities-1.0.0.cs /n %NS_ROOT%.Capabilities %XSD_OPTS%
rem Common 1.0.0
xsd2code.exe BatchPropertyCollection-1.0.0.xsd %GEN_ROOT%\BatchPropertyCollection-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe DataStoreList-1.0.0.xsd %GEN_ROOT%\DataStoreList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe DrawingSectionList-1.0.0.xsd %GEN_ROOT%\DrawingSectionList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe DrawingSectionResourceList-1.0.0.xsd %GEN_ROOT%\DrawingSectionResourceList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe DrawingSource-1.0.0.xsd %GEN_ROOT%\DrawingSource-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe Envelope-1.0.0.xsd %GEN_ROOT%\Envelope-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe FdoLongTransactionList-1.0.0.xsd %GEN_ROOT%\FdoLongTransactionList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe FdoSpatialContextList-1.0.0.xsd %GEN_ROOT%\FdoSpatialContextList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe FeatureProviderRegistry-1.0.0.xsd %GEN_ROOT%\FeatureProviderRegistry-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe FeatureSet-1.0.0.xsd %GEN_ROOT%\FeatureSet-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe Group-1.0.0.xsd %GEN_ROOT%\Group-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe GroupList-1.0.0.xsd %GEN_ROOT%\GroupList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe PlatformCommon-1.0.0.xsd %GEN_ROOT%\PlatformCommon-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe RepositoryContent-1.0.0.xsd %GEN_ROOT%\RepositoryContent-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe RepositoryList-1.0.0.xsd %GEN_ROOT%\RepositoryList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS% 
xsd2code.exe ResourceDataList-1.0.0.xsd %GEN_ROOT%\ResourceDataList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe ResourceDocumentHeader-1.0.0.xsd %GEN_ROOT%\ResourceDocumentHeader-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe ResourceFolderHeader-1.0.0.xsd %GEN_ROOT%\ResourceFolderHeader-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS% /eit+
xsd2code.exe ResourceList-1.0.0.xsd %GEN_ROOT%\ResourceList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe ResourcePackageManifest-1.0.0.xsd %GEN_ROOT%\ResourcePackageManifest-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe ResourceReferenceList-1.0.0.xsd %GEN_ROOT%\ResourceReferenceList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS% /eit+
xsd2code.exe ResourceSecurity-1.0.0.xsd %GEN_ROOT%\ResourceSecurity-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS% /eit+
xsd2code.exe Role-1.0.0.xsd %GEN_ROOT%\Role-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe SiteInformation-1.0.0.xsd %GEN_ROOT%\SiteInformation-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe SiteVersion-1.0.0.xsd %GEN_ROOT%\SiteVersion-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe SelectAggregate-1.0.0.xsd %GEN_ROOT%\SelectAggregate-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe Server-1.0.0.xsd %GEN_ROOT%\Server-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS% /eit+
xsd2code.exe ServerList-1.0.0.xsd %GEN_ROOT%\ServerList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS% /eit+
xsd2code.exe StringCollection-1.0.0.xsd %GEN_ROOT%\StringCollection-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe Types-1.0.0.xsd %GEN_ROOT%\Types-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe UnmanagedDataList-1.0.0.xsd %GEN_ROOT%\UnmanagedDataList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe User-1.0.0.xsd %GEN_ROOT%\User-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe UserList-1.0.0.xsd %GEN_ROOT%\UserList-1.0.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
rem Common 2.2.0
xsd2code.exe SiteVersion-2.2.0.xsd %GEN_ROOT%\UserList-2.2.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
xsd2code.exe SiteInformation-2.2.0.xsd %GEN_ROOT%\SiteInformation-2.2.0.cs /n %NS_ROOT%.Common %XSD_OPTS%
move *.cs %GEN_ROOT%