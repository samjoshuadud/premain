Imports Microsoft.Data.SqlClient

Public Class DeliverySaveProduct
    Private transactionNumber As String
    Private connectionString As String = AppConfig.ConnectionString

    ' Method to load delivery items into DataGridView
    Private Sub LoadDeliveryItems()
        ' SQL query to get all delivery items
        Dim query As String = "SELECT * FROM dbo.deliveryItems" ' Ensure proper schema reference

        ' Create a new connection object using the connection string
        Using connection As New SqlConnection(connectionString)
            ' Open the connection
            connection.Open()

            ' Create a new command object with the query and connection
            Using command As New SqlCommand(query, connection)
                ' Create a DataTable to hold the data
                Dim dataTable As New DataTable()

                ' Use a data adapter to fill the DataTable
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(dataTable)
                End Using

                ' Set the DataGridView's DataSource to the DataTable
                dgvItems.DataSource = dataTable

                ' Hide the ID column if it exists
                If dgvItems.Columns.Contains("deliveryitemid") Then
                    dgvItems.Columns("deliveryitemid").Visible = False
                    dgvItems.Columns("deliveryid").Visible = False
                End If
            End Using
        End Using
    End Sub


    ' Form Load event handler
    Private Sub DeliverySaveProduct_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Call the LoadDeliveryItems method to populate the DataGridView
        LoadDeliveryItems()

        ' Ensure that the DataGridView is in full row select mode
        dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    ' DataGridView SelectionChanged event to enable/disable Void button
    Private Sub dgvItems_SelectionChanged(sender As Object, e As EventArgs) Handles dgvItems.SelectionChanged
        ' Enable the Void button only if a row is selected
        btnVoid.Enabled = dgvItems.SelectedRows.Count > 0
    End Sub

    ' Void Button Click Event
    Private Sub btnVoid_Click(sender As Object, e As EventArgs) Handles btnVoid.Click
        ' Check if a delivery item is selected in the DataGridView
        If dgvItems.SelectedRows.Count > 0 Then
            ' Get the delivery_item_id of the selected row
            Dim deliveryItemId As Integer = Convert.ToInt32(dgvItems.SelectedRows(0).Cells("deliveryitemid").Value)
            Dim transactionNumber As String = dgvItems.SelectedRows(0).Cells("transactionNumber").Value.ToString()

            ' Confirm the void action with the user
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to void this delivery item?", "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

            If result = DialogResult.Yes Then
                ' SQL query to update the voided status of the selected delivery item
                Dim query As String = "UPDATE dbo.deliveryItems SET voided = 'Voided' WHERE deliveryitemid = @deliveryItemId"

                ' Create a new connection object and execute the query
                Using connection As New SqlConnection(connectionString)
                    Using command As New SqlCommand(query, connection)
                        ' Add the delivery_item_id parameter to the SQL query
                        command.Parameters.AddWithValue("@deliveryItemId", deliveryItemId)

                        ' Open the connection
                        connection.Open()

                        ' Execute the query to update the status
                        command.ExecuteNonQuery()
                    End Using
                End Using

                ' Check if all items in the delivery have been voided and update the Deliveries table
                UpdateDeliveryStatus(transactionNumber)

                ' Reload the delivery items in the DataGridView to reflect the changes
                LoadDeliveryItems()

                ' Notify the user that the delivery item has been voided
                MessageBox.Show("Delivery item has been voided successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            ' Show a message if no row is selected
            MessageBox.Show("Please select a delivery item to void.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Check if all items in the delivery have been voided and update the Deliveries table
    Private Sub UpdateDeliveryStatus(transactionNumber As String)
        ' Query to check if all items in the delivery are voided
        Dim checkQuery As String = "SELECT COUNT(*) FROM dbo.deliveryItems WHERE transactionNumber = @transactionNumber AND voided <> 'Voided'"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(checkQuery, connection)
                ' Add the transaction number parameter
                command.Parameters.AddWithValue("@transactionNumber", transactionNumber)

                connection.Open()

                ' Execute the query and check if all items are voided
                Dim remainingItems As Integer = Convert.ToInt32(command.ExecuteScalar())

                ' If no remaining items, update the delivery status to 'Voided'
                If remainingItems = 0 Then
                    Dim updateQuery As String = "UPDATE dbo.Deliveries SET Voided = 'Voided' WHERE TransactionNumber = @transactionNumber"

                    Using updateCommand As New SqlCommand(updateQuery, connection)
                        updateCommand.Parameters.AddWithValue("@transactionNumber", transactionNumber)
                        updateCommand.ExecuteNonQuery()
                    End Using
                End If
            End Using
        End Using
    End Sub

End Class
