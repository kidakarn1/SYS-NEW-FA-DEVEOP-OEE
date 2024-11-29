Imports System.Web.Script.Serialization
Imports NodaTime
Public Class OEE_SQLITE
    Public Shared Function GetTimeWorkingByModel(st_shift As String, line_cd As String, date_start As String, dateCurr As String, TimeCurr As String)
        Dim api As New api
        Dim dateTimestart = date_start + " " + st_shift + ":00"
        Dim dateTimeEnd = dateCurr + " " + TimeCurr
        Try
            Dim Sql = "SELECT top 1 * from production_actual_detail where st_time between '" & dateTimestart & "' and '" & dateTimeEnd & "' and item_cd = '" & line_cd & "'"
            Console.WriteLine(Sql)
            Dim jsonData As String = api.Load_dataSQLite(Sql)
            Return jsonData
        Catch ex As Exception
            MsgBox("Error Files OEE_SQLITE In Function GetTimeWorkingByModel")
        End Try
    End Function
    Public Shared Function GetDataProgressbarA(st_shift As String, end_shift As String, line_cd As String, dateTimeswmodel As String, statusSwitchModel As String, IsOnlyone As String, SpecFlgLine As String)
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
        Dim dateTimeend As String
        Dim date_start = Convert.ToDateTime(date_st).ToString("yyyy-MM-dd")
        Dim date_Crr = Convert.ToDateTime(date_now_date).ToString("yyyy-MM-dd")
        Dim timeCurr As Integer = DateTime.Now.Hour
        Dim dateTimestart As String
        If statusSwitchModel = 0 Then
            dateTimestart = date_start & " " & st_shift & ":00"
        ElseIf statusSwitchModel = 1 OrElse statusSwitchModel = 2 Then
            If IsOnlyone = "1" Then
                dateTimestart = date_start & " " & st_shift & ":00"
            Else
                dateTimestart = dateTimeswmodel
            End If
        End If
        date_Crr = Convert.ToDateTime(date_Crr).ToString("yyyy-MM-dd")
        dateTimeend = date_Crr & " " & end_shift & ":00"
        If String.Compare(end_shift, "00:00") >= 0 AndAlso String.Compare(end_shift, "08:00") <= 0 Then
            Dim convertDateStart As DateTime = DateTime.Parse(date_start)
            convertDateStart = convertDateStart.AddDays(1)
            Dim newDateStr As String = convertDateStart.ToString("yyyy-MM-dd")
            dateTimeend = newDateStr & " " & end_shift & ":00"
        End If
        dateTimeend = DateTime.ParseExact(dateTimeend, "yyyy-MM-dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss")
        Dim diffdatesec As Long = DateDiff(DateInterval.Second, DateTime.Parse(dateTimestart), DateTime.Parse(dateTimeend))
        Dim breakTime = 20
        Dim lunch = 0
        Dim Dinnerbrak = 0
        If diffdatesec >= 3600 Then ' > 1 Hr
            If timeCurr >= 12 OrElse timeCurr >= 24 Then
                lunch = 40
            End If
            If end_shift = "20:00" OrElse end_shift = "08:00" Then
                If timeCurr >= 12 OrElse timeCurr >= 24 Then
                    Dinnerbrak = 30
                End If
            End If
        Else
            lunch = 0
            Dinnerbrak = 0
        End If
        Dim Allbreak = breakTime + lunch + Dinnerbrak
        Dim diffdatemin = (diffdatesec / 60) - Allbreak
        Dim loadingTime As Integer
        Dim SqlString As String = "SELECT COALESCE(SUM(loss_time), 0) AS totalLoss FROM (
                                                    SELECT la.start_loss, la.loss_time
                                                    FROM loss_actual AS la
                                                    JOIN sys_loss_mst AS slm ON la.loss_cd_id = slm.id
                                                    WHERE la.line_cd = '" & line_cd & "'
                                                    AND la.start_loss BETWEEN '" & dateTimestart & "' AND '" & dateTimeend & "'
                                                    AND slm.loss_cd IN ('A', 'B', 'T', 'D', 'P1')
                                                    GROUP BY la.start_loss, la.loss_time
                                                ) AS unique_losses"
        Console.WriteLine(SqlString)
        Dim jsonData As String = api.Load_dataSQLite(SqlString)
        '  If jsonData <> "0" Then
        Dim dcResultdata As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(jsonData)
        For Each items As Object In dcResultdata
            loadingTime = items("totalLoss").ToString()
        Next
        '   End If
        Dim SqlString2 As String
        If SpecFlgLine <> 2 Then
            SqlString2 = "SELECT
            COALESCE(SUM(loss_time), 0) AS totalLoss
        FROM
            (SELECT la.start_loss, la.loss_time
            FROM loss_actual AS la
            JOIN sys_loss_mst AS slm ON la.loss_cd_id = slm.id
            WHERE la.line_cd = '" & line_cd & "'
            AND la.start_loss  BETWEEN '" & dateTimestart & "' AND'" & dateTimeend & "'
            AND slm.loss_cd NOT IN ('A', 'B', 'T', 'D', 'P1', 'X')
            ) AS distinct_losses"
        Else
            SqlString2 = "SELECT
            COALESCE(SUM(loss_time), 0)  / 5 AS totalLoss
        FROM
            (SELECT la.start_loss, la.loss_time
            FROM loss_actual AS la
            JOIN sys_loss_mst AS slm ON la.loss_cd_id = slm.id
            WHERE la.line_cd = '" & line_cd & "'
            AND la.start_loss  BETWEEN '" & dateTimestart & "' AND'" & dateTimeend & "'
            AND slm.loss_cd NOT IN ('A', 'B', 'T', 'D', 'P1', 'X')
            ) AS distinct_losses"
        End If
        Console.WriteLine(SqlString2)
        Dim jsonData2 As String = api.Load_dataSQLite(SqlString)
        Dim OperatingTime As Integer = 0
        ' If jsonData2 <> "0" Then
        Dim dcResultdata2 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(jsonData2)
        For Each items As Object In dcResultdata2
            OperatingTime = loadingTime - items("totalLoss").ToString()
        Next
        ' End If
        Dim rs As Double
        Dim result As Double = OperatingTime / loadingTime
        If Double.IsNaN(result) Then
            rs = Math.Floor(1 * 100)
        Else
            rs = Math.Floor(result * 100)
        End If
        Dim rsTotal = Math.Floor(rs)
        Return rsTotal
    End Function
    Public Shared Function OEE_GET_Data_LOSS(line_cd As String, lot_no As String, shift As String, dateStart As String, dateTimeswModel As String, statusSwitchModel As String, IsOnlyone As String, SpecFlgLine As String)
        Dim start_dateTime As String
        If statusSwitchModel = 0 Then
            start_dateTime = dateStart & " " & st_shift
        ElseIf statusSwitchModel = 1 Or statusSwitchModel = 2 Then
            If IsOnlyone = "1" Then
                start_dateTime = dateStart & " " & st_shift
            Else
                start_dateTime = dateTimeswModel
            End If
        End If
        Dim end_dateTime As String = dateEnd & " " & end_shift
        Dim result As DataTable
        Dim chk_spec_line As Integer = SpecFlgLine
        Dim sqlQuery As String = ""
        If chk_spec_line = 0 Or chk_spec_line = 1 Then
            ' Normal case
            sqlQuery = "
        SELECT TOP 3 slm.loss_cd,
                     SUM(la.loss_time) AS lossTime,
                     (SELECT SUM(la2.loss_time)
                      FROM loss_actual AS la2
                           INNER JOIN production_working_info AS pwi2 ON pwi2.pwi_id = la2.pwi_id
                      WHERE pwi2.pwi_lot_no = '" & lot_no & "'
                        AND la2.line_cd = '" & line_cd & "'
                        AND la2.start_loss BETWEEN '" & start_dateTime & "' AND '" & end_dateTime & "'
                        AND la2.loss_cd_id != '36'
                        AND la2.loss_cd_id != '35') AS AllLossTime
        FROM loss_actual AS la
             INNER JOIN production_working_info AS pwi ON pwi.pwi_id = la.pwi_id
             INNER JOIN sys_loss_mst AS slm ON la.loss_cd_id = slm.id
        WHERE pwi.pwi_lot_no =  '" & lot_no & "'
          AND la.line_cd ='" & line_cd & "'
          AND la.start_loss BETWEEN  '" & start_dateTime & "' AND '" & end_dateTime & "'
          AND la.loss_cd_id != '36'
          AND la.loss_cd_id != '35'
        GROUP BY slm.loss_cd
        ORDER BY SUM(la.loss_time) DESC"
        Else
            ' Special case
            start_dateTime = dateStart & " " & st_shift
            sqlQuery = "
        SELECT TOP 3 slm.loss_cd,
                     SUM(la.loss_time) / 5 AS lossTime,
                     (SELECT SUM(la2.loss_time) / 5 
                      FROM loss_actual AS la2
                           INNER JOIN production_working_info AS pwi2 ON pwi2.pwi_id = la2.pwi_id
                           WHERE pwi2.pwi_lot_no = '" & lot_no & "'
                        AND la2.line_cd = '" & line_cd & "'
                        AND la2.start_loss BETWEEN '" & start_dateTime & "' AND '" & end_dateTime & "'
                        AND la2.loss_cd_id != '36'
                        AND la2.loss_cd_id != '35') AS AllLossTime
        FROM loss_actual AS la
             INNER JOIN production_working_info AS pwi ON pwi.pwi_id = la.pwi_id
             INNER JOIN sys_loss_mst AS slm ON la.loss_cd_id = slm.id
        WHERE pwi.pwi_lot_no =  '" & lot_no & "'
          AND la.line_cd =  '" & line_cd & "'
          AND la.start_loss BETWEEN  '" & start_dateTime & "' AND '" & end_dateTime & "'
          AND la.loss_cd_id != '36'
          AND la.loss_cd_id != '35'
        GROUP BY slm.loss_cd
        ORDER BY SUM(la.loss_time) DESC"
        End If
        Dim api = New api()
        Dim jsonData2 As String = api.Load_dataSQLite(sqlQuery)
        ' If jsonData2 <> "0" Then
        Return jsonData2
    End Function
End Class
