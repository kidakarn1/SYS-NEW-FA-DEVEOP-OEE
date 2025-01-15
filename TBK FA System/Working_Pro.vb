Imports VB = Microsoft.VisualBasic
Imports System
Imports System.Management
Imports System.ComponentModel
Imports System.Threading
Imports System.IO
Imports System.IO.Ports
Imports System.Text
Imports System.Globalization
Imports System.Drawing.Imaging
Imports IDAutomation.Windows.Forms.LinearBarCode
Imports System.Drawing.Printing
Imports System.Configuration
Imports GenCode128
Imports BarcodeLib.Barcode
'Imports NationalInstruments.DAQmx 
Imports System.Net
Imports System.Web.Script.Serialization
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports QRCoder
Public Class Working_Pro
    Private WithEvents WebViewProgressbar As WebView2
    Private WithEvents WebViewEmergency As WebView2
    ' Public Shared comportTowerLamp = "COM7"
    ' Public Shared ArrayDataSpecial As New List(Of DataPlan)
    Public check_cal_eff As Integer = 0
    Public Gdate_now_date As Date
    Public Shared QtyMold As integer 
    Public Gtime As Date
    Public counterNewDIO
    Dim QR_Generator As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
    Dim start_flg As Integer = 0
    Dim comp_flg As Integer = 0
    Dim start_year As Integer = 0
    Dim start_month As Integer = 0
    Dim start_days As Integer = 0
    Dim Id As Short                             ' Device ID
    Dim Ret As Integer                          ' Return Code
    Dim szError As New StringBuilder("", 256)   ' Error String
    Dim szText As New String("", 100)
    Public Spwi_id As New List(Of String)
    Dim UpCount(2) As Short                     'zr Up Counter
    Dim DownCount(2) As Short                   ' Down Counter
    Dim Check(7) As CheckBox
    Dim Edit_Up(1) As TextBox
    Dim Edit_Down(7) As TextBox
    Dim loadingForm As New loadData()
    Dim delay_btn As Integer = 0
    Dim check_bull As Integer = 0
    Public Shared check_in_up_seq As Integer = 0
    Dim value_next_process As String = ""
    Public check_format_tag As String = Backoffice_model.B_check_format_tag()
    Public Shared api = New api()
    Public Shared check_tag_type = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/GET_LINE_TYPE?line_cd=" & MainFrm.Label4.Text)
    Public Shared load_data = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/GET_DATA_WORKING?WI=" & Prd_detail.lb_wi.Text)
    Public Shared V_check_line_reprint As String = Backoffice_model.check_line_reprint()
    Public WithEvents serialPort As New SerialPort
    'Dim digitalReadTask_new_dio As New Task()
    'Dim reader_new_dio As DigitalSingleChannelReader
    'Dim data_new_dio As UInt32
    Public Shared countCavity As Integer = 0
    Public s_mecg_name As String = ""
    Public s_delay As String = ""
    Public Shared wiNo As String = ""
    Public Shared pFg As String = ""
    Public Shared lineCd As String = ""
    Public Shared lotNo As String = ""
    Public Shared seqNo As String = ""
    Public Shared model As String = ""
    Public Shared pwi_id As String = ""
    Public Shared rsWindow
    Public Shared status_conter As String = ""
    Public Shared statusDefect As String = ""
    Public Shared tag_group_no As String = ""
    Public Shared mec_name As String = ""
    Public Shared comportTowerLamp = ""
    Public Shared status_emergency As String = ""
    Public Shared GoodQty As Double = 0.0
    Public Shared carvity As Integer = MainFrm.cavity.Text
    Public Shared ResultPrint As Integer = 0
    Public Shared statusPrint As String = "Normal"
    Delegate Sub SetTextCallback(ByVal [text] As String)
    Public Shared moe_min_a As Integer = 0
    Public Shared moe_min_p As Integer = 0
    Public Shared moe_min_q As Integer = 0
    Public Shared moe_min_oee As Integer = 0
    Public Shared gobal_stTimeModel As String = ""
    Public Shared statusSwitchModel As String = ""
    Public Shared IsOnlyone As String = ""
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Label44.Text = TimeOfDay.ToString("H:mm:ss")
        Label17.Text = TimeOfDay.ToString("H:mm:ss")
        'Label1.Text = DateTime.Now.ToString("D")
        Label1.Text = DateTime.Now.ToString("yyyy, MMMM dd")
        Label43.Text = DateTime.Now.ToString("yyyy/MM/dd")
        lb_loss_status.Text = lb_loss_status.Text
        If Backoffice_model.gobal_Flg_autoTranferProductions = 1 Then
            SetStartTime.Crr_time.Text = TimeOfDay.ToString("HH:mm:ss")
            SetStartTime.lb_dateCrr.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
        If lb_loss_status.Right < 0 Then
            lb_loss_status.Left = Panel6.ClientSize.Width
        Else
            lb_loss_status.Left -= 10
        End If
    End Sub
    Public Function checkStatusEmergency(line_cd As String)
        Try
            Dim api = New api()
            Dim result_data As String = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/checkStatusEmergency?line_cd=" & line_cd)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/checkStatusEmergency?line_cd=" & line_cd)
            If result_data <> "0" Then
                Dim dict2 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(result_data)
                For Each item As Object In dict2
                    status_emergency = item("me_status_emergency").ToString()
                Next
            End If
        Catch ex As Exception
            status_emergency = "0"
        End Try
        Return status_emergency
    End Function
    Public Sub RunCmd(line_cd As String)
        Try
            Dim api = New api()
            Dim Command As String = ""
            Dim parameters As String = ""
            Dim result_data As String = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_DATA_NEW_FA/RunCmd?line_cd=" & line_cd)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_DATA_NEW_FA/RunCmd?line_cd=" & line_cd)
            If result_data <> "0" Then
                Dim dict2 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(result_data)
                For Each item As Object In dict2
                    Command = item("command").ToString()
                    parameters = item("parameters").ToString()
                    System.Diagnostics.Process.Start(Command, parameters)
                Next
            End If
        Catch ex As Exception
            status_emergency = "0"
        End Try
    End Sub
    Public Function load_data_defeult_master_server(line_cd As String)
        Dim api = New api()
        Dim result_data As String = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/JOIN_CHECK_LINE_MASTER?line_cd=" & line_cd)
        If result_data <> "0" Then
            Dim dict2 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(result_data)
            For Each item As Object In dict2
                s_mecg_name = item("mecg_name").ToString()
                mec_name = item("mec_name").ToString()
                comportTowerLamp = item("metl_comport").ToString()
                If Backoffice_model.GET_STATUS_DELAY_BY_LINE(MainFrm.Label4.Text) = 0 Then
                    status_conter = 0
                    s_delay = item("me_sig_del").ToString()
                Else
                    status_conter = 1
                End If
            Next
        End If
    End Function
    Public Async Function setlvA(line_cd As String, lot_no As String, shift As String, dateStart As String, timeShift As String, stTimeModel As String, special_flg As String) As Task
        Dim OEE = New OEE_NODE
        Dim OEE_LOCAL = New OEE_SQLITE
        lvA.Items.Clear()
        lbOverTimeAvailability.Text = 0
        'stTimeModel = OEE.OEE_getDataGetWorkingTimeModel(timeShift, line_cd, Label3.Text)
        Dim rslvA = OEE.OEE_GET_Data_LOSS(line_cd, lot_no, shift, dateStart, stTimeModel, statusSwitchModel, IsOnlyone)
        ' Dim rslvA = OEE_LOCAL.OEE_GET_Data_LOSS(line_cd, lot_no, shift, dateStart, stTimeModel, statusSwitchModel, IsOnlyone, special_flg)
        If rslvA <> "0" Then
            Dim dict3 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rslvA)
            Try
                For Each item As Object In dict3
                    lbOverTimeAvailability.Text = item("AllLossTime").ToString()
                    datlvDefectsumary = New ListViewItem(item("loss_cd").ToString())
                    datlvDefectsumary.SubItems.Add(item("lossTime").ToString())
                    lvA.Items.Add(datlvDefectsumary)
                Next
            Catch ex As Exception
            End Try
        End If
    End Function
    Public Sub showWorkker()
        Dim emp_cd As String = List_Emp.ListView1.Items(0).Text
        Dim tclient As New WebClient
        Dim tImage As Bitmap = Nothing ' Initialize tImage to avoid potential null reference exceptions
        Try
            Dim url As String = "http://" & Backoffice_model.svApi & "/tbkk_shopfloor_sys/asset/img_emp/" & emp_cd & ".jpg"
            Dim data As Byte() = tclient.DownloadData(url)
            Using stream As New MemoryStream(data)
                tImage = New Bitmap(stream)
            End Using
        Catch ex As Exception
            ' If there's an error downloading or creating the image, load a default image
            Dim defaultUrl As String = "http://" & Backoffice_model.svApi & "/tbkk_shopfloor_sys/asset/img_emp/no_user.jpg"
            Dim defaultData As Byte() = tclient.DownloadData(defaultUrl)
            Using defaultStream As New MemoryStream(defaultData)
                tImage = New Bitmap(defaultStream)
            End Using
        End Try
        ' Assign the retrieved or default image to pcWorker1.Image
        pcWorker1.Image = tImage
        Label48.Text = "1"
        If CDbl(Val(Label29.Text)) > 1 Then
            tImage = Nothing
            emp_cd = List_Emp.ListView1.Items(1).Text
            Try
                Dim url As String = "http://" & Backoffice_model.svApi & "/tbkk_shopfloor_sys/asset/img_emp/" & emp_cd & ".jpg"
                Dim data As Byte() = tclient.DownloadData(url)
                Using stream As New MemoryStream(data)
                    tImage = New Bitmap(stream)
                End Using
            Catch ex As Exception
                ' If there's an error downloading or creating the image, load a default image
                Dim defaultUrl As String = "http://" & Backoffice_model.svApi & "/tbkk_shopfloor_sys/asset/img_emp/no_user.jpg"
                Dim defaultData As Byte() = tclient.DownloadData(defaultUrl)
                Using defaultStream As New MemoryStream(defaultData)
                    tImage = New Bitmap(defaultStream)
                End Using
            End Try
            pcWorker2.Image = tImage
            Label48.Text = "2"
        End If
    End Sub
    Public Function cal_progressbarQ(NGAll As String, Good As String)
        Dim rs = CInt(NGAll) + CInt(Good)
        Dim totalProgressbar = (Good / rs) * 100
        If Good = "0" And rs = "0" Then
            totalProgressbar = Int(100)
        End If
        If rs = "0" Then
            totalProgressbar = Int(100)
        End If
        If Math.Ceiling(totalProgressbar) > 100 Then
            progressbarQ.Text = Int(100) & "%"
            progressbarQ.Value = Int(100)
        ElseIf Math.Ceiling(totalProgressbar) < 0 Then
            progressbarQ.Text = Int(0) & "%"
            progressbarQ.Value = Int(0)
        Else
            progressbarQ.Text = Int(totalProgressbar) & "%"
            progressbarQ.Value = Int(totalProgressbar)
        End If

        If totalProgressbar >= moe_min_q Then
            progressbarQ.ProgressColor = Color.FromArgb(20, 255, 0) ' Green color in RGB
        ElseIf totalProgressbar < moe_min_q Then
            progressbarQ.ProgressColor = Color.Red
        End If
        Return totalProgressbar
        'CircularProgressBar2.Text = sum_prg2 & "%"
        'CircularProgressBar2.Value = sum_prg2
    End Function
    Public Sub calProgressOEE(A, Q, P)
        Dim total = A + Q + P
        If A > 100 Then
            A = 100
        ElseIf A < 0 Then
            A = 0
        End If
        If Q > 100 Then
            Q = 100
        ElseIf Q < 0 Then
            Q = 0
        End If
        If P > 100 Then
            P = 100
        ElseIf P < 0 Then
            P = 0
        End If
        A = A / 100
        Q = Q / 100
        P = P / 100
        ' Dim totalProgressbar = Int(FormatNumber((A * Q * P), 2) * 100) 'Int((total / 300) * 100)
        Dim result As Double = A * Q * P
        ' ปัดเศษให้เป็นทศนิยม 2 ตำแหน่ง
        Dim roundedResult As Double = Math.Round(result, 2)
        Dim flooredResult As Double = Math.Floor(result * 100)
        Dim totalProgressbar As Integer = flooredResult.ToString("F2")
        If totalProgressbar > 100 Then
            progressbarOEE.Text = Int(100) & "%"
            progressbarOEE.Value = Int(100)
        ElseIf totalProgressbar < 0 Then
            progressbarOEE.Text = Int(0) & "%"
            progressbarOEE.Value = Int(0)
        Else
            progressbarOEE.Text = totalProgressbar & "%"
            progressbarOEE.Value = totalProgressbar
        End If
        If totalProgressbar >= moe_min_oee Then
            progressbarOEE.ProgressColor = Color.FromArgb(20, 255, 0) ' Green color in RGB
            '   ElseIf totalProgressbar <= 90 And totalProgressbar >= 80 Then
            '       progressbarOEE.ProgressColor = Color.FromArgb(255, 97, 0) ' Green orange
        ElseIf totalProgressbar < moe_min_oee Then
            progressbarOEE.ProgressColor = Color.Red
        End If
    End Sub
    Public Function cal_progressbarA(line_cd As String, st_shift As String, end_shift As String)
        Dim OEE = New OEE_NODE
        'stTimeModel = OEE.OEE_getDataGetWorkingTimeModel(Prd_detail.Label12.Text.Substring(3, 5), line_cd, Label3.Text)
        Dim totalProgressbar = OEE.GetDataProgressbarA(st_shift, end_shift, line_cd, gobal_stTimeModel, statusSwitchModel, IsOnlyone)
<<<<<<< HEAD
        ' Dim totalProgressbar = OEE_LOCAL.mas_GetDataProgressbarA(st_shift, end_shift, line_cd, gobal_stTimeModel, statusSwitchModel, IsOnlyone, MainFrm.chk_spec_line)
=======
>>>>>>> parent of 5ffecdd (Updated Version 2.0.7)
        If totalProgressbar > 100 Then
            progressbarA.Text = Int(100) & "%"
            progressbarA.Value = Int(100)
        ElseIf totalProgressbar < 0 Then
            progressbarA.Text = Int(0) & "%"
            progressbarA.Value = Int(0)
        Else
            progressbarA.Text = totalProgressbar & "%"
            progressbarA.Value = totalProgressbar
        End If
        If totalProgressbar >= moe_min_a Then
            progressbarA.ProgressColor = Color.FromArgb(20, 255, 0) ' Green color in RGB
            ' ElseIf totalProgressbar <= 90 And totalProgressbar >= 80 Then
            '     progressbarA.ProgressColor = Color.FromArgb(255, 97, 0) ' Green orange
        ElseIf totalProgressbar < moe_min_a Then
            progressbarA.ProgressColor = Color.Red
        End If
        Return totalProgressbar
    End Function
    Public Function setgetSpeedLoss(NG As String, Good As String, timeShift As String, std_cd As String, line_cd As String, stTimeModel As String)
        'MsgBox("DateTimeStartofShift.Text======>" & DateTimeStartofShift.Text)
        Dim OEE = New OEE_NODE
        Dim OEE_LOCAL = New OEE_SQLITE
        'Dim per = "%"
        'per.Font = New Font("Arial", 20, FontStyle.Bold)
        'Dim rs = OEE.OEE_getSpeedLoss(NG, Good, timeShift, std_cd)
        Dim startDate As Date
        If statusSwitchModel = 0 Then
            'startDate = DateTime.Parse(DateTimeStartofShift.Text.ToString.Substring(0, 10)) & " " & timeShift
            startDate = DateTime.Parse(DateTimeStartofShift.Text.ToString)
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            '   If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
            '   startDate = startDate.AddDays(-1)
            'End If
            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss")
            Console.WriteLine("IFFFFFFFFFFFF0001")
        ElseIf statusSwitchModel = 1 Or statusSwitchModel = 2 Then
            If IsOnlyone = "1" Then
                ' startDate = DateTime.Parse(DateTimeStartofShift.Text.ToString.Substring(0, 10)) & " " & timeShift
                startDate = DateTime.Parse(DateTimeStartofShift.Text.ToString)
                Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
                Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
                ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
                Dim time As String = DateTime.Now.ToString("HH:mm:ss")
                Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
                Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
                ' If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                ' startDate = startDate.AddDays(-1)
                'End If
                Console.WriteLine("IFFFFFFFFFFFF0002")
                startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss")
            Else
                Console.WriteLine("ELSE")
                startDate = Convert.ToDateTime(stTimeModel).ToString("yyyy-MM-dd HH:mm:ss")
            End If
        End If
        If MainFrm.chk_spec_line = "2" Then ' for M083
            startDate = DateTime.Parse(Backoffice_model.date_time_start_master_shift)
            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss")
            If DateTime.Now.ToString("HH:mm:ss") >= "00:00:00 AM" And DateTime.Now.ToString("HH:mm:ss") <= "08:00:00 AM" Then
                startDate = Backoffice_model.date_time_start_master_shift.AddDays(-1)
            End If
        End If
        If Backoffice_model.S_chk_spec_line = "0" Then 'normal
        Else ' For K1M025
            startDate = DateTime.Parse(Backoffice_model.date_time_start_master_shift)
            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss")
            'MsgBox("IF ")
            If DateTime.Now.ToString("HH:mm:ss") >= "00:00:00 AM" And DateTime.Now.ToString("HH:mm:ss") <= "08:00:00 AM" Then
                If Backoffice_model.S_chk_spec_line = "0" Then ' Normal 
                Else 'for line M025
                    startDate = Backoffice_model.date_time_start_master_shift.AddDays(-1)
                End If
            End If
        End If
        Dim secSwitchmodel = DateDiff("s", startDate, Now())
        ' MsgBox("secSwitchmodel====>" & secSwitchmodel)
        Dim MinSwitchmodel As Integer = secSwitchmodel / 60
        ' MsgBox("MinSwitchmodel ====>" & MinSwitchmodel)
<<<<<<< HEAD
        Dim OEE_actual_detailByHour = OEE.OEE_getProduction_actual_detailByHour(line_cd, MinSwitchmodel, startDate, Label3.Text)
        'Dim OEE_actual_detailByHour = OEE_LOCAL.mas_getProduction_actual_detailByHour(line_cd, MinSwitchmodel, startDate, Label3.Text, MainFrm.chk_spec_line)
=======
        Dim OEE_actual_detailByHour = OEE.OEE_getProduction_actual_detailByHour(line_cd, MinSwitchmodel, startDate)
>>>>>>> parent of 5ffecdd (Updated Version 2.0.7)
        ' MsgBox("OEE_actual_detailByHour=====>" & OEE_actual_detailByHour)
        actualP.Text = OEE_actual_detailByHour
        ' Try
        ' หาความแตกต่างในหน่วยนาที
        ' startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss") wait test 
        '   MsgBox("startDate ===>" & startDate)
        ' MsgBox("Now ===>" & Now())
        rsDateDiff = DateDiff("s", startDate, Now())
        'MsgBox("rsDateDiff===>> + ==>" & rsDateDiff)
        Dim minrsDateDiff = rsDateDiff / 60
        If rsDateDiff < 3600 Then
            stdJobP.Text = Math.Ceiling(rsDateDiff / std_cd)
            Label25.Text = Math.Ceiling(minrsDateDiff)
            minACTUAL_P.Text = Math.Ceiling(minrsDateDiff)
            ' PanelSTDJOBP.Visible = True
            ' PanelACTP.Visible = True
            Try
                lbOverTimePerformance.Text = Math.Ceiling(rsDateDiff / actualP.Text)
            Catch ex As Exception
                lbOverTimePerformance.Text = 1
            End Try
        Else
            Dim GetLossByHouseP1 = OEE.OEE_GetLossByHouseP1(line_cd)
            'Dim GetLossByHouseP1 = OEE_LOCAL.mas_OEE_GetLossByHouseP1(line_cd) 
            Dim calHour = 0
            Dim rsmanage = 0
            If GetLossByHouseP1 <> "0" Then
                rsmanage = secSwitchmodel - GetLossByHouseP1
            Else
                rsmanage = secSwitchmodel
            End If
            'PanelSTDJOBP.Visible = False
            'PanelACTP.Visible = False
            stdJobP.Text = Math.Ceiling(rsmanage / std_cd)
            lbOverTimePerformance.Text = Math.Ceiling(rsmanage / actualP.Text)
        End If
        ' Catch ex As Exception
        '
        ' End Try
        ' stdJobP.Text = Math.Ceiling(3600 / std_cd) ' CInt((CDbl(Val(Label7.Text)) / CDbl(Val(HourOverAllShift.Text))))
        Dim workingTime = OEE.OEE_getWorkingTime(line_cd, timeShift)
        ' Dim workingTime = OEE_LOCAL.mas_GetWorkingTime(line_cd, timeShift)
        Dim workingTimemin = workingTime * 60
        Dim workingTimesec = workingTimemin * 60
        Dim rscalwork_std = workingTimesec / std_cd
        Dim totalProgressbar
        If actualP.Text = "0" And stdJobP.Text = "0" Then
            totalProgressbar = 0
        Else
            totalProgressbar = (actualP.Text / stdJobP.Text) * 100  '((Good + NG) / rscalwork_std)
        End If
        ' MsgBox("actualP.Text===>" & actualP.Text)
        'MsgBox("stdJobP.Text===>" & stdJobP.Text)
        ' MsgBox("totalProgressbar===>" & totalProgressbar)
        Console.WriteLine(totalProgressbar)
        If Math.Ceiling(totalProgressbar) > 100 Then
            progressbarP.Text = Int(100) & "%"
            progressbarP.Value = Int(100)
        ElseIf Math.Ceiling(totalProgressbar) < 0 Then
            progressbarP.Text = Int(0) & "%"
            progressbarP.Value = Int(0)
        Else
            progressbarP.Text = Int(totalProgressbar) & "%"
            progressbarP.Value = Int(totalProgressbar)
        End If
        If totalProgressbar >= moe_min_p Then
            progressbarP.ProgressColor = Color.FromArgb(20, 255, 0) ' Green color in RGB
            'ElseIf totalProgressbar <= 90 And totalProgressbar >= 80 Then
            '    progressbarP.ProgressColor = Color.FromArgb(255, 97, 0) ' Green orange
        ElseIf totalProgressbar < moe_min_p Then
            progressbarP.ProgressColor = Color.Red
        End If
        Return totalProgressbar
    End Function
    Public Sub setNgByHour(line_cd As String, lot_no As String)
        'Dim sqlite = New ModelSqliteDefect
        'Dim rs = sqlite.mSqliteGetDataNG_BYHOUR(line_cd, lot_no)
        'If rs <> "0" Then
        'Dim dict3 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
        'Try
        'For Each item As Object In dict3a
        'Next
        'Catch ex As Exception
        'End Try
        'End If
        'lbNG.Text = lbOverTimeQuality.Text
    End Sub
    Public Async Function set_AccTarget(TimestartShift As String, std_ct As String, stTimeModel As String) As Task
        Dim OEE = New OEE_NODE
        Dim OEE_LOCAL = New OEE_SQLITE
        lbAccTarget.Text = OEE.OEE_GET_Data_AccTarget(TimestartShift, std_ct)
        'lbAccTarget.Text = OEE_LOCAL.mas_OEE_GET_Data_AccTarget(TimestartShift, std_ct)
        ''
    End Function
    Public Async Function setlvQ(line_cd As String, lot_no As String, TimeShift As String, stTimeModel As String) As Task
        lvQ.Items.Clear()
        lbOverTimeQuality.Text = "0"
        lbNG.Text = "0"
        Dim OEE = New OEE_NODE
        Dim sqlite = New ModelSqliteDefect
        Dim startDate As Date
        'MsgBox("DateTimeStartofShift.Text.ToString=====>" & DateTimeStartofShift.Text.ToString)
        If statusSwitchModel = 0 Then
            startDate = DateTime.Parse(DateTimeStartofShift.Text.ToString)
            startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Console.WriteLine("setlvQ IFFFFFFFFFFFF0001")
        ElseIf statusSwitchModel = 1 Or statusSwitchModel = 2 Then
            If IsOnlyone = "1" Then
                startDate = DateTime.Parse(DateTimeStartofShift.Text.ToString)
                Console.WriteLine("setlvQ IFFFFFFFFFFFF0002")
                startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Else
                Console.WriteLine("setlvQ ELSE")
                startDate = Convert.ToDateTime(stTimeModel).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            End If
        End If
        Dim rslvQ = sqlite.mSqliteGetDataQuality(line_cd, lot_no, startDate)
        Dim rsOverAllNG = sqlite.mSqliteGetDataQualityOverAllNG(line_cd, lot_no, DateTimeStartofShift.Text)
        ' MsgBox("DateTimeStartofShift ===>" & DateTimeStartofShift.Text)
        If rsOverAllNG <> "0" Then
            Dim dictOverall As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rsOverAllNG)
            For Each item As Object In dictOverall
                lbNG.Text = item("AllDefect").ToString()
            Next
        End If
        If rslvQ <> "0" Then
            Dim dict3 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rslvQ)
            Try
                For Each item As Object In dict3
                    lbOverTimeQuality.Text = item("AllDefect").ToString()
                    datlvDefectsumary = New ListViewItem(item("dt_code").ToString())
                    datlvDefectsumary.SubItems.Add(item("TotalQ").ToString())
                    lvQ.Items.Add(datlvDefectsumary)
                Next
            Catch ex As Exception
            End Try
        End If
    End Function
    Private Async Function ShowLoadingAndLoadData() As Task
        loadingForm.Show()
        ' loadingForm.Dock = DockStyle.Fill
        ' loadingForm.BringToFront()
        ' เริ่มต้นการโหลดข้อมูลหรือทำงานที่ต้องการ
        Await LoadDataAsync()
        ' ปิดหน้าต่างกำลังโหลดเมื่อเสร็จสิ้นการโหลดข้อมูล
    End Function
    Private Async Function LoadDataAsync() As Task
        ' จำลองการโหลดข้อมูล (แทนที่ด้วยการโหลดข้อมูลจริง)
        Await Task.Delay(5000) ' รอ 3 วินาที
    End Function
    Private Async Sub Working_Pro_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadWebviewEmergency()
        'GenQrScanChecklist(MainFrm.Label4.Text)
        Me.Enabled = False
        statusPrint = "Normal"
        PanelProgressbar.BringToFront()
        Label29.BringToFront()
        'Me.Enabled = False
        If CheckOs() Then
            counterNewDIO = New CheckWindow
            counterNewDIO.Per_CheckCounter()
        End If
        Dim OEE = New OEE_NODE
        Dim OEE_LOCAL = New OEE_SQLITE
        showWorkker()
        Try
            ' เรียกใช้ฟังก์ชัน loadDataProgressBar แบบ Async
            Await loadDataProgressBar(MainFrm.Label4.Text, Label14.Text)
        Catch ex As Exception
            MessageBox.Show($"Failed to load data: {ex.Message}")
        End Try
        Await ShowLoadingAndLoadData()
        ''
        Dim mastOEE = OEE.OEE_LOAD_MSTOEE(MainFrm.Label4.Text) ' ใช้ API ได้ เนื่องจาก เป็นตัว ควบคุม OEE 
        Dim i As Integer = 1
        Try
            For Each item As Object In mastOEE
                If item("moe_min_oee").ToString Is Nothing Then 'ไม่ได้ Set OEE ใน Database' 
                    ' MsgBox("IF")
                Else
                    moe_min_a = item("moe_min_a").ToString
                    moe_min_p = item("moe_min_p").ToString
                    moe_min_q = item("moe_min_q").ToString
                    moe_min_oee = item("moe_min_oee").ToString
                End If
            Next
        Catch ex As Exception

        End Try
        Dim date_now As String = DateTime.Now.ToString("dd-MM-yyyy")
        Dim date_now_date As Date = DateTime.Now.ToString("dd-MM-yyyy")
        Gdate_now_date = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
        Dim time As Date = DateTime.Now.ToString("H:m:s")
        Dim date_st = DateTime.Now.ToString("dd-MM-yyyy")
        Dim date_end = DateTime.Now.ToString("dd-MM-yyyy")
        Dim result_qty = DateTime.Now.ToString("dd-MM-yyyy")
        Dim time_st = " 00:00:00"
        Dim time_end = " 23:59:59"
        If Label14.Text = "A" Then
            time_st = " 08:00:00"
            time_end = " 17:00:00"
        ElseIf Label14.Text = "P" Then
            time_st = " 08:00:00"
            time_end = " 20:00:00"
        ElseIf Label14.Text = "B" Then
            time_st = " 20:00:00"
            time_end = " 05:00:00"
        ElseIf Label14.Text = "Q" Then
            time_st = " 20:00:00"
            time_end = " 08:00:00"
        ElseIf Label14.Text = "S" Then
            time_st = " 17:00:00"
            time_end = " 02:00:00"
        End If
        If time > "23:59:59" Then
            date_st = date_now_date.AddDays(-1)
            date_st = Convert.ToDateTime(date_st).ToString("dd-MM-yyyy")
            date_end = date_now_date
            date_end = Convert.ToDateTime(date_end).ToString("dd-MM-yyyy")
            ' time_st = " 20:00:00"
            ' time_end = " 08:00:00"
        End If
        If time >= "00:00:00" And time <= "08:00:00" Then
            date_st = date_now_date.AddDays(-1)
            date_st = Convert.ToDateTime(date_st).ToString("dd-MM-yyyy")
            date_end = date_now_date
            date_end = Convert.ToDateTime(date_end).ToString("dd-MM-yyyy")
            '  time_st = " 20:00:00"
            '  time_end = " 08:00:00"
        End If
        DateTimeStartofShift.Text = OEE.OEE_getDateTimeStart(Prd_detail.Label12.Text.Substring(3, 5), MainFrm.Label4.Text)
        '  DateTimeStartofShift.Text = OEE_LOCAL.mas_OEE_getDateTimeStart(Prd_detail.Label12.Text.Substring(3, 5), MainFrm.Label4.Text)
        Dim DateTimeStartmasterShift As Date = date_st & " " & time_st
        tag_group_no = Backoffice_model.Get_tag_group_no()
        CircularProgressBar2.Visible = False
        Dim ObjGetmodel = OEE.OEE_getDataGetWorkingTimeModel(Prd_detail.Label12.Text.Substring(3, 5), MainFrm.Label4.Text, Label3.Text)
        ' Dim ObjGetmodel = Await OEE_LOCAL.mas_OEE_getDataGetWorkingTimeModel(Prd_detail.Label12.Text.Substring(3, 5), MainFrm.Label4.Text, Label3.Text)
        statusSwitchModel = ObjGetmodel("status").ToString()
        IsOnlyone = ObjGetmodel("IsOnlyone").ToString()
        Backoffice_model.Get_close_lot_time(Label14.Text)
        If ObjGetmodel("item_cd").ToString() = "New_Model" Then
            ' MsgBox("New_Model===>" & DateTimeStartmasterShift)
            gobal_stTimeModel = DateTimeStartmasterShift.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        ElseIf ObjGetmodel("item_cd").ToString() = "Switch_Model" Then
            '  MsgBox("sw model ObjGetmodel.ToString===>" & ObjGetmodel("rs").ToString)
            gobal_stTimeModel = ObjGetmodel("rs").ToString
        Else
            If ObjGetmodel("item_cd").ToString = Label3.Text Then
                ' MsgBox("__part ===>" & ObjGetmodel("rs").ToString)
                gobal_stTimeModel = ObjGetmodel("rs").ToString
            Else
                gobal_stTimeModel = DateTimeStartmasterShift.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                '   MsgBox("else part  ===>" & gobal_stTimeModel)
            End If
        End If
        Label7.Text = OEE.OEE_GET_NEW_TARGET(Prd_detail.Label12.Text.Substring(3, 5), Prd_detail.Label12.Text.Substring(11, 5), Label38.Text, Label14.Text) ' load Target Of Shift 
        ''
        Await setlvA(MainFrm.Label4.Text, Label18.Text, Label14.Text, DateTime.Now.ToString("yyyy-MM-dd"), Prd_detail.Label12.Text.Substring(3, 5), gobal_stTimeModel, MainFrm.chk_spec_line)
        Await setlvQ(MainFrm.Label4.Text, Label18.Text, Prd_detail.Label12.Text.ToString.Substring(3, 5), gobal_stTimeModel)
        Await set_AccTarget(Prd_detail.Label12.Text.Substring(3, 5), Label38.Text, gobal_stTimeModel)
        setNgByHour(MainFrm.Label4.Text, Label18.Text)
        'PictureBox12.Visible = False
        PictureBox10.Visible = True
        'PictureBox11.Visible = False
        Wait_data.Close()
        'Prd_detail.Timer3.Enabled = False
        Label47.Visible = False
        Timer1.Start()
        'sc_prd_plan.SerialPort1.Close()
        wiNo = wi_no.Text
        pFg = Label3.Text
        lineCd = MainFrm.Label4.Text
        lotNo = Label18.Text
        seqNo = Label22.Text
        model = lb_model.Text
        lb_loss_status.Parent = Panel6
        check_tag_type = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/GET_LINE_TYPE?line_cd=" & MainFrm.Label4.Text)
        load_data = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/GET_DATA_NEW_FA/GET_DATA_WORKING?WI=" & Prd_detail.lb_wi.Text)
        BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text)
        lbNextTime.Text = BreakTime
        ''
        HourOverAllShift.Text = OEE.OEE_GET_Hour(Label14.Text)
        'TimerLossBT.Start()
        V_check_line_reprint = Backoffice_model.check_line_reprint()
        Dim next_process = Backoffice_model.GET_NEXT_PROCESS()
        Backoffice_model.S_chk_spec_line = Backoffice_model.chk_spec_line()
        While next_process.read()
            value_next_process = next_process("NEXT_PROCESS").ToString
        End While
        next_process.Close()
        lb_loss_status.Location = New Point(Panel6.ClientSize.Width,
        Panel6.ClientSize.Height / 2 - (lb_loss_status.Height / 2))
        Check(0) = _Check_0
        Check(1) = _Check_1
        Edit_Up(0) = _Edit_Up_0
        Edit_Up(1) = _Edit_Up_1
        'Edit_DeviceName.Text = "DIO000"
        Edit_DeviceName.Text = MainFrm.lb_dio_port.Text
        Label24.Text = Backoffice_model.GET_LINE_PRODUCTION()
        For ii = 0 To 1
            Edit_Up(ii).Text = ""
        Next ii
        For iii = 0 To 1
            Check(iii).Checked = True
        Next
        connect_counter_qty()
        If Backoffice_model.GET_STATUS_DELAY_BY_LINE(MainFrm.Label4.Text) = 1 Then
            s_delay = (Prd_detail.lb_ct.Text * 60) / 2
            status_conter = 1
        Else
            status_conter = 0
        End If
        Dim reader_shift = Backoffice_model.GET_QTY_SHIFT(MainFrm.Label4.Text, wi_no.Text, Label14.Text, date_st, date_end, time_st, time_end, Label18.Text) '
        Dim reader_defact_nc = Backoffice_model.GET_QTY_DEFACT_NC(wi_no.Text, MainFrm.Label4.Text)
        While reader_defact_nc.read()
            lb_nc_qty.Text = reader_defact_nc("QTY_DEFACT_NC").ToString()
            If lb_nc_qty.Text = "" Then
                lb_nc_qty.Text = 0
            End If
        End While
        reader_defact_nc.close()
        'While reader_shift.read()
        'LB_COUNTER_SHIP.Text = reader_shift("QTY_SHIFT").ToString()
        'End While
        LB_COUNTER_SHIP.Text = OEE.OEE_getProduction_actual_detailByShift(MainFrm.Label4.Text, Label14.Text)
        'LB_COUNTER_SHIP.Text = OEE_LOCAL.mas_OEE_getProduction_actual_detailByShift(MainFrm.Label4.Text, Label14.Text)
        If LB_COUNTER_SHIP.Text = "" Then
            LB_COUNTER_SHIP.Text = 0
        Else
            If MainFrm.chk_spec_line = "2" Then
                LB_COUNTER_SHIP.Text = CDbl(Val(LB_COUNTER_SHIP.Text)) / Confrime_work_production.ArrayDataPlan.Count
                Label44.Text = "set"
                Label42.Text = "set"
            End If
        End If
        Dim reader_seq = Backoffice_model.GET_QTY_SEQ(wi_no.Text, CDbl(Val(Label22.Text))) '
        While reader_seq.read()
            If reader_seq.read Then
                LB_COUNTER_SEQ.Text = reader_seq("QTY_SEQ").ToString()
            Else
                LB_COUNTER_SEQ.Text = 0
            End If
        End While
        Dim mdDf = New modelDefect
        lb_good.Text = mdDf.mGetGoodWILot(wi_no.Text, Label18.Text)
        Main()
        Dim P = setgetSpeedLoss(lbOverTimeQuality.Text, lb_good.Text, Prd_detail.Label12.Text.Substring(3, 5), Label38.Text, MainFrm.Label4.Text, gobal_stTimeModel)
        Dim GoodByPartNo As Integer = CDbl(Val(actualP.Text)) - CDbl(Val(lbOverTimeQuality.Text))
        Dim Q = cal_progressbarQ(lbOverTimeQuality.Text, GoodByPartNo)
        Dim A = cal_progressbarA(MainFrm.Label4.Text, Prd_detail.Label12.Text.Substring(3, 5), Prd_detail.Label12.Text.Substring(11, 5))
        calProgressOEE(A, Q, P)
        Timer2.Start()
        Me.Enabled = True
        loadingForm.Hide()
        If Backoffice_model.gobal_Flg_autoTranferProductions = 1 Then
            SetStartTime.Show()
        End If
    End Sub
    Public Sub check_seq_data()
        If CDbl(Val(LB_COUNTER_SEQ.Text)) <> "0" Then
            lb_box_count.Text = lb_box_count.Text + 1
            Label_bach.Text += 1
            GoodQty = Label6.Text
            tag_print()
            lb_box_count.Text = 0
            Label_bach.Text = 0
        End If
    End Sub
    Public Sub connect_counter_qty()
        Dim load_total As String = load_data_defeult_master_server(MainFrm.Label4.Text)
        If s_mecg_name = "DIO-3232LX-USB" Then
            Try
                Dim szDeviceName As String
                ' Get device name from screen
                szDeviceName = Edit_DeviceName.Text
                ' Execute initialization function and get ID
                Ret = DioInit(szDeviceName, Id)
                ' Error process
                Call DioGetErrorString(Ret, szError)
                If Ret = 10000 Then
                    Edit_DeviceName.Text = "DIO001"
                    ' Get device name from screen
                    szDeviceName = Edit_DeviceName.Text
                    ' Execute initialization function and get ID
                    Ret = DioInit(szDeviceName, Id)
                    ' Error process
                    Call DioGetErrorString(Ret, szError)
                    If Ret = 10000 Then
                        Edit_DeviceName.Text = "DIO002"
                        ' Get device name from screen
                        szDeviceName = Edit_DeviceName.Text
                        ' Execute initialization function and get ID
                        Ret = DioInit(szDeviceName, Id)
                        ' Error process
                        Call DioGetErrorString(Ret, szError)
                    End If
                End If
                'MsgBox(Ret)
                'Edit_ReturnCode.Text = "Ret = " & Ret & ":" & szError.ToString()
                Dim i As Short
                Dim BitNo As Short
                Dim Kind As Short
                Dim Tim As Integer
                For i = 0 To 1
                    UpCount(i) = 0
                    DownCount(i) = 0
                Next i
                Tim = 100
                Kind = DIO_TRG_RISE
                For BitNo = 0 To 1
                    If Check(BitNo).CheckState = 1 Then
                        Ret = DioNotifyTrg(Id, BitNo, Kind, Tim, Me.Handle.ToInt32)
                        If (Ret <> DIO_ERR_SUCCESS) Then
                            'MsgBox("Connect success")
                            Exit For
                        End If
                    Else
                        Ret = DioStopNotifyTrg(Id, BitNo)
                        If (Ret <> DIO_ERR_SUCCESS) Then
                            'MsgBox("Connect Failed")
                            Exit For
                        End If
                    End If
                Next BitNo
                PictureBox10.Visible = False
                Label2.Visible = False
                Panel2.Visible = False
                PictureBox16.Visible = False
                Call DioGetErrorString(Ret, szError)
            Catch ex As Exception
                Button1.Enabled = False
                btnStart.Enabled = False
                btn_back.Enabled = False
                panelpcWorker1.Enabled = False
                btnInfo.Enabled = False
                Dim listdetail = "Please check DIO !"
                PictureBox10.BringToFront()
                PictureBox10.Show()
                PictureBox16.BringToFront()
                PictureBox16.Show()
                Panel2.BringToFront()
                Panel2.Show()
                Label2.Text = listdetail
                Label2.BringToFront()
                Label2.Show()
            End Try
        ElseIf s_mecg_name = "RS232" Then
            Try
                serialPort = Backoffice_model.OpenRS232(mec_name)
                If serialPort.IsOpen Then
                Else
                    serialPort.PortName = mec_name
                    serialPort.BaudRate = 9600
                    serialPort.Parity = IO.Ports.Parity.None
                    serialPort.StopBits = IO.Ports.StopBits.One
                    serialPort.DataBits = 8
                    serialPort.Open()
                    serialPort.RtsEnable = True
                End If
                'serialPort = serialPort(mec_name, 9600, Parity.None, 8, StopBits.One)
                PictureBox10.Visible = False
                Label2.Visible = False
                Panel2.Visible = False
                PictureBox16.Visible = False
            Catch ex As Exception
                Button1.Enabled = False
                btnStart.Enabled = False
                btn_back.Enabled = False
                panelpcWorker1.Enabled = False
                btnInfo.Enabled = False
                Dim listdetail = "Port Not Found Please Check Port. " & mec_name & "."
                PictureBox10.BringToFront()
                PictureBox10.Show()
                PictureBox16.BringToFront()
                PictureBox16.Show()
                Panel2.BringToFront()
                Panel2.Show()
                Label2.Text = listdetail
                Label2.BringToFront()
                Label2.Show()
            End Try
        ElseIf s_mecg_name = "NO DEVICE" Then
            PictureBox10.Visible = False
            Label2.Visible = False
            PictureBox16.Visible = False
            PictureBox15.Visible = False
        Else
            Console.WriteLine("CHECK OS")
            If CheckOs() Then
                Dim rs = counterNewDIO.count_NIMAX()
                If rs <> "OK" Then
                    Button1.Enabled = False
                    btnStart.Enabled = False
                    btn_back.Enabled = False
                    panelpcWorker1.Enabled = False
                    btnInfo.Enabled = False
                    Dim listdetail = "Please check DIO !"
                    PictureBox10.BringToFront()
                    PictureBox10.Show()
                    PictureBox16.BringToFront()
                    PictureBox16.Show()
                    Panel2.BringToFront()
                    Panel2.Show()
                    Label2.Text = listdetail
                    Label2.BringToFront()
                    Label2.Show()
                Else
                    PictureBox10.Visible = False
                    Label2.Visible = False
                    Panel2.Visible = False
                    PictureBox16.Visible = False
                End If
            Else
                'MsgBox("")
                Button1.Enabled = False
                btnStart.Enabled = False
                btn_back.Enabled = False
                panelpcWorker1.Enabled = False
                btnInfo.Enabled = False
                Dim listdetail = "Not Support Counter NI MAX because  OS window 7."
                PictureBox10.BringToFront()
                PictureBox10.Show()
                PictureBox16.BringToFront()
                PictureBox16.Show()
                Panel2.BringToFront()
                Panel2.Show()
                Label2.Text = listdetail
                Label2.BringToFront()
                Label2.Show()
            End If
        End If
    End Sub
    Private Sub Label8_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub btn_setup_Click(sender As Object, e As EventArgs) Handles btn_setup.Click, btnSetUp.Click
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Sel_prd_setup.Show()
                Me.Enabled = False
            Else
                load_show.Show()
            End If
        Catch ex As Exception
            load_show.Show()
        End Try
        'Dim i As Integer
        'ProgressBar1.Minimum = 0
        'ProgressBar1.Maximum = 200
        'For i = 0 To 200
        'ProgressBar1.Value = i
        'Next
    End Sub

    Private Sub SendMessageLong(hwnd As Object, prgBrClr As Object, p1 As Object, p2 As Object)
        Throw New NotImplementedException()
    End Sub
    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs)
    End Sub
    Public Sub stop_working()
        PanelProgressbar.Visible = False
        btnInfo.Visible = True
        btnStart.BringToFront()
        ' LB_COUNTER_SHIP.Visible = False
        'btnStart.Visible = True
        'PictureBox11.Visible = True
        'panelpcWorker1.BackColor = Color.FromArgb(63, 63, 63)
        'Panel3.Location = New Point(47, 209)
        'Label6.Location = New Point(38, 324)
        'Label10.Location = New Point(38, 439)
        'Label24.BackColor = Color.FromArgb(63, 63, 63)
        'Label17.BackColor = Color.FromArgb(63, 63, 63)
        ' Label1.BackColor = Color.FromArgb(63, 63, 63)
        'Label3.BackColor = Color.FromArgb(63, 63, 63)
        'Label18.BackColor = Color.FromArgb(63, 63, 63)
        'Label18.Location = New Point(490, 121)
        'Label16.BackColor = Color.FromArgb(63, 63, 63)
        Label20.Visible = False
        'Label20.BackColor = Color.FromArgb(63, 63, 63)
        LB_COUNTER_SEQ.SendToBack()
        CircularProgressBar2.Visible = False
        'CircularProgressBar2.BackColor = Color.FromArgb(63, 63, 63)
        'CircularProgressBar2.InnerColor = Color.FromArgb(63, 63, 63)
        ' Label7.Location = New Point(150, 25)
        '        Label7.BackColor = Color.FromArgb(12, 27, 45)
        ''        Label7.BringToFront()

        ' Label8.Location = New Point(56, 297)
        'Label8.BackColor = Color.FromArgb(12, 27, 45)
        'Label8.BringToFront()

        ' Label6.Location = New Point(43, 403)
        'Label6.BackColor = Color.FromArgb(12, 27, 45)
        'Label6.BringToFront()
        '   Label10.Location = New Point(41, 510)
        'Label10.BackColor = Color.FromArgb(12, 27, 45)
        'Label10.BringToFront()
        Dim line_id As String = MainFrm.line_id.Text
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.line_status_upd(line_id)
            Else
                Backoffice_model.line_status_upd_sqlite(line_id)
            End If
        Catch ex As Exception
            Backoffice_model.line_status_upd_sqlite(line_id)
        End Try '
        Dim date_st As String = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
        Dim date_end As String = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.line_status_ins(line_id, date_st, date_end, "1", "0", 24, "0", Prd_detail.lb_wi.Text)
            Else
                Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "1", "0", 24, "0", Prd_detail.lb_wi.Text)
            End If
        Catch ex As Exception
            Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "1", "0", 24, "0", Prd_detail.lb_wi.Text)
        End Try
        start_flg = 0
        'Chang_Loss.Show()
        Button1.Visible = False
        Panel1.BackColor = Color.Red
        Label30.Text = "STOPPED"
        'btn_back.Visible = True
        btnSetUp.Visible = True
        btn_ins_act.Visible = True
        btn_desc_act.Visible = True
        btnDefects.Visible = True
        btnInfo.Visible = True
        btnCloseLot.Visible = True
        PictureBox11.Visible = True
        redBox.Visible = True
        btn_stop.Visible = True
        btnStart.Visible = True
        CheckMenu()
    End Sub
    Private Sub btn_stop_Click(sender As Object, e As EventArgs) Handles btn_stop.Click
        check_network_frist = 1
        Try
            If s_mecg_name = "RS232" Then '
                serialPort.Close()
            End If
            serialPort.Close()
        Catch ex As Exception
        End Try
        stop_working()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim dfHome = New defectHome()
        dfHome.Show()
        'defectHome.Show()
        Me.Enabled = False
        'Close_lot.Show()
        'Me.Close()
    End Sub
    Private Sub Label10_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub Label7_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label28_Click(sender As Object, e As EventArgs) Handles Label28.Click

    End Sub
    Private Sub Label29_Click(sender As Object, e As EventArgs) Handles Label29.Click

    End Sub
    Private Sub Label10_Click_1(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub
    Private Sub CircularProgressBar1_Click(sender As Object, e As EventArgs) Handles CircularProgressBar1.Click

    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btn_back.Click
        Dim line_id As String = MainFrm.line_id.Text
        Backoffice_model.line_status_upd(line_id)
        Prd_detail.Enabled = True
        Prd_detail.Timer3.Enabled = True
        Insert_list.ListView1.View = View.Details
        Dim line_cd As String = MainFrm.Label4.Text
        Dim LoadSQL = Backoffice_model.get_prd_plan(line_cd)
        Dim numberOfindex As Integer = 0
        While LoadSQL.Read()
            'MsgBox(LoadSQL("prd_flg").ToString())
            If LoadSQL("prd_flg").ToString() = 1 Then
                'MsgBox("Red")
                Insert_list.ListView1.ForeColor = Color.Red
                Insert_list.ListView1.Items.Add(LoadSQL("WI").ToString()).SubItems.AddRange(New String() {LoadSQL("ITEM_CD").ToString(), LoadSQL("ITEM_NAME").ToString(), LoadSQL("QTY").ToString(), LoadSQL("remain_qty").ToString()})
                Insert_list.ListView1.Items(numberOfindex).ForeColor = Color.Red
            Else
                'MsgBox("Blue")
                Insert_list.ListView1.ForeColor = Color.Blue
                Insert_list.ListView1.Items.Add(LoadSQL("WI").ToString()).SubItems.AddRange(New String() {LoadSQL("ITEM_CD").ToString(), LoadSQL("ITEM_NAME").ToString(), LoadSQL("QTY").ToString(), LoadSQL("remain_qty").ToString()})
                Insert_list.ListView1.Items(numberOfindex).ForeColor = Color.Blue
            End If
            Insert_list.ListBox1.Items.Add(LoadSQL("PS_UNIT_NUMERATOR"))
            Insert_list.ListBox2.Items.Add(LoadSQL("CT"))
            Insert_list.ListBox3.Items.Add(LoadSQL("seq_count"))
            Insert_list.lbx_dlv_date.Items.Add(LoadSQL("DLV_DATE"))
            Insert_list.lbx_location.Items.Add(LoadSQL("LOCATION_PART"))
            Insert_list.lbx_model.Items.Add(LoadSQL("MODEL"))
            Insert_list.lbx_prd_type.Items.Add(LoadSQL("PRODUCT_TYP"))
            numberOfindex = numberOfindex + 1
            'Insert_list.ListView1.ForeColor = Color.Red
            'Insert_list.ListView1.Items.Add(LoadSQL("WI").ToString()).SubItems.AddRange(New String() {LoadSQL("ITEM_CD").ToString(), LoadSQL("ITEM_NAME").ToString(), LoadSQL("QTY").ToString(), LoadSQL("remain_qty").ToString()})
            'line_id.Text = LoadSQL("line_id").ToString()
        End While
        'Insert_list.Show()
        Dim mdModel = New modelMoldCavity
        Dim rsUpdate = mdModel.mUpdateStatusproduction("2", mdModel.imc_id, MainFrm.Label4.Text, "0", "2")
        Prd_detail.Show()
        Me.Close()
    End Sub
    Public Sub close_lot_seq()
        Dim st_counter As String = Label32.Text
        If st_counter = "0" Then
            Label16.Text = TimeOfDay.ToString("H : mm")
            Label32.Text = "1"
            btn_back.Enabled = False
            If lb_ch_man_flg.Text = "1" Then
                Dim value_temps1 As Double
                value_temps1 = Double.TryParse(Label34.Text, value_temps1)
                Dim testt1 As Integer = Label34.Text
                Dim newDate1 As Date = DateAdd("n", testt1, Now)
                Label20.Text = newDate1.ToString("H : mm")
            End If
            Dim pd As String = MainFrm.Label6.Text
            Dim line_cd As String = MainFrm.Label4.Text
            Dim wi_plan As String = wi_no.Text
            Dim item_cd As String = Label3.Text
            Dim item_name As String = Label12.Text
            Dim staff_no As String = Label29.Text
            Dim seq_no As String = Label22.Text
            Dim prd_qty As Integer = 0
            Dim start_time As Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim end_time As Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim use_time As Double = 0.00
            Dim tr_status As String = "0"
            Dim number_qty As Integer = Label6.Text
            Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    ' MsgBox("ready insert data sqlite if ")
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_time, number_qty, pwi_id, tr_status)
                    'MsgBox("Ping completed")
                Else
                    ' MsgBox("ready insert data sqlite else ")
                    tr_status = "0"
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    'MsgBox("Ping incompleted")
                End If
            Catch ex As Exception
                tr_status = "0"
                ' MsgBox("ready insert data sqlite catch ")
                Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
            End Try
            st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            btnSetUp.Enabled = True
            btn_ins_act.Enabled = True
            btn_desc_act.Enabled = True
            btnDefects.Enabled = True
            PictureBox11.Enabled = True
            btnInfo.Enabled = True
            btnCloseLot.Enabled = True
            redBox.Enabled = True
            'Dim temppo As Double = Label34.Text
            CircularProgressBar2.Text = 0 & "%"
            CircularProgressBar2.Value = 0
            Dim value_temps As Double
            value_temps = Double.TryParse(Label34.Text, value_temps)
            Dim testt As Integer = Label34.Text
            Dim newDate As Date = DateAdd("n", testt, Now)
            Label20.Text = newDate.ToString("H : mm")
            'MsgBox(Label20.Text)
        Else
            'Label32.Text = "0"
        End If
        Panel1.BackColor = Color.Green
        Label30.Text = "NORMAL"
        btnStart.Visible = False
        btn_back.Visible = False
        btnSetUp.Visible = False
        btn_ins_act.Visible = False
        btn_desc_act.Visible = False
        btnDefects.Visible = False
        PictureBox11.Visible = False
        btnInfo.Visible = False
        btnCloseLot.Visible = False
        redBox.Visible = False
        btn_stop.Visible = True
        Prd_detail.Timer3.Enabled = False
    End Sub
    Private Sub btn_start_Click(sender As Object, e As EventArgs) Handles btn_start.Click
        Start_Production()
    End Sub
    Public Sub ins_loss_code(pd As String, line_cd As String, wi_plan As String, item_cd As String, seq_no As String, shift_prd As String, start_loss As String, end_loss As String, total_loss As String, loss_type As String, loss_cd_id As String, op_id As String, pwi_id As String)
        Dim flg_control As String = "0"
        If loss_cd_id = "36" Then
            flg_control = "1"
            op_id = "0"
        Else
            flg_control = "0"
        End If
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                transfer_flg = "1"
                Backoffice_model.ins_loss_act(pd, line_cd, wi_plan, item_cd, seq_no, shift_prd, start_loss, end_loss, total_loss, loss_type, loss_cd_id, op_id, transfer_flg, flg_control, pwi_id)
                Backoffice_model.ins_loss_act_sqlite(pd, line_cd, wi_plan, item_cd, seq_no, shift_prd, start_loss, end_loss, total_loss, loss_type, loss_cd_id, op_id, transfer_flg, flg_control, pwi_id)
            Else
                transfer_flg = "0"
                Backoffice_model.ins_loss_act_sqlite(pd, line_cd, wi_plan, item_cd, seq_no, shift_prd, start_loss, end_loss, total_loss, loss_type, loss_cd_id, op_id, transfer_flg, flg_control, pwi_id)
            End If
        Catch ex As Exception
            transfer_flg = "0"
            Backoffice_model.ins_loss_act_sqlite(pd, line_cd, wi_plan, item_cd, seq_no, shift_prd, start_loss, end_loss, total_loss, loss_type, loss_cd_id, op_id, transfer_flg, flg_control, pwi_id)
        End Try
    End Sub
    Public Async Function Start_Production() As Task '
        If check_network_frist = 0 Then
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    Prd_detail.Timer3.Enabled = False
                End If
            Catch ex As Exception
            End Try
        End If
        If check_in_up_seq = 0 Then
            Dim date_st1 As String = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
            Dim date_end1 As String = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
            ' MsgBox("Backoffice_model.gobal_DateTimeComputerDown===:>" & Backoffice_model.gobal_DateTimeComputerDown)
            If Backoffice_model.gobal_DateTimeComputerDown = "" Then
                Backoffice_model.date_time_click_start = DateTime.Now.ToString("yyyy-MM-dd HH:mm") & ":00"
            Else
                Dim dateTimeconvert As DateTime = Backoffice_model.gobal_DateTimeComputerDown.ToString
                Backoffice_model.date_time_click_start = dateTimeconvert.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ' มาจาก คอมดับ 
            End If
            'MsgBox("asdasd=================>" & Backoffice_model.date_time_click_start)
            Dim rsTime As Integer = calTimeBreakTime(Backoffice_model.date_time_click_start, lbNextTime.Text)
            '  TimerCountBT.Interval = rsTime * 1000
            '  If rsTime <> 0 Then
            '  TimerCountBT.Enabled = True
            '  TimerCountBT.Start()
            'End If

            Dim rsInsertData As String = ""
            Dim rsInsertDataSqlite As String = ""
            Dim GET_SEQ
            If MainFrm.chk_spec_line = "2" Then
                Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                Dim Iseq = GenSEQ
                For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                    Iseq += 1
                    Dim indRow As String = itemPlanData.IND_ROW
                    Dim pwi_shift As String = itemPlanData.IND_ROW
                    rsInsertData = Backoffice_model.INSERT_production_working_info(indRow, Label18.Text, Iseq, Label14.Text)
                    'rsInsertDataSqlite = model_api_sqlite.mas_INSERT_production_working_info(indRow, Label18.Text, Iseq, Label14.Text, pwi_id)
                    GET_SEQ = Backoffice_model.GET_SEQ_PLAN_current(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1)
                Next
            Else
                rsInsertData = Backoffice_model.INSERT_production_working_info(LB_IND_ROW.Text, Label18.Text, Label22.Text, Label14.Text)
                'rsInsertDataSqlite = model_api_sqlite.mas_INSERT_production_working_info(LB_IND_ROW.Text, Label18.Text, Label22.Text, Label14.Text, pwi_id)
                GET_SEQ = Backoffice_model.GET_SEQ_PLAN_current(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1)
            End If
            Try
                If GET_SEQ.Read() Then
                    Dim C_seq_no As Integer = CDbl(Val(GET_SEQ("seq_no")))
                    If C_seq_no > 0 Then
                        Dim seq_no_naja = GET_SEQ("seq_no")
                        If MainFrm.chk_spec_line = "2" Then
                            Dim update_data = Backoffice_model.Update_seqplan(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1, CDbl(Val(Prd_detail.lb_seq.Text)) + CDbl(Val(MainFrm.ArrayDataPlan.ToArray().Length)))
                            '     Dim update_datasqlite = model_api_sqlite.mas_Update_seqplan(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1, CDbl(Val(Prd_detail.lb_seq.Text)) + CDbl(Val(MainFrm.ArrayDataPlan.ToArray().Length)))
                        Else
                            Dim update_data = Backoffice_model.Update_seqplan(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1, CDbl(Val(Prd_detail.lb_seq.Text)) + CDbl(Val(MainFrm.ArrayDataPlan.ToArray().Length)))
                            '  Dim update_datasqlite = model_api_sqlite.mas_Update_seqplan(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1, CDbl(Val(Prd_detail.lb_seq.Text)) + CDbl(Val(MainFrm.ArrayDataPlan.ToArray().Length)))
                        End If
                    Else
                        Dim insert_data = Backoffice_model.INSERT_tmp_planseq(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1, Label22.Text, "1")
                        Dim seq_no_naja = 0
                    End If
                End If
            Catch ex As Exception
                Dim insert_data = Backoffice_model.INSERT_tmp_planseq(Prd_detail.lb_wi.Text, Backoffice_model.GET_LINE_PRODUCTION(), date_st1, date_end1, Label22.Text, "1")
                Dim seq_no_naja = 0
            End Try
            GET_SEQ.close()
            Dim temp_co_emp As Integer = List_Emp.ListView1.Items.Count
            If MainFrm.chk_spec_line = "2" Then
                Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                Dim Iseq = GenSEQ
                Spwi_id = New List(Of String)
                For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                    Iseq += 1
                    Dim indRow As String = itemPlanData.IND_ROW
                    Dim pwi_shift As String = itemPlanData.IND_ROW
                    Dim wi As String = itemPlanData.wi
                    pwi_id = Backoffice_model.GET_DATA_PRODUCTION_WORKING_INFO(indRow, Label18.Text, Iseq)
                    rsInsertDataSqlite = model_api_sqlite.mas_INSERT_production_working_info(indRow, Label18.Text, Iseq, Label14.Text, pwi_id)
                    Spwi_id.Add(pwi_id)
                    ' ArrayDataSpecial.Add(New GSpwi_id With {.Spwi_id = pwi_id})
                    For i = 0 To temp_co_emp - 1
                        emp_cd = List_Emp.ListView1.Items(i).Text
                        Backoffice_model.Insert_production_emp_detail_realtime(wi, emp_cd, Iseq, pwi_id)
                    Next
                Next
            Else
                pwi_id = Backoffice_model.GET_DATA_PRODUCTION_WORKING_INFO(LB_IND_ROW.Text, Label18.Text, Label22.Text)
                rsInsertDataSqlite = model_api_sqlite.mas_INSERT_production_working_info(LB_IND_ROW.Text, Label18.Text, Label22.Text, Label14.Text, pwi_id)
                For i = 0 To temp_co_emp - 1
                    emp_cd = List_Emp.ListView1.Items(i).Text
                    Backoffice_model.Insert_production_emp_detail_realtime(wi_no.Text, emp_cd, Label22.Text, pwi_id)
                    'MsgBox(List_Emp.ListView1.Items(i).Text)
                Next
            End If
            Dim OEE = New OEE_NODE
            Dim mdmold = New modelMoldCavity
            Await mdmold.mUpdateStatusProduction("1", mdmold.imc_id, MainFrm.Label4.Text, "0", "2")
            If mdmold.shortTest <> "0" Then
                Dim rs = Await updateShot(mdmold.mm_id, pwi_id, mdmold.shortTest, "1", mdmold.ima_start_date, mdmold.ima_end_date, "1", MainFrm.Label4.Text, MainFrm.Label4.Text)
                If rs = "1" Then
                    Me.Close()
                End If
                mdmold.shortTest = "0"
            End If
            Dim maxShort = modelMoldCavity.mcheckMaxMold(modelMoldCavity.mm_id)
            If maxShort = 0 Then
                MsgBox("Please Check Mold & Cavity.")
                Prd_detail.Show()
                moldCalculator.lbMax.Text = modelMoldCavity.mcheckMaxMold(modelMoldCavity.mm_id)
                Prd_detail.Test_short.Enabled = False
                Prd_detail.Enabled = True
                Me.Close()
            End If
            'Dim ObjGetmodel = OEE.OEE_getDataGetWorkingTimeModel(Prd_detail.Label12.Text.Substring(3, 5), Label24.Text, Label3.Text)
            'gobal_stTimeModel = ObjGetmodel("rs").ToString
            check_in_up_seq += 1
            ' Await the completion of setting DateTimeStartofShift.Text
        End If
<<<<<<< HEAD
        insLossClickStart(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture))
=======
        Main()
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Dim bf = New Backoffice_model
                Dim RsCheckProduction_Plan = bf.Get_Plan_All_By_Line(Backoffice_model.GET_LINE_PRODUCTION(), Label14.Text, DateTime.Now.ToString("yyyy-MM-dd"))
                If RsCheckProduction_Plan <> "0" Then
                    Dim loss_type As String = "0"
                    Dim op_id As String = "0"
                    Dim dict3 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(RsCheckProduction_Plan)
                    Dim start_loss As String = ""
                    Dim end_loss_codex As String = ""
                    Dim start_loss_codex As String = ""
                    Dim Loss_Time_codex As String = ""
                    Dim Loss_Code As String = ""
                    Try
                        For Each item As Object In dict3
                            start_loss_codex = item("Start_Loss").ToString()
                            end_loss_codex = item("End_Loss").ToString()
                            Loss_Time_codex = item("Loss_Time").ToString()
                            Loss_Code = item("Loss_Code").ToString()
                            If CDbl(Val(Loss_Time_codex)) > 0 Then
                                If MainFrm.chk_spec_line = "2" Then
                                    Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                                    Dim Iseq = GenSEQ
                                    Dim j As Integer = 0
                                    For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                                        Iseq += 1
                                        Dim indRow As String = itemPlanData.IND_ROW
                                        Dim wi As String = itemPlanData.wi
                                        Dim item_cd As String = itemPlanData.item_cd
                                        ins_loss_code(MainFrm.Label6.Text, MainFrm.Label4.Text, wi, item_cd, Iseq, Label14.Text, start_loss_codex, end_loss_codex, Loss_Time_codex, loss_type, Loss_Code, "0", Spwi_id(j))
                                        j = j + 1
                                    Next
                                Else
                                    ins_loss_code(MainFrm.Label6.Text, MainFrm.Label4.Text, wi_no.Text, Label3.Text, CDbl(Val(Label22.Text)), Label14.Text, start_loss_codex, end_loss_codex, Loss_Time_codex, loss_type, Loss_Code, "0", pwi_id)
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                End If
            End If
        Catch ex As Exception
        End Try
>>>>>>> parent of 5ffecdd (Updated Version 2.0.7)
        Dim line_id As String = MainFrm.line_id.Text
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.line_status_upd(line_id)
                Backoffice_model.line_status_upd_sqlite(line_id)
            Else
                Backoffice_model.line_status_upd_sqlite(line_id)
            End If
        Catch ex As Exception
            Backoffice_model.line_status_upd_sqlite(line_id)
        End Try
        Dim date_st As String = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
        Dim date_end As String = DateTime.Now.ToString("yyyy/MM/dd H:m:s")
        If MainFrm.chk_spec_line = "2" Then
            Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
            Dim Iseq = GenSEQ
            Dim j As Integer = 0
            For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                Iseq += 1
                Dim wi As String = itemPlanData.wi
                Try
                    If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                        Backoffice_model.line_status_ins(line_id, date_st, date_end, "2", "0", 0, "0", wi)
                        Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "2", "0", 0, "0", wi)
                    Else
                        Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "2", "0", 0, "0", wi)
                    End If
                Catch ex As Exception
                    Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "2", "0", 0, "0", wi)
                End Try
            Next
        Else
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    Backoffice_model.line_status_ins(line_id, date_st, date_end, "2", "0", 0, "0", Prd_detail.lb_wi.Text)
                    Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "2", "0", 0, "0", Prd_detail.lb_wi.Text)
                Else
                    Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "2", "0", 0, "0", Prd_detail.lb_wi.Text)
                End If
            Catch ex As Exception
                Backoffice_model.line_status_ins_sqlite(line_id, date_st, date_end, "2", "0", 0, "0", Prd_detail.lb_wi.Text)
            End Try
        End If
        Dim c_type As String = MainFrm.count_type.Text
        If c_type = "TOUCH" Then
            Button1.Visible = True
        Else
            Button1.Visible = False
        End If
        start_flg = 1
        'Button1.Visible = True
        Dim st_counter As String = Label32.Text
        If st_counter = "0" Then
            If Backoffice_model.gobal_DateTimeComputerDown = "" Then

                Label16.Text = TimeOfDay.ToString("H:mm:ss")

            Else
                Dim dateTimeconvert As DateTime = Backoffice_model.gobal_DateTimeComputerDown.ToString
                Label16.Text = dateTimeconvert.ToString("H:mm:ss", CultureInfo.InvariantCulture) ' มาจาก คอมดับ 
            End If
            Label32.Text = "1"
            btn_back.Enabled = False
            If lb_ch_man_flg.Text = "1" Then
                Dim value_temps1 As Double
                value_temps1 = Double.TryParse(Label34.Text, value_temps1)
                Dim testt1 As Integer = Label34.Text
                Dim newDate1 As Date = DateAdd("n", testt1, Now)
                Label20.Text = newDate1.ToString("H : mm")
            End If
            pd = MainFrm.Label6.Text
            Dim line_cd As String = MainFrm.Label4.Text
            Dim wi_plan As String = wi_no.Text
            Dim item_cd As String = Label3.Text
            Dim item_name As String = Label12.Text
            Dim staff_no As String = Label29.Text
            seq_no = Label22.Text
            Dim prd_qty As Integer = 0
            Dim start_time As Date
            'MsgBox("Backoffice_model.gobal_DateTimeComputerDown ===>" & Backoffice_model.gobal_DateTimeComputerDown)
            If Backoffice_model.gobal_DateTimeComputerDown = "" Then
                start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                'MsgBox("start_time===>" & start_time)
            Else
                Dim dateTimeconvert As DateTime = Backoffice_model.gobal_DateTimeComputerDown.ToString
                start_time = dateTimeconvert.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ' มาจาก คอมดับ 
            End If
            Dim end_time As Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim use_time As Double = 0.00
            Dim tr_status As String = "0"
            Dim number_qty As Integer = Label6.Text
            Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Try
                'MsgBox("Backoffice_model.gobal_QTYComputerDown===>" & Backoffice_model.gobal_QTYComputerDown)
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            Backoffice_model.Insert_prd_detail(pd, line_cd, wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, start_time, end_time, use_time, number_qty, Spwi_id(j), tr_status)
                            If Backoffice_model.gobal_DateTimeComputerDown = "0" Then

                            Else
                                If Backoffice_model.gobal_QTYComputerDown > "0" Then
                                    insComputerDown(Backoffice_model.gobal_DateTimeComputerDown)
                                End If
                            End If
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_time, number_qty, pwi_id, tr_status)
                        If Backoffice_model.gobal_DateTimeComputerDown = "" Then
                        Else
                            If Backoffice_model.gobal_QTYComputerDown > "0" Then
                                insComputerDown(Backoffice_model.gobal_DateTimeComputerDown)
                            End If
                        End If
                    End If
                    'MsgBox("Ping completed")
                Else
                    tr_status = "0"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi, special_item_cd, special_item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                    'MsgBox("Ping incompleted")
                End If
            Catch ex As Exception
                tr_status = "0"
                If MainFrm.chk_spec_line = "2" Then
                    Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                    Dim Iseq = GenSEQ
                    Dim j As Integer = 0
                    For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                        Iseq += 1
                        Dim wi As String = itemPlanData.wi
                        Dim special_item_cd As String = itemPlanData.item_cd
                        Dim special_item_name As String = itemPlanData.item_name
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        j = j + 1
                    Next
                Else
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End If
            End Try
            st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            'Starting
            btnSetUp.Enabled = True
            btn_ins_act.Enabled = True
            btn_desc_act.Enabled = True
            btnDefects.Enabled = True
            PictureBox11.Enabled = True
            btnInfo.Enabled = True
            btnCloseLot.Enabled = True
            redBox.Enabled = True
            'Starting
            'End
            'End
            'Dim temppo As Double = Label34.Text
            CircularProgressBar2.Text = 0 & "%"
            CircularProgressBar2.Value = 0
            Dim value_temps As Double
            value_temps = Double.TryParse(Label34.Text, value_temps)
            Dim testt As Integer = Label34.Text
            Dim newDate As Date = DateAdd("n", testt, Now)
            Label20.Text = newDate.ToString("H : mm")
            'MsgBox(Label20.Text)
        Else
            'Label32.Text = "0"
        End If
        statusPrint = "Normal"
        cal_eff()
        'TIME_CAL_EFF.Start()
        LB_COUNTER_SHIP.Visible = True
        Panel1.BackColor = Color.Green
        Label30.Text = "NORMAL"
        btnStart.Visible = False
        btn_back.Visible = False
        btnSetUp.Visible = False
        btn_ins_act.Visible = False
        btn_desc_act.Visible = False
        btnDefects.Visible = False
        PictureBox11.Visible = False
        btnInfo.Visible = False
        btnCloseLot.Visible = False
        redBox.Visible = False
        PanelProgressbar.Visible = True
        'Dim rswebview = loadDataProgressBar(Label24.Text, Label14.Text)
        ' WebViewProgressbar.Reload()
        btn_stop.Visible = True
        Prd_detail.Timer3.Enabled = False
        btnStart.Visible = False
        'PictureBox11.Visible = False
        'PictureBox12.Visible = True
        'PictureBox10.Visible = True
        'Label24.BackColor = Color.FromArgb(44, 93, 129)
        'Label24.BringToFront()
        'btn_stop.BringToFront()
        'Label17.BackColor = Color.FromArgb(44, 93, 129)
        'Label17.BringToFront()
        'Label1.BackColor = Color.FromArgb(44, 93, 129)
        'Label1.BringToFront()
        'Label29.BackColor = Color.FromArgb(44, 93, 129)
        '        Label3.BackColor = Color.FromArgb(44, 88, 130)
        '        Label3.BringToFront()
        '        Label18.BackColor = Color.FromArgb(44, 88, 130)
        '        Label18.BringToFront()

        '  Label16.BackColor = Color.FromArgb(44, 82, 131)
        '  Label16.BringToFront()
        '  Label20.BackColor = Color.FromArgb(44, 82, 131)
        '  Label20.BringToFront()
        'CircularProgressBar2.BackColor = Color.FromArgb(44, 67, 133)
        'CircularProgressBar2.InnerColor = Color.FromArgb(44, 67, 133)
        'CircularProgressBar2.BringToFront()
        ' lbNextTime.BringToFront()
        'Panel7.BringToFront()
        ' lb_ng_qty.BringToFront()
        'LB_COUNTER_SHIP.BringToFront()
        'LB_COUNTER_SEQ.BringToFront()
        'Label29.BringToFront()
        panelpcWorker1.BackColor = Color.FromArgb(44, 93, 129)
        btnInfo.BackColor = Color.FromArgb(44, 88, 130)
        '        Label7.Location = New Point(420, 120)
        '        Label7.BackColor = Color.FromArgb(62, 97, 146)
        '       Label7.BringToFront()
        'Label8.Location = New Point(56, 297)
        'Label8.BackColor = Color.FromArgb(12, 27, 45)
        'Label8.BringToFront()
        '
        'Label6.Location = New Point(43, 403)
        'Label6.BackColor = Color.FromArgb(12, 27, 45)
        '  Label6.BringToFront()
        'Label10.Location = New Point(41, 510)
        ' Label10.BackColor = Color.FromArgb(12, 27, 45)
        ' Label10.BringToFront()
        Label20.Visible = True
        'LB_COUNTER_SEQ.Visible = True
        'LB_COUNTER_SEQ.BringToFront()
        ' CircularProgressBar2.Visible = True
        'MsgBox("ready load")
        connect_counter_qty()
        ' MsgBox("ready load2")
        'MsgBox("CDbl(Val(check_in_up_seq)) - 1)====>" & CDbl(Val(check_in_up_seq)) - 1)
        CheckMN()
        If (CDbl(Val(check_in_up_seq)) - 1) = 0 Then
            Dim OEE = New OEE_NODE
            Dim OEE_LOCAL = New OEE_SQLITE
            DateTimeStartofShift.Text = OEE.OEE_getDateTimeStart(Prd_detail.Label12.Text.Substring(3, 5), MainFrm.Label4.Text)
            ' DateTimeStartofShift.Text = OEE_LOCAL.mas_OEE_getDateTimeStart(Prd_detail.Label12.Text.Substring(3, 5), MainFrm.Label4.Text)
            ' MsgBox("DateTimeStartofShift.Text load seq ====>" & DateTimeStartofShift.Text)
            ' Await the completion of LOAD_OEE
            ' MsgBox("frith start ===>")
            'MsgBox("ready Check mold")
            If modelMoldCavity.imc_id <> "0" Then
                MainFrm.cavity.Text = modelMoldCavity.cavity
            End If
            Await LOAD_OEE()
        End If
        Main()
    End Function
    Public Async Function CheckMN() As Task
        Dim modelmn = New modelMaintenance
        If modelmn.getDataMN(MainFrm.Label4.Text) <> "0" Then
            Me.Enabled = False
            Sel_prd_setup.loadDataLossCrr()
            Chang_Loss.btnNextLossCrr()
            Loss_reg.Button4.Visible = True
            Loss_reg.GetDefectMenuMaintenance()
            Await Loss_reg.LoadMN()
            'Chang_Loss.Show()
        End If
    End Function
    Public Sub insComputerDown(tb As String)
        'MsgBox("tb===>" & tb)
        LB_COUNTER_SHIP.Text = CDbl(Val(LB_COUNTER_SHIP.Text)) + CDbl(Val(tb))
        LB_COUNTER_SEQ.Text = CDbl(Val(LB_COUNTER_SEQ.Text)) + CDbl(Val(tb))
        QtyMold = QtyMold + CDbl(Val(tb))
        lb_good.Text = CDbl(Val(lb_good.Text)) + CDbl(Val(tb))
        Dim max_val As String = Label10.Text
        max_val = max_val.Substring(1, max_val.Length - 1)
        Dim max_val_int As Integer = Convert.ToInt32(max_val)
        Backoffice_model.qty_int = tb
        lb_ins_qty.Text = tb
        'MsgBox("tb2222===>" & tb)
        ins_qty_fn_manual()
    End Sub
    Public Async Function loadDataProgressBar(line_cd As String, shift As String) As Task
        ' ตรวจสอบและ Dispose instance ของ WebViewProgressbar หากมีการสร้างไว้ก่อนแล้ว
        ' If WebViewProgressbar IsNot Nothing Then
        ' WebViewProgressbar.Dispose()
        ' End If
        ' Create a new instance of WebView2 control
        WebViewProgressbar = New WebView2() With {
            .Dock = DockStyle.Fill
        }
        PanelProgressbar.Controls.Add(WebViewProgressbar)
        Try
            ' กำหนดไดเรกทอรีสำหรับ environment
            Dim webViewEnvironment = Await CoreWebView2Environment.CreateAsync(Nothing, "C:\Temp")
            ' สร้าง instance ของ WebView2 control
            Await WebViewProgressbar.EnsureCoreWebView2Async(webViewEnvironment)
            ' เรียกใช้ URL โดยแสดงค่า line_cd และ shift
            WebViewProgressbar.CoreWebView2.Navigate("http://" & Backoffice_model.svApi & "/productionHrprogressDev/?line_cd=" & line_cd & "&shift=" & shift)
            'WebViewProgressbar.CoreWebView2.Navigate("http://" & Backoffice_model.svApi & "/productionHrprogress/?line_cd=" & line_cd & "&shift=" & shift)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/productionHrprogressDev/?line_cd=" & line_cd & "&shift=" & shift)
        Catch ex As Exception
            ' แสดงข้อผิดพลาดในกรณีที่การเริ่มต้นใช้งาน WebView2 ล้มเหลว
            Console.WriteLine($"Failed to initialize WebView2: {ex.Message}")
        End Try
    End Function
    Public Async Function loadWebviewEmergency() As Task
        ' ตรวจสอบว่ามี WebView2 instance ที่ใช้งานอยู่หรือไม่ ถ้ามีให้ Dispose ก่อน
        ' If WebViewProgressbar IsNot Nothing Then
        ' WebViewProgressbar.Dispose()
        ' End If
        ' Create a new instance of WebView2 control
        WebViewEmergency = New WebView2() With {
        .Dock = DockStyle.Fill
    }
        ' ตั้งค่าตำแหน่งของ PanelWebviewEmergency
        PanelWebviewEmergency.Location = New Point(0, 99)
        ' ตั้งค่าขนาดของ PanelWebviewEmergency
        PanelWebviewEmergency.Size = New Size(800, 501)
        PanelWebviewEmergency.Controls.Add(WebViewEmergency)
        Try
            ' กำหนดไดเรกทอรีสำหรับ environment
            Dim webViewEnvironment = Await CoreWebView2Environment.CreateAsync(Nothing, "C:\Temp")
            ' สร้าง instance ของ WebView2 control
            Await WebViewEmergency.EnsureCoreWebView2Async(webViewEnvironment)
            ' เรียกใช้ URL โดยแสดงค่า line_cd และ shift
            PanelWebviewEmergency.BringToFront()
            WebViewEmergency.CoreWebView2.Navigate("http://" & Backoffice_model.svApi & "/API_NEW_FA/SpecialCode/EMERGENCY")
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/SpecialCode/EMERGENCY")
        Catch ex As Exception
            ' แสดงข้อผิดพลาดในกรณีที่การเริ่มต้นใช้งาน WebView2 ล้มเหลว
            Console.WriteLine($"Failed to initialize WebView2: {ex.Message}")
        End Try
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text)
        lbNextTime.Text = BreakTime
        Dim yearNow As Integer = DateTime.Now.ToString("yyyy")
        Dim monthNow As Integer = DateTime.Now.ToString("MM")
        Dim dayNow As Integer = DateTime.Now.ToString("dd")
        Dim hourNow As Integer = DateTime.Now.ToString("HH")
        Dim minNow As Integer = DateTime.Now.ToString("mm")
        Dim secNow As Integer = DateTime.Now.ToString("ss")
        'MsgBox(yearNow & monthNow & dayNow & hourNow & minNow & secNow)
        Dim yearSt As Integer = st_time.Text.Substring(0, 4)
        Dim monthSt As Integer = st_time.Text.Substring(5, 2)
        Dim daySt As Integer = st_time.Text.Substring(8, 2)
        Dim hourSt As Integer = st_time.Text.Substring(11, 2)
        Dim minSt As Integer = st_time.Text.Substring(14, 2)
        Dim secSt As Integer = st_time.Text.Substring(17, 2)
        'MsgBox(yearSt & minthSt & daySt & hourSt & minSt & secSt)
        Dim firstDate As New System.DateTime(yearSt, monthSt, daySt, hourSt, minSt, secSt)
        Dim secondDate As New System.DateTime(yearNow, monthNow, dayNow, hourNow, minNow, secNow)
        Dim diff As System.TimeSpan = secondDate.Subtract(firstDate)
        Dim diff1 As System.TimeSpan = secondDate - firstDate
        Dim diff2 As String = (secondDate - firstDate).TotalSeconds.ToString()
        'MsgBox(diff2)
        'MsgBox(diff2 / 60)
        Dim actCT As Double = Format(diff2 / 60, "0.00")
        'Format(ListBox2.Items(numOfindex), "0.00")
        'st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim cnt_btn As Integer = Integer.Parse(MainFrm.cavity.Text)
        'Counter
        count = count + cnt_btn
        _Edit_Up_0.Text = count
        Dim Act As Integer = 0
        If Label6.Text <= 0 Then
            Act = 1
        Else
            Act = Label6.Text
        End If
        If comp_flg = 0 Then
            Dim result_mod As Double = Integer.Parse(Act + 1) Mod Integer.Parse(Label27.Text) 'Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            lb_qty_for_box.Text = lb_qty_for_box.Text + cnt_btn
            Dim textp_result As Integer = Label10.Text
            textp_result = Math.Abs(textp_result) - 1
            'MsgBox(textp_result)
            If result_mod = 0 And textp_result <> 0 Then
                If V_check_line_reprint = "0" Then
                    lb_box_count.Text = lb_box_count.Text + 1
                    Label_bach.Text = Label_bach.Text + 1
                    GoodQty = Label6.Text
                    tag_print()
                Else
                    If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                    Else
                        lb_box_count.Text = lb_box_count.Text + 1
                        Label_bach.Text = Label_bach.Text + 1
                        GoodQty = Label6.Text
                        tag_print()
                    End If
                End If
            End If
            Dim sum_act_total As Integer = Label6.Text + cnt_btn
            Label6.Text = sum_act_total
            Dim sum_prg As Integer = (Label6.Text * 100) / Label8.Text
            'MsgBox(sum_prg)
            If sum_prg > 100 Then
                sum_prg = 100
            End If
            CircularProgressBar1.Text = sum_prg & "%"
            CircularProgressBar1.Value = sum_prg
            Dim use_time As Integer = Label34.Text
            'MsgBox(use_time)
            'Dim Starttime As New DateTime(Label16.Text)     ' 10:25:06 AM
            'Dim EndTime As New DateTime(TimeOfDay())     ' 1:25:06 PM
            'DateDiff(DateInterval.Day, st_time.Text, Now)
            'Dim Result As Long = DateDiff(DateInterval.Day, st_time.Text, Now)
            'MsgBox(st_time.Text)
            Dim dt1 As DateTime = DateTime.Now
            Dim dt2 As DateTime = st_count_ct.Text
            Dim dtspan As TimeSpan = dt1 - dt2
            'MsgBox(("Second: " & dtspan.Seconds))
            'use_time = 1
            'MsgBox(dtspan)
            'MsgBox(dtspan.Minutes)
            Dim actCT_jing As Double = Format((dtspan.Seconds / _Edit_Up_0.Text) + (dtspan.Minutes * 60), "0.00")
            'Label37.Text = actCT_jing
            ListBox1.Items.Add((dtspan.Seconds) + (dtspan.Minutes * 60) + (dtspan.Hours * 3600))
            Dim Total As Double = 0
            Dim Count As Double = 0
            Dim Average As Double = 0
            Dim I As Integer
            For I = 0 To ListBox1.Items.Count - 1
                Total += Val(ListBox1.Items(I))
                Count += 1
            Next
            Average = Total / Count
            'MsgBox(Count)
            If Count = 1 Then
                Try
                    Backoffice_model.Tag_seq_rec_sqlite(lb_ref_scan.Text.Substring(0, 10), lb_ref_scan.Text.Substring(10, 3), lb_ref_scan.Text.Substring(13, (lb_ref_scan.Text.Length - 13)), RTrim(lb_ref_scan.Text))
                Catch ex As Exception

                End Try
            End If
            Label37.Text = Format(Average, "0.0")

            Dim start_time As Date = st_count_ct.Text

            st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")


            Dim dt11 As DateTime = DateTime.Now
            Dim dt22 As DateTime = st_time.Text
            Dim dtspan1 As TimeSpan = dt11 - dt22

            'MsgBox(dtspan1.Minutes)

            If (dtspan1.Minutes + (dtspan1.Hours * 60)) >= use_time Then
                Label20.ForeColor = Color.Red
            End If
            Dim temppola As Double = ((dtspan1.Seconds / 60) + (dtspan1.Minutes + (dtspan1.Hours * 60)))
            'MsgBox("Minute diff : " & dtspan1.Minutes)
            'MsgBox("Hour diff : " & (dtspan1.Hours * 60))
            If temppola < 1 Then
                temppola = 1
            End If

            Dim loss_sum As Integer
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    Dim LoadSQL = Backoffice_model.get_sum_loss(Trim(wi_no.Text))
                    While LoadSQL.Read()
                        loss_sum = LoadSQL("sum_loss")
                    End While
                Else
                    loss_sum = 0
                End If
            Catch ex As Exception
                load_show.Show()
                'loss_sum = 0
            End Try
            Dim sum_prg2 As Integer = (((Label38.Text * _Edit_Up_0.Text) / ((temppola * 60) - loss_sum)) * 100)
            'Dim sum_prg2 As Integer = (((CycleTime.Text * _Edit_Up_0.Text) / temppola) * 100)
            'MsgBox("((" & CycleTime.Text & "*" & _Edit_Up_0.Text & ") /" & temppola & ") * 100")
            'MsgBox(sum_prg2 / cnt_btn)
            sum_prg2 = sum_prg2 / cnt_btn
            'MsgBox(sum_prg2)
            If sum_prg2 > 100 Then
                sum_prg2 = 100
            End If
            'MsgBox(sum_prg2)
            If sum_prg2 <= 49 Then
                CircularProgressBar2.ProgressColor = Color.Red
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 50 And sum_prg2 <= 79 Then
                CircularProgressBar2.ProgressColor = Color.Chocolate
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 80 And sum_prg2 <= 100 Then
                CircularProgressBar2.ProgressColor = Color.Green
                CircularProgressBar2.ForeColor = Color.Black
            End If
            Dim avarage_eff As Double = Format(sum_prg2, "0.00")
            lb_sum_prg.Text = avarage_eff
            CircularProgressBar2.Text = sum_prg2 & "%"
            CircularProgressBar2.Value = sum_prg2
            Dim actCT_jingna As Double = Format(dtspan.Seconds + (dtspan.Minutes * 60), "0.00")
            Dim pd As String = MainFrm.Label6.Text
            Dim line_cd As String = MainFrm.Label4.Text
            Dim wi_plan As String = wi_no.Text
            Dim item_cd As String = Label3.Text
            Dim item_name As String = Label12.Text
            Dim staff_no As String = Label29.Text
            Dim seq_no As String = Label22.Text
            Dim prd_qty As Integer = cnt_btn
            Dim end_time As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            Dim use_timee As Double = actCT_jingna
            Dim tr_status As String = "0"
            Dim number_qty As Integer = Label6.Text
            'Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_timee, tr_status)
            Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_time, number_qty, Label18.Text, tr_status)
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, number_qty, start_time2, end_time, use_timee, number_qty, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                Else
                    tr_status = "0"
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, number_qty, start_time2, end_time, use_timee, number_qty, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End If
            Catch ex As Exception
                tr_status = "0"
                Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, number_qty, start_time2, end_time, use_timee, number_qty, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
            End Try
            Dim sum_diff As Integer = Label8.Text - Label6.Text
            If sum_diff < 0 Then
                Label10.Text = "0"
            Else
                Label10.Text = "-" & sum_diff
            End If
            If sum_diff < 1 Then
                If sum_diff = 0 Then
                    lb_box_count.Text = lb_box_count.Text + 1
                    'If Backoffice_model.check_line_reprint() = "0" Then
                    Label_bach.Text = Label_bach.Text + 1
                    GoodQty = Label6.Text
                    tag_print()
                    'End If
                Else

                End If
                Me.Enabled = False
                comp_flg = 1

                Finish_work.Show()
                Dim plan_qty As Integer = Label8.Text
                Dim act_qty As Integer = _Edit_Up_0.Text
                Dim shift_prd As String = Label14.Text
                Dim prd_st_datetime As Date = st_time.Text
                Dim prd_end_datetime As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim lot_no As String = Label18.Text
                Dim comp_flg2 As String = "1"
                Dim transfer_flg As String = "1"
                Dim del_flg As String = "0"
                Dim prd_flg As String = "1"
                Dim close_lot_flg As String = "1"
                Dim avarage_act_prd_time As Double = Average
                Try
                    act_qty = LB_COUNTER_SEQ.Text 'Working_Pro._Edit_Up_0.Text
                    LB_COUNTER_SHIP.Text = 0
                    LB_COUNTER_SEQ.Text = 0
                Catch ex As Exception
                    act_qty = 0
                End Try

                Try
                    If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                        transfer_flg = "0"

                        Backoffice_model.Insert_prd_close_lot(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                        Backoffice_model.Insert_prd_close_lot_sqlite(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                        'Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_timee, number_qty)
                        Backoffice_model.work_complete(wi_plan)
                        Dim temp_co_emp As Integer = List_Emp.ListView1.Items.Count
                        Dim emp_cd As String
                        For I = 0 To temp_co_emp - 1
                            emp_cd = List_Emp.ListView1.Items(I).Text
                            Backoffice_model.Insert_emp_cd(wi_plan, emp_cd, seq_no)
                            'MsgBox(List_Emp.ListView1.Items(i).Text)
                        Next
                        'MsgBox("Ins completed")
                    Else
                        transfer_flg = "0"
                        Backoffice_model.Insert_prd_close_lot_sqlite(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                        'Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time, end_time, use_timee, tr_status)
                        'MsgBox("Ins incompleted1")
                    End If
                Catch ex As Exception
                    'MsgBox("Ins incompleted2")
                    transfer_flg = "0"
                    Backoffice_model.Insert_prd_close_lot_sqlite(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                    'Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time, end_time, use_timee, tr_status)
                End Try
            End If
        End If
    End Sub

    Private Sub Label18_Click(sender As Object, e As EventArgs) Handles Label18.Click

    End Sub

    Private Sub Label6_Paint(sender As Object, e As PaintEventArgs) Handles Label6.Paint

    End Sub
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Dim i As Short
        Dim BitNo As Short
        Dim Kind As Short
        Dim Tim As Integer


        For i = 0 To 1
            UpCount(i) = 0
            DownCount(i) = 0
        Next i

        Tim = 100
        Kind = DIO_TRG_RISE
        For BitNo = 0 To 1

            If Check(BitNo).CheckState = 1 Then
                Ret = DioNotifyTrg(Id, BitNo, Kind, Tim, Me.Handle.ToInt32)
                If (Ret <> DIO_ERR_SUCCESS) Then
                    'MsgBox("Connect success")
                    Exit For
                End If

            Else
                Ret = DioStopNotifyTrg(Id, BitNo)
                If (Ret <> DIO_ERR_SUCCESS) Then
                    'MsgBox("Connect Failed")
                    Exit For
                End If
            End If
        Next BitNo

        Call DioGetErrorString(Ret, szError)
    End Sub

    'Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    '    Dim szDeviceName As String
    '    szDeviceName = Edit_DeviceName.Text
    '    Ret = DioInit(szDeviceName, Id)
    '    Call DioGetErrorString(Ret, szError)
    'End Sub

    Function HiLoWord(ByRef Number As Integer, ByRef HiLo As Short) As Integer

        Dim Hi As Integer
        Dim Lo As Integer
        Dim StrData As String
        Dim i As Short

        StrData = Hex(Number)
        If Len(StrData) < 2 Then
            For i = 1 To 2 - Len(StrData)
                StrData = "0" & StrData
            Next i
        End If

        Lo = Val("&h" & VB.Right(StrData, 4))
        'Hi = Val("&h" & VB.Left(StrData, 4))
        If HiLo = 0 Then
            HiLoWord = Lo
        ElseIf HiLo = 1 Then
            HiLoWord = Hi
        End If

    End Function
    Public count As String = 0
    Public Async Function counter_contect_DIO() As Task(Of Integer)
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.updated_data_to_dbsvr()
            End If
        Catch ex As Exception
        End Try
        Dim resultSumDiff As Integer = CDbl(Val(Label8.Text)) - (CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text))
        If resultSumDiff < 0 Then
            MsgBox("Please Check Cavity Over Plan contect.")
            Return 0
        End If
        Dim yearNow As Integer = DateTime.Now.ToString("yyyy")
        Dim monthNow As Integer = DateTime.Now.ToString("MM")
        Dim dayNow As Integer = DateTime.Now.ToString("dd")
        Dim hourNow As Integer = DateTime.Now.ToString("HH")
        Dim minNow As Integer = DateTime.Now.ToString("mm")
        Dim secNow As Integer = DateTime.Now.ToString("ss")
        'MsgBox(yearNow & monthNow & dayNow & hourNow & minNow & secNow)
        Dim yearSt As Integer = st_time.Text.Substring(0, 4)
        Dim monthSt As Integer = st_time.Text.Substring(5, 2)
        Dim daySt As Integer = st_time.Text.Substring(8, 2)
        Dim hourSt As Integer = st_time.Text.Substring(11, 2)
        Dim minSt As Integer = st_time.Text.Substring(14, 2)
        Dim secSt As Integer = st_time.Text.Substring(17, 2)
        'MsgBox(yearSt & minthSt & daySt & hourSt & minSt & secSt)
        Dim firstDate As New System.DateTime(yearSt, monthSt, daySt, hourSt, minSt, secSt)
        Dim secondDate As New System.DateTime(yearNow, monthNow, dayNow, hourNow, minNow, secNow)
        Dim diff As System.TimeSpan = secondDate.Subtract(firstDate)
        Dim diff1 As System.TimeSpan = secondDate - firstDate
        Dim diff2 As String = (secondDate - firstDate).TotalSeconds.ToString()
        'MsgBox(diff2)
        'MsgBox(diff2 / 60)
        Dim actCT As Double = Format(diff2 / 60, "0.00")
        'Format(ListBox2.Items(numOfindex), "0.00")
        'st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        'Counter
        Dim cnt_btn As Integer = Integer.Parse(MainFrm.cavity.Text)
        'Counter
        count = count + cnt_btn
        _Edit_Up_0.Text = count
        Dim Act As Integer = 0
        Dim action_plus As Integer = 0


        If Label6.Text <= 0 Then ' condition  
            Act = 1
            action_plus = 0
        Else
            Act = lb_good.Text 'Label6.Text
            action_plus = 1
        End If
        Console.WriteLine("TESTTTTTTT INNNNN")
        If comp_flg = 0 Then
            statusPrint = "Normal_contect_DIO"
            'MsgBox("G")
            Console.WriteLine("delays")
            'Dim result_mod As Double = Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            Dim result_mod As Double = Integer.Parse(Act + action_plus) Mod Integer.Parse(Label27.Text) 'Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            lb_qty_for_box.Text = lb_qty_for_box.Text + cnt_btn
            Dim textp_result As Integer = Label10.Text
            textp_result = Math.Abs(textp_result) - 1
            'MsgBox(textp_result)
            Dim sum_act_total As Integer = Label6.Text + cnt_btn
            Dim start_time As Date = st_count_ct.Text
            Dim end_time As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim checkTransection As String
            'Try
            'If My.Computer.Network.Ping("192.168.161.101") Then
            'checkTransection = Backoffice_model.checkTransection(pwi_id, CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text), start_time2)
            ' MsgBox("pwi_id==>" & pwi_id)
            ' MsgBox("Label6==>" & CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text))
            ' MsgBox("start_time2==>" & start_time2)
            'Else
            '    checkTransection = "1"
            'End If
            '    Catch ex As Exception
            '    checkTransection = "1"
            '    End Try
            ' If checkTransection = "1" Then
            Label6.Text = sum_act_total
            LB_COUNTER_SHIP.Text += cnt_btn
            LB_COUNTER_SEQ.Text += cnt_btn
            lb_good.Text += cnt_btn
            QtyMold += cnt_btn
            If check_tag_type = "3" Then
                ' for line break
                Dim break = lbPosition1.Text & " " & lbPosition2.Text
                Dim plb = New PrintLabelBreak
                plb.loadData(Label3.Text, break, Label18.Text, Label22.Text, CDbl(Val(LB_COUNTER_SEQ.Text)))
            End If
            If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                lb_box_count.Text = lb_box_count.Text + 1
                print_back.PrintDocument2.Print()
                If result_mod = "0" Then
                    Label_bach.Text += 1
                    GoodQty = Label6.Text
                    'MsgBox("IF ===>" & GoodQty)
                    tag_print()
                End If
            Else
                If result_mod = 0 And textp_result <> 0 Then
                    If V_check_line_reprint = "0" Then
                        lb_box_count.Text = lb_box_count.Text + 1
                        Label_bach.Text = Label_bach.Text + 1
                        'GoodQty = Label6.Text
                        GoodQty = lb_good.Text
                        'MsgBox("IF IF ===>" & GoodQty)
                        tag_print()
                    Else
                        If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                        Else
                            lb_box_count.Text = lb_box_count.Text + 1
                            Label_bach.Text = Label_bach.Text + 1
                            GoodQty = Label6.Text
                            'MsgBox("ELSE ===>" & GoodQty)
                            tag_print()
                        End If
                    End If
                End If
            End If
            Dim sum_prg As Integer = (Label6.Text * 100) / Label8.Text
            'MsgBox(sum_prg)
            If sum_prg > 100 Then
                sum_prg = 100
            ElseIf sum_prg < 0 Then
                sum_prg = 0
            End If
            CircularProgressBar1.Text = sum_prg & "%"
            CircularProgressBar1.Value = sum_prg
            Dim use_time As Integer = Label34.Text
            Dim dt1 As DateTime = DateTime.Now
            Dim dt2 As DateTime = st_count_ct.Text
            Dim dtspan As TimeSpan = dt1 - dt2
            'MsgBox(("Second: " & dtspan.Seconds))
            'use_time = 1
            'MsgBox(dtspan)
            'MsgBox(dtspan.Minutes)

            Dim actCT_jing As Double = Format((dtspan.Seconds / _Edit_Up_0.Text) + (dtspan.Minutes * 60), "0.00")
            'Label37.Text = actCT_jing

            ListBox1.Items.Add((dtspan.Seconds) + (dtspan.Minutes * 60) + (dtspan.Hours * 3600))

            Dim Total As Double = 0
            Dim Count As Double = 0
            Dim Average As Double = 0
            Dim I As Integer

            For I = 0 To ListBox1.Items.Count - 1
                Total += Val(ListBox1.Items(I))
                Count += 1
            Next
            Average = Total / Count
            'MsgBox(Count)
            If Count = 1 Then
                'MsgBox(lb_ref_scan.Text)
                Try
                    Backoffice_model.Tag_seq_rec_sqlite(lb_ref_scan.Text.Substring(0, 10), lb_ref_scan.Text.Substring(10, 3), lb_ref_scan.Text.Substring(13, (lb_ref_scan.Text.Length - 13)), RTrim(lb_ref_scan.Text))
                Catch ex As Exception

                End Try
            End If
            Label37.Text = Format(Average, "0.0")
            st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            Dim dt11 As DateTime = DateTime.Now
            Dim dt22 As DateTime = st_time.Text
            Dim dtspan1 As TimeSpan = dt11 - dt22
            'MsgBox(dtspan1.Minutes)
            If (dtspan1.Minutes + (dtspan1.Hours * 60)) >= use_time Then
                Label20.ForeColor = Color.Red
            End If

            Dim temppola As Double = ((dtspan1.Seconds / 60) + (dtspan1.Minutes + (dtspan1.Hours * 60)))
            'MsgBox("Minute diff : " & dtspan1.Minutes)
            'MsgBox("Hour diff : " & (dtspan1.Hours * 60))
            If temppola < 1 Then
                temppola = 1
            End If
            Dim loss_sum As Integer
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    Dim LoadSQL = Backoffice_model.get_sum_loss(Trim(wi_no.Text))
                    While LoadSQL.Read()
                        loss_sum = LoadSQL("sum_loss")
                    End While
                Else
                    loss_sum = 0
                End If
            Catch ex As Exception
                'load_show.Show()
                'loss_sum = 0
            End Try
            Dim sum_prg2 As Integer = (((Label38.Text * _Edit_Up_0.Text) / ((temppola * 60) - loss_sum)) * 100)

            sum_prg2 = sum_prg2 / cnt_btn

            If sum_prg2 > 100 Then
                sum_prg2 = 100
            ElseIf sum_prg2 < 0 Then
                sum_prg2 = 0
            End If
            If sum_prg2 <= 49 Then
                CircularProgressBar2.ProgressColor = Color.Red
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 50 And sum_prg2 <= 79 Then
                CircularProgressBar2.ProgressColor = Color.Chocolate
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 80 And sum_prg2 <= 100 Then
                CircularProgressBar2.ProgressColor = Color.Green
                CircularProgressBar2.ForeColor = Color.Black
            End If
            Dim avarage_eff As Double = Format(sum_prg2, "0.00")
            lb_sum_prg.Text = avarage_eff
            CircularProgressBar2.Text = sum_prg2 & "%"
            CircularProgressBar2.Value = sum_prg2
            Dim actCT_jingna As Double = Format(dtspan.Seconds + (dtspan.Minutes * 60), "0.00")
            Dim pd As String = MainFrm.Label6.Text
            Dim line_cd As String = MainFrm.Label4.Text
            Dim wi_plan As String = wi_no.Text
            Dim item_cd As String = Label3.Text
            Dim item_name As String = Label12.Text
            Dim staff_no As String = Label29.Text
            Dim seq_no As String = Label22.Text
            Dim prd_qty As Integer = cnt_btn
            Dim use_timee As Double = actCT_jingna
            Dim tr_status As String = "0"
            Dim number_qty As Integer = Label6.Text
            Dim result_use_time As Double = Cal_Use_Time_ins_qty_fn_manual(start_time2, end_time2)
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            Backoffice_model.Insert_prd_detail(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, start_time, end_time, result_use_time, number_qty, Spwi_id(j), tr_status)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, result_use_time, number_qty, pwi_id, tr_status)
                    End If
                Else
                    tr_status = "0"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                End If
            Catch ex As Exception
                tr_status = "0"
                If MainFrm.chk_spec_line = "2" Then
                    Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                    Dim Iseq = GenSEQ
                    Dim j As Integer = 0
                    For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                        Iseq += 1
                        Dim special_wi As String = itemPlanData.wi
                        Dim special_item_cd As String = itemPlanData.item_cd
                        Dim special_item_name As String = itemPlanData.item_name
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        j = j + 1
                    Next
                Else
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End If
            End Try
            Dim sum_diff As Integer = Label8.Text - Label6.Text
            Label10.Text = "-" & sum_diff
            If sum_diff = 0 Then
                Me.Enabled = False
                comp_flg = 1
                If Finish_work.Visible = False Then
                    Finish_work.Show() ' เกี่ยว
                End If
            End If
            If sum_diff < 1 Then
                If sum_diff = 0 Then
                    If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                        lb_box_count.Text = lb_box_count.Text + 1
                        print_back.PrintDocument2.Print()
                        If result_mod = "0" Then
                            Label_bach.Text += 1
                            GoodQty = Label6.Text
                            'MsgBox("IF ===>" & GoodQty)
                            tag_print()
                        End If
                    Else
                        lb_box_count.Text = lb_box_count.Text + 1
                        Label_bach.Text = Label_bach.Text + 1
                        GoodQty = Label6.Text
                        'MsgBox("ELSE ===>" & GoodQty)
                        tag_print()
                    End If
                Else
                End If
                Me.Enabled = False
                comp_flg = 1
                If Finish_work.Visible = False Then
                    Finish_work.Show() ' เกี่ยว
                End If
                Dim plan_qty As Integer = Label8.Text
                Dim shift_prd As String = Label14.Text
                Dim prd_st_datetime As Date = st_time.Text
                Dim prd_end_datetime As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim lot_no As String = Label18.Text
                Dim comp_flg2 As String = "1"
                Dim transfer_flg As String = "1"
                Dim del_flg As String = "0"
                Dim prd_flg As String = "1"
                Dim close_lot_flg As String = "1"
                Dim avarage_act_prd_time As Double = Average
            End If
            ' End If
        End If
        '--------------------------------------
        ' Expression
        '--------------------------------------
        'Edit_Up(BitNo).Text = CStr(UpCount(BitNo))
        'Edit_Down(BitNo).Text = CStr(DownCount(BitNo))
    End Function
    Public Async Function counter_contect_DIO_RS232() As Task(Of Integer)
        Console.WriteLine("READY NI MAX")
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.updated_data_to_dbsvr()
            End If
        Catch ex As Exception
        End Try
        Dim resultSumDiff As Integer = CDbl(Val(Label8.Text)) - (CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text))
        If resultSumDiff < 0 Then
            MsgBox("Please Check Cavity Over Plan RS232.")
            Return 0
        End If
        Dim yearNow As Integer = DateTime.Now.ToString("yyyy")
        Dim monthNow As Integer = DateTime.Now.ToString("MM")
        Dim dayNow As Integer = DateTime.Now.ToString("dd")
        Dim hourNow As Integer = DateTime.Now.ToString("HH")
        Dim minNow As Integer = DateTime.Now.ToString("mm")
        Dim secNow As Integer = DateTime.Now.ToString("ss")
        'MsgBox(yearNow & monthNow & dayNow & hourNow & minNow & secNow)
        Dim yearSt As Integer = st_time.Text.Substring(0, 4)
        Dim monthSt As Integer = st_time.Text.Substring(5, 2)
        Dim daySt As Integer = st_time.Text.Substring(8, 2)
        Dim hourSt As Integer = st_time.Text.Substring(11, 2)
        Dim minSt As Integer = st_time.Text.Substring(14, 2)
        Dim secSt As Integer = st_time.Text.Substring(17, 2)
        'MsgBox(yearSt & minthSt & daySt & hourSt & minSt & secSt)
        Dim firstDate As New System.DateTime(yearSt, monthSt, daySt, hourSt, minSt, secSt)
        Dim secondDate As New System.DateTime(yearNow, monthNow, dayNow, hourNow, minNow, secNow)
        Dim diff As System.TimeSpan = secondDate.Subtract(firstDate)
        Dim diff1 As System.TimeSpan = secondDate - firstDate
        Dim diff2 As String = (secondDate - firstDate).TotalSeconds.ToString()
        'MsgBox(diff2)
        'MsgBox(diff2 / 60)
        Dim actCT As Double = Format(diff2 / 60, "0.00")
        'Format(ListBox2.Items(numOfindex), "0.00")
        'st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        'Counter
        Dim cnt_btn As Integer = Integer.Parse(MainFrm.cavity.Text)
        'Counter
        count = count + cnt_btn
        _Edit_Up_0.Text = count
        Dim Act As Integer = 0
        Dim action_plus As Integer = 0
        If Label6.Text <= 0 Then ' condition  
            Act = 1
            action_plus = 0
        Else
            Act = lb_good.Text 'Label6.Text
            action_plus = 1
        End If
        Dim start_time As Date = st_count_ct.Text
        Dim end_time As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        Dim checkTransection As String
        'Try
        'If My.Computer.Network.Ping("192.168.161.101") Then
        'checkTransection = Backoffice_model.checkTransection(pwi_id, CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text), start_time2)
        'Else
        'checkTransection = "1"
        'End If
        'Catch ex As Exception
        'checkTransection = "1"
        'End Try
        ' If checkTransection = "1" Then

        If comp_flg = 0 Then
            statusPrint = "Normal_DIO_RS232"
            'Dim result_mod As Double = Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            Dim result_mod As Double = Integer.Parse(Act + action_plus) Mod Integer.Parse(Label27.Text) 'Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            lb_qty_for_box.Text = lb_qty_for_box.Text + cnt_btn
            Dim textp_result As Integer = Label10.Text
            textp_result = Math.Abs(textp_result) - 1
            'MsgBox(textp_result)
            Dim sum_act_total As Integer = Label6.Text + cnt_btn
            Label6.Text = sum_act_total
            LB_COUNTER_SHIP.Text += cnt_btn
            LB_COUNTER_SEQ.Text += cnt_btn
            QtyMold += cnt_btn
            lb_good.Text += cnt_btn
            If check_tag_type = "3" Then
                Dim break = lbPosition1.Text & " " & lbPosition2.Text
                Dim plb = New PrintLabelBreak
                plb.loadData(Label3.Text, break, Label18.Text, Label22.Text, CDbl(Val(LB_COUNTER_SEQ.Text)))
            End If
            If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                lb_box_count.Text = lb_box_count.Text + 1
                print_back.PrintDocument2.Print()
                If result_mod = "0" Then
                    Label_bach.Text += 1
                    GoodQty = Label6.Text
                    tag_print()
                End If
            Else
                If result_mod = 0 And textp_result <> 0 Then
                    If V_check_line_reprint = "0" Then
                        lb_box_count.Text = lb_box_count.Text + 1
                        Label_bach.Text = Label_bach.Text + 1
                        'GoodQty = Label6.Text
                        GoodQty = lb_good.Text
                        tag_print()
                    Else
                        If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                        Else
                            lb_box_count.Text = lb_box_count.Text + 1
                            Label_bach.Text = Label_bach.Text + 1
                            GoodQty = Label6.Text
                            tag_print()
                        End If
                    End If
                End If
            End If


            Dim sum_prg As Integer = (Label6.Text * 100) / Label8.Text
            'MsgBox(sum_prg)

            If sum_prg > 100 Then
                sum_prg = 100
            ElseIf sum_prg < 0 Then
                sum_prg = 0
            End If

            CircularProgressBar1.Text = sum_prg & "%"
            CircularProgressBar1.Value = sum_prg


            Dim use_time As Integer = Label34.Text


            Dim dt1 As DateTime = DateTime.Now
            Dim dt2 As DateTime = st_count_ct.Text
            Dim dtspan As TimeSpan = dt1 - dt2
            Dim actCT_jing As Double = Format((dtspan.Seconds / _Edit_Up_0.Text) + (dtspan.Minutes * 60), "0.00")
            ListBox1.Items.Add((dtspan.Seconds) + (dtspan.Minutes * 60) + (dtspan.Hours * 3600))
            Dim Total As Double = 0
            Dim Count As Double = 0
            Dim Average As Double = 0
            Dim I As Integer
            For I = 0 To ListBox1.Items.Count - 1
                Total += Val(ListBox1.Items(I))
                Count += 1
            Next
            Average = Total / Count
            If Count = 1 Then
                Try
                    Backoffice_model.Tag_seq_rec_sqlite(lb_ref_scan.Text.Substring(0, 10), lb_ref_scan.Text.Substring(10, 3), lb_ref_scan.Text.Substring(13, (lb_ref_scan.Text.Length - 13)), RTrim(lb_ref_scan.Text))
                Catch ex As Exception

                End Try
            End If
            Label37.Text = Format(Average, "0.0")

            st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")

            Dim dt11 As DateTime = DateTime.Now
            Dim dt22 As DateTime = st_time.Text
            Dim dtspan1 As TimeSpan = dt11 - dt22

            'MsgBox(dtspan1.Minutes)

            If (dtspan1.Minutes + (dtspan1.Hours * 60)) >= use_time Then
                Label20.ForeColor = Color.Red
            End If

            Dim temppola As Double = ((dtspan1.Seconds / 60) + (dtspan1.Minutes + (dtspan1.Hours * 60)))
            If temppola < 1 Then
                temppola = 1
            End If
            Dim loss_sum As Integer
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    Dim LoadSQL = Backoffice_model.get_sum_loss(Trim(wi_no.Text))
                    While LoadSQL.Read()
                        loss_sum = LoadSQL("sum_loss")
                    End While
                Else
                    loss_sum = 0
                End If
            Catch ex As Exception
                'load_show.Show()
                'loss_sum = 0
            End Try
            Dim sum_prg2 As Integer = (((Label38.Text * _Edit_Up_0.Text) / ((temppola * 60) - loss_sum)) * 100)
            'Dim sum_prg2 As Integer = (((CycleTime.Text * _Edit_Up_0.Text) / temppola) * 100)
            'MsgBox("((" & CycleTime.Text & "*" & _Edit_Up_0.Text & ") /" & temppola & ") * 100")
            'MsgBox(sum_prg2 / cnt_btn)

            sum_prg2 = sum_prg2 / cnt_btn
            'MsgBox(sum_prg2)

            If sum_prg2 > 100 Then
                sum_prg2 = 100
            ElseIf sum_prg2 < 0 Then
                sum_prg2 = 0
            End If
            'MsgBox(sum_prg2)
            If sum_prg2 <= 49 Then
                CircularProgressBar2.ProgressColor = Color.Red
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 50 And sum_prg2 <= 79 Then
                CircularProgressBar2.ProgressColor = Color.Chocolate
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 80 And sum_prg2 <= 100 Then
                CircularProgressBar2.ProgressColor = Color.Green
                CircularProgressBar2.ForeColor = Color.Black
            End If
            Dim avarage_eff As Double = Format(sum_prg2, "0.00")
            lb_sum_prg.Text = avarage_eff
            CircularProgressBar2.Text = sum_prg2 & "%"
            CircularProgressBar2.Value = sum_prg2
            Dim actCT_jingna As Double = Format(dtspan.Seconds + (dtspan.Minutes * 60), "0.00")
            Dim pd As String = MainFrm.Label6.Text
            Dim line_cd As String = MainFrm.Label4.Text
            Dim wi_plan As String = wi_no.Text
            Dim item_cd As String = Label3.Text
            Dim item_name As String = Label12.Text
            Dim staff_no As String = Label29.Text
            Dim seq_no As String = Label22.Text
            Dim prd_qty As Integer = cnt_btn
            Dim use_timee As Double = actCT_jingna
            Dim tr_status As String = "0"
            Dim number_qty As Integer = Label6.Text
            'Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_timee, tr_status)
            Dim result_use_time As Double = Cal_Use_Time_ins_qty_fn_manual(start_time2, end_time2)
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim indRow As String = itemPlanData.IND_ROW
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            Backoffice_model.Insert_prd_detail(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, start_time, end_time, result_use_time, number_qty, Spwi_id(j), tr_status)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, result_use_time, number_qty, pwi_id, tr_status)
                    End If
                Else
                    tr_status = "0"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim indRow As String = itemPlanData.IND_ROW
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                End If
            Catch ex As Exception
                tr_status = "0"
                If MainFrm.chk_spec_line = "2" Then
                    Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                    Dim Iseq = GenSEQ
                    Dim j As Integer = 0
                    For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                        Iseq += 1
                        Dim indRow As String = itemPlanData.IND_ROW
                        Dim special_wi As String = itemPlanData.wi
                        Dim special_item_cd As String = itemPlanData.item_cd
                        Dim special_item_name As String = itemPlanData.item_name
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        j = j + 1
                    Next
                Else
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End If
            End Try
            Dim sum_diff As Integer = Label8.Text - Label6.Text
            Label10.Text = "-" & sum_diff
            If sum_diff = 0 Then
                Me.Enabled = False
                comp_flg = 1
                'If result_mod <> "0" Then ' New
                'tag_print()
                'End If
                If Finish_work.Visible = False Then
                    Finish_work.Show() ' เกี่ยว
                End If
            End If
            'MsgBox(sum_diff)
            If sum_diff < 1 Then
                If sum_diff = 0 Then
                    If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                        lb_box_count.Text = lb_box_count.Text + 1
                        print_back.PrintDocument2.Print()
                        If result_mod = "0" Then
                            Label_bach.Text += 1
                            GoodQty = Label6.Text
                            tag_print()
                        End If
                    Else
                        lb_box_count.Text = lb_box_count.Text + 1
                        'If Backoffice_model.check_line_reprint() = "0" Then
                        Label_bach.Text = Label_bach.Text + 1
                        GoodQty = Label6.Text
                        tag_print()
                        'End If
                    End If
                Else
                End If
                Me.Enabled = False
                comp_flg = 1
                If Finish_work.Visible = False Then
                    Finish_work.Show() ' เกี่ยว
                End If
                Dim plan_qty As Integer = Label8.Text
                Dim shift_prd As String = Label14.Text
                Dim prd_st_datetime As Date = st_time.Text
                Dim prd_end_datetime As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim lot_no As String = Label18.Text
                Dim comp_flg2 As String = "1"
                Dim transfer_flg As String = "1"
                Dim del_flg As String = "0"
                Dim prd_flg As String = "1"
                Dim close_lot_flg As String = "1"
                Dim avarage_act_prd_time As Double = Average
            End If
        End If

    End Function
    Private Async Function Manage_counter_contect_DIO() As Task
        check_bull = 1
        Console.WriteLine("ssss IN FUNCTION")
        ' เรียกใช้งานฟังก์ชัน counter_contect_DIO
        Await counter_contect_DIO()
        ' คำนวณประสิทธิภาพ
        cal_eff()
        Console.WriteLine("OUT FUNCTION")
        Dim delay_setting As Integer = 0
        Console.WriteLine("G1")
        Console.WriteLine("status_conter====>" & status_conter)
        ' กำหนดค่า delay_setting ตามสถานะของ status_conter
        If status_conter = "0" Then
            delay_setting = s_delay * 100
            Console.WriteLine("AF1")
        Else
            delay_setting = s_delay * 1000
            Console.WriteLine("AF2")
        End If
        Console.WriteLine("DELAY SETTTTTING=====>" & delay_setting)
        ' สร้าง CancellationTokenSource
        Dim cts As New CancellationTokenSource()
        Try
            ' รอหน่วงเวลาแบบ Asynchronous พร้อมกับ Token สำหรับการยกเลิก
            Await Task.Delay(delay_setting, cts.Token).ContinueWith(Sub(task)
                                                                        ' ตรวจสอบว่าถ้า Task ไม่ถูกยกเลิก
                                                                        If Not task.IsCanceled Then
                                                                            ' ตรวจสอบว่า Handle ของฟอร์มยังถูกสร้างอยู่และไม่ถูก Dispose
                                                                            If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                                                                                ' เรียกใช้ Me.Invoke เพื่ออัพเดท UI อย่างปลอดภัยบน UI Thread
                                                                                Me.Invoke(Sub()
                                                                                              check_bull = 0
                                                                                              Console.WriteLine("Asynchronous operation completed.")
                                                                                          End Sub)
                                                                            End If
                                                                        End If
                                                                    End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        Catch ex As Exception
            ' จัดการข้อผิดพลาดและแสดงข้อความใน Console
            Console.WriteLine("err===>" & ex.Message)
        End Try
    End Function

    Private Async Function Manage_counter_contect_DIO_RS232() As Task
        ' เริ่มต้นการทำงานแบบ Asynchronous
        check_bull = 1
        Console.WriteLine("IN FUNCTION")
        ' เรียกใช้งานฟังก์ชัน counter_contect_DIO_RS232
        Await counter_contect_DIO_RS232()
        ' คำนวณประสิทธิภาพ
        cal_eff()
        Console.WriteLine("OUT FUNCTION")

        Dim delay_setting As Integer = 0
        Console.WriteLine("G1")
        Console.WriteLine("status_conter====>" & status_conter)
        ' กำหนดค่า delay_setting ตามสถานะของ status_conter
        If status_conter = "0" Then
            delay_setting = s_delay * 100
            Console.WriteLine("AF1")
        Else
            delay_setting = s_delay * 1000
            Console.WriteLine("AF2")
        End If

        Console.WriteLine("DELAY SETTTTTING=====>" & delay_setting)

        ' สร้าง CancellationTokenSource
        Dim cts As New CancellationTokenSource()

        Try
            ' รอหน่วงเวลาแบบ Asynchronous พร้อมกับ Token สำหรับการยกเลิก
            Await Task.Delay(delay_setting, cts.Token).ContinueWith(Sub(task)
                                                                        ' ตรวจสอบว่าถ้า Task ไม่ถูกยกเลิก
                                                                        If Not task.IsCanceled Then
                                                                            ' ตรวจสอบว่า Handle ของฟอร์มยังถูกสร้างอยู่และไม่ถูก Dispose
                                                                            If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                                                                                Try
                                                                                    ' เรียกใช้ Me.Invoke เพื่ออัพเดท UI อย่างปลอดภัยบน UI Thread
                                                                                    Me.Invoke(Sub()
                                                                                                  check_bull = 0
                                                                                                  Console.WriteLine("Asynchronous operation completed.")
                                                                                              End Sub)
                                                                                Catch ex As Exception
                                                                                    ' จัดการข้อผิดพลาดภายใน Invoke
                                                                                    check_bull = 0
                                                                                End Try
                                                                            End If
                                                                        End If
                                                                    End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        Catch ex As Exception
            ' จัดการข้อผิดพลาดและแสดงข้อความใน Console
            Console.WriteLine("err===>" & ex.Message)
        End Try
    End Function

    Private Async Function Manage_counter_NI_MAX() As Task
        Try
            ' เริ่มต้นการทำงานแบบ Asynchronous
            check_bull = 1
            ' เรียกใช้งานฟังก์ชัน counter_data_new_dio
            Await counter_data_new_dio()
            ' คำนวณประสิทธิภาพ
            cal_eff()
            Dim delay_setting As Integer = 0
            If status_conter = "0" Then
                delay_setting = s_delay * 100
                Console.WriteLine("F1")
            Else
                delay_setting = s_delay * 1000
                Console.WriteLine("F2")
            End If
            Console.WriteLine("delay_setting========<><><>>>>>>" & delay_setting)
            ' สร้าง CancellationTokenSource
            Dim cts As New CancellationTokenSource()
            ' รอหน่วงเวลาแบบ Asynchronous พร้อมกับ Token สำหรับการยกเลิก
            Await Task.Delay(delay_setting, cts.Token).ContinueWith(Sub(task)
                                                                        ' ตรวจสอบว่าถ้า Task ไม่ถูกยกเลิก
                                                                        If Not task.IsCanceled Then
                                                                            ' ตรวจสอบว่า Handle ของฟอร์มยังถูกสร้างอยู่และไม่ถูก Dispose
                                                                            If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                                                                                Try
                                                                                    ' เรียกใช้ Me.Invoke เพื่ออัพเดท UI อย่างปลอดภัยบน UI Thread
                                                                                    Me.Invoke(Sub()
                                                                                                  check_bull = 0
                                                                                                  Console.WriteLine("Asynchronous operation completed.")
                                                                                              End Sub)
                                                                                Catch ex As Exception
                                                                                    ' จัดการข้อผิดพลาดภายใน Invoke
                                                                                    check_bull = 0
                                                                                End Try
                                                                            End If
                                                                        End If
                                                                    End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        Catch ex As Exception
            ' จัดการข้อผิดพลาดและแสดงข้อความใน Console
            Console.WriteLine(ex.Message)
        End Try
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        If check_bull = 0 Then
            Dim BitNo As Short
            Dim Id As Short
            Dim Kind As Short
            'MsgBox(m.Msg)
            'MsgBox(DIOM_TRIGGER)
            'MsgBox("test")
            '--------------------------------------
            ' trigger Message 
            check_bull = 0
            delay_btn = 0
            '--------------------------------------
            'Console.WriteLine("N1")
            If start_flg = 1 Then
                'Console.WriteLine("HF1")
                If m.Msg = DIOM_TRIGGER Then
                    Console.WriteLine("READY")
                    Dim result = Manage_counter_contect_DIO()
                    ' Timer3.Enabled = True
                    Console.WriteLine("STOP JAAAA")
                End If
            End If
        Else

        End If
        ' Console.WriteLine("BOOT")
        MyBase.WndProc(m)
    End Sub
    Private Sub Label37_Click(sender As Object, e As EventArgs) Handles Label37.Click

    End Sub
    Public Sub CheckMenu()
        statusDefect = Backoffice_model.GetDefectMenu(MainFrm.Label4.Text)
        If statusDefect = "0" Then
            btnDefects.Enabled = False
        Else
            btnDefects.Enabled = True
        End If
    End Sub

    Private Sub btn_closelot_Click(sender As Object, e As EventArgs) Handles btn_closelot.Click, btnCloseLot.Click
        'MsgBox("Please confirm")
        Me.Enabled = False
        ' Dim clSummary As New closeLotsummary()
        closeLotsummary.statusPage.Text = "working"
        closeLotsummary.Show()
        'Close_lot_cfm.Show()
    End Sub
    Public Function check_tagprint()
        'Dim result_snp As Integer = CDbl(Val(Label6.Text)) Mod CDbl(Val(Label27.Text))
        ' Dim defectAll = CDbl(Val(lb_ng_qty.Text)) + CDbl(Val(lb_nc_qty.Text))
        Dim SQLite = New ModelSqliteDefect
        Dim defectAll = SQLite.mSqlieGetDataNGbyWILot(MainFrm.Label4.Text, Label18.Text, DateTimeStartofShift.Text, Prd_detail.lb_wi.Text)
        ' Dim result_snp As Double = (Integer.Parse(Label6.Text) - defectAll) Mod Integer.Parse(Label27.Text) 'Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
        Dim modsucc As Integer
        If statusPrint = "CloseLot" Then
            modsucc = (CDbl(Val(GoodQty)))
        ElseIf statusPrint = "Normal" Then 'manual
            modsucc = (CDbl(Val(GoodQty)))
        ElseIf statusPrint = "Normal_contect_DIO" Or statusPrint = "Normal_NI_MAX" Or statusPrint = "Normal_DIO_RS232" Then ' auto counter qty
            If CDbl(Val(Label10.Text)) = "0" Then ' remain of wi = 0 ให้ ลบ Defect ด้วย ถ้าำม่ลบ จะมีปัญหาตอนออก แท็ก จะไม่เอา Defect ไปลบ
                modsucc = (CDbl(Val(GoodQty))) - defectAll
            Else
                modsucc = (CDbl(Val(GoodQty)))
            End If
        End If
        Dim result_snp As Integer = modsucc Mod CDbl(Val(Label27.Text)) '(CDbl(Val(GoodQty))) Mod CDbl(Val(Label27.Text)) ' Check From Good
        Dim flg_control As Integer = 0
        If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
            flg_control = 1
        Else
            If result_snp = "0" Then
                flg_control = 1
            End If
        End If
        Return flg_control
    End Function
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim aPen = New Pen(Color.Black)
        aPen.Width = 2.0F
        'vertical
        e.Graphics.DrawLine(aPen, 150, 10, 150, 290)
        e.Graphics.DrawLine(aPen, 300, 175, 300, 290)
        e.Graphics.DrawLine(aPen, 590, 10, 590, 175)
        e.Graphics.DrawLine(aPen, 410, 120, 410, 235)
        e.Graphics.DrawLine(aPen, 410, 175, 410, 235)
        e.Graphics.DrawLine(aPen, 225, 175, 225, 235)
        e.Graphics.DrawLine(aPen, 490, 10, 490, 65)
        e.Graphics.DrawLine(aPen, 520, 175, 520, 290)
        e.Graphics.DrawLine(aPen, 610, 175, 610, 290)
        e.Graphics.DrawLine(aPen, 700, 10, 700, 290)
        'Horizontal
        e.Graphics.DrawLine(aPen, 150, 11, 700, 11)
        e.Graphics.DrawLine(aPen, 150, 65, 590, 65)
        e.Graphics.DrawLine(aPen, 150, 120, 700, 120)
        e.Graphics.DrawLine(aPen, 150, 175, 700, 175)
        e.Graphics.DrawLine(aPen, 150, 235, 610, 235)
        e.Graphics.DrawLine(aPen, 150, 289, 700, 289)
        'TAG LAYOUT
        e.Graphics.DrawString("PART NO.", lb_font1.Font, Brushes.Black, 152, 13)
        e.Graphics.DrawString(Label3.Text, lb_font2.Font, Brushes.Black, 152, 25)
        e.Graphics.DrawString("QTY.", lb_font1.Font, Brushes.Black, 492, 13)
        'Dim result_snp As Integer = CDbl(Val(Label6.Text)) Mod CDbl(Val(Label27.Text)) ' Check From Actual
        'Dim defectAll = CDbl(Val(lb_ng_qty.Text)) + CDbl(Val(lb_nc_qty.Text))
        Dim SQLite = New ModelSqliteDefect
        Dim defectAll = SQLite.mSqlieGetDataNGbyWILot(MainFrm.Label4.Text, Label18.Text, DateTimeStartofShift.Text, wi_no.Text)
        'MsgBox("statusPrint===>" & statusPrint)
        'MsgBox("defectAll ===>" & defectAll)
        'MsgBox("GoodQty====>" & GoodQty)
        '  Dim result_snp As Integer = (CDbl(Val(Label6.Text)) - defectAll) Mod CDbl(Val(Label27.Text)) ' Check From Good
        Dim modsucc As Integer
        If statusPrint = "CloseLot" Then
            modsucc = (CDbl(Val(GoodQty)))
        ElseIf statusPrint = "Normal" Then ' manual
            modsucc = (CDbl(Val(GoodQty)))
        ElseIf statusPrint = "Normal_contect_DIO" Or statusPrint = "Normal_NI_MAX" Or statusPrint = "Normal_DIO_RS232" Then ' auto counter qty
            'MsgBox("Label10.Text====>" & CDbl(Val(Label10.Text)))
            If CDbl(Val(Label10.Text)) = "0" Then ' remain of wi = 0 ให้ ลบ Defect ด้วย ถ้าำม่ลบ จะมีปัญหาตอนออก แท็ก จะไม่เอา Defect ไปลบ
                modsucc = (CDbl(Val(GoodQty))) - defectAll
            Else
                modsucc = (CDbl(Val(GoodQty)))
            End If
        End If
        Dim result_snp As Integer = modsucc Mod CDbl(Val(Label27.Text)) '(CDbl(Val(GoodQty))) Mod CDbl(Val(Label27.Text)) ' Check From Good
        ' MsgBox("before result_snp===>" & result_snp)
        Dim status_tag As String = "[ Incomplete Tag]"
        If V_check_line_reprint = "0" Then
            If result_snp = "0" Then
                result_snp = Label27.Text
                status_tag = " "
            End If
        Else
            If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                result_snp = LB_COUNTER_SEQ.Text
                status_tag = " "
            Else
                If result_snp = "0" Then
                    result_snp = Label27.Text
                    status_tag = " "
                Else
                    'MsgBox("lb_good.Text=====>" & lb_good.Text)
                    'result_snp = CDbl(Val(Label6.Text)) Mod CDbl(Val(Label27.Text)) 'LB_COUNTER_SEQ.Text
                    result_snp = CDbl(Val(lb_good.Text)) Mod CDbl(Val(Label27.Text))
                    status_tag = "[ Incomplete Tag ]"
                End If
            End If
        End If
        ' MsgBox("after result_snp===>" & result_snp)
        e.Graphics.DrawString(result_snp, lb_font2.Font, Brushes.Black, 505, 25)
        e.Graphics.DrawString("PART NAME", lb_font1.Font, Brushes.Black, 152, 67)
        Dim PART_NAME As String = ""
        If Len(Label12.Text) > 36 Then
            PART_NAME = Label12.Text.Replace(vbCrLf, "") '
            Dim pastNameLine1 = Label12.Text.Substring(0, 30)
            Dim pastNameLine2 = Label12.Text.Substring(30)
            e.Graphics.DrawString(pastNameLine1, Label9_fontModel.Font, Brushes.Black, 152, 79)
            e.Graphics.DrawString(pastNameLine2, Label9_fontModel.Font, Brushes.Black, 152, 98)
        Else
            PART_NAME = Label12.Text.Replace(vbCrLf, "")
            e.Graphics.DrawString(PART_NAME, lb_font2.Font, Brushes.Black, 152, 79)
        End If
        'e.Graphics.DrawString(Label12.Text, lb_font2.Font, Brushes.Black, 152, 79)
        e.Graphics.DrawString("MODEL", lb_font1.Font, Brushes.Black, 152, 123)
        e.Graphics.DrawString(lb_model.Text, lb_font4.Font, Brushes.Black, 152, 141)
        e.Graphics.DrawString("NEXT PROCESS", lb_font1.Font, Brushes.Black, 412, 123)
        e.Graphics.DrawString(Backoffice_model.NEXT_PROCESS, lb_font4.Font, Brushes.Black, 414, 141)
        Dim plan_seq As String
        Dim num_char_seq As Integer
        num_char_seq = Label22.Text.Length
        If num_char_seq = 1 Then
            plan_seq = "00" & Label22.Text
        ElseIf num_char_seq = 2 Then
            plan_seq = "0" & Label22.Text
        Else
            plan_seq = Label22.Text
        End If
        Dim the_box_seq As String
        Dim num_box_seq As Integer
        num_box_seq = lb_box_count.Text.Length
        If num_box_seq = 1 Then
            the_box_seq = "00" & lb_box_count.Text
        ElseIf num_box_seq = 2 Then
            the_box_seq = "0" & lb_box_count.Text
        Else
            the_box_seq = lb_box_count.Text
        End If
        e.Graphics.DrawString("LOCATION", lb_font1.Font, Brushes.Black, 592, 123)
        e.Graphics.DrawString(lb_location.Text, lb_font4.Font, Brushes.Black, 596, 141)
        e.Graphics.DrawString("SHIFT", lb_font1.Font, Brushes.Black, 152, 178)
        e.Graphics.DrawString(Label14.Text, lb_font2.Font, Brushes.Black, 170, 190)
        e.Graphics.DrawString("PRO. SEQ.", lb_font1.Font, Brushes.Black, 227, 178)
        e.Graphics.DrawString(plan_seq, lb_font2.Font, Brushes.Black, 231, 190)
        e.Graphics.DrawString("BOX NO.", lb_font1.Font, Brushes.Black, 302, 178)
        e.Graphics.DrawString(the_box_seq, lb_font2.Font, Brushes.Black, 320, 190)
        e.Graphics.DrawString("ACTUAL DATE", lb_font1.Font, Brushes.Black, 412, 178)
        e.Graphics.DrawString(DateTime.Now.ToString("dd/MM/yyyy"), lb_font5.Font, Brushes.Black, 412, 196)
        Dim plan_cd As String
        Dim factory_cd As String
        If MainFrm.Label6.Text = "K2PD06" Then
            factory_cd = "Phase8"
            plan_cd = "52"
        Else
            factory_cd = "Phase10"
            plan_cd = "51"
        End If
        e.Graphics.DrawString("FACTORY", lb_font1.Font, Brushes.Black, 522, 178)
        ' e.Graphics.DrawString(factory_cd, lb_font5.Font, Brushes.Black, 522, 196)
        e.Graphics.DrawString("Fac1", lb_font5.Font, Brushes.Black, 522, 196)
        e.Graphics.DrawString("INFO.", lb_font1.Font, Brushes.Black, 612, 178)
        e.Graphics.DrawString("LINE", lb_font1.Font, Brushes.Black, 152, 238)
        e.Graphics.DrawString(Label24.Text, lb_font2.Font, Brushes.Black, 152, 250)
        Dim result_plan_date As String = ""
        Try
            Dim da As Date = Date.ParseExact(lb_dlv_date.Text.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
            result_plan_date = da.ToString("dd/MM/yyyy")
        Catch ex As Exception
            result_plan_date = lb_dlv_date.Text
        End Try
        e.Graphics.DrawString("PLAN DATE", lb_font1.Font, Brushes.Black, 302, 238)
        e.Graphics.DrawString(result_plan_date, lb_font6.Font, Brushes.Black, 334, 250)
        e.Graphics.DrawString("LOT NO.", lb_font1.Font, Brushes.Black, 522, 238)
        e.Graphics.DrawString(Label18.Text, lb_font2.Font, Brushes.Black, 522, 250)
        e.Graphics.DrawString("Company", lb_font2.Font, Brushes.Black, 15, 13)
        e.Graphics.DrawString("(Thailand) Co., Ltd.", lb_font1.Font, Brushes.Black, 15, 45)
        e.Graphics.DrawString("Shop floor system", lb_font3.Font, Brushes.Black, 15, 73)
        e.Graphics.DrawString("(Gemba5.0 System)", lb_font3.Font, Brushes.Black, 15, 85)
        e.Graphics.DrawString("WI : " & wi_no.Text, lb_font3.Font, Brushes.Black, 15, 123)
        Dim prdtype As String
        If lb_prd_type.Text = "10" Then
            prdtype = "PART TYPE : FG"
        ElseIf lb_prd_type.Text = "40" Then
            prdtype = "PART TYPE : Parts"
        Else
            prdtype = "PART TYPE : FW"
        End If
        e.Graphics.DrawString(prdtype, lb_font3.Font, Brushes.Black, 15, 136)
        e.Graphics.DrawString(status_tag, lb_font3.Font, Brushes.Black, 15, 166)
        Dim iden_cd As String
        If MainFrm.Label6.Text = "K1PD01" Then
            iden_cd = "GA"
        Else
            iden_cd = "GB"
        End If
        Dim plan_date As String = result_plan_date.Substring(6, 4) & result_plan_date.Substring(3, 2) & result_plan_date.Substring(0, 2)
        Dim part_no_res1 As String
        Dim part_no_res As String
        Dim part_numm As Integer = 0
        For part_numm = Label3.Text.Length To 24
            part_no_res = part_no_res & " "
        Next part_numm
        part_no_res1 = Label3.Text & part_no_res
        Dim act_date As String
        Dim actdateConv As Date = DateTime.Now.ToString("dd/MM/yyyy")
        act_date = Format(actdateConv, "yyyyMMdd")
        Dim qty_num As String
        Dim num_char_qty As Integer
        num_char_qty = Len(Trim(result_snp)) 'Label27.Text.Length 'lb_qty_for_box.Text.Length
        If num_char_qty = 1 Then
            qty_num = "     " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 2 Then
            qty_num = "    " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 3 Then
            qty_num = "   " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 4 Then
            qty_num = "  " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 5 Then
            qty_num = " " & result_snp 'lb_qty_for_box.Text
        Else
            qty_num = result_snp 'lb_qty_for_box.Text
        End If
        Dim cus_part_no As String = "                         "
        Dim box_no As String
        Dim num_char_box As Integer
        num_char_box = lb_box_count.Text.Length
        If num_char_box = 1 Then
            box_no = "00" & lb_box_count.Text
        ElseIf num_char_box = 2 Then
            box_no = "0" & lb_box_count.Text
        Else
            box_no = lb_box_count.Text
        End If
        Dim qr_detailss As String
        Try
            PictureBox1.Image = QR_Generator.Encode(iden_cd & Label24.Text & plan_date & plan_seq & part_no_res1 & act_date & qty_num & Label18.Text & cus_part_no & act_date & plan_seq & plan_cd & box_no)
            e.Graphics.DrawImage(PictureBox1.Image, 597, 17, 95, 95)
            e.Graphics.DrawImage(PictureBox1.Image, 31, 190, 95, 95)
            PictureBox8.Image = QR_Generator.Encode(iden_cd & Label24.Text & plan_date & plan_seq & part_no_res1 & act_date & qty_num & Label18.Text & cus_part_no & act_date & plan_seq & plan_cd & box_no)
            e.Graphics.DrawImage(PictureBox8.Image, 620, 199, 70, 70)
            qr_detailss = iden_cd & Label24.Text & plan_date & plan_seq & part_no_res1 & act_date & qty_num & Label18.Text & cus_part_no & act_date & plan_seq & plan_cd & box_no
        Catch ex As Exception
            qr_detailss = ""
        End Try
        ' Try
        'If My.Computer.Network.Ping("192.168.161.101") Then
        ' Backoffice_model.Insert_tag_print(wi_no.Text, qr_detailss, box_no, 1, plan_seq, Label14.Text, check_tagprint(), Label3.Text)
        'Else
        'End If
        'Catch ex As Exception
        'End Try
        lb_qty_for_box.Text = "0"
    End Sub
    Public Sub keep_data_and_gen_qr_tag_fa_completed()
        Dim L_wi As String = ""
        Dim L_boxNo As String = ""
        Dim L_printCount As String = ""
        Dim L_SeqNo As String = ""
        Dim L_Shift As String = ""
        Dim L_flg_control As String = ""
        Dim L_item_cd As String = ""
        Dim result_snp As Integer
        ' Dim defectAll = CDbl(Val(lb_ng_qty.Text)) + CDbl(Val(lb_nc_qty.Text))
        Dim sqlite = New ModelSqliteDefect
        Dim defectAll = sqlite.mSqlieGetDataNGbyWILot(MainFrm.Label4.Text, Label18.Text, DateTimeStartofShift.Text, wi_no.Text)
        If statusPrint = "CloseLot" Then
            result_snp = CDbl(Val(GoodQty)) Mod CDbl(Val(Label27.Text))
        ElseIf statusPrint = "Normal" Then ' manual
            result_snp = CDbl(Val(GoodQty)) Mod CDbl(Val(Label27.Text))
        ElseIf statusPrint = "Normal_contect_DIO" Or statusPrint = "Normal_NI_MAX" Or statusPrint = "Normal_DIO_RS232" Then ' auto counter qty
            Dim rs
            If CDbl(Val(Label10.Text)) = "0" Then ' remain of wi = 0 ให้ ลบ Defect ด้วย ถ้าำม่ลบ จะมีปัญหาตอนออก แท็ก จะไม่เอา Defect ไปลบ
                rs = ((CDbl(Val(GoodQty))) - defectAll)
            Else
                rs = ((CDbl(Val(GoodQty))))
            End If
            result_snp = CDbl(Val(rs)) Mod CDbl(Val(Label27.Text))
        End If
        Dim status_tag As String = "[ Incomplete Tag ]"
        If V_check_line_reprint = "0" Then
            If result_snp = "0" Then
                result_snp = Label27.Text
                status_tag = " "
            End If
        Else
            If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                result_snp = LB_COUNTER_SEQ.Text
                status_tag = " "
            Else
                If result_snp = "0" Then
                    result_snp = Label27.Text
                    status_tag = " "
                Else
                    'result_snp = CDbl(Val(GoodQty)) Mod CDbl(Val(Label27.Text)) 'LB_COUNTER_SEQ.Text 
                    status_tag = "[ Incomplete Tag ]"
                End If
            End If
        End If
        Dim plan_seq As String
        Dim num_char_seq As Integer
        num_char_seq = Label22.Text.Length
        If num_char_seq = 1 Then
            plan_seq = "00" & Label22.Text
        ElseIf num_char_seq = 2 Then
            plan_seq = "0" & Label22.Text
        Else
            plan_seq = Label22.Text
        End If
        Dim the_box_seq As String
        Dim num_box_seq As Integer
        num_box_seq = lb_box_count.Text.Length
        If num_box_seq = 1 Then
            the_box_seq = "00" & lb_box_count.Text
        ElseIf num_box_seq = 2 Then
            the_box_seq = "0" & lb_box_count.Text
        Else
            the_box_seq = lb_box_count.Text
        End If
        Dim plan_cd As String
        Dim factory_cd As String
        If MainFrm.Label6.Text = "K2PD06" Then
            factory_cd = "Phase8"
            plan_cd = "52"
        Else
            factory_cd = "Phase10"
            plan_cd = "51"
        End If
        Dim result_plan_date As String = ""
        Try
            Dim da As Date = Date.ParseExact(lb_dlv_date.Text.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
            result_plan_date = da.ToString("dd/MM/yyyy")
        Catch ex As Exception
            result_plan_date = lb_dlv_date.Text
        End Try
        Dim prdtype As String
        If lb_prd_type.Text = "10" Then
            prdtype = "PART TYPE : FG"
        ElseIf lb_prd_type.Text = "40" Then
            prdtype = "PART TYPE : Parts"
        Else
            prdtype = "PART TYPE : FW"
        End If
        Dim iden_cd As String
        If MainFrm.Label6.Text = "K1PD01" Then
            iden_cd = "GA"
        Else
            iden_cd = "GB"
        End If
        Dim plan_date As String = result_plan_date.Substring(6, 4) & result_plan_date.Substring(3, 2) & result_plan_date.Substring(0, 2)
        Dim part_no_res1 As String
        Dim part_no_res As String
        Dim part_numm As Integer = 0
        For part_numm = Label3.Text.Length To 24
            part_no_res = part_no_res & " "
        Next part_numm
        part_no_res1 = Label3.Text & part_no_res
        Dim act_date As String
        Dim actdateConv As Date = DateTime.Now.ToString("dd/MM/yyyy")
        act_date = Format(actdateConv, "yyyyMMdd")
        act_date = Format(actdateConv, "yyyyMMdd")
        Dim qty_num As String
        Dim num_char_qty As Integer
        num_char_qty = Len(Trim(result_snp)) 'Label27.Text.Length 'lb_qty_for_box.Text.Length
        If num_char_qty = 1 Then
            qty_num = "     " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 2 Then
            qty_num = "    " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 3 Then
            qty_num = "   " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 4 Then
            qty_num = "  " & result_snp 'lb_qty_for_box.Text
        ElseIf num_char_qty = 5 Then
            qty_num = " " & result_snp 'lb_qty_for_box.Text
        Else
            qty_num = result_snp 'lb_qty_for_box.Text
        End If
        Dim cus_part_no As String = "                         "
        Dim box_no As String
        Dim num_char_box As Integer
        num_char_box = lb_box_count.Text.Length
        If num_char_box = 1 Then
            box_no = "00" & lb_box_count.Text
        ElseIf num_char_box = 2 Then
            box_no = "0" & lb_box_count.Text
        Else
            box_no = lb_box_count.Text
        End If
        Dim qr_detailss As String = ""
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                If check_tag_type = "2" Then
                    Dim the_Label_bach As String
                    If Trim(Len(Label_bach.Text)) = 1 Then
                        the_Label_bach = "00" & Label_bach.Text
                    ElseIf Trim(Len(Label_bach.Text)) = 2 Then
                        the_Label_bach = "0" & Label_bach.Text
                    Else
                        the_Label_bach = Label_bach.Text
                    End If
                    box_no = the_Label_bach
                End If
                Dim qr_detailsss = iden_cd & Label24.Text & plan_date & plan_seq & part_no_res1 & act_date & qty_num & Label18.Text & cus_part_no & act_date & plan_seq & plan_cd & box_no
                Backoffice_model.Insert_tag_print(wi_no.Text, qr_detailsss, box_no, 1, plan_seq, Label14.Text, check_tagprint(), Label3.Text, pwi_id, tag_group_no, GoodQty)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function tag_print()
        'asdfasd
        Working_Pro.keep_data_and_gen_qr_tag_fa_completed()
        If check_tag_type = "1" Then
            Working_Pro.PrintDocument1.Print()
        ElseIf check_tag_type = "2" Then
            Backoffice_model.flg_cat_layout_line = "2"
            print_back.print()
        ElseIf check_tag_type = "3" Then
            Working_Pro.PrintDocument1.Print()
        End If
    End Function
    Public Shared Function tag_print_incomplete()
        Working_Pro.PrintDocument2.Print()
    End Function

    Private Sub Label43_Click(sender As Object, e As EventArgs) Handles Label43.Click

    End Sub

    Private Sub Label46_Click(sender As Object, e As EventArgs) Handles lb_loss_status.Click

    End Sub

    'Print incomplete tag
    Private Sub PrintDocument2_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Dim aPen = New Pen(Color.Black)
        aPen.Width = 2.0F
        'e.Graphics.DrawString("TBKK", lb_font2.Font, Brushes.Black, 152, 50)
        'e.Graphics.DrawString("TBKK", lb_font2.Font, Brushes.Black, 152, 50)
        'e.Graphics.DrawString("(Thailand) Co., Ltd.", lb_font1.Font, Brushes.Black, 152, 80)

        'e.Graphics.DrawString("Shopfloor System", lb_font1.Font, Brushes.Black, 152, 110)
        'e.Graphics.DrawString("(New FA system)", lb_font1.Font, Brushes.Black, 152, 122)

        'e.Graphics.DrawString("TBKK", lb_font2.Font, Brushes.Black, 15, 13)
        'e.Graphics.DrawString("(Thailand) Co., Ltd.", lb_font1.Font, Brushes.Black, 15, 45)

        e.Graphics.DrawString("TBKK", lb_font2.Font, Brushes.Black, 15, 13)
        e.Graphics.DrawString("(Thailand) Co., Ltd.", lb_font3.Font, Brushes.Black, 15, 44)

        e.Graphics.DrawString("Shop floor system", lb_font3.Font, Brushes.Black, 15, 68)
        e.Graphics.DrawString("(New FA system)", lb_font3.Font, Brushes.Black, 15, 80)

        e.Graphics.DrawString("INCOMPLETE TAG", lb_font2.Font, Brushes.Black, 242, 5)
        e.Graphics.DrawLine(aPen, 15, 150, 315, 150)

        e.Graphics.DrawLine(aPen, 315, 96, 315, 112)
        e.Graphics.DrawLine(aPen, 315, 183, 315, 199)

        e.Graphics.DrawLine(aPen, 420, 96, 420, 112)
        e.Graphics.DrawLine(aPen, 420, 183, 420, 199)

        e.Graphics.DrawLine(aPen, 314, 96, 333, 96)
        e.Graphics.DrawLine(aPen, 402, 96, 420, 96)

        e.Graphics.DrawLine(aPen, 314, 199, 333, 199)
        e.Graphics.DrawLine(aPen, 402, 199, 420, 199)

        e.Graphics.DrawString(Label24.Text, lb_font2.Font, Brushes.Black, 310, 39)

        e.Graphics.DrawString("Date: " & DateTime.Now.ToString("dd/MM/yyyy"), lb_font3.Font, Brushes.Black, 307, 72)

        e.Graphics.DrawString("PLAN :" & Label8.Text, lb_font4.Font, Brushes.Black, 475, 60)
        e.Graphics.DrawString("COMPLETED :" & Label6.Text, lb_font4.Font, Brushes.Black, 475, 80)
        e.Graphics.DrawString("REMAINING :" & Label10.Text.Substring(1), lb_font4.Font, Brushes.Black, 475, 100)

        Dim plan_seq As String
        Dim num_char_seq As Integer
        num_char_seq = Label22.Text.Length
        If num_char_seq = 1 Then
            plan_seq = "00" & Label22.Text
        ElseIf num_char_seq = 2 Then
            plan_seq = "0" & Label22.Text
        Else
            plan_seq = Label22.Text
        End If
        Try
            PictureBox9.Image = QR_Generator.Encode(wi_no.Text & plan_seq & lb_qty_for_box.Text)
            e.Graphics.DrawImage(PictureBox9.Image, 320, 100, 95, 95)

            PictureBox8.Image = QR_Generator.Encode(wi_no.Text & plan_seq & lb_qty_for_box.Text)
            e.Graphics.DrawImage(PictureBox8.Image, 655, 2, 50, 50)
        Catch ex As Exception

        End Try
        e.Graphics.DrawLine(aPen, 418, 150, 702, 150)
        e.Graphics.DrawString("PART No.:" & Label3.Text, lb_font4.Font, Brushes.Black, 15, 152)
        e.Graphics.DrawString("PART NAME:" & Label12.Text, lb_font4.Font, Brushes.Black, 15, 172)
        e.Graphics.DrawString("MODEL:" & lb_model.Text, lb_font4.Font, Brushes.Black, 15, 192)
        e.Graphics.DrawString("WI PLAN:" & wi_no.Text, lb_font4.Font, Brushes.Black, 15, 212)
        e.Graphics.DrawString("QTY.:" & lb_qty_for_box.Text, lb_font4.Font, Brushes.Black, 15, 232)
        e.Graphics.DrawString("LOT No.:" & Label18.Text, lb_font4.Font, Brushes.Black, 15, 252)
        Dim plan_cd As String
        Dim factory_cd As String
        If MainFrm.Label6.Text = "K2PD06" Then
            factory_cd = "8"
            plan_cd = "52"
        Else
            factory_cd = "10"
            plan_cd = "51"
        End If
        Dim prdtype As String
        If lb_prd_type.Text = "10" Then
            prdtype = "FG"
        ElseIf lb_prd_type.Text = "40" Then
            prdtype = "Parts"
        Else
            prdtype = "FW"
        End If
        e.Graphics.DrawString("LOCATION:" & lb_location.Text, lb_font4.Font, Brushes.Black, 430, 152)
        e.Graphics.DrawString("PHASE:" & factory_cd, lb_font4.Font, Brushes.Black, 430, 172)
        e.Graphics.DrawString("PART TYPE:" & prdtype, lb_font4.Font, Brushes.Black, 430, 192)
        e.Graphics.DrawString("SHIFT:" & Label14.Text, lb_font4.Font, Brushes.Black, 430, 212)
        e.Graphics.DrawString("SNP:" & Label27.Text, lb_font4.Font, Brushes.Black, 430, 232)
        e.Graphics.DrawString("SEQ:" & plan_seq, lb_font4.Font, Brushes.Black, 430, 252)
        e.Graphics.DrawString("Use @ TBKK(Thailand) Co., Ltd.", lb_font1.Font, Brushes.Black, 525, 282)
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.Insert_tag_print(wi_no.Text, qr_detailss, box_no, 1, plan_seq, Label14.Text, check_tagprint(), Label3.Text, pwi_id, Working_Pro.tag_group_no, GoodQty)
                'MsgBox("Ping completed")
            Else
                'MsgBox("Ping incompleted")
            End If
        Catch ex As Exception
            'tr_status = "0"
            ' Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_timee, tr_status)
        End Try
    End Sub
    Public Sub manage_print()
        Dim SQLite = New ModelSqliteDefect
        Dim defectAll = SQLite.mSqlieGetDataNGbyWILot(MainFrm.Label4.Text, Label18.Text, DateTimeStartofShift.Text, wi_no.Text)
        Dim result_add As Integer = CDbl(Val(lb_ins_qty.Text)) + CDbl(Val(Label6.Text))
        Dim rsGood As Integer = CDbl(Val(lb_good.Text))
        Dim loop_check As Integer = result_add / CDbl(Val(Label27.Text))
        Dim loop_check_good As Integer = rsGood / CDbl(Val(Label27.Text))
        Dim D As Integer = 0
        ' Dim value_label6 As Integer = Label6.Text
        Dim value_label6 As Integer = rsGood - CDbl(Val(lb_ins_qty.Text))
        Dim tmp_good As Integer = value_label6
        'For index_check_print As Integer = Label6.Text To result_add '10
        '	MsgBox("value= " & index_check_print)
        '		Next
        If value_label6 <= 0 Then
            value_label6 = 1
        Else
            value_label6 += 1
        End If
        Dim flg_reprint As String = V_check_line_reprint
        Label6.Text = result_add
        Dim i As Integer = 1
        For index_check_print As Integer = value_label6 To rsGood  'result_add   '10
            'For index_check As Integer = 1 To loop_check
            tmp_good = tmp_good + i
            Dim result_mod As Integer = index_check_print Mod CDbl(Val(Label27.Text))
            'หาเศษ
            'If result_mod = 0 And textp_result <> 0 Then
            If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                lb_box_count.Text = lb_box_count.Text + 1
                print_back.PrintDocument2.Print()
                If result_mod = "0" Or Label10.Text = "0" Then
                    Label_bach.Text += 1
                    GoodQty = Label6.Text
                    tag_print()
                End If
            Else
                If flg_reprint = "0" Then
                    If result_mod = "0" Then
                        Label_bach.Text += 1
                        lb_box_count.Text = lb_box_count.Text + 1
                        GoodQty = tmp_good 'lb_good.Text 'Label6.Text
                        tag_print()
                    Else
                        Dim ActBywi = tmp_good + defectAll
                        If ActBywi = Label8.Text Then ' act = plan
                            lb_box_count.Text = lb_box_count.Text + 1
                            Label_bach.Text += 1
                            GoodQty = lb_good.Text
                            tag_print()
                        End If
                    End If
                Else
                    If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                    Else
                        If result_mod = "0" Then
                            Label_bach.Text = Label_bach.Text + 1
                            lb_box_count.Text = lb_box_count.Text + 1
                            GoodQty = Label6.Text
                            tag_print()
                        End If
                    End If
                End If
            End If
            'End If
        Next
    End Sub
    Public Function ins_qty_fn_manual()
        statusPrint = "Normal"
        Dim add_value_loop As Integer = 0
        Dim result_add As Integer = CDbl(Val(lb_ins_qty.Text)) + CDbl(Val(Label6.Text))
        Dim loop_check As Integer = result_add / CDbl(Val(Label27.Text))
        If V_check_line_reprint = "1" Then
            If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                If (CDbl(Val(Label6.Text) + CDbl(Val(lb_ins_qty.Text)))) = CDbl(Val(Label8.Text)) Then
                    Label6.Text = result_add
                    lb_box_count.Text = lb_box_count.Text + 1
                    Label_bach.Text = Label_bach.Text + 1
                    GoodQty = Label6.Text
                    tag_print()
                Else
                    Label6.Text = result_add
                End If
            Else
                manage_print()
            End If
        Else
            'MsgBox("ready")
            manage_print()
        End If
        Dim pd As String = MainFrm.Label6.Text
        Dim line_cd As String = MainFrm.Label4.Text
        Dim wi_plan As String = wi_no.Text
        Dim item_cd As String = Label3.Text
        Dim item_name As String = Label12.Text
        Dim staff_no As String = Label29.Text
        Dim seq_no As String = Label22.Text
        Dim prd_qty As Integer = cnt_btn
        Dim end_time As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim tr_status As String = "0"
        Dim number_qty As Integer = Label6.Text
        Dim start_time As Date = st_count_ct.Text
        st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        If Backoffice_model.start_check_date_paralell_line <> "" Then
            start_time2 = Backoffice_model.start_check_date_paralell_line
            end_time2 = Backoffice_model.end_check_date_paralell_line
        End If
        Dim use_time As Double = Cal_Use_Time_ins_qty_fn_manual(start_time2, end_time2) '"1"
        If CDbl(Val(Label8.Text)) > CDbl(Val(Label6.Text)) Then
            Dim result_friff As Integer = CDbl(Val(Label8.Text)) - CDbl(Val(Label6.Text))
            Dim driff As Integer = 0
            If result_friff < 0 Then
                driff = "0"
            Else
                driff = "-" & result_friff
            End If
            Label10.Text = driff
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            Backoffice_model.Insert_prd_detail(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, start_time2, end_time2, use_time, number_qty, Spwi_id(j), tr_status)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, start_time2, end_time2, use_time, number_qty, pwi_id, tr_status)
                    End If
                Else
                    tr_status = "0"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                End If
            Catch ex As Exception
                tr_status = "0"
                If MainFrm.chk_spec_line = "2" Then
                    Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                    Dim Iseq = GenSEQ
                    Dim j As Integer = 0
                    For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                        Iseq += 1
                        Dim special_wi As String = itemPlanData.wi
                        Dim special_item_cd As String = itemPlanData.item_cd
                        Dim special_item_name As String = itemPlanData.item_name
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        j = j + 1
                    Next
                Else
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End If
            End Try
        End If
        If CDbl(Val(Label8.Text)) = CDbl(Val(Label6.Text)) Then
            Dim result_friff As Integer = CDbl(Val(Label8.Text)) - result_add
            Dim driff As Integer = 0
            If result_friff < 0 Then
                driff = "0"
            Else
                driff = "-" & result_friff
            End If
            Label10.Text = driff
            comp_flg = 1
            If comp_flg = 1 Then
                Try
                    If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                        tr_status = "1"
                        If MainFrm.chk_spec_line = "2" Then
                            Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                            Dim Iseq = GenSEQ
                            Dim j As Integer = 0
                            For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                                Iseq += 1
                                Dim special_wi As String = itemPlanData.wi
                                Dim special_item_cd As String = itemPlanData.item_cd
                                Dim special_item_name As String = itemPlanData.item_name
                                Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                                Backoffice_model.Insert_prd_detail(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, start_time2, end_time2, use_time, number_qty, Spwi_id(j), tr_status)
                                j = j + 1
                            Next
                        Else
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, start_time2, end_time2, use_time, number_qty, pwi_id, tr_status)
                        End If
                    Else
                        tr_status = "0"
                        If MainFrm.chk_spec_line = "2" Then
                            Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                            Dim Iseq = GenSEQ
                            Dim j As Integer = 0
                            For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                                Iseq += 1
                                Dim special_wi As String = itemPlanData.wi
                                Dim special_item_cd As String = itemPlanData.item_cd
                                Dim special_item_name As String = itemPlanData.item_name
                                Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                                j = j + 1
                            Next
                        Else
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        End If
                    End If
                Catch ex As Exception
                    tr_status = "0"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, lb_ins_qty.Text, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                End Try
                Dim shift_prd3 As String = Label14.Text
                Dim prd_st_datetime3 As Date = st_time.Text
                Dim prd_end_datetime3 As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim lot_no3 As String = Label18.Text
                Dim comp_flg3 As String = "1"
                Dim transfer_flg3 As String = "1"
                Dim del_flg3 As String = "0"
                Dim prd_flg3 As String = "1"
                Dim close_lot_flg3 As String = "1"
            End If
            Me.Enabled = False
            Dim result_mod As Integer = CDbl(Val(Label6.Text)) Mod CDbl(Val(Label27.Text))
            If result_mod <> "0" Then
                lb_box_count.Text = lb_box_count.Text + 1
                GoodQty = Label6.Text
                tag_print()
            End If
            Finish_work.Show() ' เกี่ยว
        End If
        cal_eff()
    End Function
    Private cts As CancellationTokenSource = New CancellationTokenSource()
    Async Function LOAD_OEE() As Task
        ' MsgBox("load oee")
        Try
            While True
                Console.WriteLine("READY LOAD OEE 2 ")
                ' รอ 60 วินาที
                Await Task.Delay(60000, cts.Token).ContinueWith(Sub(task)
                                                                    ' ตรวจสอบว่า Task ไม่ถูกยกเลิก
                                                                    If Not task.IsCanceled Then
                                                                        ' ตรวจสอบว่า Handle ของฟอร์มยังถูกสร้างอยู่และไม่ถูก Dispose
                                                                        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                                                                            Try
                                                                                Me.Invoke(Sub()
                                                                                              Try
                                                                                                  Console.WriteLine("READY LOAD OEE call ")
                                                                                                  cal_eff()
                                                                                              Catch ex As Exception
                                                                                                  Console.WriteLine("CATCH LOAD OEE call: " & ex.Message)
                                                                                              End Try
                                                                                          End Sub)
                                                                            Catch ex As Exception
                                                                                Console.WriteLine("CATCH NO LOAD OEE: " & ex.Message)
                                                                            End Try
                                                                        End If
                                                                    End If
                                                                End Sub, TaskScheduler.FromCurrentSynchronizationContext())
            End While
        Catch ex As Exception
            Console.WriteLine("err cal_eff ===>" & ex.Message)
        End Try
    End Function
    ' ฟังก์ชันสำหรับยกเลิกการโหลด OEE
    Public Sub CancelLOAD_OEE()
        cts.Cancel()
        cts = New CancellationTokenSource() ' สร้าง CancellationTokenSource ใหม่
    End Sub
    Public Async Function cal_eff() As Task
        Try
            Try
                If checkStatusEmergency(MainFrm.Label4.Text) = "0" Then
                    PanelWebviewEmergency.Enabled = False
                    PanelWebviewEmergency.Visible = False
                    WebViewEmergency.CoreWebView2.Navigate("http://" & Backoffice_model.svApi & "/API_NEW_FA/SpecialCode/Defaultpage")
                Else
                    If PanelWebviewEmergency.Visible Then
                    Else
                        WebViewEmergency.CoreWebView2.Navigate("http://" & Backoffice_model.svApi & "/API_NEW_FA/SpecialCode/EMERGENCY")
                        PanelWebviewEmergency.Enabled = True
                        PanelWebviewEmergency.Visible = True
                    End If
                End If
            Catch ex As Exception
            End Try
            RunCmd(MainFrm.Label4.Text)
            Console.WriteLine("READY LOAD OEE cal_eff ")
            ' Inside the continuation, invoke on the UI thread
            Console.WriteLine("CAL OEE cal_eff")
            ' Perform various UI operations
            set_AccTarget(Prd_detail.Label12.Text.Substring(3, 5), Label38.Text, gobal_stTimeModel)
            setlvA(MainFrm.Label4.Text, Label18.Text, Label14.Text, DateTime.Now.ToString("yyyy-MM-dd"), Prd_detail.Label12.Text.Substring(3, 5), gobal_stTimeModel, MainFrm.chk_spec_line)
            setlvQ(MainFrm.Label4.Text, Label18.Text, Prd_detail.Label12.Text.Substring(3, 5), gobal_stTimeModel)
            Dim P = setgetSpeedLoss(lbNG.Text, lb_good.Text, Prd_detail.Label12.Text.Substring(3, 5), Label38.Text, MainFrm.Label4.Text, gobal_stTimeModel)
            Dim GoodByPartNo As Integer = CDbl(Val(actualP.Text)) - CDbl(Val(lbOverTimeQuality.Text))
            Dim Q = cal_progressbarQ(lbOverTimeQuality.Text, GoodByPartNo)
            Dim A = cal_progressbarA(MainFrm.Label4.Text, Prd_detail.Label12.Text.Substring(3, 5), Prd_detail.Label12.Text.Substring(11, 5))
            setNgByHour(MainFrm.Label4.Text, Label18.Text)
            'Dim rswebview = loadDataProgressBar(MainFrm.Label4.Text, Label14.Text)
            ' WebViewProgressbar.Reload()
            calProgressOEE(A, Q, P)
        Catch ex As Exception
            Console.WriteLine("err cal_eff ===>" & ex.Message)
        End Try
    End Function
    Public Function Cal_Use_Time_ins_qty_fn_manual(date_start_data As Date, date_end As Date)
        Dim total_ins_qty As Integer = DateDiff(DateInterval.Second, date_start_data, date_end)
        lb_use_time.Text = total_ins_qty
        If total_ins_qty < 0 Then
            Minutes_total = Math.Abs(CDbl(Val(lb_use_time.Text)))
            lb_use_time.Text = Minutes_total
        ElseIf total_ins_qty = 0 Then
            lb_use_time.Text = 1
        End If
        Return lb_use_time.Text
    End Function
    Public Function ins_qty_fn()
        Dim yearNow As Integer = DateTime.Now.ToString("yyyy")
        Dim monthNow As Integer = DateTime.Now.ToString("MM")
        Dim dayNow As Integer = DateTime.Now.ToString("dd")
        Dim hourNow As Integer = DateTime.Now.ToString("HH")
        Dim minNow As Integer = DateTime.Now.ToString("mm")
        Dim secNow As Integer = DateTime.Now.ToString("ss")
        'MsgBox(yearNow & monthNow & dayNow & hourNow & minNow & secNow)
        Dim yearSt As Integer = st_time.Text.Substring(0, 4)
        Dim monthSt As Integer = st_time.Text.Substring(5, 2)
        Dim daySt As Integer = st_time.Text.Substring(8, 2)
        Dim hourSt As Integer = st_time.Text.Substring(11, 2)
        Dim minSt As Integer = st_time.Text.Substring(14, 2)
        Dim secSt As Integer = st_time.Text.Substring(17, 2)
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.updated_data_to_dbsvr()
            End If
        Catch ex As Exception
        End Try
        'MsgBox(yearSt & minthSt & daySt & hourSt & minSt & secSt)
        Dim firstDate As New System.DateTime(yearSt, monthSt, daySt, hourSt, minSt, secSt)
        Dim secondDate As New System.DateTime(yearNow, monthNow, dayNow, hourNow, minNow, secNow)
        Dim diff As System.TimeSpan = secondDate.Subtract(firstDate)
        Dim diff1 As System.TimeSpan = secondDate - firstDate
        Dim diff2 As String = (secondDate - firstDate).TotalSeconds.ToString()
        'MsgBox(diff2)
        'MsgBox(diff2 / 60)
        Dim actCT As Double = Format(diff2 / 60, "0.00")
        'Format(ListBox2.Items(numOfindex), "0.00")
        'st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim round_ins As Integer = lb_ins_qty.Text
        If round_ins <= 0 Then
            round_ins = Backoffice_model.qty_int
            Backoffice_model.qty_int = 0
        End If
        For index As Integer = 1 To round_ins
            Dim cnt_btn As Integer = 1
            count = CInt(count)
            count = count + cnt_btn
            _Edit_Up_0.Text = count
            If count <= 0 Then
                count = 1 'Backoffice_model.qty_int
                Backoffice_model.qty_int = "0"
            End If
            If _Edit_Up_0.Text <= 0 Then
                _Edit_Up_0.Text = 1
            End If
            Dim Act As Integer = 0
            If Label6.Text <= 0 Then
                Act = 1
            Else
                Act = Label6.Text
            End If
            If comp_flg = 0 Then
                Dim result_mod As Double = Integer.Parse(Act + 1) Mod Integer.Parse(Label27.Text)
                lb_qty_for_box.Text = lb_qty_for_box.Text + cnt_btn
                Dim textp_result As Integer = Label10.Text
                textp_result = Math.Abs(textp_result) - 1
                'MsgBox(textp_result)
                Dim sum_act_total As Integer = Label6.Text + cnt_btn
                Label6.Text = sum_act_total
                If result_mod = 0 And textp_result <> 0 Then
                    If V_check_line_reprint = "0" Then
                        lb_box_count.Text = lb_box_count.Text + 1
                        Label_bach.Text = Label_bach.Text + 1
                        GoodQty = Label6.Text
                        tag_print()
                    Else
                        If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                        Else
                            lb_box_count.Text = lb_box_count.Text + 1
                            Label_bach.Text = Label_bach.Text + 1
                            GoodQty = Label6.Text
                            tag_print()
                        End If
                    End If
                End If
                Dim sum_prg As Integer = (Label6.Text * 100) / Label8.Text
                If sum_prg > 100 Then
                    sum_prg = 100
                ElseIf sum_prg < 0 Then
                    sum_prg = 0
                End If
                CircularProgressBar1.Text = sum_prg & "%"
                CircularProgressBar1.Value = sum_prg
                Dim use_time As Integer = Label34.Text
                Dim dt1 As DateTime = DateTime.Now
                Dim dt2 As DateTime = st_count_ct.Text
                Dim dtspan As TimeSpan = dt1 - dt2
                Dim actCT_jing As Double = Format((dtspan.Seconds / _Edit_Up_0.Text) + (dtspan.Minutes * 60), "0.00")
                ListBox1.Items.Add((dtspan.Seconds) + (dtspan.Minutes * 60) + (dtspan.Hours * 3600))
                Dim Total As Double = 0
                Dim Count As Double = 0
                Dim Average As Double = 0
                Dim I As Integer
                For I = 0 To ListBox1.Items.Count - 1
                    Total += Val(ListBox1.Items(I))
                    Count += 1
                Next
                Average = Total / Count
                Label37.Text = Format(Average, "0.0")
                Dim start_time As Date = st_count_ct.Text
                st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim dt11 As DateTime = DateTime.Now
                Dim dt22 As DateTime = st_time.Text
                Dim dtspan1 As TimeSpan = dt11 - dt22
                If (dtspan1.Minutes + (dtspan1.Hours * 60)) >= use_time Then
                    Label20.ForeColor = Color.Red
                End If
                Dim temppola As Double = ((dtspan1.Seconds / 60) + (dtspan1.Minutes + (dtspan1.Hours * 60)))
                If temppola < 1 Then
                    temppola = 1
                End If
                Dim loss_sum As Integer
                Try
                    If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                        Dim LoadSQL = Backoffice_model.get_sum_loss(Trim(wi_no.Text))
                        While LoadSQL.Read()
                            loss_sum = LoadSQL("sum_loss")
                        End While
                    Else
                        loss_sum = 0
                    End If
                Catch ex As Exception
                End Try
                Dim sum_prg2 As Integer = (((Label38.Text * _Edit_Up_0.Text) / ((temppola * 60) - loss_sum)) * 100)
                sum_prg2 = sum_prg2 / cnt_btn
                If sum_prg2 > 100 Then
                    sum_prg2 = 100
                ElseIf sum_prg2 < 0 Then
                    sum_prg2 = 0
                End If
                If sum_prg2 <= 49 Then
                    CircularProgressBar2.ProgressColor = Color.Red
                    CircularProgressBar2.ForeColor = Color.Black
                ElseIf sum_prg2 >= 50 And sum_prg2 <= 79 Then
                    CircularProgressBar2.ProgressColor = Color.Chocolate
                    CircularProgressBar2.ForeColor = Color.Black
                ElseIf sum_prg2 >= 80 And sum_prg2 <= 100 Then
                    CircularProgressBar2.ProgressColor = Color.Green
                    CircularProgressBar2.ForeColor = Color.Black
                End If
                Dim avarage_eff As Double = Format(sum_prg2, "0.00")
                lb_sum_prg.Text = avarage_eff
                CircularProgressBar2.Text = sum_prg2 & "%"
                CircularProgressBar2.Value = sum_prg2
                Dim actCT_jingna As Double = Format(dtspan.Seconds + (dtspan.Minutes * 60), "0.00")
                Dim pd As String = MainFrm.Label6.Text
                Dim line_cd As String = MainFrm.Label4.Text
                Dim wi_plan As String = wi_no.Text
                Dim item_cd As String = Label3.Text
                Dim item_name As String = Label12.Text
                Dim staff_no As String = Label29.Text
                Dim seq_no As String = Label22.Text
                Dim prd_qty As Integer = cnt_btn
                Dim end_time As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim use_timee As Double = actCT_jingna
                Dim tr_status As String = "0"
                Dim number_qty As Integer = Label6.Text
                Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                If Backoffice_model.start_check_date_paralell_line <> "" Then
                    start_time2 = Backoffice_model.start_check_date_paralell_line
                    end_time2 = Backoffice_model.end_check_date_paralell_line
                End If
                Try
                    If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                        tr_status = "1"
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time2, end_time2, use_time, number_qty, pwi_id, tr_status)
                    Else
                        tr_status = "0"
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                Catch ex As Exception
                    tr_status = "0"
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End Try
                Dim sum_diff As Integer = Label8.Text - Label6.Text
                If sum_diff < 0 Then
                    Label10.Text = "0"
                Else
                    Label10.Text = "-" & sum_diff
                End If
                If sum_diff < 1 Then
                    If sum_diff = 0 Then
                        lb_box_count.Text = lb_box_count.Text + 1
                        Label_bach.Text = Label_bach.Text + 1
                        GoodQty = Label6.Text
                        tag_print()
                    Else
                    End If
                    Me.Enabled = False
                    comp_flg = 1
                    Finish_work.Show()
                    Dim plan_qty As Integer = Label8.Text
                    Dim act_qty As Integer = _Edit_Up_0.Text
                    Try
                        act_qty = LB_COUNTER_SEQ.Text 'Working_Pro._Edit_Up_0.Text
                        LB_COUNTER_SHIP.Text = 0
                        LB_COUNTER_SEQ.Text = 0
                        QtyMold = 0
                    Catch ex As Exception
                        act_qty = 0
                    End Try
                    Dim shift_prd As String = Label14.Text
                    Dim prd_st_datetime As Date = st_time.Text
                    Dim prd_end_datetime As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                    Dim lot_no As String = Label18.Text
                    Dim comp_flg2 As String = "1"
                    Dim transfer_flg As String = "1"
                    Dim del_flg As String = "0"
                    Dim prd_flg As String = "1"
                    Dim close_lot_flg As String = "1"
                    Dim avarage_act_prd_time As Double = Average
                    Try
                        If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                            transfer_flg = "1"
                            Backoffice_model.Insert_prd_close_lot(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                            Backoffice_model.Insert_prd_close_lot_sqlite(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                            'Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_timee, number_qty)
                            Backoffice_model.work_complete(wi_plan)
                            Dim temp_co_emp As Integer = List_Emp.ListView1.Items.Count
                            Dim emp_cd As String
                            For I = 0 To temp_co_emp - 1
                                emp_cd = List_Emp.ListView1.Items(I).Text
                                Backoffice_model.Insert_emp_cd(wi_plan, emp_cd, seq_no)
                            Next
                        Else
                            transfer_flg = "0"
                            Backoffice_model.Insert_prd_close_lot_sqlite(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                        End If
                    Catch ex As Exception
                        'MsgBox("Ins incompleted2")
                        transfer_flg = "0"
                        Backoffice_model.Insert_prd_close_lot_sqlite(wi_plan, line_cd, item_cd, plan_qty, act_qty, seq_no, shift_prd, staff_no, prd_st_datetime, prd_end_datetime, lot_no, comp_flg2, transfer_flg, del_flg, prd_flg, close_lot_flg, avarage_eff, avarage_act_prd_time)
                        'Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time, end_time, use_timee, tr_status)
                    End Try
                End If
            End If
        Next
    End Function
    Private Sub btn_ins_act_Click(sender As Object, e As EventArgs) Handles btn_ins_act.Click
        Me.Enabled = False
        select_int_qty.Show()
        'ins_qty.Show()
    End Sub
    Private Sub btn_desc_act_Click(sender As Object, e As EventArgs) Handles btn_desc_act.Click
        Dim snp As Integer = Convert.ToInt32(Label27.Text)
        Try
            Desc_act.Label1.Text = Label6.Text 'LB_COUNTER_SHIP.Text '_Edit_Up_0.Text Mod snp
        Catch ex As Exception
            Desc_act.Label1.Text = "0"
        End Try
        Dim result As Integer = 0
        If CDbl(Val(LB_COUNTER_SHIP.Text)) = 0 Or CDbl(Val(Label6.Text)) = 0 Then
            result = 0 'LB_COUNTER_SHIP.Text
        ElseIf CDbl(Val(LB_COUNTER_SHIP.Text)) > CDbl(Val(Label6.Text)) Then
            result = CDbl(Val(Label6.Text)) 'CDbl(Val(LB_COUNTER_SHIP.Text)) - CDbl(Val(Label6.Text))
        ElseIf CDbl(Val(LB_COUNTER_SHIP.Text)) < CDbl(Val(Label6.Text)) Then
            result = LB_COUNTER_SHIP.Text
        Else
            result = LB_COUNTER_SHIP.Text
        End If
        Desc_act.Label1.Text = CDbl(Val(LB_COUNTER_SEQ.Text)) 'result
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.updated_data_to_dbsvr()
                Desc_act.Show()
                Me.Enabled = False
            Else
                load_show.Show()
            End If
        Catch ex As Exception
            load_show.Show()
        End Try
    End Sub
    Private Sub _Edit_Up_0_TextChanged(sender As Object, e As EventArgs) Handles _Edit_Up_0.TextChanged

    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim date_data As Date = DateTime.Now.ToString("HH:mm:ss")
        If TimeOfDay.ToString("HH:mm:ss") >= Backoffice_model.coles_lot_start_shift And TimeOfDay.ToString("HH:mm:ss") <= Backoffice_model.coles_lot_end_shift Then
            'If Auto_close_lot.Visible = True Then
            If closeLotsummary.Visible = True Then

            Else
                ' Auto_close_lot.Show()
                If btn_start.Enabled Then
                    stop_working()
                End If
                Dim rs = Backoffice_model.AlertCheck_close_lot(MainFrm.Label4.Text, MainFrm.Label6.Text)
                closeLotsummary.Show()
            End If
        End If
    End Sub
    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs)

    End Sub
    Private Async Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        delay_btn += 1
        If delay_btn = s_delay Then
            check_bull = 0
            delay_btn = 0
            Timer3.Enabled = False
        Else
            check_bull = 1
        End If
    End Sub
    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles Label16.Click
    End Sub
    Public Async Function counter_data_new_dio() As Task(Of Integer)
        Console.WriteLine("READY NI MAX")
        Try
            If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                Backoffice_model.updated_data_to_dbsvr()
            End If
        Catch ex As Exception
        End Try
        Dim resultSumDiff As Integer = CDbl(Val(Label8.Text)) - (CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text))
        If resultSumDiff < 0 Then
            MsgBox("Please Check Cavity Over Plan new dio NI MAX.")
            Return 0
        End If
        Dim yearNow As Integer = DateTime.Now.ToString("yyyy")
        Dim monthNow As Integer = DateTime.Now.ToString("MM")
        Dim dayNow As Integer = DateTime.Now.ToString("dd")
        Dim hourNow As Integer = DateTime.Now.ToString("HH")
        Dim minNow As Integer = DateTime.Now.ToString("mm")
        Dim secNow As Integer = DateTime.Now.ToString("ss")
        'MsgBox(yearNow & monthNow & dayNow & hourNow & minNow & secNow)
        Dim yearSt As Integer = st_time.Text.Substring(0, 4)
        Dim monthSt As Integer = st_time.Text.Substring(5, 2)
        Dim daySt As Integer = st_time.Text.Substring(8, 2)
        Dim hourSt As Integer = st_time.Text.Substring(11, 2)
        Dim minSt As Integer = st_time.Text.Substring(14, 2)
        Dim secSt As Integer = st_time.Text.Substring(17, 2)
        'MsgBox(yearSt & minthSt & daySt & hourSt & minSt & secSt)
        Dim firstDate As New System.DateTime(yearSt, monthSt, daySt, hourSt, minSt, secSt)
        Dim secondDate As New System.DateTime(yearNow, monthNow, dayNow, hourNow, minNow, secNow)
        Dim diff As System.TimeSpan = secondDate.Subtract(firstDate)
        Dim diff1 As System.TimeSpan = secondDate - firstDate
        Dim diff2 As String = (secondDate - firstDate).TotalSeconds.ToString()
        'MsgBox(diff2)
        'MsgBox(diff2 / 60)
        Dim actCT As Double = Format(diff2 / 60, "0.00")
        'Format(ListBox2.Items(numOfindex), "0.00")
        'st_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        'Counter
        Dim cnt_btn As Integer = Integer.Parse(MainFrm.cavity.Text)
        'Counter
        count = count + cnt_btn
        _Edit_Up_0.Text = count
        Dim Act As Integer = 0
        Dim action_plus As Integer = 0
        If Label6.Text <= 0 Then ' condition  
            Act = 1
            action_plus = 0
        Else
            Act = lb_good.Text 'Label6.Text
            action_plus = 1
        End If
        Dim start_time As Date = st_count_ct.Text
        Dim end_time As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim start_time2 As String = start_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        Dim end_time2 As String = end_time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        Dim checkTransection As String
        statusPrint = "Normal_NI_MAX"
        'Try
        'If My.Computer.Network.Ping("192.168.161.101") Then
        'checkTransection = Backoffice_model.checkTransection(pwi_id, CDbl(Val(Label6.Text)) + Integer.Parse(MainFrm.cavity.Text), start_time2)
        'Else
        'checkTransection = "1"
        'End If
        'Catch ex As Exception
        'checkTransection = "1"
        'End Try
        ' If checkTransection = "1" Then
        If comp_flg = 0 Then
            'Dim result_mod As Double = Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            'MsgBox("Act + action_plus===>" & (Act + action_plus))
            Dim result_mod As Double = Integer.Parse(Act + action_plus) Mod Integer.Parse(Label27.Text) 'Integer.Parse(_Edit_Up_0.Text) Mod Integer.Parse(Label27.Text)
            ' MsgBox("result_mod===>" & result_mod)
            lb_qty_for_box.Text = lb_qty_for_box.Text + cnt_btn
            Dim textp_result As Integer = Label10.Text
            textp_result = Math.Abs(textp_result) - 1
            'MsgBox(textp_result)
            Dim sum_act_total As Integer = Label6.Text + cnt_btn
            Label6.Text = sum_act_total
            LB_COUNTER_SHIP.Text += cnt_btn
            LB_COUNTER_SEQ.Text += cnt_btn
            lb_good.Text += cnt_btn
            QtyMold += cnt_btn
            If check_tag_type = "3" Then
                Dim break = lbPosition1.Text & " " & lbPosition2.Text
                Dim plb = New PrintLabelBreak
                plb.loadData(Label3.Text, break, Label18.Text, Label22.Text, CDbl(Val(LB_COUNTER_SEQ.Text)))
            End If
            If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                lb_box_count.Text = lb_box_count.Text + 1
                print_back.PrintDocument2.Print()
                If result_mod = "0" Then
                    Label_bach.Text += 1
                    GoodQty = Label6.Text
                    '  MsgBox("IF result_mod ====>" & GoodQty)
                    tag_print()
                End If
            Else
                If result_mod = 0 And textp_result <> 0 Then
                    If V_check_line_reprint = "0" Then
                        lb_box_count.Text = lb_box_count.Text + 1
                        'Label_bach.Text = Label_bach.Text + 1
                        'GoodQty = Label6.Text
                        GoodQty = lb_good.Text
                        'MsgBox("IF IF result_mod  GoodQty ====>" & GoodQty)
                        tag_print()
                    Else
                        If CDbl(Val(Label27.Text)) = 1 Or CDbl(Val(Label27.Text)) = 999999 Then
                        Else
                            lb_box_count.Text = lb_box_count.Text + 1
                            Label_bach.Text = Label_bach.Text + 1
                            GoodQty = Label6.Text
                            '  MsgBox("IF IF result_mod ====>" & GoodQty)
                            tag_print()
                        End If
                    End If
                End If
            End If
            Dim sum_prg As Integer = (Label6.Text * 100) / Label8.Text
            'MsgBox(sum_prg)
            If sum_prg > 100 Then
                sum_prg = 100
            ElseIf sum_prg < 0 Then
                sum_prg = 0
            End If
            CircularProgressBar1.Text = sum_prg & "%"
            CircularProgressBar1.Value = sum_prg
            Dim use_time As Integer = Label34.Text
            Dim dt1 As DateTime = DateTime.Now
            Dim dt2 As DateTime = st_count_ct.Text
            Dim dtspan As TimeSpan = dt1 - dt2
            Dim actCT_jing As Double = Format((dtspan.Seconds / _Edit_Up_0.Text) + (dtspan.Minutes * 60), "0.00")
            ListBox1.Items.Add((dtspan.Seconds) + (dtspan.Minutes * 60) + (dtspan.Hours * 3600))
            Dim Total As Double = 0
            Dim Count As Double = 0
            Dim Average As Double = 0
            Dim I As Integer
            For I = 0 To ListBox1.Items.Count - 1
                Total += Val(ListBox1.Items(I))
                Count += 1
            Next
            Average = Total / Count
            If Count = 1 Then
                Try
                    Backoffice_model.Tag_seq_rec_sqlite(lb_ref_scan.Text.Substring(0, 10), lb_ref_scan.Text.Substring(10, 3), lb_ref_scan.Text.Substring(13, (lb_ref_scan.Text.Length - 13)), RTrim(lb_ref_scan.Text))
                Catch ex As Exception
                End Try
            End If
            Label37.Text = Format(Average, "0.0")
            st_count_ct.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            Dim dt11 As DateTime = DateTime.Now
            Dim dt22 As DateTime = st_time.Text
            Dim dtspan1 As TimeSpan = dt11 - dt22
            'MsgBox(dtspan1.Minutes)
            If (dtspan1.Minutes + (dtspan1.Hours * 60)) >= use_time Then
                Label20.ForeColor = Color.Red
            End If
            Dim temppola As Double = ((dtspan1.Seconds / 60) + (dtspan1.Minutes + (dtspan1.Hours * 60)))
            If temppola < 1 Then
                temppola = 1
            End If
            Dim loss_sum As Integer
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    Dim LoadSQL = Backoffice_model.get_sum_loss(Trim(wi_no.Text))
                    While LoadSQL.Read()
                        loss_sum = LoadSQL("sum_loss")
                    End While
                Else
                    loss_sum = 0
                End If
            Catch ex As Exception
                'load_show.Show()
                'loss_sum = 0
            End Try
            Dim sum_prg2 As Integer = (((Label38.Text * _Edit_Up_0.Text) / ((temppola * 60) - loss_sum)) * 100)
            'Dim sum_prg2 As Integer = (((CycleTime.Text * _Edit_Up_0.Text) / temppola) * 100)
            'MsgBox("((" & CycleTime.Text & "*" & _Edit_Up_0.Text & ") /" & temppola & ") * 100")
            'MsgBox(sum_prg2 / cnt_btn)
            sum_prg2 = sum_prg2 / cnt_btn
            'MsgBox(sum_prg2)
            If sum_prg2 > 100 Then
                sum_prg2 = 100
            ElseIf sum_prg2 < 0 Then
                sum_prg2 = 0
            End If
            'MsgBox(sum_prg2)
            If sum_prg2 <= 49 Then
                CircularProgressBar2.ProgressColor = Color.Red
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 50 And sum_prg2 <= 79 Then
                CircularProgressBar2.ProgressColor = Color.Chocolate
                CircularProgressBar2.ForeColor = Color.Black
            ElseIf sum_prg2 >= 80 And sum_prg2 <= 100 Then
                CircularProgressBar2.ProgressColor = Color.Green
                CircularProgressBar2.ForeColor = Color.Black
            End If
            Dim avarage_eff As Double = Format(sum_prg2, "0.00")
            lb_sum_prg.Text = avarage_eff
            CircularProgressBar2.Text = sum_prg2 & "%"
            CircularProgressBar2.Value = sum_prg2
            Dim actCT_jingna As Double = Format(dtspan.Seconds + (dtspan.Minutes * 60), "0.00")
            Dim pd As String = MainFrm.Label6.Text
            Dim line_cd As String = MainFrm.Label4.Text
            Dim wi_plan As String = wi_no.Text
            Dim item_cd As String = Label3.Text
            Dim item_name As String = Label12.Text
            Dim staff_no As String = Label29.Text
            Dim seq_no As String = Label22.Text
            Dim prd_qty As Integer = cnt_btn
            Dim use_timee As Double = actCT_jingna
            Dim tr_status As String = "0"
            Dim number_qty As Integer = Label6.Text
            'Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, use_timee, tr_status)
            Dim result_use_time As Double = Cal_Use_Time_ins_qty_fn_manual(start_time2, end_time2)
            Try
                If My.Computer.Network.Ping(Backoffice_model.svp_ping) Then
                    tr_status = "1"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim indRow As String = itemPlanData.IND_ROW
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            Backoffice_model.Insert_prd_detail(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, start_time, end_time, result_use_time, number_qty, Spwi_id(j), tr_status)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        Backoffice_model.Insert_prd_detail(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, start_time, end_time, result_use_time, number_qty, pwi_id, tr_status)
                    End If
                Else
                    tr_status = "0"
                    If MainFrm.chk_spec_line = "2" Then
                        Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                        Dim Iseq = GenSEQ
                        Dim j As Integer = 0
                        For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                            Iseq += 1
                            Dim indRow As String = itemPlanData.IND_ROW
                            Dim special_wi As String = itemPlanData.wi
                            Dim special_item_cd As String = itemPlanData.item_cd
                            Dim special_item_name As String = itemPlanData.item_name
                            Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                            j = j + 1
                        Next
                    Else
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                    End If
                End If
            Catch ex As Exception
                tr_status = "0"
                If MainFrm.chk_spec_line = "2" Then
                    Dim GenSEQ As Integer = CInt(Label22.Text) - MainFrm.ArrayDataPlan.ToArray().Length
                    Dim Iseq = GenSEQ
                    Dim j As Integer = 0
                    For Each itemPlanData As DataPlan In Confrime_work_production.ArrayDataPlan
                        Iseq += 1
                        Dim indRow As String = itemPlanData.IND_ROW
                        Dim special_wi As String = itemPlanData.wi
                        Dim special_item_cd As String = itemPlanData.item_cd
                        Dim special_item_name As String = itemPlanData.item_name
                        Backoffice_model.insPrdDetail_sqlite(pd, line_cd, special_wi, special_item_cd, special_item_name, staff_no, Iseq, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, Spwi_id(j), modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                        j = j + 1
                    Next
                Else
                    Backoffice_model.insPrdDetail_sqlite(pd, line_cd, wi_plan, item_cd, item_name, staff_no, seq_no, prd_qty, number_qty, start_time2, end_time2, result_use_time, tr_status, pwi_id, modelMoldCavity.imc_id, modelMoldCavity.mm_id, Backoffice_model.gobal_seq_mold_no)
                End If
            End Try
            Dim sum_diff As Integer = Label8.Text - Label6.Text
            Label10.Text = "-" & sum_diff
            If sum_diff = 0 Then
                Me.Enabled = False
                comp_flg = 1
                'If result_mod <> "0" Then ' New
                'tag_print()
                'End If
                If Finish_work.Visible = False Then
                    Finish_work.Show() ' เกี่ยว
                End If
            End If
            If sum_diff < 1 Then
                If sum_diff = 0 Then
                    If check_format_tag = "1" Then ' for tag_type = '2' and tag_issue_flg = '2'  OR K1M183
                        lb_box_count.Text = lb_box_count.Text + 1
                        print_back.PrintDocument2.Print()
                        If result_mod = "0" Then
                            Label_bach.Text += 1
                            GoodQty = Label6.Text
                            'MsgBox("IF IF IF ====>" & GoodQty)
                            tag_print()
                        End If
                    Else
                        lb_box_count.Text = lb_box_count.Text + 1
                        'If Backoffice_model.check_line_reprint() = "0" Then
                        Label_bach.Text = Label_bach.Text + 1
                        GoodQty = Label6.Text
                        ' MsgBox("ELSE ====>" & GoodQty)
                        tag_print()
                        'End If
                    End If
                Else
                End If
                Me.Enabled = False
                comp_flg = 1
                ' MsgBox("IN NAJA")
                If Finish_work.Visible = False Then
                    Finish_work.Show() ' เกี่ยว
                End If
                Dim plan_qty As Integer = Label8.Text
                Dim shift_prd As String = Label14.Text
                Dim prd_st_datetime As Date = st_time.Text
                Dim prd_end_datetime As Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                Dim lot_no As String = Label18.Text
                Dim comp_flg2 As String = "1"
                Dim transfer_flg As String = "1"
                Dim del_flg As String = "0"
                Dim prd_flg As String = "1"
                Dim close_lot_flg As String = "1"
                Dim avarage_act_prd_time As Double = Average
            End If
        End If
    End Function
    Private Async Sub Tiemr_new_dio_Tick(sender As Object, e As EventArgs) Handles Timer_new_dio.Tick
        Console.WriteLine("check_bull=======>" & check_bull)
        Console.WriteLine("start_flg=======>" & start_flg)
        If rsWindow Then
            Try
                'ReadSingleSamplePortUInt32
                data_new_dio = counterNewDIO.reader_new_dio.ReadSingleSamplePortUInt32
                Console.WriteLine("data_new_dio.ToString=======>" & data_new_dio.ToString)
                If data_new_dio.ToString = "254" Then
                    If check_bull = 0 Then
                        'Timer3.Enabled = True
                        If start_flg = 1 Then
                            Console.WriteLine(data_new_dio)
                            Console.WriteLine("FA1 READY")
                            Await Manage_counter_NI_MAX()
                            Console.WriteLine("FA1 STOP")
                        End If
                    End If
                End If
            Catch ex As Exception
                Button1.Enabled = False
                btnStart.Enabled = False
                btn_back.Enabled = False
                Dim listdetail = "Please Check DIO"
                PictureBox10.BringToFront()
                PictureBox10.Show()
                PictureBox16.BringToFront()
                PictureBox16.Show()
                Panel2.BringToFront()
                Panel2.Show()
                Label2.Text = listdetail
                Label2.BringToFront()
                Label2.Show()
                Timer_new_dio.Stop()
            End Try
        End If
    End Sub
    Sub Main()
        Console.WriteLine("Main program started")
        ' Delay for 2 seconds using Task.Delay
        Try
            If Application.OpenForms().OfType(Of Loss_reg).Any Or Application.OpenForms().OfType(Of Loss_reg_pass).Any Then
            Else
                Dim dateTimeStart As String = Backoffice_model.date_time_click_start.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                Dim sec = DateTime.Now.ToString("ss")
                Dim convert_dateTimeStart As Date = dateTimeStart & ":" & sec
                Dim timeNow = DateTime.Now.ToString("HH:mm:ss")
                Dim dateTimeBreak As Date = DateTime.Now.ToString("yyyy-MM-dd" & " " & lbNextTime.Text, CultureInfo.InvariantCulture)
                Dim dateNow As Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                Dim total_loss As Integer = 1
                If lbNextTime.Text <> "" Then
                    Try
                        If lbNextTime.Text >= "00:00:00" And lbNextTime.Text <= "07:59:59" Then
                            total_loss = DateDiff(DateInterval.Second, dateNow, dateTimeBreak)
                            Console.WriteLine("dateNow--->" & dateNow)
                        Else
                            total_loss = DateDiff(DateInterval.Second, dateNow, dateTimeBreak)
                            Console.WriteLine("dateNow--->" & dateNow)
                        End If
                    Catch ex As Exception
                        dateTimeBreak = dateTimeBreak.AddDays(1)
                        total_loss = DateDiff(DateInterval.Second, dateNow, dateTimeBreak)
                    End Try
                    If total_loss < 0 Then
                        Dim currentDate As DateTime = DateTime.Now
                        ' Add one day to the current date
                        Dim nextDate As DateTime = currentDate.AddDays(1)
                        ' Format the next date as "yyyy-MM-dd"
                        Dim formattedDate As String = nextDate.ToString("yyyy-MM-dd")
                        dateTimeBreak = formattedDate & " " & lbNextTime.Text
                        total_loss = DateDiff(DateInterval.Second, dateNow, dateTimeBreak)
                    End If
                    Console.WriteLine(total_loss)
                    'Dim Delays As Integer = total_loss * 1000
                    Try
                        Console.WriteLine("C1 2 3 ")
                        Task.Delay(total_loss * 1000).ContinueWith(Sub(task)
                                                                       Try
                                                                           Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"))
                                                                           ' Bring the button to the front using Invoke
                                                                           check_network_frist = 1
                                                                           Me.Invoke(Sub()
                                                                                         Try
                                                                                             If Application.OpenForms().OfType(Of closeLotsummary).Any Then
                                                                                                 Dim BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text) ' for set data 
                                                                                                 Backoffice_model.ILogLossBreakTime(MainFrm.Label4.Text, wi_no.Text, Label22.Text)
                                                                                                 lbNextTime.Text = BreakTime
                                                                                                 Main()
                                                                                             ElseIf Application.OpenForms().OfType(Of Loss_reg).Any Or Application.OpenForms().OfType(Of Loss_reg_pass).Any Then
                                                                                                 Dim BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text) ' for set data 
                                                                                                 Backoffice_model.ILogLossBreakTime(MainFrm.Label4.Text, wi_no.Text, Label22.Text)
                                                                                                 lbNextTime.Text = BreakTime
                                                                                                 Main()
                                                                                             Else
                                                                                                 stop_working()
                                                                                                 Backoffice_model.TimeStartBreakTime = DateTime.Now.ToString("HH:mm:ss")
                                                                                                 If check_in_up_seq = 0 Then
                                                                                                     Backoffice_model.IDLossCodeAuto = "36"
                                                                                                 End If
                                                                                                 StopMenu.Show()
                                                                                                 Me.Enabled = False
                                                                                             End If
                                                                                         Catch ex As Exception

                                                                                         End Try
                                                                                     End Sub)
                                                                       Catch ex As Exception

                                                                       End Try
                                                                   End Sub, TaskScheduler.FromCurrentSynchronizationContext())
                    Catch ex As Exception
                    End Try
                    Console.WriteLine("C2")
                    ' Create a delayed task using Task.Delay
                    ' Wait for user input to prevent the console from closing immediately
                    'Console.WriteLine("Press Enter to exit...")
                    'Console.ReadLine()
                End If
            End If
        Catch
        End Try
    End Sub
    Private Sub TimerCountBT_Tick(sender As Object, e As EventArgs)
        Try
            If Application.OpenForms().OfType(Of Loss_reg).Any Or Application.OpenForms().OfType(Of Loss_reg_pass).Any Then
            ElseIf Application.OpenForms().OfType(Of closeLotsummary).Any Then
                If TimeOfDay.ToString("HH:mm:ss") = Backoffice_model.TimeShowBreakTime Then
                    Dim BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text) ' for set data 
                    Backoffice_model.ILogLossBreakTime(MainFrm.Label4.Text, wi_no.Text, Label22.Text)
                    lbNextTime.Text = BreakTime
                End If
            ElseIf Application.OpenForms().OfType(Of closeLotsummary).Any Then
                If TimeOfDay.ToString("HH:mm:ss") = Backoffice_model.TimeShowBreakTime Then
                    Dim BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text) ' for set data 
                    Backoffice_model.ILogLossBreakTime(MainFrm.Label4.Text, wi_no.Text, Label22.Text)
                    lbNextTime.Text = BreakTime
                End If
            Else
                If Backoffice_model.TimeShowBreakTime <> "" Then
                    If TimeOfDay.ToString("HH:mm:ss") = Backoffice_model.TimeShowBreakTime Then
                        check_network_frist = 1
                        stop_working()
                        Backoffice_model.TimeStartBreakTime = TimeOfDay.ToString("HH:mm:ss")
                        StopMenu.Show()
                        Me.Enabled = False
                    End If
                Else
                    ' TimerLossBT.Enabled = False
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function calTimeBreakTime(pdStart As Date, timeNextBreak As String)
        Dim total_loss As Integer = 0
        Dim dateNow As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim dueDateTime As Date = dateNow & " " & timeNextBreak
        Dim condueDateTime As Date = dueDateTime.ToString("yyyy-MM-dd HH:mm:ss")
        Try
            total_loss = DateDiff(DateInterval.Second, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse((condueDateTime).ToString("yyyy-MM-dd HH:mm") & ":00"))
            If total_loss < 0 Then
                total_loss = Math.Abs(CDbl(Val(total_loss)))
            End If
        Catch ex As Exception
            total_loss = 0
        End Try
        Return total_loss
    End Function
    Private Sub TimerLossBT_Tick(sender As Object, e As EventArgs)
        Try
            If Application.OpenForms().OfType(Of Loss_reg).Any Or Application.OpenForms().OfType(Of Loss_reg_pass).Any Then

            ElseIf Application.OpenForms().OfType(Of closeLotsummary).Any Then
                If TimeOfDay.ToString("HH:mm:ss") = Backoffice_model.TimeShowBreakTime Then
                    Dim BreakTime = Backoffice_model.GetTimeAutoBreakTime(MainFrm.Label4.Text) ' for set data 
                    Backoffice_model.ILogLossBreakTime(MainFrm.Label4.Text, wi_no.Text, Label22.Text)
                    lbNextTime.Text = BreakTime
                End If
            Else
                If Backoffice_model.TimeShowBreakTime <> "" Then
                    If TimeOfDay.ToString("HH:mm:ss") = Backoffice_model.TimeShowBreakTime Then
                        check_network_frist = 1
                        stop_working()
                        Backoffice_model.TimeStartBreakTime = TimeOfDay.ToString("HH:mm:ss")
                        StopMenu.Show()
                        Me.Enabled = False
                    End If
                Else
                    ' TimerLossBT.Enabled = False
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click

    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

    End Sub
    Private Sub lb_ref_scan_Click(sender As Object, e As EventArgs) Handles lb_ref_scan.Click

    End Sub
    Private Sub CircularProgressBar2_Click(sender As Object, e As EventArgs) Handles CircularProgressBar2.Click

    End Sub
    Private Sub Label23_Click(sender As Object, e As EventArgs) Handles Label23.Click

    End Sub
    Private Sub PictureBox10_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub PictureBox13_Click(sender As Object, e As EventArgs) Handles panelpcWorker1.Click
        Dim work = New Show_Worker
        work.Show()
    End Sub
    Public Shared Function CheckOs()
        Dim arr_os = My.Computer.Info.OSFullName.Split(" ")
        If arr_os(2).ToString = "7" Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub PictureBox14_Click(sender As Object, e As EventArgs) Handles PictureBox14.Click, btnInfo.Click
        'Dim showD = New show_detail_production
        Me.Enabled = False
        show_detail_production.Show()
    End Sub
    Private Sub PictureBox16_Click(sender As Object, e As EventArgs) Handles PictureBox16.Click
        PictureBox10.Hide()
        PictureBox16.Hide()
        Panel2.Hide()
        panelpcWorker1.Enabled = True
        btnInfo.Enabled = True
        Button1.Enabled = True
        btnStart.Enabled = True
        btn_back.Enabled = True
    End Sub
    Private Sub TIME_CAL_EFF_Tick(sender As Object, e As EventArgs) Handles TIME_CAL_EFF.Tick
        'If check_cal_eff = 1000 Then
        'cal_eff()
        'check_cal_eff = 1
        'Else
        'check_cal_eff = check_cal_eff + 1
        'End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Main()
    End Sub
    Private Sub ReceivedText(ByVal [text] As String)
        If Me.InvokeRequired Then
            Dim x As New SetTextCallback(AddressOf ReceivedText)
            Me.Invoke(x, New Object() {(text)})
        Else
            BeginInvoke(Sub()
                            Console.WriteLine("RES check_bull===>", check_bull)
                            If check_bull = 0 Then
                                Manage_counter_contect_DIO_RS232()
                            End If
                        End Sub)
        End If
        Console.WriteLine("Read Data Success")
    End Sub
    Private Sub serialPort_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles serialPort.DataReceived
        ' ทำอะไรกับข้อมูลที่ได้รับ RS232 อ่านค่า
        ReceivedText(serialPort.ReadExisting())
        Console.WriteLine("READ OK")
        '    Dim data As String = serialPort.ReadExisting()
    End Sub
    Private Sub serialPort_PinChanged(sender As Object, e As SerialPinChangedEventArgs) Handles serialPort.PinChanged
        ' การตรวจสอบการเปลี่ยนแปลงของ CTS หรือ DSR
        If e.EventType = SerialPinChange.CtsChanged Then
            If serialPort.CtsHolding Then
                Console.WriteLine("1 High OK")
            Else
                Console.WriteLine("1 LOW")
            End If
        ElseIf e.EventType = SerialPinChange.DsrChanged Then
            If serialPort.DsrHolding Then
                '   Console.WriteLine("2 High OK")
            Else
                '  Console.WriteLine("2 LOW")
            End If
        End If
    End Sub
    Public Sub TowerLamp(DataBits As Integer, WriteLine As Integer)
        Try
            If comportTowerLamp <> "NO DEVICE" Then
                If SerialPortLamp.IsOpen() Then
                    SerialPortLamp.Close()
                End If
                Try
                    SerialPortLamp.PortName = comportTowerLamp
                    SerialPortLamp.BaudRate = 9600
                    SerialPortLamp.Parity = IO.Ports.Parity.None
                    SerialPortLamp.StopBits = IO.Ports.StopBits.One
                    SerialPortLamp.DataBits = DataBits
                    SerialPortLamp.Open()
                    SerialPortLamp.WriteLine(WriteLine)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If
        Catch ex As Exception
            MsgBox("Please Check Comport For Tower Lamp In Server ")
        End Try
    End Sub
    Public Sub ResetRed()
        Try
            If comportTowerLamp <> "NO DEVICE" Then
                If SerialPortLamp.IsOpen() Then
                    SerialPortLamp.Close()
                End If
                SerialPortLamp.PortName = comportTowerLamp
                SerialPortLamp.BaudRate = 9600
                SerialPortLamp.Parity = IO.Ports.Parity.None
                SerialPortLamp.StopBits = IO.Ports.StopBits.One
                SerialPortLamp.DataBits = 5
                SerialPortLamp.Open()
                SerialPortLamp.WriteLine(9999)
                Console.ReadLine()
                If SerialPortLamp.IsOpen() Then
                    SerialPortLamp.Close()
                End If
                SerialPortLamp.PortName = comportTowerLamp
                SerialPortLamp.BaudRate = 9600
                SerialPortLamp.Parity = IO.Ports.Parity.None
                SerialPortLamp.StopBits = IO.Ports.StopBits.One
                SerialPortLamp.DataBits = 6
                SerialPortLamp.Open()
                SerialPortLamp.WriteLine(11)
                Console.ReadLine()
            End If
        Catch ex As Exception
            MsgBox("Please Check Comport For Tower Lamp In Server ")
        End Try
    End Sub
    Public Sub Load_Working_OEE()
        Working_OEE.Show()
    End Sub
    Private Sub PictureBox11_Click(sender As Object, e As EventArgs) Handles redBox.Click
    End Sub
    Private Sub lvA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvA.SelectedIndexChanged
    End Sub
    Private Sub btn_desc_act_Click_1(sender As Object, e As EventArgs)
    End Sub
    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Start_Production()
    End Sub
    Public Shared Async Function updateShot(mm_id As Integer, pwi_id As Integer, ima_use_shot As Integer, ima_type_actual As String, ima_start_date As DateTime, ima_end_date As DateTime, ima_status_flg As Integer, emp_code As String, line_cd As String) As Task(Of Integer)
        Try
            Dim mdmold = New modelMoldCavity()
            Dim maxShort As Integer = modelMoldCavity.mcheckMaxMold(mm_id)
            mdmold.mupdateShot(mm_id, pwi_id, ima_use_shot, ima_type_actual, ima_start_date, ima_end_date, ima_status_flg, emp_code, line_cd)
            Dim rs = modelMoldCavity.mCheckLifetime(mm_id, MainFrm.Label4.Text, modelMoldCavity.shortTest)
            If CDbl(Val(modelMoldCavity.shortTest)) >= maxShort And check_in_up_seq = 0 Then
                MsgBox("Please Check Mold & Cavity.")
                moldCalculator.lbMax.Text = modelMoldCavity.mcheckMaxMold(modelMoldCavity.mm_id)
                Prd_detail.Show()
                Prd_detail.Test_short.Enabled = False
                Prd_detail.Enabled = True
                Return 1
            End If
            Return 0
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
            Return -1
        End Try
    End Function

    Public Sub GenQrScanChecklist(line_cd As String)
        ' สร้าง QR Code generator
        Dim qrGenerator As New QRCodeGenerator()
        ' สร้าง QR Code จาก URL หรือข้อความที่ต้องการ
        Dim url As String = "http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/CheckList/form_Checklist?line_cd=" & line_cd
        Console.WriteLine(url)
        Dim qrCodeData As QRCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q)
        ' สร้าง QR Code
        Dim qrCode As New QRCode(qrCodeData)
        ' ปรับขนาด QR Code ที่สร้างขึ้น
        Dim qrCodeImage As Bitmap = qrCode.GetGraphic(6) 'ปรับค่าที่นี่เพื่อทำให้ QR Code ใหญ่ขึ้น
        ' แสดง QR Code ใน PictureBox
        qrScanChecklist.Image = qrCodeImage
        ' ปรับขนาด Panel เพื่อให้พอดีกับ QR Code
        qrScanChecklist.Location = New Point(15, 9) ' กำหนดตำแหน่ง
        qrScanChecklist.Size = New Size(330, 330)   ' ปรับขนาด Panel ให้ใหญ่ขึ้นตาม QR Code
        PanelQrScanChecklist.Visible = True
        PanelQrScanChecklist.Location = New Point(183, 101)
        PanelQrScanChecklist.Size = New Size(353, 349)
    End Sub
    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        ' WebViewProgressbar.Reload()
    End Sub
    Private Sub btnDefects_Click(sender As Object, e As EventArgs) Handles btnDefects.Click
        Dim dfHome = New defectHome()
        dfHome.Show()
        'defectHome.Show()
        Me.Enabled = False
        'Close_lot.Show()
        'Me.Close()
    End Sub
    Private Sub PanelQrScanChecklist_Paint(sender As Object, e As PaintEventArgs) Handles PanelQrScanChecklist.Paint
    End Sub
End Class