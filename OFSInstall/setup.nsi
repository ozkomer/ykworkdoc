;NSIS Modern User Interface
;Multilingual Example Script
;Written by Joost Verburg, modified by yeer 2010-07-23


;--------------------------------
;Include Setting;类似C语言的头文件
  !include "MUI2.nsh"
  !include "Logiclib.nsh"
  !include  "nsDialogs.nsh"

	
;--------------------------------
;General
  Name "OrderFormSystem"	
  OutFile "Setup.exe"
  
  InstallDir "$PROGRAMFILES\OrderFormSystem"		;缺省安装目录
  InstallDirRegKey HKCU "Software\OrderFormSystem" ""	;写入注册表值
  RequestExecutionLevel highest  		;win7支持


;--------------------------------
;Global Defining
  ;产品名称
  !define PRODUCT_NAME "OrderFormSystem"
  ;可执行文件名
  !define PRODUCT_MAINEXE "OrderFormSystem.exe"
  ;版本号
  !define PRODUCT_VERSION "1.0"
  ;发行者
  !define PRODUCT_PUBLISHER "yeer"
  ;网址
  !define PRODUCT_WEB_SITE "www.xiami.com"
  ;开机运行目录
  !define PRODUCT_REGRUN "Software\Microsoft\Windows\CurrentVersion\Run"
  ;产品发布者在注册表的路径
  !define PRODUCT_PUBLISHER_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_PUBLISHER}"
  ;产品在注册表的安装路径
  !define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
  ;产品在注册表卸载路径
  !define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  ;卸载根目录
  !define PRODUCT_UNINST_ROOT_KEY "HKLM"


;--------------------------------
;Interface Settings
  !define MUI_ICON "image\lavrock.ico"                       ;安装程序图标
  !define MUI_UNICON "image\uninstall.ico"                   ;卸载程序图标
  !define MUI_HEADERIMAGE                                    ;使用上方横幅图片
  !define MUI_HEADERIMAGE_BITMAP "image\header.bmp"          ;安装开始和结束时候上方横幅图片路径
  !define MUI_HEADERIMAGE_UNBITMAP "image\header-uninstall.bmp"        ;卸载开始和结束时候上方横幅图片路径
  !define MUI_WELCOMEFINISHPAGE_BITMAP "image\orange.bmp"     ;安装开始和结束时候左边竖立的图片路径
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP "image\orange-uninstall.bmp"   ;卸载开始和结束时候左边竖立的图片路径
  !define MUI_FINISHPAGE_RUN "$INSTDIR\${PRODUCT_MAINEXE}"   ;安装成功后是否启动运行程序,带复选框,默认勾上
  !define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\License.txt"   ;安装成功后是否打开自述文件,带复选框
  !define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED               ;打开自述文件默认不够上
  !define MUI_ABORTWARNING                                   ;忽略警告
  

;------------------------------
;repair page settings
  Var UninstallFileName        ;卸载程序
  Var RADIO_REPAIR             ;修复单选按钮
  Var RADIO_REMOVE             ;除去卸载单选按钮
  Var Checkbox_State_REPAIR    ;修复单选按钮选中状态
  Var Checkbox_State_REMOVE    ;除去卸载单选按钮选中状态
  Var Checkbox_State           ;按钮选中状态


;--------------------------------
;Language Selection Dialog Settings
;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU"                       ;多语言注册表根目录
  !define MUI_LANGDLL_REGISTRY_KEY "Software\OrderFormSystem"            ;多语言注册表子路径键值
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"    ;当前语言类型


;--------------------------------
;Pages
  !insertmacro MUI_PAGE_WELCOME                                  ;安装程序欢迎界面
  Page custom nsDialogsRepair nsDialogsRepairLeave               ;自带探测是否已经安装了本程序的页面
  !insertmacro MUI_PAGE_LICENSE "release\License.txt"            ;自述协议文件路径
  !insertmacro MUI_PAGE_DIRECTORY                                ;路径选择
  !insertmacro MUI_PAGE_COMPONENTS                               ;组件选择
  !insertmacro MUI_PAGE_INSTFILES                                ;开始安装
  !insertmacro MUI_PAGE_FINISH                                   ;安装成功
  
  !insertmacro MUI_UNPAGE_WELCOME                                ;卸载欢迎界面
  !insertmacro MUI_UNPAGE_CONFIRM                                ;卸载确认界面
  !insertmacro MUI_UNPAGE_INSTFILES                              ;卸载程序
  !insertmacro MUI_UNPAGE_FINISH                                 ;卸载完成


;--------------------------------
;Languages
  !insertmacro MUI_LANGUAGE "English" ;first language is the default language
  !insertmacro MUI_LANGUAGE "French"
  !insertmacro MUI_LANGUAGE "German"
  !insertmacro MUI_LANGUAGE "Spanish"
  !insertmacro MUI_LANGUAGE "SpanishInternational"
  !insertmacro MUI_LANGUAGE "SimpChinese"
  !insertmacro MUI_LANGUAGE "TradChinese"
  !insertmacro MUI_LANGUAGE "Japanese"
  !insertmacro MUI_LANGUAGE "Korean"
  !insertmacro MUI_LANGUAGE "Italian"
  !insertmacro MUI_LANGUAGE "Dutch"
  !insertmacro MUI_LANGUAGE "Danish"
  !insertmacro MUI_LANGUAGE "Swedish"
  !insertmacro MUI_LANGUAGE "Norwegian"
  !insertmacro MUI_LANGUAGE "NorwegianNynorsk"
  !insertmacro MUI_LANGUAGE "Finnish"
  !insertmacro MUI_LANGUAGE "Greek"
  !insertmacro MUI_LANGUAGE "Russian"
  !insertmacro MUI_LANGUAGE "Portuguese"
  !insertmacro MUI_LANGUAGE "PortugueseBR"
  !insertmacro MUI_LANGUAGE "Polish"
  !insertmacro MUI_LANGUAGE "Ukrainian"
  !insertmacro MUI_LANGUAGE "Czech"
  !insertmacro MUI_LANGUAGE "Slovak"
  !insertmacro MUI_LANGUAGE "Croatian"
  !insertmacro MUI_LANGUAGE "Bulgarian"
  !insertmacro MUI_LANGUAGE "Hungarian"
  !insertmacro MUI_LANGUAGE "Thai"
  !insertmacro MUI_LANGUAGE "Romanian"
  !insertmacro MUI_LANGUAGE "Latvian"
  !insertmacro MUI_LANGUAGE "Macedonian"
  !insertmacro MUI_LANGUAGE "Estonian"
  !insertmacro MUI_LANGUAGE "Turkish"
  !insertmacro MUI_LANGUAGE "Lithuanian"
  !insertmacro MUI_LANGUAGE "Slovenian"
  !insertmacro MUI_LANGUAGE "Serbian"
  !insertmacro MUI_LANGUAGE "SerbianLatin"
  !insertmacro MUI_LANGUAGE "Arabic"
  !insertmacro MUI_LANGUAGE "Farsi"
  !insertmacro MUI_LANGUAGE "Hebrew"
  !insertmacro MUI_LANGUAGE "Indonesian"
  !insertmacro MUI_LANGUAGE "Mongolian"
  !insertmacro MUI_LANGUAGE "Luxembourgish"
  !insertmacro MUI_LANGUAGE "Albanian"
  !insertmacro MUI_LANGUAGE "Breton"
  !insertmacro MUI_LANGUAGE "Belarusian"
  !insertmacro MUI_LANGUAGE "Icelandic"
  !insertmacro MUI_LANGUAGE "Malay"
  !insertmacro MUI_LANGUAGE "Bosnian"
  !insertmacro MUI_LANGUAGE "Kurdish"
  !insertmacro MUI_LANGUAGE "Irish"
  !insertmacro MUI_LANGUAGE "Uzbek"
  !insertmacro MUI_LANGUAGE "Galician"
  !insertmacro MUI_LANGUAGE "Afrikaans"
  !insertmacro MUI_LANGUAGE "Catalan"
  !insertmacro MUI_LANGUAGE "Esperanto"


;--------------------------------
;Reserve Files
;If you are using solid compression, files that are required before
;the actual installation should be stored first in the data block,
;because this will make your installer start faster.
  !insertmacro MUI_RESERVEFILE_LANGDLL


;--------------------------------
;Installer Sections
  Section "OrderFormSystem (required)" SecOrderFormSystem
  SectionIn RO                               ;表示该选项段不能修改
  SetOutPath "$INSTDIR"                      ;程序输出路径
  File /r "release\*"                        ;要打包安装的程序的路径,这里用相对路径
  WriteUninstaller "$INSTDIR\Uninstall.exe"  ;创建卸载程序
  ;写入已安装程序相关信息到注册表
  WriteRegStr HKCU "Software\OrderFormSystem" "" $INSTDIR	;Store installation folder
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\${PRODUCT_MAINEXE}"
  ;写入已安装程序的卸载程序相关信息到注册表
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\${PRODUCT_MAINEXE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


;--------------------------------
; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts" SecSMSC
  CreateDirectory "$SMPROGRAMS\OrderFormSystem"                                                                          ;创建开始菜单快捷方式目录
  CreateShortCut "$SMPROGRAMS\OrderFormSystem\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0      ;创建卸载开始快捷方式
  CreateShortCut "$SMPROGRAMS\OrderFormSystem\OrderFormSystem.lnk" "$INSTDIR\${PRODUCT_MAINEXE}" "" "$INSTDIR\${PRODUCT_MAINEXE}" 0 ;创建程序开始快捷方式
SectionEnd

;--------------------------------
; Optional section (can be disabled by the user)
Section "Desktop Shortcuts" SecDESKTOP
  CreateDirectory "$DESKTOP\OrderFormSystem"                                                                              ;创建桌面菜单快捷方式目录
  CreateShortCut "$DESKTOP\OrderFormSystem.lnk" "$INSTDIR\${PRODUCT_MAINEXE}" "" "$INSTDIR\${PRODUCT_MAINEXE}" 0            ;创建桌面菜单快捷方式
SectionEnd

;--------------------------------
; Optional section (can be disabled by the user)
Section "Run When Boot" SecRunWhenBoot
  WriteRegStr HKLM "${PRODUCT_REGRUN}" "${PRODUCT_NAME}" "$INSTDIR\${PRODUCT_MAINEXE}"				  ;开机自启动
SectionEnd

;--------------------------------
;Install DotNet Framework Functions
Function InstallDotNetFx 
    SetOutPath "$PLUGINSDIR" 
    File /r "dotnetfx.exe" 
    Banner::show /NOUNLOAD "正在安装.NET运行库，请耐心等待..." 
    nsExec::ExecToStack '"dotnetfx.exe" /q /c:"install.exe /noaspupgrade /q"' 
    Banner::destroy 
FunctionEnd

;--------------------------------
;Installer Functions
Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY                                                                                ;安装程序启动时加载多语言
  ReadRegStr $UninstallFileName ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"              ;读取注册表卸载信息

    
    StrCpy $2 "2.5"
    Push $2
    xtInfoPlugin::GetMDACVersion
    Pop $1
    Push $1
    xtInfoPlugin::CompareVersion
    Pop $0
    ${if} $0 >= 0
        ;MessageBox MB_OK "MDAC version is Newer or Equal to $2 (found: $1)"
    ${else}
        ;MessageBox MB_OK "MDAC version is OLDER than $2 (found: $1)"
    ${endif}

    ; ---------------------------------------------------------------------
    ; Say we wan't our application to only work with .NET Framework v1.1
    xtInfoPlugin::IsDotNetFrameworkInstalled
    Pop $0
    ${if} $0 == true
        xtInfoPlugin::GetDotNetFrameworkId
        ; GetDotNetFrameworkId (id methods) return x.x and not build info
        Pop $0
        ${if} $0 == "1.1"
            StrCpy $0 "Version 1.1 Installed"
        ${else}
            StrCpy $0 "Version 1.0 Installed"
        ${endif}
    ${else}
        StrCpy $0 "Not installed"
    	Pop $1
	;MessageBox MB_OK ".NET Framework version check = $0 (found: $1)"
	MessageBox MB_OK "没有找到 .NET Framework, 请先安装.net framework 2.0 或2.0以上版本的程序以支持软件运行"
	Call InstallDotNetFx
	Quit ; Quit before installer start
    ${endif}
    xtInfoPlugin::GetDotNetFrameworkVersion

FunctionEnd

;--------------------------------
;Repair Functions
Function nsDialogsRepairLeave
  ${NSD_GetState} $RADIO_REPAIR $Checkbox_State_REPAIR     ;获取修复按钮勾选状态
  ${NSD_GetState} $RADIO_REMOVE $Checkbox_State_REMOVE     ;获取卸载按钮勾选状态
  ${If} $Checkbox_State_REMOVE == ${BST_CHECKED}           ;如果卸载勾上了,就执行卸载程序,并跳过安装
    Exec $UninstallFileName
    Quit
  ${EndIf}
FunctionEnd


Function nsDialogsRepair
  ${if} $UninstallFileName == ""                           ;程序还没安装,跳过
    Abort
  ${EndIf}
  !insertmacro MUI_HEADER_TEXT "已经安装" "选择您要执行的操作"

 nsDialogs::Create /NOUNLOAD 1018

  ${NSD_CreateLabel} 10u 0u 300u 30u "软件已经安装，请选择您要执行的操作，并点击『下一步(N)』继续"

 ${NSD_CreateRadioButton}  40u 30u 100u 30u "修复或重新安装"                                                      ;创建修复按钮
 Pop $RADIO_REPAIR
 ${If} $Checkbox_State_REPAIR == ${BST_CHECKED}
  ${NSD_Check} $RADIO_REPAIR
  ${NSD_GetState} $RADIO_REPAIR $Checkbox_State
 ${EndIf}

  ${NSD_CreateRadioButton}  40u 60u 100u 30u "卸载"                                                               ;创建卸载按钮
  Pop $RADIO_REMOVE
 ${If} $Checkbox_State_REMOVE == ${BST_CHECKED}
  ${NSD_Check} $RADIO_REMOVE
  ${NSD_GetState} $RADIO_REMOVE $Checkbox_State
 ${EndIf}

  ${If} $Checkbox_State <> ${BST_CHECKED}                                                                         ;默认勾选上修单选按钮
    ${NSD_Check} $RADIO_REPAIR
  ${EndIf}
 nsDialogs::Show
FunctionEnd


;--------------------------------
;Descriptions
  ;USE A LANGUAGE STRING IF YOU WANT YOUR DESCRIPTIONS TO BE LANGAUGE SPECIFIC
  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecOrderFormSystem} "主程序"              ;组件鼠标移上显示文字,NOTE这里做法不正规,应该应用那些多组件的程序
    !insertmacro MUI_DESCRIPTION_TEXT ${SecSMSC} "开始快捷菜单"               ;用来做安装选项怪怪的
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDESKTOP} "桌面快捷方式"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecRunWhenBoot} "开机自启动"
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

 
;--------------------------------
;Uninstaller Section
Section "Uninstall"
  RMDir /r /rebootok "$INSTDIR"                                               ;删除程序安装目录所有东西
  ; Remove shortcuts, if any
  RMDir /r "$SMPROGRAMS\OrderFormSystem"                                              ;删除开始菜单快捷方式
  delete  "$DESKTOP\OrderFormSystem.lnk"                                              ;删除桌面快捷方式

  ;删除程序注册表信息
  DeleteRegKey /ifempty HKCU "Software\OrderFormSystem"                               
  DeleteRegValue HKLM "${PRODUCT_REGRUN}" "${PRODUCT_NAME}"
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  DeleteRegKey HKLM "${PRODUCT_PUBLISHER_REGKEY}"
  #SetAutoClose true
SectionEnd

;--------------------------------
;Uninstaller Functions
Function un.onInit
  ;探测是否有实例正在运行
  FindProcDLL::FindProc "${PRODUCT_MAINEXE}"
  IntCmp $R0 1 0 notRunning
    MessageBox MB_OK|MB_ICONEXCLAMATION "OrderFormSystem is running. Please close it first" /SD IDOK
    Abort
  notRunning:
  !insertmacro MUI_UNGETLANGUAGE
FunctionEnd

