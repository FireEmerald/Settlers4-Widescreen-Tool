Public Class GameResolution
    Public Enum AVAILABLE_RESOLUTIONS
        RES_UNKOWN = -1
        RES_DEFAULT = 0
        RES_1024_600 = 1
        RES_1280_720 = 2
        RES_1280_800 = 3
        RES_1366_768 = 4
        RES_1440_900 = 5
        RES_1680_1050 = 6
        RES_1920_1080 = 7
        RES_1920_1200 = 8
    End Enum

    Private _GameResolution As AVAILABLE_RESOLUTIONS
    Private _Width, _Height, _Dll_SHA1Hash, _UnkownInfo As String
    Private _CheckBoxIndex As Integer
    Private _Dll As Byte()

    Sub New(GameResolution As AVAILABLE_RESOLUTIONS, Optional UnkownInfo As String = "")
        _GameResolution = GameResolution
        _UnkownInfo = UnkownInfo

        Select Case GameResolution
            Case AVAILABLE_RESOLUTIONS.RES_UNKOWN
                _Width = ""
                _Height = ""
                _Dll_SHA1Hash = ""
                _CheckBoxIndex = -1
                _Dll = Nothing
            Case AVAILABLE_RESOLUTIONS.RES_DEFAULT
                _Width = "1024"
                _Height = "768"
                _Dll_SHA1Hash = "F25CA243F617BB626614EFA8AB611509C971E6C4"
                _CheckBoxIndex = 0
                _Dll = My.Resources.GfxEngine_default
            Case AVAILABLE_RESOLUTIONS.RES_1024_600
                _Width = "1024"
                _Height = "600"
                _Dll_SHA1Hash = "4968B9D20D87C901F57AB37F1BCAAC405365A89A"
                _CheckBoxIndex = 1
                _Dll = My.Resources.GfxEngine_1024x600
            Case AVAILABLE_RESOLUTIONS.RES_1280_720
                _Width = "1280"
                _Height = "720"
                _Dll_SHA1Hash = "DE125A1E238D568D165DF0FFFEAB047C690C7ED2"
                _CheckBoxIndex = 2
                _Dll = My.Resources.GfxEngine_1280x720
            Case AVAILABLE_RESOLUTIONS.RES_1280_800
                _Width = "1280"
                _Height = "800"
                _Dll_SHA1Hash = "1107429472318D11DFF90691964281A4E61743BC"
                _CheckBoxIndex = 3
                _Dll = My.Resources.GfxEngine_1280x800
            Case AVAILABLE_RESOLUTIONS.RES_1366_768
                _Width = "1366"
                _Height = "768"
                _Dll_SHA1Hash = "1E3A0B201DC71F9E6D5AC0A9BF4419E575BE4799"
                _CheckBoxIndex = 4
                _Dll = My.Resources.GfxEngine_1366x768
            Case AVAILABLE_RESOLUTIONS.RES_1440_900
                _Width = "1440"
                _Height = "900"
                _Dll_SHA1Hash = "11D68FF1AA00DBE0E78528D1DD913EF3B34C1E23"
                _CheckBoxIndex = 5
                _Dll = My.Resources.GfxEngine_1440x900
            Case AVAILABLE_RESOLUTIONS.RES_1680_1050
                _Width = "1680"
                _Height = "1050"
                _Dll_SHA1Hash = "B9923B050E51C1A5F9E1DE8828861111DF811980"
                _CheckBoxIndex = 6
                _Dll = My.Resources.GfxEngine_1680x1050
            Case AVAILABLE_RESOLUTIONS.RES_1920_1080
                _Width = "1920"
                _Height = "1080"
                _Dll_SHA1Hash = "183DE9D83D2971AE9DCFD0E1ADB41A1A581C63FE"
                _CheckBoxIndex = 7
                _Dll = My.Resources.GfxEngine_1920x1080
            Case AVAILABLE_RESOLUTIONS.RES_1920_1200
                _Width = "1920"
                _Height = "1200"
                _Dll_SHA1Hash = "B08496740134C2B4660C864E3F0DB3C980F14C4B"
                _CheckBoxIndex = 8
                _Dll = My.Resources.GfxEngine_1920x1200
            Case Else
                Throw New Exception("GameResolution not handeled.")
        End Select
    End Sub

    Public ReadOnly Property GameResolution As AVAILABLE_RESOLUTIONS
        Get
            Return _GameResolution
        End Get
    End Property
    Public ReadOnly Property Width As String
        Get
            Return _Width
        End Get
    End Property
    Public ReadOnly Property Height As String
        Get
            Return _Height
        End Get
    End Property
    Public ReadOnly Property Dll_SHA1Hash As String
        Get
            Return _Dll_SHA1Hash
        End Get
    End Property
    Public ReadOnly Property UnkownInfo As String
        Get
            Return _UnkownInfo
        End Get
    End Property
    Public ReadOnly Property CheckBoxIndex As Integer
        Get
            Return _CheckBoxIndex
        End Get
    End Property
    Public ReadOnly Property Dll As Byte()
        Get
            Return _Dll
        End Get
    End Property
End Class
