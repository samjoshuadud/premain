Imports Microsoft.Data.SqlClient

Public Class ChangePassword
    ' Connection string for the database (replace AppConfig.ConnectionString with your actual connection string)
    Private connectionString As String = AppConfig.ConnectionString
    Private connection As SqlConnection

    ' Example: The currently logged-in username (you can replace this with the actual logged-in user)
    Dim loggedInUser As String = "adminUser"  ' This should come from your authentication system

    ' Event handler for when the Change Password button is clicked
    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        ' Get the username, old password, and new password from the form fields
        Dim username As String = txtUsername.Text
        Dim oldPassword As String = txtOldPassword.Text
        Dim newPassword As String = txtNewPassword.Text

        ' Check if any field is empty
        If String.IsNullOrWhiteSpace(username) Or String.IsNullOrWhiteSpace(oldPassword) Or String.IsNullOrWhiteSpace(newPassword) Then
            MessageBox.Show("Please fill in all fields.")
            Return
        End If

        ' Step 1: Check if the old password is correct
        If Not IsOldPasswordCorrect(username, oldPassword) Then
            MessageBox.Show("The old password is incorrect.")
            Return
        End If

        ' Step 2: If old password is correct, update it with the new password
        If UpdatePassword(username, newPassword) Then
            MessageBox.Show("Password has been successfully updated.")
        Else
            MessageBox.Show("Failed to update the password.")
        End If
    End Sub

    ' Function to check if the old password is correct for the given username
    Private Function IsOldPasswordCorrect(username As String, oldPassword As String) As Boolean
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' SQL query to check if the old password matches the username
                Dim query As String = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @OldPassword"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@Username", username)
                    command.Parameters.AddWithValue("@OldPassword", oldPassword)

                    Dim result As Integer = Convert.ToInt32(command.ExecuteScalar())

                    ' If result is 1, it means the password is correct
                    If result = 1 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return False
        End Try
    End Function

    ' Function to update the password to the new one
    Private Function UpdatePassword(username As String, newPassword As String) As Boolean
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                ' SQL query to update the password
                Dim query As String = "UPDATE Users SET Password = @NewPassword WHERE Username = @Username"
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@NewPassword", newPassword)  ' The new password
                    command.Parameters.AddWithValue("@Username", username)  ' The username whose password is being updated

                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show("Password updated successfully.")
                        Return True
                    Else
                        MessageBox.Show("User not found or password not updated.")
                        Return False
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub btnClosePanel_Click(sender As Object, e As EventArgs) Handles btnClosePanel.Click
        Me.Close()
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

    Private Sub ChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = ColorTranslator.FromHtml("#F1EFEC") ' Or set it to your desired color again

    End Sub
End Class
