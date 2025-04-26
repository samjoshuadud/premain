Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Drawing.Drawing2D


Public Class ExpirationReports
    ' Define the connection string to connect to the database
    Private connectionString As String = AppConfig.ConnectionString

    ' Print and PDF export variables
    Private WithEvents printDocument As New PrintDocument()
    Private printPreviewDialog As New PrintPreviewDialog()
    Private currentPage As Integer
    Private rowsPrinted As Integer

    ' Event handler for when the form is loaded
    Private Sub ExpirationReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Call the method to load the expiration report when the form loads
        LoadExpirationReport()

        ' Disable the default selection color (remove blue highlight)
        dgvExpirationReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect  ' Optional: Set to FullRowSelect to select the entire row
        dgvExpirationReport.DefaultCellStyle.SelectionBackColor = Color.Transparent   ' Make the selection background color transparent
        dgvExpirationReport.DefaultCellStyle.SelectionForeColor = Color.Black         ' Set the text color of selected cells to black (or any other color)

        ' Configure print preview dialog
        printPreviewDialog.Document = printDocument

        ' Add Print and Export buttons
        AddPrintExportButtons()
        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvExpirationReport.BackgroundColor = Color.White
        dgvExpirationReport.BorderStyle = BorderStyle.None
        dgvExpirationReport.AllowUserToAddRows = False
        dgvExpirationReport.ReadOnly = True
        dgvExpirationReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvExpirationReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvExpirationReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvExpirationReport.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvExpirationReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvExpirationReport.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvExpirationReport.EnableHeadersVisualStyles = False
        dgvExpirationReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvExpirationReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        'dgvExpirationReport.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
    End Sub
    ' Add Print and Export buttons to the form
    Private Sub AddPrintExportButtons()
        ' Create Export to PDF button
        Dim btnExportPDF As New Button()
        btnExportPDF.Text = "Export to PDF"
        btnExportPDF.BackColor = Color.FromArgb(0, 102, 204)
        btnExportPDF.ForeColor = Color.White
        btnExportPDF.FlatStyle = FlatStyle.Flat
        btnExportPDF.Size = New Size(155, 42)
        btnExportPDF.Location = New Point(dgvExpirationReport.Location.X, dgvExpirationReport.Location.Y - 50)
        AddHandler btnExportPDF.Click, AddressOf BtnExportPDF_Click
        Me.Controls.Add(btnExportPDF)

        ' Create Print button
        Dim btnPrint As New Button()
        btnPrint.Text = "Print Report"
        btnPrint.BackColor = Color.FromArgb(34, 139, 34)
        btnPrint.ForeColor = Color.White
        btnPrint.FlatStyle = FlatStyle.Flat
        btnPrint.Size = New Size(155, 42)
        btnPrint.Location = New Point(btnExportPDF.Location.X + btnExportPDF.Width + 10, btnExportPDF.Location.Y)
        AddHandler btnPrint.Click, AddressOf BtnPrint_Click
        Me.Controls.Add(btnPrint)
    End Sub

    ' Method to load expiration data from the database
    Private Sub LoadExpirationReport()
        ' Create a connection to the SQL Server
        Using connection As New SqlConnection(connectionString)
            Try
                ' Open the connection
                connection.Open()

                ' SQL query to retrieve the latest expiration data per product from Inventory
                Dim query As String = "
                WITH LatestPerProduct AS (
                    SELECT i.*, 
                           ROW_NUMBER() OVER (PARTITION BY i.ProductID ORDER BY i.ExpirationDate DESC) AS rn
                    FROM dbo.Inventory i
                )
                SELECT 
                    p.ProductName, 
                    lpp.ExpirationDate, 
                    lpp.QuantityInStock,
                    DATEDIFF(DAY, GETDATE(), lpp.ExpirationDate) AS DaysToExpire,
                    CASE
                        WHEN DATEDIFF(DAY, GETDATE(), lpp.ExpirationDate) <= 0 THEN 'Expired'
                        WHEN DATEDIFF(DAY, GETDATE(), lpp.ExpirationDate) <= 30 THEN 'Expiring Soon'
                        ELSE 'Good'
                    END AS Status,
                    s.SupplierName
                FROM LatestPerProduct lpp
                LEFT JOIN dbo.Products p ON lpp.ProductID = p.ProductID
                LEFT JOIN dbo.Suppliers s ON lpp.SupplierID = s.SupplierID
                WHERE lpp.rn = 1
                ORDER BY lpp.ExpirationDate DESC;"

                ' Create the command with the query
                Dim command As New SqlCommand(query, connection)

                ' Execute the query and get the data
                Dim reader As SqlDataReader = command.ExecuteReader()

                ' Clear the DataGridView before loading new data
                dgvExpirationReport.Rows.Clear()

                ' Add columns to the DataGridView in the desired order
                If dgvExpirationReport.Columns.Count = 0 Then
                    dgvExpirationReport.Columns.Add("ProductName", "Product Name")
                    dgvExpirationReport.Columns.Add("SupplierName", "Supplier")
                    dgvExpirationReport.Columns.Add("QuantityInStock", "Stock Quantity")
                    dgvExpirationReport.Columns.Add("Status", "Status")
                    dgvExpirationReport.Columns.Add("DaysToExpire", "Days to Expire")
                    dgvExpirationReport.Columns.Add("ExpirationDate", "Expiration Date")
                End If

                ' Process the results and load them into the DataGridView
                While reader.Read()
                    ' Handle DBNull for ExpirationDate
                    Dim expirationDate As String
                    If IsDBNull(reader("ExpirationDate")) Then
                        expirationDate = "N/A" ' Default if ExpirationDate is NULL
                    Else
                        expirationDate = Convert.ToDateTime(reader("ExpirationDate")).ToString("MM/dd/yyyy")
                    End If

                    ' Add the row to the DataGridView
                    dgvExpirationReport.Rows.Add(
                    reader("ProductName").ToString(),
                    If(IsDBNull(reader("SupplierName")), "N/A", reader("SupplierName").ToString()),
                    reader("QuantityInStock").ToString(),
                    reader("Status").ToString(),
                    reader("DaysToExpire").ToString(),
                    expirationDate ' Use the properly formatted expiration date
                )
                End While

                ' Automatically adjust column widths to fill available space
                dgvExpirationReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                ' Manually adjust specific columns if needed for better visual layout
                dgvExpirationReport.Columns("ProductName").Width = 200  ' Set width for Product Name
                dgvExpirationReport.Columns("SupplierName").Width = 150  ' Set width for Supplier Name
                dgvExpirationReport.Columns("QuantityInStock").Width = 120  ' Set width for Quantity
                dgvExpirationReport.Columns("Status").Width = 150  ' Set width for Status
                dgvExpirationReport.Columns("DaysToExpire").Width = 120  ' Set width for Days to Expire
                dgvExpirationReport.Columns("ExpirationDate").Width = 150  ' Set width for Expiration Date

            Catch ex As SqlException
                ' Handle any SQL-related errors
                MessageBox.Show("SQL Error: " & ex.Message)
            Catch ex As Exception
                ' Handle any other errors
                MessageBox.Show("Error: " & ex.Message)
            Finally
                ' Ensure that the connection is closed, even if there was an error
                connection.Close()
            End Try
        End Using
    End Sub




    ' Event handler for cell formatting (this will highlight expired and expiring soon status)
    Private Sub dgvExpirationReport_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvExpirationReport.CellFormatting
        If dgvExpirationReport.Columns(e.ColumnIndex).Name = "Status" AndAlso e.Value IsNot Nothing Then
            Dim status As String = e.Value.ToString().Trim().ToLower()

            Select Case status
                Case "expired"
                    e.CellStyle.BackColor = Color.Red
                    e.CellStyle.ForeColor = Color.White
                Case "expiring soon"
                    e.CellStyle.BackColor = Color.Yellow
                    e.CellStyle.ForeColor = Color.Black
                Case "good"
                    e.CellStyle.BackColor = Color.Green
                    e.CellStyle.ForeColor = Color.White
                Case Else
                    e.CellStyle.BackColor = Color.White
                    e.CellStyle.ForeColor = Color.Black
            End Select
        End If
    End Sub


    ' Export to PDF button click event handler
    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        If dgvExpirationReport.Rows.Count = 0 Then
            MessageBox.Show("No data to export. Please generate a report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Create a SaveFileDialog to get the file save location
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
        saveDialog.Title = "Save Expiration Report as PDF"
        saveDialog.FileName = $"ExpirationReport_{DateTime.Now:yyyyMMdd}.pdf"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Try
                ExportToPDF(saveDialog.FileName)
                MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error exporting to PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Method to export the expiration data to PDF
    Private Sub ExportToPDF(filePath As String)
        ' Create a new PDF document
        Dim document As New Document(PageSize.A4.Rotate(), 40, 40, 40, 40) ' Use landscape orientation for wider table

        Try
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))

            ' Open the document for writing
            document.Open()

            ' Add a title
            Dim titleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 16, iTextSharp.text.Font.BOLD, BaseColor.Black)
            Dim title As New Paragraph("SHIENNA'S MINI GROCERY STORE", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            document.Add(title)

            ' Add report title
            Dim reportTitleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, iTextSharp.text.Font.BOLD, BaseColor.Black)
            Dim reportTitle As New Paragraph("Product Expiration Report", reportTitleFont)
            reportTitle.Alignment = Element.ALIGN_CENTER
            document.Add(reportTitle)

            ' Add date information
            Dim dateFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim dateInfo As New Paragraph($"Report Date: {DateTime.Now.ToString("MMMM dd, yyyy")}", dateFont)
            dateInfo.Alignment = Element.ALIGN_CENTER
            dateInfo.SpacingAfter = 20
            document.Add(dateInfo)

            ' Create a table for the data
            Dim table As New PdfPTable(6) ' 6 columns for all the fields
            table.WidthPercentage = 100
            table.SetWidths(New Single() {2.5F, 1.5F, 1.0F, 1.5F, 1.0F, 1.5F}) ' Adjust column widths

            ' Add table headers
            Dim headerFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10, iTextSharp.text.Font.BOLD, BaseColor.White)
            Dim cellBackColor As New BaseColor(0, 0, 0) ' Black background for header

            ' Add table header cells
            AddHeaderCell(table, "Product Name", headerFont, cellBackColor)
            AddHeaderCell(table, "Supplier", headerFont, cellBackColor)
            AddHeaderCell(table, "Stock Quantity", headerFont, cellBackColor)
            AddHeaderCell(table, "Status", headerFont, cellBackColor)
            AddHeaderCell(table, "Days to Expire", headerFont, cellBackColor)
            AddHeaderCell(table, "Expiration Date", headerFont, cellBackColor)

            ' Add table data
            Dim cellFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim expiredBackColor As New BaseColor(255, 200, 200) ' Light red for expired
            Dim expiringSoonBackColor As New BaseColor(255, 255, 200) ' Light yellow for expiring soon
            Dim goodBackColor As New BaseColor(200, 255, 200) ' Light green for good

            For i As Integer = 0 To dgvExpirationReport.Rows.Count - 1
                If i < dgvExpirationReport.Rows.Count Then
                    Dim row As DataGridViewRow = dgvExpirationReport.Rows(i)

                    ' Determine background color based on status
                    Dim backColor As BaseColor = BaseColor.White
                    Dim status As String = SafeGetCellValue(row, "Status")

                    Select Case status
                        Case "Expired"
                            backColor = expiredBackColor
                        Case "Expiring Soon"
                            backColor = expiringSoonBackColor
                        Case "Good"
                            backColor = goodBackColor
                    End Select

                    ' Add cells with data
                    AddCell(table, SafeGetCellValue(row, "ProductName"), cellFont, backColor)
                    AddCell(table, SafeGetCellValue(row, "SupplierName"), cellFont, backColor)
                    AddCell(table, SafeGetCellValue(row, "QuantityInStock"), cellFont, backColor, Element.ALIGN_CENTER)
                    AddCell(table, status, cellFont, backColor, Element.ALIGN_CENTER)
                    AddCell(table, SafeGetCellValue(row, "DaysToExpire"), cellFont, backColor, Element.ALIGN_CENTER)
                    AddCell(table, SafeGetCellValue(row, "ExpirationDate"), cellFont, backColor, Element.ALIGN_CENTER)
                End If
            Next

            ' Add the table to the document
            document.Add(table)

            ' Add footer with generation date
            Dim footer As New Paragraph($"Report generated on {DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss")}",
                                       New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8, iTextSharp.text.Font.ITALIC, BaseColor.Gray))
            footer.Alignment = Element.ALIGN_CENTER
            footer.SpacingBefore = 30
            document.Add(footer)
        Catch ex As Exception
            ' In case of error, ensure document is closed
            If document.IsOpen() Then
                document.Close()
            End If
            Throw ' Re-throw the exception to be caught by the caller
        Finally
            ' Close the document
            If document.IsOpen() Then
                document.Close()
            End If
        End Try
    End Sub

    ' Helper method to add a header cell to the PDF table
    Private Sub AddHeaderCell(table As PdfPTable, text As String, font As Font, backColor As BaseColor)
        Dim cell As New PdfPCell(New Phrase(text, font))
        cell.BackgroundColor = backColor
        cell.HorizontalAlignment = Element.ALIGN_CENTER
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Padding = 5
        table.AddCell(cell)
    End Sub

    ' Helper method to add a data cell to the PDF table
    Private Sub AddCell(table As PdfPTable, text As String, font As Font, backColor As BaseColor, Optional alignment As Integer = Element.ALIGN_LEFT)
        Dim cell As New PdfPCell(New Phrase(text, font))
        cell.BackgroundColor = backColor
        cell.HorizontalAlignment = alignment
        cell.VerticalAlignment = Element.ALIGN_MIDDLE
        cell.Padding = 5
        table.AddCell(cell)
    End Sub

    ' Print button click event
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs)
        If dgvExpirationReport.Rows.Count = 0 Then
            MessageBox.Show("No data to print. Please load the report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Reset pagination variables
        currentPage = 0
        rowsPrinted = 0

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
        Try
            ' Page title and header
            Dim titleFont As New System.Drawing.Font("Arial", 16, FontStyle.Bold)
            Dim headerFont As New System.Drawing.Font("Arial", 10, FontStyle.Bold)
            Dim normalFont As New System.Drawing.Font("Arial", 9)
            Dim smallFont As New System.Drawing.Font("Arial", 8)

            ' Print margins and positions
            Dim leftMargin As Single = e.MarginBounds.Left
            Dim topMargin As Single = e.MarginBounds.Top
            Dim width As Single = e.MarginBounds.Width
            Dim currentY As Single = topMargin

            ' Print store name and report title
            Dim titleText As String = "SHIENNA'S MINI GROCERY STORE"
            Dim titleSize As SizeF = e.Graphics.MeasureString(titleText, titleFont)
            e.Graphics.DrawString(titleText, titleFont, Brushes.Black,
                            leftMargin + (width / 2) - (titleSize.Width / 2), currentY)
            currentY += 30

            Dim reportText As String = "Product Expiration Report"
            Dim reportSize As SizeF = e.Graphics.MeasureString(reportText, headerFont)
            e.Graphics.DrawString(reportText, headerFont, Brushes.Black,
                            leftMargin + (width / 2) - (reportSize.Width / 2), currentY)
            currentY += 20

            ' Print date
            Dim dateText As String = $"Report Date: {DateTime.Now.ToString("MMMM dd, yyyy")}"
            Dim dateSize As SizeF = e.Graphics.MeasureString(dateText, normalFont)
            e.Graphics.DrawString(dateText, normalFont, Brushes.Black,
                            leftMargin + (width / 2) - (dateSize.Width / 2), currentY)
            currentY += 30

            ' Table header positions - adjust for smaller font and landscape orientation
            Dim columnCount As Integer = 6
            Dim columnWidths As Single() = {width * 0.25F, width * 0.15F, width * 0.1F, width * 0.15F, width * 0.1F, width * 0.15F}
            Dim cellPadding As Single = 2
            Dim rowHeight As Single = 20

            ' Draw table headers with black background
            Dim headerRect As New RectangleF(leftMargin, currentY, width, rowHeight)
            e.Graphics.FillRectangle(Brushes.Black, headerRect)

            ' Draw header text in white
            Dim columnHeaderTexts As String() = {"Product Name", "Supplier", "Stock Quantity", "Status", "Days to Expire", "Expiration Date"}
            Dim currentX As Single = leftMargin

            For i As Integer = 0 To columnHeaderTexts.Length - 1
                Dim headerText As String = columnHeaderTexts(i)
                Dim headerTextSize As SizeF = e.Graphics.MeasureString(headerText, headerFont)

                ' Center text within column for header
                Dim textX As Single = currentX + (columnWidths(i) / 2) - (headerTextSize.Width / 2)

                e.Graphics.DrawString(headerText, headerFont, Brushes.White, textX, currentY + cellPadding)
                currentX += columnWidths(i)
            Next

            currentY += rowHeight

            ' Start from rowsPrinted (for pagination)
            Dim rowsPerPage As Integer = 30 ' More rows per page due to landscape orientation
            Dim endRow As Integer = Math.Min(dgvExpirationReport.Rows.Count - 1, rowsPrinted + rowsPerPage - 1)

            For i As Integer = rowsPrinted To endRow
                If i < dgvExpirationReport.Rows.Count Then
                    Dim row As DataGridViewRow = dgvExpirationReport.Rows(i)

                    ' Get status to determine row color
                    Dim status As String = SafeGetCellValue(row, "Status")
                    Dim rowBrush As Brush

                    Select Case status
                        Case "Expired"
                            rowBrush = New SolidBrush(Color.FromArgb(255, 200, 200)) ' Light red
                        Case "Expiring Soon"
                            rowBrush = New SolidBrush(Color.FromArgb(255, 255, 200)) ' Light yellow
                        Case "Good"
                            rowBrush = New SolidBrush(Color.FromArgb(200, 255, 200)) ' Light green
                        Case Else
                            rowBrush = Brushes.White
                    End Select

                    ' Draw row background
                    Dim rowRect As New RectangleF(leftMargin, currentY, width, rowHeight)
                    e.Graphics.FillRectangle(rowBrush, rowRect)

                    ' Draw cell values with appropriate alignment
                    currentX = leftMargin

                    ' Product Name - left aligned
                    e.Graphics.DrawString(SafeGetCellValue(row, "ProductName"), normalFont, Brushes.Black,
                                        currentX + cellPadding, currentY + cellPadding)
                    currentX += columnWidths(0)

                    ' Supplier - left aligned
                    e.Graphics.DrawString(SafeGetCellValue(row, "SupplierName"), normalFont, Brushes.Black,
                                        currentX + cellPadding, currentY + cellPadding)
                    currentX += columnWidths(1)

                    ' Stock Quantity - center aligned
                    Dim qtyText As String = SafeGetCellValue(row, "QuantityInStock")
                    Dim qtySize As SizeF = e.Graphics.MeasureString(qtyText, normalFont)
                    e.Graphics.DrawString(qtyText, normalFont, Brushes.Black,
                                        currentX + (columnWidths(2) / 2) - (qtySize.Width / 2), currentY + cellPadding)
                    currentX += columnWidths(2)

                    ' Status - center aligned
                    Dim statusSize As SizeF = e.Graphics.MeasureString(status, normalFont)
                    e.Graphics.DrawString(status, normalFont, Brushes.Black,
                                        currentX + (columnWidths(3) / 2) - (statusSize.Width / 2), currentY + cellPadding)
                    currentX += columnWidths(3)

                    ' Days to Expire - center aligned
                    Dim daysText As String = SafeGetCellValue(row, "DaysToExpire")
                    Dim daysSize As SizeF = e.Graphics.MeasureString(daysText, normalFont)
                    e.Graphics.DrawString(daysText, normalFont, Brushes.Black,
                                        currentX + (columnWidths(4) / 2) - (daysSize.Width / 2), currentY + cellPadding)
                    currentX += columnWidths(4)

                    ' Expiration Date - center aligned
                    Dim dateValText As String = SafeGetCellValue(row, "ExpirationDate")
                    Dim dateValSize As SizeF = e.Graphics.MeasureString(dateValText, normalFont)
                    e.Graphics.DrawString(dateValText, normalFont, Brushes.Black,
                                        currentX + (columnWidths(5) / 2) - (dateValSize.Width / 2), currentY + cellPadding)
                End If

                currentY += rowHeight
                rowsPrinted += 1

                ' Check if we need to go to a new page
                If currentY + rowHeight > e.MarginBounds.Bottom Then
                    e.HasMorePages = True
                    currentPage += 1
                    Return
                End If
            Next

            ' If we've printed all rows, show footer (only on last page)
            If rowsPrinted >= dgvExpirationReport.Rows.Count Then
                ' Add footer
                currentY = e.MarginBounds.Bottom - 30
                Dim footerText As String = $"Report generated on {DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss")}"
                Dim footerSize As SizeF = e.Graphics.MeasureString(footerText, smallFont)
                e.Graphics.DrawString(footerText, smallFont, Brushes.Gray,
                                leftMargin + (width / 2) - (footerSize.Width / 2), currentY)
            Else
                ' More pages needed
                e.HasMorePages = True
                currentPage += 1
            End If

        Catch ex As Exception
            ' Handle any exceptions during printing
            MessageBox.Show($"Error while printing: {ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.HasMorePages = False
        End Try
    End Sub

    ' Helper method to safely get cell value with null checking
    Private Function SafeGetCellValue(row As DataGridViewRow, columnName As String) As String
        Try
            If row Is Nothing OrElse row.Cells(columnName) Is Nothing OrElse row.Cells(columnName).Value Is Nothing Then
                Return ""
            End If

            Return row.Cells(columnName).Value.ToString()
        Catch ex As Exception
            ' Return empty string if any error occurs
            Return ""
        End Try
    End Function

    Private Sub btnClosePanel_Click(sender As Object, e As EventArgs) Handles btnClosePanel.Click
        Me.Close()
    End Sub
End Class
