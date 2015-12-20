Imports System.IO
Imports System.Security.Cryptography

Public Class fmMainWindow

#Region "Windows API Calls"
    Private Declare Ansi Function GetPrivateProfileString Lib "kernel32.dll" Alias "GetPrivateProfileStringA" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    Private Declare Ansi Function WritePrivateProfileString Lib "kernel32.dll" Alias "WritePrivateProfileStringA" (ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
#End Region

    Private ReadOnly _AvailableResolutions As New List(Of GameResolution)({New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_DEFAULT), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1024_600), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1280_720), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1280_800), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1366_768), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1440_900), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1680_1050), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1920_1080), _
                                                                           New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_1920_1200)})

    Private Const INI_SECTION As String = "GAMESETTINGS"
    Private Const INI_KEY_HEIGHT As String = "WindowHeight"
    Private Const INI_KEY_WIDTH As String = "WindowWidth"
    Private Const INI_KEY_FULLSCREEN As String = "Fullscreen"
    Private Const INI_KEY_SCREENMODE As String = "Screenmode"
    Private Const INI_DEFAULT_VALUE As String = ""

#Region "GameSettings Handling"
    ''' <summary>
    ''' Retrieves a string from the specified section in an initialization file.
    ''' </summary>
    ''' <param name="strSection">The name of the section containing the key name.</param>
    ''' <param name="strKey">The name of the key whose associated string is to be retrieved.</param>
    ''' <param name="strDefault">A default string. If the strKey cannot be found in the initialization file.</param>
    ''' <param name="strFile">The name of the initialization file.</param>
    ''' <returns>The return value is the number of characters copied to the buffer, not including the terminating null character.</returns>
    ''' <remarks>The GetPrivateProfileString function searches the specified initialization file for a key that matches the name specified by the lpKeyName parameter under the section heading specified by the lpAppName parameter. If it finds the key, the function copies the corresponding string to the buffer. If the key does not exist, the function copies the default character string specified by the lpDefault parameter.</remarks>
    Private Function INI_ReadValueFromFile(ByVal strSection As String, ByVal strKey As String, ByVal strDefault As String, ByVal strFile As String) As String
        Dim strTemp As String = Space(1024), lLength As Integer
        lLength = GetPrivateProfileString(strSection, strKey, strDefault, strTemp, strTemp.Length, strFile)
        Return (strTemp.Substring(0, lLength))
    End Function

    ''' <summary>
    ''' Copies a string into the specified section of an initialization file.
    ''' </summary>
    ''' <param name="strSection">The name of the section to which the string will be copied. If the section does not exist, it is created. The name of the section is case-independent; the string can be any combination of uppercase and lowercase letters.</param>
    ''' <param name="strKey">The name of the key to be associated with a string. If the key does not exist in the specified section, it is created. If this parameter is NULL, the entire section, including all entries within the section, is deleted.</param>
    ''' <param name="strValue">A null-terminated string to be written to the file. If this parameter is NULL, the key pointed to by the strKey parameter is deleted.</param>
    ''' <param name="strFile">The name of the initialization file.</param>
    ''' <returns>If the function successfully copies the string to the initialization file, the return value is nonzero. If the function fails, or if it flushes the cached version of the most recently accessed initialization file, the return value is zero.</returns>
    Public Function INI_WriteValueToFile(ByVal strSection As String, ByVal strKey As String, ByVal strValue As String, ByVal strFile As String) As Boolean
        Return (Not (WritePrivateProfileString(strSection, strKey, " " & strValue, strFile) = 0))
    End Function
#End Region

    Private Sub Button_Handler(sender As Object, e As EventArgs) Handles btnApply.Click, _
                                                                         btnExit.Click, _
                                                                         btnOpenFileDialog_S4_exe.Click, _
                                                                         btnPlay.Click
        Select Case True
            Case sender Is btnApply
                If cbResolutions.SelectedIndex >= 0 And tbS4_exe_Filepath.Text.Length > 0 Then
                    tbMessages.Clear()

                    Dim _NewResolution As GameResolution = ApplyNewResolution()
                    If _NewResolution.GameResolution = GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN Then
                        Message(_NewResolution.UnkownInfo)
                    Else
                        Message(String.Format("\nChanges applied successfully. Resolution set to {0} x {1} Pixel.", _
                                              _NewResolution.Width, _NewResolution.Height))
                    End If
                Else
                    MessageBox.Show("Please enter your Settlers 4 path and select any resolution," & Environment.NewLine & "then hit 'Apply'.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Case sender Is btnExit
                Application.Exit()
            Case sender Is btnOpenFileDialog_S4_exe
                Dim _FileDialog As OpenFileDialog = New OpenFileDialog With {.Title = "Select S4.exe from '<InstallDir>\S4.exe' ...", _
                                                                             .Filter = "Settlers 4 Executable|S4.exe", _
                                                                             .FilterIndex = 1, _
                                                                             .RestoreDirectory = True}
                If _FileDialog.ShowDialog() = DialogResult.OK Then
                    tbS4_exe_Filepath.Text = _FileDialog.FileName
                End If
            Case sender Is btnPlay
                If Process.GetProcessesByName("S4").Count = 0 Then
                    Try
                        Process.Start(tbS4_exe_Filepath.Text)
                    Catch ex As Exception
                        Message(ex.Message)
                        Exit Sub
                    End Try
                    Application.Exit()
                Else
                    Message("The Settlers 4 is already running.")
                End If
            Case Else
                Throw New Exception("Button not handeled.")
        End Select
    End Sub

    Private Sub fmMainWindow_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        lblVersion.Text = "Version: " & Application.ProductVersion

        Dim _DetectedResolution As GameResolution = GetCurrentResolution()
        If _DetectedResolution.GameResolution = GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN Then
            Message(_DetectedResolution.UnkownInfo)
        Else
            Message("Existing settings successfully read.")
        End If
        SetCheckBoxResolution(_DetectedResolution)
    End Sub

    Private Sub SetCheckBoxResolution(Resolution As GameResolution)
        Select Case Resolution.GameResolution
            Case GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_DEFAULT, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1024_600, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1280_720, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1280_800, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1366_768, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1440_900, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1680_1050, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1920_1080, _
                 GameResolution.AVAILABLE_RESOLUTIONS.RES_1920_1200
                cbResolutions.SelectedIndex = Resolution.CheckBoxIndex
            Case Else
                Throw New Exception("GameResolution not handled.")
        End Select
    End Sub

    Private Function ApplyNewResolution() As GameResolution
        Dim _fiS4_exe As FileInfo = Nothing, _fiGameSettings As FileInfo = Nothing, _fiGfxEngine As FileInfo = Nothing

        '// Perform the initial checks and get the FileInfos. If we get a empty string, all is OK.
        Dim _UnkownInfo As String = PerformInitialChecks(_fiS4_exe, _fiGameSettings, _fiGfxEngine)
        If Not String.Equals(_UnkownInfo, String.Empty) Then
            Return New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN, _UnkownInfo)
        End If

        '// Create a backup of the existing files if desired.
        If cbBackup.CheckState = CheckState.Checked AndAlso Not File.Exists(_fiGameSettings.FullName & ".bak") AndAlso Not File.Exists(_fiGfxEngine.FullName & ".bak") Then
            _fiGameSettings.CopyTo(_fiGameSettings.FullName & ".bak", False)
            _fiGfxEngine.CopyTo(_fiGfxEngine.FullName & ".bak", False)
            Message("GameSettings.cfg and GfxEngine.dll backed up as *.bak files.\n")
        End If

        '// Determine the correct GameResolution with the Index from the ComboBox. Then write the INI settings.
        Dim _Resolution As GameResolution = _AvailableResolutions.First(Function(r) r.CheckBoxIndex = cbResolutions.SelectedIndex)
        If Not INI_WriteValueToFile(INI_SECTION, INI_KEY_HEIGHT, _Resolution.Height, _fiGameSettings.FullName) Or _
           Not INI_WriteValueToFile(INI_SECTION, INI_KEY_WIDTH, _Resolution.Width, _fiGameSettings.FullName) Or _
           Not INI_WriteValueToFile(INI_SECTION, INI_KEY_FULLSCREEN, "1", _fiGameSettings.FullName) Or _
           Not INI_WriteValueToFile(INI_SECTION, INI_KEY_SCREENMODE, "2", _fiGameSettings.FullName) Then
            Return New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN, "A unkown error occured, while writing your GameSettings.cfg!\nPlease open the file with Notepad and check the content. It should look like:\n[GAMESETTINGS]\n{\n    AILevel = 0\nand so on...\nYou could also try to delete the file, it will be created new.")
        Else
            Message("GameSettings.cfg - PATCHED OK !")
            'Message(String.Format("GameSettings.cfg:\n{0} set to {1}.\n{2} set to {3}\n{4} set to {5}\n{6} set to {7}", _
            '                      INI_KEY_HEIGHT, _Resolution.Height, INI_KEY_WIDTH, _Resolution.Width, INI_KEY_FULLSCREEN, "1", INI_KEY_SCREENMODE, "2"))
        End If

        '// Delete the existing DLL file and create the new one.
        _fiGfxEngine.Delete()
        File.WriteAllBytes(_fiGfxEngine.FullName, _Resolution.Dll)
        Message("GfxEngine.dll - PATCHED OK !")

        Return _Resolution
    End Function

    Private Function PerformInitialChecks(ByRef fiS4_exe As FileInfo, ByRef fiGameSettings As FileInfo, ByRef fiGfxEngine As FileInfo) As String
        Message("Checking existing files...")
        If tbS4_exe_Filepath.Text.Length = 0 Then
            Return "Please enter your Settlers 4 path and select a resolution, then hit 'Apply'."
        End If

        fiS4_exe = New FileInfo(tbS4_exe_Filepath.Text)
        If Not fiS4_exe.Exists Then
            Return "<InstallDir>\S4.exe doesn't exist!"
        End If

        '// There should be no Settlers 4 process, because the INI file is overwritten after each start of the game and of course we shouldn't change a DLL which is in use.
        If Process.GetProcessesByName("S4").Count > 0 Then
            Return "Process S4.exe is running. Please close it."
        End If

        fiGameSettings = New FileInfo(Path.Combine(fiS4_exe.Directory.FullName, "Config\GameSettings.cfg"))
        fiGfxEngine = New FileInfo(Path.Combine(fiS4_exe.Directory.FullName, "Exe\GfxEngine.dll"))
        If Not fiGameSettings.Exists Or Not fiGfxEngine.Exists Then
            Return "<InstallDir>\Config\GameSettings.cfg or <InstallDir>\Exe\GfxEngine.dll doesn't exist!"
        End If
        Return String.Empty
    End Function

    Private Function GetCurrentResolution() As GameResolution
        Dim _fiS4_exe As FileInfo = Nothing, _fiGameSettings As FileInfo = Nothing, _fiGfxEngine As FileInfo = Nothing

        '// Perform the initial checks and get the FileInfos. If we get a empty string, all is OK.
        Dim _UnkownInfo As String = PerformInitialChecks(_fiS4_exe, _fiGameSettings, _fiGfxEngine)
        If Not String.Equals(_UnkownInfo, String.Empty) Then
            Return New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN, _UnkownInfo)
        End If

        '// Read the height and width from the INI file.
        Dim _Height As String = INI_ReadValueFromFile(INI_SECTION, INI_KEY_HEIGHT, INI_DEFAULT_VALUE, _fiGameSettings.FullName)
        Dim _Width As String = INI_ReadValueFromFile(INI_SECTION, INI_KEY_WIDTH, INI_DEFAULT_VALUE, _fiGameSettings.FullName)
        Dim _CurrentResolution_fromGameSettings As GameResolution = _AvailableResolutions.FirstOrDefault(Function(r) String.Equals(r.Height, _Height) AndAlso String.Equals(r.Width, _Width))

        '// Calculate the SHA1 hash of the DLL.
        Dim _GfxEngine_SHA1Hash As String = SHA1_CalculateHash(_fiGfxEngine)
        Dim _CurrentResolution_fromGfxEngine As GameResolution = _AvailableResolutions.FirstOrDefault(Function(r) String.Equals(r.Dll_SHA1Hash, _GfxEngine_SHA1Hash))

        '// Compare both results if they match.
        If Not IsNothing(_CurrentResolution_fromGameSettings) AndAlso Not IsNothing(_CurrentResolution_fromGfxEngine) AndAlso _CurrentResolution_fromGameSettings.GameResolution = _CurrentResolution_fromGfxEngine.GameResolution Then
            Return _CurrentResolution_fromGfxEngine
        Else
            Return New GameResolution(GameResolution.AVAILABLE_RESOLUTIONS.RES_UNKOWN, "Your GameSettings.cfg and GfxEngine.dll doesn't fit together.\nSelect any Resolution and click Apply to correct this.")
        End If
    End Function

    Private Function SHA1_CalculateHash(FileToHash As FileInfo) As String
        Const BLOCKSIZE As Integer = 256 * 256

        Using _SHA1 As New SHA1CryptoServiceProvider, _fs As New FileStream(FileToHash.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)
            '// Get filesize
            Dim _fileSize As Long = _fs.Length

            '// Declare buffer + other vars
            Dim _readBuffer(BLOCKSIZE - 1) As Byte, _readBytes As Integer
            Dim _transformBuffer As Byte(), transformBytes As Integer, _transformBytesTotal As Long = 0
            '// Read first block
            _readBytes = _fs.Read(_readBuffer, 0, BLOCKSIZE)
            '// Read + transform blockwise
            While _readBytes > 0
                '// Save last read
                _transformBuffer = _readBuffer
                transformBytes = _readBytes
                '// Read buffer
                _readBuffer = New Byte(BLOCKSIZE - 1) {}
                _readBytes = _fs.Read(_readBuffer, 0, BLOCKSIZE)
                '// Transform
                Select Case _readBytes
                    Case 0 : _SHA1.TransformFinalBlock(_transformBuffer, 0, transformBytes)
                    Case Else : _SHA1.TransformBlock(_transformBuffer, 0, transformBytes, _transformBuffer, 0)
                End Select
                '// Inform about progress here
                _transformBytesTotal += transformBytes
            End While
            '// All done
            Return HexStringFromBytes(_SHA1.Hash)
        End Using
    End Function

    ''' <summary>
    ''' Convert an array of bytes to a string of hex digits.
    ''' </summary>
    ''' <param name="bytes">Array of bytes.</param>
    ''' <returns>String of upper case hex digits.</returns>
    Public Shared Function HexStringFromBytes(bytes As Byte()) As String
        Dim _sb = New System.Text.StringBuilder()
        For Each b As Byte In bytes
            _sb.Append(b.ToString("x2"))
        Next
        Return _sb.ToString.ToUpper
    End Function

    Private Sub Message(FormattedMessage As String)
        '// \n is interpreted as a new line.
        tbMessages.AppendText(FormattedMessage.Replace("\n", Environment.NewLine) & Environment.NewLine)
    End Sub

End Class
