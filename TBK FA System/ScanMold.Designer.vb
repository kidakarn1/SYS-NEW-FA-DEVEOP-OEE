<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScanMold
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScanMold))
        Me.tbLeaderMold = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.PictureBox()
        CType(Me.Button2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Button1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbLeaderMold
        '
        Me.tbLeaderMold.BackColor = System.Drawing.Color.FromArgb(CType(CType(58, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(95, Byte), Integer))
        Me.tbLeaderMold.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tbLeaderMold.Font = New System.Drawing.Font("Microsoft Sans Serif", 33.0!)
        Me.tbLeaderMold.ForeColor = System.Drawing.Color.White
        Me.tbLeaderMold.Location = New System.Drawing.Point(110, 246)
        Me.tbLeaderMold.Name = "tbLeaderMold"
        Me.tbLeaderMold.Size = New System.Drawing.Size(454, 50)
        Me.tbLeaderMold.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Transparent
        Me.Button2.Location = New System.Drawing.Point(580, 232)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(110, 114)
        Me.Button2.TabIndex = 5
        Me.Button2.TabStop = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Transparent
        Me.Button1.Location = New System.Drawing.Point(262, 389)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(227, 84)
        Me.Button1.TabIndex = 6
        Me.Button1.TabStop = False
        '
        'ScanMold
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(775, 477)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.tbLeaderMold)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ScanMold"
        Me.Text = "ScanMold"
        CType(Me.Button2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Button1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbLeaderMold As TextBox
    Friend WithEvents Button2 As PictureBox
    Friend WithEvents Button1 As PictureBox
End Class
