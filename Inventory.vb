Imports Microsoft.Data.SqlClient

Public Class Inventory
    ' Declare the connection string at the class level
    Private connectionString As String = AppConfig.ConnectionString

    ' Method to load inventory data
    Private Sub LoadInventoryData()
        Try
            ' Use LEFT JOINs to ensure we get all inventory items even if some related data is missing
            Dim query As String = "SELECT I.Barcode, I.ProductID, P.ProductName, C.CategoryName, S.CompanyName, I.QuantityInStock, " &
                              "I.WholesaleDiscount, I.ExpirationDate, I.CriticalLevel, I.UnitPrice " &
                              "FROM Inventory I " &
                              "LEFT JOIN Products P ON I.ProductID = P.ProductID " &
                              "LEFT JOIN Categories C ON P.CategoryID = C.CategoryID " &
                              "LEFT JOIN Suppliers S ON I.SupplierID = S.SupplierID"

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim cmd As New SqlCommand(query, conn)
                Dim dt As New DataTable()
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                ' Filter out rows with no barcodes
                Dim filteredRows = dt.Select("Barcode IS NOT NULL AND Barcode <> ''")
                Dim filteredTable As DataTable = dt.Clone() ' Clone the structure of the original DataTable

                For Each row As DataRow In filteredRows
                    filteredTable.ImportRow(row) ' Import only rows with valid barcodes
                Next

                ' Bind the filtered data to the DataGridView
                dgvInventory.DataSource = filteredTable

                ' Hide the ProductID column (optional)
                dgvInventory.Columns("ProductID").Visible = False

                ' Set the Barcode column properties
                If dgvInventory.Columns.Contains("Barcode") Then
                    dgvInventory.Columns("Barcode").HeaderText = "Barcode"
                    dgvInventory.Columns("Barcode").Width = 200 ' Adjust width as needed
                End If

                ' Set the width of the ProductName column
                dgvInventory.Columns("ProductName").Width = 550

                ' Set the width of the CategoryName and CompanyName columns
                If dgvInventory.Columns.Contains("CategoryName") Then
                    dgvInventory.Columns("CategoryName").Width = 200
                End If

                If dgvInventory.Columns.Contains("CompanyName") Then
                    dgvInventory.Columns("CompanyName").Width = 200
                End If

                ' Format the WholesaleDiscount column header
                If dgvInventory.Columns.Contains("WholesaleDiscount") Then
                    dgvInventory.Columns("WholesaleDiscount").HeaderText = "Wholesale Discount"
                End If

                If dgvInventory.Columns.Contains("UnitPrice") Then
                    dgvInventory.Columns("UnitPrice").HeaderText = "Unit Price"
                    dgvInventory.Columns("UnitPrice").DefaultCellStyle.Format = "C2" ' Format as currency
                    dgvInventory.Columns("UnitPrice").Width = 150 ' Set a fixed width
                End If

                ' Set other columns to auto-size to fill remaining space
                For Each column As DataGridViewColumn In dgvInventory.Columns
                    If column.Name <> "ProductName" AndAlso column.Name <> "ProductID" AndAlso
                   column.Name <> "CategoryName" AndAlso column.Name <> "CompanyName" Then
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    End If
                Next

                ' Hide the row selection column (extra column with FullRowSelect mode)
                dgvInventory.RowHeadersVisible = False

                ' Refresh the DataGridView to ensure the data is loaded
                dgvInventory.Refresh()

                ' Check for critical stock levels
                CheckCriticalLevel(filteredTable)
            End Using

            ' Set DataGridView background color to white
            dgvInventory.BackgroundColor = Color.White

            ' Remove the extra row (the row for adding new data)
            dgvInventory.AllowUserToAddRows = False

        Catch ex As Exception
            MessageBox.Show("Error loading inventory data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvInventory.BackgroundColor = Color.White
        dgvInventory.BorderStyle = BorderStyle.None
        dgvInventory.AllowUserToAddRows = False
        dgvInventory.ReadOnly = True
        dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvInventory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvInventory.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvInventory.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvInventory.EnableHeadersVisualStyles = False
        dgvInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvInventory.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvInventory.ColumnHeadersHeight = 35
    End Sub

    ' Method to check for critical stock levels and show notification panel
    Private Sub CheckCriticalLevel(dt As DataTable)
        Dim criticalCount As Integer = 0 ' Counter for critical products

        ' Loop through the rows and check stock levels
        For Each row As DataRow In dt.Rows
            If row IsNot Nothing Then
                If CInt(row("QuantityInStock")) <= CInt(row("CriticalLevel")) Then
                    criticalCount += 1
                End If
            End If
        Next

        ' Display critical stock message if any products have low stock
        If criticalCount > 0 Then
            lblCriticalMessage.Text = criticalCount.ToString() & " Product(s) Are Low On Stock."
            pnlCriticalStock.Visible = True  ' Show the panel with the message
        Else
            pnlCriticalStock.Visible = False  ' Hide the panel if no products are critical
        End If
    End Sub

    ' When the form loads, call LoadInventoryData to populate the grid
    Private Sub Inventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadInventoryData()
        SetupDataGridView()
    End Sub


    ' When a row is clicked, populate the TextBox with Critical Level and Wholesale Discount
    Private Sub dgvInventory_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvInventory.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvInventory.Rows(e.RowIndex)

            ' Ensure txtCriticalLevel is initialized
            If txtCriticalLevel IsNot Nothing Then
                txtCriticalLevel.Text = row.Cells("CriticalLevel").Value.ToString()
                txtCriticalLevel.Tag = row.Cells("ProductID").Value ' Store ProductID in Tag property
            Else
                MessageBox.Show("Critical Level TextBox is not initialized.")
            End If

            ' Ensure txtWholeSalePrice is initialized
            If txtWholeSalePrice IsNot Nothing Then
                If dgvInventory.Columns.Contains("WholesaleDiscount") Then
                    ' Get raw value without any formatting (like % symbol)
                    Dim discountValue = row.Cells("WholesaleDiscount").Value
                    If discountValue IsNot Nothing AndAlso Not IsDBNull(discountValue) Then
                        ' Try to parse as decimal to ensure we're working with the numeric value
                        Dim discount As Decimal
                        If Decimal.TryParse(discountValue.ToString(), discount) Then
                            ' Format as whole number if no decimal places
                            If discount = Math.Floor(discount) Then
                                txtWholeSalePrice.Text = Math.Floor(discount).ToString()
                            Else
                                txtWholeSalePrice.Text = discount.ToString()
                            End If
                        Else
                            ' If cannot parse (maybe already has % or other formatting), clean it
                            Dim cleanedValue = discountValue.ToString().Replace("%", "").Trim()
                            txtWholeSalePrice.Text = cleanedValue
                        End If
                    Else
                        txtWholeSalePrice.Text = "0" ' Default value if null
                    End If
                    txtWholeSalePrice.Tag = row.Cells("ProductID").Value ' Store ProductID in Tag property
                Else
                    txtWholeSalePrice.Text = "0" ' Default if column doesn't exist yet
                End If
            End If
        End If
    End Sub


    ' Button click event to save changes
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If txtCriticalLevel Is Nothing Then
            MessageBox.Show("Critical Level TextBox is not initialized.")
            Return
        End If

        If txtCriticalLevel.Tag Is Nothing Then
            MessageBox.Show("No product selected. Please select a product.", "Select Product", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim productId As Integer = txtCriticalLevel.Tag
        Dim criticalLevel As Integer
        Dim wholesaleDiscount As Decimal = 0

        ' Validate critical level input
        If Not Integer.TryParse(txtCriticalLevel.Text, criticalLevel) Then
            MessageBox.Show("Invalid Critical Level. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validate wholesale discount input
        If Not String.IsNullOrWhiteSpace(txtWholeSalePrice.Text) Then
            If Not Decimal.TryParse(txtWholeSalePrice.Text, wholesaleDiscount) Then
                MessageBox.Show("Invalid Wholesale Discount. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If wholesaleDiscount < 0 Or wholesaleDiscount > 100 Then
                MessageBox.Show("Wholesale discount must be between 0 and 100 percent.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        ' Proceed to update the critical level and wholesale discount
        UpdateCriticalLevelAndWholesale(productId, criticalLevel, wholesaleDiscount)
    End Sub

    Private Sub UpdateCriticalLevelAndWholesale(productId As Integer, criticalLevel As Integer, wholesaleDiscount As Decimal)
        ' Check current values from database and update if changed
        Dim currentCriticalLevel As Integer = GetCurrentCriticalLevel(productId)
        Dim currentWholesaleDiscount As Decimal = GetCurrentWholesaleDiscount(productId)
        Dim hasChanges As Boolean = False
        Dim changeMessage As String = ""

        If criticalLevel <> currentCriticalLevel Then
            hasChanges = True
            changeMessage += "Critical Level Updated" & Environment.NewLine
        End If

        If wholesaleDiscount <> currentWholesaleDiscount Then
            hasChanges = True
            changeMessage += "Wholesale Discount Updated" & Environment.NewLine
        End If

        If hasChanges Then
            Try
                Using conn As New SqlConnection(connectionString)
                    conn.Open()

                    ' Update critical level in Inventory table
                    Dim updateCriticalQuery As String = "UPDATE Inventory SET CriticalLevel = @CriticalLevel WHERE ProductID = @ProductID"
                    Using cmd As New SqlCommand(updateCriticalQuery, conn)
                        cmd.Parameters.AddWithValue("@ProductID", productId)
                        cmd.Parameters.AddWithValue("@CriticalLevel", criticalLevel)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Update wholesale discount in Products table
                    Dim updateWholesaleQuery As String = "UPDATE Inventory SET WholesaleDiscount = @WholesaleDiscount WHERE ProductID = @ProductID"
                    Using cmd As New SqlCommand(updateWholesaleQuery, conn)
                        cmd.Parameters.AddWithValue("@ProductID", productId)
                        cmd.Parameters.AddWithValue("@WholesaleDiscount", wholesaleDiscount)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                ' Refresh the grid to reflect the updated data
                LoadInventoryData()

                ' Clear the textboxes and show success message
                txtCriticalLevel.Clear()
                txtWholeSalePrice.Clear()
                MessageBox.Show("Update successful:" & Environment.NewLine & changeMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error updating values: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("No changes detected. Values are the same as before.", "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function GetCurrentCriticalLevel(productId As Integer) As Integer
        ' Fetch current critical level from database
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT CriticalLevel FROM Inventory WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productId)
                    Return CInt(cmd.ExecuteScalar()) ' Get the current critical level
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching current critical level: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return 0
            End Try
        End Using
    End Function

    Private Sub txtWholeSalePrice_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtWholeSalePrice.KeyPress
        ' Puwede lang mag-type ng digits (0-9), decimal point at backspace
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> ChrW(8) Then
            e.Handled = True ' Huwag ituloy ang key press
        End If

        ' Optional: Puwede mo rin i-check kung isang decimal lang ang puwedeng itype
        If e.KeyChar = "."c AndAlso txtWholeSalePrice.Text.Contains(".") Then
            e.Handled = True ' Huwag payagan mag-type ng multiple decimal points
        End If
    End Sub

    Private Sub txtCriticalLevel_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCriticalLevel.KeyPress
        ' Puwede lang mag-type ng digits (0-9), decimal point at backspace
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c AndAlso e.KeyChar <> ChrW(8) Then
            e.Handled = True ' Huwag ituloy ang key press
        End If

        ' Optional: Puwede mo rin i-check kung isang decimal lang ang puwedeng itype
        If e.KeyChar = "."c AndAlso txtCriticalLevel.Text.Contains(".") Then
            e.Handled = True ' Huwag payagan mag-type ng multiple decimal points
        End If
    End Sub


    Private Function GetCurrentWholesaleDiscount(productId As Integer) As Decimal
        ' Fetch current wholesale discount from database
        Using conn As New SqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT WholesaleDiscount FROM Inventory WHERE ProductID = @ProductID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProductID", productId)
                    Dim result = cmd.ExecuteScalar()
                    Return If(result IsNot Nothing AndAlso result IsNot DBNull.Value, Convert.ToDecimal(result), 0)
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching current wholesale discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return 0
            End Try
        End Using
    End Function

    ' Button click event to open the Delivery form
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim DeliveryForm As New Delivery
        DeliveryForm.Show()
    End Sub

    ' Button click event to open the DeliveryList form
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim deliveryListForm As New DeliveryList()
        deliveryListForm.Show()
    End Sub

    ' The CellFormatting event handler to apply formatting (single definition)
    Private Sub dgvInventory_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvInventory.CellFormatting
        ' Check if dgvInventory is initialized (not Nothing)
        If dgvInventory Is Nothing Then
            MessageBox.Show("DataGridView is not initialized.")
            Return
        End If

        ' Format QuantityInStock column
        If e.ColumnIndex = dgvInventory.Columns("QuantityInStock").Index Then
            ' Ensure the value is valid (not null or empty)
            If e.Value IsNot Nothing AndAlso IsNumeric(e.Value) Then
                Dim quantityInStock As Integer = Convert.ToInt32(e.Value)

                ' Apply colors based on stock level
                If quantityInStock <= 5 Then
                    e.CellStyle.BackColor = Color.Red
                    e.CellStyle.ForeColor = Color.White
                ElseIf quantityInStock <= 10 Then
                    e.CellStyle.BackColor = Color.Yellow
                    e.CellStyle.ForeColor = Color.Black
                Else
                    e.CellStyle.BackColor = Color.White
                    e.CellStyle.ForeColor = Color.Black
                End If
            End If
        End If

        ' Format WholesaleDiscount column with percentage symbol
        If dgvInventory.Columns.Contains("WholesaleDiscount") AndAlso
           e.ColumnIndex = dgvInventory.Columns("WholesaleDiscount").Index Then
            ' Make sure the value is not null or DBNull
            If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                ' Try to parse the value as a Decimal
                Dim discount As Decimal
                If Decimal.TryParse(e.Value.ToString(), discount) Then
                    ' Format as integer if it's a whole number, otherwise keep decimal places
                    If discount = Math.Floor(discount) Then
                        e.Value = Math.Floor(discount).ToString() & "%"
                    Else
                        e.Value = discount.ToString() & "%"
                    End If
                    e.FormattingApplied = True
                End If
            End If
        End If
    End Sub

    ' Panel paint event to customize panel appearance
    Private Sub pnlCriticalStock_Paint(sender As Object, e As PaintEventArgs) Handles pnlCriticalStock.Paint
        pnlCriticalStock.BackColor = ColorTranslator.FromHtml("#EBE8DB")
    End Sub

    Private Sub PanelCategory_Paint(sender As Object, e As PaintEventArgs) Handles PanelCategory.Paint
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Define the corner radius
        Dim cornerRadius = 20

        ' Create a rounded rectangle path
        Dim path As New Drawing2D.GraphicsPath
        Dim rect As New Rectangle(0, 0, PanelCategory.Width, PanelCategory.Height)

        ' Add rounded corners
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' Apply rounded region to the panel
        PanelCategory.Region = New Region(path)

        ' Fill the background (optional)
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#3399FF"))
            g.FillPath(brush, path)
        End Using

        ' Draw the border (optional)
        Using pen As New Pen(ColorTranslator.FromHtml("#3399FF"), 2)
            g.DrawPath(pen, path)
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        LoadInventoryData()
    End Sub

    Private Function GetCurrentUserRole() As String
        ' Show debugging information with available SessionData properties
        Dim debugInfo As String = "SessionData Debug Info:" & vbCrLf &
                                 "fullName: " & If(SessionData.fullName, "null") & vbCrLf &
                                 "role: " & If(SessionData.role, "null")

        MessageBox.Show(debugInfo, "SessionData Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' If SessionData.role is already available, use it
        If Not String.IsNullOrEmpty(SessionData.role) Then
            Return SessionData.role
        End If

        ' Try to get user role from database based on current user's fullName
        Try
            If Not String.IsNullOrEmpty(SessionData.fullName) Then
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim query As String = "SELECT Role FROM Users WHERE CONCAT(FirstName, ' ', LastName) = @FullName"
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@FullName", SessionData.fullName)
                        Dim result As Object = cmd.ExecuteScalar()

                        If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                            ' Store it in SessionData for future use
                            SessionData.role = result.ToString()
                            Return SessionData.role
                        End If
                    End Using
                End Using
            End If
        Catch ex As Exception
            MessageBox.Show("Error retrieving user role from database: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        ' Fallback: If role is still not found, use the manual dialog for testing
        Dim defaultRole As String = InputBox("Your role was not found in the database. For testing purposes, please enter your role (Admin/Staff):", "Role Required", "Admin")

        ' Store the role in session for future use
        SessionData.role = defaultRole

        Return defaultRole
    End Function
End Class
