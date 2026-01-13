; -- FHUORs2_RU_v.2.0.iss --
; Установщик русской локализации для Five Hearts Under One Roof season 2 (MelonLoader версия)

[Setup]
AppName=FHUORs2 Russian Localization (MelonLoader)
AppVersion=v.2.0
AppId=FHUOR2-RU-v.2.0
AppPublisher=osata.n
DefaultDirName={code:GetGamePath}
DefaultGroupName=FHUORs2 Russian Localization
UninstallDisplayIcon={app}\FHUOR2.exe
SetupIconFile=icon.ico
Compression=lzma2
SolidCompression=yes
OutputDir=.\Output
OutputBaseFilename=FHUORs2_RU_v.2.0
DisableDirPage=no
DisableProgramGroupPage=yes
PrivilegesRequired=admin
VersionInfoVersion=2.0
VersionInfoProductName=FHUORs2 Russian Localization (MelonLoader)
VersionInfoProductVersion=2.0
VersionInfoCompany=FHUORs2 Localization Team
VersionInfoDescription=Russian Localization for FHUORs2
VersionInfoCopyright=© 2026 FHUORs2 Localization Team
WizardStyle=modern

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Messages]
WelcomeLabel2=Установка русской локализации для Five Hearts Under One Roof season 2 (v.2.0) с MelonLoader
ClickDirsLabel3=Если игра найдена в Steam, путь подставится автоматически.

[Types]
Name: "full"; Description: "Полная установка"; Flags: iscustom

[Components]
Name: translation; Description: "Русская локализация (v.2.0)"; Types: full; Flags: fixed
Name: melonloader; Description: "Файлы MelonLoader (если не установлены)"; Types: full

[Files]
Source: "icon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "Files\version.dll"; DestDir: "{app}"; Components: melonloader; Flags: ignoreversion onlyifdoesntexist
Source: "Files\MelonLoader\*"; DestDir: "{app}\MelonLoader"; Components: melonloader; Flags: ignoreversion onlyifdoesntexist recursesubdirs createallsubdirs
Source: "Files\UserData\*"; DestDir: "{app}\UserData"; Components: melonloader; Flags: ignoreversion onlyifdoesntexist recursesubdirs createallsubdirs
Source: "Files\Mods\TranslationMod.dll"; DestDir: "{app}\Mods"; Components: translation; Flags: ignoreversion
Source: "Files\Mods\translations\*"; DestDir: "{app}\Mods\translations"; Components: translation; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\FHUORs2-RU (v.2.0)"; Filename: "{app}\FHUOR2.exe"; IconFilename: "{app}\icon.ico"; Comment: "Five Hearts Under One Roof season 2 with Russian Localisation"
Name: "{group}\Удалить русскую локализацию"; Filename: "{uninstallexe}"
Name: "{commondesktop}\FHUORs2 (v.2.0)"; Filename: "{app}\FHUOR2.exe"; IconFilename: "{app}\icon.ico"; Comment: "Запустить с русской локализацией"

[Run]
Filename: "{app}\FHUOR2.exe"; Description: "Запустить FHUORs2 с локализацией"; Flags: postinstall nowait skipifsilent unchecked

[UninstallDelete]
Type: files; Name: "{app}\Mods\TranslationMod.dll"
Type: files; Name: "{app}\Mods\FHUOR2_Translation.dll"
Type: files; Name: "{app}\Mods\*.dll"
Type: filesandordirs; Name: "{app}\Mods\translations"
Type: filesandordirs; Name: "{app}\Mods\FHUOR2_Translation"
Type: files; Name: "{app}\version.dll"
Type: filesandordirs; Name: "{app}\MelonLoader"
Type: filesandordirs; Name: "{app}\UserData"
Type: filesandordirs; Name: "{group}"
Type: files; Name: "{commondesktop}\FHUORs2 (v.2.0).lnk"
Type: files; Name: "{app}\icon.ico"

[Code]
var
  GameFound: Boolean;
  CustomPage: TWizardPage;
  DirEdit: TNewEdit;
  DirBrowseButton: TNewButton;
  GameExe: String;
  MelonInstalled: Boolean;
  ChangelogPage: TWizardPage;
  ChangelogMemo: TMemo;
  OurMelonFilesExist: Boolean;

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
  ChangelogPage := CreateCustomPage(wpWelcome, 'Изменения в версии 2.0', 
    'Ознакомьтесь с изменениями перед установкой');
  
  ChangelogMemo := TMemo.Create(WizardForm);
  ChangelogMemo.Parent := ChangelogPage.Surface;
  ChangelogMemo.Left := 0;
  ChangelogMemo.Top := 0;
  ChangelogMemo.Width := ChangelogPage.SurfaceWidth;
  ChangelogMemo.Height := ChangelogPage.SurfaceHeight;
  ChangelogMemo.ScrollBars := ssVertical;
  ChangelogMemo.ReadOnly := True;
  ChangelogMemo.WordWrap := True;
  ChangelogMemo.Text := 
    'Глобальные изменения:' + #13#10 +
    '• Полностью переработан мод под фреймворк MelonLoader' + #13#10 +
    '• Поддержка официального патча игры на Il2Cpp' + #13#10#13#10 +
    'Примечание: Эта версия полностью переработана и требует наличия MelonLoader. ' +
    'Если у вас установлена старая версия локализации (v.1.0), рекомендуется удалить её перед установкой данной версии.'+ #13#10#13#10 +
    'Исправленные переводы:' + #13#10 +
    '• Rocker Pose=Жест рокета - исправлено на "Жест рокера";' + #13#10 +
    '• Still, I''m glad everything wrapped up well!=Но я все равно рада, что все удачно завершилось! - исправлено на "Но я все равно рад, что все удачно завершилось!"' + #13#10 +
    '• Ah, sorry... I must''ve mistaken you for someone else.=А, прости... Должно быть, я обзнался. - исправлено на "А, прости... Должно быть, я обозонался."' + #13#10 +
    '• Maybe because you had him back at work right after giving birth…=Может, потому что nы заставилf его вернуться к работе сразу после выписки… - исправлено на "Может, потому что ты заставила его вернуться к работе сразу после выписки…"' + #13#10 +
    '• Eun-bi, high school starts earlier now, remember~=Ын-би, в старшей школе занятия раньше, я же говорила~ - исправлено на "Ын-би, в старшей школе занятия раньше, я же говорил~"' + #13#10 +
    '• В сцене "В тот год, мы..." в главе 5 добавлен контекстный перевод для "Right?" - "Я права?"';

  if not GameFound then
  begin
    CustomPage := CreateCustomPage(ChangelogPage.ID, 'Игра не найдена', 'Укажите путь к папке с игрой Five Hearts Under One Roof season 2');
    
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
    MsgBox('Русская локализация FHUORs2 (v.2.0) успешно установлена!' + #13#10#13#10 +
           'Игра теперь переведена на русский язык.' + #13#10 +
           'MelonLoader и все необходимые файлы установлены.', 
           mbInformation, MB_OK);
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  FindRec: TFindRec;
  ModsPath: String;
begin
  if CurUninstallStep = usPostUninstall then
  begin
    ModsPath := ExpandConstant('{app}\Mods');
    
    DeleteFile(ExpandConstant('{app}\Mods\TranslationMod.dll'));
    DeleteFile(ExpandConstant('{app}\Mods\FHUOR2_Translation.dll'));
    
    if DirExists(ExpandConstant('{app}\Mods\translations')) then
    begin
      DelTree(ExpandConstant('{app}\Mods\translations'), True, True, True);
    end;
    
    if DirExists(ExpandConstant('{app}\Mods\FHUOR2_Translation')) then
    begin
      DelTree(ExpandConstant('{app}\Mods\FHUOR2_Translation'), True, True, True);
    end;
    
    OurMelonFilesExist := DirExists(ExpandConstant('{app}\MelonLoader'));
    if OurMelonFilesExist then
    begin
      DelTree(ExpandConstant('{app}\MelonLoader'), True, True, True);
      DeleteFile(ExpandConstant('{app}\version.dll'));
      
      if DirExists(ExpandConstant('{app}\UserData')) then
      begin
        DelTree(ExpandConstant('{app}\UserData'), True, True, True);
      end;
    end;
    
    DeleteFile(ExpandConstant('{app}\icon.ico'));
    
    if DirExists(ModsPath) then
    begin
      if FindFirst(ModsPath + '\*', FindRec) then
      begin
        try
          repeat
            if (FindRec.Name <> '.') and (FindRec.Name <> '..') then
            begin
              Exit;
            end;
          until not FindNext(FindRec);
          
          RemoveDir(ModsPath);
        finally
          FindClose(FindRec);
        end;
      end;
    end;
    
    MsgBox('Русская локализация полностью удалена.' + #13#10 +
           'Все файлы мода были удалены из системы.', 
           mbInformation, MB_OK);
  end;
end;