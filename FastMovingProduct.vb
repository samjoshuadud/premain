Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient
Imports System.Globalization
Imports System.Drawing.Printing
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class FastMovingProduct
    Private connectionString As String = AppConfig.ConnectionString
    Private WithEvents printDocument As New PrintDocument()
    Private printPreviewDialog As New PrintPreviewDialog()
    Private currentPage As Integer
    Private rowsPrinted As Integer
    Private startDateValue As DateTime
    Private endDateValue As DateTime

    Public Sub New()
        InitializeComponent()
        
        ' Configure print preview dialog
        printPreviewDialog.Document = printDocument
        
        ' Add Print and Export buttons
        AddPrintExportButtons()
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
        btnExportPDF.Location = New Point(btnGenerateReport.Location.X + btnGenerateReport.Width + 10, btnGenerateReport.Location.Y) ' Position to right of Generate Report button
        AddHandler btnExportPDF.Click, AddressOf BtnExportPDF_Click
        Me.Controls.Add(btnExportPDF)
        
        ' Create Print button
        Dim btnPrint As New Button()
        btnPrint.Text = "Print Report"
        btnPrint.BackColor = Color.FromArgb(34, 139, 34)
        btnPrint.ForeColor = Color.White
        btnPrint.FlatStyle = FlatStyle.Flat
        btnPrint.Size = New Size(155, 42)
        btnPrint.Location = New Point(btnExportPDF.Location.X + btnExportPDF.Width + 10, btnGenerateReport.Location.Y) ' Position to right of Export PDF button
        AddHandler btnPrint.Click, AddressOf BtnPrint_Click
        Me.Controls.Add(btnPrint)
    End Sub

    ' This method gets the top selling products for the current month
    Public Sub GetFastMovingProducts(Optional startDate As DateTime? = Nothing, Optional endDate As DateTime? = Nothing)
        ' Create a connection to the database
        Using connection As New SqlConnection(connectionString)
            Try
                ' Open the connection
                connection.Open()

                ' If no date range is provided, use the default current month range
                If Not startDate.HasValue Then
                    startDateValue = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                Else
                    startDateValue = startDate.Value
                End If
                If Not endDate.HasValue Then
                    endDateValue = startDateValue.AddMonths(1).AddDays(-1)
                Else
                    endDateValue = endDate.Value
                End If

                ' SQL query to calculate the total quantity sold for each product and total sales value
                Dim query As String = "
                SELECT p.ProductName, 
                       SUM(si.Quantity) AS TotalSold, 
                       FORMAT(SUM(si.Quantity * si.UnitPrice), 'C', 'en-PH') AS TotalSalesValue,
                       c.CategoryName,
                       s.SupplierName
                FROM SaleItems si
                INNER JOIN Products p ON si.ProductID = p.ProductID
                INNER JOIN Sales sa ON si.SaleID = sa.SaleID
                LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                WHERE sa.SaleDate BETWEEN @StartDate AND @EndDate
                GROUP BY p.ProductName, c.CategoryName, s.SupplierName
                ORDER BY TotalSold DESC
            "

                ' Create the command with parameters to prevent SQL injection
                Dim command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@StartDate", startDateValue)
                command.Parameters.AddWithValue("@EndDate", endDateValue)

                ' Execute the query and get the data
                Dim reader As SqlDataReader = command.ExecuteReader()

                ' Clear the DataGridView before loading new data
                dgvFastMovingProduct.Rows.Clear()

                ' Add columns to the DataGridView if not already added
                If dgvFastMovingProduct.Columns.Count = 0 Then
                    dgvFastMovingProduct.Columns.Add("ProductName", "Product Name")
                    dgvFastMovingProduct.Columns.Add("SupplierName", "Supplier")
                    dgvFastMovingProduct.Columns.Add("CategoryName", "Category")
                    dgvFastMovingProduct.Columns.Add("TotalSold", "Total Sold")
                    dgvFastMovingProduct.Columns.Add("TotalSalesValue", "Total Sales Value")
                End If

                ' Process the results and load into the DataGridView
                While reader.Read()
                    ' Add each row into the DataGridView
                    dgvFastMovingProduct.Rows.Add(reader("ProductName").ToString(),
                                                   reader("SupplierName").ToString(),
                                                   reader("CategoryName").ToString(),
                                                   reader("TotalSold").ToString(),
                                                   reader("TotalSalesValue").ToString())
                End While

                ' Automatically adjust column widths to fill available space
                dgvFastMovingProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                ' Manually adjust specific columns if needed
                dgvFastMovingProduct.Columns("TotalSold").Width = 120
                dgvFastMovingProduct.Columns("TotalSalesValue").Width = 150

            Catch ex As Exception
                ' Handle any errors that occur during the database connection or query execution
                MessageBox.Show("Error: " & ex.Message)
            Finally
                ' Ensure the connection is closed even if there was an error
                connection.Close()
            End Try
        End Using
    End Sub

    ' When the form loads, generate the fast-moving product report automatically
    Private Sub FastMovingProduct_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Call GetFastMovingProducts to load the data when the form is loaded
        GetFastMovingProducts()
        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvFastMovingProduct.BackgroundColor = Color.White
        dgvFastMovingProduct.BorderStyle = BorderStyle.None
        dgvFastMovingProduct.AllowUserToAddRows = False
        dgvFastMovingProduct.ReadOnly = True
        dgvFastMovingProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvFastMovingProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvFastMovingProduct.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvFastMovingProduct.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvFastMovingProduct.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvFastMovingProduct.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvFastMovingProduct.EnableHeadersVisualStyles = False
        dgvFastMovingProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvFastMovingProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        'dgvFastMovingProduct.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvFastMovingProduct.ColumnHeadersHeight = 35
    End Sub

    ' Button click event to generate the report manually
    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' Call GetFastMovingProducts when the report button is clicked
        GetFastMovingProducts()
    End Sub
    
    ' Export to PDF button click event handler
    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        If dgvFastMovingProduct.Rows.Count = 0 Then
            MessageBox.Show("No data to export. Please generate a report first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Create a SaveFileDialog to get the file save location
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
        saveDialog.Title = "Save Fast Moving Products Report as PDF"
        saveDialog.FileName = $"FastMovingProducts_{startDateValue:yyyyMMdd}_{endDateValue:yyyyMMdd}.pdf"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            Try
                ExportToPDF(saveDialog.FileName)
                MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error exporting to PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    
    ' Method to export the fast moving products data to PDF
    Private Sub ExportToPDF(filePath As String)
        ' Create a new PDF document
        Dim document As New Document(PageSize.A4, 40, 40, 40, 40)
        
        Try
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))

            ' Open the document for writing
            document.Open()

            ' Add a title
            Dim titleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK)
            Dim title As New Paragraph("FCM SUPERMARKET", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            document.Add(title)

            ' Add report title
            Dim reportTitleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK)
            Dim reportTitle As New Paragraph("Fast Moving Products Report", reportTitleFont)
            reportTitle.Alignment = Element.ALIGN_CENTER
            document.Add(reportTitle)
            
            ' Add date range
            Dim dateRangeFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)
            Dim dateRange As New Paragraph($"Date Range: {startDateValue.ToString("MMMM dd, yyyy")} - {endDateValue.ToString("MMMM dd, yyyy")}", dateRangeFont)
            dateRange.Alignment = Element.ALIGN_CENTER
            dateRange.SpacingAfter = 20
            document.Add(dateRange)

            ' Create a table for the data
            Dim table As New PdfPTable(5) ' 5 columns for ProductName, SupplierName, CategoryName, TotalSold, TotalSalesValue
            table.WidthPercentage = 100
            table.SetWidths(New Single() {2.5F, 1.5F, 1.5F, 1.0F, 1.5F}) ' Adjust column widths

            ' Add table headers
            Dim headerFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE)
            Dim cellBackColor As New BaseColor(0, 0, 0) ' Black background for header

            ' Add table header cells
            AddHeaderCell(table, "Product Name", headerFont, cellBackColor)
            AddHeaderCell(table, "Supplier", headerFont, cellBackColor)
            AddHeaderCell(table, "Category", headerFont, cellBackColor)
            AddHeaderCell(table, "Total Sold", headerFont, cellBackColor)
            AddHeaderCell(table, "Total Sales Value", headerFont, cellBackColor)

            ' Add table data
            Dim cellFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)
            Dim altCellBackColor As New BaseColor(240, 240, 240) ' Light gray for alternating rows

            Dim totalQuantitySold As Integer = 0
            Dim totalSalesValue As Decimal = 0

            For i As Integer = 0 To dgvFastMovingProduct.Rows.Count - 1
                If i < dgvFastMovingProduct.Rows.Count Then
                    Dim row As DataGridViewRow = dgvFastMovingProduct.Rows(i)
                    Dim backColor As BaseColor = If(i Mod 2 = 0, BaseColor.WHITE, altCellBackColor)

                    ' Add cells with data
                    AddCell(table, row.Cells("ProductName").Value.ToString(), cellFont, backColor)
                    AddCell(table, row.Cells("SupplierName").Value.ToString(), cellFont, backColor)
                    AddCell(table, row.Cells("CategoryName").Value.ToString(), cellFont, backColor)
                    
                    ' Total Sold (right-aligned)
                    AddCell(table, row.Cells("TotalSold").Value.ToString(), cellFont, backColor, Element.ALIGN_RIGHT)
                    
                    ' Total Sales Value (right-aligned)
                    AddCell(table, row.Cells("TotalSalesValue").Value.ToString(), cellFont, backColor, Element.ALIGN_RIGHT)
                    
                    ' Calculate totals if possible
                    Integer.TryParse(row.Cells("TotalSold").Value.ToString().Replace(",", ""), totalQuantitySold)
                End If
            Next

            ' Add the table to the document
            document.Add(table)

            ' Add footer with generation date
            Dim footer As New Paragraph($"Report generated on {DateTime.Now.ToString("MMMM dd, yyyy HH:mm:ss")}", 
                                       New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8, iTextSharp.text.Font.ITALIC, BaseColor.GRAY))
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
        If dgvFastMovingProduct.Rows.Count = 0 Then
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
        
        Dim reportText As String = "Fast Moving Products Report"
        Dim reportSize As SizeF = e.Graphics.MeasureString(reportText, headerFont)
        e.Graphics.DrawString(reportText, headerFont, Brushes.Black, 
                           leftMargin + (width / 2) - (reportSize.Width / 2), currentY)
        currentY += 25

        ' Print date range
        Dim dateRangeText As String = $"Date Range: {startDateValue.ToString("MMMM dd, yyyy")} - {endDateValue.ToString("MMMM dd, yyyy")}"
        Dim dateRangeSize As SizeF = e.Graphics.MeasureString(dateRangeText, normalFont)
        e.Graphics.DrawString(dateRangeText, normalFont, Brushes.Black, 
                           leftMargin + (width / 2) - (dateRangeSize.Width / 2), currentY)
        currentY += 25

        ' Table header positions
        Dim columnCount As Integer = 5 ' ProductName, SupplierName, CategoryName, TotalSold, TotalSalesValue
        Dim columnWidths As Single() = {width * 0.3F, width * 0.2F, width * 0.2F, width * 0.1F, width * 0.2F} ' Proportional widths
        Dim cellPadding As Single = 5
        Dim rowHeight As Single = 25

        ' Draw table headers with black background
        Dim headerRect As New RectangleF(leftMargin, currentY, width, rowHeight)
        e.Graphics.FillRectangle(Brushes.Black, headerRect)

        ' Draw header text in white
        Dim columnHeaderTexts As String() = {"Product Name", "Supplier", "Category", "Total Sold", "Total Sales Value"}
        Dim currentX As Single = leftMargin
        
        For i As Integer = 0 To columnHeaderTexts.Length - 1
            Dim headerText As String = columnHeaderTexts(i)
            e.Graphics.DrawString(headerText, headerFont, Brushes.White, currentX + cellPadding, currentY + cellPadding)
            currentX += columnWidths(i)
        Next
        
        currentY += rowHeight

        ' Start from rowsPrinted (for pagination)
        Dim rowsPerPage As Integer = 20 ' Approximate rows per page
        Dim endRow As Integer = Math.Min(dgvFastMovingProduct.Rows.Count - 1, rowsPrinted + rowsPerPage - 1)

        Try
            For i As Integer = rowsPrinted To endRow
                ' Alternate row colors
                If i Mod 2 = 0 Then
                    Dim rowRect As New RectangleF(leftMargin, currentY, width, rowHeight)
                    e.Graphics.FillRectangle(Brushes.LightGray, rowRect)
                End If

                ' Draw cell values for each row
                If i < dgvFastMovingProduct.Rows.Count Then
                    Dim row As DataGridViewRow = dgvFastMovingProduct.Rows(i)

                    ' Reset X position for each row
                    currentX = leftMargin

                    ' Product Name
                    Dim productName As String = SafeGetCellValue(row, "ProductName")
                    e.Graphics.DrawString(productName, normalFont, Brushes.Black,
                                        currentX + cellPadding, currentY + cellPadding)
                    currentX += columnWidths(0)

                    ' Supplier
                    Dim supplierName As String = SafeGetCellValue(row, "SupplierName")
                    e.Graphics.DrawString(supplierName, normalFont, Brushes.Black,
                                        currentX + cellPadding, currentY + cellPadding)
                    currentX += columnWidths(1)

                    ' Category
                    Dim categoryName As String = SafeGetCellValue(row, "CategoryName")
                    e.Graphics.DrawString(categoryName, normalFont, Brushes.Black,
                                        currentX + cellPadding, currentY + cellPadding)
                    currentX += columnWidths(2)

                    ' Total Sold (right-aligned)
                    Dim totalSoldText As String = SafeGetCellValue(row, "TotalSold")
                    Dim totalSoldSize As SizeF = e.Graphics.MeasureString(totalSoldText, normalFont)
                    e.Graphics.DrawString(totalSoldText, normalFont, Brushes.Black,
                                        currentX + columnWidths(3) - totalSoldSize.Width - cellPadding, currentY + cellPadding)
                    currentX += columnWidths(3)

                    ' Total Sales Value (right-aligned)
                    Dim salesValueText As String = SafeGetCellValue(row, "TotalSalesValue")
                    Dim salesValueSize As SizeF = e.Graphics.MeasureString(salesValueText, normalFont)
                    e.Graphics.DrawString(salesValueText, normalFont, Brushes.Black,
                                        currentX + columnWidths(4) - salesValueSize.Width - cellPadding, currentY + cellPadding)
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
            If rowsPrinted >= dgvFastMovingProduct.Rows.Count Then
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
End Class
