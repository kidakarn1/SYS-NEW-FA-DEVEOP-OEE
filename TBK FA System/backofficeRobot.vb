﻿Public Class backofficeRobot
    Public Shared Function loadRequest()
        Dim api = New api()
        Dim rs = api.Load_data("http://" & Backoffice_model.svApi & "/API_GEMBA/RobotApppSuport/getRequest?line_cd=" & MainFrm.Label4.Text)
        Return rs
    End Function
    Public Shared Function loadsolution(iir_title As String)
        Dim api = New api()
        Dim rs = api.Load_data("http://" & Backoffice_model.svApi & "/API_GEMBA/RobotApppSuport/Solution?iir_title=" & iir_title)
        Return rs
    End Function
End Class