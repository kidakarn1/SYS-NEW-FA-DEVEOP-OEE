<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowMold
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShowMold))
        Me.lvMold = New System.Windows.Forms.ListView()
        Me.MoldNo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MoldName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MoldStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imc_id = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.mm_id = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.moldCavity = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnBack = New System.Windows.Forms.PictureBox()
        Me.btnSave = New System.Windows.Forms.PictureBox()
        CType(Me.btnBack, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnSave, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvMold
        '
        Me.lvMold.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.MoldNo, Me.MoldName, Me.MoldStatus, Me.imc_id, Me.mm_id, Me.moldCavity})
        Me.lvMold.Font = New System.Drawing.Font("Microsoft Sans Serif", 19.0!)
        Me.lvMold.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvMold.HideSelection = False
        Me.lvMold.Location = New System.Drawing.Point(12, 149)
        Me.lvMold.Name = "lvMold"
        Me.lvMold.Size = New System.Drawing.Size(776, 353)
        Me.lvMold.TabIndex = 0
        Me.lvMold.UseCompatibleStateImageBehavior = False
        Me.lvMold.View = System.Windows.Forms.View.Details
        '
        'MoldNo
        '
        Me.MoldNo.Text = "MoldNo"
        Me.MoldNo.Width = 250
        '
        'MoldName
        '
        Me.MoldName.Text = "MoldName"
        Me.MoldName.Width = 268
        '
        'MoldStatus
        '
        Me.MoldStatus.Text = "MoldStatus"
        Me.MoldStatus.Width = 250
        '
        'imc_id
        '
        Me.imc_id.Width = 0
        '
        'mm_id
        '
        Me.mm_id.Width = 0
        '
        'moldCavity
        '
        Me.moldCavity.Text = "moldCavity"
        Me.moldCavity.Width = 0
        '
        'btnBack
        '
        Me.btnBack.BackColor = System.Drawing.Color.Transparent
        Me.btnBack.Location = New System.Drawing.Point(3, 515)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(193, 82)
        Me.btnBack.TabIndex = 3
        Me.btnBack.TabStop = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSave.Location = New System.Drawing.Point(597, 509)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(193, 82)
        Me.btnSave.TabIndex = 4
        Me.btnSave.TabStop = False
        '
        'ShowMold
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.lvMold)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ShowMold"
        Me.Text = "ShowMold"
        CType(Me.btnBack, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnSave, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lvMold As ListView
    Friend WithEvents MoldNo As ColumnHeader
    Friend WithEvents MoldName As ColumnHeader
    Friend WithEvents MoldStatus As ColumnHeader
    Friend WithEvents imc_id As ColumnHeader
    Friend WithEvents mm_id As ColumnHeader
    Friend WithEvents moldCavity As ColumnHeader
    Friend WithEvents btnBack As PictureBox
    Friend WithEvents btnSave As PictureBox
End Class
