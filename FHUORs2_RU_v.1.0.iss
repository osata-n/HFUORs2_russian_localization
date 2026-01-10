; -- FHUORs2_RU_v.1.0.iss --
; Установщик русской локализации для Five Hearts Under One Roof season 2

[Setup]
AppName=FHUORs2 Russian Localisation
AppVersion=v.1.0
AppId=FHUOR2-RU-v.1.0
AppPublisher=osata.n
DefaultDirName={code:GetGamePath}
DefaultGroupName=FHUORs2 Russian Localisation
UninstallDisplayIcon={app}\FHUOR2.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.\Output
OutputBaseFilename=FHUORs2_RU_v.1.0
// SetupIconFile=E:\SteamLibrary\steamapps\common\Five Hearts Under One Roof season2\FHUOR2.exe
DisableDirPage=no
DisableProgramGroupPage=yes
PrivilegesRequired=admin
VersionInfoVersion=1.0
VersionInfoProductName=FHUORs2 Russian Localisation
VersionInfoProductVersion=1.0
VersionInfoCompany=FHUORs2 Localisation Team
WizardStyle=modern

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Messages]
WelcomeLabel2=Установка русской локализации для Five Hearts Under One Roof season 2 (v.1.0)
ClickDirsLabel3=Если игра найдена в Steam, путь подставится автоматически.

[Types]
Name: "full"; Description: "Полная установка"; Flags: iscustom

[Components]
Name: translation; Description: "Русская локализация (v.1.0)"; Types: full; Flags: fixed
Name: bepinex; Description: "Файлы BepInEx (если не установлены)"; Types: full

[Files]
Source: "Files\BepInEx\cache\*"; DestDir: "{app}\BepInEx\cache"; Components: bepinex; Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist
Source: "Files\BepInEx\config\*"; DestDir: "{app}\BepInEx\config"; Components: bepinex; Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist
Source: "Files\BepInEx\core\*"; DestDir: "{app}\BepInEx\core"; Components: bepinex; Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist
Source: "Files\BepInEx\plugins\*"; DestDir: "{app}\BepInEx\plugins"; Components: bepinex; Flags: ignoreversion recursesubdirs createallsubdirs onlyifdoesntexist

Source: "Files\BepInEx\plugins\TranslationMod.dll"; DestDir: "{app}\BepInEx\plugins"; Components: translation; Flags: ignoreversion
Source: "Files\BepInEx\plugins\translations\*"; DestDir: "{app}\BepInEx\plugins\translations"; Components: translation; Flags: ignoreversion recursesubdirs createallsubdirs

Source: "Files\winhttp.dll"; DestDir: "{app}"; Components: bepinex; Flags: ignoreversion onlyifdoesntexist
Source: "Files\doorstop_config.ini"; DestDir: "{app}"; Components: bepinex; Flags: ignoreversion onlyifdoesntexist

[Icons]
Name: "{group}\FHUORs2 (v.1.0)"; Filename: "{app}\FHUOR2.exe"; Comment: "Five Hearts Under One Roof season 2 with Russian Localisation"
Name: "{group}\Удалить русскую локализацию"; Filename: "{uninstallexe}"
Name: "{commondesktop}\FHUORs2 (v.1.0)"; Filename: "{app}\FHUOR2.exe"; Comment: "Запустить с русской локализацией"

[Run]
Filename: "{app}\FHUOR2.exe"; Description: "Запустить FHUORs2 с локализацией"; Flags: postinstall nowait skipifsilent unchecked

[UninstallDelete]
Type: filesandordirs; Name: "{app}\BepInEx\translations"
Type: files; Name: "{app}\BepInEx\plugins\TranslationMod.dll"

[Code]
var
  GameFound: Boolean;
  CustomPage: TWizardPage;
  DirEdit: TNewEdit;
  DirBrowseButton: TNewButton;
  GameExe: String;

function GetGamePath(Param: String): String;
var
  SteamPath: String;
  LibraryPaths: TArrayOfString;
  I: Integer;
  LibraryFile: String;
  GamePath: String;
begin
  Result := '';
  GameFound := False;
  
  if RegQueryStringValue(HKLM, 'SOFTWARE\Wow6432Node\Valve\Steam', 'InstallPath', SteamPath) or
     RegQueryStringValue(HKCU, 'SOFTWARE\Valve\Steam', 'SteamPath', SteamPath) then
  begin
    LibraryFile := SteamPath + '\steamapps\libraryfolders.vdf';
    if FileExists(LibraryFile) then
    begin
      SetArrayLength(LibraryPaths, 1);
      LibraryPaths[0] := SteamPath;
      
      for I := 0 to GetArrayLength(LibraryPaths)-1 do
      begin
        if LibraryPaths[I] <> '' then
        begin
          GamePath := LibraryPaths[I] + '\steamapps\common\Five Hearts Under One Roof season2';
          if FileExists(GamePath + '\FHUOR2.exe') then
          begin
            Result := GamePath;
            GameFound := True;
            Exit;
          end;
        end;
      end;
    end;
    
    GamePath := SteamPath + '\steamapps\common\Five Hearts Under One Roof season2';
    if FileExists(GamePath + '\FHUOR2.exe') then
    begin
      Result := GamePath;
      GameFound := True;
      Exit;
    end;
  end;
  
  if FileExists('E:\SteamLibrary\steamapps\common\Five Hearts Under One Roof season2\FHUOR2.exe') then
  begin
    Result := 'E:\SteamLibrary\steamapps\common\Five Hearts Under One Roof season2';
    GameFound := True;
  end
  else if FileExists('D:\SteamLibrary\steamapps\common\Five Hearts Under One Roof season2\FHUOR2.exe') then
  begin
    Result := 'D:\SteamLibrary\steamapps\common\Five Hearts Under One Roof season2';
    GameFound := True;
  end
  else if FileExists('C:\Program Files (x86)\Steam\steamapps\common\Five Hearts Under One Roof season2\FHUOR2.exe') then
  begin
    Result := 'C:\Program Files (x86)\Steam\steamapps\common\Five Hearts Under One Roof season2';
    GameFound := True;
  end
  else
  begin
    Result := 'C:\Program Files (x86)\Steam\steamapps\common\Five Hearts Under One Roof season2';
    GameFound := False;
  end;
end;

function InitializeSetup(): Boolean;
begin
  if not IsAdminLoggedOn() then
  begin
    MsgBox('Для установки локализации требуются права администратора.' + #13#10 +
           'Пожалуйста, запустите установщик от имени администратора.', mbError, MB_OK);
    Result := False;
    Exit;
  end;
  
  GameExe := GetGamePath('') + '\FHUOR2.exe';
  
  if not FileExists(GameExe) then
  begin
    if MsgBox('Игра FHUOR2.exe не найдена по пути:' + #13#10 + GameExe + #13#10#13#10 +
              'Пожалуйста, установите игру через Steam или' + #13#10 +
              'выберите правильную директорию вручную.' + #13#10#13#10 +
              'Продолжить установку?', mbError, MB_YESNO) = IDNO then
    begin
      Result := False;
    end
    else
      Result := True;
  end
  else
    Result := True;
end;

procedure BrowseButtonClick(Sender: TObject);
var
  Dir: String;
begin
  Dir := DirEdit.Text;
  if BrowseForFolder('Выберите папку с игрой', Dir, False) then
  begin
    DirEdit.Text := Dir;
  end;
end;

procedure InitializeWizard();
begin
  if not GameFound then
  begin
    CustomPage := CreateCustomPage(wpWelcome, 'Игра не найдена', 'Укажите путь к папке с игрой Five Hearts Under One Roof season 2');
    
    DirEdit := TNewEdit.Create(WizardForm);
    DirEdit.Parent := CustomPage.Surface;
    DirEdit.Left := 0;
    DirEdit.Top := 0;
    DirEdit.Width := CustomPage.SurfaceWidth - 75;
    DirEdit.Height := ScaleY(23);
    DirEdit.Text := GetGamePath('');
    
    DirBrowseButton := TNewButton.Create(WizardForm);
    DirBrowseButton.Parent := CustomPage.Surface;
    DirBrowseButton.Left := CustomPage.SurfaceWidth - 70;
    DirBrowseButton.Top := 0;
    DirBrowseButton.Width := ScaleX(70);
    DirBrowseButton.Height := ScaleY(23);
    DirBrowseButton.Caption := 'Обзор...';
    DirBrowseButton.OnClick := @BrowseButtonClick;
    
    with TNewStaticText.Create(WizardForm) do
    begin
      Parent := CustomPage.Surface;
      Left := 0;
      Top := ScaleY(30);
      Width := CustomPage.SurfaceWidth;
      Height := ScaleY(50);
      WordWrap := True;
      Caption := 'Игра не найдена в автоматическом режиме.' + #13#10 +
                 'Пожалуйста, укажите путь к папке с игрой, где находится файл FHUOR2.exe';
    end;
  end;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  GameExeLocal: String;
begin
  Result := True;
  
  if (not GameFound) and (CustomPage <> nil) and (CurPageID = CustomPage.ID) then
  begin
    GameExeLocal := DirEdit.Text + '\FHUOR2.exe';
    
    if not FileExists(GameExeLocal) then
    begin
      MsgBox('Файл FHUOR2.exe не найден по указанному пути:' + #13#10 + GameExeLocal, mbError, MB_OK);
      Result := False;
    end
    else
    begin
      WizardForm.DirEdit.Text := RemoveBackslashUnlessRoot(DirEdit.Text);
    end;
  end;
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  if (PageID = wpSelectDir) and GameFound then
    Result := True
  else
    Result := False;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    MsgBox('Русская локализация FHUORs2 (v.1.0) успешно установлена!' + #13#10#13#10 +
           'Игра теперь переведена на русский язык.',
           mbInformation, MB_OK);
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
  begin
    MsgBox('Русская локализация удалена.' + #13#10 +
           'Для полного удаления BepInEx удалите папку BepInEx вручную.', 
           mbInformation, MB_OK);
  end;
end;