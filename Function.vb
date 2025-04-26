Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Common
Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Org.BouncyCastle.Asn1.Misc
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Engines
Imports Org.BouncyCastle.Tls

Module DatabaseFunctions

    Private connection As SqlConnection
    Public ipaddss As String
    Public username As String
    Public pass As String
    Public status As String
    Public conBool As Boolean

    ' Assume you are storing this from config or assigning at runtime
    Private connectionString As String = AppConfig.ConnectionString

    Public Sub initializeConnection()
        Dim connect As String = Login.Label3.Text
        Dim disconnect As String = Login.Label3.Text
        Try
            ' Use the values from ipaddss, username, and pass module variables if they're set
            ' Otherwise fall back to AppConfig values
            Dim serverIP As String = If(String.IsNullOrEmpty(ipaddss), AppConfig.ServerIP, ipaddss)
            Dim userID As String = If(String.IsNullOrEmpty(username), AppConfig.UserID, username)
            Dim password As String = If(String.IsNullOrEmpty(pass), AppConfig.Password, pass)
            
            If Not String.IsNullOrEmpty(serverIP) AndAlso Not String.IsNullOrEmpty(userID) AndAlso Not String.IsNullOrEmpty(password) Then
                ' Use the AppConfig connection string format with our variables
                Dim connString As String = $"Data Source={serverIP},1433;Initial Catalog=updated;User ID={userID};Password={password};Encrypt=True;TrustServerCertificate=True"
                connection = New SqlConnection(connString)

                Try
                    connection.Open()
                    If connection.State = ConnectionState.Open Then
                        status = "CONNECTED"
                        Login.Label3.Text = status
                        conBool = True
                        Login.Label3.Text = connect
                    Else
                        status = "DISCONNECTED"
                        conBool = False
                        Login.Label3.Text = status
                        Login.Label3.Text = disconnect
                    End If
                Catch ex As Exception
                    status = "DISCONNECTED"
                    conBool = False
                    Login.Label3.Text = status
                    Login.Label3.Text = disconnect
                End Try
            Else
                status = "DISCONNECTED"
                conBool = False
                Login.Label3.Text = status
                Login.Label3.Text = disconnect
            End If
        Catch ex As Exception
            ' Optionally log exception
            status = "DISCONNECTED"
            Login.Label3.Text = status
            conBool = False
        End Try
    End Sub

    Public Sub closeConnection()
        If connection IsNot Nothing AndAlso connection.State = ConnectionState.Open Then
            connection.Close()
            Console.WriteLine("Connection closed.")
        End If
    End Sub

End Module
