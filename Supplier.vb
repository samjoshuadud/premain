Imports Microsoft.Data.SqlClient

Public Class Supplier
    ' Your SQL Server connection string
    Private connectionString As String = AppConfig.ConnectionString

    Private selectedSupplierId As Integer ' Declare the variable at the class level


    '============= FOR LOAD =============

    ' Form load event to load supplier data into DataGridView
    Private Sub Supplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSuppliers() ' Load suppliers when the form is loaded

        ' Set DataGridView settings and appearance
        With dgvSuppliers
            .AllowUserToAddRows = False
            .BackgroundColor = Color.White
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .DefaultCellStyle.SelectionBackColor = .BackgroundColor
            .DefaultCellStyle.SelectionForeColor = .ForeColor
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
            .DefaultCellStyle.BackColor = Color.White
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End With

        ' Setup DataGridView appearance
        SetupDataGridView()

        ' Set specific column widths
        If dgvSuppliers.Columns.Contains("CompanyName") Then
            dgvSuppliers.Columns("CompanyName").Width = 250
        End If
        If dgvSuppliers.Columns.Contains("ContactName") Then
            dgvSuppliers.Columns("ContactName").Width = 250
        End If
        If dgvSuppliers.Columns.Contains("ContactNumber") Then
            dgvSuppliers.Columns("ContactNumber").Width = 250
        End If
        If dgvSuppliers.Columns.Contains("Email") Then
            dgvSuppliers.Columns("Email").Width = 350
        End If

        ' Add Edit column if not already added
        If dgvSuppliers.Columns.Contains("Edit") = False Then
            Dim editColumn As New DataGridViewImageColumn()
            editColumn.Name = "Edit"
            editColumn.HeaderText = "Select / Edit"
            editColumn.Width = 30
            Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
            editColumn.Image = Image.FromFile(imagePath) ' <-- path to your image

            dgvSuppliers.Columns.Add(editColumn)
        End If

        '' Add Delete column if not already added
        'If dgvSuppliers.Columns.Contains("Delete") = False Then
        '    Dim deleteColumn As New DataGridViewImageColumn()
        '    deleteColumn.Name = "Delete"
        '    deleteColumn.HeaderText = "Delete"
        '    deleteColumn.Width = 30
        '    deleteColumn.Image = Image.FromFile("C:\Users\Aspire 5\source\repos\oreo-main\Resources\icons8-delete-35.png") ' <-- path to your image
        '    dgvSuppliers.Columns.Add(deleteColumn)
        'End If

        SetPlaceholders()
    End Sub


    ' Load suppliers into DataGridView (filtered by search term if provided)
    Private Sub LoadSuppliers(Optional searchTerm As String = "")
        Using connection As New SqlConnection(connectionString)
            Dim query As String = "SELECT supplierid, CompanyName, contactname, contactnumber, email, address FROM suppliers"

            If Not String.IsNullOrEmpty(searchTerm) Then
                query &= " WHERE CompanyName LIKE @searchTerm OR contactname LIKE @searchTerm OR contactnumber LIKE @searchTerm OR email LIKE @searchTerm OR address LIKE @searchTerm"
            End If

            Dim adapter As New SqlDataAdapter(query, connection)

            If Not String.IsNullOrEmpty(searchTerm) Then
                adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" & searchTerm & "%")
            End If

            Dim table As New DataTable()
            adapter.Fill(table)
            dgvSuppliers.DataSource = table

            ' Hide supplierid
            If dgvSuppliers.Columns.Contains("supplierid") Then
                dgvSuppliers.Columns("supplierid").Visible = False
            End If

            ' Set fill mode for text columns
            dgvSuppliers.Columns("CompanyName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSuppliers.Columns("contactname").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSuppliers.Columns("contactnumber").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSuppliers.Columns("email").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvSuppliers.Columns("address").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End Using
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvSuppliers.BackgroundColor = Color.White
        dgvSuppliers.BorderStyle = BorderStyle.None
        dgvSuppliers.AllowUserToAddRows = False
        dgvSuppliers.ReadOnly = True
        dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvSuppliers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvSuppliers.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvSuppliers.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance with uppercase first letter
        dgvSuppliers.EnableHeadersVisualStyles = False
        dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

        ' Set all column headers to have the first letter capitalized
        For Each column As DataGridViewColumn In dgvSuppliers.Columns
            column.HeaderText = CapitalizeFirstLetter(column.HeaderText)
        Next

        ' Customize header height
        dgvSuppliers.ColumnHeadersHeight = 35
    End Sub

    ' Function to capitalize the first letter of a string
    Private Function CapitalizeFirstLetter(input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return input
        End If

        ' Capitalize the first letter and keep the rest as it is
        Return Char.ToUpper(input(0)) & input.Substring(1)
    End Function



    '============= FOR BUTTON ================

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Get the supplier details from the form
        Dim CompanyName As String = txtCompanyName.Text
        Dim contactName As String = txtContactName.Text
        Dim contactNumber As String = txtContactNumber.Text
        Dim email As String = txtEmail.Text
        Dim address As String = txtAddress.Text

        ' Validate required fields
        If String.IsNullOrEmpty(CompanyName) OrElse String.IsNullOrEmpty(contactName) Then
            MessageBox.Show("Please provide the necessary supplier details.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Add the new supplier to the database
        Using connection As New SqlConnection(connectionString)
            Dim insertQuery As String = "INSERT INTO suppliers (CompanyName, contactname, contactnumber, email, address) " &
                                    "VALUES (@CompanyName, @contactName, @contactNumber, @email, @address)"
            Dim command As New SqlCommand(insertQuery, connection)

            command.Parameters.AddWithValue("@CompanyName", CompanyName)
            command.Parameters.AddWithValue("@contactName", contactName)
            command.Parameters.AddWithValue("@contactNumber", contactNumber)
            command.Parameters.AddWithValue("@email", email)
            command.Parameters.AddWithValue("@address", address)

            connection.Open()
            command.ExecuteNonQuery()
        End Using

        ' Notify the user
        MessageBox.Show("New supplier added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Reload the suppliers and reset the form fields
        LoadSuppliers()
        ResetForm()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If selectedSupplierId > 0 Then
            ' Get the updated details from the form
            Dim CompanyName As String = txtCompanyName.Text
            Dim contactName As String = txtContactName.Text
            Dim contactNumber As String = txtContactNumber.Text
            Dim email As String = txtEmail.Text
            Dim address As String = txtAddress.Text

            ' Ensure that supplierName and contactName are provided
            If String.IsNullOrEmpty(CompanyName) OrElse String.IsNullOrEmpty(contactName) Then
                MessageBox.Show("Please provide the necessary supplier details.", "Please Select", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Fetch the original details of the supplier from the database (for comparison)
            Dim originalUser As New Dictionary(Of String, String)

            Using connection As New SqlConnection(connectionString)
                Dim query As String = "SELECT CompanyName, contactname, contactnumber, email, address FROM suppliers WHERE supplierid = @supplierId"
                Dim command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@supplierId", selectedSupplierId)

                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        originalUser.Add("CompanyName", reader("CompanyName").ToString())
                        originalUser.Add("contactname", reader("contactname").ToString())
                        originalUser.Add("contactnumber", reader("contactnumber").ToString())
                        originalUser.Add("email", reader("email").ToString())
                        originalUser.Add("address", reader("address").ToString())
                    End If
                End Using
            End Using

            ' Now we compare the original details with the updated details
            Dim changes As String = "Edited supplier: " & txtCompanyName.Text & vbCrLf

            ' Create a dictionary for the updated details
            Dim updatedUser As New Dictionary(Of String, String) From {
        {"CompanyName", CompanyName},
        {"contactname", contactName},
        {"contactnumber", contactNumber},
        {"email", email},
        {"address", address}
    }

            ' Loop through each field to compare the original and updated values
            For Each key In originalUser.Keys
                If originalUser(key) <> updatedUser(key) Then
                    ' If the value is different, log the field name and the changes
                    changes &= $"{key} changed from '{originalUser(key)}' to '{updatedUser(key)}'" & vbCrLf
                End If
            Next

            ' Log the change in the audit trail
            Dim currentRole As String = SessionData.role  ' Get the role of the logged-in user
            Dim currentFullName As String = SessionData.fullName  ' Get the full name of the logged-in user

            ' Call Logaudittrail to log the changes
            Logaudittrail(currentRole, currentFullName, changes)

            ' Now update the supplier in the database
            Using connection As New SqlConnection(connectionString)
                Dim updateQuery As String = "UPDATE suppliers SET CompanyName = @CompanyName, contactname = @contactName, " &
                                    "contactnumber = @contactNumber, email = @email, address = @address WHERE supplierid = @supplierId"
                Dim command As New SqlCommand(updateQuery, connection)

                ' Add parameters to the update command
                command.Parameters.AddWithValue("@CompanyName", CompanyName)
                command.Parameters.AddWithValue("@contactName", contactName)
                command.Parameters.AddWithValue("@contactNumber", contactNumber)
                command.Parameters.AddWithValue("@email", email)
                command.Parameters.AddWithValue("@address", address)
                command.Parameters.AddWithValue("@supplierId", selectedSupplierId)

                connection.Open()
                command.ExecuteNonQuery()
            End Using

            ' Now update the products related to this supplier
            Using connection As New SqlConnection(connectionString)
                Dim updateProductsQuery As String = "UPDATE Products SET SupplierID = @supplierId WHERE SupplierID = @oldSupplierId"
                Dim command As New SqlCommand(updateProductsQuery, connection)

                ' Add parameters to update the products (if necessary)
                command.Parameters.AddWithValue("@supplierId", selectedSupplierId)
                command.Parameters.AddWithValue("@oldSupplierId", selectedSupplierId)

                connection.Open()
                command.ExecuteNonQuery()
            End Using

            ' Notify user and reload the suppliers and reset the form fields
            MessageBox.Show("Supplier updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Reload the suppliers and reset the form fields
            LoadSuppliers()
            ResetForm()

        Else
            MessageBox.Show("Please select a supplier to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' Ensure that a supplier is selected in the DataGridView before deleting
        If selectedSupplierId <= 0 Then
            MessageBox.Show("Please select a supplier to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Step 1: Retrieve supplier details before deleting
        Dim supplierDetails As New Dictionary(Of String, String)
        Dim originalQuery As String = "SELECT CompanyName, contactname, email FROM suppliers WHERE supplierid = @supplierId"
        Try
            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand(originalQuery, connection)
                command.Parameters.AddWithValue("@supplierId", selectedSupplierId)
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        supplierDetails("CompanyName") = reader("CompanyName").ToString()
                        supplierDetails("ContactName") = reader("contactname").ToString()
                        supplierDetails("Email") = reader("email").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching supplier data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End Try

        ' Step 2: Confirm the deletion action
        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete this supplier? The related product and inventory records will not be deleted, but their SupplierID will be set to NULL.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            Try
                ' Step 3: Disassociate the supplier from the product and inventory records (set supplierid to NULL)
                Using connection As New SqlConnection(connectionString)
                    connection.Open()

                    ' Set the supplierid to NULL in the Product table (disassociate the product from the supplier)
                    Dim updateProductCommand As New SqlCommand("UPDATE Products SET supplierid = NULL WHERE supplierid = @supplierId", connection)
                    updateProductCommand.Parameters.AddWithValue("@supplierId", selectedSupplierId)
                    updateProductCommand.ExecuteNonQuery()

                    ' Set the supplierid to NULL in the Inventory table (disassociate the inventory from the supplier)
                    Dim updateInventoryCommand As New SqlCommand("UPDATE Inventory SET supplierid = NULL WHERE supplierid = @supplierId", connection)
                    updateInventoryCommand.Parameters.AddWithValue("@supplierId", selectedSupplierId)
                    updateInventoryCommand.ExecuteNonQuery()

                    ' Step 4: Delete the supplier from the suppliers table
                    Dim deleteSupplierCommand As New SqlCommand("DELETE FROM suppliers WHERE supplierid = @supplierId", connection)
                    deleteSupplierCommand.Parameters.AddWithValue("@supplierId", selectedSupplierId)
                    deleteSupplierCommand.ExecuteNonQuery()
                End Using

                ' Step 5: Log the deletion in the audit trail
                Dim actionDescription As String = $"Deleted Supplier: {supplierDetails("CompanyName")}"
                Logaudittrail(SessionData.role, SessionData.fullName, actionDescription)

                ' Step 6: Notify user and reload the supplier list
                MessageBox.Show("Supplier has been successfully deleted.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadSuppliers()
                ResetForm()
            Catch ex As Exception
                MessageBox.Show("An error occurred while deleting the supplier: " & ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        ' Enable add button after deletion
        btnAdd.Enabled = True
    End Sub
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtAddress.Clear()
        txtContactName.Clear()
        txtEmail.Clear()
        txtContactNumber.Clear()
        txtCompanyName.Clear()

    End Sub

    '============== FOR DGV =====================
    Private Sub dgvSuppliers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSuppliers.CellClick
        ' Ensure that a valid row is selected
        If e.RowIndex >= 0 Then
            ' Get the selected supplier's data from DataGridView
            selectedSupplierId = Convert.ToInt32(dgvSuppliers.Rows(e.RowIndex).Cells("Supplierid").Value)
            Dim CompanyName As String = dgvSuppliers.Rows(e.RowIndex).Cells("CompanyName").Value.ToString()
            Dim contactName As String = dgvSuppliers.Rows(e.RowIndex).Cells("Contactname").Value.ToString()
            Dim contactNumber As String = dgvSuppliers.Rows(e.RowIndex).Cells("Contactnumber").Value.ToString()
            Dim email As String = dgvSuppliers.Rows(e.RowIndex).Cells("Email").Value.ToString()
            Dim address As String = dgvSuppliers.Rows(e.RowIndex).Cells("Address").Value.ToString()

            ' Fill textboxes with the selected supplier's data
            txtCompanyName.Text = CompanyName
            txtContactName.Text = contactName
            txtContactNumber.Text = contactNumber
            txtEmail.Text = email
            txtAddress.Text = address
        End If
    End Sub
    Private Sub dgvSuppliers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSuppliers.CellContentClick
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = dgvSuppliers.Rows(e.RowIndex)

            If dgvSuppliers.Columns(e.ColumnIndex).Name = "Edit" Then
                ' Show the panel
                PanelSupplier.Visible = True

                ' Fill the textboxes with selected row values
                txtCompanyName.Text = selectedRow.Cells("CompanyName").Value.ToString()
                txtContactName.Text = selectedRow.Cells("ContactName").Value.ToString()
                txtEmail.Text = selectedRow.Cells("Email").Value.ToString()
                txtContactNumber.Text = selectedRow.Cells("ContactNumber").Value.ToString()

            ElseIf dgvSuppliers.Columns(e.ColumnIndex).Name = "Delete" Then
                Dim confirm = MessageBox.Show("Delete this supplier?", "Confirm", MessageBoxButtons.YesNo)
                If confirm = DialogResult.Yes Then
                    dgvSuppliers.Rows.RemoveAt(e.RowIndex)
                End If
            End If
        End If
    End Sub

    '============== FOR RESET =================
    Private Sub ResetForm()
        selectedSupplierId = 0 ' Clear the selected supplier ID
        txtCompanyName.Clear()
        txtContactName.Clear()
        txtContactNumber.Clear()
        txtEmail.Clear()
        txtAddress.Clear()
    End Sub

    '============== FOR AUDIT TRAIL ================
    Private Sub Logaudittrail(ByVal role As String, ByVal fullName As String, ByVal action As String)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO audittrail (Role, FullName, Action, Form, Date) VALUES (@Role, @FullName, @action, @Form, @Date)"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@Role", role)
                    cmd.Parameters.AddWithValue("@FullName", fullName)
                    cmd.Parameters.AddWithValue("@action", action)
                    cmd.Parameters.AddWithValue("@Form", "Supplier Form") ' You can change this based on your context
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub SetPlaceholders()
        ' Set placeholder text for each textbox
        txtCompanyName.PlaceholderText = "Enter the Supplier's Name"
        txtContactName.PlaceholderText = "Enter the Contact Person's Name"
        txtContactNumber.PlaceholderText = "Enter the Contact Number (e.g., +1234567890)"
        txtEmail.PlaceholderText = "Enter the Email Address (e.g., user@example.com)"
        txtAddress.PlaceholderText = "Enter the Complete Address"
    End Sub




    '================== KEY PRESSS =======================
    Private Sub txtCompanyName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCompanyName.KeyPress
        ' Allow only letters and spaces, as well as control keys (e.g., backspace)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> " "c Then
            e.Handled = True ' Block invalid characters
        End If
    End Sub

    Private Sub txtContactName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactName.KeyPress
        ' Allow only letters and spaces, as well as control keys (e.g., backspace)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> " "c Then
            e.Handled = True ' Block invalid characters
        End If
    End Sub

    Private Sub txtContactNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactNumber.KeyPress
        ' Check if the pressed key is a digit or control key (e.g., backspace)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True ' Block invalid characters
        End If

        ' Ensure that no more than 11 digits are allowed
        If txtContactNumber.Text.Length >= 11 AndAlso Char.IsDigit(e.KeyChar) Then
            e.Handled = True ' Prevent additional digits
        End If
    End Sub

    Private Sub txtEmail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmail.KeyPress
        ' Allow letters, digits, '@', '.', '-', '_', and control keys (e.g., backspace)
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse e.KeyChar = "@"c OrElse e.KeyChar = "."c OrElse e.KeyChar = "-"c OrElse e.KeyChar = "_"c OrElse Char.IsControl(e.KeyChar)) Then
            e.Handled = True ' Block invalid characters
        End If
    End Sub

    Private Sub txtAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAddress.KeyPress
        ' Allow letters, numbers, spaces, commas, periods, and control keys
        If Not (Char.IsLetterOrDigit(e.KeyChar) OrElse e.KeyChar = " "c OrElse e.KeyChar = ","c OrElse e.KeyChar = "."c OrElse Char.IsControl(e.KeyChar)) Then
            e.Handled = True ' Block invalid characters
        End If
    End Sub


    '======== FOR PANEL =========
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
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles PanelSupplier.Paint
        PanelSupplier.Left = (Me.ClientSize.Width - PanelSupplier.Width) \ 2
        PanelSupplier.Top = (Me.ClientSize.Height - PanelSupplier.Height) \ 2
        PanelSupplier.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub


    '======= FOR BUTTON 2 =================
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PanelSupplier.Visible = True
        ResetForm()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PanelSupplier.Visible = False
        ResetForm()


    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Me.Close()
    End Sub


    '============ FOR SEARCH ===============
    Private Sub txtSearchSupplier_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadSupplierData(txtSearch.Text)
    End Sub

    Private Sub LoadSupplierData(searchTerm As String)
        ' SQL query to search suppliers based on SupplierName, ContactName, or Email
        Dim query As String = "SELECT SupplierID, CompanyName, ContactName, ContactNumber, Email, Address " &
                          "FROM Suppliers " &
                          "WHERE CompanyName LIKE @search OR ContactName LIKE @search OR Email LIKE @search"

        ' Set up the database connection and execute the query
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()

                Try
                    conn.Open()
                    adapter.Fill(table)
                    dgvSuppliers.DataSource = table ' Display results in the DataGridView
                Catch ex As Exception
                    MessageBox.Show("Database Error: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

End Class
