Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class DeliveryList
    ' Connection string to the SQL Server database
    Private connectionString As String = AppConfig.ConnectionString

    ' Variables for printing
    Private WithEvents printDocument As New PrintDocument()
    Private printPreviewDialog As New PrintPreviewDialog()

    ' Variables to track pagination for printing
    Private currentPage As Integer
    Private rowsPrinted As Integer

    ' Constructor - add PDF and Print buttons
    Public Sub New()
        ' This call is required by the designer
        InitializeComponent()

        ' Add Print and Export buttons to the form
        AddPrintExportButtons()

        ' Configure print preview dialog
        printPreviewDialog.Document = printDocument
    End Sub

    ' Add Print and Export buttons
    Private Sub AddPrintExportButtons()
        ' Create Export to PDF button
        Dim btnExportPDF As New Button()
        btnExportPDF.Text = "Export to PDF"
        btnExportPDF.BackColor = Color.FromArgb(0, 102, 204)
        btnExportPDF.ForeColor = Color.White
        btnExportPDF.FlatStyle = FlatStyle.Flat
        btnExportPDF.Size = New Size(155, 42)
        btnExportPDF.Location = New Point(300, 30) ' Adjust position as needed
        AddHandler btnExportPDF.Click, AddressOf BtnExportPDF_Click
        Me.Controls.Add(btnExportPDF)

        ' Create Print button
        Dim btnPrint As New Button()
        btnPrint.Text = "Print Report"
        btnPrint.BackColor = Color.FromArgb(34, 139, 34)
        btnPrint.ForeColor = Color.White
        btnPrint.FlatStyle = FlatStyle.Flat
        btnPrint.Size = New Size(155, 42)
        btnPrint.Location = New Point(465, 30) ' Adjust position as needed
        AddHandler btnPrint.Click, AddressOf BtnPrint_Click
        Me.Controls.Add(btnPrint)
    End Sub

    ' Load event for the form
    Private Sub DeliveryList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Call the method to load data into the DataGridView
        LoadDeliveryList()
        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvDeliveryList.BackgroundColor = Color.White
        dgvDeliveryList.BorderStyle = BorderStyle.None
        dgvDeliveryList.AllowUserToAddRows = False
        dgvDeliveryList.ReadOnly = True
        dgvDeliveryList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvDeliveryList.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDeliveryList.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvDeliveryList.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvDeliveryList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvDeliveryList.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvDeliveryList.EnableHeadersVisualStyles = False
        dgvDeliveryList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvDeliveryList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        'dgvDeliveryList.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvDeliveryList.ColumnHeadersHeight = 35
    End Sub

    ' Method to load data from the database into the DataGridView
    Private Sub LoadDeliveryList()
        ' SQL query to fetch the delivery list
        Dim query As String = "SELECT * FROM Deliveries"  ' Replace "Deliveries" with your actual table name

        ' Create a connection to the database
        Using connection As New SqlConnection(connectionString)
            ' Create a data adapter to fetch the data
            Dim adapter As New SqlDataAdapter(query, connection)
            ' Create a DataTable to hold the fetched data
            Dim deliveryData As New DataTable()

            ' Try to fill the DataTable with data
            Try
                ' Open the connection and fill the DataTable with the data
                connection.Open()
                adapter.Fill(deliveryData)

                ' Bind the data to the DataGridView
                dgvDeliveryList.DataSource = deliveryData

                ' Hide the 'ProductID', 'SupplierID', and 'DeliveryID' columns
                ' Assuming your column names are 'ProductID', 'SupplierID', and 'DeliveryID'
                dgvDeliveryList.Columns("ProductID").Visible = False
                dgvDeliveryList.Columns("SupplierID").Visible = False
                dgvDeliveryList.Columns("DeliveryID").Visible = False

                ' Set DataGridView appearance
                dgvDeliveryList.BackgroundColor = Color.White
                dgvDeliveryList.AllowUserToAddRows = False
                dgvDeliveryList.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                dgvDeliveryList.DefaultCellStyle.SelectionBackColor = dgvDeliveryList.BackgroundColor
                dgvDeliveryList.DefaultCellStyle.SelectionForeColor = dgvDeliveryList.ForeColor

                ' Format columns for better appearance
                FormatDataGridViewColumns()

            Catch ex As Exception
                ' If there's an error, display a message
                MessageBox.Show("Error loading data: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Format DataGridView columns
    Private Sub FormatDataGridViewColumns()
        ' Check if DataGridView has columns
        If dgvDeliveryList.Columns.Count > 0 Then
            ' Format date columns
            For Each column As DataGridViewColumn In dgvDeliveryList.Columns
                If column.Name.Contains("Date") Then
                    column.DefaultCellStyle.Format = "MM/dd/yyyy"
                End If
            Next

            ' Adjust column widths based on content
            dgvDeliveryList.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        End If
    End Sub

    ' Event handler for PDF export button
    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = CType(dgvDeliveryList.DataSource, DataTable)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MessageBox.Show("No data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Create a SaveFileDialog to get the file save location
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
        saveDialog.Title = "Save Delivery List Report as PDF"
        saveDialog.FileName = $"DeliveryList_{DateTime.Now:yyyyMMdd}.pdf"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Try
                ExportToPDF(saveDialog.FileName, dt)
                MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error exporting to PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Method to export the data to PDF
    Private Sub ExportToPDF(filePath As String, dataTable As DataTable)
        ' Create a new PDF document with reduced margins (from 40 to 25)
        Dim document As New Document(PageSize.A4.Rotate(), 25, 25, 25, 25) ' Reduced margins

        Try
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))

            ' Open the document for writing
            document.Open()

            ' Add a title with smaller font (from 16 to 14)
            Dim titleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim title As New Paragraph("FCM SUPERMARKET", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            document.Add(title)

            ' Add report title with smaller font (from 14 to 12)
            Dim reportTitleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim reportTitle As New Paragraph("Delivery List Report", reportTitleFont)
            reportTitle.Alignment = Element.ALIGN_CENTER
            document.Add(reportTitle)

            ' Add date with smaller font (from 12 to 10)
            Dim dateFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim dateText As New Paragraph($"Report Date: {DateTime.Now.ToString("MMMM dd, yyyy")}", dateFont)
            dateText.Alignment = Element.ALIGN_CENTER
            dateText.SpacingAfter = 15 ' Reduced spacing after (from 20 to 15)
            document.Add(dateText)

            ' Get visible columns - exclude hidden columns
            Dim columns As New List(Of String)
            For Each column As DataColumn In dataTable.Columns
                If column.ColumnName <> "ProductID" AndAlso
                   column.ColumnName <> "SupplierID" AndAlso
                   column.ColumnName <> "DeliveryID" Then
                    columns.Add(column.ColumnName)
                End If
            Next

            ' Create a table
            Dim table As New PdfPTable(columns.Count)
            table.WidthPercentage = 100 ' Use full width of the page
            table.SpacingBefore = 5 ' Reduced spacing (from 10 to 5)

            ' Set relative widths based on content - adjust as needed
            Dim widths(columns.Count - 1) As Single
            For i As Integer = 0 To columns.Count - 1
                ' Default width ratio
                widths(i) = 1.0F

                ' Adjust based on column type
                If columns(i).Contains("Date") Then
                    widths(i) = 1.2F
                ElseIf columns(i).Contains("Quantity") Then
                    widths(i) = 0.8F
                ElseIf columns(i).Contains("Name") Or columns(i).Contains("Description") Then
                    widths(i) = 2.0F
                End If
            Next
            table.SetWidths(widths)

            ' Add table headers with smaller font size (from 10 to 9)
            Dim headerFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9, iTextSharp.text.Font.NORMAL, BaseColor.White)
            Dim cellBackColor As New BaseColor(0, 0, 0) ' Black background for header

            For Each columnName As String In columns
                Dim cell As New PdfPCell(New Phrase(FormatColumnHeader(columnName), headerFont))
                cell.BackgroundColor = cellBackColor
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Padding = 3 ' Reduced padding (from 5 to 3)
                table.AddCell(cell)
            Next

            ' Add table data with smaller font (from 9 to 8)
            Dim cellFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim altCellBackColor As New BaseColor(240, 240, 240) ' Light gray for alternating rows

            For i As Integer = 0 To dataTable.Rows.Count - 1
                Dim row As DataRow = dataTable.Rows(i)
                Dim backColor As BaseColor = If(i Mod 2 = 0, BaseColor.White, altCellBackColor)

                For j As Integer = 0 To columns.Count - 1
                    Dim columnName As String = columns(j)
                    Dim cellValue As String = FormatCellValue(row, columnName)

                    Dim cell As New PdfPCell(New Phrase(cellValue, cellFont))
                    cell.BackgroundColor = backColor

                    ' Set appropriate alignment based on column type
                    If IsNumericColumn(columnName) Then
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    ElseIf columnName.Contains("Date") Then
                        cell.HorizontalAlignment = Element.ALIGN_CENTER
                    Else
                        cell.HorizontalAlignment = Element.ALIGN_LEFT
                    End If

                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    cell.Padding = 3 ' Reduced padding (from 5 to 3)
                    cell.MinimumHeight = 18 ' Set minimum height to ensure proper spacing
                    table.AddCell(cell)
                Next
            Next

            ' Add the table to the document
            document.Add(table)

            ' Add footer with generation date (smaller font, from 8 to 7)
            Dim footer As New Paragraph($"Report generated on {DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss")}",
                                       New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 7, iTextSharp.text.Font.ITALIC, BaseColor.Gray))
            footer.Alignment = Element.ALIGN_CENTER
            footer.SpacingBefore = 20 ' Reduced spacing (from 30 to 20)
            document.Add(footer)
        Catch ex As Exception
            ' In case of error, ensure document is closed
            document.Close()
            Throw ' Re-throw the exception to be caught by the caller
        Finally
            ' Close the document
            If document.IsOpen() Then
                document.Close()
            End If
        End Try
    End Sub

    ' Print button click event
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = CType(dgvDeliveryList.DataSource, DataTable)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MessageBox.Show("No data to print.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Reset pagination variables
        currentPage = 0
        rowsPrinted = 0

        ' Set landscape orientation
        printDocument.DefaultPageSettings.Landscape = True

        ' Configure print preview dialog
        printPreviewDialog.StartPosition = FormStartPosition.CenterScreen
        printPreviewDialog.WindowState = FormWindowState.Maximized

        ' Show print preview dialog
        Try
            printPreviewDialog.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error showing print preview: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Print document page print event handler
    Private Sub printDocument_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDocument.PrintPage
        Dim dt As DataTable = CType(dgvDeliveryList.DataSource, DataTable)

        ' Page title and header with smaller fonts
        Dim titleFont As New System.Drawing.Font("Arial", 14, FontStyle.Bold) ' Reduced from 16 to 14
        Dim headerFont As New System.Drawing.Font("Arial", 9, FontStyle.Bold) ' Reduced from 11 to 9
        Dim normalFont As New System.Drawing.Font("Arial", 8) ' Reduced from 9 to 8
        Dim smallFont As New System.Drawing.Font("Arial", 7) ' Reduced from 8 to 7

        ' Print margins and positions - optimize for landscape with reduced margins
        Dim leftMargin As Single = e.MarginBounds.Left + 10 ' Reduced from 20 to 10
        Dim rightMargin As Single = e.MarginBounds.Right - 10 ' Reduced from 20 to 10
        Dim topMargin As Single = e.MarginBounds.Top + 5 ' Reduced from 10 to 5
        Dim width As Single = rightMargin - leftMargin
        Dim currentY As Single = topMargin

        ' Print store name and report title
        Dim titleText As String = "SHIENNA'S MINI GROCERY STORE"
        Dim titleSize As SizeF = e.Graphics.MeasureString(titleText, titleFont)
        e.Graphics.DrawString(titleText, titleFont, Brushes.Black,
                           leftMargin + (width / 2) - (titleSize.Width / 2), currentY)
        currentY += 25 ' Reduced from 30 to 25

        Dim reportText As String = "Delivery List Report"
        Dim reportSize As SizeF = e.Graphics.MeasureString(reportText, headerFont)
        e.Graphics.DrawString(reportText, headerFont, Brushes.Black,
                           leftMargin + (width / 2) - (reportSize.Width / 2), currentY)
        currentY += 20 ' Reduced from 25 to 20

        ' Print report date
        Dim dateText As String = $"Report Date: {DateTime.Now.ToString("MMMM dd, yyyy")}"
        Dim dateSize As SizeF = e.Graphics.MeasureString(dateText, normalFont)
        e.Graphics.DrawString(dateText, normalFont, Brushes.Black,
                           leftMargin + (width / 2) - (dateSize.Width / 2), currentY)
        currentY += 20 ' Reduced from 30 to 20

        ' Get visible columns - exclude hidden columns
        Dim columns As New List(Of String)
        For Each column As DataColumn In dt.Columns
            If column.ColumnName <> "ProductID" AndAlso
               column.ColumnName <> "SupplierID" AndAlso
               column.ColumnName <> "DeliveryID" Then
                columns.Add(column.ColumnName)
            End If
        Next

        ' Define column widths based on content - using relative proportions
        Dim columnWidths As New Dictionary(Of String, Single)
        Dim totalWidthUnits As Single = 10.0F ' Total units to distribute

        ' Set default width ratio for all columns
        For Each column In columns
            columnWidths.Add(column, 1.0F)
        Next

        ' Adjust width ratios for specific column types
        For i As Integer = 0 To columns.Count - 1
            If columns(i).Contains("Date") Then
                columnWidths(columns(i)) = 1.2F
            ElseIf columns(i).Contains("Quantity") Then
                columnWidths(columns(i)) = 0.8F
            ElseIf columns(i).Contains("Name") Or columns(i).Contains("Description") Then
                columnWidths(columns(i)) = 2.0F
            End If
        Next

        ' Calculate actual widths
        Dim columnPositions As New Dictionary(Of String, Single)
        Dim currentX As Single = leftMargin
        columnPositions.Add("Start", currentX)

        Dim totalRatio As Single = 0
        For Each ratio In columnWidths.Values
            totalRatio += ratio
        Next

        For Each columnName In columns
            Dim widthRatio As Single = columnWidths(columnName)
            Dim columnWidth As Single = (width * widthRatio) / totalRatio
            currentX += columnWidth
            columnPositions.Add(columnName, currentX)
        Next

        ' Table header positions
        Dim cellPadding As Single = 3 ' Reduced from 5 to 3
        Dim rowHeight As Single = 20 ' Reduced from 25 to 20

        ' Draw table headers with black background
        Dim headerRect As New RectangleF(leftMargin, currentY, width, rowHeight)
        e.Graphics.FillRectangle(Brushes.Black, headerRect)

        ' Draw header text in white
        currentX = leftMargin + cellPadding
        For i As Integer = 0 To columns.Count - 1
            Dim columnName As String = columns(i)
            Dim columnStart As Single = If(i = 0, leftMargin, columnPositions(columns(i - 1)))
            Dim columnEnd As Single = columnPositions(columnName)
            Dim columnWidth As Single = columnEnd - columnStart

            ' Calculate text position for header
            Dim textX As Single = columnStart + cellPadding
            e.Graphics.DrawString(FormatColumnHeader(columnName), headerFont, Brushes.White, textX, currentY + cellPadding - 2)
        Next
        currentY += rowHeight

        ' Start from rowsPrinted (for pagination)
        Dim rowsPerPage As Integer = 35 ' Increased from 28 to 35 due to smaller font and row height
        Dim endRow As Integer = Math.Min(dt.Rows.Count - 1, rowsPrinted + rowsPerPage - 1)

        For i As Integer = rowsPrinted To endRow
            Dim row As DataRow = dt.Rows(i)

            ' Alternate row colors
            If i Mod 2 = 0 Then
                Dim rowRect As New RectangleF(leftMargin, currentY, width, rowHeight)
                e.Graphics.FillRectangle(Brushes.LightGray, rowRect)
            End If

            ' Draw cell values
            For j As Integer = 0 To columns.Count - 1
                Dim columnName As String = columns(j)
                Dim cellValue As String = FormatCellValue(row, columnName)

                Dim columnStart As Single = If(j = 0, leftMargin, columnPositions(columns(j - 1)))
                Dim columnEnd As Single = columnPositions(columnName)
                Dim columnWidth As Single = columnEnd - columnStart

                ' Set alignment based on column type
                Dim textX As Single = columnStart + cellPadding
                Dim textSize As SizeF = e.Graphics.MeasureString(cellValue, normalFont)

                ' Adjust alignment based on column type
                If IsNumericColumn(columnName) Then
                    ' Right align
                    textX = columnEnd - textSize.Width - cellPadding
                ElseIf columnName.Contains("Date") Then
                    ' Center align
                    textX = columnStart + (columnWidth / 2) - (textSize.Width / 2)
                End If

                ' Draw cell text
                e.Graphics.DrawString(cellValue, normalFont, Brushes.Black, textX, currentY + cellPadding - 1) ' Adjusted for vertical centering
            Next

            currentY += rowHeight
            rowsPrinted += 1

            ' Check if we need to go to a new page
            If currentY + rowHeight > e.MarginBounds.Bottom Then
                e.HasMorePages = True
                currentPage += 1
                Return
            End If
        Next

        ' If we've printed all rows, show footer
        If rowsPrinted >= dt.Rows.Count Then
            ' Add footer
            currentY = e.MarginBounds.Bottom - 30 ' Reduced from 40 to 30
            Dim footerText As String = $"Report generated on {DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss")}"
            Dim footerSize As SizeF = e.Graphics.MeasureString(footerText, smallFont)
            e.Graphics.DrawString(footerText, smallFont, Brushes.Gray,
                             leftMargin + (width / 2) - (footerSize.Width / 2), currentY)
        Else
            ' More pages needed
            e.HasMorePages = True
            currentPage += 1
        End If
    End Sub

    ' Helper method to format column headers (makes them more readable)
    Private Function FormatColumnHeader(columnName As String) As String
        ' Add spaces before capital letters
        Dim result As String = columnName
        For i As Integer = 1 To columnName.Length - 1
            If Char.IsUpper(columnName(i)) AndAlso Not Char.IsWhiteSpace(columnName(i - 1)) Then
                result = result.Insert(i, " ")
                i += 1
            End If
        Next

        Return result
    End Function

    ' Helper method to safely format cell values for display
    Private Function FormatCellValue(row As DataRow, columnName As String) As String
        If IsDBNull(row(columnName)) Then Return ""

        Try
            ' Format date columns
            If columnName.Contains("Date") AndAlso TypeOf row(columnName) Is DateTime Then
                Return Convert.ToDateTime(row(columnName)).ToString("MM/dd/yyyy")

                ' Format numeric values
            ElseIf IsNumericColumn(columnName) Then
                If Decimal.TryParse(row(columnName).ToString(), New Decimal) Then
                    Return Format(Decimal.Parse(row(columnName).ToString()), "#,##0.00")
                End If
            End If

            ' Default handling for all other columns
            Return row(columnName).ToString()
        Catch ex As Exception
            ' Fallback for any exceptions
            Return If(row(columnName) IsNot Nothing, row(columnName).ToString(), "")
        End Try
    End Function

    ' Helper method to check if a column is likely to contain numeric values
    Private Function IsNumericColumn(columnName As String) As Boolean
        Return columnName.Contains("Price") OrElse
               columnName.Contains("Cost") OrElse
               columnName.Contains("Amount") OrElse
               columnName.Contains("Quantity") OrElse
               columnName.Contains("Total")
    End Function

    Private Sub btnClosePanel_Click(sender As Object, e As EventArgs) Handles btnClosePanel.Click
        Me.Close()
    End Sub



    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
