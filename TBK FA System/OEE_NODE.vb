Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Globalization
Imports System.Data
Imports System.Web.Script.Serialization
Public Class OEE_NODE
    Public Shared Function OEE_LOAD_MSTOEE(line_cd As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://192.168.161.78:6100/api/dataGetmstOEE?line_cd=" & line_cd)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGetmstOEE?line_cd=" & line_cd)
            Dim dcResultdata As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(jsonString)
            Dim i As Integer = 1
            For Each item As Object In dcResultdata
                If item("moe_min_oee").ToString Is Nothing Then
                    ' MsgBox("IF")
                Else
                    'MsgBox("ELSE")
                End If
            Next
            Return dcResultdata
        Catch ex As Exception
            MsgBox("Please Check Master OEE In Table line_mst")
            ' MsgBox("ERROR OEE FUNCTION OEE_GET_Hour Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_GET_NEW_TARGET(st_shift As String, end_shift As String, std_ct As String, shift As String)
        Try

            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            Else
                If shift = "S" Or shift = "Q" Or shift = "B" Then
                    date_end = date_now_date.AddDays(1)
                    date_end = Convert.ToDateTime(date_end).ToString("yyyy-MM-dd")
                End If
                '   If shift = "S" Or shift = "Q" Then
                ' date_end = date_now_date.AddDays(1)
                ' date_end = Convert.ToDateTime(date_end).ToString("yyyy-MM-dd")
                'End If
            End If
            date_st = date_st
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            ' Dim TarGet = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/NEW_GET_TARGET?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct)
            '   Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/NEW_GET_TARGET?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct)
            Console.WriteLine("http://192.168.161.78:6100/api/dataGettarget?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct & "&date_start=" & convertDateStart & "&date_end=" & date_end)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGettarget?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct & "&date_start=" & convertDateStart & "&date_end=" & date_end)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim TarGet As Integer = data("Target").ToString
            'Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/NEW_GET_TARGET?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct)
            Return TarGet
        Catch ex As Exception
            'MsgBox("ERROR OEE FUNCTION OEE_GET_NEW_TARGET Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_GET_Hour(shift As String)
        Try
            Dim api = New api()
            ' Dim TarGet = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/NEW_GET_TARGET?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct)
            '   Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/NEW_GET_TARGET?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGetHour?shift=" & shift)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim TarGet As Double = data("WorkHour").ToString
            Console.WriteLine("http://192.168.161.78:6100/api/dataGetHour?shift=" & shift)
            'Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/NEW_GET_TARGET?st_shift=" & st_shift & "&end_shift=" & end_shift & "&std_ct=" & std_ct)
            Return TarGet
        Catch ex As Exception
            ' MsgBox("ERROR OEE FUNCTION OEE_GET_Hour Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_getProduction_actual_detailByHour(line_cd As String, minSwitchModel As Integer)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim dateTimeend As Date
            dateTimeend = date_end & " " & time
            '  Dim newDateMinutes As DateTime = dateTimeend.AddMinutes(-60)
            Dim newDateMinutes As DateTime = dateTimeend.AddMinutes(-minSwitchModel)
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Dim convertDateCrr = Convert.ToDateTime(date_now_date).ToString("yyyy-MM-dd")
            Dim convertnewDateMinutes = Convert.ToDateTime(newDateMinutes).ToString("yyyy-MM-dd HH:mm:ss")
            '  MsgBox("convertDateCrr======>" & convertDateCrr)
            '  MsgBox("convertnewDateMinutes======>" & convertnewDateMinutes)
            Console.WriteLine("http://192.168.161.78:6100/api/dataDetailByHouse?line_cd=" & line_cd & "&date_crr=" & convertDateCrr & "&time_crr=" & time & "&convertnewDateMinutes=" & convertnewDateMinutes)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataDetailByHouse?line_cd=" & line_cd & "&date_crr=" & convertDateCrr & "&time_crr=" & time & "&convertnewDateMinutes=" & convertnewDateMinutes)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim rs As Integer = data("TotalByHour").ToString
            Return rs
        Catch ex As Exception
            'MsgBox("ERROR OEE FUNCTION OEE_getProduction_actual_detailByHour Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_getProduction_actual_detailByShift(line_cd As String, shift As String)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            Else
                ' If time_now > "20:00:00 AM" Then
                If shift = "S" Or shift = "Q" Or shift = "B" Then
                    date_end = date_now_date.AddDays(1)
                    date_end = Convert.ToDateTime(date_end).ToString("yyyy-MM-dd")
                End If
            End If
            date_st = date_st
            Dim dateTimeend As Date
            dateTimeend = date_end & " " & time
            Dim newDateMinutes As DateTime = dateTimeend.AddMinutes(-60)
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Dim convertDateCrr = Convert.ToDateTime(date_now_date).ToString("yyyy-MM-dd")
            Dim convertDateEnd = Convert.ToDateTime(date_end).ToString("yyyy-MM-dd")
            Dim convertnewDateMinutes = Convert.ToDateTime(newDateMinutes).ToString("yyyy-MM-dd HH:mm:ss")
            '  MsgBox("convertDateCrr======>" & convertDateCrr)
            '  MsgBox("convertnewDateMinutes======>" & convertnewDateMinutes)
            Dim st_time = Prd_detail.Label12.Text.Substring(3, 5) & ":00"
            Dim end_time = Prd_detail.Label12.Text.Substring(11, 5) & ":00"
            Console.WriteLine("http://192.168.161.78:6100/api/dataDetailByShift?line_cd=" & line_cd & "&date_start=" & convertDateStart & "&date_end=" & convertDateEnd & "&st_shift=" & st_time & "&end_shift=" & end_time)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataDetailByShift?line_cd=" & line_cd & "&date_start=" & convertDateStart & "&date_end=" & convertDateEnd & "&st_shift=" & st_time & "&end_shift=" & end_time)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim rs As Integer = data("TotalActualDetail").ToString
            Return rs
        Catch ex As Exception
            ' MsgBox("ERROR OEE FUNCTION OEE_getProduction_actual_detailByShift Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function GetDataProgressbarA(st_shift As String, end_shift As String, line_cd As String, dateTimeswmodel As String, statusSwitchModel As String, IsOnlyone As String)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim dateTimeend As Date
            dateTimeend = date_end & " " & time
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Dim convertDateCrr = Convert.ToDateTime(date_now_date).ToString("yyyy-MM-dd")
            Console.WriteLine("http://192.168.161.78:6100/api/dataProgressA?st_shift=" & st_shift & "&end_shift=" & end_shift & "&line_cd=" & line_cd & "&date_start=" & convertDateStart & "&date_Crr=" & convertDateCrr & "&TimeCrr=" & time & "&dateTimeswmodel=" & dateTimeswmodel & "&statusSwitchModel=" & statusSwitchModel & "&IsOnlyone=" & IsOnlyone)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataProgressA?st_shift=" & st_shift & "&end_shift=" & end_shift & "&line_cd=" & line_cd & "&date_start=" & convertDateStart & "&date_Crr=" & convertDateCrr & "&TimeCrr=" & time & "&dateTimeswmodel=" & dateTimeswmodel & "&statusSwitchModel=" & statusSwitchModel & "&IsOnlyone=" & IsOnlyone)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim rs As Integer = data("PercentA").ToString
            Return rs
        Catch ex As Exception
            ' MsgBox("ERROR OEE FUNCTION GetDataProgressbarA Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_GetLossByHouseP1(line_cd As String)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim dateTimeend As Date
            dateTimeend = date_end & " " & time
            Dim newDateMinutes As DateTime = dateTimeend.AddMinutes(-60)
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Dim convertnewDateMinutes = Convert.ToDateTime(newDateMinutes).ToString("yyyy-MM-dd HH:mm:ss")
            ' MsgBox("convertnewDateMinutes ===>" & convertnewDateMinutes)
            Console.WriteLine("http://192.168.161.78:6100/api/dataGetlossbyhouse?line_cd=" & line_cd & "&date_start=" & date_st & "&date_end=" & date_end & "&time_crr=" & time & "&convertnewDateMinutes=" & convertnewDateMinutes)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGetlossbyhouse?line_cd=" & line_cd & "&date_start=" & date_st & "&date_end=" & date_end & "&time_crr=" & time & "&convertnewDateMinutes=" & convertnewDateMinutes)
            ' Deserialize the JSON string to a Dictionary
            Dim jsSerializer As New JavaScriptSerializer()
            Dim data As List(Of Dictionary(Of String, Object)) = jsSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(jsonString)
            ' Access the integer value from the first dictionary in the list
            Dim targetValue As Integer = Convert.ToInt32(data(0)("TotalLoss"))
            ' Retrieve the integer value from the dictionary
            ' Access the value
            Dim rs As Integer = targetValue
            Return rs
        Catch ex As Exception
            'MsgBox("ERROR OEE FUNCTION OEE_GetLossByHouseP1 Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_GET_Data_LOSS(line_cd As String, lot_no As String, shift As String, dateStart As String, dateTimeswModel As String, statusSwitchModel As String, IsOnlyone As String)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            'MsgBox("date_st ===>" & convertDateStart)
            'MsgBox("dateTime_end ===>" & date_end)
            'MsgBox("time = " & time)
            Dim st_time = Prd_detail.Label12.Text.Substring(3, 5) & ":00"
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGetDataAvailabillty?line_cd=" & line_cd & "&lot_no=" & lot_no & "&shift=" & shift & "&dateStart=" & convertDateStart & "&dateEnd=" & date_end & "&st_shift=" & st_time & "&end_shift=" & time & "&dateTimeswModel=" & dateTimeswModel & "&statusSwitchModel=" & statusSwitchModel & "&IsOnlyone=" & IsOnlyone)
            Console.WriteLine("http://192.168.161.78:6100/api/dataGetDataAvailabillty?line_cd=" & line_cd & "&lot_no=" & lot_no & "&shift=" & shift & "&dateStart=" & convertDateStart & "&dateEnd=" & date_end & "&st_shift=" & st_time & "&end_shift=" & time & "&dateTimeswModel=" & dateTimeswModel & "&statusSwitchModel=" & statusSwitchModel & "&IsOnlyone=" & IsOnlyone)
            Console.WriteLine(jsonString)
            Return jsonString
        Catch ex As Exception
            ' MsgBox("ERROR OEE FUNCTION OEE_GET_Data_LOSS Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_GET_Data_AccTarget(st_shift As String, std_ct As String)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/datagetAccTarget?st_shift=" & st_shift & "&std_ct=" & std_ct & "&dateStart=" & convertDateStart & "&dateEnd=" & date_end & "&end_shift=" & time)
            Console.WriteLine("http://192.168.161.78:6100/api/datagetAccTarget?st_shift=" & st_shift & "&std_ct=" & std_ct & "&dateStart=" & convertDateStart & "&dateEnd=" & date_end & "&end_shift=" & time)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim rs As Integer = data("ActualTarget").ToString
            Console.WriteLine("http: //192.168.161.78:6100/api/datagetAccTarget?st_shift=" & st_shift & "&std_ct=" & std_ct)
            Return rs
        Catch ex As Exception
            'MsgBox("ERROR OEE FUNCTION OEE_GET_Data_AccTarget Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_getSpeedLoss(NG As String, Good As String, Timeshift As String, std_cd As String)
        Try
            Dim api = New api()
            Dim jsonString = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/getSpeedLoss?NG=" & NG & "&Good=" & Good & "&Timeshift=" & Timeshift & "&std_cd=" & std_cd)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim rs As Integer = data("ActualTarget")
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/GET_OEE/getSpeedLoss?NG=" & NG & "&Good=" & Good & "&Timeshift=" & Timeshift & "&std_cd=" & std_cd)
            Return rs
        Catch ex As Exception
            ' MsgBox("ERROR OEE FUNCTION OEE_getSpeedLoss Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_getWorkingTime(line_cd As String, Timeshift As String)
        Try
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Console.WriteLine("http://192.168.161.78:6100/api/dataGetWorkingTime?line_cd=" & line_cd & "&st_shift=" & Timeshift & "&date_crr=" & date_end & "&time_crr=" & time & "&dates_start=" & convertDateStart)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGetWorkingTime?line_cd=" & line_cd & "&st_shift=" & Timeshift & "&date_crr=" & date_end & "&time_crr=" & time & "&dates_start=" & convertDateStart)
            Dim jsSerializer As New JavaScriptSerializer()
            ' Deserialize the JSON string to a Dictionary
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            ' Access the value
            Dim rs As Integer = data("rs").ToString
            Return rs
        Catch ex As Exception
            ' MsgBox("ERROR OEE FUNCTION OEE_getWorkingTime Please Check API")
            load_show.Show()
        End Try
    End Function
    Public Shared Function OEE_getDateTimeStart(st_shift As String, line_cd As String)
        Try
ReConnect:
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Console.WriteLine("http://192.168.161.78:6100/api/dataDataTimestart?st_shift=" & st_shift & "&line_cd=" & line_cd & "&date_start=" & convertDateStart & "&dateCurr=" & date_end & "&TimeCurr=" & time)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataDataTimestart?st_shift=" & st_shift & "&line_cd=" & line_cd & "&date_start=" & convertDateStart & "&dateCurr=" & date_end & "&TimeCurr=" & time)
            Dim jsSerializer As New JavaScriptSerializer()
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            Dim rs As String = data("formattedDateTime").ToString
            Return rs
        Catch ex As Exception
            GoTo ReConnect
        End Try
    End Function


    Public Shared Function OEE_getDataGetWorkingTimeModel(st_shift As String, line_cd As String, item_cd As String)
        Try
ReConnect:
            Dim api = New api()
            Dim date_now_date As Date = DateTime.Now.ToString("yyyy-MM-dd")
            ' Dim time As Date = TimeOfDay.ToString("HH:mm:ss") 'DateTime.Now.ToString("HH:mm:ss")
            Dim time As String = DateTime.Now.ToString("HH:mm:ss")
            Dim date_st = DateTime.Now.ToString("yyyy-MM-dd")
            Dim date_end = DateTime.Now.ToString("yyyy-MM-dd")
            Dim time_now As String = DateTime.Now.ToString("HH:mm:ss tt")
            If time_now >= "00:00:00 AM" And time_now <= "08:00:00 AM" Then
                date_st = date_now_date.AddDays(-1)
            End If
            date_st = date_st
            Dim convertDateStart = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
            Console.WriteLine("http://192.168.161.78:6100/api/dataGetWorkingTimeModel?st_shift=" & st_shift & "&line_cd=" & line_cd & "&dates_start=" & convertDateStart & "&date_crr=" & date_end & "&time_crr=" & time & "&item_cd=" & item_cd)
            Dim jsonString = api.Load_data("http://192.168.161.78:6100/api/dataGetWorkingTimeModel?st_shift=" & st_shift & "&line_cd=" & line_cd & "&dates_start=" & convertDateStart & "&date_crr=" & date_end & "&time_crr=" & time & "&item_cd=" & item_cd)
            Dim jsSerializer As New JavaScriptSerializer()
            Dim data As Dictionary(Of String, Object) = jsSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
            Dim st_time As String = data("rs").ToString
            Return data
        Catch ex As Exception
            GoTo ReConnect
        End Try
    End Function
End Class