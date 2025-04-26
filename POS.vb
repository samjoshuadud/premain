Imports Emgu.CV.Features2D
Imports Microsoft.Data.SqlClient
Imports System.Data
Imports System.Drawing.Printing
Imports System.Text
Imports System.IO

Public Class POS
    Inherits Form

    Private connectionString As String = AppConfig.ConnectionString
    Private connection As SqlConnection

    ' Add this declaration at the top of the class  
    Private lastScanTime As DateTime = DateTime.Now

    ' Database Helper
    Private dbHelper As DatabaseHelper = DatabaseHelper.Instance
    Private cart As DataTable ' To store cart items in memory
    Private transactionNumber As String

    ' Cashier Information
    Private CurrentCashierId As Integer

    ' FOR ROLE 
    Private role As String

    ' Wholesale mode flag and discount percentage
    Private isWholesaleMode As Boolean = False
    Private wholesaleMinimumQuantity As Integer = 50 ' Minimum quantity to qualify for wholesale
    Private autoApplyWholesale As Boolean = True ' Automatically apply wholesale pricing for bulk orders

    Public Class Sale
        Public ID As Integer
        Public Quantity As Integer
    End Class



    ' Constructor
    Public Sub New(userRole As String)
        role = userRole
        InitializeComponent() ' Automatically initializes the designer elements 

        ' Add wholesale mode toggle
        AddWholesaleModeToggle()
    End Sub

    ' Constructor
    Public Sub New(cashierId As Integer)
        InitializeComponent()
        Me.CurrentCashierId = cashierId
        ' Initialize UI and backend
        GenerateTransactionNumber()
        InitializeCashierDetails()
        Dim lblCashier As New Label() ' Make sure this is declared in the form's designer

        ' Add wholesale mode toggle
        AddWholesaleModeToggle()
    End Sub


    '===================== FOR DATE ==============================================
    Private Sub InitializeDate()
        Try
            ' Set the label to show the current date in the desired format
            lblDate.Text = $"Date: {DateTime.Now.ToString("MMMM dd, yyyy")}"
        Catch ex As Exception
            MessageBox.Show($"Error fetching date: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '====================== FOR TRANSACTION NUMBER =======================================

    ' Generate Transaction Number
    Private Sub GenerateTransactionNumber()
        transactionNumber = "TXN-" & DateTime.Now.ToString("yyyyMMddHHmmss")
        lblTransactionNumber.Text = "Transaction #: " & transactionNumber
    End Sub



    '===================== FOR CASHIER DETEILS ==============================================

    Private Sub InitializeCashierDetails()
        Try
            Dim query As String = "SELECT fullname, role FROM users WHERE userid = @userId"
            Dim parameters As SqlParameter() = {New SqlParameter("@userId", CurrentCashierId)}
            Dim userData As DataTable = dbHelper.ExecuteQuery(query, parameters)

            If userData.Rows.Count > 0 Then
                Dim fullName As String = userData.Rows(0)("fullname").ToString()
                Dim role As String = userData.Rows(0)("role").ToString()
                lblCashier.Text = $" {fullName} ({role})"
            Else
                Throw New Exception("User details not found!")
            End If
        Catch ex As Exception
            MessageBox.Show($"Error fetching user details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub



    '=====================  FOR DGV ======================================================

    ' Customize DataGridView appearance
    Private Sub CustomizeDataGridView()
        With dgvCart
            ' Header Styles
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Black
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Regular)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Cell Styles
            .DefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .DefaultCellStyle.WrapMode = DataGridViewTriState.False ' Prevent text wrapping
            .GridColor = Color.LightBlue
            .CellBorderStyle = DataGridViewCellBorderStyle.Single

            ' Row Settings
            .RowTemplate.Height = 40.0
            .RowHeadersVisible = False

            ' Auto-size columns but keep fixed width for images
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        End With
    End Sub

    ' Add Edit and Delete Image Columns
    Private Sub AddImageColumns()
        ' Delete column
        Dim deleteColumn As New DataGridViewImageColumn()
        deleteColumn.HeaderText = ""
        deleteColumn.Name = "Delete"
        Dim deleteimagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-delete-35.png")
        deleteColumn.Image = Image.FromFile(deleteimagePath) ' <-- path to your image
        deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        dgvCart.Columns.Insert(0, deleteColumn) ' Insert at first position
        dgvCart.Columns("Delete").Width = 80 ' Set width of Delete column to 80

        ' Edit column
        Dim editColumn As New DataGridViewImageColumn()
        editColumn.HeaderText = ""
        editColumn.Name = "Edit"
        editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
        editColumn.Image = Image.FromFile(imagePath) ' <-- path to your image
        dgvCart.Columns.Insert(1, editColumn) ' Insert at second position
        dgvCart.Columns("Edit").Width = 45 ' Set width of Edit column to 45
    End Sub



    Private Sub dgvCart_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCart.CellClick
        ' Check if the Delete column was clicked
        If e.ColumnIndex = dgvCart.Columns("Delete").Index AndAlso e.RowIndex >= 0 Then
            ' Ask for confirmation before deleting
            If MessageBox.Show("Are you sure you want to remove this item?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ' Remove the selected row
                dgvCart.Rows.RemoveAt(e.RowIndex)
                ' Update the cart summary after deletion
                UpdateCartSummary()
            End If

            ' Check if the Edit column was clicked
        ElseIf e.ColumnIndex = dgvCart.Columns("Edit").Index AndAlso e.RowIndex >= 0 Then
            ' Get the selected row data
            Dim selectedRow As DataGridViewRow = dgvCart.Rows(e.RowIndex)

            ' Clear the highlighting from all rows
            For Each row As DataGridViewRow In dgvCart.Rows
                row.DefaultCellStyle.BackColor = Color.White
            Next

            ' Highlight the selected row
            selectedRow.DefaultCellStyle.BackColor = Color.LightYellow

            ' Populate the textboxes for editing
            txtBarcode.Text = selectedRow.Cells("Barcode").Value.ToString()
            txtQuantity.Text = selectedRow.Cells("Quantity").Value.ToString()

            ' Show the quantity panel for editing
            ShowQuantityPanel(txtBarcode.Text)

            ' Add a label to indicate edit mode
            If PanelQuantity.Controls.ContainsKey("lblProductInfo") Then
                Dim lblInfo As Label = DirectCast(PanelQuantity.Controls("lblProductInfo"), Label)
                lblInfo.Text = "EDITING: " & lblInfo.Text
                lblInfo.ForeColor = Color.Yellow
            End If

            ' If confirm button exists, update its text for edit mode
            If PanelQuantity.Controls.ContainsKey("btnConfirmQuantity") Then
                Dim btnConfirm As Button = DirectCast(PanelQuantity.Controls("btnConfirmQuantity"), Button)
                btnConfirm.Text = "Update Item"
                btnConfirm.BackColor = Color.Orange
            End If
        End If

        ' Ensure a valid row and column index
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = dgvCart.Columns(e.ColumnIndex).Name

            ' Show PanelDiscount when clicking anywhere in the Discount column
            If columnName = "Discount" Then
                DiscountPanel.Visible = True
                DiscountPanel.BringToFront()
            End If
        End If
    End Sub


    '==================== FOR CART AND DGV ===================================================

    ' Initialize Cart
    Private Sub InitializeCart()
        If cart Is Nothing Then
            ' Initialize DataTable with necessary columns
            cart = New DataTable()
            cart.Columns.Add("#", GetType(Integer))
            cart.Columns.Add("Barcode", GetType(String))
            cart.Columns.Add("ItemName", GetType(String))
            cart.Columns.Add("Quantity", GetType(Integer))
            cart.Columns.Add("UnitPrice", GetType(Decimal))
            cart.Columns.Add("Discount", GetType(Decimal))
            cart.Columns.Add("Total", GetType(Decimal))

            ' Bind cart DataTable to DataGridView
            dgvCart.DataSource = cart

            ' Move this part AFTER setting DataSource
            AddImageColumns()

            ' Customize DataGridView appearance
            CustomizeDataGridView()

            ' Set specific column widths
            dgvCart.Columns(0).Width = 25  ' Set width of first column ("#")
            dgvCart.Columns(1).Width = 100 ' Set width of Barcode column
            dgvCart.Columns(2).Width = 150 ' Set width of ItemName column
            dgvCart.Columns(3).Width = 75  ' Set width of Quantity column
            dgvCart.Columns(4).Width = 100 ' Set width of UnitPrice column
            dgvCart.Columns(5).Width = 100 ' Set width of Discount column
            dgvCart.Columns(6).Width = 100 ' Set width of Total column

            ' Hide the Barcode, Discount, and Total columns during load
            dgvCart.Columns("Barcode").Visible = False
            dgvCart.Columns("Discount").Visible = False
            dgvCart.Columns("Total").Visible = False
            dgvCart.Columns("Edit").Width = 75 ' Set width of Edit column to 45
            dgvCart.Columns("Delete").Width = 75 ' Set width of Edit column to 45
            dgvCart.Columns("#").Width = 85 ' Set width of Edit column to 45
            dgvCart.Columns("Quantity").Width = 100 ' Set width of Edit column to 45
            dgvCart.Columns("UnitPrice").Width = 100 ' Set width of Edit column to 45



        End If
    End Sub




    ' Declare a global variable to store the original subtotal
    Private originalSubtotal As Decimal = 0

    Private Sub UpdateCartSummary()
        ' Get discount and VAT rates
        Dim discountRate As Decimal = GetCurrentDiscount() ' Example: 10% or 20%
        Dim vatRate As Decimal = 12 ' VAT rate set to 12%

        ' Initialize variables
        Dim totalBeforeDiscount As Decimal = 0
        Dim totalDiscount As Decimal = 0
        Dim totalVAT As Decimal = 0

        ' Calculate the total before discount and VAT (Item * Quantity)
        For Each row As DataRow In cart.Rows
            Dim quantity As Integer = row.Field(Of Integer)("Quantity")
            Dim unitPrice As Decimal = row.Field(Of Decimal)("UnitPrice")
            Dim itemTotal As Decimal = quantity * unitPrice

            ' Calculate VAT for this item (per product), but don't add to the total
            Dim itemVAT As Decimal = (itemTotal * vatRate) / 100
            totalVAT += itemVAT

            ' Accumulate total before discount (price without VAT)
            totalBeforeDiscount += itemTotal
        Next

        ' Store the original subtotal (before discount or VAT)
        originalSubtotal = totalBeforeDiscount

        ' Compute the discount based on subtotal before VAT
        Dim discount As Decimal = (discountRate / 100) * totalBeforeDiscount
        totalDiscount = discount

        ' Debug: Check if discount is applied correctly
        Console.WriteLine("Discount: " & discount.ToString("₱ 0.00"))

        ' Calculate the discounted subtotal: Subtotal - Discount
        Dim discountedSubtotal As Decimal = totalBeforeDiscount - totalDiscount

        ' The final total should be subtotal after discount (without VAT)
        Dim finalTotal As Decimal = discountedSubtotal

        ' Display values with proper formatting and ₱ symbol
        lblSubtotal.Text = "₱ " & totalBeforeDiscount.ToString("0.00")  ' Subtotal without discount or VAT
        lblVAT.Text = "₱ " & totalVAT.ToString("0.00")                 ' Total VAT for all products (displayed separately)
        lblDiscount.Text = "₱ " & totalDiscount.ToString("0.00")        ' Discount amount
        lblTotal.Text = "₱ " & finalTotal.ToString("0.00")              ' Final total after discount (without VAT)
        lblTotalItems.Text = cart.Rows.Count.ToString() ' Display total number of items

        ' Debug: Output the final total to console to check if it matches expectations
        Console.WriteLine("Final Total (without VAT included): " & finalTotal.ToString("₱ 0.00"))
    End Sub





    ' For Cart
    ' This is the global variable that stores the final total after payment
    Private finalProcessedTotal As Decimal = 0

    Private Sub AddToCart(barcode As String)
        If String.IsNullOrWhiteSpace(barcode) Then Return

        If cart Is Nothing Then InitializeCart()

        Try
            Dim query As String = "SELECT i.Barcode, i.UnitPrice, i.QuantityInStock, " &
                              "p.ProductName, i.WholesaleDiscount " &
                              "FROM Inventory i " &
                              "INNER JOIN Products p ON i.ProductID = p.ProductID " &
                              "WHERE i.Barcode = @barcode"

            Dim parameters As SqlParameter() = {New SqlParameter("@barcode", barcode)}
            Dim productTable As DataTable = dbHelper.ExecuteQuery(query, parameters)

            If productTable.Rows.Count = 0 Then
                MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim productName As String = productTable.Rows(0)("ProductName").ToString()
            Dim unitPrice As Decimal = Convert.ToDecimal(productTable.Rows(0)("UnitPrice"))
            Dim wholesaleDiscount As Decimal = 0

            If productTable.Rows(0)("WholesaleDiscount") IsNot DBNull.Value Then
                wholesaleDiscount = Convert.ToDecimal(productTable.Rows(0)("WholesaleDiscount"))
            End If

            Dim availableQuantity As Integer = Convert.ToInt32(productTable.Rows(0)("QuantityInStock"))
            Dim manualQuantity As Integer = 1

            If Not String.IsNullOrWhiteSpace(txtQuantity.Text) Then
                manualQuantity = Convert.ToInt32(txtQuantity.Text)
            End If

            Dim finalPrice As Decimal = unitPrice
            Dim wholesaleApplied As Boolean = False

            If (isWholesaleMode AndAlso manualQuantity >= wholesaleMinimumQuantity) OrElse
           (autoApplyWholesale AndAlso manualQuantity >= wholesaleMinimumQuantity) Then
                finalPrice = unitPrice * (1 - (wholesaleDiscount / 100))
                wholesaleApplied = True
            ElseIf isWholesaleMode AndAlso manualQuantity < wholesaleMinimumQuantity Then
                MessageBox.Show($"Wholesale pricing requires a minimum of {wholesaleMinimumQuantity} items. " &
                            $"Regular pricing will be applied for {manualQuantity} items.",
                            "Wholesale Minimum Not Met", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If manualQuantity > availableQuantity Then
                MessageBox.Show("Insufficient stock. Only " & availableQuantity.ToString() & " items are available.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtQuantity.Clear()
                Return
            End If

            Dim existingRow = cart.AsEnumerable().FirstOrDefault(Function(row) row.Field(Of String)("Barcode") = barcode)
            If existingRow IsNot Nothing Then
                Dim currentQuantity As Integer = existingRow("Quantity")
                Dim newQuantity As Integer = currentQuantity + manualQuantity

                If newQuantity > availableQuantity Then
                    MessageBox.Show("Cannot add more items. Only " & availableQuantity.ToString() & " items are available.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Dim currentUnitPrice As Decimal = existingRow("UnitPrice")
                Dim wholesalePrice As Decimal = unitPrice * (1 - (wholesaleDiscount / 100))

                If currentUnitPrice = unitPrice AndAlso newQuantity >= wholesaleMinimumQuantity AndAlso autoApplyWholesale Then
                    existingRow("UnitPrice") = wholesalePrice
                    MessageBox.Show($"Wholesale discount of {wholesaleDiscount}% has been automatically applied to {productName} " &
                                $"since quantity is now {newQuantity} (minimum {wholesaleMinimumQuantity}).",
                                "Wholesale Pricing Applied", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                existingRow("Quantity") = newQuantity
                existingRow("Total") = newQuantity * existingRow("UnitPrice")
            Else
                Dim rowIndex As Integer = cart.Rows.Count + 1
                Dim newRow = cart.Rows.Add(rowIndex, barcode, productName, manualQuantity, finalPrice, 0, manualQuantity * finalPrice)

                If wholesaleApplied Then
                    newRow("ItemName") = productName & " (Wholesale)"
                End If
            End If

            dgvCart.DataSource = Nothing
            dgvCart.DataSource = cart
            UpdateCartSummary()
            txtQuantity.Clear()
            HighlightNewItem()
            PlaySuccessSound()

        Catch ex As Exception
            MessageBox.Show("Error adding item: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        lblVAT.Visible = False
        lblVatUI.Visible = True
        txtBarcode.Focus()

        dgvCart.Columns("Barcode").Visible = False
        dgvCart.Columns("Discount").Visible = False
        dgvCart.Columns("Total").Visible = False
        dgvCart.Columns("Edit").Width = 75
        dgvCart.Columns("Delete").Width = 75
        dgvCart.Columns("#").Width = 85
        dgvCart.Columns("Quantity").Width = 100
        dgvCart.Columns("UnitPrice").Width = 100
    End Sub



    Private Sub RemoveFromCart(barcode As String)
        If String.IsNullOrWhiteSpace(barcode) Then Return ' Exit if barcode is empty or null

        ' Check if the cart is initialized
        If cart Is Nothing Then Return

        Try
            ' Find the product in the cart based on the barcode
            Dim existingRow = cart.AsEnumerable().FirstOrDefault(Function(row) row.Field(Of String)("Barcode") = barcode)

            If existingRow IsNot Nothing Then
                ' Ask for confirmation before removing the item
                Dim result As DialogResult = MessageBox.Show("Are you sure you want to remove this item from the cart?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = DialogResult.Yes Then
                    ' Remove the row from the cart
                    cart.Rows.Remove(existingRow)

                    ' Refresh the DataGridView to show the updated cart
                    dgvCart.DataSource = Nothing
                    dgvCart.DataSource = cart

                    ' Update the cart summary (total, VAT, discount, etc.)
                    UpdateCartSummary()

                    ' Play a sound to indicate successful removal
                    PlaySuccessSound()

                    ' Optionally, you can highlight the removed item or provide feedback
                    MessageBox.Show("Item successfully removed from the cart.", "Item Removed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("Product not found in cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Error removing item: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub











    '==================== FOR GetProductId ==================================================
    Private Function GetProductId(barcode As String) As Integer
        Try
            ' SQL query to fetch the product ID based on the barcode from the Inventory table
            Dim query As String = "SELECT I.ProductID " &
                              "FROM Inventory I " &
                              "WHERE I.Barcode = @barcode"

            ' Define the parameter for the query
            Dim parameters As SqlParameter() = {New SqlParameter("@barcode", barcode)}

            ' Execute the query and get the result (using ExecuteScalar for a single value)
            Dim result = dbHelper.ExecuteScalar(query, parameters)

            ' Check if result is found, otherwise throw an exception
            If result IsNot Nothing Then
                Return Convert.ToInt32(result)
            Else
                Throw New Exception("Product ID not found for the given barcode.")
            End If
        Catch ex As Exception
            ' Show an error message if there's an issue
            MessageBox.Show($"Error fetching product ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return -1 ' Return -1 to indicate an error
        End Try
    End Function


    '===================== FOR DICOUNT ====================================================
    Private Sub LoadDiscounts()
        Try
            ' Define the query to fetch active discounts
            Dim query As String = "SELECT discountname, discountrate FROM discounts"

            ' Get the discount data
            Dim dt As DataTable = dbHelper.GetDataTable(query)

            ' Add a default value (0% discount) at the top of the ComboBox
            Dim defaultRow As DataRow = dt.NewRow()
            defaultRow("discountname") = "No Discount"
            defaultRow("discountrate") = 0
            dt.Rows.InsertAt(defaultRow, 0)

            ' Bind the ComboBox to the discount data
            cmbDiscount.DataSource = dt
            cmbDiscount.ValueMember = "discountrate"
            cmbDiscount.DisplayMember = "discountname"  ' Display the discount name to the user

            ' Optionally set the default selection to "No Discount"
            cmbDiscount.SelectedIndex = 0

        Catch ex As Exception
            MessageBox.Show("Error loading discounts: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    Private Sub cmbDiscount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDiscount.SelectedIndexChanged
        ' Call method to update the discount label when discount is changed
        UpdateDiscountLabel()
    End Sub

    Private Sub UpdateDiscountLabel()
        ' Get the selected discount rate
        Dim discountRate As Decimal = GetCurrentDiscount()

        ' Calculate the discount based on the subtotal
        Dim discount As Decimal = (discountRate / 100) * originalSubtotal

        ' Update the lblDiscount with the calculated value
        lblDiscount.Text = discount.ToString("₱ 0.00")

        ' Update the cart summary to reflect the new discount
        UpdateCartSummary()
    End Sub

    Private Function GetCurrentDiscount() As Decimal
        Dim discountRate As Decimal = 0
        Try
            ' Check if a discount is selected in ComboBox
            If cmbDiscount.SelectedIndex >= 0 Then
                ' Ensure the value in the ComboBox is a valid decimal, else default to 0
                If cmbDiscount.SelectedValue IsNot Nothing AndAlso IsNumeric(cmbDiscount.SelectedValue) Then
                    discountRate = Convert.ToDecimal(cmbDiscount.SelectedValue)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return discountRate
    End Function





    '====================== FOR VAT ======================================================
    Private Function GetCurrentVAT() As Decimal
        Dim vatRate As Decimal = 0
        Try
            ' Fetch the most recent VAT rate based on the effective date
            Dim query As String = "SELECT TOP 1 vatrate FROM vat ORDER BY effectivedate DESC"
            Dim result As Object = dbHelper.ExecuteScalar(query)
            If result IsNot Nothing Then
                vatRate = Convert.ToDecimal(result)
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching VAT: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return vatRate
    End Function

    '====================== FOR PROCESS PAYMENT ========================================
    Private Sub BtnProcessPayment_Click(sender As Object, e As EventArgs) Handles btnProcessPayment.Click
        ' Check if cart is empty
        If cart.Rows.Count = 0 Then
            MessageBox.Show("Please add items to the cart before processing payment.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            PanelPay.Visible = False
            txtBarcode.Focus()
            Return
        End If

        Dim amountPaid As Decimal
        Dim totalAmount As Decimal = Convert.ToDecimal(lblTotal.Text.Replace("₱", "").Trim())

        ' Show waiting cursor
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        ' Validate the amount paid
        If Not Decimal.TryParse(txtAmountPaid.Text.Replace("₱", ""), amountPaid) Then
            MessageBox.Show("Please enter a valid amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Me.Cursor = Cursors.Default
            Return
        End If

        ' Check if amount is sufficient
        If amountPaid < totalAmount Then
            MessageBox.Show("Insufficient amount paid. Please enter at least ₱" & totalAmount.ToString("0.00"), "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAmountPaid.Focus()
            txtAmountPaid.SelectAll()
            Me.Cursor = Cursors.Default
            Return
        End If

        Dim discountAmount As Decimal = Convert.ToDecimal(lblDiscount.Text.Replace("₱", "")) ' Remove ₱ sign before conversion
        Dim vatAmount As Decimal = Convert.ToDecimal(lblVAT.Text.Replace("₱", "")) ' Remove ₱ sign before conversion

        ' Use the originalSubtotal for the total amount calculation
        Dim finalAmount As Decimal = totalAmount

        ' Calculate the change (amount paid - final amount)
        Dim change As Decimal = amountPaid - finalAmount
        lblChange.Text = "₱ " & change.ToString("0.00")

        ' Generate the invoice number
        Dim invoiceNumber As String = "SI# " & DateTime.Now.ToString("yy-") & GetRandomNumber(10000000)

        ' Process the payment (insert sales data and update stock)
        Try
            dbHelper.BeginTransaction()

            ' Insert sale details into the Sales table
            Dim saleQuery As String = "INSERT INTO Sales (TransactionNumber, SaleDate, CashierID, TotalAmount, DiscountAmount, VatAmount, NetAmount, Invoice) " &
                                  "VALUES (@TransactionNumber, GETDATE(), @CashierID, @TotalAmount, @DiscountAmount, @VatAmount, @NetAmount, @Invoice); " &
                                  "SELECT SCOPE_IDENTITY();"
            Dim saleParams As SqlParameter() = {
            New SqlParameter("@TransactionNumber", transactionNumber),
            New SqlParameter("@CashierID", SessionData.CurrentUserId),
            New SqlParameter("@TotalAmount", originalSubtotal),
            New SqlParameter("@DiscountAmount", discountAmount),
            New SqlParameter("@VatAmount", vatAmount),
            New SqlParameter("@NetAmount", originalSubtotal - discountAmount),
            New SqlParameter("@Invoice", invoiceNumber) ' Save the invoice number
        }
            Dim saleId As Integer = Convert.ToInt32(dbHelper.ExecuteScalar(saleQuery, saleParams))

            ' Process each item in the cart
            For Each row As DataRow In cart.Rows
                Dim productId As Integer = GetProductId(row("Barcode"))
                Dim quantity As Integer = Convert.ToInt32(row("Quantity"))
                Dim unitPrice As Decimal = Convert.ToDecimal(row("UnitPrice"))
                Dim totalPrice As Decimal = Convert.ToDecimal(row("Total"))

                ' Insert into SaleItems table
                Dim itemQuery As String = "INSERT INTO SaleItems (SaleID, ProductID, Quantity, UnitPrice, TotalPrice) " &
                                      "VALUES (@SaleID, @ProductID, @Quantity, @UnitPrice, @TotalPrice)"
                Dim itemParams As SqlParameter() = {
                New SqlParameter("@SaleID", saleId),
                New SqlParameter("@ProductID", productId),
                New SqlParameter("@Quantity", quantity),
                New SqlParameter("@UnitPrice", unitPrice),
                New SqlParameter("@TotalPrice", totalPrice)
            }
                dbHelper.ExecuteNonQuery(itemQuery, itemParams)

                ' Insert into TodaySales table
                Dim todaySalesQuery As String = "INSERT INTO TodaySales (TransactionNumber, ProductID, Quantity, UnitPrice, TotalPrice, Discount) " &
                                            "VALUES (@TransactionNumber, @ProductID, @Quantity, @UnitPrice, @TotalPrice, @Discount)"
                Dim todaySalesParams As SqlParameter() = {
                New SqlParameter("@TransactionNumber", transactionNumber),
                New SqlParameter("@ProductID", productId),
                New SqlParameter("@Quantity", quantity),
                New SqlParameter("@UnitPrice", unitPrice),
                New SqlParameter("@TotalPrice", totalPrice),
                New SqlParameter("@Discount", discountAmount / cart.Rows.Count) ' Distribute discount evenly
            }
                dbHelper.ExecuteNonQuery(todaySalesQuery, todaySalesParams)

                ' Deduct stock from Inventory
                Dim updateStockQuery As String = "UPDATE Inventory SET QuantityInStock = QuantityInStock - @Quantity WHERE ProductID = @ProductID"
                Dim updateStockParams As SqlParameter() = {
                New SqlParameter("@Quantity", quantity),
                New SqlParameter("@ProductID", productId)
            }
                dbHelper.ExecuteNonQuery(updateStockQuery, updateStockParams)
            Next

            ' Commit the transaction
            dbHelper.CommitTransaction()

            ' Print the receipt
            PrintReceipt()

            ' Log audit trail for adding a user action
            Dim actionDescription = $"Processed Payment - Transaction: {transactionNumber}, Cashier: {SessionData.fullName}, Amount Paid: {amountPaid}"
            Logaudittrail(SessionData.role, SessionData.fullName, actionDescription)

            ' Reset the form
            BtnClearCart_Click(sender, e)
            GenerateTransactionNumber()
            txtAmountPaid.Clear()
            lblChange.Text = "₱ 0.00"

        Catch ex As SqlException
            dbHelper.RollbackTransaction()
            MessageBox.Show("Database Error: " & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            dbHelper.RollbackTransaction()
            MessageBox.Show("Error processing payment: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Reset cursor
            Me.Cursor = Cursors.Default
        End Try

        PanelQuantity.Visible = False
        lblVatUI.Visible = False
        lblVAT.Visible = True
        PanelPay.Visible = False
        txtBarcode.Focus()
        lblProductInformation.Visible = False
    End Sub

    '================ FOR UDPDATE THE LABALE AND CURRENT DATE AND TIME ========================

    ' This is the function that will update the label with the current date and time
    Private Sub DisplayDateAndTime()
        lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    End Sub


    '=========================== FOR EXPIRATION =====================================
    Private Sub UpdateExpirationTracking(orders As Dictionary(Of Integer, Integer))
        Try
            For Each kvp As KeyValuePair(Of Integer, Integer) In orders
                Dim productId As Integer = kvp.Key
                Dim quantityToDeduct As Integer = kvp.Value

                ' Query to get expiration records based on FIFO (earliest expiration first)
                Dim expirationQuery As String = "
                SELECT trackingid, quantity
                FROM expiration_tracking
                WHERE productid = @productid
                ORDER BY expiration_date ASC"

                ' Use SqlParameter for SQL Server
                Dim expirationParams As SqlParameter() = {New SqlParameter("@productid", productId)}

                ' Execute the query to fetch expiration tracking data
                Dim expirationData As DataTable = dbHelper.ExecuteQuery(expirationQuery, expirationParams)

                For Each row As DataRow In expirationData.Rows
                    If quantityToDeduct <= 0 Then
                        Exit For
                    End If

                    Dim trackingId As Integer = Convert.ToInt32(row("trackingid"))
                    Dim availableQuantity As Integer = Convert.ToInt32(row("quantity"))

                    Dim quantityToReduce As Integer = Math.Min(availableQuantity, quantityToDeduct)
                    quantityToDeduct -= quantityToReduce

                    ' Query to update the quantity in expiration_tracking
                    Dim updateExpirationQuery As String = "
                    UPDATE expirationtracking
                    SET quantity = quantity - @quantityToReduce
                    WHERE trackingid = @trackingId"

                    ' Use SqlParameter for SQL Server
                    Dim updateParams As SqlParameter() = {
                    New SqlParameter("@quantityToReduce", quantityToReduce),
                    New SqlParameter("@trackingId", trackingId)
                }

                    ' Execute the update query to reduce the quantity
                    dbHelper.ExecuteNonQuery(updateExpirationQuery, updateParams)
                Next
            Next
        Catch ex As Exception
            Throw New Exception($"Error updating expiration tracking: {ex.Message}")
        End Try
    End Sub


    '================= FOR IS PRODUCT DELIVERED ========================================
    Private Function IsProductDelivered(productId As Integer) As Boolean
        ' SQL query to check if the product is delivered by counting the entries in delivery_items
        Dim query As String = "SELECT COUNT(*) FROM deliveryitems WHERE productid = @productid"

        ' Replace MySqlParameter with SqlParameter for SQL Server
        Dim parameters As SqlParameter() = {New SqlParameter("@productid", productId)}

        Try
            ' Execute the query and get the count of deliveries for the product
            Dim count As Integer = Convert.ToInt32(dbHelper.ExecuteScalar(query, parameters))

            ' If the count is greater than 0, the product is delivered
            Return count > 0
        Catch ex As Exception
            ' Show an error message in case of an exception
            MessageBox.Show("Error checking product delivery status: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function


    '====================== FOR INVETORY ====================================================
    Private Sub UpdateInventory(order As Dictionary(Of Integer, Integer))
        For Each productId In order.Keys
            Dim soldQuantity = order(productId)

            ' Update the inventory by reducing the quantity sold
            Dim updateInventoryQuery = "UPDATE inventory SET QuantityInStock = QuantityInStock - @soldQuantity WHERE productid = @productid"
            Dim updateInventoryParams = {
            New SqlParameter("@soldQuantity", soldQuantity),
            New SqlParameter("@productid", productId)
        }
            dbHelper.ExecuteNonQuery(updateInventoryQuery, updateInventoryParams)
        Next
    End Sub


    '====================== FOR PRINT ==================================================

    Private WithEvents PrintDocument As New PrintDocument
    Private WithEvents PrintPreviewDialog As New PrintPreviewDialog
    Private PrinterSettings As New PrinterSettings()


    Private Function GenerateReceiptContent() As String
        Dim sb As New StringBuilder()

        ' Generate invoice number (SI#)
        Dim invoiceNumber As String = "SI# " & DateTime.Now.ToString("yy-") & GetRandomNumber(10000000)
        Dim transNumber As String = "Trans# " & transactionNumber

        ' Store Information Header - Centered text with divider lines
        sb.AppendLine("       SHIENA MINI GROCERY STORE              ")
        sb.AppendLine("    16 Kakawati, Taguig, 1630 Metro Manila    ") ' Address
        sb.AppendLine("            SALES INVOICE                  ")

        ' Only show wholesale mode indicator if wholesale mode is manually enabled
        If isWholesaleMode Then
            sb.AppendLine("           ** WHOLESALE MODE **            ")
        End If

        sb.AppendLine(New String("-"c, 150))
        sb.AppendLine($"Date: {DateTime.Now:MM/dd/yyyy HH:mm:ss}")
        ' Commented out Cashier line to hide it
        ' sb.AppendLine($"Cashier: {SessionData.fullName}")  
        sb.AppendLine($"{invoiceNumber}")
        sb.AppendLine($"{transNumber}")
        sb.AppendLine(New String("-"c, 150))
        sb.AppendLine("ITEM                  QTY   PRICE    TOTAL")
        sb.AppendLine(New String("-"c, 150))

        ' Track if any wholesale discounts were applied
        Dim hasWholesaleDiscount As Boolean = False
        Dim totalWholesaleDiscount As Decimal = 0

        ' Items with proper formatting for alignment
        For Each row As DataRow In cart.Rows
            Dim itemName As String = row("ItemName").ToString()
            Dim quantity As Integer = Convert.ToInt32(row("Quantity"))
            Dim unitPrice As Decimal = Convert.ToDecimal(row("UnitPrice"))
            Dim totalPrice As Decimal = quantity * unitPrice

            ' Check if this item had wholesale discount applied
            If itemName.Contains("(Wholesale)") Then
                hasWholesaleDiscount = True

                ' Get regular price for this product to calculate the discount
                Dim barcode As String = row("Barcode").ToString()
                Dim query As String = "SELECT p.UnitPrice, p.WholesaleDiscount FROM Products p WHERE p.Barcode = @Barcode"
                Dim parameters As SqlParameter() = {New SqlParameter("@Barcode", barcode)}
                Dim productData As DataTable = dbHelper.ExecuteQuery(query, parameters)

                If productData.Rows.Count > 0 Then
                    Dim regularPrice As Decimal = Convert.ToDecimal(productData.Rows(0)("UnitPrice"))
                    Dim itemDiscount As Decimal = (regularPrice - unitPrice) * quantity
                    totalWholesaleDiscount += itemDiscount
                End If
            End If

            ' Truncate long item names and format for proper alignment
            Dim shortName As String = itemName
            If shortName.Length > 18 Then
                shortName = shortName.Substring(0, 15) & "..."
            End If
            shortName = shortName.PadRight(20, " "c)

            ' Create the formatted line with proper padding
            Dim quantityStr As String = quantity.ToString().PadLeft(5, " "c)
            Dim unitPriceStr As String = unitPrice.ToString("0.00").PadLeft(6, " "c)
            Dim totalPriceStr As String = totalPrice.ToString("0.00").PadLeft(6, " "c)
            sb.AppendLine($"{shortName}{quantityStr}   {unitPriceStr}   {totalPriceStr}")
        Next

        ' Summary section
        sb.AppendLine(New String("-"c, 150))

        Dim subtotal As Decimal = Convert.ToDecimal(lblSubtotal.Text.Replace("₱", "").Trim())
        Dim discount As Decimal = Convert.ToDecimal(lblDiscount.Text.Replace("₱", "").Trim())
        Dim vat As Decimal = Convert.ToDecimal(lblVAT.Text.Replace("₱", "").Trim())
        Dim totalDue As Decimal = Convert.ToDecimal(lblTotal.Text.Replace("₱", "").Trim())
        Dim cashAmount As Decimal = 0
        Decimal.TryParse(txtAmountPaid.Text, cashAmount)
        Dim changeAmount As Decimal = Convert.ToDecimal(lblChange.Text.Replace("₱", "").Trim())

        sb.AppendLine($"ITEMS COUNT:                 {cart.Rows.Count}")
        sb.AppendLine($"SUBTOTAL:                   ₱ {subtotal:0.00}")

        ' Only show discount if it's greater than 0
        If discount > 0 Then
            Dim discountRate As Decimal = GetCurrentDiscount()
            sb.AppendLine($"DISCOUNT ({discountRate}%):           ₱ {discount:0.00}")
        End If

        ' Add wholesale discount information if any wholesale items exist
        If hasWholesaleDiscount Then
            sb.AppendLine($"WHOLESALE SAVINGS:          ₱ {totalWholesaleDiscount:0.00}")
        End If

        sb.AppendLine($"VAT (12%):                  ₱ {vat:0.00}")

        ' NEW: Add Vatable Sales / Price Without VAT
        Dim vatableSales As Decimal = totalDue / 1.12
        sb.AppendLine($"VATABLE SALES:              ₱ {vatableSales:0.00}")

        sb.AppendLine(New String("-"c, 150))

        sb.AppendLine($"TOTAL:                      ₱ {totalDue:0.00}")
        sb.AppendLine($"CASH:                       ₱ {cashAmount:0.00}")
        sb.AppendLine($"CHANGE:                     ₱ {changeAmount:0.00}")
        sb.AppendLine(New String("-"c, 150))

        ' Additional information section
        sb.AppendLine("           THANK YOU FOR SHOPPING!           ")
        sb.AppendLine("   This serves as your official receipt.     ")

        ' Add wholesale terms if in wholesale mode
        sb.AppendLine("   Items can be returned within 7 days       ")
        sb.AppendLine("   with receipt and in original condition.   ")

        Return sb.ToString()
    End Function






    ' Helper method to generate random numbers for receipt formatting
    Private Function GetRandomNumber(ByVal maxValue As Long) As String
        Dim random As New Random()
        Dim result As String = ""

        ' For large numbers, generate each digit separately
        If maxValue > Integer.MaxValue Then
            ' Generate a string of the appropriate length
            Dim length As Integer = maxValue.ToString().Length
            For i As Integer = 0 To length - 1
                result &= random.Next(0, 10).ToString()
            Next
        Else
            ' For smaller numbers, use standard Random.Next
            result = random.Next(CInt(maxValue)).ToString()
        End If

        ' Pad with leading zeros if needed
        Return result.PadLeft(maxValue.ToString().Length, "0"c)
    End Function



    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim receiptContent As String = GenerateReceiptContent()

        ' Font sizes
        Dim maxTitleFontSize As Single = 8.0F
        Dim maxRegularFontSize As Single = 8.0F
        Dim maxSmallFontSize As Single = 7.0F

        ' Print area bounds
        Dim printAreaLeft As Single = e.PageBounds.Left
        Dim printAreaRight As Single = e.PageBounds.Right
        Dim printWidth As Single = e.PageBounds.Width
        Dim currentY As Single = e.PageBounds.Top

        ' Split the content into lines
        Dim lines As String() = receiptContent.Split(New String() {Environment.NewLine}, StringSplitOptions.None)

        ' Keywords for smaller font size
        Dim smallTextKeywords As String() = {"TIN:", "POS Terminal:", "Date:", "Cashier:", "SI#:", "Trans#:"}

        ' Loop through each line of receipt content
        For Each originalLineRaw As String In lines
            Dim originalLine As String = originalLineRaw.Trim()
            Dim baseFontSize As Single = maxRegularFontSize
            Dim fontStyle As FontStyle = FontStyle.Bold

            ' Decide font size based on content
            If originalLine.Equals("SHIENA MINI GROCERY STORE", StringComparison.OrdinalIgnoreCase) OrElse
           originalLine.Equals("SALES INVOICE", StringComparison.OrdinalIgnoreCase) OrElse
           originalLine.Equals("16 Kakawati, Taguig, 1630 Metro Manila", StringComparison.OrdinalIgnoreCase) OrElse
           originalLine.Contains("WHOLESALE MODE") OrElse
           originalLine.Contains("THANK YOU") Then
                baseFontSize = maxTitleFontSize
            ElseIf smallTextKeywords.Any(Function(k) originalLine.StartsWith(k)) Then
                baseFontSize = maxSmallFontSize
            End If

            ' Create font and measure string width
            Dim font As New Font("Courier New", baseFontSize, fontStyle)
            Dim lineWidth As Single = e.Graphics.MeasureString(originalLine, font).Width

            ' Shrink font if line is too wide
            While lineWidth > printWidth AndAlso font.Size > 2
                font = New Font("Courier New", font.Size - 0.5F, fontStyle)
                lineWidth = e.Graphics.MeasureString(originalLine, font).Width
            End While

            ' Decide alignment
            Dim drawX As Single = printAreaLeft

            ' Right-aligned for "CASH" and "CHANGE"
            If originalLine.ToUpper().Contains("CASH") OrElse originalLine.ToUpper().Contains("CHANGE") Then
                ' Push "CASH" and "CHANGE" to the far right edge of the page
                drawX = printAreaRight - lineWidth
            ElseIf originalLine.ToUpper().Contains("SUBTOTAL") OrElse originalLine.ToUpper().Contains("TOTAL") Then
                ' Left-aligned for "SUBTOTAL" and "TOTAL"
                drawX = printAreaLeft
            ElseIf originalLine.Equals("SHIENA MINI GROCERY STORE", StringComparison.OrdinalIgnoreCase) OrElse
                originalLine.Equals("SALES INVOICE", StringComparison.OrdinalIgnoreCase) OrElse
                originalLine.Equals("16 Kakawati, Taguig, 1630 Metro Manila", StringComparison.OrdinalIgnoreCase) OrElse
                originalLine.Contains("WHOLESALE MODE") OrElse
                originalLine.Contains("THANK YOU") Then
                ' Center-aligned for header text
                drawX = printAreaLeft + (printWidth - lineWidth) / 2
            Else
                ' Left-aligned for all other lines
                drawX = printAreaLeft
            End If

            ' Draw the text line
            e.Graphics.DrawString(originalLine, font, Brushes.Black, drawX, currentY)
            currentY += font.GetHeight() + 1

            ' Check if content goes beyond page bounds
            If currentY > e.PageBounds.Bottom Then
                e.HasMorePages = True
                Exit Sub
            End If
        Next

        e.HasMorePages = False
    End Sub













    Private Sub PrintReceipt()
        Try
            ' Set up print document settings
            AddHandler PrintDocument.PrintPage, AddressOf PrintDocument_PrintPage
            PrintDocument.PrinterSettings = PrinterSettings
            PrintPreviewDialog.Document = PrintDocument
            PrintPreviewDialog.ShowDialog()
            'PrintDocument.Print()
        Catch ex As Exception
            MessageBox.Show("Error printing receipt: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    '====================== FOR KEY CHANGE ===================================
    Private Sub txtAmountPaid_TextChanged(sender As Object, e As EventArgs) Handles txtAmountPaid.TextChanged
        Dim amountPaid As Decimal

        ' Kunin ang amount paid, at i-check kung valid number
        If Not Decimal.TryParse(txtAmountPaid.Text, amountPaid) Then
            lblChange.Text = "₱ 0.00"
            Return
        End If
    End Sub

    '======================= FOR KEYPRESS ====================================

    Private Sub txtBarcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBarcode.KeyPress
        ' Allow only numbers and control keys (like backspace)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtQuantity_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQuantity.KeyPress
        ' Allow numbers, control keys, and only one hyphen (-) at the beginning
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            ' Allow hyphen (-) only at the start and only once
            If e.KeyChar = "-"c AndAlso txtQuantity.SelectionStart = 0 AndAlso Not txtQuantity.Text.Contains("-") Then
                ' Allow hyphen
            Else
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub txtAmountPaid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmountPaid.KeyPress
        ' Check if the key pressed is a control key (backspace, delete, etc.) or a number
        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then
            ' Allow control keys (like Backspace) and digits
            Return
        End If

        ' Allow the user to type only one dot "."
        If e.KeyChar = "."c Then
            ' If there is already a dot in the TextBox, block the second dot
            If txtAmountPaid.Text.Contains("."c) Then
                e.Handled = True ' Block this dot
            End If
        End If

        ' If it's anything else (letters, symbols, etc.), prevent input
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
            e.Handled = True ' Block invalid characters (letters, special characters, etc.)
        End If
    End Sub

    '======================== FOR KEYDOWN ====================================

    Private Sub txtBarcode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBarcode.KeyDown
        ' Check if the Enter key is pressed
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True ' Prevent "ding" sound on Enter key press

            ' Check for scan timeout
            If (DateTime.Now - lastScanTime).TotalMilliseconds > 500 Then
                ' Don't add to cart immediately, show the quantity panel instead
                If Not String.IsNullOrWhiteSpace(txtBarcode.Text.Trim()) Then
                    ' Show the quantity panel and focus on quantity field
                    ShowQuantityPanel(txtBarcode.Text.Trim())
                End If

                ' Update the last scan time
                lastScanTime = DateTime.Now
            End If
        End If
    End Sub

    ' New method to show the quantity panel with the current barcode
    Private Sub ShowQuantityPanel(barcode As String)
        Dim productExists As Boolean = False
        Dim productName As String = ""
        Dim unitPrice As Decimal = 0
        Dim wholesaleDiscount As Decimal = 0 ' Default discount
        Dim availableStock As Integer = 0

        Try
            Dim query As String = "SELECT i.Barcode, i.UnitPrice, i.QuantityInStock, " &
                              "p.ProductName, i.WholesaleDiscount " &
                              "FROM Inventory i " &
                              "INNER JOIN Products p ON i.ProductID = p.ProductID " &
                              "WHERE i.Barcode = @barcode"

            Dim parameters As SqlParameter() = {New SqlParameter("@barcode", barcode)}
            Dim productTable As DataTable = dbHelper.ExecuteQuery(query, parameters)

            If productTable.Rows.Count > 0 Then
                productExists = True
                productName = productTable.Rows(0)("ProductName").ToString()
                unitPrice = Convert.ToDecimal(productTable.Rows(0)("UnitPrice"))
                availableStock = Convert.ToInt32(productTable.Rows(0)("QuantityInStock"))

                If productTable.Rows(0)("WholesaleDiscount") IsNot DBNull.Value Then
                    wholesaleDiscount = Convert.ToDecimal(productTable.Rows(0)("WholesaleDiscount"))
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error checking product: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If Not productExists Then
            MessageBox.Show("Product not found for barcode: " & barcode, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtBarcode.Clear()
            Return
        End If

        txtQuantity.Text = "1"
        txtQuantity.Font = New Font("Segoe UI", 10, FontStyle.Bold)

        Dim wholesalePrice As Decimal = unitPrice * (1 - (wholesaleDiscount / 100))

        lblProductInfo.Text = $"Product Name: {productName.ToUpper()}{Environment.NewLine}" &
                          $"Regular Price: ₱{unitPrice:0.00}{Environment.NewLine}" &
                          $"Wholesale Price: ₱{wholesalePrice:0.00} (min {wholesaleMinimumQuantity} items, {wholesaleDiscount}% off){Environment.NewLine}" &
                          $"In Stock: {availableStock}"

        lblProductInfo.ForeColor = Color.Black
        lblProductInfo.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblProductInfo.Visible = True

        PanelQuantity.Visible = True
        PanelQuantity.BringToFront()
        PanelQuantity.BackColor = Color.White

        txtQuantity.Focus()
        txtQuantity.SelectAll()
    End Sub

    Private Sub txtQuantity_KeyDown(sender As Object, e As KeyEventArgs) Handles txtQuantity.KeyDown
        ' Check if Enter key is pressed
        If e.KeyCode = Keys.Enter Then
            ' Call AddToCart to add or update the quantity of the item
            If Not String.IsNullOrWhiteSpace(txtBarcode.Text) AndAlso
           Not String.IsNullOrWhiteSpace(txtQuantity.Text) Then

                ' Validate that quantity is a valid integer (positive or negative)
                Dim quantity As Integer
                If Integer.TryParse(txtQuantity.Text, quantity) Then
                    ' Check if the quantity is not zero
                    If quantity <> 0 Then
                        ' If the quantity is positive, add the item to the cart
                        If quantity > 0 Then
                            AddToCart(txtBarcode.Text)
                        Else
                            ' If quantity is negative, update the existing cart item by reducing the quantity
                            Dim existingRow = cart.AsEnumerable().FirstOrDefault(Function(row) row.Field(Of String)("Barcode") = txtBarcode.Text)
                            If existingRow IsNot Nothing Then
                                ' Decrease the quantity but ensure it doesn't go below zero
                                Dim currentQuantity As Integer = existingRow("Quantity")
                                Dim newQuantity As Integer = currentQuantity + quantity ' Subtract quantity (if negative)

                                If newQuantity >= 0 Then
                                    existingRow("Quantity") = newQuantity
                                    existingRow("Total") = newQuantity * existingRow("UnitPrice") ' Update the total based on the new quantity
                                Else
                                    ' Prevent quantity from going below zero
                                    MessageBox.Show("Quantity cannot be less than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If
                            Else
                                ' If the item is not in the cart, show a message
                                MessageBox.Show("Product not found in the cart.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If

                        ' Hide the quantity panel and reset the input fields
                        PanelQuantity.Visible = False
                        txtBarcode.Clear()
                        txtBarcode.Focus()
                    Else
                        ' If quantity is zero, show a message
                        MessageBox.Show("Please enter a quantity greater than zero (positive or negative).", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtQuantity.Focus()
                        txtQuantity.SelectAll()
                    End If
                Else
                    ' If the quantity input is not a valid integer
                    MessageBox.Show("Please enter a valid integer quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtQuantity.Focus()
                    txtQuantity.SelectAll()
                End If

            Else
                MessageBox.Show("Barcode or quantity is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' Prevent the beep sound when pressing Enter
            e.SuppressKeyPress = True
        End If
    End Sub


    Private Sub txtAmountPaid_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAmountPaid.KeyDown
        ' Check if the pressed key is the Backspace key
        If e.KeyCode = Keys.Back Then
            ' If backspace is pressed, remove the last character from the TextBox
            If txtAmountPaid.Text.Length > 0 Then
                txtAmountPaid.Text = txtAmountPaid.Text.Substring(0, txtAmountPaid.Text.Length - 1)
            End If
        End If
        ' Prevent Copy (Ctrl + C) and Paste (Ctrl + V) actions
        If e.Control AndAlso (e.KeyCode = Keys.C OrElse e.KeyCode = Keys.V) Then
            e.SuppressKeyPress = True ' This will suppress the key press (i.e., block the action)
        End If
    End Sub


    '===================PANELS========================================='


    '===================PANEL BUTTONS FUNTIONS ======================='
    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    DiscountPanel.Visible = True
    'End Sub
    'Private Sub Button2_Click(sender As Object, e As EventArgs)
    '    DiscountPanel.Visible = False
    'End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PanelPay.Visible = True
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        PanelPay.Visible = False
    End Sub
    'Private Sub Button5_Click(sender As Object, e As EventArgs)
    '    DiscountPanel.Visible = False
    'End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        txtBarcode.Clear()
        PanelQuantity.Visible = False
        lblProductInfo.Visible = False
        txtBarcode.Focus()
    End Sub

    'Button Clear Cart
    Private Sub BtnClearCart_Click(sender As Object, e As EventArgs) Handles btnClearcart.Click
        cart.Clear()
        UpdateCartSummary()
        GenerateTransactionNumber()

    End Sub



    '===================== FOR RESET =========================================
    Private Sub reset()
        ' Clear the input fields
        txtBarcode.Text = String.Empty
        txtQuantity.Text = String.Empty
        GenerateTransactionNumber()
    End Sub


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
                    cmd.Parameters.AddWithValue("@Form", "User Form")
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnCancelOrder_Click(sender As Object, e As EventArgs) Handles btnCancelOrder.Click
        ' Create a new instance of the CancelOrder form
        Dim TodaySales As New TodaySales

        ' Option 1: Show the CancelOrder form (non-modal)
        TodaySales.Show()

    End Sub



    ' Event Handlers for number buttons
    Private Sub Button_Click(sender As Object, e As EventArgs) Handles btn9.Click, btn8.Click, btn34.Click, btn6.Click, btn7.Click, btn4.Click, btn3.Click, btn2.Click, btn1.Click, btn0.Click
        ' Append the clicked number to the TextBox
        txtAmountPaid.Text &= CType(sender, Button).Text
    End Sub

    ' Event Handler for the Decimal point button
    Private Sub btnDot_Click(sender As Object, e As EventArgs) Handles btnDot.Click
        If Not txtAmountPaid.Text.Contains(".") Then
            txtAmountPaid.Text &= "."
        End If
    End Sub

    ' Event Handler for the Clear button
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear2.Click
        ' Remove the last character from the TextBox if there is any text
        If txtAmountPaid.Text.Length > 0 Then
            txtAmountPaid.Text = txtAmountPaid.Text.Substring(0, txtAmountPaid.Text.Length - 1)
        End If
    End Sub


    Private Sub btn1_Click(sender As Object, e As EventArgs) Handles btn9.Click
        ' Append the clicked number to the TextBox
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn2_Click(sender As Object, e As EventArgs) Handles btn8.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn3_Click(sender As Object, e As EventArgs) Handles btn34.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn4_Click(sender As Object, e As EventArgs) Handles btn6.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn5_Click(sender As Object, e As EventArgs) Handles btn7.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn6_Click(sender As Object, e As EventArgs) Handles btn4.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn7_Click(sender As Object, e As EventArgs) Handles btn3.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn8_Click(sender As Object, e As EventArgs) Handles btn2.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn9_Click(sender As Object, e As EventArgs) Handles btn1.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub btn0_Click(sender As Object, e As EventArgs) Handles btn0.Click
        txtAmountPaid.Text &= ""
    End Sub

    Private Sub PanelPay_Paint(sender As Object, e As PaintEventArgs) Handles PanelPay.Paint
        ' Center the PanelPay inside the form whenever it's painted
        PanelPay.Left = (Me.ClientSize.Width - PanelPay.Width) \ 2
        PanelPay.Top = (Me.ClientSize.Height - PanelPay.Height) \ 2
        PanelPay.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DiscountPanel.Visible = True

    End Sub

    Private Sub DiscountPanel_Paint(sender As Object, e As PaintEventArgs) Handles DiscountPanel.Paint
        DiscountPanel.BringToFront()
        DiscountPanel.Left = (Me.ClientSize.Width - DiscountPanel.Width) \ 2
        DiscountPanel.Top = (Me.ClientSize.Height - DiscountPanel.Height) \ 2
        DiscountPanel.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        DiscountPanel.Visible = False

    End Sub

    ' Add this method to show helpful tooltips when hovering over controls
    Private Sub SetupTooltips()
        Dim toolTip As New ToolTip()

        ' Set tooltip properties for better visibility
        toolTip.AutoPopDelay = 5000
        toolTip.InitialDelay = 500
        toolTip.ReshowDelay = 500
        toolTip.ShowAlways = True
        toolTip.IsBalloon = True
        toolTip.ToolTipIcon = ToolTipIcon.Info
        toolTip.ToolTipTitle = "Help"

        ' Add tooltips for key controls
        toolTip.SetToolTip(txtBarcode, "Scan or enter product barcode here and press Enter")
        toolTip.SetToolTip(btnProcessPayment, "Process the payment for current transaction")
        toolTip.SetToolTip(btnClearcart, "Clear all items and start a new transaction (F2)")
        toolTip.SetToolTip(Button3, "Open payment screen to complete transaction (F3)")
        toolTip.SetToolTip(Button1, "Apply discount to this transaction (F4)")
        toolTip.SetToolTip(btnCancelOrder, "View today's sales report")
        toolTip.SetToolTip(dgvCart, "Click Edit or Delete icons to modify cart items")
        toolTip.SetToolTip(txtQuantity, "Enter the quantity for this item")
        toolTip.SetToolTip(txtAmountPaid, "Enter the amount paid by customer")

        ' Add tooltip for wholesale mode checkbox
        Dim chkWholesale As CheckBox = DirectCast(Me.Controls("chkWholesaleMode"), CheckBox)
        toolTip.SetToolTip(chkWholesale, $"Force wholesale pricing for all items regardless of quantity. " &
                                        $"(Normally wholesale pricing is automatically applied when ordering {wholesaleMinimumQuantity}+ of an item)")

        ' Add tooltips for keyboard shortcuts
        Dim shortcutsLabel As New Label()
        shortcutsLabel.Name = "lblShortcuts"
        shortcutsLabel.Text = "Keyboard Shortcuts"
        shortcutsLabel.AutoSize = True
        shortcutsLabel.Location = New Point(750, 575)
        shortcutsLabel.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        shortcutsLabel.ForeColor = Color.DarkBlue
        shortcutsLabel.Cursor = Cursors.Help
        Me.Controls.Add(shortcutsLabel)

        toolTip.SetToolTip(shortcutsLabel, "F3: New Transaction" & vbCrLf &
                                          "F3: Payment Screen" & vbCrLf &
                                          "F4: Discount Screen" & vbCrLf &
                                          "F5: Quantity Screen" & vbCrLf &
                                          "F6: Toggle Manual Wholesale Mode" & vbCrLf &
                                          "ESC: Close any open panel")
    End Sub

    ' Add this method to enhance the visual styling of the form
    Private Sub EnhanceVisualStyle()
        ' Style the main form
        Me.BackColor = Color.WhiteSmoke

        ' Style the DataGridView
        With dgvCart
            .BackgroundColor = Color.White
            .EnableHeadersVisualStyles = False
            .ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5, FontStyle.Regular)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            .BorderStyle = BorderStyle.Fixed3D
            .GridColor = Color.LightGray
        End With

        ' Style the barcode input panel
        Panel1.BackColor = Color.White
        Panel1.BorderStyle = BorderStyle.Fixed3D
        txtBarcode.Font = New Font("Segoe UI", 12, FontStyle.Regular)
        txtBarcode.BackColor = Color.White

        ' Style the summary panel
        Panel2.BackColor = Color.White
        Panel2.BorderStyle = BorderStyle.Fixed3D

        ' Style the labels in Panel2 with a consistent look
        For Each ctrl As Control In Panel2.Controls
            If TypeOf ctrl Is Label Then
                Dim lbl As Label = DirectCast(ctrl, Label)
                If lbl.Name.StartsWith("lbl") Then
                    lbl.Font = New Font("Segoe UI", 14, FontStyle.Bold)

                    ' Highlight important financial labels
                    If lbl.Name = "lblTotal" Then
                        lbl.ForeColor = Color.Red
                        lbl.Font = New Font("Segoe UI", 16, FontStyle.Bold)
                    ElseIf lbl.Name = "lblSubtotal" Or lbl.Name = "lblDiscount" Or lbl.Name = "lblVAT" Or lbl.Name = "lblChange" Then
                        lbl.ForeColor = Color.Black
                    End If
                Else
                    lbl.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    lbl.ForeColor = Color.DimGray
                End If
            End If
        Next

        ' Style the quantity panel
        PanelQuantity.BackColor = Color.White
        PanelQuantity.BorderStyle = BorderStyle.Fixed3D
        txtQuantity.Font = New Font("Segoe UI", 14, FontStyle.Regular)
        txtQuantity.TextAlign = HorizontalAlignment.Center

        ' Style the payment panel
        PanelPay.BackColor = Color.White
        PanelPay.BorderStyle = BorderStyle.Fixed3D
        txtAmountPaid.Font = New Font("Segoe UI", 20, FontStyle.Bold)
        txtAmountPaid.BackColor = Color.LightBlue
        txtAmountPaid.TextAlign = HorizontalAlignment.Center

        ' Style the transaction label
        lblTransactionNumber.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblTransactionNumber.ForeColor = Color.Red

        ' Style the discount panel
        DiscountPanel.BackColor = Color.White
        DiscountPanel.BorderStyle = BorderStyle.Fixed3D

        ' Style action buttons with consistent look
        StyleActionButtons()
    End Sub

    ' Style all action buttons consistently
    Private Sub StyleActionButtons()
        ' Get all buttons on the form
        Dim buttons As New List(Of Button)
        GetAllControls(Me, buttons)

        ' Style regular buttons
        For Each btn As Button In buttons
            ' Skip number pad buttons
            If btn.Name.StartsWith("btn") AndAlso Not (btn.Name = "btn0" OrElse btn.Name = "btn1" OrElse
                btn.Name = "btn2" OrElse btn.Name = "btn3" OrElse btn.Name = "btn4" OrElse
                btn.Name = "btn5" OrElse btn.Name = "btn6" OrElse btn.Name = "btn7" OrElse
                btn.Name = "btn8" OrElse btn.Name = "btn9" OrElse btn.Name = "btnDot") Then

            End If
        Next

        ' Style the number pad buttons
        For i As Integer = 0 To 9
            Dim numButton As Button = DirectCast(Me.Controls.Find($"btn{i}", True).FirstOrDefault(), Button)
            If numButton IsNot Nothing Then
                numButton.BackColor = Color.White
                numButton.FlatStyle = FlatStyle.Flat
                numButton.FlatAppearance.BorderColor = Color.LightGray
                numButton.FlatAppearance.BorderSize = 1
                numButton.Font = New Font("Segoe UI", 14, FontStyle.Bold)
                numButton.Cursor = Cursors.Hand
            End If
        Next

        ' Style the Button3 (Pay button)
        If Button3 IsNot Nothing Then
            Button3.BackColor = Color.FromArgb(0, 192, 0)
            Button3.FlatStyle = FlatStyle.Flat
            Button3.FlatAppearance.BorderSize = 0
            Button3.ForeColor = Color.White
            Button3.Font = New Font("Segoe UI", 12, FontStyle.Bold)
            Button3.Cursor = Cursors.Hand
        End If
    End Sub

    ' Helper method to get all controls of a specific type
    Private Sub GetAllControls(Of T As Control)(container As Control, ByRef list As List(Of T))
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is T Then
                list.Add(DirectCast(ctrl, T))
            End If

            If ctrl.Controls.Count > 0 Then
                GetAllControls(ctrl, list)
            End If
        Next
    End Sub

    ' Call the visual enhancement in the form load
    Private Sub POS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the cart and UI components
        InitializeCart()
        DisplayDateAndTime()
        LoadDiscounts()
        GenerateTransactionNumber()
        SetupTooltips()
        EnhanceVisualStyle() ' Apply visual enhancements
        btnCancelOrder.BackColor = ColorTranslator.FromHtml("#77CDFF")
        Button8.BackColor = ColorTranslator.FromHtml("#5F99AE")
        Button7.BackColor = ColorTranslator.FromHtml("#E1EEBC")
        btnClearcart.BackColor = ColorTranslator.FromHtml("#7AC6D2")


        ' Set up panels
        PanelQuantity.Visible = False
        lblVAT.Visible = True
        PrintPreviewDialog.StartPosition = FormStartPosition.CenterScreen

        ' Configure column widths
        If dgvCart.Columns.Contains("Edit") Then dgvCart.Columns("Edit").Width = 75
        If dgvCart.Columns.Contains("Delete") Then dgvCart.Columns("Delete").Width = 75
        If dgvCart.Columns.Contains("#") Then dgvCart.Columns("#").Width = 85
        If dgvCart.Columns.Contains("Quantity") Then dgvCart.Columns("Quantity").Width = 100
        If dgvCart.Columns.Contains("UnitPrice") Then dgvCart.Columns("UnitPrice").Width = 100

        ' Hide specific columns
        If dgvCart.Columns.Contains("Barcode") Then dgvCart.Columns("Barcode").Visible = False
        If dgvCart.Columns.Contains("Discount") Then dgvCart.Columns("Discount").Visible = False
        If dgvCart.Columns.Contains("Total") Then dgvCart.Columns("Total").Visible = False

        ' Set default values
        DiscountPanel.Location = New Point(29, 209)
        lblChange.Text = "₱ 0.00"

        ' Add shortcut keys information to transaction number label
        lblTransactionNumber.Text = lblTransactionNumber.Text & " (F2: New | F3: Pay | F4: Discount | F6: Wholesale)"

        ' Ensure focus on barcode input
        txtBarcode.Select()
        txtBarcode.Focus()
        Application.DoEvents()

        ' Make all labels use Segoe UI and bold font
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Label Then
                Dim lbl As Label = CType(ctrl, Label)
                lbl.Font = New Font("Segoe UI", lbl.Font.Size, FontStyle.Bold)
            End If
        Next

        ' Also check inside panels or groupboxes
        For Each container As Control In Me.Controls
            If TypeOf container Is Panel OrElse TypeOf container Is GroupBox Then
                For Each ctrl As Control In container.Controls
                    If TypeOf ctrl Is Label Then
                        Dim lbl As Label = CType(ctrl, Label)
                        lbl.Font = New Font("Segoe UI", lbl.Font.Size, FontStyle.Bold)
                    End If
                Next
            End If
        Next

        ' Initial resize to set correct positions
        POS_Resize(Nothing, Nothing)
    End Sub

    ' Add keyboard shortcuts for common actions
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        ' F2 - New Transaction (Clear Cart)
        If keyData = Keys.F2 Then
            BtnClearCart_Click(Nothing, Nothing)
            Return True
        End If

        ' F3 - Pay (Show Payment Panel)
        If keyData = Keys.F3 Then
            Button3_Click(Nothing, Nothing)
            Return True
        End If

        ' F4 - Discount (Show Discount Panel)
        If keyData = Keys.F4 Then
            Button1_Click(Nothing, Nothing)
            Return True
        End If

        ' F5 - Quantity (Show Quantity Panel)
        If keyData = Keys.F5 AndAlso Not String.IsNullOrWhiteSpace(txtBarcode.Text) Then
            ShowQuantityPanel(txtBarcode.Text)
            Return True
        End If

        ' F6 - Toggle Manual Wholesale Mode
        If keyData = Keys.F6 Then
            Dim chkWholesale As CheckBox = DirectCast(Me.Controls("chkWholesaleMode"), CheckBox)
            chkWholesale.Checked = Not chkWholesale.Checked
            Return True
        End If

        ' Escape - Close open panels
        If keyData = Keys.Escape Then
            PanelPay.Visible = False
            PanelQuantity.Visible = False
            DiscountPanel.Visible = False
            txtBarcode.Focus()
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    ' Add visual feedback when a product is added to the cart
    Private Sub HighlightNewItem()
        If dgvCart.Rows.Count > 0 Then
            ' Get the last added row (newest item)
            Dim lastRowIndex As Integer = dgvCart.Rows.Count - 1
            Dim row As DataGridViewRow = dgvCart.Rows(lastRowIndex)

            ' Temporarily highlight the row with a green background
            row.DefaultCellStyle.BackColor = Color.LightGreen

            ' Create a timer to reset the highlight after 1.5 seconds
            Dim highlightTimer As New Timer()
            highlightTimer.Interval = 1500

            ' Add event handler for the timer
            AddHandler highlightTimer.Tick, Sub(s, e)
                                                ' Reset the background color
                                                row.DefaultCellStyle.BackColor = Color.White

                                                ' If it's an alternating row, use the alternating color
                                                If lastRowIndex Mod 2 = 1 AndAlso dgvCart.AlternatingRowsDefaultCellStyle.BackColor <> Color.Empty Then
                                                    row.DefaultCellStyle.BackColor = dgvCart.AlternatingRowsDefaultCellStyle.BackColor
                                                End If

                                                ' Stop and dispose the timer
                                                DirectCast(s, Timer).Stop()
                                                DirectCast(s, Timer).Dispose()
                                            End Sub

            ' Start the timer
            highlightTimer.Start()

            ' Scroll to the new item
            dgvCart.FirstDisplayedScrollingRowIndex = lastRowIndex
            dgvCart.CurrentCell = dgvCart.Rows(lastRowIndex).Cells(2) ' Set focus to the item name cell
        End If
    End Sub

    ' Add sound feedback for successful scanning
    Private Sub PlaySuccessSound()
        Try
            ' Play the system asterisk sound
            My.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Asterisk)
        Catch ex As Exception
            ' Silently fail if sound can't play
        End Try
    End Sub

    ' Add wholesale mode toggle checkbox
    Private Sub AddWholesaleModeToggle()
        Dim chkWholesale As New CheckBox With {
            .Name = "chkWholesaleMode",
            .Text = "Manual Wholesale Mode (F6)",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .ForeColor = Color.Blue,
            .Location = New Point(550, 45),
            .Visible = True
        }

        ' Add event handler for checkbox state change
        AddHandler chkWholesale.CheckedChanged, AddressOf WholesaleModeToggle_CheckedChanged

        ' Add to the form
        Me.Controls.Add(chkWholesale)

        ' Add a label to display wholesale discount info
        Dim lblWholesaleInfo As New Label With {
            .Name = "lblWholesaleInfo",
            .Text = $"Auto wholesale: Product-specific discounts for {wholesaleMinimumQuantity}+ items",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 8, FontStyle.Italic),
            .ForeColor = Color.DarkBlue,
            .Location = New Point(550, 70),
            .Visible = True
        }

        Me.Controls.Add(lblWholesaleInfo)
    End Sub

    ' Event handler for wholesale mode toggle
    Private Sub WholesaleModeToggle_CheckedChanged(sender As Object, e As EventArgs)
        Dim chkWholesale As CheckBox = DirectCast(sender, CheckBox)
        isWholesaleMode = chkWholesale.Checked

        ' Show/hide wholesale info label
        Dim lblWholesaleInfo As Label = DirectCast(Me.Controls("lblWholesaleInfo"), Label)
        lblWholesaleInfo.Visible = isWholesaleMode

        ' Update cart if items already exist
        If cart IsNot Nothing AndAlso cart.Rows.Count > 0 Then
            If MessageBox.Show("Switching pricing mode will update all items in the cart. Continue?",
                              "Mode Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                UpdateCartPrices()
            Else
                ' Revert the checkbox if user cancels
                chkWholesale.Checked = Not isWholesaleMode
                isWholesaleMode = Not isWholesaleMode
            End If
        End If

        ' Update UI to reflect wholesale mode
        If isWholesaleMode Then
            ' Change form title to indicate wholesale mode
            Me.Text = "Point of Sale - WHOLESALE MODE"
            Panel1.BackColor = Color.LightYellow
        Else
            ' Reset form title
            Me.Text = "Point of Sale"
            Panel1.BackColor = Color.White
        End If
    End Sub

    ' Update prices in cart when switching between retail and wholesale mode
    Private Sub UpdateCartPrices()
        If cart Is Nothing OrElse cart.Rows.Count = 0 Then Return

        ' Store the barcodes and quantities of current items
        Dim items As New List(Of Tuple(Of String, Integer))

        For Each row As DataRow In cart.Rows
            Dim barcode As String = row("Barcode").ToString()
            Dim quantity As Integer = Convert.ToInt32(row("Quantity"))
            items.Add(New Tuple(Of String, Integer)(barcode, quantity))
        Next

        ' Clear cart and re-add all items to apply new pricing
        cart.Clear()

        ' Re-add each item to apply the current pricing mode
        For Each item In items
            ' Get the barcode and quantity
            Dim barcode As String = item.Item1
            Dim quantity As Integer = item.Item2

            ' Set quantity to textbox for AddToCart method
            txtQuantity.Text = quantity.ToString()

            ' Re-add the item with updated pricing
            AddToCart(barcode)
        Next

        ' Update the cart summary
        UpdateCartSummary()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Me.Close()
    End Sub

    Private Sub PanelQuantity_Paint(sender As Object, e As PaintEventArgs) Handles PanelQuantity.Paint
        ' Show the panel
        PanelQuantity.Visible = True
        PanelQuantity.BringToFront()
        PanelQuantity.BackColor = Color.White

        ' Center the panel in the form
        PanelQuantity.Left = (Me.ClientSize.Width - PanelQuantity.Width) \ 2
        PanelQuantity.Top = (Me.ClientSize.Height - PanelQuantity.Height) \ 2




    End Sub

    Private Sub btnSelectProduct_Click(sender As Object, e As EventArgs) Handles btnSelectProduct.Click
        ' Create instance of the product selection form
        Dim selector As New SelectProduct()

        ' Show the product selector as a modal dialog
        If selector.ShowDialog() = DialogResult.OK Then
            ' Get the selected barcode
            Dim selectedBarcode As String = selector.SelectedBarcode

            ' Check if a barcode was selected
            If Not String.IsNullOrEmpty(selectedBarcode) Then
                ' Add the selected product to cart using the barcode
                AddToCart(selectedBarcode)
            Else
                MessageBox.Show("No product was selected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub txtProduct_TextChanged(sender As Object, e As EventArgs) Handles txtProduct.TextChanged

    End Sub

    Private Sub POS_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' Adjust panel positions and sizes when form is resized
        If Me.WindowState <> FormWindowState.Minimized Then
            ' Recalculate positions of panels
            Panel1.Width = CInt(Me.ClientSize.Width * 0.7) ' 70% of form width
            Panel2.Width = CInt(Me.ClientSize.Width * 0.3) ' 30% of form width
            Panel2.Left = Panel1.Width

            ' Adjust height of panels
            Panel1.Height = Me.ClientSize.Height
            Panel2.Height = Me.ClientSize.Height

            ' Adjust DataGridView size
            dgvCart.Width = Panel1.Width - 20
            dgvCart.Height = CInt(Panel1.Height * 0.6)

            ' Center PanelPay if visible
            If PanelPay.Visible Then
                PanelPay.Left = (Me.ClientSize.Width - PanelPay.Width) \ 2
                PanelPay.Top = (Me.ClientSize.Height - PanelPay.Height) \ 2
            End If

            ' Center PanelQuantity if visible
            If PanelQuantity.Visible Then
                PanelQuantity.Left = (Me.ClientSize.Width - PanelQuantity.Width) \ 2
                PanelQuantity.Top = (Me.ClientSize.Height - PanelQuantity.Height) \ 2
            End If

            ' Center DiscountPanel if visible
            If DiscountPanel.Visible Then
                DiscountPanel.Left = (Me.ClientSize.Width - DiscountPanel.Width) \ 2
                DiscountPanel.Top = (Me.ClientSize.Height - DiscountPanel.Height) \ 2
            End If
        End If
    End Sub
End Class