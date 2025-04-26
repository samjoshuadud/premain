Imports Microsoft.Data.SqlClient

Public Class LogHistory
    Private connectionString As String = AppConfig.ConnectionString
    Private connection As SqlConnection

    Public Sub New()
        InitializeComponent()
        connection = New SqlConnection(connectionString)
    End Sub

    ' Load Log History Data
    Private Sub LoadLogHistory()
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim query As String = "SELECT LogID, Role, FullName, Action, Date FROM loghistory ORDER BY Date DESC"
                Dim adapter As New SqlDataAdapter(query, connection)
                Dim table As New DataTable
                adapter.Fill(table)
                dgvLogHistory.DataSource = table

                ' Hide the "LogID" column
                dgvLogHistory.Columns("LogID").Visible = False

                ' Set the "FullName" column to fill available space
                dgvLogHistory.Columns("FullName").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                ' Adjust the width of the "Action" column and set it to fill the available space
                dgvLogHistory.Columns("Action").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

                ' Set other columns to adjust automatically to the size of their contents
                dgvLogHistory.Columns("Role").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                dgvLogHistory.Columns("Date").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading log history: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvLogHistory.BackgroundColor = Color.White
        dgvLogHistory.BorderStyle = BorderStyle.None
        dgvLogHistory.AllowUserToAddRows = False
        dgvLogHistory.ReadOnly = True
        dgvLogHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvLogHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvLogHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvLogHistory.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvLogHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvLogHistory.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvLogHistory.EnableHeadersVisualStyles = False
        dgvLogHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvLogHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvLogHistory.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvLogHistory.ColumnHeadersHeight = 35
    End Sub



    ' Form Load Event
    Private Sub LogHistoryForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadLogHistory()
    End Sub
End Class
