Imports Microsoft.Data.SqlClient

Public Class AppConfig
    ' Static fields to store connection parameters
    Private Shared _serverIP As String = "192.168.55.105"
    Private Shared _userID As String = "oreo123"
    Private Shared _password As String = "oreo12345"

    ' Properties to get/set connection parameters
    Public Shared Property ServerIP As String
        Get
            Return _serverIP
        End Get
        Set(value As String)
            _serverIP = value
        End Set
    End Property

    Public Shared Property UserID As String
        Get
            Return _userID
        End Get
        Set(value As String)
            _userID = value
        End Set
    End Property

    Public Shared Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    ' Database connection string
    Public Shared ReadOnly Property ConnectionString As String
        Get
            Return $"Data Source={_serverIP},1433;Initial Catalog=updated;User ID={_userID};Password={_password};Encrypt=True;TrustServerCertificate=True"
        End Get
    End Property

    ' Method to update connection parameters
    Public Shared Sub UpdateConnectionParameters(serverIP As String, userID As String, password As String)
        _serverIP = serverIP
        _userID = userID
        _password = password
    End Sub

    ' Method to test connection
    Public Shared Function TestConnection() As Boolean
        Try
            Using conn As New Microsoft.Data.SqlClient.SqlConnection(ConnectionString)
                conn.Open()
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ValidateConnection() As String
        Try
            Using conn As New Microsoft.Data.SqlClient.SqlConnection(ConnectionString)
                conn.Open()
                Return "Success"
            End Using
        Catch ex As SqlException
            ' Handle specific SQL exceptions
            Select Case ex.Number
                Case 53 ' Network-related or instance-specific error
                    Return "Unable to connect to the server. Please check the server IP address."
                Case 18456 ' Login failed for user
                    Return "Invalid username or password. Please check your credentials."
                Case Else
                    Return $"SQL Error: {ex.Message.Split("."c)(0)}."
            End Select
        Catch ex As Exception
            ' Handle general exceptions
            Return $"Error: {ex.Message}"
        End Try
    End Function


    ' Resources folder path
    'Public Shared ReadOnly Property ResourcesPath As String
    '    Get
    '        'Return "C:\Users\Caleb\Desktop\BACK UP\smgsisbstp2025\Resources"
    '    End Get
    'End Property
End Class 