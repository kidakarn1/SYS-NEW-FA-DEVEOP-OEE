Public Class ProgressBar_Form
    Private Const StartHour As Integer = 8
    Private Const EndHour As Integer = 20
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ProgressBar1.Minimum = 0
        'ProgressBar1.Maximum = 100
        'ProgressBar1.Value = 0
        '' อัปเดตค่า ProgressBar
        '  ProgressBar1.Value = 50 ' อัปเดต ProgressBar ไปที่ 50%
        '  ProgressBar1.ForeColor = Color.Red
        ManageTimeline()
    End Sub
    Public Sub setProgressBar()
    End Sub
    Private Sub ManageTimeline()
        Dim currentTime As DateTime = DateTime.Now
        Dim currentHour As Integer = currentTime.Hour
        Dim timeArray() As String
        If currentHour >= StartHour AndAlso currentHour <= EndHour Then
            timeArray = {"08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00"}
        Else
            timeArray = {"20:00", "21:00", "22:00", "23:00", "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00"}
        End If
        ' For i As Integer = 0 To 12
        ' CreateLabel(timeArray(i), 54 * (i + 1), 445)
        ' CreatePanels(380, 470)
        ' Next
        Dim firstLabelWidth As Integer = 65
        Dim panelHeight As Integer = 25 ' ความสูงของ Label
        Dim yPosition As Integer = 490   ' ตำแหน่ง Y เริ่มต้น
        Dim xPosition As Integer = 65     ' ตำแหน่ง X เริ่มต้น
        Dim remainingWidth As Integer = 745 - firstLabelWidth
        Dim otherLabelsWidth As Integer = remainingWidth \ (timeArray.Length - 1)
        For i As Integer = 0 To timeArray.Length - 1
            Dim currentWidth As Integer
            If i = 0 Then
                currentWidth = firstLabelWidth ' ความกว้างเฉพาะตัวแรก
            Else
                currentWidth = otherLabelsWidth ' ความกว้างของตัวอื่นๆ
            End If
            ' สร้าง Label
            CreateLabel(xPosition, yPosition, currentWidth, panelHeight, timeArray(i))
            ' ปรับตำแหน่ง X สำหรับ Label ถัดไป
            xPosition += currentWidth
        Next
    End Sub
    Private Sub CreateLabel(x As Integer, y As Integer, width As Integer, height As Integer, text As String)
        ' สร้าง Label
        Dim labelTime As New Label()
        labelTime.Text = text
        labelTime.Font = New Font("Arial", 9)
        labelTime.AutoSize = False ' ปิด AutoSize เพื่อใช้ความกว้างที่กำหนด
        labelTime.Size = New Size(width, height)
        ' จัดตำแหน่ง Label
        labelTime.Location = New Point(x, y)
        labelTime.TextAlign = ContentAlignment.MiddleCenter ' จัดข้อความให้อยู่กลาง
        labelTime.BackColor = Color.FromArgb(52, 66, 254) ' ใช้สี #3442FE
        labelTime.ForeColor = Color.White
        ' เพิ่ม Label ลงในฟอร์ม
        Me.Controls.Add(labelTime)
        labelTime.BringToFront()
    End Sub
    Private Sub CreatePanels(numPanels As Integer, y As Integer)
        Dim W As Integer = 2
        For j As Integer = 0 To numPanels
            Dim panel As New Panel()
            panel.Location = New Point(W, y)
            panel.BackColor = If((j + 1) Mod 12 = 0, Color.FromArgb(183, 34, 34), Color.FromArgb(113, 255, 36))
            panel.Width = If(j = numPanels, 3, 2)
            panel.Height = 50
            Me.Controls.Add(panel)
            W += If(j = 0, 18, If(j = numPanels, 3, 2))
            panel.BringToFront()
        Next
    End Sub
    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs)
    End Sub
End Class