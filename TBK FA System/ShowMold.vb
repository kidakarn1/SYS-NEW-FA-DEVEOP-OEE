Imports System.Web.Script.Serialization

Public Class ShowMold
    Private Sub ShowMold_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setDatamGetdatamoldcavity(MainFrm.Label4.Text)
    End Sub
    Public Sub setDatamGetdatamoldcavity(line_cd As String)
        Dim mdMold = New modelMoldCavity
        Dim rs = mdMold.mGetdatamoldcavity(line_cd)
        If rs <> "0" Then
            Dim dict3 As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
            Dim i = 0
            btnSave.Visible = True
            For Each item As Object In dict3
                Dim mm_id = item("mm_id").ToString()
                Dim imc_id = item("imc_id").ToString()
                datlvDefectsumary = New ListViewItem(item("mm_mold_number").ToString())
                datlvDefectsumary.SubItems.Add(item("mm_name").ToString())
                datlvDefectsumary.SubItems.Add(mdMold.mGetStatusMoldCavity(mm_id, imc_id, MainFrm.Label4.Text))
                datlvDefectsumary.SubItems.Add(item("imc_id").ToString())
                datlvDefectsumary.SubItems.Add(item("mm_id").ToString())
                datlvDefectsumary.SubItems.Add(item("mm_cavity_shot").ToString())
                ' datlvDefectsumary.SubItems.Add(item("mm_name").ToString())
                lvMold.Items.Add(datlvDefectsumary)
                i += 1
            Next
            lvMold.Items(0).Selected = True
        Else
            btnSave.Visible = False
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Prd_detail.Enabled = True
        Me.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim wi = Prd_detail.lb_wi.Text
        Dim imc_id As String = "0"
        Dim mm_id As String = "0"
        Dim mm_name As String = "0"
        Dim mm_mold_number As String = "0"
        Dim cavity As String = "0"
        Dim status_Mold As String
        If Application.OpenForms().OfType(Of Chang_Loss).Any Then
            modelMoldCavity.before_imc_id = modelMoldCavity.imc_id
            modelMoldCavity.before_mm_id = modelMoldCavity.mm_id
            modelMoldCavity.before_mm_name = modelMoldCavity.mm_name
            modelMoldCavity.before_mm_mold_number = modelMoldCavity.mm_mold_number
            modelMoldCavity.before_cavity = modelMoldCavity.cavity
            Dim rsUpdate = modelMoldCavity.mUpdateStatuschangemold("0", modelMoldCavity.imc_id, MainFrm.Label4.Text, "1", "2", MainFrm.Label4.Text)
            modelMoldCavity.mUpdateStatusProduction("0", modelMoldCavity.imc_id, MainFrm.Label4.Text, "0", "2")
        Else
            modelMoldCavity.mUpdateStatusProduction("0", modelMoldCavity.imc_id, MainFrm.Label4.Text, "0", "2")
        End If
        If lvMold.SelectedItems.Count > 0 Then
            For Each lvItem As ListViewItem In lvMold.SelectedItems
                status_Mold = lvMold.Items(lvItem.Index).SubItems(2).Text
                mm_name = lvMold.Items(lvItem.Index).SubItems(1).Text
                imc_id = lvMold.Items(lvItem.Index).SubItems(3).Text
                mm_id = lvMold.Items(lvItem.Index).SubItems(4).Text
                cavity = lvMold.Items(lvItem.Index).SubItems(5).Text
                modelMoldCavity.imc_id = imc_id
                modelMoldCavity.mm_id = mm_id
                modelMoldCavity.mm_name = mm_name
                modelMoldCavity.mm_mold_number = lvMold.Items(lvItem.Index).SubItems(0).Text
                modelMoldCavity.cavity = cavity
            Next
        End If
        If status_Mold = "Unavailable" Or status_Mold = "Using for this WI" Then
        Else
            modelMoldCavity.mUpdateStatusProduction("2", modelMoldCavity.imc_id, MainFrm.Label4.Text, "0", "2")
            Dim rsUpdate As String = ""
            If Application.OpenForms().OfType(Of Chang_Loss).Any Then
                rsUpdate = modelMoldCavity.mUpdateStatuschangemold("1", imc_id, MainFrm.Label4.Text, "0", "2", MainFrm.Label4.Text)
            Else
                rsUpdate = modelMoldCavity.mUpdateStatuschangemold("0", imc_id, MainFrm.Label4.Text, "0", "2", modelMoldCavity.emp_codeLeader)
            End If
            If rsUpdate = "1" Then
                Dim emp_cd As String = ""
                If Application.OpenForms().OfType(Of Chang_Loss).Any Then
                    emp_cd = MainFrm.Label4.Text
                Else
                    emp_cd = modelMoldCavity.emp_codeLeader
                End If
                Dim rsInsert = modelMoldCavity.mInsertemployeechangemold("1", "2", emp_cd, wi, imc_id)
                If rsInsert = "1" Then
                    If Application.OpenForms().OfType(Of Chang_Loss).Any Then
                        Loss_reg.txtareamold.Text = modelMoldCavity.mm_name
                        Loss_reg.Show()
                    End If
                    Prd_detail.Enabled = True
                    Prd_detail.mold.Text = modelMoldCavity.mm_mold_number
                    Prd_detail.Test_short.Enabled = True
                    If modelMoldCavity.mm_mold_number = "" Then
                        Prd_detail.mold.Text = "NO DATA "
                    End If
                    Me.Close()
                Else
                    MsgBox("Insert Mold Faill")
                End If
            Else
                MsgBox("Update Mold Faill ")
            End If
        End If
    End Sub
End Class