;----------------------------------------------------------------------
; NSIS Installer script for MapGuide Maestro
; based off the same script for FDO Toolbox
;
;
; Author: Jackie Ng (jumpinjackie@gmail.com) 
;----------------------------------------------------------------------

;----------------------
; Include NSIS headers
;----------------------

# Modern UI 2
!include "MUI2.nsh"

# File functions
!include "FileFunc.nsh"

!include "WordFunc.nsh"
!include "LogicLib.nsh"

;-------------------------------
; Installer compilation settings
;-------------------------------

SetCompressor /SOLID /FINAL lzma

;-------------------------------
; Windows > Vista settings
;-------------------------------

RequestExecutionLevel admin

;-------------------
; Script variables
;-------------------

# Globals
!ifndef SLN_CONFIG
	!define SLN_CONFIG "Release" #"Debug"
!endif

!ifndef CPU
	!define CPU "x86"
!endif

!echo "Building installer in configuration: ${SLN_CONFIG} (${CPU})"

!define SLN_DIR ".."
!define SLN_THIRDPARTY "${SLN_DIR}\Thirdparty"
!ifndef RELEASE_VERSION
	!define RELEASE_VERSION "Trunk"
!endif

# Installer vars
!define INST_PRODUCT "MapGuide Maestro"
!if ${SLN_CONFIG} == "Release"
!define INST_PRODUCT_QUALIFIED "${INST_PRODUCT} ${RELEASE_VERSION}"
!define INST_PRODUCT_NAME "${INST_PRODUCT} ${RELEASE_VERSION}"
#	!define INST_PRODUCT_QUALIFIED "${INST_PRODUCT}"
#	!define INST_PRODUCT_NAME "${INST_PRODUCT_QUALIFIED} ${RELEASE_VERSION}"
!else
!error "We don't make debug installers"
#	!define INST_PRODUCT_QUALIFIED "${INST_PRODUCT} (Debug)"
#	!define INST_PRODUCT_NAME "${INST_PRODUCT_QUALIFIED} ${RELEASE_VERSION}"
!endif

!define PROJECT_URL "http://trac.osgeo.org/mapguide/wiki/maestro"
!define INST_SRC "."
!define INST_LICENSE "..\Maestro\license.txt"
#!define INST_OUTPUT "MapGuide Maestro-${SLN_CONFIG}-${RELEASE_VERSION}-${CPU}-Setup.exe"
!define INST_OUTPUT "MapGuide Maestro-${SLN_CONFIG}-${RELEASE_VERSION}-Setup.exe"

# We'll disable this for preview releases, because release version will not be a valid version string

#!if "${RELEASE_VERSION}" != "Trunk"
#	VIProductVersion "${RELEASE_VERSION}"
#	VIAddVersionKey "ProductName" "${INST_PRODUCT_NAME}"
#	VIAddVersionKey "LegalCopyright" "© 2011 Jackie Ng"
#	VIAddVersionKey "FileDescription" "Installer package for MapGuide Maestro"
#	VIAddVersionKey "FileVersion" "${RELEASE_VERSION}"
#!endif

!define REG_KEY_UNINSTALL "Software\Microsoft\Windows\CurrentVersion\Uninstall\${INST_PRODUCT_QUALIFIED}"

# Project Output
#!define INST_OUTPUT_MAESTRO "${SLN_DIR}\out\${CPU}\${SLN_CONFIG}"
!define INST_OUTPUT_MAESTRO "${SLN_DIR}\out\${SLN_CONFIG}"
!define INST_OUTDIR "${SLN_DIR}\out"

# Executables
!define EXE_MAESTRO "Maestro.exe"

# Shortcuts
!define LNK_MAESTRO "${INST_PRODUCT_QUALIFIED}"

;-------------------
; General
;-------------------

; Name and file
Name "${INST_PRODUCT}"
Caption "${INST_PRODUCT_NAME} Setup"
OutFile "${INST_OUTDIR}\${INST_OUTPUT}"

; Default installation folder
; For preview releases, use $INST_PRODUCT_NAME. Once ready for prime-time, switch to $INST_PRODUCT
!if ${CPU} == "x64"
InstallDir "$PROGRAMFILES64\OSGeo\${INST_PRODUCT_NAME}"
!else
InstallDir "$PROGRAMFILES\OSGeo\${INST_PRODUCT_NAME}"
!endif

!ifdef INST_LICENSE
LicenseText "License"
LicenseData "${INST_SRC}\${INST_LICENSE}"
!endif

;-------------------
; Interface Settings
;-------------------
!define MUI_ABORTWARNING

;-------------------
; Pages
;-------------------

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "${INST_LICENSE}"
#!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
    # These indented statements modify settings for MUI_PAGE_FINISH
    !define MUI_FINISHPAGE_NOAUTOCLOSE
    !define MUI_FINISHPAGE_RUN
    !define MUI_FINISHPAGE_RUN_CHECKED
    !define MUI_FINISHPAGE_RUN_TEXT "Run MapGuide Maestro"
    !define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;-------------------
; Languages
;-------------------

!insertmacro MUI_LANGUAGE "English"

;-------------------
; Installer Sections
;-------------------

!define HELP_USER "FDOToolbox.chm"
!define HELP_API "FDO Toolbox Core API.chm"

# default section
Section 
	; Windows > Vista
	SetShellVarContext all

	; Registry
	!if ${CPU} == "x64"
	SetRegView 64
	!else
	SetRegView 32
	!endif

	# set installation dir
	SetOutPath $INSTDIR
	
	# directories
	File /r "${INST_OUTPUT_MAESTRO}\WebStudio"
	File /r "${INST_OUTPUT_MAESTRO}\AddIns"
	
	# docs
	#File "${INST_OUTPUT_MAESTRO}\${HELP_USER}"
	#File "${INST_OUTPUT_MAESTRO}\${HELP_API}"
	File "${INST_OUTPUT_MAESTRO}\changelog.txt"
	File "${INST_OUTPUT_MAESTRO}\license.txt"
	
	# data/config files
    File "${INST_OUTPUT_MAESTRO}\Maestro.exe.config"
    File "${INST_OUTPUT_MAESTRO}\MaestroFsPreview.exe.config"
	File "${INST_OUTPUT_MAESTRO}\ConnectionProviders.xml"
    File "${INST_OUTPUT_MAESTRO}\FsEditorMap.xml"
    File "${INST_OUTPUT_MAESTRO}\OdbcDriverMap.xml"
	
	# libraries
	File "${INST_OUTPUT_MAESTRO}\Aga.Controls.dll"
    File "${INST_OUTPUT_MAESTRO}\Ciloci.Flee.dll"
	File "${INST_OUTPUT_MAESTRO}\ICSharpCode.Core.dll"
	File "${INST_OUTPUT_MAESTRO}\ICSharpCode.Core.WinForms.dll"
    File "${INST_OUTPUT_MAESTRO}\ICSharpCode.SharpZipLib.dll"
    File "${INST_OUTPUT_MAESTRO}\MapGuideDotNetApi.dll"
	File "${INST_OUTPUT_MAESTRO}\Maestro.Base.dll"
	File "${INST_OUTPUT_MAESTRO}\Maestro.Editors.dll"
	File "${INST_OUTPUT_MAESTRO}\Maestro.Login.dll"
    File "${INST_OUTPUT_MAESTRO}\Maestro.Packaging.dll"
	File "${INST_OUTPUT_MAESTRO}\Maestro.Shared.UI.dll"
	File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.MaestroAPI.dll"
	File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.MaestroAPI.Http.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.MaestroAPI.Native.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.LayerDefinition-1.1.0.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.LayerDefinition-1.2.0.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.LayerDefinition-1.3.0.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.LoadProcedure-1.1.0.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.LoadProcedure-2.2.0.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.SymbolDefinition-1.1.0.dll"
    File "${INST_OUTPUT_MAESTRO}\OSGeo.MapGuide.ObjectModels.WebLayout-1.1.0.dll"
	File "${INST_OUTPUT_MAESTRO}\Topology.dll"
    File "${INST_OUTPUT_MAESTRO}\Topology.IO.MapGuide.dll"
	
	# main executables
	File "${INST_OUTPUT_MAESTRO}\${EXE_MAESTRO}"
	File "${INST_OUTPUT_MAESTRO}\MgCooker.exe"
    File "${INST_OUTPUT_MAESTRO}\MgCookerCmd.exe"
    File "${INST_OUTPUT_MAESTRO}\MaestroFsPreview.exe"
	
	# create uninstaller
	WriteUninstaller "$INSTDIR\uninstall.exe"
	
	# create Add/Remove Programs entry
	WriteRegStr HKLM "${REG_KEY_UNINSTALL}" \
					 "DisplayName" "${INST_PRODUCT_NAME}"

	WriteRegStr HKLM "${REG_KEY_UNINSTALL}" \
					 "UninstallString" "$INSTDIR\uninstall.exe"
	
	WriteRegStr HKLM "${REG_KEY_UNINSTALL}" \
					 "URLInfoAbout" "${PROJECT_URL}"
	
	WriteRegStr HKLM "${REG_KEY_UNINSTALL}" \
					 "DisplayVersion" "${RELEASE_VERSION}"
	
	# TODO: Add more useful information to Add/Remove programs
	# See: http://nsis.sourceforge.net/Add_uninstall_information_to_Add/Remove_Programs
	
	# create Maestro shortcuts. Use INST_PRODUCT_QUALIFIED so we can have x86 and x64 entries that don't clash
	CreateDirectory "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}"
	
	CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\${LNK_MAESTRO}.lnk" "$INSTDIR\${EXE_MAESTRO}"
	#CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\MgCooker.lnk" "$INSTDIR\MgCooker.exe"
    #CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\Maestro Feature Source Preview.lnk" "$INSTDIR\MaestroFsPreview.exe"
	#CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\User Documentation.lnk" "$INSTDIR\${HELP_USER}"
	#CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\Core API Documentation.lnk" "$INSTDIR\${HELP_API}"
	CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
	
	CreateShortCut "$DESKTOP\${LNK_MAESTRO}.lnk" "$INSTDIR\${EXE_MAESTRO}"
	
SectionEnd

# uninstall section
Section "uninstall"
	; Windows > Vista
	SetShellVarContext all

	; Registry
	!if ${CPU} == "x64"
	SetRegView 64
	!else
	SetRegView 32
	!endif

    # remove uninstaller
	Delete "$INSTDIR\uninstall.exe"
	
	# remove desktop shortcut
	Delete "$DESKTOP\${LNK_MAESTRO}.lnk"
	
	# remove Add/Remove programs registry entry
	DeleteRegKey HKLM "${REG_KEY_UNINSTALL}"
	
	# remove installation directory
	RMDir /r "$INSTDIR"
	
	# remove shortcuts
	RMDir /r "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}"
SectionEnd

Function .onInit
	; Registry
	!if ${CPU} == "x64"
	SetRegView 64
	!else
	SetRegView 32
	!endif
	
	!insertmacro MUI_LANGDLL_DISPLAY
  
	; Check .NET version
	ReadRegDWORD $0 HKLM 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727' SP
	
	; SP level of 1 or higher is enough
	${If} $0 < 1
		MessageBox MB_OK|MB_ICONINFORMATION "${INST_PRODUCT_QUALIFIED} requires that the .net Framework 2.0 SP1 or above is installed. Please download and install the .net Framework 2.0 SP1 or above before installing ${INST_PRODUCT}."
	    Quit
	${EndIf}
FunctionEnd

Function LaunchLink
	; TODO: Needs to launch under standard user. If installer was run under UAC elevated privileges, it will run under the
	; user who elevated these privileges.
	ExecShell "" "$INSTDIR\${EXE_MAESTRO}"
FunctionEnd
