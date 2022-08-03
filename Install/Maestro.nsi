;----------------------------------------------------------------------
; NSIS Installer script for MapGuide Maestro
; based off the same script for FDO Toolbox
;
; Author: Jackie Ng (jumpinjackie@gmail.com) 
;----------------------------------------------------------------------

!addplugindir .\NSISPlugins

;TODO: Figure out how to brand this installer

;----------------------
; Include NSIS headers
;----------------------

# Modern UI 2
!include "MUI2.nsh"

# File functions
!include "FileFunc.nsh"

!include "WordFunc.nsh"
!include "LogicLib.nsh"

# VCRedist detection
!include "VCRedist.nsh"
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
Icon "Maestro.ico"
LicenseData "LGPL21.rtf"

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

!define PROJECT_URL "https://github.com/jumpinjackie/mapguide-maestro"
!define INST_SRC "."
!define INST_LICENSE "..\Maestro\license.txt"
#!define INST_OUTPUT "MapGuideMaestro-${SLN_CONFIG}-${RELEASE_VERSION}-${CPU}-Setup.exe"
!define INST_OUTPUT "MapGuideMaestro-${SLN_CONFIG}-${RELEASE_VERSION}-Setup.exe"

# We'll disable this for preview releases, because release version will not be a valid version string

#!if "${RELEASE_VERSION}" != "Trunk"
#	VIProductVersion "${RELEASE_VERSION}"
#	VIAddVersionKey "ProductName" "${INST_PRODUCT_NAME}"
#	VIAddVersionKey "LegalCopyright" "� 2011-2022 Jackie Ng"
#	VIAddVersionKey "FileDescription" "Installer package for MapGuide Maestro"
#	VIAddVersionKey "FileVersion" "${RELEASE_VERSION}"
#!endif

!define REG_KEY_UNINSTALL "Software\Microsoft\Windows\CurrentVersion\Uninstall\${INST_PRODUCT_QUALIFIED}"

# Project Output
!define INST_OUTPUT_MAESTRO "${SLN_DIR}\out\publish\${SLN_CONFIG}"
!define INST_OUTDIR "${SLN_DIR}\artifacts"

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
InstallDir "$PROGRAMFILES64\${INST_PRODUCT_NAME}"
!else
InstallDir "$PROGRAMFILES\${INST_PRODUCT_NAME}"
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
    
    # directories / core addins
    File /r "${INST_OUTPUT_MAESTRO}\AddIns"
    File /r "${INST_OUTPUT_MAESTRO}\Data"
    File /r "${INST_OUTPUT_MAESTRO}\Schemas"

    # Stdlib for IronPython
    File /r "${INST_OUTPUT_MAESTRO}\Lib"

    # Support files for Maestro.MapPublisher
    File /r "${INST_OUTPUT_MAESTRO}\viewer_content"
    
    # docs
    File "${INST_OUTPUT_MAESTRO}\*.txt"
    
    # data/config files
    File "${INST_OUTPUT_MAESTRO}\*.config"
    File "${INST_OUTPUT_MAESTRO}\*.xml"
    
    # libraries
    File "${INST_OUTPUT_MAESTRO}\*.dll"
    
    # pdbs for greater context in exception stack traces
    #File "${INST_OUTPUT_MAESTRO}\Aga.Controls.pdb"
    File "${INST_OUTPUT_MAESTRO}\*.pdb"

    # main executables
    File "${INST_OUTPUT_MAESTRO}\*.exe"

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
    CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\Maestro Feature Source Preview.lnk" "$INSTDIR\MaestroFsPreview.exe"
    CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\Live Map Definition Editor.lnk" "$INSTDIR\Maestro.LiveMapEditor.exe"
    CreateShortCut "$SMPROGRAMS\${INST_PRODUCT_QUALIFIED}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
    
    CreateShortCut "$DESKTOP\${LNK_MAESTRO}.lnk" "$INSTDIR\${EXE_MAESTRO}"

    # Run LocalConfigure (we should be elevated here)
    Exec "$INSTDIR\LocalConfigure.exe"
    
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
FunctionEnd
