Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Data.SqlClient

Public Class Vat
    ' Connection string for SQL Server
    Private connectionString As String = AppConfig.ConnectionString
    Private connection As SqlConnection
    Private selectedVatId As Integer = -1 ' Tracks the selected VAT ID

    ' Constructor to initialize the connection
    Public Sub New()
        ' Initialize the connection
        connection = New SqlConnection(connectionString)
        InitializeComponent() ' Make sure controls are initialized first
    End Sub

    ' Event handler for form load
    Private Sub VatMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load VAT data after form load
        LoadVATData()
    End Sub

    ' Load VAT data into the grid
    Private Sub LoadVATData()
        Try
            ' Open the connection if it's closed
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If

            ' Modify the SELECT statement to exclude effective_date and only select vat_id and vat_rate
            Dim query As String = "SELECT vatid, vatrate FROM vat"
            Using cmd As New SqlCommand(query, connection)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    ' Load data into DataTable
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    dgvVat.DataSource = dt
                End Using
            End Using

            ' Set AutoSizeMode to Fill for all columns
            For Each column As DataGridViewColumn In dgvVat.Columns
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Next

            ' Hide the vat_id column after loading the data
            If dgvVat.Columns.Contains("vatid") Then
                dgvVat.Columns("vatid").Visible = False
            End If

            ' Format the vatrate column to display as percentage
            If dgvVat.Columns.Contains("vatrate") Then
                dgvVat.Columns("vatrate").DefaultCellStyle.Format = "P2" ' Format as percentage with 2 decimal places
            End If

            ' Set the background color of the DataGridView to white
            dgvVat.BackgroundColor = Color.White

            ' Remove the blue highlight when a row is selected
            dgvVat.DefaultCellStyle.SelectionBackColor = Color.White
            dgvVat.DefaultCellStyle.SelectionForeColor = Color.Black

            ' Optional: Set the selection mode to cell selection (instead of row selection)
            dgvVat.SelectionMode = DataGridViewSelectionMode.CellSelect

            ' Remove the extra row that appears at the bottom (new row for adding data)
            dgvVat.AllowUserToAddRows = False

        Catch ex As Exception
            ' Show an error message if an exception occurs
            MessageBox.Show("Error loading VAT data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Ensure the connection is closed after the operation
            If connection.State = ConnectionState.Open Then
                connection.Close()
            End If
        End Try

        SetupDataGridView()
        AddActionColumns() ' Add action columns (Edit/Delete)
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvVat.BackgroundColor = Color.White
        dgvVat.BorderStyle = BorderStyle.None
        dgvVat.AllowUserToAddRows = False
        dgvVat.ReadOnly = True

        dgvVat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvVat.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVat.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvVat.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvVat.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvVat.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvVat.EnableHeadersVisualStyles = False
        dgvVat.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvVat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvVat.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvVat.ColumnHeadersHeight = 45
    End Sub

    ' Add action columns (Edit and Delete)
    Private Sub AddActionColumns()
        ' Remove existing if reloading
        If dgvVat.Columns.Contains("Edit") Then dgvVat.Columns.Remove("Edit")
        If dgvVat.Columns.Contains("Delete") Then dgvVat.Columns.Remove("Delete")

        ' Edit column
        Dim editColumn As New DataGridViewImageColumn()
        editColumn.Name = "Edit"
        editColumn.HeaderText = "Edit"
        editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        editColumn.Width = 30
        Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
        editColumn.Image = Image.FromFile(imagePath)
        dgvVat.Columns.Add(editColumn)

        ' Delete column
        Dim deleteColumn As New DataGridViewImageColumn()
        deleteColumn.Name = "Delete"
        deleteColumn.HeaderText = "Delete"
        deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        deleteColumn.Width = 30
        Dim deleteimagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-delete-35.png")
        deleteColumn.Image = Image.FromFile(deleteimagePath)
        dgvVat.Columns.Add(deleteColumn)
    End Sub

    ' Handle row selection in the grid
    Private Sub DgvVat_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvVat.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvVat.Rows(e.RowIndex)
            selectedVatId = Convert.ToInt32(row.Cells("vatid").Value)
            ' Show the VAT rate as a percentage in the textbox
            txtVatRate.Text = (Convert.ToDecimal(row.Cells("vatrate").Value) * 100).ToString("F2") ' Display as 12.00
        End If
    End Sub

    ' Handle image column click (Edit/Delete)
    Private Sub dgvVat_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvVat.CellContentClick
        If e.RowIndex < 0 Then Exit Sub ' Prevent clicking the header row

        Dim columnName As String = dgvVat.Columns(e.ColumnIndex).Name
        Dim row As DataGridViewRow = dgvVat.Rows(e.RowIndex)
        Dim vatId As Integer = Convert.ToInt32(row.Cells("vatid").Value)
        Dim vatRate As Decimal = Convert.ToDecimal(row.Cells("vatrate").Value)

        If columnName = "Edit" Then
            ' When Edit is clicked, set the selected VAT ID and display the VAT rate in the textbox
            selectedVatId = vatId
            txtVatRate.Text = (vatRate * 100).ToString("F2") ' Show VAT rate in textbox (convert to percentage)

            ' Show the panel when Edit button is clicked
            VatPanel.Visible = True ' Assuming pnlEditVat is the panel you want to show
        ElseIf columnName = "Delete" Then
            ' When Delete is clicked, confirm and then delete the VAT rate
            Dim confirm = MessageBox.Show("Are you sure you want to delete this VAT rate?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = DialogResult.Yes Then
                DeleteVatRate(vatId, vatRate)
            End If
        End If
    End Sub


    ' Add a new VAT rate
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' Validate the input for VAT rate
        If Not ValidateInputs() Then Exit Sub

        Try
            connection.Open()

            ' Convert the entered VAT rate (percentage) to a decimal for database storage
            Dim newVatRate As Decimal = Convert.ToDecimal(txtVatRate.Text) / 100

            ' Check if the VAT rate already exists in the database
            Dim checkQuery As String = "SELECT COUNT(*) FROM vat WHERE vatrate = @vatrate"
            Using cmd As New SqlCommand(checkQuery, connection)
                cmd.Parameters.AddWithValue("@vatrate", newVatRate)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                If count > 0 Then
                    ' VAT rate already exists
                    MessageBox.Show("This VAT rate already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End Using

            ' Insert the new VAT rate into the database
            Dim insertQuery As String = "INSERT INTO vat (vatrate) VALUES (@vatrate)"
            Using cmd As New SqlCommand(insertQuery, connection)
                cmd.Parameters.AddWithValue("@vatrate", newVatRate)
                cmd.ExecuteNonQuery()
            End Using

            ' Log the changes for the audit trail
            Dim changes As String = $"Added new VAT rate: {txtVatRate.Text}%"
            LogAuditTrail(SessionData.role, SessionData.fullName, changes)

            MessageBox.Show("VAT rate added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadVATData()

        Catch ex As Exception
            MessageBox.Show("Error adding VAT rate: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    ' Delete VAT rate method
    Private Sub DeleteVatRate(vatId As Integer, vatRate As Decimal)
        Try
            connection.Open()

            ' Delete the selected VAT rate
            Dim deleteQuery As String = "DELETE FROM vat WHERE vatid = @vatid"
            Using cmd As New SqlCommand(deleteQuery, connection)
                cmd.Parameters.AddWithValue("@vatid", vatId)
                cmd.ExecuteNonQuery()
            End Using

            ' Log the audit
            Dim actionDescription As String = $"Deleted VAT rate with ID: {vatId}, which had a rate of {vatRate * 100}%."
            LogAuditTrail(SessionData.role, SessionData.fullName, actionDescription)

            MessageBox.Show("VAT rate deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadVATData()

        Catch ex As Exception
            MessageBox.Show("Error deleting VAT rate: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
    End Sub

    ' Validate user inputs
    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtVatRate.Text) OrElse Not Decimal.TryParse(txtVatRate.Text, Nothing) Then
            MessageBox.Show("Please enter a valid VAT rate.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim vatRate As Decimal = Convert.ToDecimal(txtVatRate.Text)
        If vatRate < 0 OrElse vatRate > 100 Then
            MessageBox.Show("VAT rate must be between 0 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    ' Log Audit Trail (for logging actions like VAT deletion)
    Private Sub LogAuditTrail(ByVal role As String, ByVal fullName As String, ByVal action As String)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO audittrail (Role, FullName, Action, Form, Date) VALUES (@Role, @FullName, @Action, @Form, @Date)"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@Role", role)
                    cmd.Parameters.AddWithValue("@FullName", fullName)
                    cmd.Parameters.AddWithValue("@Action", action)
                    cmd.Parameters.AddWithValue("@Form", "VAT Form") ' Adjusted form name to VAT Form
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
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



    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        VatPanel.Visible = True

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        VatPanel.Visible = False

    End Sub

    Private Sub VatPanel_Paint(sender As Object, e As PaintEventArgs) Handles VatPanel.Paint
        VatPanel.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub
End Class
