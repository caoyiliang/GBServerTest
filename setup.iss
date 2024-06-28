; 脚本用 Inno Setup 脚本向导生成。
; 查阅文档获取创建 INNO SETUP 脚本文件详细资料!

[Setup]
AppName=国标协议测试-服务端
AppVerName=国标协议测试-服务端 1.0
AppPublisher=CSoft
AppPublisherURL=宇宙联盟
AppSupportURL=宇宙联盟
AppUpdatesURL=宇宙联盟
DefaultDirName={pf}\国标协议测试-服务端
DefaultGroupName=国标协议测试-服务端
AllowNoIcons=yes
Compression=lzma
SolidCompression=yes

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\caoyi\source\repos\GBServerTest\GBServerTest\bin\Release\net8.0-windows\publish\win-x86\GBServerTest.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\caoyi\source\repos\GBServerTest\GBServerTest\bin\Release\net8.0-windows\publish\win-x86\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
; 注意: 不要在任何共享系统文件中使用“Flags: ignoreversion”

[Icons]
Name: "{group}\国标协议测试-服务端"; Filename: "{app}\GBServerTest.exe"
Name: "{group}\{cm:UninstallProgram,国标协议测试-服务端}"; Filename: "{uninstallexe}"
Name: "{userdesktop}\国标协议测试-服务端"; Filename: "{app}\GBServerTest.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\国标协议测试-服务端"; Filename: "{app}\GBServerTest.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\GBServerTest.exe"; Description: "{cm:LaunchProgram,国标协议测试-服务端}"; Flags: nowait postinstall skipifsilent

