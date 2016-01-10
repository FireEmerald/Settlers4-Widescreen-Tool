<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmMainWindow
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmMainWindow))
        Me.cbBackup = New System.Windows.Forms.CheckBox()
        Me.btnOpenFileDialog_S4_exe = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbResolutions = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.tbMessages = New System.Windows.Forms.TextBox()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.tbS4_exe_Filepath = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbBackup
        '
        Me.cbBackup.AutoSize = True
        Me.cbBackup.Location = New System.Drawing.Point(173, 21)
        Me.cbBackup.Name = "cbBackup"
        Me.cbBackup.Size = New System.Drawing.Size(229, 17)
        Me.cbBackup.TabIndex = 7
        Me.cbBackup.Text = "Backup (GfxEngine.dll / GameSettings.cfg)"
        Me.cbBackup.UseVisualStyleBackColor = True
        '
        'btnOpenFileDialog_S4_exe
        '
        Me.btnOpenFileDialog_S4_exe.Location = New System.Drawing.Point(360, 20)
        Me.btnOpenFileDialog_S4_exe.Name = "btnOpenFileDialog_S4_exe"
        Me.btnOpenFileDialog_S4_exe.Size = New System.Drawing.Size(36, 23)
        Me.btnOpenFileDialog_S4_exe.TabIndex = 5
        Me.btnOpenFileDialog_S4_exe.Text = "..."
        Me.btnOpenFileDialog_S4_exe.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(175, 256)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 1
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tbS4_exe_Filepath)
        Me.GroupBox1.Controls.Add(Me.btnOpenFileDialog_S4_exe)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(408, 55)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "1. Settlers 4 (S4.exe) Executable"
        '
        'cbResolutions
        '
        Me.cbResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbResolutions.FormattingEnabled = True
        Me.cbResolutions.Items.AddRange(New Object() {"Default Resolution", "1024 x 600 Pixel (17:10)", "1280 x 720 Pixel (16:9)", "1280 x 800 Pixel (16:10)", "1366 x 768 Pixel (16:9)", "1440 x 900 Pixel (16:10)", "1680 x 1050 Pixel (16:10)", "1920 x 1080 Pixel (16:9)", "1920 x 1200 Pixel (16:10)"})
        Me.cbResolutions.Location = New System.Drawing.Point(9, 19)
        Me.cbResolutions.Name = "cbResolutions"
        Me.cbResolutions.Size = New System.Drawing.Size(152, 21)
        Me.cbResolutions.TabIndex = 6
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cbResolutions)
        Me.GroupBox2.Controls.Add(Me.cbBackup)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 73)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(408, 54)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "2. Select your Resolution"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(345, 256)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Help
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ButtonShadow
        Me.lblVersion.Location = New System.Drawing.Point(9, 266)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(140, 13)
        Me.lblVersion.TabIndex = 7
        Me.lblVersion.Text = "Version: X.X.X.X (README)"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.tbMessages)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 133)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(408, 117)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "3. Log"
        '
        'tbMessages
        '
        Me.tbMessages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbMessages.BackColor = System.Drawing.SystemColors.InfoText
        Me.tbMessages.ForeColor = System.Drawing.Color.Chartreuse
        Me.tbMessages.Location = New System.Drawing.Point(10, 19)
        Me.tbMessages.Multiline = True
        Me.tbMessages.Name = "tbMessages"
        Me.tbMessages.ReadOnly = True
        Me.tbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbMessages.Size = New System.Drawing.Size(386, 92)
        Me.tbMessages.TabIndex = 8
        '
        'btnPlay
        '
        Me.btnPlay.Location = New System.Drawing.Point(260, 256)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(75, 23)
        Me.btnPlay.TabIndex = 2
        Me.btnPlay.Text = "Start Now!"
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'tbS4_exe_Filepath
        '
        Me.tbS4_exe_Filepath.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.Settlers_4_Widescreen_Tool.My.MySettings.Default, "S4_exe_Filepath", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.tbS4_exe_Filepath.Location = New System.Drawing.Point(9, 22)
        Me.tbS4_exe_Filepath.Name = "tbS4_exe_Filepath"
        Me.tbS4_exe_Filepath.Size = New System.Drawing.Size(345, 20)
        Me.tbS4_exe_Filepath.TabIndex = 4
        Me.tbS4_exe_Filepath.Text = Global.Settlers_4_Widescreen_Tool.My.MySettings.Default.S4_exe_Filepath
        '
        'fmMainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(435, 291)
        Me.Controls.Add(Me.btnPlay)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnApply)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "fmMainWindow"
        Me.Text = "The Settlers 4: Widescreen Tool"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbBackup As System.Windows.Forms.CheckBox
    Friend WithEvents btnOpenFileDialog_S4_exe As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbResolutions As System.Windows.Forms.ComboBox
    Friend WithEvents tbS4_exe_Filepath As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents tbMessages As System.Windows.Forms.TextBox
    Friend WithEvents btnPlay As System.Windows.Forms.Button

End Class
