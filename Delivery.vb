Imports System.IO ' Add this import for file handling
Imports Microsoft.Data.SqlClient
Imports System.Data.SqlClient
Imports iTextSharp.text.pdf

Public Class Delivery
    ' Global connection string
    Private connectionString As String = AppConfig.ConnectionString

    Private currentEditRowIndex As Integer = -1 ' Declare a variable to store the row index

    ' Helper function to execute a query and return a DataTable
    Private Function ExecuteQuery(query As String) As DataTable
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Using cmd As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim dt As New DataTable()
                        dt.Load(reader)
                        Return dt
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error executing query: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Using
    End Function

    ' Load Suppliers into ComboBox
    Private Sub LoadSuppliers()
        Dim query As String = "SELECT SupplierID, CompanyName FROM Suppliers"
        Dim dt As DataTable = ExecuteQuery(query)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            cmbSupplierID.DataSource = dt
            cmbSupplierID.DisplayMember = "CompanyName"
            cmbSupplierID.ValueMember = "SupplierID"
        Else
            MessageBox.Show("No suppliers found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' Load Products into ComboBox
    Private Sub LoadProducts()
        Dim query As String = "SELECT ProductID, ProductName FROM Products"
        Dim dt As DataTable = ExecuteQuery(query)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            cmbProductID.DataSource = dt
            cmbProductID.DisplayMember = "ProductName"
            cmbProductID.ValueMember = "ProductID"
        Else
            MessageBox.Show("No products found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' Form Load Event
    Private Sub Delivery_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load suppliers and products
        LoadSuppliers()
        LoadProducts()

        ' Set the "Received By" field to the current user's full name
        If Not String.IsNullOrEmpty(SessionData.fullName) Then
            txtReceivedBy.Text = SessionData.fullName
        Else
            MessageBox.Show("Error: The current user's name is not set. Please ensure the user is logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtReceivedBy.Text = "Unknown User" ' Fallback value
        End If
        txtReceivedBy.ReadOnly = True ' Make it read-only

        ' Set ComboBoxes to no selection
        cmbSupplierID.SelectedIndex = -1
        cmbProductID.SelectedIndex = -1

        ' Initialize DataGridView columns
        InitializeDataGridView()
        SetupDataGridView()

        ' Update the label with the generated transaction number
        Dim transactionNumber As String = "TRX - " & DateTime.Now.ToString("yyyyMMddHHmmss")
        lblTransactionNumber.Text = transactionNumber

        Try
            ' Set minimum date for expiration to be 1 year from today
            Dim minExpirationDate As DateTime = DateTime.Now.Date.AddYears(1)
            dtpExpirationDate.MinDate = minExpirationDate
            dtpExpirationDate.Value = minExpirationDate

            ' Fix delivery date to today only
            dtpDeliveryDate.MinDate = DateTime.Now.Date
            dtpDeliveryDate.MaxDate = DateTime.Now.Date
            dtpDeliveryDate.Value = DateTime.Now.Date
            dtpDeliveryDate.Enabled = False
        Catch ex As ArgumentException
            MessageBox.Show("Error initializing date controls: " & ex.Message, "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvPendingItems.BackgroundColor = Color.White
        dgvPendingItems.BorderStyle = BorderStyle.None
        dgvPendingItems.AllowUserToAddRows = False
        dgvPendingItems.ReadOnly = True

        dgvPendingItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvPendingItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPendingItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvPendingItems.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvPendingItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvPendingItems.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvPendingItems.EnableHeadersVisualStyles = False
        dgvPendingItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvPendingItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvPendingItems.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvPendingItems.ColumnHeadersHeight = 45
    End Sub
    ' Initialize DataGridView columns to store SupplierID and ProductID
    Private Sub InitializeDataGridView()
        dgvPendingItems.Columns.Add("Barcode", "Barcode")
        dgvPendingItems.Columns.Add("TransactionNumber", "Transaction Number")
        dgvPendingItems.Columns.Add("ReceivedBy", "Received By")
        dgvPendingItems.Columns.Add("SupplierID", "Supplier ID")
        dgvPendingItems.Columns.Add("CompanyName", "Company Name")
        dgvPendingItems.Columns.Add("ProductID", "Product ID")
        dgvPendingItems.Columns.Add("ProductName", "Product Name")
        dgvPendingItems.Columns.Add("Quantity", "Quantity")
        dgvPendingItems.Columns.Add("SellingPrice", "Selling Price")
        dgvPendingItems.Columns.Add("CostPrice", "Cost Price")
        dgvPendingItems.Columns.Add("ExpirationDate", "Expiration Date")
        dgvPendingItems.Columns.Add("DeliveryDate", "Delivery Date")
        dgvPendingItems.Columns.Add("UnitPrice", "Unit Price")
        dgvPendingItems.Columns.Add("CategoryID", "Category ID")
        dgvPendingItems.Columns("CategoryID").Visible = False

        'dgvPendingItems.Columns.Add("Image", "Image")
        'dgvPendingItems.Columns("Image").Visible = False

        dgvPendingItems.Columns.Add("Expiration", "Expiration")
        dgvPendingItems.Columns("Expiration").Visible = False ' Hide it if not needed for display




        ' Hide SupplierID and ProductID columns
        dgvPendingItems.Columns("SupplierID").Visible = False
        dgvPendingItems.Columns("ProductID").Visible = False

        ' Format ExpirationDate column to show only date (no time)
        dgvPendingItems.Columns("ExpirationDate").DefaultCellStyle.Format = "yyyy-MM-dd"
    End Sub

    ' Confirm Button: Add data to dgvPendingItems
    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        Try

            Dim barcode As String = txtBarcode.Text.Trim()
            If String.IsNullOrEmpty(barcode) Then
                MessageBox.Show("Please enter a valid barcode.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBarcode.Focus()
                Return
            End If

            If IsBarcodeExists(Barcode) Then
                MessageBox.Show("The barcode already exists in the system. Please use a unique barcode.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBarcode.Focus()
                Return
            End If


            ' Validate ComboBox selections for Supplier and Product
            If cmbSupplierID.SelectedIndex < 0 Then
                MessageBox.Show("Please select a valid supplier.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbSupplierID.Focus()
                Return
            End If

            If cmbProductID.SelectedIndex < 0 Then
                MessageBox.Show("Please select a valid product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cmbProductID.Focus()
                Return
            End If

            ' Validate Quantity (must be a valid number and greater than 0)
            Dim quantity As Integer
            If Not Integer.TryParse(txtQuantity.Text.Trim(), quantity) OrElse quantity <= 0 Then
                MessageBox.Show("Please enter a valid Quantity (greater than 0).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtQuantity.Focus()
                Return
            End If

            ' Validate Selling Price (numeric and greater than zero)
            Dim sellingPrice As Decimal
            If String.IsNullOrWhiteSpace(txtSellingPrice.Text) OrElse Not Decimal.TryParse(txtSellingPrice.Text.Trim(), sellingPrice) Then
                MessageBox.Show("Please enter a valid Selling Price (numeric values only).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSellingPrice.Focus()
                Return
            End If
            If sellingPrice <= 0 Then
                MessageBox.Show("Selling Price must be greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSellingPrice.Focus()
                Return
            End If

            ' Validate Cost Price (numeric and greater than zero)
            Dim costPrice As Decimal
            If String.IsNullOrWhiteSpace(txtCostPrice.Text) OrElse Not Decimal.TryParse(txtCostPrice.Text.Trim(), costPrice) Then
                MessageBox.Show("Please enter a valid Cost Price (numeric values only).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCostPrice.Focus()
                Return
            End If
            If costPrice <= 0 Then
                MessageBox.Show("Cost Price must be greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCostPrice.Focus()
                Return
            End If

            ' Ensure Selling Price is greater than Cost Price
            If sellingPrice <= costPrice Then
                MessageBox.Show("Selling Price must be greater than Cost Price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSellingPrice.Focus()
                Return
            End If

            ' Validate Expiration Date (must be at least 1 year in the future)
            Dim minimumExpirationDate As DateTime = DateTime.Now.Date.AddYears(1)
            ' Validate Expiration Date before assigning
            If dtpExpirationDate.Value.Date < dtpExpirationDate.MinDate Then
                dtpExpirationDate.Value = dtpExpirationDate.MinDate
                MessageBox.Show("Expiration Date adjusted to the minimum allowed date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            ' Validate Delivery Date before assigning
            If dtpDeliveryDate.Value.Date < dtpDeliveryDate.MinDate OrElse dtpDeliveryDate.Value.Date > dtpDeliveryDate.MaxDate Then
                dtpDeliveryDate.Value = DateTime.Now.Date
                MessageBox.Show("Delivery Date adjusted to today's date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            MessageBox.Show("After Validation:" & vbCrLf &
                        "dtpExpirationDate.Value = " & dtpExpirationDate.Value.ToString() & vbCrLf &
                        "dtpDeliveryDate.Value = " & dtpDeliveryDate.Value.ToString(), "Debug Info")


            ' Auto-generate Transaction Number using DateTime
            Dim transactionNumber As String = "TRX - " & DateTime.Now.ToString("yyyyMMddHHmmss")
            lblTransactionNumber.Text = transactionNumber ' Update the label with the transaction number


            ' Get supplier and product names directly from ComboBoxes
            Dim companyName As String = cmbSupplierID.Text ' Use CompanyName instead of SupplierName
            Dim productName As String = cmbProductID.Text ' Directly use the ProductName from ComboBox
            Dim currentUser As String = SessionData.fullName ' Get current user from SessionData

            Dim rowIndex As Integer = dgvPendingItems.Rows.Add()
            Dim newRow As DataGridViewRow = dgvPendingItems.Rows(rowIndex)

            newRow.Cells("Barcode").Value = txtBarcode.Text.Trim()
            newRow.Cells("TransactionNumber").Value = lblTransactionNumber.Text
            newRow.Cells("ReceivedBy").Value = txtReceivedBy.Text
            newRow.Cells("SupplierID").Value = cmbSupplierID.SelectedValue
            newRow.Cells("CompanyName").Value = cmbSupplierID.Text
            newRow.Cells("ProductID").Value = cmbProductID.SelectedValue
            newRow.Cells("ProductName").Value = cmbProductID.Text
            newRow.Cells("Quantity").Value = quantity
            newRow.Cells("SellingPrice").Value = sellingPrice
            newRow.Cells("CostPrice").Value = costPrice
            newRow.Cells("ExpirationDate").Value = dtpExpirationDate.Value.Date
            newRow.Cells("DeliveryDate").Value = dtpDeliveryDate.Value.Date
            newRow.Cells("UnitPrice").Value = sellingPrice ' Assuming UnitPrice is the same as SellingPrice
            newRow.Cells("CategoryID").Value = DBNull.Value ' Or set the actual CategoryID value if available
            newRow.Cells("Expiration").Value = dtpExpirationDate.Value.Date ' Or set the actual expiration date





            MessageBox.Show("Delivery added to Pending list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear input fields after adding to dgvPendingItems
            ClearFields()

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        btnSave.Visible = True
    End Sub

    ' Handle CellClick to populate selected row data
    Private Sub dgvPendingItems_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPendingItems.CellClick
        ' Ensure the click is not on the header row
        If e.RowIndex < 0 Then Return

        ' Set the current row index for editing
        currentEditRowIndex = e.RowIndex

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = dgvPendingItems.Rows(e.RowIndex)

        ' Get the SupplierID and ProductID values from the selected row
        Dim supplierID As Integer = Convert.ToInt32(selectedRow.Cells("SupplierID").Value)
        Dim productID As Integer = Convert.ToInt32(selectedRow.Cells("ProductID").Value)

        ' Populate Supplier ComboBox with the SupplierName
        For Each item As DataRowView In cmbSupplierID.Items
            If Convert.ToInt32(item("SupplierID")) = supplierID Then
                cmbSupplierID.SelectedItem = item
                Exit For
            End If
        Next

        ' Populate Product ComboBox with the ProductName
        For Each item As DataRowView In cmbProductID.Items
            If Convert.ToInt32(item("ProductID")) = productID Then
                cmbProductID.SelectedItem = item
                Exit For
            End If
        Next

        ' Populate other textboxes
        txtQuantity.Text = selectedRow.Cells("Quantity").Value.ToString()
        txtSellingPrice.Text = selectedRow.Cells("SellingPrice").Value.ToString()
        txtCostPrice.Text = selectedRow.Cells("CostPrice").Value.ToString()

        ' Set the Expiration Date and Delivery Date from the DataGridView
        If Not IsDBNull(selectedRow.Cells("ExpirationDate").Value) Then
            dtpExpirationDate.Value = Convert.ToDateTime(selectedRow.Cells("ExpirationDate").Value)
        End If

        If Not IsDBNull(selectedRow.Cells("DeliveryDate").Value) Then
            dtpDeliveryDate.Value = Convert.ToDateTime(selectedRow.Cells("DeliveryDate").Value)
        End If

        ' Get the "ReceivedBy" value
        Dim userFullName As String = selectedRow.Cells("ReceivedBy").Value.ToString()

        ' Populate the cmbUserFullName ComboBox with the ReceivedBy value
        For Each item As DataRowView In cmbUserFullName.Items
            If item("FullName").ToString() = userFullName Then
                cmbUserFullName.SelectedItem = item
                Exit For
            End If
        Next

    End Sub

    ' Clear input fields
    Private Sub ClearFields()
        ' Clear textboxes
        txtBarcode.Clear()
        txtQuantity.Clear()
        txtSellingPrice.Clear()
        txtCostPrice.Clear()

        ' Reset DateTimePickers with proper validation
        Try
            ' Set expiration date to 1 year from today
            Dim minExpirationDate As DateTime = DateTime.Now.Date.AddYears(1)
            dtpExpirationDate.MinDate = minExpirationDate
            dtpExpirationDate.Value = minExpirationDate

            ' Set delivery date to today
            dtpDeliveryDate.Value = DateTime.Now.Date
        Catch ex As ArgumentException
            ' Handle any potential argument exceptions
            MessageBox.Show("Error setting date values: " & ex.Message, "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Reset ComboBoxes
        cmbSupplierID.SelectedIndex = -1
        cmbProductID.SelectedIndex = -1
        cmbUserFullName.SelectedIndex = -1
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try
            ' Ensure a row is selected for editing
            If currentEditRowIndex < 0 Then
                MessageBox.Show("Please select a row to edit.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If



            ' Validate ComboBox selections for Supplier and Product
            If cmbSupplierID.SelectedIndex < 0 Then
                MessageBox.Show("Please select a valid supplier.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If cmbProductID.SelectedIndex < 0 Then
                MessageBox.Show("Please select a valid product.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Validate Selling Price
            If String.IsNullOrWhiteSpace(txtSellingPrice.Text) OrElse Not IsNumeric(txtSellingPrice.Text) Then
                MessageBox.Show("Please enter a valid Selling Price (numeric values only).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSellingPrice.Focus()
                Return
            End If

            Dim sellingPrice As Decimal = Convert.ToDecimal(txtSellingPrice.Text.Trim())
            If sellingPrice <= 0 Then
                MessageBox.Show("Selling Price must be greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSellingPrice.Focus()
                Return
            End If

            ' Validate Cost Price
            If String.IsNullOrWhiteSpace(txtCostPrice.Text) OrElse Not IsNumeric(txtCostPrice.Text) Then
                MessageBox.Show("Please enter a valid Cost Price (numeric values only).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCostPrice.Focus()
                Return
            End If

            Dim costPrice As Decimal = Convert.ToDecimal(txtCostPrice.Text.Trim())
            If costPrice <= 0 Then
                MessageBox.Show("Cost Price must be greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCostPrice.Focus()
                Return
            End If

            ' Ensure Selling Price is greater than Cost Price
            If sellingPrice <= costPrice Then
                MessageBox.Show("Selling Price must be greater than Cost Price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtSellingPrice.Focus()
                Return
            End If

            ' Validate Expiration Date (must be at least 1 year in the future)
            Dim minimumExpirationDate As DateTime = DateTime.Now.Date.AddYears(1)
            If dtpExpirationDate.Value.Date < minimumExpirationDate Then
                MessageBox.Show("Expiration Date must be at least 1 year from today.",
                               "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                dtpExpirationDate.Focus()
                Return
            End If

            ' Validate Delivery Date
            If dtpDeliveryDate.Value.Date <> DateTime.Now.Date Then
                MessageBox.Show("Delivery Date must be today's date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                dtpDeliveryDate.Focus()
                Return
            End If

            ' Extract other values from input fields
            Dim companyName As String = cmbSupplierID.Text  ' Changed from supplierName to companyName
            Dim productName As String = cmbProductID.Text
            Dim quantity As Integer = Convert.ToInt32(txtQuantity.Text.Trim())

            ' Update the row in the DataGridView
            Dim selectedRow As DataGridViewRow = dgvPendingItems.Rows(currentEditRowIndex)

            selectedRow.Cells("ReceivedBy").Value = SessionData.fullName  ' Use current user from SessionData
            selectedRow.Cells("SupplierID").Value = cmbSupplierID.SelectedValue
            selectedRow.Cells("CompanyName").Value = companyName  ' Changed from SupplierName to CompanyName
            selectedRow.Cells("ProductID").Value = cmbProductID.SelectedValue
            selectedRow.Cells("ProductName").Value = productName
            selectedRow.Cells("Quantity").Value = quantity
            selectedRow.Cells("SellingPrice").Value = sellingPrice
            selectedRow.Cells("CostPrice").Value = costPrice
            selectedRow.Cells("ExpirationDate").Value = dtpExpirationDate.Value
            selectedRow.Cells("DeliveryDate").Value = dtpDeliveryDate.Value

            MessageBox.Show("Delivery updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear input fields after updating
            ClearFields()

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' Check if DataGridView is empty
            If dgvPendingItems.Rows.Count = 0 Then
                MessageBox.Show("No deliveries to save. Please add items to the list.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get the Transaction Number from the label
            Dim transactionNumber As String = lblTransactionNumber.Text ' Example: Using a Label for TransactionNumber

            ' Log the action of saving the delivery with Transaction Number
            Dim currentFullName As String = SessionData.fullName  ' Replace with actual method to get the full name
            Dim currentRole As String = SessionData.role  ' Replace with actual method to get the current user's role
            Logaudittrail(currentRole, currentFullName, "Saving a delivery with Transaction Number: " & transactionNumber)

            ' Loop through each row in the DataGridView and save the data
            For Each row As DataGridViewRow In dgvPendingItems.Rows
                ' Ensure the row is not empty
                If Not row.IsNewRow Then
                    ' Get ReceivedBy from DataGridView
                    Dim receivedBy As String = If(row.Cells("ReceivedBy").Value IsNot Nothing, row.Cells("ReceivedBy").Value.ToString(), String.Empty)

                    ' Get ExpirationDate directly from DataGridView
                    Dim expirationDate As DateTime
                    If row.Cells("ExpirationDate").Value IsNot Nothing AndAlso DateTime.TryParse(row.Cells("ExpirationDate").Value.ToString(), expirationDate) Then
                        ' Expiration Date is valid, proceed
                    Else
                        MessageBox.Show("Expiration Date is required and must be a valid date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    ' Prepare the SQL Insert Query for Deliveries
                    Dim deliveryQuery As String = "INSERT INTO Deliveries (TransactionNumber, ReceivedBy, SupplierID, CompanyName, ProductID, ProductName, Quantity, SellingPrice, CostPrice, ExpirationDate, DeliveryDate) " &
                                              "VALUES (@TransactionNumber, @ReceivedBy, @SupplierID, @CompanyName, @ProductID, @ProductName, @Quantity, @SellingPrice, @CostPrice, @ExpirationDate, @DeliveryDate)"

                    Dim productInsertQuery As String = "INSERT INTO Products (ProductName, CategoryID, Expiration) " &
                                   "VALUES (@ProductName, @CategoryID, @Expiration)"


                    ' Prepare SQL Insert Query for Inventory
                    Dim inventoryInsertQuery As String = "INSERT INTO Inventory (ProductID, SupplierID, QuantityInStock, ExpirationDate, Barcode, UnitPrice) " &
                                                     "VALUES (@ProductID, @SupplierID, @Quantity, @ExpirationDate, @Barcode, @UnitPrice)"

                    ' Prepare SQL Insert Query for Products


                    ' Create a new connection to the database
                    Using connection As New SqlConnection(connectionString)
                        ' Open the connection before starting the transaction
                        connection.Open()

                        ' Start a transaction to ensure atomicity of all operations
                        Using transaction As SqlTransaction = connection.BeginTransaction()
                            Try
                                ' Insert the delivery record
                                Using deliveryCommand As New SqlCommand(deliveryQuery, connection, transaction)
                                    ' Add parameters for Deliveries
                                    deliveryCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber)
                                    deliveryCommand.Parameters.AddWithValue("@ReceivedBy", SessionData.fullName)  ' Use current user from SessionData
                                    deliveryCommand.Parameters.AddWithValue("@SupplierID", row.Cells("SupplierID").Value)
                                    deliveryCommand.Parameters.AddWithValue("@CompanyName", row.Cells("CompanyName").Value)
                                    deliveryCommand.Parameters.AddWithValue("@ProductID", row.Cells("ProductID").Value)
                                    deliveryCommand.Parameters.AddWithValue("@ProductName", row.Cells("ProductName").Value)
                                    deliveryCommand.Parameters.AddWithValue("@Quantity", Convert.ToInt32(row.Cells("Quantity").Value))
                                    deliveryCommand.Parameters.AddWithValue("@SellingPrice", Convert.ToDecimal(row.Cells("SellingPrice").Value))
                                    deliveryCommand.Parameters.AddWithValue("@CostPrice", Convert.ToDecimal(row.Cells("CostPrice").Value))
                                    deliveryCommand.Parameters.AddWithValue("@Barcode", row.Cells("Barcode").Value.ToString())
                                    deliveryCommand.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(row.Cells("UnitPrice").Value))
                                    deliveryCommand.Parameters.AddWithValue("@ExpirationDate", expirationDate)
                                    deliveryCommand.Parameters.AddWithValue("@DeliveryDate", dtpDeliveryDate.Value)

                                    ' Execute Insert Query for Deliveries
                                    deliveryCommand.ExecuteNonQuery()
                                End Using


                                ' Insert the product record
                                Using productCommand As New SqlCommand(productInsertQuery, connection, transaction)
                                    ' Add parameters for Products
                                    productCommand.Parameters.AddWithValue("@ProductName", row.Cells("ProductName").Value)
                                    productCommand.Parameters.AddWithValue("@CategoryID", If(row.Cells("CategoryID")?.Value, DBNull.Value)) ' Pass CategoryID or NULL
                                    'productCommand.Parameters.AddWithValue("@Image", If(row.Cells("Image")?.Value, DBNull.Value)) ' Pass Image or NULL
                                    productCommand.Parameters.AddWithValue("@Expiration", If(row.Cells("Expiration")?.Value, DBNull.Value)) ' Pass Expiration or NULL

                                    ' Execute Insert Query for Products
                                    productCommand.ExecuteNonQuery()
                                End Using


                                ' Insert the inventory record
                                Using inventoryCommand As New SqlCommand(inventoryInsertQuery, connection, transaction)
                                    ' Add parameters for Inventory
                                    inventoryCommand.Parameters.AddWithValue("@ProductID", row.Cells("ProductID").Value)
                                    inventoryCommand.Parameters.AddWithValue("@SupplierID", row.Cells("SupplierID").Value)
                                    inventoryCommand.Parameters.AddWithValue("@Quantity", Convert.ToInt32(row.Cells("Quantity").Value))
                                    inventoryCommand.Parameters.AddWithValue("@ExpirationDate", expirationDate)
                                    inventoryCommand.Parameters.AddWithValue("@Barcode", row.Cells("Barcode").Value.ToString())
                                    inventoryCommand.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(row.Cells("UnitPrice").Value))

                                    ' Execute Insert Query for Inventory
                                    inventoryCommand.ExecuteNonQuery()
                                End Using

                                ' Commit the transaction
                                transaction.Commit()

                            Catch ex As SqlException
                                ' Rollback the transaction if an error occurs
                                transaction.Rollback()
                                MessageBox.Show("SQL Error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return
                            Catch ex As Exception
                                ' Rollback the transaction if an error occurs
                                transaction.Rollback()
                                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return
                            End Try
                        End Using
                    End Using
                End If
            Next

            ' Clear the fields after saving
            ClearFields()

            ' Clear the DataGridView rows after saving
            dgvPendingItems.Rows.Clear()

            MessageBox.Show("Deliveries saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("An error occurred while saving the delivery: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function IsBarcodeExists(barcode As String) As Boolean
        Dim query As String = "SELECT COUNT(1) FROM Inventory WHERE Barcode = @Barcode"
        Using connection As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@Barcode", barcode)
                Try
                    connection.Open()
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                Catch ex As Exception
                    MessageBox.Show("Error checking barcode: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            End Using
        End Using
    End Function


    Private Sub Logaudittrail(ByVal role As String, ByVal fullName As String, ByVal action As String)
        Try
            'Dim role As String = SessionData.role
            'Dim fullName As String = SessionData.fullName
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO audittrail (Role, FullName, Action, Form, Date) VALUES (@Role, @FullName, @action, @Form, @Date)"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@Role", role)
                    cmd.Parameters.AddWithValue("@FullName", fullName)
                    cmd.Parameters.AddWithValue("@action", action)
                    cmd.Parameters.AddWithValue("@Form", "Delivery Form")
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBarcode.KeyDown
        ' Check if the pressed key is Enter (usually used as the final character after scanning the barcode)
        If e.KeyCode = Keys.Enter Then
            ' Only proceed if the barcode text length is greater than 0
            If txtBarcode.Text.Length > 0 Then
                ' Step 1: Load the product list into the ComboBox
                LoadProducts()

                ' Step 2: Search for the product based on the barcode
                Dim barcode As String = txtBarcode.Text
                Dim query As String = "SELECT ProductID FROM Products WHERE Barcode = @barcode"

                Try
                    ' Replace with your actual database connection string
                    Using conn As New SqlConnection("Data Source=LAPTOP-HC3L03IC\SQLEXPRESS;Initial Catalog=updated;Integrated Security=True;Encrypt=True;Trust Server Certificate=True")
                        Using cmd As New SqlCommand(query, conn)
                            cmd.Parameters.AddWithValue("@barcode", barcode)
                            conn.Open()

                            ' Execute the query and get the ProductID
                            Dim result As Object = cmd.ExecuteScalar()

                            If result IsNot Nothing Then
                                ' Set the ProductID to the ComboBox directly
                                cmbProductID.SelectedValue = result
                            Else

                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    ' Handle potential errors (e.g., connection issues)
                    MessageBox.Show("An error occurred while accessing the database: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If

            ' Optional: Prevent the 'Enter' key from producing a beep sound after being pressed
            e.SuppressKeyPress = True
        End If
    End Sub

    '============ KEY PRESS ============
    Private Sub txtbarcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBarcode.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtBatchNumber_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then

            e.Handled = True
        End If
    End Sub

    Private Sub cmbUserFullName_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbUserFullName.KeyDown
        If e.KeyCode = Keys.F2 OrElse e.KeyCode = Keys.Enter Then
            cmbUserFullName.DroppedDown = True
        End If
    End Sub

    Private Sub cmbSupplierID_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbSupplierID.KeyDown
        If e.KeyCode = Keys.F2 OrElse e.KeyCode = Keys.Enter Then
            cmbSupplierID.DroppedDown = True
        End If
    End Sub

    Private Sub txtQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQuantity.KeyPress, txtSellingPrice.KeyPress, txtCostPrice.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)
        'Panel1.BackColor = ColorTranslator.FromHtml("#251F1F")

    End Sub
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

    Private Sub txtBarcode_TextChanged(sender As Object, e As EventArgs) Handles txtBarcode.TextChanged
        If txtBarcode.Text.Length >= 13 Then ' Optional: Avoid querying too early
            Dim productName As String = GetProductNameByBarcode(txtBarcode.Text.Trim())
            If Not String.IsNullOrEmpty(productName) Then
                prodtxt.Text = productName
            Else
                prodtxt.Text = "Product not found"
            End If
        Else
            prodtxt.Clear()
        End If
    End Sub

    Private Function GetProductNameByBarcode(barcode As String) As String
        Dim productName As String = ""
        Dim query As String = "SELECT TOP 1 ProductName FROM [updated].[dbo].[Products] WHERE Barcode = @Barcode"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Barcode", barcode)

                Try
                    conn.Open()
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        productName = result.ToString()
                    End If
                Catch ex As Exception
                    MessageBox.Show("Database error: " & ex.Message)
                End Try
            End Using
        End Using

        Return productName
    End Function

    Private Sub dtpDeliveryDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpDeliveryDate.ValueChanged
        ' If user tries to change the date, set it back to today
        If dtpDeliveryDate.Value.Date <> DateTime.Now.Date Then
            dtpDeliveryDate.Value = DateTime.Now.Date
            MessageBox.Show("Delivery date must be today's date.", "Date Fixed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class
