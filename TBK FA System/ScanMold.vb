Public Class ScanMold
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Prd_detail.Enabled = True
        Me.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        keyboardScanMold.Show()
    End Sub
    Private Sub tbLeaderMold_KeyDown(sender As Object, e As KeyEventArgs) Handles tbLeaderMold.KeyDown
        If e.KeyCode = Keys.Enter Then
            OKConfrime(tbLeaderMold.Text)
        End If
    End Sub
    Public Sub OKConfrime(emp_cd As String)
        Dim mdMold = New modelMoldCavity
        Dim rsStatus = mdMold.mCheckPermisson(emp_cd)
        If rsStatus = "0" Then
            MsgBox("Not Permission.")
            tbLeaderMold.Clear()
            tbLeaderMold.Select()
        ElseIf rsStatus = "1" Then
            ShowMold.Show()
            mdMold.emp_codeLeader = tbLeaderMold.Text
            Me.Close()
        End If
    End Sub
End Class