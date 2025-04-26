Imports Microsoft.Data.SqlClient

Public Class UserCancel
    Private transactionNumber As String
    Private productID As Integer
    Private cancelQuantity As Integer
    Private cancellationReason As String
    Private addToInventory As Boolean
    Private loggedInUserFullName As String
    Private cancelledBy As String
    Private connectionString As String = AppConfig.ConnectionString

    ' Constructor to initialize cancellation parameters
    Public Sub New(transactionNumber As String, productID As Integer, cancelQuantity As Integer, reasons As String, addToInventory As Boolean, cancelledBy As String)
        InitializeComponent()
        Me.transactionNumber = transactionNumber
        Me.productID = productID
        Me.cancelQuantity = cancelQuantity
        Me.cancellationReason = reasons
        Me.addToInventory = addToInventory
        Me.cancelledBy = cancelledBy
    End Sub

    ' Authenticate the user
    Private Function AuthenticateUser(username As String, password As String) As Boolean
        Dim query As String = "SELECT FullName FROM Users WHERE Username = @Username AND Password = @Password"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Username", username)
                command.Parameters.AddWithValue("@Password", password)

                Try
                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()

                    If result IsNot Nothing Then
                        loggedInUserFullName = result.ToString()
                        Return True
                    Else
                        MessageBox.Show("Invalid username or password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error authenticating user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            End Using
        End Using
    End Function

    ' Insert cancellation record into OrderCancellation table
    Private Sub InsertCancellationRecord()
        Dim insertQuery As String = "INSERT INTO OrderCancellation (TransactionNumber, ProductID, CancelledBy, VoidBy, CancelQuantity, AddToInventory, Reasons, CancelDate) " &
                                    "VALUES (@TransactionNumber, @ProductID, @CancelledBy, @VoidBy, @CancelQuantity, @AddToInventory, @Reasons, @CancelDate)"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@TransactionNumber", transactionNumber)
                command.Parameters.AddWithValue("@ProductID", productID)
                command.Parameters.AddWithValue("@CancelledBy", cancelledBy)
                command.Parameters.AddWithValue("@VoidBy", loggedInUserFullName)
                command.Parameters.AddWithValue("@CancelQuantity", cancelQuantity)
                command.Parameters.AddWithValue("@AddToInventory", If(addToInventory, "Yes", "No"))
                command.Parameters.AddWithValue("@Reasons", cancellationReason)
                command.Parameters.AddWithValue("@CancelDate", DateTime.Now)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                Catch ex As Exception
                    MessageBox.Show("Error inserting cancellation record: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    ' Void the transaction in the TodaySales table and adjust inventory based on AddToInventory
    Private Sub UpdateTodaySales()
        Dim updateQuery As String

        If addToInventory Then
            ' Add the canceled quantity back to the inventory if AddToInventory is Yes
            updateQuery = "UPDATE TodaySales SET Quantity = Quantity - @CancelQuantity WHERE TransactionNumber = @TransactionNumber AND ProductID = @ProductID"

            ' Update Inventory if AddToInventory is True
            Dim inventoryUpdateQuery As String = "UPDATE Inventory SET QuantityInStock = QuantityInStock + @CancelQuantity WHERE ProductID = @ProductID"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(updateQuery, connection)
                    command.Parameters.AddWithValue("@CancelQuantity", cancelQuantity)
                    command.Parameters.AddWithValue("@TransactionNumber", transactionNumber)
                    command.Parameters.AddWithValue("@ProductID", productID)

                    Try
                        connection.Open()

                        ' Update TodaySales (decrease quantity for the canceled sale)
                        command.ExecuteNonQuery()

                        ' Add to Inventory if AddToInventory is True
                        Dim inventoryCommand As New SqlCommand(inventoryUpdateQuery, connection)
                        inventoryCommand.Parameters.AddWithValue("@CancelQuantity", cancelQuantity)
                        inventoryCommand.Parameters.AddWithValue("@ProductID", productID)

                        inventoryCommand.ExecuteNonQuery()

                        MessageBox.Show("Inventory updated successfully and transaction quantity adjusted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("Error during transaction update: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            End Using
        Else
            ' Just reduce the quantity in TodaySales without updating the inventory if AddToInventory is No
            updateQuery = "UPDATE TodaySales SET Quantity = Quantity - @CancelQuantity WHERE TransactionNumber = @TransactionNumber AND ProductID = @ProductID"

            Using connection As New SqlConnection(connectionString)
                Using command As New SqlCommand(updateQuery, connection)
                    command.Parameters.AddWithValue("@CancelQuantity", cancelQuantity)
                    command.Parameters.AddWithValue("@TransactionNumber", transactionNumber)
                    command.Parameters.AddWithValue("@ProductID", productID)

                    Try
                        connection.Open()
                        command.ExecuteNonQuery()

                        MessageBox.Show("Transaction quantity adjusted successfully without affecting inventory.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("Error during transaction update: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Using
            End Using
        End If
    End Sub


    ' Button click event for void operation
    Private Sub btnVoid_Click(sender As Object, e As EventArgs) Handles btnVoid.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both username and password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If AuthenticateUser(username, password) Then
            InsertCancellationRecord()
            UpdateTodaySales()  ' Update the TodaySales table based on AddToInventory flag
            Me.Close()
        Else
            MessageBox.Show("Invalid credentials. Please try again.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


    Private Sub UserCancel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the PasswordChar property to mask the input
        txtPassword.PasswordChar = "*" ' This will display asterisks (****) for each character entered
    End Sub


    Private Sub btnCLose_MouseEnter(sender As Object, e As EventArgs) Handles btnCLose.MouseEnter
        btnCLose.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub

    ' Optional: Revert color when mouse leaves button
    Private Sub btnCLose_MouseLeave(sender As Object, e As EventArgs) Handles btnCLose.MouseLeave
        btnCLose.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click

        Me.Close() ' Close the form or whatever functionality you need
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ' Set the background color of Panel1
        Panel1.BackColor = ColorTranslator.FromHtml("#251F1F")
    End Sub
End Class
