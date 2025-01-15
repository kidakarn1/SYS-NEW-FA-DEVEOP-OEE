Imports System.Web.Script.Serialization
Public Class moldCalculator
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        shortTest.Text = shortTest.Text + "1"
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        shortTest.Text = shortTest.Text + "2"
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        shortTest.Text = shortTest.Text + "3"
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        shortTest.Text = shortTest.Text + "4"
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        shortTest.Text = shortTest.Text + "5"
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        shortTest.Text = shortTest.Text + "6"
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        shortTest.Text = shortTest.Text + "7"
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        shortTest.Text = shortTest.Text + "8"
    End Sub
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        shortTest.Text = shortTest.Text + "9"
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        shortTest.Text = shortTest.Text + "0"
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        shortTest.Clear()
    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim txt_lenght As Integer = shortTest.Text.Length
        Try
            shortTest.Text = shortTest.Text.Substring(0, txt_lenght - 1)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub moldCalculator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbPartNo.Text = Prd_detail.lb_item_cd.Text
        lbMax.Text = modelMoldCavity.mcheckMaxMold(modelMoldCavity.mm_id)
        shortTest.Focus()
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim mdMold = New modelMoldCavity
        mdMold.shortTest = shortTest.Text
        ' If CDbl(Val(shortTest.Text)) <= CDbl(Val(lbMax.Text)) Then
        Me.Close()
        '    Else
        '     MsgBox("Please Check QTY.")
        '     End If
    End Sub
End Class