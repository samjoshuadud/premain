Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports Microsoft.Data.SqlClient
Imports Microsoft.Identity.Client
Imports System.IO

Public Class Login
    ' SQL Server connection string
    Private connectionString As String = AppConfig.ConnectionString
    
    ' Reference to the DatabaseLogin form
    Private dbLoginForm As DatabaseLogin

    ' Form Load Event for login form
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize database connection form
        dbLoginForm = New DatabaseLogin()
        
        ' Clear session data when login form is loaded
        SessionData.CurrentUserId = 0
        SessionData.role = String.Empty
        SessionData.fullName = String.Empty

        ' Set the password field to hide text by default
        txtPassword.PasswordChar = "*" ' Hide the password characters
        ' Optionally, you can set the toggle button's default text or icon here.
        btnTogglePassword.Text = "Show Password" ' Or set an icon if needed
        
        ' Check if there's an admin user in the database
        CheckForAdminUser()
    End Sub

    ' Method to check if any admin users exist in the database
    Private Sub CheckForAdminUser()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                
                ' First check if the Users table exists
                If TableExists("Users") Then
                    ' Query to check if any Admin users exist
                    Dim query As String = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'"
                    
                    Using cmd As New SqlCommand(query, conn)
                        Dim adminCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                        
                        ' If no admins exist, show the admin creation form
                        If adminCount = 0 Then
                            ShowCreateAdminForm()
                        End If
                    End Using
                Else
                    ' Users table doesn't exist, need to initialize database
                    Dim result = MessageBox.Show("The Users table does not exist in the database. Would you like to initialize the database and create an admin user?", 
                                                "Database Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    
                    If result = DialogResult.Yes Then
                        ' Create the Users table
                        CreateUsersTable()
                        ' Then show the admin creation form
                        ShowCreateAdminForm()
                    Else
                        MessageBox.Show("The application requires a properly set up database. The application will now close.", 
                                        "Application Closing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Application.Exit()
                    End If
                End If
            End Using
        Catch ex As SqlException
            ' Handle specific SQL exceptions
            If ex.Number = 4060 Then ' Database does not exist
                MessageBox.Show("The database does not exist. Please set up the database first.", 
                               "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                
                ' Show the database connection form to allow setting up a new connection
                dbLoginForm.ShowDialog()
                
                ' Check again after the user has set up the connection
                If AppConfig.TestConnection() Then
                    CheckForAdminUser() ' Recursive call to check again
                Else
                    Application.Exit()
                End If
            Else
                ' Other SQL exceptions
                MessageBox.Show("Database error: " & ex.Message & vbCrLf & "Please check your database connection.", 
                               "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                dbLoginForm.ShowDialog() ' Show connection form
            End If
        Catch ex As Exception
            ' If there's a general error, inform the user
            MessageBox.Show("Error checking for administrator accounts: " & ex.Message & 
                           vbCrLf & "Please check your database connection.", 
                           "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    
    ' Helper method to show the admin creation form
    Private Sub ShowCreateAdminForm()
        MessageBox.Show("No administrator accounts found in the system. You need to create an administrator account to continue.", 
                       "Admin Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        
        ' Show the admin creation form
        Dim createAdminForm As New CreateAdminForm()
        Dim result = createAdminForm.ShowDialog()
        
        ' If user cancelled without creating an admin, exit the application
        If result = DialogResult.Cancel Then
            MessageBox.Show("The system requires at least one administrator account. The application will now close.", 
                           "Application Closing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Application.Exit()
        End If
    End Sub
    
    ' Method to check if a table exists in the database
    Private Function TableExists(tableName As String) As Boolean
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            
            ' Query to check if table exists
            Dim query As String = "SELECT CASE WHEN EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName) THEN 1 ELSE 0 END"
            
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@TableName", tableName)
                Return Convert.ToBoolean(cmd.ExecuteScalar())
            End Using
        End Using
    End Function
    
    ' Method to create the Users table if it doesn't exist
    Private Sub CreateUsersTable()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                
                ' SQL query to create the Users table
                Dim query As String = "
                CREATE TABLE Users (
                    UserID INT IDENTITY(1,1) PRIMARY KEY,
                    Username NVARCHAR(50) NOT NULL UNIQUE,
                    Password NVARCHAR(100) NOT NULL,
                    FullName NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100),
                    Role NVARCHAR(20) NOT NULL,
                    FirstName NVARCHAR(50),
                    MiddleInitial NVARCHAR(10),
                    LastName NVARCHAR(50),
                    Gender NVARCHAR(10),
                    Age INT,
                    Address NVARCHAR(200),
                    PhoneNumber NVARCHAR(20)
                )"
                
                Using cmd As New SqlCommand(query, conn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Users table created successfully.", "Database Setup", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
                
                ' Create log history table if needed
                If Not TableExists("loghistory") Then
                    Dim logQuery As String = "
                    CREATE TABLE loghistory (
                        ID INT IDENTITY(1,1) PRIMARY KEY,
                        Role NVARCHAR(20),
                        FullName NVARCHAR(100),
                        Action NVARCHAR(200),
                        Date DATETIME DEFAULT GETDATE()
                    )"
                    
                    Using cmd As New SqlCommand(logQuery, conn)
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Log history table created successfully.", "Database Setup", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error creating database tables: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Get username and password, trim to avoid leading/trailing spaces
        Dim username = txtUsername.Text.Trim
        Dim password = txtPassword.Text.Trim

        ' Check for empty input fields
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            ' Open connection to SQL Server
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' SQL query to validate user with plain-text password and get role
                Dim query = "SELECT UserID, FullName, Role FROM Users WHERE Username = @Username AND Password = @Password"
                Using cmd As New SqlCommand(query, conn)
                    ' Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@Password", password)

                    ' Execute the query and check if we found a valid user
                    Using reader = cmd.ExecuteReader
                        If reader.Read Then
                            ' Retrieve user information
                            Dim userId As Integer = reader("UserID")
                            Dim fullName = reader("FullName").ToString
                            Dim role = reader("Role").ToString

                            ' Store user info in SessionData
                            CurrentUserId = userId
                            SessionData.fullName = fullName
                            SessionData.role = role  ' Store the role in SessionData

                            ' Log the login action in the loghistory table
                            LogHistoryEntry(role, fullName, "User Logged In")

                            ' Show the main form to all users
                            Dim mainForm As New Main(fullName)
                            Hide()
                            mainForm.ShowDialog()
                            Close()
                        Else
                            ' Invalid credentials
                            MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As SqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error during login: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LogHistoryEntry(ByVal Role As String, ByVal FullName As String, ByVal Action As String)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim cmd As New SqlCommand("INSERT INTO loghistory (Role, FullName, Action, Date) VALUES (@Role, @FullName, @Action, GETDATE())", connection)
                cmd.Parameters.AddWithValue("@Role", Role)
                cmd.Parameters.AddWithValue("@FullName", FullName)
                cmd.Parameters.AddWithValue("@Action", Action)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging action: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Event handler for Toggle Password Visibility
    Private Sub btnTogglePassword_Click(sender As Object, e As EventArgs) Handles btnTogglePassword.Click
        ' Check if the password is currently hidden
        If txtPassword.PasswordChar = "*" Then
            ' Show the password
            txtPassword.PasswordChar = Char.MinValue
            ' Optionally, change the button text or icon to "hide"
            btnTogglePassword.Text = "Hide"
        Else
            ' Hide the password
            txtPassword.PasswordChar = "*"
            ' Optionally, change the button text or icon to "show"
            btnTogglePassword.Text = "Show"
        End If
    End Sub

    Private Function ValidateCredentials(username As String, password As String) As Boolean
        Dim isValid As Boolean = False

        ' Create the SQL query to check if the username and password match
        Dim query As String = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                ' Adding parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@Username", username)
                cmd.Parameters.AddWithValue("@Password", password) ' In a real application, hash the password before storing it

                Try
                    conn.Open()
                    Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' If result is greater than 0, it means the username and password match
                    If result > 0 Then
                        isValid = True
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while connecting to the database: " & ex.Message)
                End Try
            End Using
        End Using

        Return isValid
    End Function

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint
        ' Change the background color of the Panel during the paint event
        Panel2.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub

    ' Update the Label3_Click method to show the DatabaseLogin form
    Private Sub Label3_Click_1(sender As Object, e As EventArgs) Handles Label3.Click
        dbLoginForm.ShowDialog() ' Show the DatabaseLogin form as a dialog
        ' After dialog closes, update the connection string
        connectionString = AppConfig.ConnectionString
    End Sub
End Class
