Imports Microsoft.Data.SqlClient

Public Class DbHelperClass
    Private connectionString As String = AppConfig.ConnectionString

    ' Method to execute queries and return a DataTable
    Public Function ExecuteQuery(query As String, parameters As SqlParameter()) As DataTable
        Dim result As New DataTable()

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    ' Add parameters to the command to prevent SQL injection
                    cmd.Parameters.AddRange(parameters)

                    ' Open connection and execute query
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        result.Load(reader)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return result
    End Function
End Class