Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.Data.SqlClient
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ApplicationServices   ' Add explicit import for WinForms

Partial Public Class User
    ' SQL Server connection string
    Private connectionString As String = AppConfig.ConnectionString

    ' Variable to store selected UserID for editing or deleting
    Private selectedUserID As Integer

    ' Declare the Exit button
    Private btnExit As Button

    Dim allowClose As Boolean = False

    Public Sub New()
        InitializeComponent()

        '========= BUTTON HANDLER ====== 

        AddHandler btnAdd.Click, AddressOf btnAdd_Click
        AddHandler btnEdit.Click, AddressOf btnEdit_Click
        AddHandler btnDelete.Click, AddressOf btnDelete_Click
        AddHandler btnReset.Click, AddressOf btnReset_Click
        AddHandler btnCLose.Click, AddressOf btnClose_Click
        AddHandler btnClosePanel.Click, AddressOf btnClosePanel_Click
        AddHandler btnAddUser.Click, AddressOf btnAddUser_Click
        AddHandler dgvUsers.CellContentClick, AddressOf dgvUsers_CellClick
        AddHandler dgvUsers.CellClick, AddressOf dgvUsers_CellClick
        AddHandler dgvUsers.MouseDown, AddressOf dgvUsers_MouseDown

    End Sub


    '=============== FOR LOAD ===========================
    Private Sub User_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Mask password fields
        txtPassword.UseSystemPasswordChar = True
        txtConfirmPassword.UseSystemPasswordChar = True
        txtPassword.Enabled = True
        txtConfirmPassword.Enabled = True
        ' Set tooltips for better user guidance
        SetPlaceholders()

        ' Load user data
        LoadUserData()

        ' Configure the DataGridView
        SetupDataGridView()

        CenterPanel()

        Dim editColumn As New DataGridViewImageColumn()
        editColumn.Name = "Edit"
        editColumn.HeaderText = "Select / Edit"
        editColumn.Width = 30
        Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
        editColumn.Image = Image.FromFile(imagePath)
        dgvUsers.Columns.Add(editColumn)

        'Dim deleteColumn As New DataGridViewImageColumn()
        'deleteColumn.Name = "Delete"
        'deleteColumn.HeaderText = "Delete"
        'deleteColumn.Width = 30
        'deleteColumn.Image = Image.FromFile("C:\Users\Aspire 5\source\repos\oreo-main\Resources\icons8-delete-35.png") ' <-- path to your image
        'dgvUsers.Columns.Add(deleteColumn)


        ' Button handler
        AddHandler btnCLose.Click, AddressOf btnClose_Click

        ' Set MaxLength for all textboxes to 255, except txtAge
        txtFirstName.MaxLength = 255
        txtLastName.MaxLength = 255
        txtMiddleInitial.MaxLength = 255
        txtUsername.MaxLength = 255
        txtPassword.MaxLength = 255
        txtConfirmPassword.MaxLength = 255
        txtEmailAddress.MaxLength = 255
        txtPhoneNumber.MaxLength = 255
        txtAddress.MaxLength = 255

        ' For Age, no MaxLength, but we can optionally set a max of 3 digits
        txtAge.MaxLength = 2  ' Optional, as Age is usually 3 digits or less
    End Sub



    Private Sub dgvUsers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellContentClick
        ' Ensure that the clicked index is valid and it's not a header row (e.RowIndex >= 0)
        If e.RowIndex >= 0 AndAlso e.RowIndex < dgvUsers.Rows.Count Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = dgvUsers.Rows(e.RowIndex)

            ' Get the column name of the clicked cell
            Dim columnName As String = dgvUsers.Columns(e.ColumnIndex).Name

            If columnName = "Edit" Then
                ' Show the user edit panel (if it's hidden)
                UserAddPanel.Visible = True

                ' Load data from the selected row into the form fields, with DBNull handling
                txtFirstName.Text = GetCellValue(selectedRow, "FirstName")
                txtMiddleInitial.Text = GetCellValue(selectedRow, "MiddleInitial")
                txtLastName.Text = GetCellValue(selectedRow, "LastName")
                txtUsername.Text = GetCellValue(selectedRow, "Username")
                txtPassword.Text = GetCellValue(selectedRow, "Password")
                txtEmailAddress.Text = GetCellValue(selectedRow, "Email")

                ' Handle Role population properly
                Dim roleValue As String = GetCellValue(selectedRow, "Role")
                If cbRole.Items.Contains(roleValue) Then
                    cbRole.SelectedItem = roleValue
                Else
                    cbRole.SelectedItem = Nothing
                End If

                cbGender.Text = GetCellValue(selectedRow, "Gender")
                txtAge.Text = GetCellValue(selectedRow, "Age")
                txtAddress.Text = GetCellValue(selectedRow, "Address")
                txtPhoneNumber.Text = GetCellValue(selectedRow, "PhoneNumber")

            ElseIf columnName = "Delete" Then
                ' Get the UserID of the selected row
                Dim userIdToDelete As Integer = Convert.ToInt32(selectedRow.Cells("UserID").Value)

                ' Ask for confirmation before deleting
                Dim confirm = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo)
                If confirm = DialogResult.Yes Then
                    ' Ensure the row index is still valid before removing the row
                    If e.RowIndex >= 0 AndAlso e.RowIndex < dgvUsers.Rows.Count Then
                        ' Remove the row from the DataGridView
                        dgvUsers.Rows.RemoveAt(e.RowIndex)

                        ' Call method to delete the user from the database
                        DeleteUserFromDatabase(userIdToDelete)

                        ' Optionally, refresh the DataGridView after deletion
                        RefreshUsersGrid()
                    Else
                        MessageBox.Show("The row index is out of range.")
                    End If
                End If
            End If
        End If

        txtPassword.Enabled = False
        txtConfirmPassword.Enabled = False

    End Sub




    ' Helper function to handle DBNull values
    Private Function GetCellValue(row As DataGridViewRow, columnName As String) As String
        ' Check for DBNull and return an empty string if necessary
        If row.Cells(columnName).Value IsNot DBNull.Value Then
            Return row.Cells(columnName).Value.ToString()
        Else
            Return String.Empty
        End If
    End Function


    Private Sub DeleteUserFromDatabase(userId As Integer)
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "DELETE FROM Users WHERE UserID = @UserID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@UserID", userId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            MessageBox.Show("User deleted from database.")
        Catch ex As Exception
            MessageBox.Show("Error deleting user: " & ex.Message)
        End Try
    End Sub

    Private Sub RefreshUsersGrid()
        ' Reload users from database into DataGridView
        Try
            ' Store current column visibility and row visibility settings
            Dim hiddenColumns As New List(Of DataGridViewColumn)()
            For Each column As DataGridViewColumn In dgvUsers.Columns
                If column.Visible = False Then
                    hiddenColumns.Add(column)
                End If
            Next

            ' Reload data (use a BindingSource if you're binding the grid to a data source)
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT * FROM Users"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim dt As New DataTable()
                        dt.Load(reader)
                        dgvUsers.DataSource = dt
                    End Using
                End Using
            End Using

            ' Restore column visibility settings
            For Each column As DataGridViewColumn In hiddenColumns
                column.Visible = False
            Next

            ' Optionally, keep the row selection
            If dgvUsers.Rows.Count > 0 Then
                dgvUsers.Rows(0).Selected = True
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading users: " & ex.Message)
        End Try
    End Sub





    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvUsers.BackgroundColor = Color.White
        dgvUsers.BorderStyle = BorderStyle.None
        dgvUsers.AllowUserToAddRows = False
        dgvUsers.ReadOnly = True
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvUsers.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvUsers.EnableHeadersVisualStyles = False
        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvUsers.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvUsers.ColumnHeadersHeight = 35
    End Sub



    '====== CENTER SCREEN ============
    Private Sub CenterPanel()
        ' Make sure layout is up-to-date
        Me.PerformLayout()

        ' Get client area size of the form
        Dim formWidth As Integer = Me.ClientSize.Width
        Dim formHeight As Integer = Me.ClientSize.Height

        ' Get the panel size
        Dim panelWidth As Integer = UserAddPanel.Width
        Dim panelHeight As Integer = UserAddPanel.Height

        ' Calculate centered position
        Dim x As Integer = (formWidth - panelWidth) \ 2
        Dim y As Integer = (formHeight - panelHeight) \ 2

        ' Set the new location
        UserAddPanel.Location = New Point(x, y)
    End Sub


    '============= FOR LOAD USER ========================
    Private Sub LoadUserData()
        ' SQL query to fetch data from the Users table
        Dim query As String = "SELECT UserID, FirstName, MiddleInitial, LastName, Username, Password, Email, Role, Gender, Age, Address, PhoneNumber FROM Users"

        ' Establishing the SQL Server database connection
        Using conn As New SqlConnection(connectionString)
            Try
                ' Open the connection
                conn.Open()

                ' Create the SQL command using the query and connection
                Using cmd As New SqlCommand(query, conn)
                    Using adapter As New SqlDataAdapter(cmd)
                        Dim table As New DataTable()

                        ' Fill the DataTable with data from the database
                        adapter.Fill(table)

                        ' Check if there are rows in the table
                        If table.Rows.Count > 0 Then
                            ' Set the DataSource of the DataGridView to the filled DataTable
                            dgvUsers.DataSource = table
                        Else
                            ' If no data is found, show a message and clear the DataGridView
                            dgvUsers.DataSource = Nothing
                        End If
                    End Using
                End Using
            Catch ex As SqlException
                ' Handle any SQL exceptions (database connection or query errors)
                MessageBox.Show("Error connecting to the database: " & ex.Message)
            End Try
        End Using

        ' Hide the UserID column after loading data
        dgvUsers.Columns("UserID").Visible = False


    End Sub


    '============ FOR ALL BUTTONS ========================
    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        ' Check role value before processing
        If cbRole.SelectedItem IsNot Nothing AndAlso cbRole.SelectedItem.ToString().ToUpper().Contains("ADMIN") Then
            If AdminUserExists() Then
                MessageBox.Show("Only one administrator account is allowed. Please select a different role.",
                    "Admin Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cbRole.Focus()
                Return
            End If
        End If

        ' Validate if passwords match
        If txtPassword.Text.Trim <> txtConfirmPassword.Text.Trim Then
            MessageBox.Show("Passwords do not match. Please try again.", "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtConfirmPassword.Focus()
            txtConfirmPassword.SelectAll()
            Return
        End If

        ' Validate Email Format (Better Regex)
        Dim emailPattern = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
        If Not RegularExpressions.Regex.IsMatch(txtEmailAddress.Text.Trim, emailPattern) Then
            MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtEmailAddress.Focus()
            txtEmailAddress.SelectAll()
            Return
        End If

        ' Try parsing the value of txtAge to an integer
        Dim age As Integer
        If Not Integer.TryParse(txtAge.Text, age) OrElse age < 18 Then
            MessageBox.Show("Please enter a valid age, and it must be 18 or older.", "Invalid Age", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAge.Focus()
            txtAge.SelectAll()
            Return
        End If

        ' Validate Phone Number Format (11 digits)
        If Not RegularExpressions.Regex.IsMatch(txtPhoneNumber.Text.Trim, "^\d{11}$") Then
            MessageBox.Show("Please enter a valid phone number with 11 digits.", "Invalid Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPhoneNumber.Focus()
            txtPhoneNumber.SelectAll()
            Return
        End If

        ' Get Middle Initial (Allow Empty)
        Dim middleNameOrInitial As Object = DBNull.Value ' Default is NULL
        If Not String.IsNullOrWhiteSpace(txtMiddleInitial.Text) Then
            middleNameOrInitial = txtMiddleInitial.Text.Trim

            ' Validate format (Allow: J, J., Juvida)
            If Not RegularExpressions.Regex.IsMatch(middleNameOrInitial, "^[A-Za-z]{1,50}(\.)?$") Then
                MessageBox.Show("Middle Initial or Name must be valid. You can enter full middle name (e.g., 'James') or initials (e.g., 'J', 'J.')",
                "Invalid Middle Initial/Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtMiddleInitial.Focus()
                txtMiddleInitial.SelectAll()
                Return
            End If

            ' Auto-format: If single letter, add a period (J → J.)
            If middleNameOrInitial.ToString.Length = 1 Then
                middleNameOrInitial &= "."
            End If
        End If

        ' Check if trying to add an Admin when one already exists
        If cbRole.SelectedItem.ToString() = "Admin" AndAlso AdminUserExists() Then
            MessageBox.Show("Only one administrator account is allowed. Please select a different role.",
                   "Admin Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cbRole.Focus()
            Return
        End If


        ' Check if the username, phone number, or email already exists
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Check if the username already exists
                If CheckIfExists("Username", txtUsername.Text.Trim) Then
                    MessageBox.Show("The username you entered is already registered. Please choose a different one.",
                           "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtUsername.Focus()
                    txtUsername.SelectAll()
                    Return
                End If

                ' Check if phone number already exists
                If CheckIfExists("PhoneNumber", txtPhoneNumber.Text.Trim) Then
                    MessageBox.Show("The phone number you entered is already registered. Please choose a different one.",
                           "Duplicate Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtPhoneNumber.Focus()
                    txtPhoneNumber.SelectAll()
                    Return
                End If

                ' Check if the email already exists
                If CheckIfExists("Email", txtEmailAddress.Text.Trim) Then
                    MessageBox.Show("The email you entered is already registered. Please choose a different one.",
                           "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtEmailAddress.Focus()
                    txtEmailAddress.SelectAll()
                    Return
                End If

                ' Construct the SQL query with parameters (Middle Initial can be NULL)
                Dim query = "INSERT INTO Users (FirstName, MiddleInitial, LastName, Username, Password, Email, Role, Gender, Age, Address, PhoneNumber, DateCreated) 
       VALUES (@FirstName, @MiddleInitial, @LastName, @Username, @Password, @Email, @Role, @Gender, @Age, @Address, @PhoneNumber, GETDATE())"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim)
                    cmd.Parameters.AddWithValue("@MiddleInitial", middleNameOrInitial) ' Can be NULL
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim)
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim)
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim) ' Store plain text password (hashing recommended)
                    cmd.Parameters.AddWithValue("@Email", txtEmailAddress.Text.Trim)
                    cmd.Parameters.AddWithValue("@Role", cbRole.SelectedItem.ToString)
                    cmd.Parameters.AddWithValue("@Gender", cbGender.SelectedItem.ToString)
                    cmd.Parameters.AddWithValue("@Age", age)
                    cmd.Parameters.AddWithValue("@Address", If(String.IsNullOrWhiteSpace(txtAddress.Text), DBNull.Value, txtAddress.Text.Trim))
                    cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim)

                    cmd.ExecuteNonQuery()
                End Using
            End Using

            ' Now log the audit trail using session variables
            Dim actionDescription As String = "Added User" ' Action being performed
            Logaudittrail(SessionData.role, SessionData.fullName, actionDescription)

            MessageBox.Show("User added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields() ' Clear fields after successful insertion

        Catch ex As Exception
            MessageBox.Show("Error adding user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        LoadUserData() ' Refresh the data grid view after adding the user
        UserAddPanel.Visible = False
    End Sub

    ' Modify the AdminUserExists function to be more robust
    Private Function AdminUserExists(Optional currentUserId As Integer = 0) As Boolean
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                ' Using case-insensitive LIKE with wildcards to catch any variant of "Admin"
                Dim query As String = "SELECT COUNT(*) FROM Users WHERE UPPER(Role) LIKE '%ADMIN%' AND UserID <> @CurrentUserId"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CurrentUserId", currentUserId)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking for admin user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False ' Default to false on error (safer)
        End Try
    End Function





    ' Check if a value already exists in the database
    Private Function CheckIfExists(columnName As String, value As String) As Boolean
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Dim query As String = $"SELECT COUNT(*) FROM Users WHERE {columnName} = @Value"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Value", value)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs)
        ' Check if a user is selected
        If selectedUserID = 0 Then
            MessageBox.Show("Please select a user to edit.", "Select User", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Initialize dictionaries to hold original and updated values
        Dim originalUser As New Dictionary(Of String, String)
        Dim updatedUser As New Dictionary(Of String, String)

        ' Retrieve original user details from the database
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Query to fetch the original user details
                Dim query = "SELECT FirstName, LastName, Username, Email, PhoneNumber, Role, Gender, Age, Address, MiddleInitial FROM Users WHERE UserID = @UserID"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@UserID", selectedUserID)

                    ' Execute the command and read the original data
                    Using reader = command.ExecuteReader
                        If reader.Read Then
                            originalUser.Add("FirstName", reader("FirstName").ToString)
                            originalUser.Add("MiddleInitial", If(reader.IsDBNull(reader.GetOrdinal("MiddleInitial")), String.Empty, reader("MiddleInitial").ToString))
                            originalUser.Add("LastName", reader("LastName").ToString)
                            originalUser.Add("Username", reader("Username").ToString)
                            originalUser.Add("Email", reader("Email").ToString)
                            originalUser.Add("PhoneNumber", reader("PhoneNumber").ToString)
                            originalUser.Add("Role", reader("Role").ToString)
                            originalUser.Add("Gender", reader("Gender").ToString)
                            originalUser.Add("Age", reader("Age").ToString)
                            originalUser.Add("Address", If(reader.IsDBNull(reader.GetOrdinal("Address")), String.Empty, reader("Address").ToString))
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving user details: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Now, populate the updatedUser dictionary with the values from the form
        updatedUser.Add("FirstName", txtFirstName.Text.Trim)
        updatedUser.Add("MiddleInitial", txtMiddleInitial.Text.Trim)
        updatedUser.Add("LastName", txtLastName.Text.Trim)
        updatedUser.Add("Username", txtUsername.Text.Trim)
        updatedUser.Add("Email", txtEmailAddress.Text.Trim)
        updatedUser.Add("PhoneNumber", txtPhoneNumber.Text.Trim)
        updatedUser.Add("Role", cbRole.SelectedItem.ToString)
        updatedUser.Add("Gender", cbGender.SelectedItem.ToString)
        updatedUser.Add("Age", txtAge.Text.Trim)
        updatedUser.Add("Address", txtAddress.Text.Trim)

        If originalUser("Role").ToUpper() <> "ADMIN" AndAlso
           updatedUser("Role").ToUpper().Contains("ADMIN") AndAlso
           AdminUserExists(selectedUserID) Then
            MessageBox.Show("Only one administrator account is allowed. Please select a different role.",
                "Admin Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cbRole.Focus()
            Return
        End If

        ' Initialize the change log message
        Dim changes = "Edited user: " & txtUsername.Text & vbCrLf

        ' Compare the original and updated values and log the changes
        For Each key In originalUser.Keys
            If originalUser(key) <> updatedUser(key) Then
                ' Log the field name and the change made
                changes &= $"{key} changed from '{originalUser(key)}' to '{updatedUser(key)}'" & vbCrLf
            End If
        Next

        ' Check for duplicate username, email, or phone number (excluding the current user)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' Check for duplicate username
                If originalUser("Username") <> updatedUser("Username") Then
                    Dim usernameQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND UserID <> @UserID"
                    Using usernameCmd As New SqlCommand(usernameQuery, connection)
                        usernameCmd.Parameters.AddWithValue("@Username", updatedUser("Username"))
                        usernameCmd.Parameters.AddWithValue("@UserID", selectedUserID)
                        If Convert.ToInt32(usernameCmd.ExecuteScalar) > 0 Then
                            MessageBox.Show("The username is already taken. Please choose a different username.",
                                       "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtUsername.Focus()
                            txtUsername.SelectAll()
                            Return
                        End If
                    End Using
                End If

                ' Check for duplicate email
                If originalUser("Email") <> updatedUser("Email") Then
                    Dim emailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND UserID <> @UserID"
                    Using emailCmd As New SqlCommand(emailQuery, connection)
                        emailCmd.Parameters.AddWithValue("@Email", updatedUser("Email"))
                        emailCmd.Parameters.AddWithValue("@UserID", selectedUserID)
                        If Convert.ToInt32(emailCmd.ExecuteScalar) > 0 Then
                            MessageBox.Show("The email is already registered. Please use a different email.",
                                       "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtEmailAddress.Focus()
                            txtEmailAddress.SelectAll()
                            Return
                        End If
                    End Using
                End If

                ' Check for duplicate phone number
                If originalUser("PhoneNumber") <> updatedUser("PhoneNumber") Then
                    Dim phoneQuery = "SELECT COUNT(*) FROM Users WHERE PhoneNumber = @PhoneNumber AND UserID <> @UserID"
                    Using phoneCmd As New SqlCommand(phoneQuery, connection)
                        phoneCmd.Parameters.AddWithValue("@PhoneNumber", updatedUser("PhoneNumber"))
                        phoneCmd.Parameters.AddWithValue("@UserID", selectedUserID)
                        If Convert.ToInt32(phoneCmd.ExecuteScalar) > 0 Then
                            MessageBox.Show("The phone number is already registered. Please use a different phone number.",
                                       "Duplicate Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            txtPhoneNumber.Focus()
                            txtPhoneNumber.SelectAll()
                            Return
                        End If
                    End Using
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking for duplicates: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Now proceed to update the user in the database (if any changes were detected)
        If changes.Length > "Edited user: ".Length + txtUsername.Text.Length + vbCrLf.Length Then
            ' Confirm update with the user
            Dim confirmUpdate = MessageBox.Show("Are you sure you want to update this user?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If confirmUpdate = DialogResult.Yes Then
                ' Perform the database update
                Try
                    Using connection As New SqlConnection(connectionString)
                        connection.Open()

                        ' Construct the SQL query to update the user with parameters in the desired order
                        Dim query = "UPDATE Users SET 
                                   FirstName = @FirstName, 
                                   MiddleInitial = @MiddleInitial, 
                                   LastName = @LastName, 
                                   Username = @Username, 
                                   Password = @Password, 
                                   Email = @Email, 
                                   Role = @Role, 
                                   Gender = @Gender, 
                                   Age = @Age, 
                                   Address = @Address, 
                                   PhoneNumber = @PhoneNumber 
                                   WHERE UserID = @UserID"

                        Using cmd As New SqlCommand(query, connection)
                            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim)
                            cmd.Parameters.AddWithValue("@MiddleInitial", If(String.IsNullOrWhiteSpace(txtMiddleInitial.Text), DBNull.Value, txtMiddleInitial.Text.Trim))
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim)
                            cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim)
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim)
                            cmd.Parameters.AddWithValue("@Email", txtEmailAddress.Text.Trim)
                            cmd.Parameters.AddWithValue("@Role", cbRole.SelectedItem.ToString)
                            cmd.Parameters.AddWithValue("@Gender", cbGender.SelectedItem.ToString)
                            cmd.Parameters.AddWithValue("@Age", txtAge.Text.Trim)
                            cmd.Parameters.AddWithValue("@Address", If(String.IsNullOrWhiteSpace(txtAddress.Text), DBNull.Value, txtAddress.Text.Trim))
                            cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim)
                            cmd.Parameters.AddWithValue("@UserID", selectedUserID)

                            cmd.ExecuteNonQuery()
                        End Using
                    End Using

                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ClearFields() ' Clear the form fields
                    LoadUserData() ' Refresh the data grid view after updating the user

                    SessionData.fullName = SessionData.fullName
                    SessionData.role = SessionData.role
                    Logaudittrail(SessionData.role, SessionData.fullName, "Edit User")

                Catch ex As Exception
                    MessageBox.Show("Error updating user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Else
            ' If no changes, notify the user
            MessageBox.Show("No changes detected for the user.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs)
        If selectedUserID = 0 Then
            MessageBox.Show("Please select a user to delete.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' First check if the selected user is an Admin - don't allow deletion
        Dim isAdmin As Boolean = False
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT Role FROM Users WHERE UserID = @UserID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@UserID", selectedUserID)
                    Dim role As String = Convert.ToString(cmd.ExecuteScalar())
                    isAdmin = (role.ToUpper().Contains("ADMIN"))
                End Using
            End Using

            ' Block deletion if user is an Admin
            If isAdmin Then
                MessageBox.Show("Administrator accounts cannot be deleted for security reasons.",
                               "Deletion Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        Catch ex As Exception
            MessageBox.Show("Error checking user role: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Retrieve the user details (before deleting) to show to the user
        Dim userToDelete As New Dictionary(Of String, String)

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' Fetch the user details for the selected user
                Dim selectQuery = "SELECT FirstName, LastName, Username, Email FROM Users WHERE UserID = @UserID"
                Using selectCmd As New SqlCommand(selectQuery, conn)
                    selectCmd.Parameters.AddWithValue("@UserID", selectedUserID)
                    Using reader = selectCmd.ExecuteReader
                        If reader.Read Then
                            userToDelete("FirstName") = reader("FirstName").ToString
                            userToDelete("LastName") = reader("LastName").ToString
                            userToDelete("Username") = reader("Username").ToString
                            userToDelete("Email") = reader("Email").ToString
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching user data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Confirm deletion with the user with more details
        Dim confirmMessage = $"Are you sure you want to delete the following user?" & vbCrLf & vbCrLf &
                                   $"Name: {userToDelete("FirstName")} {userToDelete("LastName")}" & vbCrLf &
                                   $"Username: {userToDelete("Username")}" & vbCrLf &
                                   $"Email: {userToDelete("Email")}"

        Dim confirmDelete = MessageBox.Show(confirmMessage, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmDelete = DialogResult.Yes Then
            ' Database operation for deleting the user
            Try
                Using conn As New SqlConnection(connectionString)
                    conn.Open()

                    ' Construct the SQL query to delete the user
                    Dim query = "DELETE FROM Users WHERE UserID = @UserID"

                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddWithValue("@UserID", selectedUserID)

                        ' Execute the delete operation
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Clear the form and reload the user data
                ClearFields()
                LoadUserData()  ' Refresh the DataGridView after deleting the user

            Catch ex As Exception
                MessageBox.Show("Error deleting user: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

    End Sub


    Private Sub btnReset_Click(sender As Object, e As EventArgs)
        ClearSelection()
    End Sub


    '============== FOR DGV ===================================
    Private Sub dgvUsers_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvUsers.CellFormatting
        ' Check if the column is Password to mask it
        If dgvUsers.Columns(e.ColumnIndex).Name = "Password" Then
            If e.Value IsNot Nothing Then
                e.Value = New String("•"c, 8) ' Mask password with bullets (fixed length for security)
            End If
        End If
    End Sub
    Private Sub dgvUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsers.CellClick
        ' Ensure that the clicked row index is valid (not the header row)
        If e.RowIndex >= 0 Then
            ' Get the row that was clicked
            Dim selectedRow As DataGridViewRow = dgvUsers.Rows(e.RowIndex)

            ' Highlight the selected row with a different color
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black

            ' Assign values to textboxes and reset their background colors
            txtFirstName.Text = selectedRow.Cells("FirstName").Value.ToString()
            txtFirstName.BackColor = Color.White

            ' Handle MiddleInitial which could be DBNull
            If selectedRow.Cells("MiddleInitial").Value IsNot DBNull.Value Then
                txtMiddleInitial.Text = selectedRow.Cells("MiddleInitial").Value.ToString()
            Else
                txtMiddleInitial.Text = String.Empty
            End If
            txtMiddleInitial.BackColor = Color.White

            txtLastName.Text = selectedRow.Cells("LastName").Value.ToString()
            txtLastName.BackColor = Color.White

            txtAge.Text = selectedRow.Cells("Age").Value.ToString()
            txtAge.BackColor = Color.White

            cbGender.SelectedItem = selectedRow.Cells("Gender").Value.ToString()
            cbRole.SelectedItem = selectedRow.Cells("Role").Value.ToString()

            txtEmailAddress.Text = selectedRow.Cells("Email").Value.ToString()
            txtEmailAddress.BackColor = Color.White

            ' Handle Address which could be DBNull
            If selectedRow.Cells("Address").Value IsNot DBNull.Value Then
                txtAddress.Text = selectedRow.Cells("Address").Value.ToString()
            Else
                txtAddress.Text = String.Empty
            End If
            txtAddress.BackColor = Color.White

            txtUsername.Text = selectedRow.Cells("Username").Value.ToString()
            txtUsername.BackColor = Color.White

            txtPhoneNumber.Text = selectedRow.Cells("PhoneNumber").Value.ToString()
            txtPhoneNumber.BackColor = Color.White

            ' Set the selected UserID for editing or deleting
            selectedUserID = Convert.ToInt32(selectedRow.Cells("UserID").Value)

            ' Load Password into the textbox
            txtPassword.Text = selectedRow.Cells("Password").Value.ToString()
            If SessionData.role = "Admin" Then
                txtPassword.BackColor = Color.White
            End If

            ' Automatically set Confirm Password to match Password
            txtConfirmPassword.Text = txtPassword.Text
            If SessionData.role = "Admin" Then
                txtConfirmPassword.BackColor = Color.White
            End If

        End If
    End Sub


    '=============== AUDIT TRAIL =======================
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

    '===============  Clear Fields ========================
    Private Sub ClearFields()
        ' Clear text boxes
        txtFirstName.Clear()
        txtMiddleInitial.Clear()
        txtLastName.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        txtAge.Clear()
        txtConfirmPassword.Clear()
        txtEmailAddress.Clear()
        txtPhoneNumber.Clear()
        txtAddress.Clear()

        ' Reset combo boxest
        cbRole.SelectedIndex = -1
        cbGender.SelectedIndex = -1

        ' Reset background colors
        txtFirstName.BackColor = Color.White
        txtLastName.BackColor = Color.White
        txtMiddleInitial.BackColor = Color.White
        txtUsername.BackColor = Color.White
        txtEmailAddress.BackColor = Color.White
        txtAge.BackColor = Color.White
        txtPhoneNumber.BackColor = Color.White
        txtAddress.BackColor = Color.White

        ' Reset password fields background color based on user role
        If SessionData.role = "Admin" Then
            txtPassword.BackColor = Color.White
            txtConfirmPassword.BackColor = Color.White
        Else
            txtPassword.BackColor = Color.LightGray
            txtConfirmPassword.BackColor = Color.LightGray
        End If

        ' Reset UserID and enable Add button
        selectedUserID = 0




        ' Set focus to first field
        txtFirstName.Focus()
    End Sub

    '=============== SEACH USER ===========================

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        LoadUserData(txtSearch.Text)
    End Sub
    Private Sub LoadUserData(searchTerm As String)
        Dim query As String = "SELECT UserID, FirstName, MiddleInitial, LastName, Username, Password, Email, Role, Gender, Age, Address, PhoneNumber " &
                              "FROM Users " &
                              "WHERE FirstName LIKE @search OR LastName LIKE @search OR Username LIKE @search"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")

                Dim adapter As New SqlDataAdapter(cmd)
                Dim table As New DataTable()

                Try
                    conn.Open()
                    adapter.Fill(table)
                    dgvUsers.DataSource = table
                Catch ex As Exception
                    MessageBox.Show("Database Error: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub



    ' txtFirstName: Only Letters and Space Allowed
    Private Sub txtFirstName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFirstName.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtLastName: Only Letters and Space Allowed
    Private Sub txtLastName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLastName.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not Char.IsControl(e.KeyChar) And Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtMiddleInitial: Only One Letter Allowed
    Private Sub txtMiddleInitial_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMiddleInitial.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If

        ' Only allow one character
        If txtMiddleInitial.Text.Length >= 255 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtUsername: Allow Only Letters and Numbers (No Special Characters)
    Private Sub txtUsername_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUsername.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtPassword: Allow All Characters
    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        ' No restrictions
    End Sub

    ' txtConfirmPassword: Allow All Characters
    Private Sub txtConfirmPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtConfirmPassword.KeyPress
        ' No restrictions
    End Sub

    ' txtEmailAddress: Allow Letters, Numbers, @, . Only
    Private Sub txtEmailAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmailAddress.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso e.KeyChar <> "@"c AndAlso e.KeyChar <> "."c AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtPhoneNumber: Digits Only, Max Length 11
    Private Sub txtPhoneNumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPhoneNumber.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If

        ' Limit to 11 digits
        If txtPhoneNumber.Text.Length >= 11 AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' txtAddress: Allow All Characters
    Private Sub txtAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAddress.KeyPress
        ' No restrictions
    End Sub

    ' txtAge: Must be Numeric and 18 or Above (on Leave)
    Private Sub txtAge_Leave(sender As Object, e As EventArgs) Handles txtAge.Leave
        If Not IsNumeric(txtAge.Text) OrElse CInt(txtAge.Text) < 18 Then
            MessageBox.Show("Please enter a valid age (18 or older).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtAge.Focus()
        End If
    End Sub





    '============ PLACE HOLDER ===============
    Private Sub SetPlaceholders()
        ' Set placeholder text for each textbox
        txtFirstName.PlaceholderText = "First Name (letters only)"
        txtLastName.PlaceholderText = "Last Name (letters only)"
        txtMiddleInitial.PlaceholderText = "Middle Initial or Full Middle Name"
        txtUsername.PlaceholderText = "Unique Username"
        txtPassword.PlaceholderText = "Strong Password"
        txtConfirmPassword.PlaceholderText = "Repeat Password"
        txtEmailAddress.PlaceholderText = "Email Address (e.g., user@example.com)"
        txtAge.PlaceholderText = "Age (18 or older)"
        txtPhoneNumber.PlaceholderText = "11-Digit Phone Number"
        txtAddress.PlaceholderText = "Complete Address"
    End Sub


    '============ BUTTONS P2 =================
    Private Sub btnAddUser_Click(sender As Object, e As EventArgs)
        UserAddPanel.Visible = True
        txtPassword.Enabled = True
        txtConfirmPassword.Enabled = True
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Close()
        txtPassword.Enabled = True
        txtConfirmPassword.Enabled = True
    End Sub
    Private Sub btnClosePanel_Click(sender As Object, e As EventArgs)
        UserAddPanel.Visible = False
        ClearFields()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs)
        ' Ask for confirmation before closing
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to close the User Management module?",
                                                    "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    '=============== PANEL ==================
    Private Sub UserAddPanel_Paint(sender As Object, e As PaintEventArgs) Handles UserAddPanel.Paint
        CenterPanel()

        ' Set background color using the hex color code F1EFEC
        UserAddPanel.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub
    Private Sub PanelCategory_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Define the corner radius
        Dim cornerRadius = 20

        ' Create a rounded rectangle path
        Dim path As New Drawing2D.GraphicsPath
        Dim rect As New Rectangle(0, 0, Panel1.Width, Panel1.Height)

        ' Add rounded corners
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' Apply rounded region to the panel
        Panel1.Region = New Region(path)

        ' Fill the background (optional)
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#3399FF"))
            g.FillPath(brush, path)
        End Using

        ' Draw the border (optional)
        Using pen As New Pen(ColorTranslator.FromHtml("#3399FF"), 2)
            g.DrawPath(pen, path)
        End Using
    End Sub

    ' Add this method to clear selection and re-enable password fields
    Private Sub ClearSelection()
        ' Clear the DataGridView selection
        dgvUsers.ClearSelection()
        selectedUserID = 0

        ' Re-enable password fields
        txtPassword.Enabled = True
        txtConfirmPassword.Enabled = True

        ' Clear the form fields
        ClearFields()
    End Sub

    Private Sub dgvUsers_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvUsers.MouseDown
        ' Get the hit test information to see where the click happened
        Dim hitTest As DataGridView.HitTestInfo = dgvUsers.HitTest(e.X, e.Y)

        ' If the click was outside any row (on empty space)
        If hitTest.RowIndex = -1 Then
            ClearSelection()
        End If
    End Sub

    Private Sub btnAddUser_Click_1(sender As Object, e As EventArgs) Handles btnAddUser.Click

    End Sub
End Class
