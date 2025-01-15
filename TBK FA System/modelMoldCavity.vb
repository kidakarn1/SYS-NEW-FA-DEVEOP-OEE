Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Globalization
Imports System.Data
Imports Newtonsoft.Json.Linq
Imports System.Web.Script.Serialization
Public Class modelMoldCavity
    Public Shared imc_id As String = "0"
    Public Shared mm_id As String = "0"
    Public Shared mm_name As String = ""
    Public Shared mm_mold_number As String = ""
    Public Shared cavity As String = ""
    Public Shared emp_codeLeader As String = ""
    Public Shared shortTest As String = "0"
    Public Shared statusMoldLoss As String
    Public Shared ima_start_date As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    Public Shared ima_end_date As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    Public Shared before_imc_id As String = "0"
    Public Shared before_mm_id As String = "0"
    Public Shared before_mm_name As String = ""
    Public Shared moldFlg As String = ""
    Public Shared before_mm_mold_number As String = ""
    Public Shared before_cavity As String = ""
    Public Shared before_emp_codeLeader As String = ""
    Public Shared before_shortTest As String = "0"
    Public Shared before_statusMoldLoss As String
    Public Shared before_ima_start_date As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    Public Shared before_ima_end_date As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    Public Shared Function mCheckPermisson(emp_cd As String)
        Try
            Dim api = New api()
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkPermission?emp_code=" & emp_cd)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkPermission?emp_code=" & emp_cd)
            Dim dict2 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rsData)
            Dim status As String = "0"
            For Each item As Object In dict2
                status = item("status").ToString()
            Next
            Return status
        Catch ex As Exception
            MsgBox("connect Api Faill Please check modelMoldCavity in Function mCheckPermisson = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Function mupdateShot(mm_id As String, pwi_id As String, ima_use_shot As String, ima_type_actual As String, ima_start_date As String, ima_end_date As String, ima_status_flg As String, emp_code As String, line_cd As String)
        Try
            Dim api = New api()
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/updateShot?mm_id=" & mm_id & "&pwi_id=" & pwi_id & "&ima_use_shot=" & ima_use_shot & "&ima_type_actual=" & ima_type_actual & "&ima_start_date=" & ima_start_date & "&ima_end_date=" & ima_end_date & "&ima_status_flg=" & ima_status_flg & "&emp_code=" & emp_code & "&line_cd=" & line_cd)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/updateShot?mm_id=" & mm_id & "&pwi_id=" & pwi_id & "&ima_use_shot=" & ima_use_shot & "&ima_type_actual=" & ima_type_actual & "&ima_start_date=" & ima_start_date & "&ima_end_date=" & ima_end_date & "&ima_status_flg=" & ima_status_flg & "&emp_code=" & emp_code & "&line_cd=" & line_cd)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mupdateShot in Function mupdateShot = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Function GetCavity(mm_id As String)
        Try
            Dim api = New api()
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/getCavity?mm_id=" & mm_id)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/getCavity?mm_id=" & mm_id)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check modelMoldCavity in Function GetCavity = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Function mCheckworkingplan(wi As String)
        Try
            Dim api = New api()
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkWorkingPlan?wi=" & wi)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkWorkingPlan?wi=" & wi)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mCheckworkingplan in Function GetMoldByWi = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Function mGetdatamoldcavity(line_cd As String)
        Try
            Dim api = New api()
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/getListMold?line_cd=" & line_cd)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/getListMold?line_cd=" & line_cd)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mCheckPermisson = " & ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function mcheckMaxMold(mm_id As String)
        Try
            Dim api = New api()
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkMaxMold?mm_id=" & mm_id)
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkMaxMold?mm_id=" & mm_id)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mcheckMaxMold = " & ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function mGetStatusMoldCavity(mm_id As String, imc_id As String, line_cd As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkStatusMold?mm_id=" & mm_id & "&imc_id=" & imc_id & "&line_cd=" & line_cd)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkStatusMold?mm_id=" & mm_id & "&imc_id=" & imc_id & "&line_cd=" & line_cd)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mGetStatusMoldCavity = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Function mUpdateStatuschangemold(imc_status_production As String, imc_id As String, line_cd As String, lecm_status_flg As String, lecm_type_flg As String, emp_cd As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/updateStatuschangemold?imc_status_production=" & imc_status_production & "&imc_id=" & imc_id & "&line_cd=" & line_cd & "&lecm_status_flg=" & lecm_status_flg & "&lecm_type_flg=" & lecm_type_flg & "&emp_cd=" & emp_cd)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/updateStatuschangemold?imc_status_production=" & imc_status_production & "&imc_id=" & imc_id & "&line_cd=" & line_cd & "&lecm_status_flg=" & lecm_status_flg & "&lecm_type_flg=" & lecm_type_flg & "&emp_cd=" & emp_cd)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mUpdateStatuschangemold = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Async Function mUpdateStatusProduction(imc_status_production As String, imc_id As String, line_cd As String, lecm_status_flg As String, lecm_type_flg As String) As Task(Of String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/updateStatusProduction?imc_status_production=" & imc_status_production & "&imc_id=" & imc_id & "&line_cd=" & line_cd & "&lecm_status_flg=" & lecm_status_flg & "&lecm_type_flg=" & lecm_type_flg)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/updateStatusProduction?imc_status_production=" & imc_status_production & "&imc_id=" & imc_id & "&line_cd=" & line_cd & "&lecm_status_flg=" & lecm_status_flg & "&lecm_type_flg=" & lecm_type_flg)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mUpdateStatusproduction = " & ex.Message)
            Return 0
        End Try
    End Function
    Public Shared Function mInsertemployeechangemold(lecm_status_flg As String, lecm_type_flg As String, lecm_created_by As String, lecm_wi As String, imc_id As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/insertChangeMold?lecm_wi=" & lecm_wi & "&imc_id=" & imc_id & "&emp_code=" & lecm_created_by & "&lecm_status_flg=" & lecm_status_flg & "&lecm_type_flg=" & lecm_type_flg)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/insertChangeMold?lecm_wi=" & lecm_wi & "&imc_id=" & imc_id & "&emp_code=" & lecm_created_by & "&lecm_status_flg=" & lecm_status_flg & "&lecm_type_flg=" & lecm_type_flg)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mInsertemployeechangemold = " & ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function mGetDataMoldmst(mm_id As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkMaxMold?mm_id=" & mm_id)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkMaxMold?mm_id=" & mm_id)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mGetDataMoldmst = " & ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function mCheckProduction(mm_id As String, line_cd As String, flg_status As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkProduction?mm_id=" & mm_id & "&line_cd=" & line_cd & "&imc_status_production=" & flg_status)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkProduction?mm_id=" & mm_id & "&line_cd=" & line_cd & "&imc_status_production=" & flg_status)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mCheckProduction = " & ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function mCheckLifetime(mm_id As String, line_cd As String, shot_qty As String)
        Try
            Dim api = New api()
            Console.WriteLine("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkLifetime?mm_id=" & mm_id & "&emp_code=" & line_cd & "&shot_qty=" & shot_qty)
            Dim rsData = api.Load_data("http://" & Backoffice_model.svApi & "/API_NEW_FA/index.php/ApiMoldCavity/checkLifetime?mm_id=" & mm_id & "&emp_code=" & line_cd & "&shot_qty=" & shot_qty)
            Return rsData
        Catch ex As Exception
            MsgBox("connect Api Faill Please check mGetdatamoldcavity in Function mCheckLifetime = " & ex.Message)
            Return 0
        End Try
    End Function

End Class
