Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports Microsoft.Data.SqlClient

Public Class Category
    ' SQL Server connection string
    Private connectionString As String = AppConfig.ConnectionString

    ' Store selected CategoryID (default -1 if no selection)

    Private selectedCategoryID As Integer = 0


    '=========== FOR LOAD ==================
    Private Sub LoadCategories(Optional searchQuery As String = "")
        Try
            Using conn As New SqlConnection(connectionString)
                ' SQL query with optional search filter
                Dim query As String = "SELECT CategoryID, CategoryName, CategoryType, Description, CreatedAt, UpdatedAt FROM Categories"

                ' Apply the search filter to multiple columns including CategoryType
                If Not String.IsNullOrWhiteSpace(searchQuery) Then
                    query &= " WHERE CategoryName LIKE @SearchQuery OR CategoryType LIKE @SearchQuery"
                End If

                ' Set up the SQL command and parameters
                Dim cmd As New SqlCommand(query, conn)
                If Not String.IsNullOrWhiteSpace(searchQuery) Then
                    cmd.Parameters.AddWithValue("@SearchQuery", "%" & searchQuery & "%")
                End If

                ' Create a data adapter to fill the DataTable with results
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                ' Debugging: Check column names (optional)
                For Each column As DataColumn In table.Columns
                    Debug.WriteLine("Column: " & column.ColumnName)
                Next

                ' Bind the data to the DataGridView
                dgvCategories.DataSource = table

                ' Ensure CategoryType is visible
                If dgvCategories.Columns.Contains("CategoryType") Then
                    dgvCategories.Columns("CategoryType").Visible = True
                    dgvCategories.Columns("CategoryType").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End If

                ' Hide the CategoryID column
                If dgvCategories.Columns.Contains("CategoryID") Then
                    dgvCategories.Columns("CategoryID").Visible = False
                End If

                ' Adjust column widths
                With dgvCategories
                    .Columns("CategoryName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns("CategoryType").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns("Description").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns("Description").FillWeight = 250 ' Adjust this value for more width

                    .Columns("CreatedAt").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns("UpdatedAt").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    ' Prevent the extra row from showing
                    dgvCategories.AllowUserToAddRows = False
                End With
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        If dgvCategories.Columns.Contains("UpdateAt") Then
            dgvCategories.Columns("UpdateAt").Visible = False ' Hide "Update" column
        End If

        If dgvCategories.Columns.Contains("CreateAt") Then
            dgvCategories.Columns("CreateAt").Visible = False ' Hide "Create" column
        End If

    End Sub
    Private Sub Category_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load categories when the form is loaded
        LoadCategories()

        ' Hide createdAt and updatedAt columns if they exist
        If dgvCategories.Columns.Contains("createdAt") Then
            dgvCategories.Columns("createdAt").Visible = False
        End If

        If dgvCategories.Columns.Contains("updatedAt") Then
            dgvCategories.Columns("updatedAt").Visible = False
        End If

        ' Set alternating row colors
        dgvCategories.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        dgvCategories.DefaultCellStyle.BackColor = Color.White
        dgvCategories.BackgroundColor = Color.White
        dgvCategories.AllowUserToAddRows = False

        ' Selection color
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCategories.DefaultCellStyle.SelectionBackColor = dgvCategories.BackgroundColor
        dgvCategories.DefaultCellStyle.SelectionForeColor = dgvCategories.ForeColor

        ' Add Edit column
        Dim editColumn As New DataGridViewImageColumn()
        editColumn.Name = "Edit"
        editColumn.HeaderText = "Select / Edit"
        editColumn.Width = 40
        Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
        editColumn.Image = Image.FromFile(imagePath)
        editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        dgvCategories.Columns.Add(editColumn)

        ' Setup DataGridView appearance
        SetupDataGridView()
        SetPlaceholders()

        ' Disable buttons if the role is "Staff"
        If SessionData.role.Equals("Staff", StringComparison.OrdinalIgnoreCase) Then
            btnAdd.Enabled = False
            btnEdit.Enabled = False
            btnDelete.Enabled = False
        End If
    End Sub


    '===== FOR CENTER SCREEN =====
    Private Sub CenterPanel()
        Dim formWidth As Integer = Me.ClientSize.Width
        Dim formHeight As Integer = Me.ClientSize.Height

        Dim panelWidth As Integer = CategoryPanel.Width
        Dim panelHeight As Integer = CategoryPanel.Height

        Dim x As Integer = (formWidth - panelWidth) \ 2
        Dim y As Integer = (formHeight - panelHeight) \ 2

        CategoryPanel.Location = New Point(x, y)
    End Sub


    '=========== BUTTONS ====================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        ' Check if any required fields are empty or contain only whitespace
        If String.IsNullOrWhiteSpace(txtCategoryName.Text) Then
            MessageBox.Show("Please fill in the category name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        ' Check for duplicate category name
        If CategoryExists(txtCategoryName.Text.Trim()) Then
            MessageBox.Show("Category name already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Insert category
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "INSERT INTO Categories (CategoryName, CategoryType, Description, CreatedAt, UpdatedAt) VALUES (@CategoryName, @CategoryType, @Description, GETDATE(), GETDATE())"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text.Trim())
                    cmd.Parameters.AddWithValue("@CategoryType", cboCategoryType.SelectedItem) ' Assuming this is the correct value
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim())

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    Logaudittrail(SessionData.role, SessionData.fullName, $"Added category: {txtCategoryName.Text.Trim()}")

                    MessageBox.Show("Category added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadCategories()
                    ResetFields()
                End Using
            End Using
        Catch ex As Exception
        End Try
        PanelCategory.Visible = False
    End Sub

    ' Check if the category exists
    Private Function CategoryExists(categoryName As String) As Boolean
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT COUNT(*) FROM Categories WHERE CategoryName = @CategoryName"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@CategoryName", categoryName)
                conn.Open()
                Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using
        End Using
    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If selectedCategoryID = -1 Then
            MessageBox.Show("Please select a category to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Get updated values from the form
        Dim updatedCategoryType As String = cboCategoryType.Text.Trim()
        Dim updatedDescription As String = txtDescription.Text.Trim()

        If String.IsNullOrWhiteSpace(updatedCategoryType) OrElse String.IsNullOrWhiteSpace(updatedDescription) Then
            MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Step 1: Fetch original values from the database
        Dim originalValues As New Dictionary(Of String, String)
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "SELECT CategoryType, Description, CategoryName FROM Categories WHERE CategoryID = @CategoryID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            originalValues("CategoryType") = reader("CategoryType").ToString()
                            originalValues("Description") = reader("Description").ToString()
                            originalValues("CategoryName") = reader("CategoryName").ToString()  ' Assuming CategoryName is a field
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching original values: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Step 2: Prepare updated values for logging
        Dim updatedCategoryName As String = txtCategoryName.Text.Trim()
        Dim updatedValues As New Dictionary(Of String, String) From {
        {"CategoryName", updatedCategoryName},
        {"CategoryType", updatedCategoryType},
        {"Description", updatedDescription}
    }

        ' Step 3: Compare original and updated values, and log changes
        Dim changes As String = $"Edited category: {updatedCategoryName}" & vbCrLf
        For Each key In originalValues.Keys
            If originalValues(key) <> updatedValues(key) Then
                changes &= $"{key} changed from '{originalValues(key)}' to '{updatedValues(key)}'" & vbCrLf
            End If
        Next

        ' Step 3: Update the database and log changes if any
        Try
            Using conn As New SqlConnection(connectionString)
                Dim updateQuery As String = "UPDATE Categories SET CategoryName = @CategoryName, CategoryType = @CategoryType, Description = @Description, UpdatedAt = GETDATE() WHERE CategoryID = @CategoryID"
                Using cmd As New SqlCommand(updateQuery, conn)
                    cmd.Parameters.AddWithValue("@CategoryName", updatedCategoryName)
                    cmd.Parameters.AddWithValue("@CategoryType", updatedCategoryType)
                    cmd.Parameters.AddWithValue("@Description", updatedDescription)
                    cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID)

                    conn.Open()
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        ' Log changes if any
                        If changes.Length > 0 Then
                            Logaudittrail(SessionData.role, SessionData.fullName, changes)
                        End If

                        MessageBox.Show("Category updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadCategories()
                        ResetFields()
                    Else
                        MessageBox.Show("No changes were made to the category.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error updating category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' Get the current user role and show debug info
        Dim userRole As String = GetCurrentUserRole()
        MessageBox.Show("Current user role after GetCurrentUserRole: " & userRole, "Debug Info")

        ' First check if the user has permission to delete
        If userRole.ToUpper() = "STAFF" Then
            MessageBox.Show("Staff members are not authorized to delete categories.",
                           "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If selectedCategoryID = -1 Then
            MessageBox.Show("Please select a category to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show($"Are you sure you want to delete the category '{txtCategoryName.Text}'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result <> DialogResult.Yes Then Return

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Step 1: Update foreign keys in the child table to null (or default value)
                Dim updateChildQuery As String = "UPDATE Products SET CategoryID = NULL WHERE CategoryID = @CategoryID"
                Using updateCmd As New SqlCommand(updateChildQuery, conn)
                    updateCmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID)
                    updateCmd.ExecuteNonQuery()
                End Using

                ' Step 2: Delete the category record from Categories table
                Dim deleteQuery As String = "DELETE FROM Categories WHERE CategoryID = @CategoryID"
                Using deleteCmd As New SqlCommand(deleteQuery, conn)
                    deleteCmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID)
                    deleteCmd.ExecuteNonQuery()
                End Using

                ' Log the action
                Dim actionDescription = $"Deleted The Category: {txtCategoryName.Text} Type: {cboCategoryType.Text}, Description: {txtDescription.Text}"
                Logaudittrail(SessionData.role, SessionData.fullName, actionDescription)

                MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadCategories()
                ResetFields()

            End Using
        Catch ex As Exception
            MessageBox.Show("Error deleting category: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetFields()
        btnAdd.Visible = True
    End Sub


    '========== FOR DGV ==================
    Private Sub dgvCategories_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCategories.CellContentClick
        ' Ensure that the clicked index is valid and it's not a header row (e.RowIndex >= 0)
        If e.RowIndex >= 0 AndAlso e.RowIndex < dgvCategories.Rows.Count Then
            ' Get the name of the clicked column
            Dim columnName As String = dgvCategories.Columns(e.ColumnIndex).Name

            If columnName = "Edit" Then
                ' Show the category edit panel (if it's hidden)
                CategoryPanel.Visible = True

                ' Load the data from the selected row into the form fields
                Dim selectedRow As DataGridViewRow = dgvCategories.Rows(e.RowIndex)
                txtCategoryName.Text = selectedRow.Cells("CategoryName").Value.ToString()
                cboCategoryType.SelectedItem = selectedRow.Cells("CategoryType").Value.ToString() ' Assuming it's a dropdown
                txtDescription.Text = selectedRow.Cells("Description").Value.ToString()



            ElseIf columnName = "Delete" Then
                ' Get the current user role
                Dim userRole As String = GetCurrentUserRole()

                ' Check if the user has permission to delete
                If userRole.ToUpper() = "STAFF" Then
                    MessageBox.Show("Staff members are not authorized to delete categories.",
                                   "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Get the CategoryID of the selected row
                Dim selectedRow As DataGridViewRow = dgvCategories.Rows(e.RowIndex)
                Dim categoryIDToDelete As Integer = Convert.ToInt32(selectedRow.Cells("CategoryID").Value)

                ' Ask for confirmation before deleting
                Dim confirm = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButtons.YesNo)
                If confirm = DialogResult.Yes Then
                    ' Remove the row from DataGridView
                    dgvCategories.Rows.RemoveAt(e.RowIndex)

                    ' Call method to delete the category from the database
                    DeleteCategoryFromDatabase(categoryIDToDelete)
                End If
            End If
        End If
    End Sub
    Private Sub DeleteCategoryFromDatabase(categoryID As Integer)
        Using conn As New SqlConnection(AppConfig.ConnectionString)
            Dim query As String = "DELETE FROM Categories WHERE CategoryID = @CategoryID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@CategoryID", categoryID)

                Try
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Category deleted successfully.")
                Catch ex As Exception
                    MessageBox.Show("Error deleting category: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvCategories.BackgroundColor = Color.White
        dgvCategories.BorderStyle = BorderStyle.None
        dgvCategories.AllowUserToAddRows = False
        dgvCategories.ReadOnly = True

        dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCategories.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvCategories.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvCategories.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvCategories.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvCategories.EnableHeadersVisualStyles = False
        dgvCategories.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvCategories.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvCategories.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvCategories.ColumnHeadersHeight = 45
    End Sub
    Private Sub dgvCategories_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCategories.CellClick
        If e.RowIndex >= 0 Then
            ' Get the selected row from DataGridView
            Dim row As DataGridViewRow = dgvCategories.Rows(e.RowIndex)

            ' Get the category ID from the selected row (hidden column)
            selectedCategoryID = Convert.ToInt32(row.Cells("CategoryID").Value)

            ' Fill the TextBox and ComboBox with the selected row values
            txtCategoryName.Text = row.Cells("CategoryName").Value.ToString()

            ' Ensure ComboBox is populated before setting the value
            If cboCategoryType.Items.Contains(row.Cells("CategoryType").Value.ToString()) Then
                cboCategoryType.SelectedItem = row.Cells("CategoryType").Value.ToString() ' Set the selected item
            End If

            ' Set the description TextBox value
            txtDescription.Text = row.Cells("Description").Value.ToString()
        End If

    End Sub


    '============ FOR KEY PRESS ================ 
    Private Sub txtCategoryName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCategoryName.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = " " Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboCategoryType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboCategoryType.KeyPress
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Not Char.IsControl(e.KeyChar) AndAlso e.KeyChar <> " " Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtDescription_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDescription.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) AndAlso Not Char.IsPunctuation(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub



    '=========== RESET FIELDS ==================
    Private Sub ResetFields()
        txtCategoryName.Clear()
        cboCategoryType.SelectedIndex = -1
        txtDescription.Clear()
        selectedCategoryID = -1
        txtSearch.Clear()
    End Sub


    '========== PLACE HOLDER =======
    Private Sub SetPlaceholders()
        ' Set placeholder text for category-related fields
        txtCategoryName.PlaceholderText = "Enter Category Name"
        cboCategoryType.SelectedIndex = -1 ' Reset the combo box selection
        txtDescription.PlaceholderText = "Enter Category Description"
    End Sub


    '========== FOR UI SEARCH ==================

    Private Sub ClearCategoryDetails()
        ' Clear all TextBox controls
        txtCategoryName.Clear()

        ' Reset ComboBox selections
        cboCategoryType.SelectedIndex = -1
        txtDescription.Clear()
        ' Optionally, disable buttons if no category is selected
        btnEdit.Enabled = False
        btnDelete.Enabled = False
    End Sub





    '============= AUDIT TRAIL =================
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
                    cmd.Parameters.AddWithValue("@Form", "Category Form")
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        CategoryPanel.Visible = True

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CategoryPanel.Visible = False
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Me.Close()
    End Sub

    Private Sub CategoryPanel_Paint(sender As Object, e As PaintEventArgs) Handles CategoryPanel.Paint
        CenterPanel()
        CategoryPanel.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub



    '============= FOR SEARCH =============
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadCategoryData(txtSearch.Text)
    End Sub

    Private Sub LoadCategoryData(searchTerm As String)
        Dim query As String = "SELECT CategoryID, CategoryName, CategoryType, Description, CreatedAt, UpdatedAt " &
                          "FROM Categories " &
                          "WHERE CategoryName LIKE @search OR CategoryType LIKE @search"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")
                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()

                Try
                    conn.Open()
                    adapter.Fill(table)
                    dgvCategories.DataSource = table ' Assuming dgvCategories is your DataGridView for Categories
                Catch ex As Exception
                    MessageBox.Show("Database Error: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Function GetCurrentUserRole() As String
        ' Debug: Show the current SessionData
        MessageBox.Show("SessionData Debug Info:" & vbCrLf &
                        "fullName: " & If(SessionData.fullName Is Nothing, "null", SessionData.fullName) & vbCrLf &
                        "role: " & If(SessionData.role Is Nothing, "null", SessionData.role),
                        "SessionData Debug")

        ' If SessionData.role is already available, use it
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
