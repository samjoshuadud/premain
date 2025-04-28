Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient

Public Class Discount
    Private connectionString As String = AppConfig.ConnectionString
    Private connection As SqlConnection
    Private selectedDiscountId As Integer = -1 ' Tracks the selected Discount ID

    Public Sub New()
        InitializeComponent()

        connection = New SqlConnection(connectionString)

        ' Load Discount data into the grid
        LoadDiscountData()

    End Sub

    ' Load Discount data into the grid
    Private Sub LoadDiscountData()
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If

            ' Include DiscountID in the query
            Dim query As String = "SELECT DiscountID, DiscountName, DiscountRate FROM Discounts"
            Dim cmd As New SqlCommand(query, connection)
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            ' Load data into DataGridView
            Dim dt As New DataTable()
            dt.Load(reader)
            dgvDiscounts.DataSource = dt

            ' Optionally hide the DiscountID column (if not needed for display)
            If dgvDiscounts.Columns.Contains("DiscountID") Then
                dgvDiscounts.Columns("DiscountID").Visible = False
            End If

            ' Resize DiscountName column to fit its content
            dgvDiscounts.Columns("DiscountName").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            ' Optional: Resize the other columns based on their content
            dgvDiscounts.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

            ' Format the DiscountRate column to display as percentage
            If dgvDiscounts.Columns.Contains("DiscountRate") Then
                ' Set a specific DefaultCellStyle for this column
                dgvDiscounts.Columns("DiscountRate").DefaultCellStyle.Format = "0.##\\%"
            End If

            reader.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading Discount data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If connection.State = ConnectionState.Open Then
                connection.Close()
            End If
        End Try

        SetupDataGridView()

        AddActionColumns()
    End Sub


    ' Check if the Discount Name already exists in the database
    Private Function IsDiscountNameExists(discountName As String) As Boolean
        Try
            connection.Open()
            Dim query As String = "SELECT COUNT(*) FROM discounts WHERE discountname = @name"
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@name", discountName)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            Return count > 0 ' If count is greater than 0, it means the discount name exists
        Catch ex As Exception
            MessageBox.Show("Error checking for duplicate Discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            connection.Close()
        End Try
    End Function


    ' Your existing BtnAdd_Click, BtnEdit_Click, and BtnDelete_Click methods remain the same
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs)
        ' Validate input fields
        If Not ValidateInputs() Then Exit Sub

        ' Check if the Discount Name already exists in the database
        If IsDiscountNameExists(txtDiscountName.Text) Then
            MessageBox.Show("A Discount with this name already exists. Please choose a different name.", "Duplicate Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            connection.Open()
            ' Prepare the query and remove % from the rate before saving to the database
            Dim query As String = "INSERT INTO discounts (discountname, discountrate) VALUES (@name, @rate)"
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@name", txtDiscountName.Text)

            ' Remove % before converting to a decimal
            Dim discountRate As Decimal = Convert.ToDecimal(txtDiscountRate.Text.Replace("%", "").Trim())
            cmd.Parameters.AddWithValue("@rate", discountRate)
            cmd.ExecuteNonQuery()

            ' Log the action in the audit trail
            Dim currentFullName As String = SessionData.fullName  ' Replace with actual method to get full name
            Dim currentRole As String = SessionData.role  ' Replace with actual method to get role
            Logaudittrail(currentRole, currentFullName, "Added a new discount: " & txtDiscountName.Text.Trim())

            MessageBox.Show("Discount added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadDiscountData()
        Catch ex As Exception
            MessageBox.Show("Error adding Discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
        txtDiscountName.Clear()
        txtDiscountRate.Clear()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs)
        If selectedDiscountId = -1 Then
            MessageBox.Show("Please select a Discount to edit.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not ValidateInputs() Then Exit Sub

        ' Declare original values before the edit
        Dim originalDiscountName As String = ""
        Dim originalDiscountRate As Decimal = 0

        Try
            ' Fetch the original values from the database before editing
            connection.Open()
            Dim query As String = "SELECT discountname, discountrate FROM discounts WHERE discountid = @id"
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@id", selectedDiscountId)
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                originalDiscountName = reader("discountname").ToString()
                originalDiscountRate = Convert.ToDecimal(reader("discountrate"))
            End If

            reader.Close()

            ' Prepare the update query and execute the update
            query = "UPDATE discounts SET discountname = @name, discountrate = @rate WHERE discountid = @id"
            cmd = New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@name", txtDiscountName.Text)
            cmd.Parameters.AddWithValue("@rate", Convert.ToDecimal(txtDiscountRate.Text.Replace("%", "").Trim()))
            cmd.Parameters.AddWithValue("@id", selectedDiscountId)
            cmd.ExecuteNonQuery()

            ' Build the change log message by comparing the original and updated values
            Dim changes As String = "Edited the discount: " & originalDiscountName & vbCrLf

            ' Compare the original and updated values
            If originalDiscountName <> txtDiscountName.Text Then
                changes &= $"Discount name changed from '{originalDiscountName}' to '{txtDiscountName.Text}'" & vbCrLf
            End If
            If originalDiscountRate <> Convert.ToDecimal(txtDiscountRate.Text.Replace("%", "").Trim()) Then
                changes &= $"Discount rate changed from '{originalDiscountRate}%' to '{txtDiscountRate.Text}'" & vbCrLf
            End If

            ' Log the audit trail with the change log
            Dim currentFullName As String = SessionData.fullName  ' Replace with actual method to get full name
            Dim currentRole As String = SessionData.role  ' Replace with actual method to get role
            Logaudittrail(currentRole, currentFullName, changes)

            MessageBox.Show("Discount updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadDiscountData()

        Catch ex As Exception
            MessageBox.Show("Error updating Discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
        txtDiscountName.Clear()
        txtDiscountRate.Clear()
    End Sub


    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        If selectedDiscountId = -1 Then
            MessageBox.Show("Please select a Discount to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Log the discount deletion
        Dim currentFullName As String = SessionData.fullName  ' Replace with actual method to get full name
        Dim currentRole As String = SessionData.role  ' Replace with actual method to get role
        Logaudittrail(currentRole, currentFullName, "Deleted discount: " & txtDiscountName.Text)

        Try
            connection.Open()

            ' Correct the column name to DiscountID (not DiscountDD)
            Dim query As String = "DELETE FROM Discounts WHERE DiscountID = @id"
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@id", selectedDiscountId)

            ' Execute the query to delete the selected discount
            cmd.ExecuteNonQuery()

            MessageBox.Show("Discount deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Reload the data to reflect the deletion
            LoadDiscountData()
        Catch ex As Exception
            MessageBox.Show("Error deleting Discount: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            connection.Close()
        End Try
        txtDiscountName.Clear()
        txtDiscountRate.Clear()
    End Sub


    ' Handle DataGridView Cell Click event
    Private Sub DgvDiscounts_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvDiscounts.Rows(e.RowIndex)

            ' Make sure DiscountID is available
            If dgvDiscounts.Columns.Contains("DiscountID") Then
                selectedDiscountId = Convert.ToInt32(row.Cells("DiscountID").Value)
            Else
                selectedDiscountId = -1
                MessageBox.Show("The 'DiscountID' column is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Populate the textboxes with selected row data
            txtDiscountName.Text = row.Cells("discountname").Value.ToString()

            ' Format Discount Rate as percentage if available
            If row.Cells("discountrate").Value IsNot DBNull.Value Then
                Dim discountRate As Decimal = Convert.ToDecimal(row.Cells("discountrate").Value)
                txtDiscountRate.Text = discountRate.ToString("0.##") & "%" ' Format as percentage with % symbol
            Else
                txtDiscountRate.Clear()
            End If
        End If
    End Sub



    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvDiscounts.BackgroundColor = Color.White
        dgvDiscounts.BorderStyle = BorderStyle.None
        dgvDiscounts.AllowUserToAddRows = False
        dgvDiscounts.ReadOnly = True

        dgvDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvDiscounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDiscounts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvDiscounts.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvDiscounts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvDiscounts.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvDiscounts.EnableHeadersVisualStyles = False
        dgvDiscounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvDiscounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDiscounts.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvDiscounts.ColumnHeadersHeight = 45
    End Sub

    ' Reset the form inputs
    Private Sub BtnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtDiscountName.Clear()
        txtDiscountRate.Clear()
        selectedDiscountId = -1
    End Sub

    ' Close the form
    'Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
    '    Me.Close()
    'End Sub

    ' Validate user inputs
    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtDiscountName.Text) Then
            MessageBox.Show("Please enter a valid Discount name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' Remove % from the input before validation
        Dim discountRateText As String = txtDiscountRate.Text.Replace("%", "").Trim()

        If String.IsNullOrWhiteSpace(discountRateText) OrElse Not Decimal.TryParse(discountRateText, Nothing) Then
            MessageBox.Show("Please enter a valid Discount rate percentage.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim discountRate As Decimal = Convert.ToDecimal(discountRateText)
        If discountRate < 0 OrElse discountRate > 100 Then
            MessageBox.Show("Discount rate must be between 0% and 100%.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    Private Sub DiscountMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDiscountData()

        ' Add the event handlers manually
        AddHandler BtnAdd.Click, AddressOf BtnAdd_Click
        AddHandler btnEdit.Click, AddressOf BtnEdit_Click
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        AddHandler dgvDiscounts.CellClick, AddressOf DgvDiscounts_CellClick

        ' Set DataGridView background color to white
        dgvDiscounts.BackgroundColor = Color.White

        ' Remove the extra row (the row for adding new data)
        dgvDiscounts.AllowUserToAddRows = False

        ' Disable the blue highlight when a row is selected
        dgvDiscounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDiscounts.DefaultCellStyle.SelectionBackColor = dgvDiscounts.BackgroundColor
        dgvDiscounts.DefaultCellStyle.SelectionForeColor = dgvDiscounts.ForeColor

        CenterPanel()

        If SessionData.role.Equals("Staff", StringComparison.OrdinalIgnoreCase) Then
            BtnAdd.Enabled = False
            btnEdit.Enabled = False
            btnDelete.Enabled = False
        End If



    End Sub

    Private Sub AddActionColumns()
        ' Remove existing if reloading
        If dgvDiscounts.Columns.Contains("Edit") Then dgvDiscounts.Columns.Remove("Edit")
        If dgvDiscounts.Columns.Contains("Delete") Then dgvDiscounts.Columns.Remove("Delete")

        ' Edit column
        Dim editColumn As New DataGridViewImageColumn()
        editColumn.Name = "Edit"
        editColumn.HeaderText = "Edit"
        editColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        editColumn.Width = 30
        Dim imagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-edit-34.png")
        editColumn.Image = Image.FromFile(imagePath)
        dgvDiscounts.Columns.Add(editColumn)

        ' Delete column
        Dim deleteColumn As New DataGridViewImageColumn()
        deleteColumn.Name = "Delete"
        deleteColumn.HeaderText = "Delete"
        deleteColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        deleteColumn.Width = 30
        Dim deleteimagePath As String = System.IO.Path.Combine(Application.StartupPath, "Resources\icons8-delete-35.png")
        deleteColumn.Image = Image.FromFile(deleteimagePath)

        dgvDiscounts.Columns.Add(deleteColumn)
    End Sub

    Private Sub Logaudittrail(ByVal role As String, ByVal fullName As String, ByVal action As String)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "INSERT INTO audittrail (Role, FullName, Action, Form, Date) VALUES (@Role, @FullName, @action, @Form, @Date)"
                Using cmd As New SqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@Role", role)
                    cmd.Parameters.AddWithValue("@FullName", fullName)
                    cmd.Parameters.AddWithValue("@action", action)
                    cmd.Parameters.AddWithValue("@Form", "Discount Form") ' Change to specific form name
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging audit trail: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CenterPanel()
        Dim formWidth As Integer = Me.ClientSize.Width
        Dim formHeight As Integer = Me.ClientSize.Height

        Dim panelWidth As Integer = DiscountPanel.Width
        Dim panelHeight As Integer = DiscountPanel.Height

        Dim x As Integer = (formWidth - panelWidth) \ 2
        Dim y As Integer = (formHeight - panelHeight) \ 2

        DiscountPanel.Location = New Point(x, y)
    End Sub
    Private Sub DiscountPanel_Paint(sender As Object, e As PaintEventArgs) Handles DiscountPanel.Paint
        CenterPanel()
        DiscountPanel.BackColor = ColorTranslator.FromHtml("#F1EFEC")
    End Sub

    Private Sub dgvDiscounts_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDiscounts.CellContentClick
        ' Exit if the click is on the header row (RowIndex < 0)
        If e.RowIndex < 0 Then Exit Sub

        ' Get the clicked column name (e.ColumnIndex is the clicked column's index)
        Dim columnName As String = dgvDiscounts.Columns(e.ColumnIndex).Name
        ' Get the selected row in the DataGridView
        Dim row As DataGridViewRow = dgvDiscounts.Rows(e.RowIndex)

        ' Get values from the row (You can adjust these based on the columns in your DataGridView)
        Dim discountId As Integer = Convert.ToInt32(row.Cells("discountId").Value)
        Dim discountName As String = row.Cells("discountName").Value.ToString()

        ' Handle DiscountRate which might already be formatted as a string with %
        Dim discountRateText As String = row.Cells("discountRate").Value.ToString()
        Dim discountRate As Decimal

        ' Remove % if present before converting
        If discountRateText.Contains("%") Then
            discountRate = Convert.ToDecimal(discountRateText.Replace("%", ""))
        Else
            discountRate = Convert.ToDecimal(discountRateText)
        End If

        ' Check if the clicked column is the Edit button
        If columnName = "Edit" Then
            ' Populate the textboxes or other controls with the selected row's data
            selectedDiscountId = discountId ' Store the selected Discount ID
            txtDiscountName.Text = discountName ' Populate the Discount Name textbox
            txtDiscountRate.Text = discountRate.ToString("F2") & "%" ' Populate the Discount Rate textbox

            ' Show the panel (if you want to show the panel for editing)
            DiscountPanel.Visible = True ' Assuming DiscountPanel is your panel for editing
        End If

        ' Check if the clicked column is the Delete button
        If columnName = "Delete" Then
            ' Ask for confirmation before deleting
            Dim confirm As DialogResult = MessageBox.Show("Are you sure you want to delete this discount?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            ' If confirmed, call the DeleteDiscount method (or any method you use to delete the discount)
            If confirm = DialogResult.Yes Then
                DeleteDiscount(discountId) ' Call the method to delete the discount
            End If
        End If
    End Sub

    Private Sub DeleteDiscount(discountId As Integer)
        ' SQL query to delete the discount from the database
        Dim query As String = "DELETE FROM Discounts WHERE discountId = @discountId"

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@discountId", discountId)

                Try
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Discount deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' Optionally, refresh the DataGridView or reload data
                    LoadDiscountData() ' Assuming you have a method to reload the DataGridView data
                Catch ex As Exception
                    MessageBox.Show("Error deleting discount: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub


    Private Sub txtDiscountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountName.KeyPress
        ' Allow only alphabetic characters and space
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True ' Cancel the key press
        End If
    End Sub
    Private Sub txtDiscountRate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiscountRate.KeyPress
        ' Allow only numeric characters and the decimal point
        If Not (Char.IsDigit(e.KeyChar) OrElse e.KeyChar = "."c OrElse Char.IsControl(e.KeyChar)) Then
            e.Handled = True ' Cancel the key press
        End If

        ' Allow only one decimal point
        If e.KeyChar = "."c AndAlso txtDiscountRate.Text.Contains("."c) Then
            e.Handled = True ' Cancel the key press if there is already a decimal point
        End If
    End Sub

    Private Sub txtDiscountRate_Leave(sender As Object, e As EventArgs) Handles txtDiscountRate.Leave
        ' If there's a value and it doesn't already end with %, add it
        If Not String.IsNullOrWhiteSpace(txtDiscountRate.Text) AndAlso Not txtDiscountRate.Text.EndsWith("%") Then
            ' Remove any existing % symbol
            Dim cleanValue = txtDiscountRate.Text.Replace("%", "").Trim()

            ' Try to parse as decimal
            Dim rate As Decimal
            If Decimal.TryParse(cleanValue, rate) Then
                ' Format with % symbol
                txtDiscountRate.Text = rate.ToString("0.##") & "%"
            End If
        End If
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



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DiscountPanel.Visible = True
    End Sub

    Private Sub btnCLose_Click(sender As Object, e As EventArgs) Handles btnCLose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DiscountPanel.Visible = False
    End Sub

    Private Sub dgvDiscounts_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDiscounts.CellFormatting
        ' Format DiscountRate column to show as percentage
        If e.ColumnIndex >= 0 AndAlso dgvDiscounts.Columns(e.ColumnIndex).Name = "DiscountRate" AndAlso e.Value IsNot Nothing AndAlso Not TypeOf e.Value Is String Then
            Try
                ' Check if the value is already a string with %
                If Not (TypeOf e.Value Is String AndAlso e.Value.ToString().Contains("%")) Then
                    Dim rate As Decimal = Convert.ToDecimal(e.Value)
                    e.Value = rate.ToString("0.##") & "%"
                    e.FormattingApplied = True
                End If
            Catch ex As Exception
                ' If there's any conversion error, do nothing and leave the value as is
            End Try
        End If
    End Sub
End Class
