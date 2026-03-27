; -- FHUORs2_RU_v.2.0.2 iss --
; Установщик русской локализации для Five Hearts Under One Roof season 2 (MelonLoader версия)

[Setup]
AppName=FHUORs2 Русская Локализация 2.0.2
AppVersion=v.2.0.2
AppId=FHUOR2_RU_osata-n
AppVerName=FHUORs2 Русская Локализация 2.0.2
AppPublisher=osata.n
AppPublisherURL=https://github.com/osata-n
AppSupportURL=https://github.com/osata-n
AppUpdatesURL=https://github.com/osata-n
DefaultDirName={code:GetGamePath}
DefaultGroupName=FHUORs2 Русская Локализация
UninstallDisplayIcon={app}\FHUOR2.exe
SetupIconFile=icon.ico
Compression=lzma2
SolidCompression=yes
OutputDir=.\Output
OutputBaseFilename=FHUORs2_RU_v.2.0.2
DisableDirPage=no
DisableProgramGroupPage=yes
PrivilegesRequired=admin
VersionInfoVersion=2.0.2
VersionInfoProductName=FHUORs2 Russian Localization (MelonLoader)
VersionInfoProductVersion=2.0.2
VersionInfoCompany=FHUORs2 Localization Team
VersionInfoDescription=Russian Localization for FHUORs2
VersionInfoCopyright=© 2026 FHUORs2 Localization Team
UninstallDisplayName=FHUORs2 Русская Локализация 2.0.2
WizardStyle=modern

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Messages]
WelcomeLabel2=Установка русской локализации для Five Hearts Under One Roof season 2 (v.2.0.2) с MelonLoader
ClickDirsLabel3=Если игра найдена в Steam, путь подставится автоматически.

[Types]
Name: "full"; Description: "Полная установка"; Flags: iscustom

[Components]
Name: translation; Description: "Русская локализация (v.2.0.2)"; Types: full; Flags: fixed
Name: melonloader; Description: "Файлы MelonLoader (если не установлены)"; Types: full

[Files]
Source: "icon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "Files\version.dll"; DestDir: "{app}"; Components: melonloader; Flags: ignoreversion onlyifdoesntexist
Source: "Files\MelonLoader\*"; DestDir: "{app}\MelonLoader"; Components: melonloader; Flags: ignoreversion onlyifdoesntexist recursesubdirs createallsubdirs
Source: "Files\UserData\*"; DestDir: "{app}\UserData"; Components: melonloader; Flags: ignoreversion onlyifdoesntexist recursesubdirs createallsubdirs
Source: "Files\Mods\TranslationMod.dll"; DestDir: "{app}\Mods"; Components: translation; Flags: ignoreversion
Source: "Files\Mods\translations\*"; DestDir: "{app}\Mods\translations"; Components: translation; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\FHUORs2-RU (v.2.0.2)"; Filename: "{app}\FHUOR2.exe"; IconFilename: "{app}\icon.ico"; Comment: "Five Hearts Under One Roof season 2 with Russian Localisation"
Name: "{group}\Удалить русскую локализацию"; Filename: "{uninstallexe}"
Name: "{commondesktop}\FHUORs2 (v.2.0.2)"; Filename: "{app}\FHUOR2.exe"; IconFilename: "{app}\icon.ico"; Comment: "Запустить с русской локализацией"

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
Type: files; Name: "{commondesktop}\FHUORs2 (v.2.0.2).lnk"
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

procedure UninstallOldVersions(InstallPath: string);
var
  ModsPath: string;
  OldFiles: array of string;
  I: Integer;
begin
  ModsPath := InstallPath + '\Mods';
  
  Log('=== НАЧАЛО УДАЛЕНИЯ СТАРОЙ ВЕРСИЙ ===');
  Log('Путь установки: ' + InstallPath);
  
  SetArrayLength(OldFiles, 5);
  OldFiles[0] := 'FHUORs2_RU_v.2.0.dll';
  OldFiles[1] := 'FHUORs2_RU_v.2.0.1.dll';
  OldFiles[2] := 'FHUOR2_Translation.dll';
  OldFiles[3] := 'FHUORs2_RU_v.2.0';
  OldFiles[4] := 'FHUORs2_RU_v.2.0.1';
  
  for I := 0 to GetArrayLength(OldFiles) - 1 do
  begin
    if FileExists(ModsPath + '\' + OldFiles[I]) then
    begin
      if DeleteFile(ModsPath + '\' + OldFiles[I]) then
        Log('Удален старый файл: ' + OldFiles[I])
      else
        Log('НЕ УДАЛОСЬ удалить: ' + OldFiles[I]);
    end;
    
    if DirExists(ModsPath + '\' + OldFiles[I]) then
    begin
      if DelTree(ModsPath + '\' + OldFiles[I], True, True, True) then
        Log('Удалена старая папка: ' + OldFiles[I])
      else
        Log('НЕ УДАЛОСЬ удалить папку: ' + OldFiles[I]);
    end;
  end;
  
  if FileExists(ModsPath + '\TranslationMod.dll') then
  begin
    if DeleteFile(ModsPath + '\TranslationMod.dll') then
      Log('Удален TranslationMod.dll (текущая версия)')
    else
      Log('НЕ УДАЛОСЬ удалить TranslationMod.dll');
  end;
  
  if DirExists(ModsPath + '\translations') then
  begin
    if DelTree(ModsPath + '\translations', True, True, True) then
      Log('Удалена папка translations')
    else
      Log('НЕ УДАЛОСЬ удалить папку translations');
  end;
  
  if DirExists(ModsPath) then
  begin
    if RemoveDir(ModsPath) then
      Log('Удалена папка Mods (была пуста)')
    else
      Log('Папка Mods не пуста или не удалена');
  end;
  
  if DirExists(InstallPath + '\MelonLoader') then
  begin
    if DelTree(InstallPath + '\MelonLoader', True, True, True) then
      Log('Удалена папка MelonLoader')
    else
      Log('НЕ УДАЛОСЬ удалить папку MelonLoader');
  end;
  
  if FileExists(InstallPath + '\version.dll') then
  begin
    if DeleteFile(InstallPath + '\version.dll') then
      Log('Удален файл version.dll')
    else
      Log('НЕ УДАЛОСЬ удалить version.dll');
  end;
  
  if DirExists(InstallPath + '\UserData') then
  begin
    if DelTree(InstallPath + '\UserData', True, True, True) then
      Log('Удалена папка UserData')
    else
      Log('НЕ УДАЛОСЬ удалить папку UserData');
  end;
  
  if FileExists(InstallPath + '\icon.ico') then
  begin
    if DeleteFile(InstallPath + '\icon.ico') then
      Log('Удален файл icon.ico')
    else
      Log('НЕ УДАЛОСЬ удалить icon.ico');
  end;
  
  Log('=== ЗАВЕРШЕНИЕ УДАЛЕНИЯ СТАРОЙ ВЕРСИЙ ===');
end;

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
var
  DeletePrevious: Boolean;
  UserResponse: Integer;
  GamePath: String;
begin
  if not IsAdminLoggedOn() then
  begin
    MsgBox('Для установки локализации требуются права администратора.' + #13#10 +
           'Пожалуйста, запустите установщик от имени администратора.', mbError, MB_OK);
    Result := False;
    Exit;
  end;
  
  GamePath := GetGamePath('');
  GameExe := GamePath + '\FHUOR2.exe';
  
  if not FileExists(GameExe) then
  begin
    if MsgBox('Игра FHUOR2.exe не найдена по пути:' + #13#10 + GameExe + #13#10#13#10 +
              'Пожалуйста, установите игру через Steam или' + #13#10 +
              'выберите правильную директорию вручную.' + #13#10#13#10 +
              'Продолжить установку?', mbError, MB_YESNO) = IDNO then
    begin
      Result := False;
      Exit;
    end;
  end;
  
  if FileExists(GamePath + '\Mods\TranslationMod.dll') then
  begin
    UserResponse := MsgBox('Обнаружена установленная версия локализации' + #13#10#13#10 +
                          'Выберите действие:' + #13#10#13#10 +
                          'Да - переустановить (удалить и установить заново)' + #13#10 +
                          'Нет - установить поверх (не рекомендуется)' + #13#10 +
                          'Отмена - отменить установку', 
                          mbConfirmation, MB_YESNOCANCEL);
    
    if UserResponse = IDCANCEL then
    begin
      Result := False;
      Exit;
    end
    else if UserResponse = IDYES then
    begin
      UninstallOldVersions(GamePath);
      MsgBox('Текущая версия локализации удалена. Будет выполнена чистая установка.', mbInformation, MB_OK);
    end
    else if UserResponse = IDNO then
    begin
      if MsgBox('Установка поверх существующей версии может вызвать конфликты.' + #13#10 +
                'Рекомендуется сначала удалить текущую версию.' + #13#10#13#10 +
                'Продолжить установку поверх?', mbConfirmation, MB_YESNO) = IDNO then
      begin
        Result := False;
        Exit;
      end;
    end;
  end
  else
  begin
    if FileExists(GamePath + '\Mods\FHUORs2_RU_v.2.0.dll') or
       FileExists(GamePath + '\Mods\FHUORs2_RU_v.2.0.1.dll') or
       FileExists(GamePath + '\Mods\FHUOR2_Translation.dll') then
    begin
      DeletePrevious := MsgBox('Обнаружена старая версия локализации.' + #13#10#13#10 +
                               'Удалить её перед установкой новой версии?' + #13#10 +
                               'Рекомендуется выбрать "Да" для чистой установки.', 
                               mbConfirmation, MB_YESNO) = IDYES;
      
      if DeletePrevious then
      begin
        UninstallOldVersions(GamePath);
        MsgBox('Старая версия локализации удалена. Будет выполнена чистая установка.', mbInformation, MB_OK);
      end;
    end;
  end;
  
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
  ChangelogPage := CreateCustomPage(wpWelcome, 'Изменения в версии 2.0.2', 
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
    'Версия 2.0.2:' + #13#10 +
    '=============' + #13#10#13#10 +
    'Глобальные изменения:' + #13#10 +
    '• Добавлена автоматическая очистка старых версий локализации (поддержка версий с использованием фреймворка MelonLoader)' + #13#10 +
    '• Обновлены системы контекстного перевода для "Got it." и "Got it!"' + #13#10 +
    '• Улучшена стабильность работы мода' + #13#10 +
    '• Добавлены переводы для непереведенных строк' + #13#10 +
    '• Исправлены опечатки' + #13#10#13#10 +
    'Версия 2.0.1:' + #13#10 +
    '=============' + #13#10#13#10 +
    'Глобальные изменения:' + #13#10 +
    '• Ограничено создание логов работы фреймворка' + #13#10#13#10 +
    '• Добавлены переводы для непереведенных строк' + #13#10 +
    '• Исправлены опечатки' + #13#10#13#10 +
    'Версия 2.0:' + #13#10 +
    '===========' + #13#10#13#10 +
    'Глобальные изменения:' + #13#10 +
    '• Полностью переработан мод под фреймворк MelonLoader' + #13#10 +
    '• Поддержка официального патча игры на Il2Cpp' + #13#10#13#10 +
    'Примечание: Эта версия полностью переработана и требует наличия MelonLoader. ' +
    'Если у вас установлена старая версия локализации (BepInEx), рекомендуется удалить её перед установкой данной версии.'+ #13#10#13#10 +
    '• Добавлены переводы для непереведенных строк' + #13#10 +
    '• Исправлены опечатки';

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
    MsgBox('Русская локализация FHUORs2 (v.2.0.2) успешно установлена!' + #13#10#13#10 +
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
