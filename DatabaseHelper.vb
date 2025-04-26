Imports Microsoft.Data.SqlClient
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO


' Session Data Module
Module SessionData
    ' Stores the ID of the currently logged-in user
    Public CurrentUserId As Integer

    ' Stores the role of the currently logged-in user (e.g., "Cashier", "Admin")
    Public role As String

    ' Stores the full name of the logged-in user
    Public fullName As String
End Module

' User Entity (Domain Layer)
<Serializable()>
Public Class User
    <NonSerialized()>
    Private _userId As Integer
    <NonSerialized()>
    Private _username As String
    <NonSerialized()>
    Private _fullName As String
    <NonSerialized()>
    Private _email As String
    <NonSerialized()>
    Private _passwordHash As String
    <NonSerialized()>
    Private _role As String

    <System.Xml.Serialization.XmlAttribute()>
    <System.Runtime.Serialization.DataMemberAttribute()>
    Public Property UserId As Integer
        Get
            Return _userId
        End Get
        Set(value As Integer)
            _userId = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttribute()>
    <System.Runtime.Serialization.DataMemberAttribute()>
    Public Property Username As String
        Get
            Return _username
        End Get
        Set(value As String)
            _username = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttribute()>
    <System.Runtime.Serialization.DataMemberAttribute()>
    Public Property FullName As String
        Get
            Return _fullName
        End Get
        Set(value As String)
            _fullName = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttribute()>
    <System.Runtime.Serialization.DataMemberAttribute()>
    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttribute()>
    <System.Runtime.Serialization.DataMemberAttribute()>
    Public Property PasswordHash As String
        Get
            Return _passwordHash
        End Get
        Set(value As String)
            _passwordHash = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttribute()>
    <System.Runtime.Serialization.DataMemberAttribute()>
    Public Property Role As String
        Get
            Return _role
        End Get
        Set(value As String)
            _role = value
        End Set
    End Property
End Class

' User Repository (Data Access Layer)
Public Class UserRepository
    Private ReadOnly _dbHelper As DatabaseHelper = DatabaseHelper.Instance

    ' Retrieve all users from the database
    Public Function GetAllUsers() As List(Of User)
        Dim users As New List(Of User)
        Dim query As String = "SELECT * FROM users"
        Dim dataTable As DataTable = _dbHelper.ExecuteQuery(query)

        For Each row As DataRow In dataTable.Rows
            users.Add(New User With {
                .UserId = Convert.ToInt32(row("userid")),
                .Username = row("username").ToString(),
                .FullName = row("fullname").ToString(),
                .Email = row("email").ToString(),
                .PasswordHash = row("passwordhash").ToString(),
                .Role = row("role").ToString()
            })
        Next

        Return users
    End Function

    ' Add a new user to the database
    Public Sub AddUser(user As User)
        ' SQL query for inserting a new user
        Dim query As String = "INSERT INTO Users (Username, FullName, Email, Password, Role) VALUES (@Username, @FullName, @Email, @Password, @Role)"

        ' Parameters for the SQL query
        Dim parameters As SqlParameter() = {
        New SqlParameter("@Username", SqlDbType.VarChar) With {.Value = user.Username},
        New SqlParameter("@FullName", SqlDbType.VarChar) With {.Value = user.FullName},
        New SqlParameter("@Email", SqlDbType.VarChar) With {.Value = user.Email},
        New SqlParameter("@Password", SqlDbType.VarChar) With {.Value = user.PasswordHash},
        New SqlParameter("@Role", SqlDbType.VarChar) With {.Value = user.Role}
    }

        ' Execute the query using your database helper
        _dbHelper.ExecuteNonQuery(query, parameters)
    End Sub


    ' Update an existing user in the database
    Public Sub UpdateUser(user As User)
        Dim query As String = "UPDATE Users SET Username = @Username, FullName = @FullName, Email = @Email, Password = @Password, Role = @Role WHERE UserID = @UserID"
        Dim parameters As SqlParameter() = {
        New SqlParameter("@Username", SqlDbType.VarChar) With {.Value = user.Username},
        New SqlParameter("@FullName", SqlDbType.VarChar) With {.Value = user.FullName},
        New SqlParameter("@Email", SqlDbType.VarChar) With {.Value = user.Email},
        New SqlParameter("@Password", SqlDbType.VarChar) With {.Value = user.PasswordHash},
        New SqlParameter("@Role", SqlDbType.VarChar) With {.Value = user.Role},
        New SqlParameter("@UserID", SqlDbType.Int) With {.Value = user.UserId}
    }
        _dbHelper.ExecuteNonQuery(query, parameters)
    End Sub


    ' Delete a user from the database
    Public Sub DeleteUser(userId As Integer)
        Dim query As String = "DELETE FROM Users WHERE UserID = @UserID"
        Dim parameters As SqlParameter() = {
            New SqlParameter("@UserID", SqlDbType.Int) With {.Value = userId}
        }
        _dbHelper.ExecuteNonQuery(query, parameters)
    End Sub
End Class

' User Service (Application Layer)
Public Class UserService
    Private ReadOnly _userRepository As New UserRepository()

    ' Retrieve all users
    Public Function GetAllUsers() As List(Of User)
        Return _userRepository.GetAllUsers()
    End Function

    ' Add a new user
    Public Sub AddUser(user As User)
        If String.IsNullOrWhiteSpace(user.Username) OrElse String.IsNullOrWhiteSpace(user.FullName) Then
            Throw New ArgumentException("Username and Full Name cannot be empty.")
        End If
        _userRepository.AddUser(user)
    End Sub

    ' Update an existing user
    Public Sub UpdateUser(user As User)
        If String.IsNullOrWhiteSpace(user.Username) OrElse String.IsNullOrWhiteSpace(user.FullName) Then
            Throw New ArgumentException("Username and Full Name cannot be empty.")
        End If
        _userRepository.UpdateUser(user)
    End Sub

    ' Delete a user by ID
    Public Sub DeleteUser(userId As Integer)
        _userRepository.DeleteUser(userId)
    End Sub
End Class

' Database Helper Class (Singleton for Database Access)
Public Class DatabaseHelper
    ' Singleton instance
    Private Shared _instance As DatabaseHelper
    Public Shared ReadOnly Property Instance As DatabaseHelper
        Get
            If _instance Is Nothing Then
                _instance = New DatabaseHelper()
            End If
            Return _instance
        End Get
    End Property

    ' Private constructor to enforce singleton pattern
    Private Sub New()
        connection = New SqlConnection(connectionString)
    End Sub

    ' Connection string (adjust for your database)
    ' Private connectionString As String = "Data Source=LAPTOP-HC3L03IC\SQLEXPRESS;Initial Catalog=testt;Integrated Security=True;Trust Server Certificate=True"
    ' Private connectionString As String = "Data Source=LAPTOP-R9O43L9I\SQLEXPRESS;Initial Catalog=smgsisbstp;Integrated Security=True;Trust Server Certificate=True"
    Private connectionString As String = AppConfig.ConnectionString

    Private connection As SqlConnection
    Private transaction As SqlTransaction


    ' Open the database connection
    Private Sub OpenConnection()
        If connection.State = ConnectionState.Closed Then
            connection.Open()
        End If
    End Sub

    ' Close the database connection
    Private Sub CloseConnection()
        If connection.State = ConnectionState.Open AndAlso transaction Is Nothing Then
            connection.Close()
        End If
    End Sub

    ' Log errors to a file
    Private Sub LogError(message As String)
        Dim logFile As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log")
        File.AppendAllText(logFile, $"{DateTime.Now}: {message}{Environment.NewLine}")
    End Sub

    ' Execute scalar queries (e.g., SELECT single value)
    Public Function ExecuteScalar(query As String, Optional parameters As SqlParameter() = Nothing) As Object
        Dim result As Object = Nothing
        Using command As New SqlCommand(query, connection)
            If parameters IsNot Nothing Then
                command.Parameters.AddRange(parameters)
            End If
            If transaction IsNot Nothing Then
                command.Transaction = transaction
            End If
            Try
                OpenConnection()
                result = command.ExecuteScalar()
            Catch ex As Exception
                LogError(ex.Message)
                Throw
            Finally
                CloseConnection()
            End Try
        End Using
        Return result
    End Function

    ' Execute non-query commands (INSERT, UPDATE, DELETE)
    Public Function ExecuteNonQuery(query As String, Optional parameters As SqlParameter() = Nothing) As Integer
        Dim affectedRows As Integer = 0
        Using command As New SqlCommand(query, connection)
            If parameters IsNot Nothing Then
                command.Parameters.AddRange(parameters)
            End If
            If transaction IsNot Nothing Then
                command.Transaction = transaction
            End If
            Try
                OpenConnection()
                affectedRows = command.ExecuteNonQuery()
            Catch ex As Exception
                LogError(ex.Message)
                Throw
            Finally
                CloseConnection()
            End Try
        End Using
        Return affectedRows
    End Function

    ' Execute queries and return a DataTable (SELECT queries)
    Public Function ExecuteQuery(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dataTable As New DataTable()
        Using command As New SqlCommand(query, connection)
            If parameters IsNot Nothing Then
                command.Parameters.AddRange(parameters)
            End If
            If transaction IsNot Nothing Then
                command.Transaction = transaction
            End If
            Try
                OpenConnection()
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(dataTable)
                End Using
            Catch ex As Exception
                LogError(ex.Message)
                Throw
            Finally
                CloseConnection()
            End Try
        End Using
        Return dataTable
    End Function

    ' Transaction management
    Public Sub BeginTransaction()
        Try
            OpenConnection()
            transaction = connection.BeginTransaction()
        Catch ex As Exception
            LogError("Failed to start transaction: " & ex.Message)
            Throw
        End Try
    End Sub

    Public Sub CommitTransaction()
        Try
            transaction?.Commit()
        Catch ex As Exception
            LogError("Failed to commit transaction: " & ex.Message)
            Throw
        Finally
            transaction = Nothing
            CloseConnection()
        End Try
    End Sub

    Public Sub RollbackTransaction()
        Try
            transaction?.Rollback()
        Catch ex As Exception
            LogError("Failed to rollback transaction: " & ex.Message)
            Throw
        Finally
            transaction = Nothing
            CloseConnection()
        End Try
    End Sub

    ' Get the last inserted ID
    Public Function GetLastInsertId() As Integer
        Return Convert.ToInt32(ExecuteScalar("SELECT LAST_INSERT_ID()"))
    End Function

    ' Get a DataTable from a query
    Public Function GetDataTable(query As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                Using cmd As New SqlCommand(query, conn)
                    ' Add parameters if any
                    If parameters IsNot Nothing Then
                        cmd.Parameters.AddRange(parameters)
                    End If

                    ' Use a DataReader to fill the DataTable
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return dt
    End Function



End Class


