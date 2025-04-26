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
        ' Load categories and products
        LoadCategories()
        LoadProducts()

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

        ' If needed, add Delete column or additional custom columns
        ' Uncomment and modify if needed
        'AddDeleteColumn()

        ' Check and debug the columns
        For Each column As DataGridViewColumn In dgvProduct.Columns
            Console.WriteLine(column.Name) ' This will print column names to the Output window in Visual Studio
        Next

        editColumn = New DataGridViewImageColumn()
        editColumn.Name = "Edit"
        editColumn.HeaderText = "Select / Edit"
        editColumn.Width = 40

        Try
            ' Set the image for the Edit column, check for any exceptions
            Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
            editColumn.Image = Image.FromFile(imagePath)
        Catch ex As Exception
            ' Display error if the image fails to load
            MessageBox.Show("Error loading image: " & ex.Message)
        End Try

        ' Set the image layout
        editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom

        ' Add the Edit column to the DataGridView
        dgvProduct.Columns.Add(editColumn)
    End Sub



    Private Sub AddDeleteColumn()
        ' Optional: You can also add a Delete column if needed
        Dim deleteColumn As New DataGridViewImageColumn()
        deleteColumn.Name = "Delete"
        deleteColumn.HeaderText = "Delete"
        deleteColumn.Width = 40

        Try
            ' Set the image for the Delete column, check for any exceptions
            deleteColumn.Image = Image.FromFile("C:\Users\Aspire 5\source\repos\oreo-main\Resources\icons8-delete-35.png")
        Catch ex As Exception
            ' Display error if the image fails to load
            MessageBox.Show("Error loading image: " & ex.Message)
        End Try

        ' Set the image layout
        deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom

        ' Add the Delete column to the DataGridView
        dgvProduct.Columns.Add(deleteColumn)
    End Sub

    '========== Load Products to DataGridView =========== 
    Private Sub LoadProducts()
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            ' SQL query to fetch products with category name (excluding supplier)
            Dim query As String = "SELECT p.ProductID, p.ProductName, c.CategoryName, p.Expiration, p.Image FROM Products p INNER JOIN Categories c ON p.CategoryID = c.CategoryID"
            Dim command As New SqlCommand(query, connection)

            ' Execute the query and load data into the DataGridView
            Dim reader As SqlDataReader = command.ExecuteReader()

            Dim dt As New DataTable()
            dt.Load(reader)
            dgvProduct.DataSource = dt

            ' Hide the ProductID column (first column, index 0)
            If dgvProduct.Columns.Count > 0 Then
                dgvProduct.Columns(0).Visible = False
            End If

            ' Hide the Image column
            If dgvProduct.Columns.Contains("Image") Then
                dgvProduct.Columns("Image").Visible = False
            End If


            If dgvProduct.Columns.Contains("ProductName") Then
                dgvProduct.Columns("ProductName").Width = 400  ' Set width to 400 for ProductName
                dgvProduct.Columns("ProductName").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            ' Set AutoSizeMode to Fill for remaining columns to avoid extra space at the end
            For Each column As DataGridViewColumn In dgvProduct.Columns
                If column.Name <> "Barcode" And column.Name <> "ProductName" Then
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End If
            Next

            ' Prevent the extra row from showing
            dgvProduct.AllowUserToAddRows = False

        End Using
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
        ElseIf System.Text.RegularExpressions.Regex.IsMatch(txtProductName.Text, "[0-9]") Then
            MessageBox.Show("Product name must not contain numbers.", "Invalid Product Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtProductName.Focus()
            Exit Sub
        ElseIf String.IsNullOrWhiteSpace(txtBarcode.Text) Then
            MessageBox.Show("Please enter the barcode.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtBarcode.Focus()
            Exit Sub
        ElseIf Not System.Text.RegularExpressions.Regex.IsMatch(txtBarcode.Text, "^[\d\-,.]+$") Then
            MessageBox.Show("Barcode must contain only numbers, commas, periods, or dashes.", "Invalid Barcode", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtBarcode.Focus()
            Exit Sub
        ElseIf cboCategory.SelectedIndex = -1 Then
            MessageBox.Show("Please select a category.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboCategory.Focus()
            Exit Sub
        End If

        Try
            ' Check if the barcode already exists
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim checkBarcodeQuery As String = "SELECT COUNT(*) FROM Products WHERE Barcode = @Barcode"
                Dim checkBarcodeCmd As New SqlCommand(checkBarcodeQuery, connection)
                checkBarcodeCmd.Parameters.AddWithValue("@Barcode", txtBarcode.Text)

                Dim existingCount As Integer = Convert.ToInt32(checkBarcodeCmd.ExecuteScalar())

                If existingCount > 0 Then
                    MessageBox.Show("The barcode already exists. Please enter a unique barcode.", "Duplicate Barcode", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtBarcode.Focus()
                    Exit Sub
                End If
            End Using

            ' Prepare product data
            Dim productName As String = txtProductName.Text
            Dim barcode As String = txtBarcode.Text
            Dim categoryId As Integer = Convert.ToInt32(cboCategory.SelectedValue)
            Dim expiration As String = If(chkExpirationOption.Checked, "With Expiration", "Without Expiration")
            Dim productImage As Byte() = Nothing

            ' Handle image
            If picProductImage.Image IsNot Nothing Then
                Using ms As New MemoryStream()
                    picProductImage.Image.Save(ms, picProductImage.Image.RawFormat)
                    productImage = ms.ToArray()
                End Using
            Else
                Dim imagePath As String = Path.Combine(Application.StartupPath, "Resources\noimage (1).jpeg")
                If File.Exists(imagePath) Then
                    productImage = File.ReadAllBytes(imagePath)
                Else
                    MessageBox.Show("Default image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            ' Insert product into database
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim command As New SqlCommand("INSERT INTO Products (Barcode, ProductName, CategoryID, Expiration, Image) VALUES (@Barcode, @ProductName, @CategoryID, @Expiration, @Image)", connection)
                command.Parameters.AddWithValue("@Barcode", barcode)
                command.Parameters.AddWithValue("@ProductName", productName)
                command.Parameters.AddWithValue("@CategoryID", categoryId)
                command.Parameters.AddWithValue("@Expiration", expiration)
                command.Parameters.AddWithValue("@Image", If(productImage IsNot Nothing, productImage, DBNull.Value))

                command.ExecuteNonQuery()

                ' Log to audit trail
                Dim actionDescription As String = $"Added A New Product: {productName}"
                Logaudittrail(SessionData.role, SessionData.fullName, actionDescription)

                ' Reset and reload
                ResetForm()
                MessageBox.Show("Product has been successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadProducts()
            End Using

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
        Dim originalQuery As String = "SELECT ProductName, Barcode, CategoryID, Expiration FROM Products WHERE ProductID = @ProductID"
        Dim originalProduct As New Dictionary(Of String, String)
        Try
            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand(originalQuery, connection)
                command.Parameters.AddWithValue("@ProductID", selectedProductID)
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        originalProduct("ProductName") = reader("ProductName").ToString()
                        originalProduct("Barcode") = reader("Barcode").ToString()
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
        Dim updatedProduct As New Dictionary(Of String, String) From {
    {"ProductName", txtProductName.Text},
    {"Barcode", txtBarcode.Text},
    {"CategoryID", cboCategory.SelectedValue.ToString()},
    {"Expiration", If(chkExpirationOption.Checked, "With Expiration", "Without Expiration")}
}

        ' Step 3: Generate the change log (simplified)
        Dim changes As String = "Edited the product: " & updatedProduct("ProductName") & vbCrLf
        For Each key In originalProduct.Keys
            If originalProduct(key) <> updatedProduct(key) Then
                ' Log changes based on Category using ComboBox's selected text
                If key = "CategoryID" Then
                    ' Assuming cboCategory.Text gives the category name (not the ID)
                    Dim updatedCategoryName As String = cboCategory.Text
                    Dim originalCategoryName As String = GetCategoryNameByID(originalProduct("CategoryID"))
                    changes &= $"Category changed from '{originalCategoryName}' to '{updatedCategoryName}'" & vbCrLf
                Else
                    changes &= $"{key} changed from '{originalProduct(key)}' to '{updatedProduct(key)}'" & vbCrLf
                End If
            End If
        Next

        ' If there are changes, log the audit trail
        If changes.Length > 0 Then
            Logaudittrail(SessionData.role, SessionData.fullName, changes)
        End If

        ' Step 4: Proceed with the update logic if there are any changes
        If changes.Length > 0 Then
            Try
                ' Rename the query to avoid conflict
                Dim updateQuery As String = "UPDATE Products SET Barcode = @Barcode, ProductName = @ProductName = CategoryID = @CategoryID, Expiration = @Expiration" & If(productImage IsNot Nothing, ", Image = @Image", "") & " WHERE ProductID = @ProductID"
                Using connection As New SqlConnection(connectionString)
                    connection.Open()

                    ' SQL Query to update product details (including image if changed)
                    Dim command As New SqlCommand(updateQuery, connection)
                    command.Parameters.AddWithValue("@ProductID", selectedProductID)
                    command.Parameters.AddWithValue("@Barcode", updatedProduct("Barcode"))
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
                End Using
            Catch ex As Exception
                MessageBox.Show("The product already exists. Please check the product details.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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
        ' Get the current user role
        Dim userRole As String = GetCurrentUserRole()

        ' Debug: Show the actual role value
        MessageBox.Show("Current role: " & userRole, "Debug Info")

        ' Check if the user is Staff - don't allow deletion for Staff role
        If userRole.ToUpper().Contains("STAFF") Then
            MessageBox.Show("Staff members are not authorized to delete products.",
                           "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If selectedProductID = -1 Then
            MessageBox.Show("Please select a product to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            Try
                ' Get the current user's details (Assuming SessionData holds user details)
                Dim userFullName As String = SessionData.fullName ' Logged-in user full name

                ' Fetch the product details (for audit purposes)
                Dim productToDelete As DataRow = GetProductDetailsById(selectedProductID)
                Dim actionDescription As String = $"Deleted Product: {productToDelete("ProductName")} (Barcode: {productToDelete("Barcode")})"

                ' Log the audit trail
                Logaudittrail(userRole, userFullName, actionDescription)

                ' Proceed with deleting the product
                Using connection As New SqlConnection(connectionString)
                    connection.Open()
                    Dim command As New SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", connection)
                    command.Parameters.AddWithValue("@ProductID", selectedProductID)
                    command.ExecuteNonQuery()
                End Using

                MessageBox.Show("Product has been successfully deleted.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadProducts() ' Refresh product list
                ResetForm() ' Reset the form
            Catch ex As Exception
                MessageBox.Show("An error occurred while deleting the product: " & ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        btnAdd.Enabled = True ' Re-enable the Add button
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetForm()
        btnAdd.Enabled = True
    End Sub
    Private Function GetProductDetailsById(productId As Integer) As DataRow
        ' Create a DataTable to store the product details
        Dim productDetails As New DataTable()

        ' SQL query to fetch the product details based on ProductID
        Dim query As String = "SELECT ProductName, Barcode, RetailPrice FROM Products WHERE ProductID = @ProductID"

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
        ' Ensure the clicked row is valid
        If e.RowIndex >= 0 Then
            ' Get the selected row data
            Dim row As DataGridViewRow = dgvProduct.Rows(e.RowIndex)

            ' Pre-fill the form fields with the values from the selected row
            txtProductName.Text = row.Cells("ProductName").Value.ToString()
            txtBarcode.Text = row.Cells("Barcode").Value.ToString()

            ' Fetch unit price and retail price directly from the database
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "SELECT UnitPrice, RetailPrice FROM Products WHERE ProductID = @ProductID"
                Dim command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ProductID", Convert.ToInt32(row.Cells("ProductID").Value))
            End Using

            ' Get the CategoryName from the selected row and find the corresponding CategoryID
            Dim categoryName As String = row.Cells("CategoryName").Value.ToString()

            ' Loop through the ComboBox items to find the matching CategoryName
            For Each item As DataRowView In cboCategory.Items
                If item("CategoryName").ToString() = categoryName Then
                    cboCategory.SelectedItem = item
                    Exit For
                End If
            Next

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
                ' Set a default image if no image is found
                picProductImage.Image = Nothing
            End If

            PanelProduct.Visible = True

            ' Store the selected ProductID for future update
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
        txtBarcode.Clear()

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
                txtBarcode.Text = selectedRow.Cells("Barcode").Value.ToString()
                txtProductName.Text = selectedRow.Cells("ProductName").Value.ToString()

                ' Fetch unit price and retail price directly from database
                Using connection As New SqlConnection(connectionString)
                    connection.Open()
                    Dim query As String = "SELECT UnitPrice, RetailPrice FROM Products WHERE ProductID = @ProductID"
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

        ' Create the DELETE SQL command
        Dim query As String = "DELETE FROM Products WHERE ProductID = @ProductID"

        ' Use a database connection to execute the command
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                ' Add the ProductID parameter
                cmd.Parameters.AddWithValue("@ProductID", productID)

                Try
                    conn.Open()
                    cmd.ExecuteNonQuery() ' Execute the DELETE command
                    MessageBox.Show("Product deleted successfully.", "Success")
                Catch ex As Exception
                    MessageBox.Show("Error deleting product: " & ex.Message, "Error")
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
        ' Only perform the query if there is a search term entered
        If String.IsNullOrWhiteSpace(searchTerm) Then
            dgvProduct.DataSource = Nothing ' Optionally clear the grid if you want to remove data when search term is empty
            Return
        End If

        ' SQL query to search products based on Barcode, ProductName, or CategoryID
        Dim query As String = "SELECT Barcode, ProductName, UnitPrice, RetailPrice, CategoryID, Expiration " &
                              "FROM Products " &
                              "WHERE Barcode LIKE @search OR ProductName LIKE @search OR CategoryID LIKE @search"

        ' Set up the database connection and execute the query
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                ' Add the parameter to the query with the search term (includes wildcards for partial matching)
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")

                ' Create a DataAdapter and DataTable to hold the result
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()

                Try
                    conn.Open()
                    ' Fill the table with data
                    adapter.Fill(table)
                    ' Bind the results to the DataGridView
                    dgvProduct.DataSource = table
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
End Class

