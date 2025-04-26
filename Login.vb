Imports System.Data.SqlClient
Imports System.Drawing.Drawing2D
Imports Microsoft.Data.SqlClient
Imports Microsoft.Identity.Client
Imports System.IO



Public Class Login
    ' SQL Server connection string
    Private connectionString As String = AppConfig.ConnectionString

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



    ' Form Load Event for login form
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear session data when login form is loaded
        SessionData.CurrentUserId = 0
        SessionData.role = String.Empty
        SessionData.fullName = String.Empty

        ' Set the password field to hide text by default
        txtPassword.PasswordChar = "*" ' Hide the password characters
        ' Optionally, you can set the toggle button's default text or icon here.
        btnTogglePassword.Text = "Show Password" ' Or set an icon if needed
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



    Private Sub Label3_Click_1(sender As Object, e As EventArgs) Handles Label3.Click
        If DatabaseLogin.Visible Then
            DatabaseLogin.Hide()
        Else
            DatabaseLogin.Show()
        End If
    End Sub
    ' Event handler for Toggle Password Visibility



End Class
