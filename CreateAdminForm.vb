Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient

Public Class CreateAdminForm
    ' SQL Server connection string
    Private connectionString As String = AppConfig.ConnectionString

    ' Form Load Event
    Private Sub CreateAdminForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form title and some styling
        Me.Text = "Create Administrator Account"
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MaximizeBox = False
        Me.MinimizeBox = False
    End Sub

    ' Event handler for Save button
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Validate fields
        If String.IsNullOrWhiteSpace(txtUsername.Text) OrElse
           String.IsNullOrWhiteSpace(txtPassword.Text) OrElse
           String.IsNullOrWhiteSpace(txtFullName.Text) OrElse
           String.IsNullOrWhiteSpace(txtEmail.Text) Then
            MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validate password confirmation
        If txtPassword.Text <> txtConfirmPassword.Text Then
            MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Parse full name into first and last name
            Dim nameParts As String() = txtFullName.Text.Trim().Split(" "c)
            Dim firstName As String = ""
            Dim middleInitial As String = ""
            Dim lastName As String = ""

            ' Handle different name formats
            If nameParts.Length = 1 Then
                ' Only first name provided
                firstName = nameParts(0)
                lastName = "[No Last Name]" ' Default value
            ElseIf nameParts.Length = 2 Then
                ' First and last name provided
                firstName = nameParts(0)
                lastName = nameParts(1)
            ElseIf nameParts.Length > 2 Then
                ' First name, middle name/initial, and last name provided
                firstName = nameParts(0)
                middleInitial = nameParts(1)
                ' Combine remaining parts as last name (handles multiple last names)
                lastName = String.Join(" ", nameParts.Skip(2))
            End If
            
            ' Create the admin user with separate name parts
            CreateAdminUser(txtUsername.Text.Trim(), txtPassword.Text.Trim(), 
                           firstName, middleInitial, lastName, txtEmail.Text.Trim())
                           
            MessageBox.Show("Administrator account created successfully. You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error creating administrator account: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Method to create admin user in the database
    Private Sub CreateAdminUser(username As String, password As String, firstName As String, 
                               middleInitial As String, lastName As String, email As String)
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            
            ' SQL query to insert a new admin user with all required fields
            ' Note: FullName is a computed column, so we don't need to include it in the INSERT statement
            Dim query As String = "INSERT INTO Users (Username, Password, FirstName, MiddleInitial, LastName, 
                                  Email, Role, Gender, Age, Address, PhoneNumber, DateCreated) 
                                  VALUES (@Username, @Password, @FirstName, @MiddleInitial, @LastName, 
                                  @Email, 'Admin', 'Male', 25, 'Admin Address', '00000000000', GETDATE())"
            
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Password", password) ' In a real app, you'd hash the password
                cmd.Parameters.AddWithValue("@FirstName", firstName)
                cmd.Parameters.AddWithValue("@MiddleInitial", If(String.IsNullOrWhiteSpace(middleInitial), DBNull.Value, middleInitial))
                cmd.Parameters.AddWithValue("@LastName", lastName)
                cmd.Parameters.AddWithValue("@Email", email)
                
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' Event handler for Cancel button
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' Ask for confirmation since this is critical - the system needs an admin
        Dim result = MessageBox.Show("Are you sure you want to cancel? The system requires at least one administrator account.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        
        If result = DialogResult.Yes Then
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub
End Class 