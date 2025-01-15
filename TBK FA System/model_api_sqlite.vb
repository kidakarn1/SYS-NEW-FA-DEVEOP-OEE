Imports System.Globalization
Imports System.Web.Script.Serialization

Public Class model_api_sqlite
    Public Shared Function mas_INSERT_production_working_info(ind_row As String, pwi_lot_no As String, pwi_seq_no As String, pwi_shift As String, pwi_id As String)
        Dim dateTime_Crr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Try
            Dim Sql = "Insert into production_working_info(
            pwi_id , 
			ind_row , 
			pwi_lot_no , 
			pwi_seq_no , 
			pwi_shift , 
			pwi_created_date , 
			pwi_created_by) 
			Values(
                '" & pwi_id & "' , 
				'" & ind_row & "' , 
				'" & pwi_lot_no & "' , 
				'" & pwi_seq_no & "' , 
				'" & pwi_shift & "' , 
				'" & dateTime_Crr & "' , 
				'SYSTEM')"
            '   Console.WriteLine(Sql)
            Dim api = New api
            Dim jsonData As String = api.Load_dataSQLite(Sql)
            Return 1
        Catch ex As Exception
            MsgBox("Error Files model_api_sqlite In Function mas_INSERT_production_working_info")
        End Try
    End Function

    Public Shared Function mas_Update_seqplan(wi As String, line_cd As String, date_start As String, date_end As String, Update_seq As String)
        Dim api = New api
        Try
            Dim currdated As String = DateTime.Now.ToString("yyyy/MM/dd")
            Dim today As Date = Date.Today
            Dim time_tomorrow As DateTime = today.AddDays(1)
            Dim format_tommorow = "yyyy/MM/dd"
            Dim date_tommorow = time_tomorrow.ToString(format_tommorow)
            date_end_covert = date_tommorow & " 07:59:59"
            Try
                Dim time_now As DateTime
                time_now = DateTime.Now.ToString("hh:mm:ss tt")
                If time_now >= "08:00:00 AM" And time_now <= "07:59:59 PM" Then
                    date_start = currdated & " 08:00:00"
                    ' date_start = date_start & " 08:00:00"
                Else
                    date_start = currdated & " 08:00:00"
                End If
                If time_now >= "12:00:00 AM" And time_now <= "08:00:00 AM" Then
                    Dim format_tommorow_re = "yyyy/MM/dd"
                    Dim del_date1 As DateTime = today.AddDays(-1)
                    date_start = del_date1.ToString(format_tommorow_re)
                    Dim sub_date_end1 = Trim(date_end.ToString.Substring(0, 10))
                    date_start = date_start & " 08:00:00"
                    date_end_covert = sub_date_end1 & " 07:59:59"
                End If
            Catch ex As Exception

            End Try
            Try
                Dim sql = "SELECT tmp_id from tmp_planseq where tmp_created_date BETWEEN  '" & date_start & "' and '" & date_end_covert & "' and tmp_line_cd = '" & line_cd & "'"
                Console.WriteLine("sql====>" & sql)
                Dim jsonData As String = api.Load_dataSQLite(sql)
                Dim dcResultdata As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(jsonData)
                Dim tmp_id As String = ""
                For Each items As Object In dcResultdata
                    tmp_id = items("tmp_id").ToString()
                Next
                Return tmp_id
            Catch ex As Exception
                load_show.Show()
            End Try
        Catch ex As Exception
            MsgBox("Error Files model_api_sqlite In Function mas_Update_seqplan")
        End Try
    End Function
    Public Shared Function mas_manage_mold(line_cd As String)
        Dim date_start As String = DateTime.Now.ToString("yyyy/MM/dd") & " 08:00:00"
        Dim parsed_date_start As DateTime = DateTime.ParseExact(date_start, "yyyy/MM/dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture)
        Dim formatted_date_start As DateTime = parsed_date_start ' เก็บเป็น DateTime เพื่อใช้ AddDays ได้
        Dim convertStDate = formatted_date_start.ToString("yyyy-MM-dd HH:mm:ss")
        ' เพิ่ม 1 วัน
        Dim date_end As DateTime = formatted_date_start.AddDays(1)
        Dim convertdate_end = date_end.ToString("yyyy-MM-dd HH:mm:ss")
        Dim sqlGetact_ins = "SELECT * FROM act_ins WHERE line_cd = '" & line_cd & "' ORDER BY id DESC LIMIT 1;"
        Dim api = New api
        Dim jsonData As String = api.Load_dataSQLite(sqlGetact_ins)
        Try
            Dim dcResultdata As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(jsonData)
            Dim tmp_seq_mold_no As String = ""
            Dim mm_id As String
            Dim imc_id As String
            Dim pwi_id As String
            Dim st_time As String
            Dim end_time As String
            Dim emp_code As String
            Dim ima_type_actual
            Dim ima_status_flg
            For Each items As Object In dcResultdata
                tmp_seq_mold_no = items("seq_mold_no").ToString()
                mm_id = items("mm_id").ToString
                imc_id = items("imc_id").ToString
                pwi_id = items("pwi_id").ToString
                st_time = items("st_time").ToString
                end_time = items("end_time").ToString
                emp_code = items("line_cd").ToString
                ima_type_actual = "2"
                ima_status_flg = "1"
            Next
            Dim sqlSum = "select IFNULL(SUM(qty), 0) as rs from act_ins where st_time >= '" & convertStDate & "' and end_time <= '" & convertdate_end & "' and line_cd ='" & line_cd & "' and seq_mold_no = '" & tmp_seq_mold_no & "'"
            Console.WriteLine(sqlSum)
            Dim jsonDataSum As String = api.Load_dataSQLite(sqlSum)
            Dim dcResultdataSum As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(jsonDataSum)
            Dim tmp_id As String = ""
            For Each items As Object In dcResultdataSum
                Dim Cavity = modelMoldCavity.GetCavity(mm_id)
                Dim ima_use_shot = Math.Ceiling(CDbl(Val(items("rs").ToString)) / Cavity)
                modelMoldCavity.mupdateShot(mm_id, pwi_id, ima_use_shot, ima_type_actual, st_time, end_time, ima_status_flg, emp_code, line_cd)
                modelMoldCavity.mUpdateStatusProduction("0", imc_id, line_cd, "1", "2")
            Next
        Catch ex As Exception
        End Try
        Return 0
    End Function
End Class
