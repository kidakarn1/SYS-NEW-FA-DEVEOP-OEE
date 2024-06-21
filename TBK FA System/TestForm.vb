Public Class TestForm
    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WebView21.Source = New Uri("http://192.168.161.219/productionHrProgress/?line_cd=K1A003&shift=P")
    End Sub
End Class