Imports Microsoft.Data.SqlClient

Public Class AuditTrail
    ' Set connection string with SQL Server credentials
    Private connectionString As String = AppConfig.ConnectionString

    ' Declare the connection object
    Private conn As SqlConnection

    ' Load Audit Trail Data
    Public Sub LoadAuditTrail()
        Try
            ' Initialize the connection object
            conn = New SqlConnection(connectionString)

            ' Open connection to the database
            conn.Open()

            ' SQL Query to get the audit trail data
            Dim query As String = "SELECT AuditID, Role, FullName, Action, Form, Date FROM AuditTrail ORDER BY Date DESC"
            Dim cmd As New SqlCommand(query, conn)

            ' Use SqlDataAdapter to fill the data into a DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            Dim table As New DataTable()

            ' Fill the DataTable with the data from the database
            adapter.Fill(table)

            ' Check if there are rows in the table
            If table.Rows.Count > 0 Then
                ' Bind the DataTable to the DataGridView
                dgvAuditTrail.DataSource = table

                ' Adjust column widths
                With dgvAuditTrail
                    .Columns("AuditID").Visible = False ' Hide AuditID if not needed
                    .Columns("Role").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

                    ' Increase the width of the FullName column
                    .Columns("FullName").Width = 250 ' You can adjust this value as needed

                    ' Expand "Action" column width to fit longer descriptions
                    .Columns("Action").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns("Action").FillWeight = 350 ' Increase this value to make it take more space

                    .Columns("Form").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    .Columns("Date").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                End With
            Else
                ' Clear the DataGridView if no records are found
                dgvAuditTrail.DataSource = Nothing
                MessageBox.Show("No audit trail records found.")
            End If

            ' Close the connection
            conn.Close()

        Catch ex As Exception
            ' Show an error message if there's an issue loading the data
            MessageBox.Show("Error loading audit trail: " & ex.Message)
        End Try

        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvAuditTrail.BackgroundColor = Color.White
        dgvAuditTrail.BorderStyle = BorderStyle.None
        dgvAuditTrail.AllowUserToAddRows = False
        dgvAuditTrail.ReadOnly = True
        dgvAuditTrail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvAuditTrail.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAuditTrail.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvAuditTrail.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvAuditTrail.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvAuditTrail.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvAuditTrail.EnableHeadersVisualStyles = False
        dgvAuditTrail.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvAuditTrail.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvAuditTrail.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvAuditTrail.ColumnHeadersHeight = 35
    End Sub


    ' Form Load Event
    Private Sub AuditTrailForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form title using session data (replace with actual session data logic)
        Me.Text = "Audit Trail - " & SessionData.role & ": " & SessionData.fullName
        ' Load the audit trail data when the form is loaded
        LoadAuditTrail()
    End Sub

    Private Sub PanelCategory_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Define the corner radius
        Dim cornerRadius = 20

        ' Create a rounded rectangle path
        Dim path As New Drawing2D.GraphicsPath
        Dim rect As New Rectangle(0, 0, Panel1.Width, Panel1.Height)

        ' Add rounded corners
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' Apply rounded region to the panel
        Panel1.Region = New Region(path)

        ' Fill the background (optional)
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#3399FF"))
            g.FillPath(brush, path)
        End Using

        ' Draw the border (optional)
        Using pen As New Pen(ColorTranslator.FromHtml("#3399FF"), 2)
            g.DrawPath(pen, path)
        End Using
    End Sub
End Class
