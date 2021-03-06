﻿Imports DevExpress.XtraBars
Imports BACSOFT.Db.SqlHelper
Imports DevExpress.Utils.HorzAlignment

Public Class frmPhongBanThue

    Private Sub frmPhongBanThue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loadDS()
    End Sub

    'Private Sub BarManager1_HighlightedLinkChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.HighlightedLinkChangedEventArgs) Handles BarManager1.HighlightedLinkChanged
    '    If Not e.Link Is Nothing AndAlso TypeOf e.Link.Item Is BarButtonItem Then
    '        Cursor = Cursors.Hand
    '    Else
    '        Cursor = Cursors.Default
    '    End If
    'End Sub

    Public Sub loadDS()

        Dim dt As DataTable = ExecuteSQLDataTable("SELECT ID,MaPhongBan,TenPhongBan FROM PhongBanThue ORDER BY ID")
        If Not dt Is Nothing Then
            gdv.DataSource = dt
        Else
            ShowBaoLoi(LoiNgoaiLe)
        End If
    End Sub

    Private Sub btXoa_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles mXoa.ItemClick
        If Not KiemTraQuyenSuDung("Menu", Me.Parent.Tag, DanhMucQuyen.QuyenXoa) Then Exit Sub
        If ShowCauHoi("Xóa phòng ban " & gdvChiTiet.GetFocusedRowCellValue("TenPhongBan") & " ?") Then
            AddParameterWhere("@ID", gdvChiTiet.GetFocusedRowCellValue("ID"))
            If doDelete("PhongBanThue", "ID=@ID") Is Nothing Then
                ShowBaoLoi(LoiNgoaiLe)
            Else
                gdvChiTiet.DeleteSelectedRows()
                ShowAlert("Đã xoá!")
            End If
        End If
    End Sub

    Private Sub gdvChiTiet_RowUpdated(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.RowObjectEventArgs) Handles gdvChiTiet.RowUpdated

        AddParameter("@MaPhongBan", gdvChiTiet.GetFocusedRowCellValue("MaPhongBan"))
        AddParameter("@TenPhongBan", gdvChiTiet.GetFocusedRowCellValue("TenPhongBan"))

        If Not IsDBNull(gdvChiTiet.GetFocusedRowCellValue("ID")) Then
            AddParameterWhere("@ID", gdvChiTiet.GetFocusedRowCellValue("ID"))
            If doUpdate("PhongBanThue", "ID=@ID") Is Nothing Then
                ShowBaoLoi(LoiNgoaiLe)
                loadDS()
            Else
                ShowAlert("Đã cập nhật phòng ban !")
                'gdvChiTiet.SetFocusedRowCellValue("ID", gdvChiTiet.GetFocusedRowCellValue("ID"))
            End If
        Else
            Dim id As Object = doInsert("PhongBanThue")
            If id Is Nothing Then
                ShowBaoLoi(LoiNgoaiLe)
                gdvChiTiet.DeleteSelectedRows()
                loadDS()
            Else
                ShowAlert("Đã thêm phòng ban !")
                gdvChiTiet.SetFocusedRowCellValue("ID", id)
            End If
        End If

    End Sub

    Private Sub gdvChiTiet_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gdvChiTiet.KeyDown
        If e.KeyCode = Keys.Delete Then
            mXoa.PerformClick()
        End If
    End Sub

    Private Sub gdvChiTiet_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gdvChiTiet.DoubleClick
        If Not KiemTraQuyenSuDung("Menu", Me.Parent.Tag, DanhMucQuyen.QuyenSua) Then Exit Sub
        If gdvChiTiet.FocusedRowHandle < 0 Then
            If Not KiemTraQuyenSuDung("Menu", Me.Parent.Tag, DanhMucQuyen.QuyenThem) Then Exit Sub
        End If

        gdvChiTiet.OptionsBehavior.Editable = True
        SendKeys.Send("{Enter}")

    End Sub

    Private Sub gdvChiTiet_HiddenEditor(sender As System.Object, e As System.EventArgs) Handles gdvChiTiet.HiddenEditor
        gdvChiTiet.OptionsBehavior.Editable = False
    End Sub
End Class
