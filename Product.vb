Imports Microsoft.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class Product
    ' Connection string to connect to the SQL database
    Private connectionString As String = AppConfig.ConnectionString
    Private productImage As Byte() = Nothing ' To hold the selected image as a byte array
    Private editColumn As DataGridViewImageColumn

    Private selectedProductID As Integer = -1 ' Track selected product ID for editing and deletion

    '======= FOR LOAD =========
    Private Sub Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup the column index change handler
        SetupColumnDisplayIndexChangedHandler()

        ' Load categories and products
        LoadCategories()
        LoadProducts() ' This will load products and add the Edit column

        ' Center the panel (assuming CenterPanel is a custom function)
        CenterPanel()

        ' Setup DataGridView (assuming SetupDataGridView is a custom function)
        SetupDataGridView()

        ' Set placeholder text (assuming SetPlaceholders is a custom function)
        SetPlaceholders()

        '=== BUTTONS ====
        ' Set DataGridView background color to white
        dgvProduct.BackgroundColor = Color.White

        ' Set ComboBox to DropDownList so it can't be edited
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList

        ' Remove the extra row (for adding new data)
        dgvProduct.AllowUserToAddRows = False

        ' Disable the blue highlight when a row is selected
        dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvProduct.DefaultCellStyle.SelectionBackColor = dgvProduct.BackgroundColor
        dgvProduct.DefaultCellStyle.SelectionForeColor = dgvProduct.ForeColor

        ' Force the Edit column to be the last column one more time after everything is loaded
        Application.DoEvents()
        EnsureEditColumnIsLast()

        If SessionData.role.Equals("Staff", StringComparison.OrdinalIgnoreCase) Then
            btnAdd.Enabled = False
            btnEdit.Enabled = False
            btnDelete.Enabled = False
        End If

    End Sub

    ' Helper method to ensure Edit column is the last column
    Private Sub EnsureEditColumnIsLast()
        ' Check if Edit column exists
        If dgvProduct.Columns.Contains("Edit") Then
            ' Log the current status of all columns (to debug ordering issues)
            DebugColumnOrder("Before reordering")

            ' Get index of the last visible column (to avoid invisible columns issue)
            Dim lastVisibleIndex As Integer = 0
            For i As Integer = 0 To dgvProduct.Columns.Count - 1
                If i <> dgvProduct.Columns("Edit").Index AndAlso dgvProduct.Columns(i).Visible Then
                    lastVisibleIndex = Math.Max(lastVisibleIndex, dgvProduct.Columns(i).DisplayIndex)
                End If
            Next

            ' Set the DisplayIndex to be one higher than the highest other column
            dgvProduct.Columns("Edit").DisplayIndex = lastVisibleIndex + 1

            ' Additional check to make visible
            dgvProduct.Columns("Edit").Visible = True

            ' Force layout update
            dgvProduct.PerformLayout()
            dgvProduct.Refresh()

            ' Log the result after reordering
            DebugColumnOrder("After reordering")
        End If
    End Sub

    ' Debug helper to print column order to console
    Private Sub DebugColumnOrder(message As String)
        Dim columnInfo As String = $"{message}:{Environment.NewLine}"
        For i As Integer = 0 To dgvProduct.Columns.Count - 1
            Dim col = dgvProduct.Columns(i)
            columnInfo &= $"Column {i}: Name={col.Name}, DisplayIndex={col.DisplayIndex}, Visible={col.Visible}{Environment.NewLine}"
        Next
        Debug.WriteLine(columnInfo)

        ' Also write to a message box during debugging (can be removed in production)
        ' MessageBox.Show(columnInfo, "Column Debug")
    End Sub

    '========== Load Products to DataGridView =========== 
    Private Sub LoadProducts()
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            ' Updated query to join Inventory table
            Dim query As String = "SELECT p.ProductID, p.ProductName, c.CategoryName, i.Barcode, i.UnitPrice, p.Expiration, p.Image " &
                              "FROM Products p " &
                              "INNER JOIN Categories c ON p.CategoryID = c.CategoryID " &
                              "LEFT JOIN Inventory i ON p.ProductID = i.ProductID"
            Dim command As New SqlCommand(query, connection)

            ' Execute the query and load data into the DataGridView
            Dim reader As SqlDataReader = command.ExecuteReader()

            Dim dt As New DataTable()
            dt.Load(reader)

            ' Replace empty or null Barcode values with "No Barcode Yet"
            For Each row As DataRow In dt.Rows
                If IsDBNull(row("Barcode")) OrElse String.IsNullOrWhiteSpace(row("Barcode").ToString()) Then
                    row("Barcode") = "No Barcode Yet"
                End If
            Next

            ' Remove any existing Edit columns before setting data source
            For i As Integer = dgvProduct.Columns.Count - 1 To 0 Step -1
                If dgvProduct.Columns(i).Name = "Edit" Then
                    dgvProduct.Columns.RemoveAt(i)
                End If
            Next

            ' Set the DataSource
            dgvProduct.DataSource = dt

            ' Hide the ProductID column
            If dgvProduct.Columns.Contains("ProductID") Then
                dgvProduct.Columns("ProductID").Visible = False
            End If

            ' Hide the Image column
            If dgvProduct.Columns.Contains("Image") Then
                dgvProduct.Columns("Image").Visible = False
            End If

            ' Hide the UnitPrice column
            If dgvProduct.Columns.Contains("UnitPrice") Then
                dgvProduct.Columns("UnitPrice").Visible = False
            End If

            ' Wait for all columns to be created and visible settings applied
            Application.DoEvents()

            ' Add the Edit column (always after setting DataSource)
            AddSelectEditColumn()

            ' Move the Barcode column to the first position LAST (important ordering)
            ' This must happen AFTER adding the Edit column to prevent ordering issues
            If dgvProduct.Columns.Contains("Barcode") Then
                dgvProduct.Columns("Barcode").DisplayIndex = 0
                dgvProduct.PerformLayout()
            End If

            ' Set column widths and auto-size mode
            If dgvProduct.Columns.Contains("ProductName") Then
                dgvProduct.Columns("ProductName").Width = 400
                dgvProduct.Columns("ProductName").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            ' Prevent the extra row from showing
            dgvProduct.AllowUserToAddRows = False

            ' Final check to ensure Edit column is at the end (after all other column adjustments)
            EnsureEditColumnIsLast()

            ' Freeze the grid briefly then refresh to stabilize layout
            dgvProduct.Enabled = False
            Application.DoEvents()
            dgvProduct.Enabled = True
        End Using

        ' Force a refresh of the grid
        Application.DoEvents() ' Flush pending events
        dgvProduct.Refresh()
    End Sub

    '========== Load Categories in ComboBox =========== 
    Private Sub LoadCategories()
        Try
            ' Open connection to the database
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Clear ComboBox items to ensure no previous selection is shown
                cboCategory.Items.Clear()

                ' Load Categories into cboCategory
                Dim categoryQuery As String = "SELECT CategoryID, CategoryName FROM Categories"
                Dim categoryCommand As New SqlCommand(categoryQuery, connection)
                Dim categoryAdapter As New SqlDataAdapter(categoryCommand)
                Dim categoryTable As New DataTable()
                categoryAdapter.Fill(categoryTable)

                ' Check if data is loaded successfully
                If categoryTable.Rows.Count > 0 Then
                    ' Bind data to the ComboBox directly from the DataTable
                    cboCategory.DataSource = categoryTable
                    cboCategory.DisplayMember = "CategoryName"
                    cboCategory.ValueMember = "CategoryID"
                Else
                    MessageBox.Show("No categories found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End Using

            ' Reset cboCategory to have no default selection
            cboCategory.SelectedIndex = -1

        Catch ex As Exception
            ' Display error message if something goes wrong
            MessageBox.Show("An error occurred while loading categories: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '========== Helper Function for Category Lookup =========== 
    Private Function GetCategoryIDByName(categoryName As String) As Integer
        ' Retrieve CategoryID by CategoryName
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim command As New SqlCommand("SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName", connection)
            command.Parameters.AddWithValue("@CategoryName", categoryName)
            Dim result As Object = command.ExecuteScalar()
            Return If(result IsNot DBNull.Value, Convert.ToInt32(result), -1)
        End Using
    End Function

    '========= FOR BUTTONS ===========

    '========== Browse Image Button - Open FileDialog to select image ===========
    Private Sub btnBrowseImage_Click(sender As Object, e As EventArgs) Handles btnBrowseImage.Click
        ' Open file dialog to select an image
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            ' Convert selected image to byte array
            Dim imagePath As String = openFileDialog.FileName
            productImage = File.ReadAllBytes(imagePath)

            ' Display the image in the PictureBox
            picProductImage.Image = Image.FromFile(imagePath)

            ' Assuming you have the product name available (you can adjust this according to your form)
            Dim productName As String = txtProductName.Text.Trim()

            ' If the product name is empty, exit the function
            If String.IsNullOrEmpty(productName) Then
                MessageBox.Show("You can now proceed to fill in the remaining details.", "Proceed to Fill Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Log the action in the audit trail
            Dim actionDescription As String = $"Updated image for product '{productName}'"
            Logaudittrail(SessionData.role, SessionData.fullName, actionDescription)

            ' Optional log for debugging
            MessageBox.Show("A new image has been selected successfully. Your changes will be applied once you save the product update.", "Image Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    '========== Add Product Button - Insert Product into Database ===========
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate input fields
        If String.IsNullOrWhiteSpace(txtProductName.Text) Then
            MessageBox.Show("Please enter the product name.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtProductName.Focus()
            Exit Sub
        ElseIf cboCategory.SelectedIndex = -1 Then
            MessageBox.Show("Please select a category.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboCategory.Focus()
            Exit Sub
        End If

        Try
            ' Convert the image to a byte array
            Dim imageBytes As Byte() = Nothing
            If picProductImage.Image IsNot Nothing Then
                Using ms As New MemoryStream()
                    picProductImage.Image.Save(ms, picProductImage.Image.RawFormat)
                    imageBytes = ms.ToArray()
                End Using
            End If

            ' Insert product into Products table
            Dim productId As Integer
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim command As New SqlCommand("INSERT INTO Products (ProductName, CategoryID, Expiration, Image) OUTPUT INSERTED.ProductID VALUES (@ProductName, @CategoryID, @Expiration, @Image)", connection)
                command.Parameters.AddWithValue("@ProductName", txtProductName.Text)
                command.Parameters.AddWithValue("@CategoryID", Convert.ToInt32(cboCategory.SelectedValue))
                command.Parameters.AddWithValue("@Expiration", If(chkExpirationOption.Checked, "With Expiration", "Without Expiration"))
                command.Parameters.AddWithValue("@Image", If(imageBytes IsNot Nothing, CType(imageBytes, Object), DBNull.Value))

                productId = Convert.ToInt32(command.ExecuteScalar())
            End Using

            ' Removed the Inventory insertion logic here

            MessageBox.Show("Product has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Reset form and hide panel
            ResetForm()
            PanelProduct.Visible = False

            ' Reload products
            LoadProducts()

            ' Additional check to ensure Edit column is last
            EnsureEditColumnIsLast()

        Catch ex As Exception
            MessageBox.Show("An error occurred while adding the product: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '========== Edit Product Button - Update Product in Database =========== 
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' Ensure a product is selected before editing
        If selectedProductID = -1 Then
            MessageBox.Show("Please select a product to edit before proceeding.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Step 1: Retrieve original product values from the database
        Dim originalQuery As String = "SELECT ProductName, CategoryID, Expiration FROM Products WHERE ProductID = @ProductID"
        Dim originalProduct As New Dictionary(Of String, String)
        Try
            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand(originalQuery, connection)
                command.Parameters.AddWithValue("@ProductID", selectedProductID)
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        originalProduct("ProductName") = reader("ProductName").ToString()
                        originalProduct("CategoryID") = reader("CategoryID").ToString()
                        originalProduct("Expiration") = reader("Expiration").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching original product data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Step 2: Get new values from the form
        Dim productName As String = If(txtProductName?.Text, String.Empty)
        Dim categoryID As String = If(cboCategory?.SelectedValue?.ToString(), String.Empty)
        Dim expiration As String = If(chkExpirationOption?.Checked, "With Expiration", "Without Expiration")

        Dim updatedProduct As New Dictionary(Of String, String) From {
       {"ProductName", productName},
       {"CategoryID", categoryID},
       {"Expiration", expiration}
   }

        If String.IsNullOrWhiteSpace(productName) Then
            MessageBox.Show("Product name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(categoryID) Then
            MessageBox.Show("Please select a valid category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Step 3: Check for duplicate product names
        Dim checkQuery As String = "SELECT COUNT(*) FROM Products WHERE ProductName = @ProductName AND ProductID <> @ProductID"
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim checkCommand As New SqlCommand(checkQuery, connection)
            checkCommand.Parameters.AddWithValue("@ProductName", updatedProduct("ProductName"))
            checkCommand.Parameters.AddWithValue("@ProductID", selectedProductID)
            Dim count As Integer = Convert.ToInt32(checkCommand.ExecuteScalar())

            If count > 0 Then
                MessageBox.Show("The product name already exists. Please choose a different name.", "Duplicate Product", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End Using

        ' Step 4: Proceed with the update
        Try
            Dim updateQuery As String = "UPDATE Products SET ProductName = @ProductName, CategoryID = @CategoryID, Expiration = @Expiration" & If(productImage IsNot Nothing, ", Image = @Image", "") & " WHERE ProductID = @ProductID"
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim command As New SqlCommand(updateQuery, connection)
                command.Parameters.AddWithValue("@ProductID", selectedProductID)
                command.Parameters.AddWithValue("@ProductName", updatedProduct("ProductName"))
                command.Parameters.AddWithValue("@CategoryID", updatedProduct("CategoryID"))
                command.Parameters.AddWithValue("@Expiration", updatedProduct("Expiration"))

                If productImage IsNot Nothing Then
                    command.Parameters.AddWithValue("@Image", productImage)
                End If

                command.ExecuteNonQuery()

                ' Reset form, reload products, and notify user
                ResetForm()
                MessageBox.Show("Product details have been successfully updated!", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadProducts()
                PanelProduct.Visible = False
            End Using

            ' After successful update and reloading products
            EnsureEditColumnIsLast() ' Make sure Edit column stays at the end
        Catch ex As Exception
            MessageBox.Show("An error occurred while updating the product: " & ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Function to get Category name by ID
    Private Function GetCategoryNameByID(categoryID As String) As String
        ' Fetch the category name from the database based on the CategoryID
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT CategoryName FROM Categories WHERE CategoryID = @CategoryID"
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@CategoryID", categoryID)
            Return command.ExecuteScalar().ToString()
        End Using
    End Function

    ' Function to get Supplier name by ID
    Private Function GetSupplierNameByID(supplierID As String) As String
        ' Fetch the supplier name from the database based on the SupplierID
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT SupplierName FROM Suppliers WHERE SupplierID = @SupplierID"
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@SupplierID", supplierID)
            Return command.ExecuteScalar().ToString()
        End Using
    End Function

    '========== Delete Product Button - Delete Product from Database =========== 
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If selectedProductID = -1 Then
            MessageBox.Show("Please select a product to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            Try
                ' Proceed with deleting the product
                Using connection As New SqlConnection(connectionString)
                    connection.Open()
                    Dim command As New SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", connection)
                    command.Parameters.AddWithValue("@ProductID", selectedProductID)
                    command.ExecuteNonQuery()
                End Using

                MessageBox.Show("Product has been successfully deleted.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Reset form and selected product ID
                ResetForm()
                selectedProductID = -1
                PanelProduct.Visible = False

                ' Reload products (this will re-add the Edit column)
                LoadProducts()

                ' After reloading products
                EnsureEditColumnIsLast() ' Make sure Edit column stays at the end

            Catch ex As Exception
                MessageBox.Show("An error occurred while deleting the product: " & ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub AddSelectEditColumn()
        ' First, check and remove any existing Edit columns to prevent duplicates
        For i As Integer = dgvProduct.Columns.Count - 1 To 0 Step -1
            If dgvProduct.Columns(i).Name = "Edit" Then
                dgvProduct.Columns.RemoveAt(i)
            End If
        Next

        ' Now add a new Edit column
        Dim editColumn As New DataGridViewImageColumn()
        editColumn.Name = "Edit"
        editColumn.HeaderText = "Select / Edit"
        editColumn.Width = 40

        Try
            ' Set the image for the Edit column
            Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
            If File.Exists(imagePath) Then
                editColumn.Image = Image.FromFile(imagePath)
                ' Set the image layout
                editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
            Else
                ' If image doesn't exist, use text instead
                Dim textCol As New DataGridViewButtonColumn()
                textCol.Name = "Edit"
                textCol.HeaderText = "Select / Edit"
                textCol.Text = "Edit"
                textCol.UseColumnTextForButtonValue = True
                dgvProduct.Columns.Add(textCol)
                textCol.DisplayIndex = dgvProduct.Columns.Count - 1
                Return
            End If
        Catch ex As Exception
            ' Display error if the image fails to load
            MessageBox.Show("Error loading image: " & ex.Message)
        End Try

        ' Add the column to the DataGridView
        dgvProduct.Columns.Add(editColumn)

        ' Apply column order immediately
        dgvProduct.PerformLayout()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetForm()
        btnAdd.Enabled = True
    End Sub
    Private Function GetProductDetailsById(productId As Integer) As DataRow
        ' Create a DataTable to store the product details
        Dim productDetails As New DataTable()

        ' SQL query to fetch the product details based on ProductID
        Dim query As String = "SELECT ProductName FROM Products WHERE ProductID = @ProductID"
        Try
            ' Create the SQL connection
            Using connection As New SqlConnection(connectionString)
                ' Open the connection
                connection.Open()

                ' Create the command
                Using command As New SqlCommand(query, connection)
                    ' Add the parameter for the ProductID
                    command.Parameters.AddWithValue("@ProductID", productId)

                    ' Create the data adapter
                    Using dataAdapter As New SqlDataAdapter(command)
                        ' Fill the DataTable with the query results
                        dataAdapter.Fill(productDetails)
                    End Using
                End Using
            End Using

            ' Check if any rows were returned
            If productDetails.Rows.Count > 0 Then
                ' Return the first row of the product details
                Return productDetails.Rows(0)
            Else
                ' If no product was found, return Nothing or handle as needed
                Return Nothing
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching product details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    '==================FOR DGV =================
    Private Sub dgvProduct_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProduct.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvProduct.Rows(e.RowIndex)

            ' Pre-fill the form fields with the values from the selected row
            txtProductName.Text = row.Cells("ProductName").Value.ToString()

            ' Fetch UnitPrice from the Inventory table
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "SELECT UnitPrice FROM Inventory WHERE ProductID = @ProductID"
                Dim command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(row.Cells("ProductID").Value))
                Dim unitPrice As Object = command.ExecuteScalar()

            End Using

            ' Handle expiration option
            If row.Cells("Expiration").Value.ToString() = "With Expiration" Then
                chkExpirationOption.Checked = True
            Else
                chkExpirationOption.Checked = False
            End If

            ' Optionally load the product image (if available)
            If Not IsDBNull(row.Cells("Image").Value) Then
                Dim imgData As Byte() = CType(row.Cells("Image").Value, Byte())
                Using ms As New MemoryStream(imgData)
                    picProductImage.Image = Image.FromStream(ms)
                End Using
            Else
                picProductImage.Image = Nothing
            End If

            PanelProduct.Visible = True
            selectedProductID = Convert.ToInt32(row.Cells("ProductID").Value)
        End If
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvProduct.BackgroundColor = Color.White
        dgvProduct.BorderStyle = BorderStyle.None
        dgvProduct.AllowUserToAddRows = False
        dgvProduct.ReadOnly = True
        dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvProduct.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvProduct.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvProduct.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvProduct.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance with uppercase first letter
        dgvProduct.EnableHeadersVisualStyles = False
        dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvProduct.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

        ' Customize header height
        dgvProduct.ColumnHeadersHeight = 35
    End Sub

    '========= FOR PLACE HOLDER =============
    Private Sub SetPlaceholders()
        ' Set placeholder text for product-related fields
        txtProductName.PlaceholderText = "Enter Product Name"

        ' Reset other product-related fields (if any)
        txtProductName.Clear()

    End Sub

    '=========== KEY PRESS VALIDATION ===========

    ' txtProductName – Letters at space lang
    Private Sub txtProductName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtProductName.KeyPress
        ' Letters at space lang, bawal numbers at special characters
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtBarcode – Numbers lang (0-9)
    Private Sub txtBarcode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBarcode.KeyPress
        ' Numbers lang, bawal letters at special characters
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtUnitPrice – Numbers lang with one decimal
    Private Sub txtUnitPrice_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Numbers at decimal point lang, bawal letters at special characters
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "." AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If


    End Sub

    ' txtRetailPrice – Same logic as UnitPrice
    Private Sub txtRetailPrice_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Numbers at decimal point lang, bawal letters at special characters
        If Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "." AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If


    End Sub

    '==========  FOR RESET  =========== 
    Private Sub ResetForm()
        ' Clear form fields
        txtProductName.Clear()

        cboCategory.SelectedIndex = -1
        chkExpirationOption.Checked = False
        picProductImage.Image = Nothing
        productImage = Nothing
    End Sub

    '=========== AUDIT TRAIL ==============
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
                    cmd.Parameters.AddWithValue("@Form", "Product Form")
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    '============ FOR PANEL ==============
    Private Sub PanelCategory_Paint(sender As Object, e As PaintEventArgs) Handles PanelCategory.Paint
        ' Enable anti-aliasing for smoother edges
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Define the corner radius
        Dim cornerRadius As Integer = 20

        ' Create a rounded rectangle path
        Dim path As New Drawing2D.GraphicsPath()
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
    Private Sub CenterPanel()
        Dim formWidth As Integer = Me.ClientSize.Width
        Dim formHeight As Integer = Me.ClientSize.Height

        Dim panelWidth As Integer = PanelProduct.Width
        Dim panelHeight As Integer = PanelProduct.Height

        Dim x As Integer = (formWidth - panelWidth) \ 2
        Dim y As Integer = (formHeight - panelHeight) \ 2

        PanelProduct.Location = New Point(x, y)
    End Sub

    Private Sub ProductPanel_Paint(sender As Object, e As PaintEventArgs) Handles PanelProduct.Paint
        CenterPanel()
        PanelProduct.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub

    '================ FOR BUTTON 2 ===========
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PanelProduct.Visible = True
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PanelProduct.Visible = False
        ResetForm()
    End Sub

    Private Sub dgvProduct_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProduct.CellContentClick
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = dgvProduct.Rows(e.RowIndex)

            ' Check if the clicked column is the "Edit" button
            If dgvProduct.Columns(e.ColumnIndex).Name = "Edit" Then
                ' Show the panel for editing
                PanelProduct.Visible = True

                ' Fill the textboxes with selected row values
                txtProductName.Text = selectedRow.Cells("ProductName").Value.ToString()

                ' Fetch unit price and retail price directly from database
                Using connection As New SqlConnection(connectionString)
                    connection.Open()
                    Dim query As String = "SELECT UnitPrice FROM Inventory WHERE ProductID = @ProductID"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(selectedRow.Cells("ProductID").Value))


                End Using

                ' Get the CategoryName from the selected row and find the corresponding CategoryID
                Dim categoryName As String = selectedRow.Cells("CategoryName").Value.ToString()

                ' Loop through the ComboBox items to find the matching CategoryName
                For Each item As DataRowView In cboCategory.Items
                    If item("CategoryName").ToString() = categoryName Then
                        cboCategory.SelectedItem = item
                        Exit For
                    End If
                Next

                ' Handle expiration option
                If selectedRow.Cells("Expiration").Value.ToString() = "With Expiration" Then
                    chkExpirationOption.Checked = True
                Else
                    chkExpirationOption.Checked = False
                End If

                ' Optionally load the product image (if available)
                If Not IsDBNull(selectedRow.Cells("Image").Value) Then
                    Dim imgData As Byte() = CType(selectedRow.Cells("Image").Value, Byte())
                    Using ms As New MemoryStream(imgData)
                        picProductImage.Image = Image.FromStream(ms)
                    End Using
                Else
                    ' Set a default image if no image is found
                    picProductImage.Image = Nothing
                End If

                ' Check if the clicked column is the "Delete" button
            ElseIf dgvProduct.Columns(e.ColumnIndex).Name = "Delete" Then
                ' Get the current user role
                Dim userRole As String = GetCurrentUserRole()

                ' Check if the user is Staff
                If userRole.ToUpper().Contains("STAFF") Then
                    MessageBox.Show("Staff members are not authorized to delete products.",
                                   "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Confirm deletion (only for non-staff)
                Dim confirm = MessageBox.Show("Delete this product?", "Confirm", MessageBoxButtons.YesNo)
                If confirm = DialogResult.Yes Then
                    ' Get the ProductID from the selected row
                    Dim productID As Integer = Convert.ToInt32(selectedRow.Cells("ProductID").Value)
                    ' Call the method to delete from database
                    DeleteProductFromDatabase(productID)
                    ' Remove from DataGridView
                    dgvProduct.Rows.RemoveAt(e.RowIndex)
                End If
            End If
        End If
    End Sub

    Private Sub DeleteProductFromDatabase(productID As Integer)
        ' Get the current user role
        Dim userRole As String = GetCurrentUserRole()

        ' Check if the user is Staff
        If userRole.ToUpper().Contains("STAFF") Then
            MessageBox.Show("Staff members are not authorized to delete products.",
                        "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Use a database connection to execute the commands
        Using conn As New SqlConnection(connectionString)
            conn.Open()

            ' Start a transaction to ensure both deletions succeed or fail together
            Using transaction As SqlTransaction = conn.BeginTransaction()
                Try
                    ' Delete associated rows from the Inventory table
                    Dim deleteInventoryQuery As String = "DELETE FROM Inventory WHERE ProductID = @ProductID"
                    Using deleteInventoryCmd As New SqlCommand(deleteInventoryQuery, conn, transaction)
                        deleteInventoryCmd.Parameters.AddWithValue("@ProductID", productID)
                        deleteInventoryCmd.ExecuteNonQuery()
                    End Using

                    ' Delete the product from the Products table
                    Dim deleteProductQuery As String = "DELETE FROM Products WHERE ProductID = @ProductID"
                    Using deleteProductCmd As New SqlCommand(deleteProductQuery, conn, transaction)
                        deleteProductCmd.Parameters.AddWithValue("@ProductID", productID)
                        deleteProductCmd.ExecuteNonQuery()
                    End Using

                    ' Commit the transaction
                    transaction.Commit()

                    MessageBox.Show("Product and associated inventory have been successfully deleted.", "Success")
                Catch ex As Exception
                    ' Rollback the transaction in case of an error
                    transaction.Rollback()
                    MessageBox.Show("Error deleting product and associated inventory: " & ex.Message, "Error")
                End Try
            End Using
        End Using
    End Sub

    '=========== FOR SEARCH ==========
    Private Sub txtSearchProduct_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ' Call the LoadProductData method whenever the search term changes
        LoadProductData(txtSearch.Text)
    End Sub

    Private Sub LoadProductData(searchTerm As String)
        ' Remove any existing Edit columns before changing data source
        For i As Integer = dgvProduct.Columns.Count - 1 To 0 Step -1
            If dgvProduct.Columns(i).Name = "Edit" Then
                dgvProduct.Columns.RemoveAt(i)
            End If
        Next

        Dim query As String = "SELECT p.ProductID, p.ProductName, c.CategoryName, i.Barcode, i.UnitPrice, p.Expiration, p.Image " &
                          "FROM Products p " &
                          "INNER JOIN Categories c ON p.CategoryID = c.CategoryID " &
                          "LEFT JOIN Inventory i ON p.ProductID = i.ProductID " &
                          "WHERE p.ProductName LIKE @search OR c.CategoryName LIKE @search OR i.Barcode LIKE @search"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()

                Try
                    conn.Open()
                    ' Fill the table with data
                    adapter.Fill(table)

                    ' Bind the results to the DataGridView
                    dgvProduct.DataSource = table

                    ' Hide any columns we don't want to show
                    If dgvProduct.Columns.Contains("ProductID") Then
                        dgvProduct.Columns("ProductID").Visible = False
                    End If

                    If dgvProduct.Columns.Contains("UnitPrice") Then
                        dgvProduct.Columns("UnitPrice").Visible = False
                    End If

                    If dgvProduct.Columns.Contains("Image") Then
                        dgvProduct.Columns("Image").Visible = False
                    End If

                    ' Always add the Edit column after setting DataSource
                    AddSelectEditColumn()

                    ' Ensure Edit column is at the end
                    EnsureEditColumnIsLast()

                Catch ex As Exception
                    ' Handle the error (displaying it in a message box here)
                    MessageBox.Show("Database Error: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Function GetCurrentUserRole() As String
        ' First show a simple message to confirm code execution


        ' Rest of your existing code
        If Not String.IsNullOrEmpty(SessionData.role) Then
            Return SessionData.role
        End If

        ' Try to get user role from database based on current user's fullName
        Try
            If Not String.IsNullOrEmpty(SessionData.fullName) Then
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Dim query As String = "SELECT Role FROM Users WHERE FullName = @FullName"
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

    ' Alternative approach: Add a handler for column display index changed events
    Private Sub SetupColumnDisplayIndexChangedHandler()
        ' Subscribe to the ColumnDisplayIndexChanged event
        AddHandler dgvProduct.ColumnDisplayIndexChanged, AddressOf dgvProduct_ColumnDisplayIndexChanged
    End Sub

    Private Sub dgvProduct_ColumnDisplayIndexChanged(sender As Object, e As DataGridViewColumnEventArgs)
        ' When any column's display index changes, ensure Edit column is last
        ' This happens only after form is fully loaded
        If dgvProduct.IsHandleCreated AndAlso Not dgvProduct.Disposing AndAlso Not dgvProduct.IsDisposed Then
            If e.Column.Name <> "Edit" AndAlso dgvProduct.Columns.Contains("Edit") Then
                ' Use BeginInvoke to avoid recursive event triggering
                dgvProduct.BeginInvoke(Sub()
                                           ' Only reposition if Edit isn't already last
                                           Dim editCol = dgvProduct.Columns("Edit")
                                           If editCol.DisplayIndex <> dgvProduct.Columns.Count - 1 Then
                                               ' Set Edit column to be last
                                               editCol.DisplayIndex = dgvProduct.Columns.Count - 1
                                           End If
                                       End Sub)
            End If
        End If
    End Sub
End Class

