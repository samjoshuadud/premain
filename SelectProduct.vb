Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient

Public Class SelectProduct
    Inherits System.Windows.Forms.Form

    ' Public variable to store selected barcode
    Public SelectedBarcode As String = Nothing

    Private Sub ProductProduct_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProductList()
    End Sub

    Private Sub LoadProductList()
        Dim query As String = "SELECT i.Barcode, p.ProductName, i.UnitPrice " &
                          "FROM Inventory i " &
                          "INNER JOIN Products p ON i.ProductID = p.ProductID"

        Dim dt As New DataTable()

        Try
            Using conn As New SqlConnection(AppConfig.ConnectionString)
                Using cmd As New SqlCommand(query, conn)
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using

            dgvProducts.DataSource = dt

            ' Set column headers
            dgvProducts.Columns("barcode").HeaderText = "Barcode"
            dgvProducts.Columns("productname").HeaderText = "Product Name"
            dgvProducts.Columns("Unitprice").HeaderText = "Price"

            ' Add Select button column if not yet added
            If dgvProducts.Columns("SelectButton") Is Nothing Then
                Dim btnColumn As New DataGridViewButtonColumn()
                btnColumn.Name = "SelectButton"
                btnColumn.HeaderText = "Action"
                btnColumn.Text = "Select"
                btnColumn.UseColumnTextForButtonValue = True
                btnColumn.Width = 75
                dgvProducts.Columns.Add(btnColumn)
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading products: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ' Trigger selection by double-clicking a row
    Private Sub dgvProducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProducts.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectedBarcode = dgvProducts.Rows(e.RowIndex).Cells("barcode").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
