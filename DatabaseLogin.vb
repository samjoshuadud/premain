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

        initializeConnection()

        If conBool Then
            MsgBox("Connection successful!", MsgBoxStyle.Information, "Connection Status")
            Me.Hide()
        Else
            MsgBox("Connection failed. Please check your credentials or database configuration.", MsgBoxStyle.Critical, "Connection Status")
        End If
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Me.Close()
    End Sub

    Private Sub DatabaseLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = ColorTranslator.FromHtml("#F1EFEC")

    End Sub
End Class