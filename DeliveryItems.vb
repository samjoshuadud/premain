Imports Microsoft.Data.SqlClient

Public Class DeliveryItems
    Dim transactionNumber As String
    Private connectionString As String = AppConfig.ConnectionString

    ' Constructor
    Public Sub New(transNo As String)
        InitializeComponent()
        Me.transactionNumber = transNo
    End Sub

    Private Sub GenerateBatchNumber()
        lblBatchNumber.Text = "BATCH-" & DateTime.Now.ToString("yyyyMMddHHmmss")
    End Sub

    ' Form Load Event
    Private Sub DeliveryItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblTransactionNumber.Text = "Transaction Number: " & transactionNumber
        FetchSupplierInfo(transactionNumber) ' Fetch Supplier Name when form loads
        GenerateBatchNumber()

        ' Initialize columns for dgvPendingItems, including Expiration Date
        InitializeDataGridViewColumns()
    End Sub

    Private Sub InitializeDataGridViewColumns()
        If dgvPendingItems.Columns.Count = 0 Then
            dgvPendingItems.Columns.Add("ProductId", "Product ID")
            dgvPendingItems.Columns.Add("ProductName", "Product Name")
            dgvPendingItems.Columns.Add("Quantity", "Quantity") ' Ensure this column exists
            dgvPendingItems.Columns.Add("UnitPrice", "Unit Price")
            dgvPendingItems.Columns.Add("TotalPrice", "Total Price") ' Ensure this column exists
            dgvPendingItems.Columns.Add("ExpirationDate", "Expiration Date") ' Ensure Expiration Date column exists
        End If
    End Sub




    ' Fetch Supplier Name Based on Transaction Number
    Private Sub FetchSupplierInfo(transNo As String)
        Using con As New SqlConnection(connectionString)
            Dim query As String = "SELECT s.SupplierName 
                                   FROM Deliveries d 
                                   INNER JOIN Suppliers s ON d.SupplierId = s.SupplierId 
                                   WHERE d.TransactionNumber = @transNo"

            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@transNo", transNo)

                Try
                    con.Open()
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        lblSupplier.Text = "Supplier Name: " & result.ToString()
                    Else
                        lblSupplier.Text = "Supplier Name: N/A"
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error fetching supplier: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub


    Private Sub txtBarcodeScan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBarcodeScan.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True ' Prevents the beep sound when pressing Enter

            ' Fetch product info based on the scanned barcode
            FetchProductInfo(txtBarcodeScan.Text.Trim()) ' Ensure no leading/trailing space
        End If
    End Sub


    ' Function to fetch product information based on the barcode
    Private Sub FetchProductInfo(barcode As String)
        ' Check if barcode is empty or whitespace
        If String.IsNullOrWhiteSpace(barcode) Then
            MessageBox.Show("Please scan a barcode.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Define the SQL connection
        Using con As New SqlConnection(connectionString)
            ' SQL query to fetch product data based on barcode
            Dim query As String = "SELECT ProductId, ProductName, UnitPrice, CostPrice, RetailPrice FROM Products WHERE Barcode = @barcode"

            ' Prepare the command to execute the query
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@barcode", barcode)

                Try
                    ' Open the connection and execute the query
                    con.Open()
                    Dim reader As SqlDataReader = cmd.ExecuteReader()

                    If reader.Read() Then
                        ' Set product ID in label's Tag property
                        lblProduct.Tag = reader("ProductId")

                        ' Display fetched data to labels with consistent formatting
                        lblProduct.Text = "Product Name: " & reader("ProductName").ToString()
                        lblUnitPrice.Text = "Unit Price: ₱" & Convert.ToDecimal(reader("UnitPrice")).ToString("N2")
                        lblCostPrice.Text = "Cost Price: ₱" & Convert.ToDecimal(reader("CostPrice")).ToString("N2")
                        lblRetailPrice.Text = "Retail Price: ₱" & Convert.ToDecimal(reader("RetailPrice")).ToString("N2")

                        ' Ask user if they want to add quantity
                        Dim result As DialogResult = MessageBox.Show("Do you want to add quantity?", "Add Quantity", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                        If result = DialogResult.Yes Then
                            ' Clear the quantity textbox when user clicks Yes
                            txtQuantity.Clear()

                            ' Set focus to the quantity textbox
                            txtQuantity.Visible = True
                            txtQuantity.Focus()
                        Else
                            ' If user selects No, automatically set quantity to 1 and proceed to expiration date
                            txtQuantity.Text = "1"  ' Automatically set quantity to 1
                            txtQuantity.Visible = True
                            ShowExpirationDatePrompt()
                        End If

                        ' After quantity input, calculate total price
                        CalculateTotalPrice()

                        ' Add handler to detect when Enter is pressed in the quantity textbox
                        AddHandler txtQuantity.KeyDown, AddressOf txtQuantity_KeyDown
                    Else
                        ' If no product is found, show a message and clear labels
                        MessageBox.Show("Product not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        ClearProductLabels()
                    End If

                    reader.Close()
                Catch ex As Exception
                    ' Show an error message if the query execution fails
                    MessageBox.Show("Error fetching product: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    ' Function to calculate total price (unit price * quantity)
    Private Sub CalculateTotalPrice()
        Dim unitPrice As Decimal
        If Decimal.TryParse(lblUnitPrice.Text.Replace("Unit Price: ₱", "").Trim(), unitPrice) Then
            Dim quantity As Integer
            If Integer.TryParse(txtQuantity.Text, quantity) Then
                ' Calculate total price
                Dim totalPrice As Decimal = unitPrice * quantity
                lblTotalPrice.Text = "Total Price: ₱" & totalPrice.ToString("N2")
            End If
        End If
    End Sub


    ' Event handler for when the user presses "Enter" in the quantity input field (txtQuantity)
    Private Sub txtQuantity_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            ' Ensure the quantity is a valid number
            Dim quantity As Integer
            If Integer.TryParse(txtQuantity.Text, quantity) Then
                ' Compute total price (quantity * retail price)
                CalculateTotalPrice()

                ' Hide the quantity input field
                txtQuantity.Visible = True

                ' Show the expiration date prompt
                ShowExpirationDatePrompt()
            Else
                MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub



    ' Show expiration date prompt
    Private Sub ShowExpirationDatePrompt()
        ' Confirm expiration date input
        Dim expirationDate As DateTime = dpExpirationDate.Value

        If expirationDate <= DateTime.Now Then
            MessageBox.Show("Expiration date must be in the future.", "Invalid Expiration Date", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub ' Prevent adding the product if expiration date is invalid
        End If

        ' Ask for confirmation to add product with expiration date
        Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to add this product with the selected expiration date?", "Confirm Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            ' Add product to DataGridView
            AddProductToDataGridView(Convert.ToInt32(txtQuantity.Text), expirationDate)
        End If
    End Sub
    Private Sub AddProductToDataGridView(quantity As Integer, expirationDate As DateTime)
        ' Extract product details
        Dim productId As Integer = Convert.ToInt32(lblProduct.Tag)
        Dim productName As String = lblProduct.Text.Replace("Product Name: ", "")
        Dim unitPrice As Decimal
        Decimal.TryParse(lblUnitPrice.Text.Replace("Unit Price: ₱", "").Trim(), unitPrice)

        ' Compute total price
        Dim totalPrice As Decimal = quantity * unitPrice

        ' Ensure Expiration Date column exists
        If dgvPendingItems.Columns("ExpirationDate") Is Nothing Then
            dgvPendingItems.Columns.Add("ExpirationDate", "Expiration Date")
        End If

        ' Check if product exists in DataGridView
        Dim foundRow As DataGridViewRow = Nothing
        For Each row As DataGridViewRow In dgvPendingItems.Rows
            If row.Cells("ProductId").Value IsNot Nothing AndAlso Convert.ToInt32(row.Cells("ProductId").Value) = productId Then
                foundRow = row
                Exit For
            End If
        Next

        If foundRow IsNot Nothing Then
            ' Update existing row
            Dim existingQty As Integer = Convert.ToInt32(foundRow.Cells("Quantity").Value)
            Dim newQty As Integer = existingQty + quantity
            Dim newTotalPrice As Decimal = newQty * unitPrice

            foundRow.Cells("Quantity").Value = newQty
            foundRow.Cells("TotalPrice").Value = newTotalPrice.ToString("N2")
            foundRow.Cells("ExpirationDate").Value = expirationDate.ToString("yyyy-MM-dd") ' Update Expiration Date

            MessageBox.Show("Product quantity updated.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else


            ' Add new row
            dgvPendingItems.Rows.Add(productId, productName, quantity, unitPrice.ToString("N2"), totalPrice.ToString("N2"), expirationDate.ToString("yyyy-MM-dd"))

            MessageBox.Show("Product added successfully.", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    ' Confirmation button logic (btnConfirm)
    Private Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        ' Ensure product ID is valid (product must be scanned first)
        If lblProduct.Tag Is Nothing OrElse lblProduct.Tag = 0 Then
            MessageBox.Show("Please scan a product first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Ensure quantity is valid, if not set it to 1
        Dim quantity As Integer = 1 ' Default to 1
        If Not Integer.TryParse(txtQuantity.Text, quantity) OrElse quantity <= 0 Then
            If String.IsNullOrEmpty(txtQuantity.Text) Then
                quantity = 1 ' If the quantity is empty, default to 1
            Else
                MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        ' Ensure expiration date is in the future
        If dpExpirationDate.Value <= DateTime.Now Then
            MessageBox.Show("Expiration date must be in the future.", "Invalid Expiration Date", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Set default values for Critical Level and Reorder Level
        Dim criticalLevel As Integer = 5 ' Default Critical Level = 5
        Dim reorderLevel As Integer = 20 ' Default Reorder Level = 20

        ' Override if the user entered valid values
        If Integer.TryParse(txtMinStockLevel.Text, criticalLevel) AndAlso criticalLevel >= 0 Then
            ' User provided valid input
        Else
            txtMinStockLevel.Text = criticalLevel.ToString() ' Set default value in textbox
        End If

        If Integer.TryParse(txtReorder.Text, reorderLevel) AndAlso reorderLevel >= 0 Then
            ' User provided valid input
        Else
            txtReorder.Text = reorderLevel.ToString() ' Set default value in textbox
        End If

        ' Ensure Critical Level is not greater than Reorder Level
        If criticalLevel > reorderLevel Then
            MessageBox.Show("Critical Level cannot be greater than Reorder Level.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get product details
        Dim productId As Integer = Convert.ToInt32(lblProduct.Tag)
        Dim productName As String = lblProduct.Text.Replace("Product Name: ", "")
        Dim unitPrice As Decimal = Convert.ToDecimal(lblUnitPrice.Text.Replace("Unit Price: ₱", "").Trim())
        Dim totalPrice As Decimal = unitPrice * quantity
        Dim expirationDate As DateTime = dpExpirationDate.Value

        ' Add product to DataGridView with Critical Level & Reorder Level
        dgvPendingItems.Rows.Add(productId, productName, quantity, unitPrice, totalPrice, expirationDate, criticalLevel, reorderLevel)

        ' Optionally, show a success message
        MessageBox.Show("Product has been successfully added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Clear input fields after confirmation
        txtQuantity.Clear()
        dpExpirationDate.Value = DateTime.Now
        txtMinStockLevel.Text = criticalLevel.ToString() ' Reset to default
        txtReorder.Text = reorderLevel.ToString() ' Reset to default


    End Sub


    ' Function to clear product labels if no product is found
    Private Sub ClearProductLabels()

        txtQuantity.Clear()

        ' Set the text properties of labels to default or empty values
        lblSupplier.Text = "" ' Assuming lblSupplier is a label, otherwise use the appropriate control (TextBox or ComboBox)
        lblTransactionNumber.Text = "" ' Same for lblTransactionNumber
        lblBatchNumber.Text = "" ' Same for lblBatchNumber

        lblProduct.Text = "Product Name: N/A"
        lblUnitPrice.Text = "Unit Price: ₱0.00"
        lblCostPrice.Text = "Cost Price: ₱0.00"
        lblRetailPrice.Text = "Retail Price: ₱0.00"
        lblBatchNumber.Text = "Batch Number: N/A" ' Clear batch number if product is not found
    End Sub


    'DITOOOOOO GAGAWIN NEXT YUNG DATE.
    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Dim connectionString As String = "Data Source=LAPTOP-HC3L03IC\SQLEXPRESS;Initial Catalog=smgs;Integrated Security=True;Trust Server Certificate=True"

        If dgvPendingItems.Rows.Count = 0 Then
            MessageBox.Show("No items to save.", "Empty List", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Are you sure you want to save all pending items to the delivery records?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            If String.IsNullOrWhiteSpace(lblTransactionNumber.Text) OrElse String.IsNullOrWhiteSpace(lblBatchNumber.Text) Then
                MessageBox.Show("Transaction Number or Batch Number is missing!", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim transactionNumber As String = lblTransactionNumber.Text.Trim()
            Dim batchNumber As String = lblBatchNumber.Text.Trim()
            Dim deliveryID As Integer
            Dim supplierID As Integer
            Dim deliveryDate As Date

            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Retrieve the most recent supplierID
                Dim getLastSupplierQuery As String = "SELECT TOP 1 supplierid FROM deliveries ORDER BY deliveryid DESC"
                Using cmd As New SqlCommand(getLastSupplierQuery, conn)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        supplierID = Convert.ToInt32(result)
                    Else
                        MessageBox.Show("No Supplier ID found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                End Using

                ' Retrieve the most recent deliveryID and deliveryDate from deliveries
                Dim getLastDeliveryQuery As String = "SELECT TOP 1 deliveryid, deliverydate FROM deliveries ORDER BY deliveryid DESC"
                Using cmd As New SqlCommand(getLastDeliveryQuery, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            deliveryID = Convert.ToInt32(reader("deliveryid"))
                            deliveryDate = Convert.ToDateTime(reader("deliverydate"))
                        Else
                            MessageBox.Show("No Delivery ID found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                    End Using
                End Using

                ' Insert each pending item into deliveryitems table
                Dim insertQuery As String = "INSERT INTO deliveryitems (deliveryid, transactionnumber, supplierid, productid, quantity, unitprice, costprice, totalprice, batchnumber, expirationdate, deliverydate) 
            VALUES (@deliveryid, @transactionnumber, @supplierid, @productid, @quantity, @unitprice, @costprice, @totalprice, @batchnumber, @expirationdate, @deliverydate)"

                For Each row As DataGridViewRow In dgvPendingItems.Rows
                    If row.IsNewRow Then Continue For

                    Dim productId As Integer = Convert.ToInt32(row.Cells("ProductID").Value)
                    Dim quantity As Integer = Convert.ToInt32(row.Cells("Quantity").Value)
                    Dim unitPrice As Decimal = Convert.ToDecimal(row.Cells("UnitPrice").Value)
                    Dim totalPrice As Decimal = Convert.ToDecimal(row.Cells("TotalPrice").Value)
                    Dim expirationDate As Object = row.Cells("ExpirationDate").Value

                    ' Retrieve the cost price from Products Table
                    Dim costPrice As Decimal = 0
                    Dim getCostPriceQuery As String = "SELECT costprice FROM products WHERE productid = @productid"
                    Using costCmd As New SqlCommand(getCostPriceQuery, conn)
                        costCmd.Parameters.AddWithValue("@productid", productId)
                        Dim costResult As Object = costCmd.ExecuteScalar()
                        If costResult IsNot Nothing AndAlso Not IsDBNull(costResult) Then
                            costPrice = Convert.ToDecimal(costResult)
                        Else
                            MessageBox.Show("No Cost Price found for Product ID: " & productId, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Continue For
                        End If
                    End Using

                    ' Set expiration date (ensure it's not null or empty)
                    Dim expDate As Object = If(expirationDate Is Nothing OrElse IsDBNull(expirationDate), DBNull.Value, Convert.ToDateTime(expirationDate))

                    ' Insert each item into deliveryitems table with the same delivery date for all items
                    Using cmd As New SqlCommand(insertQuery, conn)
                        cmd.Parameters.AddWithValue("@deliveryid", deliveryID)
                        cmd.Parameters.AddWithValue("@transactionnumber", transactionNumber)
                        cmd.Parameters.AddWithValue("@supplierid", supplierID)
                        cmd.Parameters.AddWithValue("@productid", productId)
                        cmd.Parameters.AddWithValue("@quantity", quantity)
                        cmd.Parameters.AddWithValue("@unitprice", unitPrice)
                        cmd.Parameters.AddWithValue("@costprice", costPrice)
                        cmd.Parameters.AddWithValue("@totalprice", totalPrice)
                        cmd.Parameters.AddWithValue("@batchnumber", batchNumber)
                        cmd.Parameters.AddWithValue("@expirationdate", expDate)
                        cmd.Parameters.AddWithValue("@deliverydate", deliveryDate) ' Same delivery date for all items

                        cmd.ExecuteNonQuery()
                    End Using

                    ' Check if the product exists in the inventory
                    Dim checkProductQuery As String = "SELECT COUNT(*) FROM Inventory WHERE ProductID = @productid"
                    Using checkCmd As New SqlCommand(checkProductQuery, conn)
                        checkCmd.Parameters.AddWithValue("@productid", productId)
                        Dim productExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If productExists = 0 Then
                            ' If product doesn't exist, insert into Inventory
                            Dim insertInventoryQuery As String = "INSERT INTO Inventory (ProductID, QuantityInStock, SupplierID, ExpirationDate, DeliveryDate, LastUpdated) 
                        VALUES (@productid, @quantity, @supplierid, @expirationdate, @deliverydate, GETDATE())"
                            Using insertInventoryCmd As New SqlCommand(insertInventoryQuery, conn)
                                insertInventoryCmd.Parameters.AddWithValue("@productid", productId)
                                insertInventoryCmd.Parameters.AddWithValue("@quantity", quantity)
                                insertInventoryCmd.Parameters.AddWithValue("@supplierid", supplierID)
                                insertInventoryCmd.Parameters.AddWithValue("@expirationdate", expDate)
                                insertInventoryCmd.Parameters.AddWithValue("@deliverydate", deliveryDate) ' Include delivery date
                                insertInventoryCmd.ExecuteNonQuery()
                            End Using
                        Else
                            ' Update product in Inventory
                            Dim updateInventoryQuery As String = "UPDATE Inventory 
                        SET QuantityInStock = QuantityInStock + @quantity, ExpirationDate = @expirationdate, DeliveryDate = @deliverydate, LastUpdated = GETDATE() 
                        WHERE ProductID = @productid"
                            Using updateInventoryCmd As New SqlCommand(updateInventoryQuery, conn)
                                updateInventoryCmd.Parameters.AddWithValue("@quantity", quantity)
                                updateInventoryCmd.Parameters.AddWithValue("@productid", productId)
                                updateInventoryCmd.Parameters.AddWithValue("@expirationdate", expDate)
                                updateInventoryCmd.Parameters.AddWithValue("@deliverydate", deliveryDate) ' Update delivery date
                                updateInventoryCmd.ExecuteNonQuery()
                            End Using
                        End If
                    End Using
                Next

                MessageBox.Show("All items successfully saved to the database and inventory updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                dgvPendingItems.Rows.Clear()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saving items: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    '===============================================================================================

End Class
