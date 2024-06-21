<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class defectSelectSupplier
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
        Me.lvDefectact = New System.Windows.Forms.ListView()
        Me.Supcd = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Supname = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lvDefectact
        '
        Me.lvDefectact.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lvDefectact.AllowColumnReorder = True
        Me.lvDefectact.AllowDrop = True
        Me.lvDefectact.AutoArrange = False
        Me.lvDefectact.BackColor = System.Drawing.Color.Peru
        Me.lvDefectact.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvDefectact.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Supcd, Me.Supname})
        Me.lvDefectact.Cursor = System.Windows.Forms.Cursors.Default
        Me.lvDefectact.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
        Me.lvDefectact.ForeColor = System.Drawing.Color.Black
        Me.lvDefectact.FullRowSelect = True
        Me.lvDefectact.GridLines = True
        Me.lvDefectact.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvDefectact.HideSelection = False
        Me.lvDefectact.Location = New System.Drawing.Point(74, 97)
        Me.lvDefectact.MultiSelect = False
        Me.lvDefectact.Name = "lvDefectact"
        Me.lvDefectact.ShowGroups = False
        Me.lvDefectact.Size = New System.Drawing.Size(662, 395)
        Me.lvDefectact.TabIndex = 57
        Me.lvDefectact.UseCompatibleStateImageBehavior = False
        Me.lvDefectact.View = System.Windows.Forms.View.Details
        '
        'Supcd
        '
        Me.Supcd.Text = "NO"
        Me.Supcd.Width = 100
        '
        'Supname
        '
        Me.Supname.Text = "PART NO"
        Me.Supname.Width = 560
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(301, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 55)
        Me.Label1.TabIndex = 58
        Me.Label1.Text = "Supplier"
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBack.Location = New System.Drawing.Point(51, 508)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(134, 80)
        Me.btnBack.TabIndex = 59
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(644, 508)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(134, 80)
        Me.btnOK.TabIndex = 60
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'defectSelectSupplier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lvDefectact)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "defectSelectSupplier"
        Me.Text = "defectSelectSupplier"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvDefectact As ListView
    Friend WithEvents Supcd As ColumnHeader
    Friend WithEvents Supname As ColumnHeader
    Friend WithEvents Label1 As Label
    Friend WithEvents btnBack As Button
    Friend WithEvents btnOK As Button
End Class
