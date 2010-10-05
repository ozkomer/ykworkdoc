;NSIS Modern User Interface
;Multilingual Example Script
;Written by Joost Verburg, modified by yeer 2010-07-23


;--------------------------------
;Include Setting;����C���Ե�ͷ�ļ�
  !include "MUI2.nsh"
  !include "Logiclib.nsh"
  !include  "nsDialogs.nsh"

	
;--------------------------------
;General
  Name "OrderFormSystem"	
  OutFile "Setup.exe"
  
  InstallDir "$PROGRAMFILES\OrderFormSystem"		;ȱʡ��װĿ¼
  InstallDirRegKey HKCU "Software\OrderFormSystem" ""	;д��ע���ֵ
  RequestExecutionLevel highest  		;win7֧��


;--------------------------------
;Global Defining
  ;��Ʒ����
  !define PRODUCT_NAME "OrderFormSystem"
  ;��ִ���ļ���
  !define PRODUCT_MAINEXE "OrderFormSystem.exe"
  ;�汾��
  !define PRODUCT_VERSION "1.0"
  ;������
  !define PRODUCT_PUBLISHER "yeer"
  ;��ַ
  !define PRODUCT_WEB_SITE "www.xiami.com"
  ;��������Ŀ¼
  !define PRODUCT_REGRUN "Software\Microsoft\Windows\CurrentVersion\Run"
  ;��Ʒ��������ע����·��
  !define PRODUCT_PUBLISHER_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_PUBLISHER}"
  ;��Ʒ��ע���İ�װ·��
  !define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}"
  ;��Ʒ��ע���ж��·��
  !define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  ;ж�ظ�Ŀ¼
  !define PRODUCT_UNINST_ROOT_KEY "HKLM"


;--------------------------------
;Interface Settings
  !define MUI_ICON "image\lavrock.ico"                       ;��װ����ͼ��
  !define MUI_UNICON "image\uninstall.ico"                   ;ж�س���ͼ��
  !define MUI_HEADERIMAGE                                    ;ʹ���Ϸ����ͼƬ
  !define MUI_HEADERIMAGE_BITMAP "image\header.bmp"          ;��װ��ʼ�ͽ���ʱ���Ϸ����ͼƬ·��
  !define MUI_HEADERIMAGE_UNBITMAP "image\header-uninstall.bmp"        ;ж�ؿ�ʼ�ͽ���ʱ���Ϸ����ͼƬ·��
  !define MUI_WELCOMEFINISHPAGE_BITMAP "image\orange.bmp"     ;��װ��ʼ�ͽ���ʱ�����������ͼƬ·��
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP "image\orange-uninstall.bmp"   ;ж�ؿ�ʼ�ͽ���ʱ�����������ͼƬ·��
  !define MUI_FINISHPAGE_RUN "$INSTDIR\${PRODUCT_MAINEXE}"   ;��װ�ɹ����Ƿ��������г���,����ѡ��,Ĭ�Ϲ���
  !define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\License.txt"   ;��װ�ɹ����Ƿ�������ļ�,����ѡ��
  !define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED               ;�������ļ�Ĭ�ϲ�����
  !define MUI_ABORTWARNING                                   ;���Ծ���
  

;------------------------------
;repair page settings
  Var UninstallFileName        ;ж�س���
  Var RADIO_REPAIR             ;�޸���ѡ��ť
  Var RADIO_REMOVE             ;��ȥж�ص�ѡ��ť
  Var Checkbox_State_REPAIR    ;�޸���ѡ��ťѡ��״̬
  Var Checkbox_State_REMOVE    ;��ȥж�ص�ѡ��ťѡ��״̬
  Var Checkbox_State           ;��ťѡ��״̬


;--------------------------------
;Language Selection Dialog Settings
;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU"                       ;������ע����Ŀ¼
  !define MUI_LANGDLL_REGISTRY_KEY "Software\OrderFormSystem"            ;������ע�����·����ֵ
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"    ;��ǰ��������


;--------------------------------
;Pages
  !insertmacro MUI_PAGE_WELCOME                                  ;��װ����ӭ����
  Page custom nsDialogsRepair nsDialogsRepairLeave               ;�Դ�̽���Ƿ��Ѿ���װ�˱������ҳ��
  !insertmacro MUI_PAGE_LICENSE "release\License.txt"            ;����Э���ļ�·��
  !insertmacro MUI_PAGE_DIRECTORY                                ;·��ѡ��
  !insertmacro MUI_PAGE_COMPONENTS                               ;���ѡ��
  !insertmacro MUI_PAGE_INSTFILES                                ;��ʼ��װ
  !insertmacro MUI_PAGE_FINISH                                   ;��װ�ɹ�
  
  !insertmacro MUI_UNPAGE_WELCOME                                ;ж�ػ�ӭ����
  !insertmacro MUI_UNPAGE_CONFIRM                                ;ж��ȷ�Ͻ���
  !insertmacro MUI_UNPAGE_INSTFILES                              ;ж�س���
  !insertmacro MUI_UNPAGE_FINISH                                 ;ж�����


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
  SectionIn RO                               ;��ʾ��ѡ��β����޸�
  SetOutPath "$INSTDIR"                      ;�������·��
  File /r "release\*"                        ;Ҫ�����װ�ĳ����·��,���������·��
  WriteUninstaller "$INSTDIR\Uninstall.exe"  ;����ж�س���
  ;д���Ѱ�װ���������Ϣ��ע���
  WriteRegStr HKCU "Software\OrderFormSystem" "" $INSTDIR	;Store installation folder
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\${PRODUCT_MAINEXE}"
  ;д���Ѱ�װ�����ж�س��������Ϣ��ע���
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
  CreateDirectory "$SMPROGRAMS\OrderFormSystem"                                                                          ;������ʼ�˵���ݷ�ʽĿ¼
  CreateShortCut "$SMPROGRAMS\OrderFormSystem\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0      ;����ж�ؿ�ʼ��ݷ�ʽ
  CreateShortCut "$SMPROGRAMS\OrderFormSystem\OrderFormSystem.lnk" "$INSTDIR\${PRODUCT_MAINEXE}" "" "$INSTDIR\${PRODUCT_MAINEXE}" 0 ;��������ʼ��ݷ�ʽ
SectionEnd

;--------------------------------
; Optional section (can be disabled by the user)
Section "Desktop Shortcuts" SecDESKTOP
  CreateDirectory "$DESKTOP\OrderFormSystem"                                                                              ;��������˵���ݷ�ʽĿ¼
  CreateShortCut "$DESKTOP\OrderFormSystem.lnk" "$INSTDIR\${PRODUCT_MAINEXE}" "" "$INSTDIR\${PRODUCT_MAINEXE}" 0            ;��������˵���ݷ�ʽ
SectionEnd

;--------------------------------
; Optional section (can be disabled by the user)
Section "Run When Boot" SecRunWhenBoot
  WriteRegStr HKLM "${PRODUCT_REGRUN}" "${PRODUCT_NAME}" "$INSTDIR\${PRODUCT_MAINEXE}"				  ;����������
SectionEnd

;--------------------------------
;Install DotNet Framework Functions
Function InstallDotNetFx 
    SetOutPath "$PLUGINSDIR" 
    File /r "dotnetfx.exe" 
    Banner::show /NOUNLOAD "���ڰ�װ.NET���п⣬�����ĵȴ�..." 
    nsExec::ExecToStack '"dotnetfx.exe" /q /c:"install.exe /noaspupgrade /q"' 
    Banner::destroy 
FunctionEnd

;--------------------------------
;Installer Functions
Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY                                                                                ;��װ��������ʱ���ض�����
  ReadRegStr $UninstallFileName ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"              ;��ȡע���ж����Ϣ

    
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
	MessageBox MB_OK "û���ҵ� .NET Framework, ���Ȱ�װ.net framework 2.0 ��2.0���ϰ汾�ĳ�����֧���������"
	Call InstallDotNetFx
	Quit ; Quit before installer start
    ${endif}
    xtInfoPlugin::GetDotNetFrameworkVersion

FunctionEnd

;--------------------------------
;Repair Functions
Function nsDialogsRepairLeave
  ${NSD_GetState} $RADIO_REPAIR $Checkbox_State_REPAIR     ;��ȡ�޸���ť��ѡ״̬
  ${NSD_GetState} $RADIO_REMOVE $Checkbox_State_REMOVE     ;��ȡж�ذ�ť��ѡ״̬
  ${If} $Checkbox_State_REMOVE == ${BST_CHECKED}           ;���ж�ع�����,��ִ��ж�س���,��������װ
    Exec $UninstallFileName
    Quit
  ${EndIf}
FunctionEnd


Function nsDialogsRepair
  ${if} $UninstallFileName == ""                           ;����û��װ,����
    Abort
  ${EndIf}
  !insertmacro MUI_HEADER_TEXT "�Ѿ���װ" "ѡ����Ҫִ�еĲ���"

 nsDialogs::Create /NOUNLOAD 1018

  ${NSD_CreateLabel} 10u 0u 300u 30u "����Ѿ���װ����ѡ����Ҫִ�еĲ��������������һ��(N)������"

 ${NSD_CreateRadioButton}  40u 30u 100u 30u "�޸������°�װ"                                                      ;�����޸���ť
 Pop $RADIO_REPAIR
 ${If} $Checkbox_State_REPAIR == ${BST_CHECKED}
  ${NSD_Check} $RADIO_REPAIR
  ${NSD_GetState} $RADIO_REPAIR $Checkbox_State
 ${EndIf}

  ${NSD_CreateRadioButton}  40u 60u 100u 30u "ж��"                                                               ;����ж�ذ�ť
  Pop $RADIO_REMOVE
 ${If} $Checkbox_State_REMOVE == ${BST_CHECKED}
  ${NSD_Check} $RADIO_REMOVE
  ${NSD_GetState} $RADIO_REMOVE $Checkbox_State
 ${EndIf}

  ${If} $Checkbox_State <> ${BST_CHECKED}                                                                         ;Ĭ�Ϲ�ѡ���޵�ѡ��ť
    ${NSD_Check} $RADIO_REPAIR
  ${EndIf}
 nsDialogs::Show
FunctionEnd


;--------------------------------
;Descriptions
  ;USE A LANGUAGE STRING IF YOU WANT YOUR DESCRIPTIONS TO BE LANGAUGE SPECIFIC
  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecOrderFormSystem} "������"              ;������������ʾ����,NOTE��������������,Ӧ��Ӧ����Щ������ĳ���
    !insertmacro MUI_DESCRIPTION_TEXT ${SecSMSC} "��ʼ��ݲ˵�"               ;��������װѡ��ֵֹ�
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDESKTOP} "�����ݷ�ʽ"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecRunWhenBoot} "����������"
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

 
;--------------------------------
;Uninstaller Section
Section "Uninstall"
  RMDir /r /rebootok "$INSTDIR"                                               ;ɾ������װĿ¼���ж���
  ; Remove shortcuts, if any
  RMDir /r "$SMPROGRAMS\OrderFormSystem"                                              ;ɾ����ʼ�˵���ݷ�ʽ
  delete  "$DESKTOP\OrderFormSystem.lnk"                                              ;ɾ�������ݷ�ʽ

  ;ɾ������ע�����Ϣ
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
  ;̽���Ƿ���ʵ����������
  FindProcDLL::FindProc "${PRODUCT_MAINEXE}"
  IntCmp $R0 1 0 notRunning
    MessageBox MB_OK|MB_ICONEXCLAMATION "OrderFormSystem is running. Please close it first" /SD IDOK
    Abort
  notRunning:
  !insertmacro MUI_UNGETLANGUAGE
FunctionEnd

