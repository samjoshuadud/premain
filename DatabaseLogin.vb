Imports System.Data.SqlClient
Imports Org.BouncyCastle.Crypto.Engines
Public Class DatabaseLogin

    Public conbool As Boolean

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ipaddss = txtip.Text.Trim()
        username = txtuser.Text.Trim()
        pass = txtpass.Text.Trim()

        If String.IsNullOrEmpty(ipaddss) OrElse String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(pass) Then
            MsgBox("Please fill out all fields.", MsgBoxStyle.Exclamation, "Validation Error")
            Login.Label3.Text = "DISCONNECTED"
            Exit Sub
        End If

        ' Update AppConfig with new connection parameters
        AppConfig.UpdateConnectionParameters(ipaddss, username, pass)
        
        ' Test the connection
        If AppConfig.TestConnection() Then
            conBool = True
            Login.Label3.Text = "CONNECTED"
            MsgBox("Connection successful!", MsgBoxStyle.Information, "Connection Status")
            Me.Close()
        Else
            conBool = False
            Login.Label3.Text = "DISCONNECTED"
            MsgBox("Connection failed. Please check your credentials or database configuration.", MsgBoxStyle.Critical, "Connection Status")
        End If
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Me.Close()
    End Sub

    Private Sub DatabaseLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = ColorTranslator.FromHtml("#F1EFEC")
        
        ' Initialize fields with current AppConfig values
        txtip.Text = AppConfig.ServerIP
        txtuser.Text = AppConfig.UserID
        txtpass.Text = AppConfig.Password
    End Sub
End Class