﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class adminPrintdefect
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(adminPrintdefect))
        Me.detail_code = New System.Windows.Forms.Label()
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.values = New System.Windows.Forms.Label()
        Me.LB_TITLE = New System.Windows.Forms.Label()
        Me.set_defaule_zero = New System.Windows.Forms.Label()
        Me.IN_FO = New System.Windows.Forms.Label()
        Me.LB_QTY = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lb_font12 = New System.Windows.Forms.Label()
        Me.lb_font1 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.title = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'detail_code
        '
        Me.detail_code.AutoSize = True
        Me.detail_code.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.detail_code.Location = New System.Drawing.Point(418, 275)
        Me.detail_code.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.detail_code.Name = "detail_code"
        Me.detail_code.Size = New System.Drawing.Size(123, 25)
        Me.detail_code.TabIndex = 92
        Me.detail_code.Text = "detail_code"
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Document = Me.PrintDocument1
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Visible = False
        '
        'PrintDocument1
        '
        '
        'values
        '
        Me.values.AutoSize = True
        Me.values.Font = New System.Drawing.Font("Arial", 14.0!, System.Drawing.FontStyle.Bold)
        Me.values.Location = New System.Drawing.Point(368, 635)
        Me.values.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.values.Name = "values"
        Me.values.Size = New System.Drawing.Size(138, 45)
        Me.values.TabIndex = 91
        Me.values.Text = "values"
        '
        'LB_TITLE
        '
        Me.LB_TITLE.AutoSize = True
        Me.LB_TITLE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.LB_TITLE.Location = New System.Drawing.Point(1038, 371)
        Me.LB_TITLE.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.LB_TITLE.Name = "LB_TITLE"
        Me.LB_TITLE.Size = New System.Drawing.Size(108, 26)
        Me.LB_TITLE.TabIndex = 90
        Me.LB_TITLE.Text = "LB_TITLE"
        '
        'set_defaule_zero
        '
        Me.set_defaule_zero.AutoSize = True
        Me.set_defaule_zero.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.set_defaule_zero.Location = New System.Drawing.Point(898, -173)
        Me.set_defaule_zero.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.set_defaule_zero.Name = "set_defaule_zero"
        Me.set_defaule_zero.Size = New System.Drawing.Size(183, 50)
        Me.set_defaule_zero.TabIndex = 89
        Me.set_defaule_zero.Text = "Label15"
        '
        'IN_FO
        '
        Me.IN_FO.AutoSize = True
        Me.IN_FO.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IN_FO.Location = New System.Drawing.Point(222, 404)
        Me.IN_FO.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.IN_FO.Name = "IN_FO"
        Me.IN_FO.Size = New System.Drawing.Size(102, 36)
        Me.IN_FO.TabIndex = 88
        Me.IN_FO.Text = "IN_FO"
        '
        'LB_QTY
        '
        Me.LB_QTY.AutoSize = True
        Me.LB_QTY.Font = New System.Drawing.Font("Consolas", 27.75!, System.Drawing.FontStyle.Bold)
        Me.LB_QTY.Location = New System.Drawing.Point(994, 67)
        Me.LB_QTY.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.LB_QTY.Name = "LB_QTY"
        Me.LB_QTY.Size = New System.Drawing.Size(283, 87)
        Me.LB_QTY.TabIndex = 87
        Me.LB_QTY.Text = "LB_QTY"
        Me.LB_QTY.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Arial Narrow", 68.0!, System.Drawing.FontStyle.Bold)
        Me.Label14.Location = New System.Drawing.Point(340, 425)
        Me.Label14.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(643, 206)
        Me.Label14.TabIndex = 86
        Me.Label14.Text = "Label14"
        '
        'lb_font12
        '
        Me.lb_font12.AutoSize = True
        Me.lb_font12.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lb_font12.Location = New System.Drawing.Point(110, 27)
        Me.lb_font12.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.lb_font12.Name = "lb_font12"
        Me.lb_font12.Size = New System.Drawing.Size(183, 51)
        Me.lb_font12.TabIndex = 85
        Me.lb_font12.Text = "Label12"
        '
        'lb_font1
        '
        Me.lb_font1.AutoSize = True
        Me.lb_font1.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lb_font1.Location = New System.Drawing.Point(1632, 312)
        Me.lb_font1.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.lb_font1.Name = "lb_font1"
        Me.lb_font1.Size = New System.Drawing.Size(166, 51)
        Me.lb_font1.TabIndex = 84
        Me.lb_font1.Text = "Label1"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Consolas", 84.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(244, 748)
        Me.Label13.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(980, 265)
        Me.Label13.TabIndex = 83
        Me.Label13.Text = "Label13"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Consolas", 42.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(-118, 67)
        Me.Label12.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(498, 134)
        Me.Label12.TabIndex = 82
        Me.Label12.Text = "Label12"
        '
        'title
        '
        Me.title.AutoSize = True
        Me.title.Font = New System.Drawing.Font("Arial", 8.5!)
        Me.title.Location = New System.Drawing.Point(132, 627)
        Me.title.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.title.Name = "title"
        Me.title.Size = New System.Drawing.Size(46, 26)
        Me.title.TabIndex = 81
        Me.title.Text = "title"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(414, 1056)
        Me.Label10.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(253, 68)
        Me.Label10.TabIndex = 80
        Me.Label10.Text = "Label10"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(102, 1056)
        Me.Label9.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(140, 44)
        Me.Label9.TabIndex = 79
        Me.Label9.Text = "Label9"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Consolas", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(-282, 1044)
        Me.Label8.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(180, 56)
        Me.Label8.TabIndex = 78
        Me.Label8.Text = "Label8"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(-278, 921)
        Me.Label7.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(95, 30)
        Me.Label7.TabIndex = 77
        Me.Label7.Text = "Label7"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(-290, 808)
        Me.Label6.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 20)
        Me.Label6.TabIndex = 76
        Me.Label6.Text = "Label6"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(-290, 633)
        Me.Label5.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(151, 48)
        Me.Label5.TabIndex = 75
        Me.Label5.Text = "Label5"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(-298, 404)
        Me.Label4.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(208, 67)
        Me.Label4.TabIndex = 73
        Me.Label4.Text = "Label4"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(-298, 196)
        Me.Label3.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 37)
        Me.Label3.TabIndex = 72
        Me.Label3.Text = "Label3"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(-302, 15)
        Me.Label2.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(175, 55)
        Me.Label2.TabIndex = 71
        Me.Label2.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(-290, -121)
        Me.Label1.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 29)
        Me.Label1.TabIndex = 70
        Me.Label1.Text = "Label1"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(660, -5)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(12, 12, 12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(920, 781)
        Me.PictureBox1.TabIndex = 74
        Me.PictureBox1.TabStop = False
        '
        'adminPrintdefect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1830, 1252)
        Me.Controls.Add(Me.detail_code)
        Me.Controls.Add(Me.values)
        Me.Controls.Add(Me.LB_TITLE)
        Me.Controls.Add(Me.set_defaule_zero)
        Me.Controls.Add(Me.IN_FO)
        Me.Controls.Add(Me.LB_QTY)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.lb_font12)
        Me.Controls.Add(Me.lb_font1)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.title)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "adminPrintdefect"
        Me.Text = "adminPrintdefect"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents detail_code As Label
    Friend WithEvents PrintPreviewDialog1 As PrintPreviewDialog
    Friend WithEvents PrintDocument1 As Printing.PrintDocument
    Friend WithEvents values As Label
    Friend WithEvents LB_TITLE As Label
    Friend WithEvents set_defaule_zero As Label
    Friend WithEvents IN_FO As Label
    Friend WithEvents LB_QTY As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents lb_font12 As Label
    Friend WithEvents lb_font1 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents title As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
