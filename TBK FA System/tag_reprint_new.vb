﻿Imports VB = Microsoft.VisualBasic
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
Imports System.Web.Script.Serialization
Public Class tag_reprint_new
    Public Shared S_index As Integer = 0
    Dim g_index As Integer = 0
    Dim QR_Generator As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If Application.OpenForms().OfType(Of Sel_prd_setup).Any Then
            Sel_prd_setup.Enabled = True
            Sel_prd_setup.Show()
            Me.Close()
        Else
            Show_reprint_wi.Show()
            Me.Close()
        End If
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Try
            If My.Computer.Network.Ping(Backoffice_model.svDatabase) Then
                tag_print()
                Dim numOfindex As Integer = ListView1.SelectedIndices(0)
                Dim qr_code As String = ListBox1.Items(numOfindex)
                If Application.OpenForms().OfType(Of Sel_prd_setup).Any Then
                    Sel_prd_setup.Enabled = True
                    Working_Pro.Enabled = True
                    Working_Pro.Show()
                    Me.Close()
                End If
            Else
                load_show.Show()
            End If
        Catch ex As Exception
            load_show.Show()
        End Try
    End Sub
    Public Function tag_print()
        Dim wi As String = ""
        If Application.OpenForms().OfType(Of Sel_prd_setup).Any Then
            wi = Working_Pro.wi_no.Text
        Else
            wi = Show_reprint_wi.hide_wi_select.Text
        End If
        Dim seq_plan = ListView1.Items(g_index).SubItems(2).Text
        Dim seq_box = ListView1.Items(g_index).SubItems(4).Text
        Try
            If My.Computer.Network.Ping(Backoffice_model.svDatabase) Then
                Dim numOfindex As Integer = ListView1.SelectedIndices(0)
                Dim qr_code As String = ListBox1.Items(numOfindex)
                Backoffice_model.update_data_new_qr_detail(qr_code)
            Else
                load_show.Show()
            End If
        Catch ex As Exception
            load_show.Show()
        End Try
        Dim api = New api()
        PrintDocument1.Print()
        Return 0
    End Function
    Public Sub print_batch()

    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim numOfindex As Integer = ListView1.SelectedIndices(0)
        'Working_Pro.Label27.Text = ListBox1.Items(numOfindex)
        Dim next_process = Backoffice_model.GET_NEXT_PROCESS()
        Dim value_next_process As String = ""
        Dim api = New api()
        Dim qr_detailss As String = ListBox1.Items(numOfindex)
        Dim part_no As String = "NO_DATA"
        Dim qty As String = "NO_DATA"
        Dim part_name As String = "NO_DATA"
        Dim model As String = "NO_DATA"
        Dim location As String = "NO_DATA"
        Dim shift As String = "NO_DATA"
        Dim pro_seq As String = "NO_DATA"
        Dim aPen = New Pen(Color.Black)
        Dim plan_seq As String
        Dim num_char_seq As Integer
        Dim product_type As String = "NO_DATA"
        Dim DLV_DATE As String = "NO_DATA"
        Dim WI As String = "NO_DATA"
        Dim line_cd As String = MainFrm.Label4.Text
        Dim check_tag_type = Backoffice_model.B_check_format_tag() ' api.Load_data("http://" & Backoffice_model.svApi & "/API_GEMBA/GET_DATA_NEW_FA/GET_LINE_TYPE?line_cd=" & MainFrm.Label4.Text)
        If check_tag_type = "0" Then
            While next_process.read()
                value_next_process = next_process("NEXT_PROCESS").ToString
            End While
            next_process.Close()
            num_char_seq = Working_Pro.Label22.Text.Length
            If num_char_seq = 1 Then
                plan_seq = "00" & Working_Pro.Label22.Text
            ElseIf num_char_seq = 2 Then
                plan_seq = "0" & Working_Pro.Label22.Text
            Else
                plan_seq = Working_Pro.Label22.Text
            End If
            Dim shift_new As String = ListBox2.Items(numOfindex)
            Dim plan_seq_new As String = qr_detailss.Substring(16, 3)
            Dim box_no_new As String = qr_detailss.Substring(100, 3)
            lot_no = "NO_DATA"
            aPen.Width = 2.0F
            Try
                If My.Computer.Network.Ping(Backoffice_model.svDatabase) Then
                    If Application.OpenForms().OfType(Of Sel_prd_setup).Any Then
                        part_no = Working_Pro.Label3.Text
                        part_name = Working_Pro.Label12.Text
                        model = Working_Pro.lb_model.Text
                        qty = ListBox1.Items(numOfindex).Substring(52, 6) 'qty_s
                        location = Working_Pro.lb_location.Text
                        shift = ListView1.Items(g_index).SubItems(3).Text
                        lot_no = qr_detailss.Substring(58, 4)
                        product_type = Working_Pro.lb_prd_type.Text
                        Dim DLV_DATE1 = Working_Pro.lb_dlv_date.Text.Substring(0, 8)
                        DLV_DATE = DLV_DATE1 & Working_Pro.lb_dlv_date.Text.Substring(8)
                        WI = Working_Pro.wi_no.Text
                        'Backoffice_model.NEXT_PROCESS = Backoffice_model.F_NEXT_PROCESS(part_no)
                    Else
                        Dim data = qr_detailss.Split(" ")
                        'MsgBox(data(0).Substring(19))
                        '		If qr_detailss.Substring(19, 15) <> "" Then
                        '		part_no = qr_detailss.Substring(19, 14)
                        'ElseIf qr_detailss.Substring(19, 12) <> "" Then
                        '	part_no = qr_detailss.Substring(19, 13)
                        'End If
                        part_no = data(0).Substring(19)
                        qty = ListBox1.Items(numOfindex).Substring(52, 6)
                        return_result = Backoffice_model.get_data_item(Show_reprint_wi.hide_wi_select.Text)
                        While return_result.read()
                            part_name = return_result("ITEM_NAME").ToString()
                            model = return_result("MODEL").ToString()
                            location = return_result("LOCATION_PART").ToString()
                            product_type = return_result("PRODUCT_TYP").ToString()
                            DLV_DATE = return_result("WORK_ODR_DLV_DATE").ToString()
                            WI = return_result("WI").ToString()
                            line_cd = return_result("LINE_CD").ToString()
                        End While
                        return_result.close()
                        shift = ListView1.Items(g_index).SubItems(3).Text
                        lot_no = qr_detailss.Substring(58, 4)
                        Backoffice_model.NEXT_PROCESS = ListView1.Items(g_index).SubItems(5).Text
                    End If
                    'MsgBox(Label10.Text)
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
                    e.Graphics.DrawString("PART NO. ", Working_Pro.lb_font1.Font, Brushes.Black, 152, 13)
                    e.Graphics.DrawString(part_no, Working_Pro.lb_font2.Font, Brushes.Black, 152, 25)
                    e.Graphics.DrawString("QTY.", Working_Pro.lb_font1.Font, Brushes.Black, 492, 13)
                    e.Graphics.DrawString(Trim(qty), Working_Pro.lb_font2.Font, Brushes.Black, 505, 25)
                    e.Graphics.DrawString("PART NAME", Working_Pro.lb_font1.Font, Brushes.Black, 152, 67)
                    If Len(part_name) > 36 Then
                        part_name = part_name.Replace(vbCrLf, "")
                        Dim pastNameLine1 = part_name.Substring(0, 30)
                        Dim pastNameLine2 = part_name.Substring(30)
                        e.Graphics.DrawString(pastNameLine1, Label9_fontModel.Font, Brushes.Black, 152, 79)
                        e.Graphics.DrawString(pastNameLine2, Label9_fontModel.Font, Brushes.Black, 152, 98)
                    Else
                        part_name = part_name.Replace(vbCrLf, "")
                        e.Graphics.DrawString(part_name, lb_font2.Font, Brushes.Black, 152, 79)
                    End If
                    e.Graphics.DrawString("MODEL", Working_Pro.lb_font1.Font, Brushes.Black, 152, 123)
                    e.Graphics.DrawString(model, Working_Pro.lb_font4.Font, Brushes.Black, 152, 141)
                    e.Graphics.DrawString("NEXT PROCESS", Working_Pro.lb_font1.Font, Brushes.Black, 412, 123)
                    e.Graphics.DrawString(Backoffice_model.NEXT_PROCESS, Working_Pro.lb_font4.Font, Brushes.Black, 414, 141)
                    e.Graphics.DrawString("LOCATION", Working_Pro.lb_font1.Font, Brushes.Black, 592, 123)
                    e.Graphics.DrawString(location, Working_Pro.lb_font4.Font, Brushes.Black, 596, 141)
                    e.Graphics.DrawString("SHIFT", Working_Pro.lb_font1.Font, Brushes.Black, 152, 178)
                    e.Graphics.DrawString(shift, Working_Pro.lb_font2.Font, Brushes.Black, 170, 190)
                    e.Graphics.DrawString("PRO. SEQ.", Working_Pro.lb_font1.Font, Brushes.Black, 227, 178)
                    e.Graphics.DrawString(plan_seq_new, Working_Pro.lb_font2.Font, Brushes.Black, 231, 190)
                    e.Graphics.DrawString("BOX NO.", Working_Pro.lb_font1.Font, Brushes.Black, 302, 178)
                    e.Graphics.DrawString(box_no_new, Working_Pro.lb_font2.Font, Brushes.Black, 320, 190)
                    e.Graphics.DrawString("ACTUAL DATE", Working_Pro.lb_font1.Font, Brushes.Black, 412, 178)
                    'Dim day_month = ListView1.Items(g_index).SubItems(1).Text.Substring(0, 6)
                    'Dim Year() = ListView1.Items(g_index).SubItems(1).Text.Substring(5, 3)
                    Try
                        Dim da As Date = Date.ParseExact(ListView1.Items(g_index).SubItems(1).Text, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        Dim date_act_date = da.ToString("dd/MM/yyyy")
                        e.Graphics.DrawString(date_act_date, Working_Pro.lb_font5.Font, Brushes.Black, 412, 196)
                        Dim plan_cd As String
                        Dim factory_cd As String
                        If MainFrm.Label6.Text = "K2PD06" Then
                            factory_cd = "Phase8"
                            plan_cd = "52"
                        Else
                            factory_cd = "Phase10"
                            plan_cd = "51"
                        End If
                        e.Graphics.DrawString("FACTORY", Working_Pro.lb_font1.Font, Brushes.Black, 522, 178)
                        e.Graphics.DrawString(factory_cd, Working_Pro.lb_font5.Font, Brushes.Black, 522, 196)
                        e.Graphics.DrawString("INFO.", Working_Pro.lb_font1.Font, Brushes.Black, 612, 178)
                        'e.Graphics.DrawString(Label14.Text, lb_font2.Font, Brushes.Black, 626, 190)
                        e.Graphics.DrawString("LINE", Working_Pro.lb_font1.Font, Brushes.Black, 152, 238)
                        e.Graphics.DrawString(line_cd, Working_Pro.lb_font2.Font, Brushes.Black, 152, 250)
                        Try
                            Dim re As Date = Date.ParseExact(DLV_DATE.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            Dim result_date = re.ToString("dd/MM/yyyy")
                            DLV_DATE = result_date
                        Catch ex As Exception
                            Dim re As Date = Date.ParseExact(DLV_DATE.Substring(0, 8), "dd/MM/yy", CultureInfo.InvariantCulture)
                            Dim result_date = re.ToString("dd/MM/yyyy")
                            DLV_DATE = result_date
                        End Try
                    Catch ex As Exception
                        MsgBox("error data1 = " & ex.Message)
                    End Try
                    'MsgBox(lb_dlv_date.Text)
                    'Dim ssdate As String = lb_dlv_date.Text
                    'Dim dDate As Date = lb_dlv_date.Text
                    'lb_dlv_date.Text = Format(lb_dlv_date.Text, "dd/MM/yyyy")
                    'lb_dlv_date.Text = Format(dDate, "dd/MM/yyyy")
                    'Dim dlv_date As Date = lb_dlv_date.ToString("yyyy-MM-dd")
                    e.Graphics.DrawString("PLAN DATE", Working_Pro.lb_font1.Font, Brushes.Black, 302, 238)
                    e.Graphics.DrawString(DLV_DATE, Working_Pro.lb_font6.Font, Brushes.Black, 334, 250)
                    e.Graphics.DrawString("LOT NO.", Working_Pro.lb_font1.Font, Brushes.Black, 522, 238)
                    e.Graphics.DrawString(lot_no, Working_Pro.lb_font2.Font, Brushes.Black, 522, 250)
                    'e.Graphics.DrawString("PROD. SEQ.", lb_font1.Font, Brushes.Black, 612, 238)
                    'e.Graphics.DrawString(plan_seq, lb_font2.Font, Brushes.Black, 622, 250)
                    e.Graphics.DrawString("TBKK", Working_Pro.lb_font2.Font, Brushes.Black, 15, 13)
                    e.Graphics.DrawString("(Thailand) Co., Ltd.", Working_Pro.lb_font1.Font, Brushes.Black, 15, 45)
                    e.Graphics.DrawString("Shop floor system", Working_Pro.lb_font3.Font, Brushes.Black, 15, 73)
                    e.Graphics.DrawString("(New FA system)", Working_Pro.lb_font3.Font, Brushes.Black, 15, 85)
                    e.Graphics.DrawString("WI : " & WI, Working_Pro.lb_font3.Font, Brushes.Black, 15, 123)
                    If product_type = "10" Then
                        prdtype = "PART TYPE : FG"
                    ElseIf product_type = "40" Then
                        prdtype = "PART TYPE : Parts"
                    Else
                        prdtype = "PART TYPE : FW"
                    End If
                    e.Graphics.DrawString(prdtype, Working_Pro.lb_font3.Font, Brushes.Black, 15, 136)
                    Dim iden_cd As String
                    If MainFrm.Label6.Text = "K1PD01" Then
                        iden_cd = "GA"
                    Else
                        iden_cd = "GB"
                    End If
                    'Dim plan_date As String
                    'If Working_Pro.lb_dlv_date.Text = Nothing Then
                    ' MsgBox("if")
                    ' plan_date = Working_Pro.lb_dlv_date.Text.Substring(6, 4) & Working_Pro.lb_dlv_date.Text.Substring(3, 2) & Working_Pro.lb_dlv_date.Text.Substring(0, 2)
                    ' Else
                    ' MsgBox("else")
                    'plan_date = Show_reprint_wi.hide_wi_select.Text
                    'End If
                    If Working_Pro.lb_dlv_date.Text Is Nothing Then
                    End If
                    Dim part_no_res1 As String
                    Dim part_no_res As String
                    Dim part_numm As Integer = 0
                    For part_numm = Working_Pro.Label3.Text.Length To 24
                        part_no_res = part_no_res & " "
                    Next part_numm
                    part_no_res1 = Working_Pro.Label3.Text & part_no_res
                    Dim act_date As String
                    Dim actdateConv As Date = DateTime.Now.ToString("dd/MM/yyyy")
                    act_date = Format(actdateConv, "yyyyMMdd")
                    Dim qty_num As String
                    Dim num_char_qty As Integer
                    num_char_qty = Working_Pro.lb_qty_for_box.Text.Length
                    If num_char_qty = 1 Then
                        qty_num = "     " & Working_Pro.lb_qty_for_box.Text
                    ElseIf num_char_qty = 2 Then
                        qty_num = "    " & Working_Pro.lb_qty_for_box.Text
                    ElseIf num_char_qty = 3 Then
                        qty_num = "   " & Working_Pro.lb_qty_for_box.Text
                    ElseIf num_char_qty = 4 Then
                        qty_num = "  " & Working_Pro.lb_qty_for_box.Text
                    ElseIf num_char_qty = 5 Then
                        qty_num = " " & Working_Pro.lb_qty_for_box.Text
                    Else
                        qty_num = Working_Pro.lb_qty_for_box.Text
                    End If
                    Dim cus_part_no As String = "                         "
                    Dim num_char_box As Integer
                    num_char_box = Working_Pro.lb_box_count.Text.Length
                    If num_char_box = 1 Then
                        box_no = "00" & Working_Pro.lb_box_count.Text
                    ElseIf num_char_box = 2 Then
                        box_no = "0" & Working_Pro.lb_box_count.Text
                    Else
                        box_no = Working_Pro.lb_box_count.Text
                    End If
                    Try
                        PictureBox3.Image = QR_Generator.Encode(qr_detailss)
                        e.Graphics.DrawImage(PictureBox3.Image, 597, 17, 95, 95)
                        e.Graphics.DrawImage(PictureBox3.Image, 31, 190, 95, 95)
                        PictureBox4.Image = QR_Generator.Encode(qr_detailss)
                        e.Graphics.DrawImage(PictureBox4.Image, 620, 199, 70, 70)
                    Catch ex As Exception

                    End Try
                Else
                    load_show.Show()
                End If
            Catch ex As Exception
                load_show.Show()
            End Try
        Else
            If ListView1.Items(g_index).SubItems(1).Text.Substring(0, 7) = "(BATCH)" Then
                Try
                    If Application.OpenForms().OfType(Of Sel_prd_setup).Any Then
                        part_no = Working_Pro.Label3.Text
                        part_name = Working_Pro.Label12.Text
                        model = Working_Pro.lb_model.Text
                        qty = ListBox1.Items(numOfindex).Substring(52, 6) 'qty_s
                        location = Working_Pro.lb_location.Text
                        shift = ListView1.Items(g_index).SubItems(3).Text
                        lot_no = qr_detailss.Substring(58, 4)
                        product_type = Working_Pro.lb_prd_type.Text
                        Dim DLV_DATE1 = Working_Pro.lb_dlv_date.Text.Substring(0, 8)
                        DLV_DATE = DLV_DATE1 & Working_Pro.lb_dlv_date.Text.Substring(8)
                        WI = Working_Pro.wi_no.Text
                        'Backoffice_model.NEXT_PROCESS = Backoffice_model.F_NEXT_PROCESS(part_no)
                    Else
                        Dim data = qr_detailss.Split(" ")
                        part_no = data(0).Substring(19)
                        qty = ListBox1.Items(numOfindex).Substring(52, 6)
                        shift = ListView1.Items(g_index).SubItems(3).Text
                        lot_no = qr_detailss.Substring(58, 4)
                        Backoffice_model.NEXT_PROCESS = ListView1.Items(g_index).SubItems(5).Text
                        return_result = Backoffice_model.get_data_item(Show_reprint_wi.hide_wi_select.Text)
                        While return_result.read()
                            part_no = return_result("ITEM_CD").ToString()
                            part_name = return_result("ITEM_NAME").ToString()
                            model = return_result("MODEL").ToString()
                            location = return_result("LOCATION_PART").ToString()
                            product_type = return_result("PRODUCT_TYP").ToString()
                            DLV_DATE = return_result("WORK_ODR_DLV_DATE").ToString()
                            WI = return_result("WI").ToString()
                            line_cd = return_result("LINE_CD").ToString()
                        End While
                        return_result.close()
                        lot_no = qr_detailss.Substring(58, 4)
                    End If
                    aPen = New Pen(Color.Black)
                    aPen.Width = 2.0F
                    'MsgBox(Label10.Text)
                    'vertical ตรง
                    e.Graphics.DrawLine(aPen, 10, 10, 10, 290)
                    e.Graphics.DrawLine(aPen, 330, 58, 330, 95)
                    e.Graphics.DrawLine(aPen, 460, 10, 460, 290)
                    e.Graphics.DrawLine(aPen, 580, 10, 580, 290) 'QTY
                    e.Graphics.DrawLine(aPen, 110, 10, 110, 290) 'QR right top
                    e.Graphics.DrawLine(aPen, 700, 10, 700, 290)
                    'Horizontal นอน
                    e.Graphics.DrawLine(aPen, 110, 58, 580, 58)
                    e.Graphics.DrawLine(aPen, 110, 95, 700, 95)
                    e.Graphics.DrawLine(aPen, 10, 11, 700, 11)
                    e.Graphics.DrawLine(aPen, 10, 110, 110, 110)
                    e.Graphics.DrawLine(aPen, 10, 220, 110, 220)
                    e.Graphics.DrawLine(aPen, 580, 67, 700, 67)
                    e.Graphics.DrawLine(aPen, 460, 140, 700, 140)
                    e.Graphics.DrawLine(aPen, 460, 190, 700, 190)
                    e.Graphics.DrawLine(aPen, 460, 240, 580, 240)
                    e.Graphics.DrawLine(aPen, 10, 289, 700, 289)
                    'DATA
                    e.Graphics.DrawString("TBKK", lb_font1.Font, Brushes.Black, 19, 15)
                    e.Graphics.DrawString("WI : " & WI, Label_wi_type.Font, Brushes.Black, 15, 60)
                    prdtype = "PART TYPE : FG"
                    e.Graphics.DrawString(prdtype, Label_wi_type.Font, Brushes.Black, 15, 85)
                    e.Graphics.DrawString("Instr. Code", lb_font5.Font, Brushes.Black, 120, 15)
                    e.Graphics.DrawString(part_no & "     " & part_name, lb_font4_B.Font, Brushes.Black, 120, 35)
                    Dim result_snp As Integer = 0
                    result_snp = ListBox1.Items(numOfindex).Substring(52, 6)
                    shift = ListView1.Items(g_index).SubItems(3).Text
                    e.Graphics.DrawString("QTY.", lb_font3.Font, Brushes.Black, 465, 15)
                    e.Graphics.DrawString(result_snp, LB_QTY.Font, Brushes.Black, 495, 28)
                    e.Graphics.DrawString("MODEL", lb_font5.Font, Brushes.Black, 120, 60)
                    e.Graphics.DrawString(model, lb_font4_B.Font, Brushes.Black, 140, 75)
                    e.Graphics.DrawString("NEXT PROCESS", lb_font5.Font, Brushes.Black, 340, 60)
                    e.Graphics.DrawString("ISUZU", lb_font4_B.Font, Brushes.Black, 360, 75)
                    e.Graphics.DrawString("LOCATION", lb_font5.Font, Brushes.Black, 465, 60)
                    e.Graphics.DrawString("D4U10A1", lb_font4_B.Font, Brushes.Black, 485, 75)
                    Dim num_box_seq As Integer
                    num_box_seq = Working_Pro.lb_box_count.Text.Length
                    num_char_seq = Working_Pro.Label22.Text.Length
                    If num_char_seq = 1 Then
                        plan_seq = "00" & Working_Pro.Label22.Text
                    ElseIf num_char_seq = 2 Then
                        plan_seq = "0" & Working_Pro.Label22.Text
                    Else
                        plan_seq = Working_Pro.Label22.Text
                    End If
                    e.Graphics.DrawString("SHIFT", lb_font5.Font, Brushes.Black, 585, 69)
                    e.Graphics.DrawString(shift, lb_font4_B.Font, Brushes.Black, 625, 75)
                    e.Graphics.DrawString("LINE", lb_font5.Font, Brushes.Black, 467, 98)
                    e.Graphics.DrawString(MainFrm.Label4.Text, lb_font4_B.Font, Brushes.Black, 470, 115)
                    Dim plan_seq_new As String = qr_detailss.Substring(16, 3)
                    Dim box_no_new As String = qr_detailss.Substring(100, 3)
                    e.Graphics.DrawString("PRO SEQ", lb_font5.Font, Brushes.Black, 585, 98)
                    e.Graphics.DrawString(plan_seq_new, lb_font4_B.Font, Brushes.Black, 610, 115)
                    Dim load_data = api.Load_data("http://" & Backoffice_model.svApi & "/API_GEMBA/GET_DATA_NEW_FA/GET_DATA_WORKING?WI=" & WI)
                    Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(load_data)
                    Dim plan_date As String = ""
                    For Each item As Object In dict
                        Dim da As Date = item("WORK_ODR_DLV_DATE").substring(0, 10)
                        Dim f As Date = Date.ParseExact(item("WORK_ODR_DLV_DATE").substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        result_plan_date = f.ToString("dd/MM/yyyy")
                        plan_date = result_plan_date
                    Next
                    e.Graphics.DrawString("PLAN DATE", lb_font5.Font, Brushes.Black, 465, 145)
                    e.Graphics.DrawString(plan_date, lb_font4_B.Font, Brushes.Black, 470, 162)
                    e.Graphics.DrawString("BOX NO", lb_font5.Font, Brushes.Black, 585, 145)
                    e.Graphics.DrawString(box_no_new, lb_font4_B.Font, Brushes.Black, 610, 162)
                    e.Graphics.DrawString("ACTUAL DATE", lb_font5.Font, Brushes.Black, 465, 195)
                    Dim result_date_act_date
                    If ListView1.Items(g_index).SubItems(1).Text.Substring(0, 7) = "(BATCH)" Then
                        Dim da As Date = Date.ParseExact(ListView1.Items(g_index).SubItems(1).Text.Substring(7, 8), "dd/MM/yy", CultureInfo.InvariantCulture)
                        result_date_act_date = da.ToString("dd/MM/yyyy")
                    Else
                        Dim da As Date = Date.ParseExact(ListView1.Items(g_index).SubItems(1).Text.Substring(0, 8), "dd/MM/yy", CultureInfo.InvariantCulture)
                        result_date_act_date = da.ToString("dd/MM/yyyy")
                    End If
                    e.Graphics.DrawString(result_date_act_date, lb_font4_B.Font, Brushes.Black, 470, 215)
                    e.Graphics.DrawString("LOT NO", lb_font5.Font, Brushes.Black, 465, 245)
                    e.Graphics.DrawString(lot_no, lb_font4_B.Font, Brushes.Black, 485, 265)
                    e.Graphics.DrawString("NO.", lb_font5.Font, Brushes.Black, 120, 105)
                    e.Graphics.DrawString("Part No.", lb_font5.Font, Brushes.Black, 160, 105)
                    e.Graphics.DrawString("Part Name.", lb_font5.Font, Brushes.Black, 250, 105)
                    e.Graphics.DrawString("QTY", lb_font5.Font, Brushes.Black, 420, 105)
                    e.Graphics.DrawLine(aPen, 110, 123, 460, 123)
                    e.Graphics.DrawLine(aPen, 110, 210, 460, 210)
                    Dim margin_left_no = 125
                    Dim margin_top_no = 130
                    Dim margin_left_item_cd = 160
                    Dim margin_left_part_name = 250
                    Dim margin_left_QTY = 425
                    Dim arr_item_cd() As String = {"898244-6240", "898244-6250", "898244-6260", "898244-6270", "898244-6280"}
                    Dim arr_qr_code_sub() As String = {"01", "02", "03", "04", "05"}
                    For i = 1 To 5 Step 1
                        e.Graphics.DrawString(i, lb_font5.Font, Brushes.Black, margin_left_no, margin_top_no)
                        e.Graphics.DrawString(arr_item_cd(i - 1), lb_font5.Font, Brushes.Black, margin_left_item_cd, margin_top_no)
                        e.Graphics.DrawString("BRACKET ASM;CAM NO" & i, lb_font5.Font, Brushes.Black, margin_left_part_name, margin_top_no)
                        e.Graphics.DrawString(result_snp.ToString(), lb_font5.Font, Brushes.Black, margin_left_QTY, margin_top_no)
                        margin_top_no += 15
                    Next
                    Dim qrcode As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
                    qrcode.QRCodeScale = 10
                    Dim bitmap_qr_box As Bitmap = qrcode.Encode("TEST")
                    Dim qr_by_model = 120
                    Dim qr_by_model_left = 118
                    Dim iden_cd As String
                    If MainFrm.Label6.Text = "K1PD01" Then
                        iden_cd = "GA"
                    Else
                        iden_cd = "GB"
                    End If
                    Dim part_no_res1 As String = ""
                    Dim part_no_res As String = ""
                    Dim part_numm As Integer = 0
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
                    Dim plan_cd As String
                    Dim factory_cd As String
                    If MainFrm.Label6.Text = "K2PD06" Then
                        factory_cd = "Phase8"
                        plan_cd = "52"
                    Else
                        factory_cd = "Phase10"
                        plan_cd = "51"
                    End If
                    Dim id_tag = api.Load_data("http://" & Backoffice_model.svApi & "/API_GEMBA/GET_DATA_NEW_FA/GET_ID_PRINT_DETAIL_MAIN?qr_code=" & qr_detailss)
                    Dim qr_sub = Backoffice_model.get_qr_detail_sub(id_tag)
                    'For j = 1 To 5 Step 1
                    Dim j As Integer = 1
                    While qr_sub.read
                        Dim qr_sub_data As String = qr_sub("tag_qr_detail").ToString()
                        bitmap_qr_box = QR_Generator.Encode(qr_sub_data)
                        e.Graphics.DrawString("QR No ." & j, lb_font5.Font, Brushes.Black, qr_by_model_left, 215)
                        e.Graphics.DrawImage(bitmap_qr_box, qr_by_model, 233, 50, 50) 'Right top
                        qr_by_model += 70
                        margin_top_no += 15
                        qr_by_model_left += 70
                        'arr_qr_code_sub(j - 1) =
                        j = j + 1
                    End While
                    qr_sub.close()
                    For part_numm = Working_Pro.Label3.Text.Length To 24
                        part_no_res = part_no_res & " "
                    Next part_numm
                    part_no_res1 = Working_Pro.Label3.Text & part_no_res
                    ' Dim numOfindex2 As Integer = ListView1.SelectedIndices(0)
                    Dim qr_code2 As String = ListBox1.Items(numOfindex)
                    Console.WriteLine(qr_code2)
                    Dim qr_code As String = qr_code2 'iden_cd & Working_Pro.Label24.Text & plan_date & plan_seq & part_no_res1 & act_date & qty_num & Working_Pro.Label18.Text & cus_part_no & act_date & plan_seq & plan_cd & box_no_new
                    bitmap_qr_box = QR_Generator.Encode(qr_code)
                    e.Graphics.DrawImage(bitmap_qr_box, 15, 120, 90, 90) 'left
                    e.Graphics.DrawImage(bitmap_qr_box, 615, 15, 45, 45) 'Right top
                    e.Graphics.DrawString("FACTORY", lb_font3.Font, Brushes.Black, 15, 230)
                    e.Graphics.DrawString("Phase10", lb_font3.Font, Brushes.Black, 33, 250)
                    e.Graphics.DrawImage(bitmap_qr_box, 600, 205, 75, 75) 'Right top
                    Backoffice_model.update_data_new_qr_detail_main(qr_code2)
                Catch ex As Exception
                    MsgBox("error data2 =  " & ex.Message)
                End Try
            Else
                Try
                    If Application.OpenForms().OfType(Of Sel_prd_setup).Any Then
                        part_no = Working_Pro.Label3.Text
                        part_name = Working_Pro.Label12.Text
                        model = Working_Pro.lb_model.Text
                        qty = ListBox1.Items(numOfindex).Substring(52, 6) 'qty_s
                        location = Working_Pro.lb_location.Text
                        shift = ListView1.Items(g_index).SubItems(3).Text
                        lot_no = qr_detailss.Substring(58, 4)
                        product_type = Working_Pro.lb_prd_type.Text
                        Dim DLV_DATE1 = Working_Pro.lb_dlv_date.Text.Substring(0, 8)
                        DLV_DATE = DLV_DATE1 & Working_Pro.lb_dlv_date.Text.Substring(8)
                        WI = Working_Pro.wi_no.Text
                        'Backoffice_model.NEXT_PROCESS = Backoffice_model.F_NEXT_PROCESS(part_no)
                    Else
                        Dim data = qr_detailss.Split(" ")
                        'MsgBox(data(0).Substring(19))
                        '		If qr_detailss.Substring(19, 15) <> "" Then
                        '		part_no = qr_detailss.Substring(19, 14)
                        'ElseIf qr_detailss.Substring(19, 12) <> "" Then
                        '	part_no = qr_detailss.Substring(19, 13)
                        'End If
                        part_no = data(0).Substring(19)
                        qty = ListBox1.Items(numOfindex).Substring(52, 6)

                        shift = ListView1.Items(g_index).SubItems(3).Text
                        lot_no = qr_detailss.Substring(58, 4)
                        Backoffice_model.NEXT_PROCESS = ListView1.Items(g_index).SubItems(5).Text
                        return_result = Backoffice_model.get_data_item(Show_reprint_wi.hide_wi_select.Text)
                        While return_result.read()
                            part_no = return_result("ITEM_CD").ToString()
                            part_name = return_result("ITEM_NAME").ToString()
                            model = return_result("MODEL").ToString()
                            location = return_result("LOCATION_PART").ToString()
                            product_type = return_result("PRODUCT_TYP").ToString()
                            DLV_DATE = return_result("WORK_ODR_DLV_DATE").ToString()
                            WI = return_result("WI").ToString()
                            line_cd = return_result("LINE_CD").ToString()
                        End While
                        return_result.close()
                        lot_no = qr_detailss.Substring(58, 4)
                    End If
                    aPen = New Pen(Color.Black)
                    aPen.Width = 2.0F
                    'MsgBox(Label10.Text)
                    'vertical ตรง
                    e.Graphics.DrawLine(aPen, 10, 10, 10, 290)
                    e.Graphics.DrawLine(aPen, 330, 58, 330, 95)
                    e.Graphics.DrawLine(aPen, 460, 10, 460, 290)
                    e.Graphics.DrawLine(aPen, 580, 10, 580, 290) 'QTY
                    e.Graphics.DrawLine(aPen, 110, 10, 110, 290) 'QR right top
                    e.Graphics.DrawLine(aPen, 700, 10, 700, 290)
                    'Horizontal นอน
                    e.Graphics.DrawLine(aPen, 110, 58, 580, 58)
                    e.Graphics.DrawLine(aPen, 110, 95, 700, 95)
                    e.Graphics.DrawLine(aPen, 10, 11, 700, 11)
                    e.Graphics.DrawLine(aPen, 10, 110, 110, 110)
                    e.Graphics.DrawLine(aPen, 10, 220, 110, 220)
                    e.Graphics.DrawLine(aPen, 580, 67, 700, 67)
                    e.Graphics.DrawLine(aPen, 460, 140, 700, 140)
                    e.Graphics.DrawLine(aPen, 460, 190, 700, 190)
                    e.Graphics.DrawLine(aPen, 460, 240, 580, 240)
                    e.Graphics.DrawLine(aPen, 10, 289, 700, 289)
                    'DATA
                    e.Graphics.DrawString("TBKK", lb_font1.Font, Brushes.Black, 19, 15)
                    e.Graphics.DrawString("WI : " & WI, Label_wi_type.Font, Brushes.Black, 15, 60)
                    prdtype = "PART TYPE : FG"
                    e.Graphics.DrawString(prdtype, Label_wi_type.Font, Brushes.Black, 15, 85)
                    e.Graphics.DrawString("Instr. Code", lb_font5.Font, Brushes.Black, 120, 15)
                    e.Graphics.DrawString(part_no & "     " & part_name, lb_font4_B.Font, Brushes.Black, 120, 35)
                    Dim result_snp As Integer = 0
                    result_snp = ListBox1.Items(numOfindex).Substring(52, 6)
                    shift = ListView1.Items(g_index).SubItems(3).Text
                    e.Graphics.DrawString("QTY.", lb_font3.Font, Brushes.Black, 465, 15)
                    e.Graphics.DrawString(result_snp, LB_QTY.Font, Brushes.Black, 495, 28)
                    e.Graphics.DrawString("MODEL", lb_font5.Font, Brushes.Black, 120, 60)
                    e.Graphics.DrawString(model, lb_font4_B.Font, Brushes.Black, 140, 75)
                    e.Graphics.DrawString("NEXT PROCESS", lb_font5.Font, Brushes.Black, 340, 60)
                    e.Graphics.DrawString("ISUZU", lb_font4_B.Font, Brushes.Black, 360, 75)
                    e.Graphics.DrawString("LOCATION", lb_font5.Font, Brushes.Black, 465, 60)
                    e.Graphics.DrawString("D4U10A1", lb_font4_B.Font, Brushes.Black, 485, 75)
                    Dim num_box_seq As Integer
                    num_box_seq = Working_Pro.lb_box_count.Text.Length
                    num_char_seq = Working_Pro.Label22.Text.Length
                    If num_char_seq = 1 Then
                        plan_seq = "00" & Working_Pro.Label22.Text
                    ElseIf num_char_seq = 2 Then
                        plan_seq = "0" & Working_Pro.Label22.Text
                    Else
                        plan_seq = Working_Pro.Label22.Text
                    End If
                    e.Graphics.DrawString("SHIFT", lb_font5.Font, Brushes.Black, 585, 69)
                    e.Graphics.DrawString(shift, lb_font4_B.Font, Brushes.Black, 625, 75)
                    e.Graphics.DrawString("LINE", lb_font5.Font, Brushes.Black, 467, 98)
                    e.Graphics.DrawString(MainFrm.Label4.Text, lb_font4_B.Font, Brushes.Black, 470, 115)
                    Dim plan_seq_new As String = qr_detailss.Substring(16, 3)
                    Dim box_no_new As String = qr_detailss.Substring(100, 3)
                    e.Graphics.DrawString("PRO SEQ", lb_font5.Font, Brushes.Black, 585, 98)
                    e.Graphics.DrawString(plan_seq_new, lb_font4_B.Font, Brushes.Black, 610, 115)
                    Dim load_data = api.Load_data("http://" & Backoffice_model.svApi & "/API_GEMBA/GET_DATA_NEW_FA/GET_DATA_WORKING?WI=" & WI)
                    Dim dict As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(load_data)
                    Dim plan_date As String = ""
                    For Each item As Object In dict
                        Dim da As Date = item("WORK_ODR_DLV_DATE").substring(0, 10)
                        Dim f As Date = Date.ParseExact(item("WORK_ODR_DLV_DATE").substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        result_plan_date = f.ToString("dd/MM/yyyy")
                        plan_date = result_plan_date
                    Next
                    e.Graphics.DrawString("PLAN DATE", lb_font5.Font, Brushes.Black, 465, 145)
                    e.Graphics.DrawString(plan_date, lb_font4_B.Font, Brushes.Black, 470, 162)
                    e.Graphics.DrawString("BOX NO", lb_font5.Font, Brushes.Black, 585, 145)
                    e.Graphics.DrawString(box_no_new, lb_font4_B.Font, Brushes.Black, 610, 162)
                    e.Graphics.DrawString("ACTUAL DATE", lb_font5.Font, Brushes.Black, 465, 195)
                    Dim result_date_act_date
                    If ListView1.Items(g_index).SubItems(1).Text.Substring(0, 7) = "(BATCH)" Then
                        Dim da As Date = Date.ParseExact(ListView1.Items(g_index).SubItems(1).Text.Substring(7, 8), "dd/MM/yy", CultureInfo.InvariantCulture)
                        result_date_act_date = da.ToString("dd/MM/yyyy")
                    Else
                        Dim da As Date = Date.ParseExact(ListView1.Items(g_index).SubItems(1).Text.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        result_date_act_date = da.ToString("dd/MM/yyyy")
                    End If
                    e.Graphics.DrawString(result_date_act_date, lb_font4_B.Font, Brushes.Black, 470, 215)
                    e.Graphics.DrawString("LOT NO", lb_font5.Font, Brushes.Black, 465, 245)
                    e.Graphics.DrawString(lot_no, lb_font4_B.Font, Brushes.Black, 485, 265)
                    e.Graphics.DrawString("NO.", lb_font5.Font, Brushes.Black, 120, 105)
                    e.Graphics.DrawString("Part No.", lb_font5.Font, Brushes.Black, 160, 105)
                    e.Graphics.DrawString("Part Name.", lb_font5.Font, Brushes.Black, 250, 105)
                    e.Graphics.DrawString("QTY", lb_font5.Font, Brushes.Black, 420, 105)
                    e.Graphics.DrawLine(aPen, 110, 123, 460, 123)
                    'e.Graphics.DrawLine(aPen, 110, 210, 460, 210)
                    Dim margin_left_no = 125
                    Dim margin_top_no = 140
                    Dim margin_left_item_cd = 160
                    Dim margin_left_part_name = 250
                    Dim margin_left_QTY = 425
                    Dim arr_item_cd() As String = {"898244-6240", "898244-6250", "898244-6260", "898244-6270", "898244-6280"}
                    Dim arr_qr_code_sub() As String = {"01", "02", "03", "04", "05"}
                    For i = 1 To 5 Step 1
                        e.Graphics.DrawString(i, LB_FONT_DATA.Font, Brushes.Black, margin_left_no, margin_top_no)
                        e.Graphics.DrawString(arr_item_cd(i - 1), LB_FONT_DATA.Font, Brushes.Black, margin_left_item_cd, margin_top_no)
                        e.Graphics.DrawString("BRACKET ASM;CAM NO" & i, LB_FONT_DATA.Font, Brushes.Black, margin_left_part_name, margin_top_no)
                        e.Graphics.DrawString(result_snp.ToString(), LB_FONT_DATA.Font, Brushes.Black, margin_left_QTY, margin_top_no)
                        margin_top_no += 30
                    Next
                    Dim qrcode As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
                    qrcode.QRCodeScale = 10

                    Dim bitmap_qr_box As Bitmap = qrcode.Encode("TEST")
                    Dim qr_by_model = 120
                    Dim qr_by_model_left = 118
                    Dim iden_cd As String
                    If MainFrm.Label6.Text = "K1PD01" Then
                        iden_cd = "GA"
                    Else
                        iden_cd = "GB"
                    End If
                    Dim part_no_res1 As String
                    Dim part_no_res As String
                    Dim part_numm As Integer = 0
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
                    Dim plan_cd As String
                    Dim factory_cd As String
                    If MainFrm.Label6.Text = "K2PD06" Then
                        factory_cd = "Phase8"
                        plan_cd = "52"
                    Else
                        factory_cd = "Phase10"
                        plan_cd = "51"
                    End If
                    'For j = 1 To 5 Step 1
                    Dim j As Integer = 1

                    'Next
                    For part_numm = Working_Pro.Label3.Text.Length To 24
                        part_no_res = part_no_res & " "
                    Next part_numm
                    part_no_res1 = Working_Pro.Label3.Text & part_no_res
                    Dim qr_code As String = iden_cd & Working_Pro.Label24.Text & plan_date & plan_seq & part_no_res1 & act_date & qty_num & Working_Pro.Label18.Text & cus_part_no & act_date & plan_seq & plan_cd & box_no_new
                    bitmap_qr_box = QR_Generator.Encode(qr_code)
                    e.Graphics.DrawImage(bitmap_qr_box, 15, 120, 90, 90) 'left
                    e.Graphics.DrawImage(bitmap_qr_box, 615, 15, 45, 45) 'Right top
                    e.Graphics.DrawString("FACTORY", lb_font3.Font, Brushes.Black, 15, 230)
                    e.Graphics.DrawString("Phase10", lb_font3.Font, Brushes.Black, 33, 250)
                    e.Graphics.DrawImage(bitmap_qr_box, 600, 205, 75, 75) 'Right top
                Catch ex As Exception
                    MsgBox("error data3 =  " & ex.Message)
                End Try
            End If
        End If

    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If IsNothing(Me.ListView1.FocusedItem) Then
        ElseIf ListView1.FocusedItem.Index >= 0 Then
            If ListView1.Items.Count > 0 Then
                Dim index As Integer = ListView1.FocusedItem.Index
                g_index = index
            End If
        Else
            MessageBox.Show("An Error has halted thid process")
        End If
    End Sub
    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub tag_reprint_new_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBox1.Visible = False
        ListBox2.Visible = False
        Timer1.Start()
    End Sub

    Private Sub PrintPreviewDialog1_Load(sender As Object, e As EventArgs) Handles PrintPreviewDialog1.Load

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = TimeOfDay.ToString("H:mm:ss")
        Label4.Text = DateTime.Now.ToString("D")
    End Sub

    Private Sub pb_down_Click(sender As Object, e As EventArgs) Handles pb_down.Click
        BTNDOWN1()
    End Sub

    Private Sub pbUp_Click(sender As Object, e As EventArgs) Handles pbUp.Click
        BTNUP1()
    End Sub
    Public Sub BTNUP1()
        If S_index < 0 Then
            S_index = 0
        ElseIf S_index > CDbl(Val((ListView1.Items.Count - 1))) Then
            S_index = CDbl(Val((ListView1.Items.Count - 1)))
        End If
        Try
            ListView1.Items(S_index).Selected = False
            S_index -= 1
            If S_index < 0 Then
                S_index = 0
                'ElseIf lvDefectact.Items.Count > S_index Then
                'S_index = CDbl(Val((lvDefectact.Items.Count - 1)))
            End If
            ListView1.Items(S_index).Selected = True
            ListView1.Items(S_index).EnsureVisible()
            ListView1.Select()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BTNDOWN1()
        If S_index < 0 Then
            S_index = 0
        ElseIf S_index > CDbl(Val((ListView1.Items.Count - 1))) Then
            S_index = CDbl(Val((ListView1.Items.Count - 1)))
        End If
        Try
            ListView1.Items(S_index).Selected = False
            S_index += 1
            If S_index < 0 Then
                S_index = 0
                'ElseIf S_index > lvDefectact.Items.Count Then
                '  S_index = CDbl(Val((lvDefectact.Items.Count - 1)))
            End If
            ListView1.Items(S_index).Selected = True
            ListView1.Items(S_index).EnsureVisible()
            ListView1.Select()
        Catch ex As Exception
        End Try
    End Sub
End Class
