Imports Microsoft.Data.SqlClient

Public Class TodaySales
    ' Define the connection string at the class level
    Private connectionString As String = AppConfig.ConnectionString

    ' Method to load data from the TodaySales table along with ProductName from the Products table
    Public Function LoadTodaySales() As DataTable
        Dim dt As New DataTable()

        ' Query to get the necessary data and exclude records where Quantity is 0
        Dim query As String = "SELECT ts.TodaySalesID, ts.TransactionNumber, ts.ProductID, p.ProductName, ts.Quantity, ts.UnitPrice, ts.TotalPrice, ts.Discount " &
                          "FROM TodaySales ts " &
                          "INNER JOIN Products p ON ts.ProductID = p.ProductID " &
                          "WHERE ts.Quantity > 0"

        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using da As New SqlDataAdapter(query, connection)
                    da.Fill(dt)  ' Fill the DataTable with the query results
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading TodaySales data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

        Return dt


    End Function


    ' Method to load users' full names into the CancelledBy ComboBox
    Private Sub LoadUsers()
        ' Query to get all users' full names
        Dim query As String = "SELECT FullName FROM Users"

        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            ' Add each user's full name to the ComboBox
                            cmbCancelledby.Items.Add(reader("FullName").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading users for cancelled by: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Event handler for DataGridView cell click to populate the textboxes
    Private Sub dgvTodaysSales_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTodaysSales.CellClick
        If e.RowIndex >= 0 Then
            ' Get the values from the clicked row and populate the textboxes
            Dim row As DataGridViewRow = dgvTodaysSales.Rows(e.RowIndex)

            ' Populate the ProductName in txtProductInfo and ProductID in txtProductID
            txtProductInfo.Text = row.Cells("ProductName").Value.ToString()  ' Display ProductName
            txtDiscount.Text = row.Cells("Discount").Value.ToString()
            txtTransactionNumber.Text = row.Cells("TransactionNumber").Value.ToString()
            txtQuantity.Text = row.Cells("Quantity").Value.ToString()
            txtUnitPrice.Text = row.Cells("UnitPrice").Value.ToString()
            txtTotalPrice.Text = row.Cells("TotalPrice").Value.ToString()

            ' Store the ProductID internally for later use
            txtProductID.Text = row.Cells("ProductID").Value.ToString()
        End If
    End Sub

    ' Button click event to open the UserCancel form and pass parameters
    Private Sub btnCancelOrder_Click(sender As Object, e As EventArgs) Handles btnCancelOrder.Click
        ' Get the selected transaction number and product ID
        Dim transactionNumber As String = txtTransactionNumber.Text
        Dim productID As Integer

        ' Ensure that ProductID and TransactionNumber are valid
        If String.IsNullOrEmpty(transactionNumber) Then
            MessageBox.Show("Please select an order to cancel.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If String.IsNullOrEmpty(txtProductID.Text) OrElse Not Integer.TryParse(txtProductID.Text, productID) Then
            MessageBox.Show("Please select a valid product to cancel.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Retrieve the cancel quantity from txtCancelQuantity
        Dim cancelQuantity As Integer
        If Not Integer.TryParse(txtCancelQuantity.Text, cancelQuantity) OrElse cancelQuantity <= 0 Then
            MessageBox.Show("Please enter a valid cancellation quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Retrieve the available quantity from txtQuantity
        Dim availableQuantity As Integer
        If Not Integer.TryParse(txtQuantity.Text, availableQuantity) Then
            MessageBox.Show("Invalid available quantity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Check if the cancel quantity exceeds the available quantity
        If cancelQuantity > availableQuantity Then
            MessageBox.Show("Cancellation quantity cannot exceed the available quantity. Available quantity is " & availableQuantity.ToString(), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Retrieve the cancellation reason from txtReasons
        Dim reasons As String = txtReasons.Text
        If String.IsNullOrEmpty(reasons) Then
            MessageBox.Show("Please provide a reason for the cancellation.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Retrieve the add-to-inventory option from cmbAddToInventory
        If cmbAddToInventory.SelectedItem Is Nothing Then
            MessageBox.Show("Please select whether to add to inventory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim addToInventory As Boolean = (cmbAddToInventory.SelectedItem.ToString() = "Yes")

        ' Get the selected user from cmbCancelledBy ComboBox
        If cmbCancelledby.SelectedItem Is Nothing Then
            MessageBox.Show("Please select the user who is cancelling.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim cancelledBy As String = cmbCancelledby.SelectedItem.ToString()

        ' Create an instance of the UserCancel form and pass the parameters
        Dim cancelForm As New UserCancel(transactionNumber, productID, cancelQuantity, reasons, addToInventory, cancelledBy)
        cancelForm.ShowDialog()  ' Show the form and wait for the user to perform the cancellation
    End Sub


    ' Event that triggers when the form is loaded
    Private Sub TodaySales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load TodaySales data
        Dim dt As DataTable = LoadTodaySales()
        dgvTodaysSales.DataSource = dt

        ' Set AutoSizeMode for the columns to fill the available space
        For Each column As DataGridViewColumn In dgvTodaysSales.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Next
        ' Hide the TodaySaleID and ProductID columns
        dgvTodaysSales.Columns("TodaySalesID").Visible = False
        dgvTodaysSales.Columns("ProductID").Visible = False

        ' Populate ComboBox with options for "Add to Inventory"
        cmbAddToInventory.Items.Add("Yes")
        cmbAddToInventory.Items.Add("No")
        cmbAddToInventory.SelectedIndex = -1  ' Set default to "Yes"

        ' Load the users into the cmbCancelledBy ComboBox
        LoadUsers()

        ' Set TextBoxes as ReadOnly
        ' Disable the TextBoxes to make them non-clickable and read-only
        txtProductInfo.Enabled = False
        txtDiscount.Enabled = False
        txtUnitPrice.Enabled = False
        txtTotalPrice.Enabled = False
        txtQuantity.Enabled = False
        txtTransactionNumber.Enabled = False
        txtVoidby.Enabled = False



        ' Set DataGridView background color to white
        dgvTodaysSales.BackgroundColor = Color.White

        ' Remove the extra row (the row for adding new data)
        dgvTodaysSales.AllowUserToAddRows = False

        ' Disable the blue highlight when a row is selected
        dgvTodaysSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvTodaysSales.DefaultCellStyle.SelectionBackColor = dgvTodaysSales.BackgroundColor
        dgvTodaysSales.DefaultCellStyle.SelectionForeColor = dgvTodaysSales.ForeColor


        ' Set alternating row colors
        dgvTodaysSales.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray ' For alternating rows
        dgvTodaysSales.DefaultCellStyle.BackColor = Color.White ' For normal rows

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ' Set the background color of Panel1
        Panel1.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub





    ' Optional: Change color when mouse enters button
    Private Sub btnCLose_MouseEnter(sender As Object, e As EventArgs) Handles btnCLose.MouseEnter
        btnCLose.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub

    ' Optional: Revert color when mouse leaves button
    Private Sub btnCLose_MouseLeave(sender As Object, e As EventArgs) Handles btnCLose.MouseLeave
        btnCLose.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click

        Me.Close() ' Close the form or whatever functionality you need
    End Sub

    





End Class
