Imports Microsoft.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class SalesReports
    ' Use the appropriate connection string for SQL Server
    Private connectionString As String = AppConfig.ConnectionString
    Private connection As SqlConnection
    Private salesData As DataTable ' Store the data for PDF export
    Private WithEvents printDocument As New PrintDocument()
    Private printPreviewDialog As New PrintPreviewDialog()

    ' Variables to track pagination for printing
    Private currentPage As Integer
    Private rowsPrinted As Integer

    Public Sub New()
        ' Initialization through Designer (no need for dynamic controls)
        InitializeComponent()
        connection = New SqlConnection(connectionString)

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
        btnExportPDF.Location = New Point(728, 87) ' Position to right of Generate Report button
        AddHandler btnExportPDF.Click, AddressOf BtnExportPDF_Click
        Me.Controls.Add(btnExportPDF)

        ' Create Print button
        Dim btnPrint As New Button()
        btnPrint.Text = "Print Report"
        btnPrint.BackColor = Color.FromArgb(34, 139, 34)
        btnPrint.ForeColor = Color.White
        btnPrint.FlatStyle = FlatStyle.Flat
        btnPrint.Size = New Size(155, 42)
        btnPrint.Location = New Point(889, 87) ' Position to right of Export PDF button
        AddHandler btnPrint.Click, AddressOf BtnPrint_Click
        Me.Controls.Add(btnPrint)
    End Sub

    ' Button click event handler to generate the report
    Private Sub BtnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        LoadSalesData()
    End Sub

    ' Load Sales Data from Database
    Private Sub LoadSalesData()
        Try
            ' Ensure the connection is open
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If

            ' SQL query to fetch sales data
            Dim query As String = "SELECT SaleDate, TransactionNumber, COALESCE(TotalAmount, 0) AS TotalAmount, " &
                                  "COALESCE(DiscountAmount, 0) AS DiscountAmount, COALESCE(VatAmount, 0) AS VatAmount, " &
                                  "COALESCE(NetAmount, 0) AS NetAmount " &
                                  "FROM Sales " &
                                  "WHERE SaleDate BETWEEN @StartDate AND @EndDate"

            ' Set up the SQL command with parameters
            Dim cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value.Date) ' Start of the selected date
            cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1)) ' End of the selected date (23:59:59.999)

            ' Fill the DataTable with query results
            Dim adapter As New SqlDataAdapter(cmd)
            salesData = New DataTable() ' Store the data for PDF export
            adapter.Fill(salesData)

            ' Set the DataGridView DataSource
            dgvSalesReport.DataSource = Nothing ' Clear the old data source
            dgvSalesReport.DataSource = salesData

            ' Check if there are any rows in the DataTable
            If salesData.Rows.Count = 0 Then
                MessageBox.Show("No records found for the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' Update Summary Labels
            Dim totalSales As Decimal = 0
            Dim totalDiscount As Decimal = 0
            Dim totalVAT As Decimal = 0
            Dim netTotal As Decimal = 0

            ' Calculate totals manually to avoid LINQ errors
            For Each row As DataRow In salesData.Rows
                totalSales += Convert.ToDecimal(row("TotalAmount"))
                totalDiscount += Convert.ToDecimal(row("DiscountAmount"))
                totalVAT += Convert.ToDecimal(row("VatAmount"))
                netTotal += Convert.ToDecimal(row("NetAmount"))
            Next

            lblTotalSales.Text = $"Total Sales: ₱{totalSales:N2}"
            lblTotalDiscount.Text = $"Total Discount: ₱{totalDiscount:N2}"
            lblTotalVAT.Text = $"Total VAT: ₱{totalVAT:N2}"
            lblNetTotal.Text = $"Net Total: ₱{netTotal:N2}"

        Catch ex As Exception
            ' Display error message with details
            MessageBox.Show("Error loading sales data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Ensure the connection is closed properly
            If connection.State = ConnectionState.Open Then
                connection.Close()
            End If
        End Try

        ' Format DataGridView columns
        If dgvSalesReport.Columns.Count > 0 Then
            dgvSalesReport.Columns("SaleDate").Width = 120
            dgvSalesReport.Columns("TransactionNumber").Width = 120
            dgvSalesReport.Columns("TotalAmount").Width = 120
            dgvSalesReport.Columns("DiscountAmount").Width = 120
            dgvSalesReport.Columns("VatAmount").Width = 120
            dgvSalesReport.Columns("NetAmount").Width = 120
        End If

        ' Set the height of the column headers
        dgvSalesReport.ColumnHeadersHeight = 30 ' Adjust this based on your needs

        ' Set default row height
        dgvSalesReport.RowTemplate.Height = 25 ' Adjust this based on your needs

        ' Set DataGridView background color to white
        dgvSalesReport.BackgroundColor = Color.White

        ' Remove the extra row (the row for adding new data)
        dgvSalesReport.AllowUserToAddRows = False

        ' Disable the blue highlight when a row is selected
        dgvSalesReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvSalesReport.DefaultCellStyle.SelectionBackColor = dgvSalesReport.BackgroundColor
        dgvSalesReport.DefaultCellStyle.SelectionForeColor = dgvSalesReport.ForeColor
    End Sub

    ' Event handler for PDF export button
    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        If salesData Is Nothing OrElse salesData.Rows.Count = 0 Then
            MessageBox.Show("No data to export. Please generate a report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Create a SaveFileDialog to get the file save location
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
        saveDialog.Title = "Save Sales Report as PDF"
        saveDialog.FileName = $"SalesReport_{dtpStartDate.Value:yyyyMMdd}_{dtpEndDate.Value:yyyyMMdd}.pdf"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Try
                ExportToPDF(saveDialog.FileName)
                MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error exporting to PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Method to export the sales data to PDF
    Private Sub ExportToPDF(filePath As String)
        ' Create a new PDF document
        Dim document As New Document(PageSize.A4, 40, 40, 40, 40)

        Try
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))

            ' Open the document for writing
            document.Open()

            ' Add a title - Use BaseFont instead of FontFamily
            Dim titleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 16, iTextSharp.text.Font.BOLD, BaseColor.Black)
            Dim title As New Paragraph("FCM SUPERMARKET", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            document.Add(title)

            ' Add report title
            Dim reportTitleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, iTextSharp.text.Font.BOLD, BaseColor.Black)
            Dim reportTitle As New Paragraph("Sales Report", reportTitleFont)
            reportTitle.Alignment = Element.ALIGN_CENTER
            document.Add(reportTitle)

            ' Add date range
            Dim dateRangeFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim dateRange As New Paragraph($"Date Range: {dtpStartDate.Value.ToString("MMMM dd, yyyy")} - {dtpEndDate.Value.ToString("MMMM dd, yyyy")}", dateRangeFont)
            dateRange.Alignment = Element.ALIGN_CENTER
            dateRange.SpacingAfter = 20
            document.Add(dateRange)

            ' Create a table for the sales data
            Dim table As New PdfPTable(6) ' 6 columns for SaleDate, TransactionNumber, TotalAmount, DiscountAmount, VatAmount, NetAmount
            table.WidthPercentage = 100
            table.SetWidths(New Single() {1.2F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F}) ' Adjust column widths

            ' Add table headers
            Dim headerFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10, iTextSharp.text.Font.BOLD, BaseColor.White)
            Dim cellBackColor As New BaseColor(0, 0, 0) ' Black background for header

            ' Add table header cells
            AddHeaderCell(table, "Sale Date", headerFont, cellBackColor)
            AddHeaderCell(table, "Transaction#", headerFont, cellBackColor)
            AddHeaderCell(table, "Total Amount", headerFont, cellBackColor)
            AddHeaderCell(table, "Discount", headerFont, cellBackColor)
            AddHeaderCell(table, "VAT", headerFont, cellBackColor)
            AddHeaderCell(table, "Net Amount", headerFont, cellBackColor)

            ' Add table data
            Dim cellFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim altCellBackColor As New BaseColor(240, 240, 240) ' Light gray for alternating rows

            ' Initialize totals
            Dim totalSales As Decimal = 0
            Dim totalDiscount As Decimal = 0
            Dim totalVAT As Decimal = 0
            Dim netTotal As Decimal = 0

            For i As Integer = 0 To salesData.Rows.Count - 1
                Dim row As DataRow = salesData.Rows(i)
                Dim backColor As BaseColor = If(i Mod 2 = 0, BaseColor.White, altCellBackColor)

                ' Format date value
                Dim saleDate As DateTime = Convert.ToDateTime(row("SaleDate"))
                AddCell(table, saleDate.ToString("MM/dd/yyyy"), cellFont, backColor)
                AddCell(table, row("TransactionNumber").ToString(), cellFont, backColor)

                ' Format and add numeric values
                Dim totalAmount As Decimal = Convert.ToDecimal(row("TotalAmount"))
                Dim discountAmount As Decimal = Convert.ToDecimal(row("DiscountAmount"))
                Dim vatAmount As Decimal = Convert.ToDecimal(row("VatAmount"))
                Dim rowNetAmount As Decimal = Convert.ToDecimal(row("NetAmount"))

                AddCell(table, $"₱{totalAmount:N2}", cellFont, backColor, Element.ALIGN_RIGHT)
                AddCell(table, $"₱{discountAmount:N2}", cellFont, backColor, Element.ALIGN_RIGHT)
                AddCell(table, $"₱{vatAmount:N2}", cellFont, backColor, Element.ALIGN_RIGHT)
                AddCell(table, $"₱{rowNetAmount:N2}", cellFont, backColor, Element.ALIGN_RIGHT)

                ' Accumulate totals
                totalSales += totalAmount
                totalDiscount += discountAmount
                totalVAT += vatAmount
                netTotal += rowNetAmount
            Next

            ' Add the table to the document
            document.Add(table)

            ' Add summary section
            document.Add(New Paragraph(" ")) ' Add some space

            ' Create a table for the summary
            Dim summaryTable As New PdfPTable(2)
            summaryTable.WidthPercentage = 50
            summaryTable.HorizontalAlignment = Element.ALIGN_RIGHT
            summaryTable.SetWidths(New Single() {1.5F, 1.0F})

            Dim summaryFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10, iTextSharp.text.Font.BOLD, BaseColor.Black)
            AddSummaryRow(summaryTable, "Total Sales:", $"₱{totalSales:N2}", summaryFont)
            AddSummaryRow(summaryTable, "Total Discount:", $"₱{totalDiscount:N2}", summaryFont)
            AddSummaryRow(summaryTable, "Total VAT:", $"₱{totalVAT:N2}", summaryFont)

            ' Add Net Total with a different background color
            Dim netTotalFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12, iTextSharp.text.Font.BOLD, BaseColor.Black)
            AddSummaryRow(summaryTable, "Net Total:", $"₱{netTotal:N2}", netTotalFont, New BaseColor(220, 220, 220))

            ' Add the summary table to the document
            document.Add(summaryTable)

            ' Add footer with generation date
            Dim footer As New Paragraph($"Report generated on {DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss")}",
                                       New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8, iTextSharp.text.Font.ITALIC, BaseColor.Gray))
            footer.Alignment = Element.ALIGN_CENTER
            footer.SpacingBefore = 30
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

    ' Helper method to add a summary row to the PDF table
    Private Sub AddSummaryRow(table As PdfPTable, label As String, value As String, font As Font, Optional backColor As BaseColor = Nothing)
        Dim labelCell As New PdfPCell(New Phrase(label, font))
        Dim valueCell As New PdfPCell(New Phrase(value, font))

        If backColor IsNot Nothing Then
            labelCell.BackgroundColor = backColor
            valueCell.BackgroundColor = backColor
        End If

        labelCell.HorizontalAlignment = Element.ALIGN_RIGHT
        labelCell.Border = PdfPCell.NO_BORDER
        valueCell.HorizontalAlignment = Element.ALIGN_RIGHT
        valueCell.Border = PdfPCell.NO_BORDER

        table.AddCell(labelCell)
        table.AddCell(valueCell)
    End Sub

    ' Print button click event
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs)
        If salesData Is Nothing OrElse salesData.Rows.Count = 0 Then
            MessageBox.Show("No data to print. Please generate a report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        ' Page title and header
        Dim titleFont As New System.Drawing.Font("Arial", 16, FontStyle.Bold)
        Dim headerFont As New System.Drawing.Font("Arial", 12, FontStyle.Bold)
        Dim normalFont As New System.Drawing.Font("Arial", 10)
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

        Dim reportText As String = "Sales Report"
        Dim reportSize As SizeF = e.Graphics.MeasureString(reportText, headerFont)
        e.Graphics.DrawString(reportText, headerFont, Brushes.Black,
                           leftMargin + (width / 2) - (reportSize.Width / 2), currentY)
        currentY += 25

        ' Print date range
        Dim dateRangeText As String = $"Date Range: {dtpStartDate.Value.ToString("MMMM dd, yyyy")} - {dtpEndDate.Value.ToString("MMMM dd, yyyy")}"
        Dim dateRangeSize As SizeF = e.Graphics.MeasureString(dateRangeText, normalFont)
        e.Graphics.DrawString(dateRangeText, normalFont, Brushes.Black,
                           leftMargin + (width / 2) - (dateRangeSize.Width / 2), currentY)
        currentY += 25

        ' Table header positions
        Dim columnCount As Integer = 6
        Dim columnWidth As Single = width / columnCount
        Dim cellPadding As Single = 5
        Dim rowHeight As Single = 25

        ' Draw table headers with black background
        Dim headerRect As New RectangleF(leftMargin, currentY, width, rowHeight)
        e.Graphics.FillRectangle(Brushes.Black, headerRect)

        ' Draw header text in white
        Dim columnHeaders As String() = {"Sale Date", "Transaction#", "Total Amount", "Discount", "VAT", "Net Amount"}
        For i As Integer = 0 To columnHeaders.Length - 1
            Dim headerText As String = columnHeaders(i)
            Dim headerX As Single = leftMargin + (i * columnWidth) + cellPadding
            e.Graphics.DrawString(headerText, headerFont, Brushes.White, headerX, currentY + cellPadding)
        Next
        currentY += rowHeight

        ' Draw table data rows
        Dim totalSales As Decimal = 0
        Dim totalDiscount As Decimal = 0
        Dim totalVAT As Decimal = 0
        Dim netTotal As Decimal = 0

        ' Start from rowsPrinted (for pagination)
        Dim rowsPerPage As Integer = 20 ' Approximate rows per page
        Dim endRow As Integer = Math.Min(salesData.Rows.Count - 1, rowsPrinted + rowsPerPage - 1)

        For i As Integer = rowsPrinted To endRow
            Dim row As DataRow = salesData.Rows(i)

            ' Alternate row colors
            If i Mod 2 = 0 Then
                Dim rowRect As New RectangleF(leftMargin, currentY, width, rowHeight)
                e.Graphics.FillRectangle(Brushes.LightGray, rowRect)
            End If

            ' Format date
            Dim saleDate As DateTime = Convert.ToDateTime(row("SaleDate"))
            Dim formattedDate As String = saleDate.ToString("MM/dd/yyyy")

            ' Draw cell values
            e.Graphics.DrawString(formattedDate, normalFont, Brushes.Black, leftMargin + cellPadding, currentY + cellPadding)
            e.Graphics.DrawString(row("TransactionNumber").ToString(), normalFont, Brushes.Black, leftMargin + columnWidth + cellPadding, currentY + cellPadding)

            ' Get and accumulate totals 
            Dim totalAmount As Decimal = Convert.ToDecimal(row("TotalAmount"))
            Dim discountAmount As Decimal = Convert.ToDecimal(row("DiscountAmount"))
            Dim vatAmount As Decimal = Convert.ToDecimal(row("VatAmount"))
            Dim rowNetAmount As Decimal = Convert.ToDecimal(row("NetAmount"))

            totalSales += totalAmount
            totalDiscount += discountAmount
            totalVAT += vatAmount
            netTotal += rowNetAmount

            ' Right-align numeric values
            Dim totalAmountText As String = $"₱{totalAmount:N2}"
            Dim totalAmountSize As SizeF = e.Graphics.MeasureString(totalAmountText, normalFont)
            e.Graphics.DrawString(totalAmountText, normalFont, Brushes.Black,
                                leftMargin + (2 * columnWidth) + columnWidth - totalAmountSize.Width - cellPadding, currentY + cellPadding)

            Dim discountText As String = $"₱{discountAmount:N2}"
            Dim discountSize As SizeF = e.Graphics.MeasureString(discountText, normalFont)
            e.Graphics.DrawString(discountText, normalFont, Brushes.Black,
                                leftMargin + (3 * columnWidth) + columnWidth - discountSize.Width - cellPadding, currentY + cellPadding)

            Dim vatText As String = $"₱{vatAmount:N2}"
            Dim vatSize As SizeF = e.Graphics.MeasureString(vatText, normalFont)
            e.Graphics.DrawString(vatText, normalFont, Brushes.Black,
                               leftMargin + (4 * columnWidth) + columnWidth - vatSize.Width - cellPadding, currentY + cellPadding)

            Dim netText As String = $"₱{rowNetAmount:N2}"
            Dim netSize As SizeF = e.Graphics.MeasureString(netText, normalFont)
            e.Graphics.DrawString(netText, normalFont, Brushes.Black,
                               leftMargin + (5 * columnWidth) + columnWidth - netSize.Width - cellPadding, currentY + cellPadding)

            currentY += rowHeight
            rowsPrinted += 1

            ' Check if we need to go to a new page
            If currentY + rowHeight > e.MarginBounds.Bottom Then
                e.HasMorePages = True
                currentPage += 1
                Return
            End If
        Next

        ' If we've printed all rows, show summary (only on last page)
        If rowsPrinted >= salesData.Rows.Count Then
            ' Draw summary section
            currentY += 20
            Dim summaryFont As New System.Drawing.Font("Arial", 11, FontStyle.Bold)

            ' Summary section position
            Dim summaryX As Single = leftMargin + (4 * columnWidth)
            Dim summaryWidth As Single = 2 * columnWidth

            ' Draw summary labels and values
            DrawSummaryLine(e, "Total Sales:", $"₱{totalSales:N2}", summaryFont, summaryX, currentY, summaryWidth)
            currentY += 25
            DrawSummaryLine(e, "Total Discount:", $"₱{totalDiscount:N2}", summaryFont, summaryX, currentY, summaryWidth)
            currentY += 25
            DrawSummaryLine(e, "Total VAT:", $"₱{totalVAT:N2}", summaryFont, summaryX, currentY, summaryWidth)
            currentY += 25

            ' Draw net total with highlight
            Dim netTotalRect As New RectangleF(summaryX, currentY, summaryWidth, 30)
            e.Graphics.FillRectangle(Brushes.LightGray, netTotalRect)
            DrawSummaryLine(e, "Net Total:", $"₱{netTotal:N2}", New System.Drawing.Font("Arial", 12, FontStyle.Bold), summaryX, currentY, summaryWidth)

            ' Add footer
            currentY = e.MarginBounds.Bottom - 40
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

    ' Helper method to draw a summary line with label and value
    Private Sub DrawSummaryLine(e As PrintPageEventArgs, label As String, value As String, font As System.Drawing.Font, x As Single, y As Single, width As Single)
        Dim labelSize As SizeF = e.Graphics.MeasureString(label, font)
        e.Graphics.DrawString(label, font, Brushes.Black, x, y)

        Dim valueSize As SizeF = e.Graphics.MeasureString(value, font)
        e.Graphics.DrawString(value, font, Brushes.Black,
                           x + width - valueSize.Width, y)
    End Sub

    Private Sub SalesReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvSalesReport.BackgroundColor = Color.White
        dgvSalesReport.BorderStyle = BorderStyle.None
        dgvSalesReport.AllowUserToAddRows = False
        dgvSalesReport.ReadOnly = True
        dgvSalesReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvSalesReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvSalesReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvSalesReport.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvSalesReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvSalesReport.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvSalesReport.EnableHeadersVisualStyles = False
        dgvSalesReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvSalesReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        'dgvSalesReport.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvSalesReport.ColumnHeadersHeight = 35
    End Sub

    Private Sub btnClosePanel_Click(sender As Object, e As EventArgs) Handles btnClosePanel.Click
        Me.Close()
    End Sub
End Class
