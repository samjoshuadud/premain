Imports System.Data.SqlClient
Imports Microsoft.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class VoidedTransaction
    ' Connection string to your database
    Private connectionString As String = AppConfig.ConnectionString
    Private WithEvents printDocument As New PrintDocument()
    Private printPreviewDialog As New PrintPreviewDialog()

    ' Variables to track pagination for printing
    Private currentPage As Integer
    Private rowsPrinted As Integer

    Public Sub New()
        ' This call is required by the designer.
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
        btnExportPDF.Location = New Point(370, 30) ' Adjust position as needed
        AddHandler btnExportPDF.Click, AddressOf BtnExportPDF_Click
        Me.Controls.Add(btnExportPDF)

        ' Create Print button
        Dim btnPrint As New Button()
        btnPrint.Text = "Print Report"
        btnPrint.BackColor = Color.FromArgb(34, 139, 34)
        btnPrint.ForeColor = Color.White
        btnPrint.FlatStyle = FlatStyle.Flat
        btnPrint.Size = New Size(155, 42)
        btnPrint.Location = New Point(535, 30) ' Adjust position as needed
        AddHandler btnPrint.Click, AddressOf BtnPrint_Click
        Me.Controls.Add(btnPrint)
    End Sub

    ' Function to load OrderCancellation data with ProductName
    Public Function LoadOrderCancellation() As DataTable
        Dim dt As New DataTable()

        ' SQL query to fetch data from OrderCancellation and join with Products table
        Dim query As String = "
            SELECT 
                OC.OrderCancellationID, 
                OC.TransactionNumber, 
                P.ProductName,  -- Changed to ProductName
                OC.CancelledBy, 
                OC.VoidBy, 
                OC.CancelQuantity, 
                OC.AddToInventory, 
                OC.Reasons, 
                OC.CancelDate 
            FROM 
                OrderCancellation OC
            LEFT JOIN 
                Products P ON OC.ProductID = P.ProductID" ' Left join to get ProductName

        Using connection As New SqlConnection(connectionString)
            ' Create a command with the query
            Dim command As New SqlCommand(query, connection)

            Try
                ' Open connection
                connection.Open()

                ' Use data adapter to fill data into DataTable
                Dim adapter As New SqlDataAdapter(command)
                adapter.Fill(dt)

            Catch ex As Exception
                ' Handle any errors that may occur
                Console.WriteLine("Error: " & ex.Message)
            End Try
        End Using

        ' Return the filled DataTable
        Return dt
    End Function

    ' This method binds the data to the DataGridView when the form loads
    Private Sub VoidedTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load data into the DataTable
        Dim dt As DataTable = LoadOrderCancellation()

        ' Bind the DataTable to the DataGridView
        dgvVoidedTrasaction.DataSource = dt

        ' Hide the 'OrderCancellationID' column (this is still useful for internal reference, but not shown)
        If dgvVoidedTrasaction.Columns.Contains("OrderCancellationID") Then
            dgvVoidedTrasaction.Columns("OrderCancellationID").Visible = False
        End If

        ' Allow horizontal scrolling if the DataGridView is wider than the form
        dgvVoidedTrasaction.ScrollBars = ScrollBars.Horizontal

        ' Set AutoSize for columns for the entire DataGridView
        dgvVoidedTrasaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Set specific columns to AutoSizeMode for automatic resizing
        If dgvVoidedTrasaction.Columns.Contains("TransactionNumber") Then
            dgvVoidedTrasaction.Columns("TransactionNumber").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End If

        If dgvVoidedTrasaction.Columns.Contains("CancelDate") Then
            dgvVoidedTrasaction.Columns("CancelDate").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End If

        ' Set the 'ProductName' column width to 250
        If dgvVoidedTrasaction.Columns.Contains("ProductName") Then
            dgvVoidedTrasaction.Columns("ProductName").Width = 170
        End If

        ' Set a fixed width for 'CancelledBy' column
        If dgvVoidedTrasaction.Columns.Contains("CancelledBy") Then
            dgvVoidedTrasaction.Columns("CancelledBy").Width = 170 ' Adjust to your preferred width
        End If

        ' Set a fixed width for 'VoidBy' column
        If dgvVoidedTrasaction.Columns.Contains("VoidBy") Then
            dgvVoidedTrasaction.Columns("VoidBy").Width = 170 ' Adjust to your preferred width
        End If

        ' Set DataGridView background color to white
        dgvVoidedTrasaction.BackgroundColor = Color.White

        ' Remove the extra row (the row for adding new data)
        dgvVoidedTrasaction.AllowUserToAddRows = False

        ' Disable the blue highlight when a row is selected
        dgvVoidedTrasaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVoidedTrasaction.DefaultCellStyle.SelectionBackColor = dgvVoidedTrasaction.BackgroundColor
        dgvVoidedTrasaction.DefaultCellStyle.SelectionForeColor = dgvVoidedTrasaction.ForeColor
        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        ' Set DataGridView properties for better appearance
        dgvVoidedTrasaction.BackgroundColor = Color.White
        dgvVoidedTrasaction.BorderStyle = BorderStyle.None
        dgvVoidedTrasaction.AllowUserToAddRows = False
        dgvVoidedTrasaction.ReadOnly = True
        dgvVoidedTrasaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Disable the blue highlight when a row is selected
        dgvVoidedTrasaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVoidedTrasaction.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 230, 250)
        dgvVoidedTrasaction.DefaultCellStyle.SelectionForeColor = Color.Black

        ' Set alternating row colors
        dgvVoidedTrasaction.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvVoidedTrasaction.DefaultCellStyle.BackColor = Color.White

        ' Customize header appearance
        dgvVoidedTrasaction.EnableHeadersVisualStyles = False
        dgvVoidedTrasaction.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 153, 255)
        dgvVoidedTrasaction.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        'dgvVoidedTrasaction.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvVoidedTrasaction.ColumnHeadersHeight = 35
    End Sub

    ' Event handler for PDF export button
    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        Dim dt As DataTable = CType(dgvVoidedTrasaction.DataSource, DataTable)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MessageBox.Show("No data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Create a SaveFileDialog to get the file save location
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
        saveDialog.Title = "Save Voided Transactions Report as PDF"
        saveDialog.FileName = $"VoidedTransactions_{DateTime.Now:yyyyMMdd}.pdf"

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
        ' Create a new PDF document
        Dim document As New Document(PageSize.A4.Rotate(), 40, 40, 40, 40) ' Landscape orientation

        Try
            Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(filePath, FileMode.Create))

            ' Open the document for writing
            document.Open()

            ' Add a title
            Dim titleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 16, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim title As New Paragraph("FCM SUPERMARKET", titleFont)
            title.Alignment = Element.ALIGN_CENTER
            document.Add(title)

            ' Add report title
            Dim reportTitleFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 14, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim reportTitle As New Paragraph("Voided Transactions Report", reportTitleFont)
            reportTitle.Alignment = Element.ALIGN_CENTER
            document.Add(reportTitle)

            ' Add date
            Dim dateFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim dateText As New Paragraph($"Report Date: {DateTime.Now.ToString("MMMM dd, yyyy")}", dateFont)
            dateText.Alignment = Element.ALIGN_CENTER
            dateText.SpacingAfter = 20
            document.Add(dateText)

            ' Get columns (excluding OrderCancellationID if it exists)
            Dim columns As New List(Of String)
            For Each column As DataColumn In dataTable.Columns
                If column.ColumnName <> "OrderCancellationID" Then
                    columns.Add(column.ColumnName)
                End If
            Next

            ' Create a table
            Dim table As New PdfPTable(columns.Count)
            table.WidthPercentage = 100
            table.SpacingBefore = 10

            ' Set relative widths based on content - adjusted for better proportions
            Dim widths(columns.Count - 1) As Single
            For i As Integer = 0 To columns.Count - 1
                Select Case columns(i)
                    Case "TransactionNumber"
                        widths(i) = 2.0F
                    Case "ProductName"
                        widths(i) = 2.0F
                    Case "CancelledBy", "VoidBy"
                        widths(i) = 1.5F
                    Case "CancelQuantity"
                        widths(i) = 0.8F
                    Case "AddToInventory"
                        widths(i) = 0.8F
                    Case "Reasons"
                        widths(i) = 2.5F
                    Case "CancelDate"
                        widths(i) = 1.5F
                    Case Else
                        widths(i) = 1.0F
                End Select
            Next
            table.SetWidths(widths)

            ' Add table headers
            Dim headerFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10, iTextSharp.text.Font.NORMAL, BaseColor.White)
            Dim cellBackColor As New BaseColor(0, 0, 0) ' Black background for header

            For Each columnName As String In columns
                Dim cell As New PdfPCell(New Phrase(columnName, headerFont))
                cell.BackgroundColor = cellBackColor
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.VerticalAlignment = Element.ALIGN_MIDDLE
                cell.Padding = 5
                table.AddCell(cell)
            Next

            ' Add table data
            Dim cellFont As New Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9, iTextSharp.text.Font.NORMAL, BaseColor.Black)
            Dim altCellBackColor As New BaseColor(240, 240, 240) ' Light gray for alternating rows

            For i As Integer = 0 To dataTable.Rows.Count - 1
                Dim row As DataRow = dataTable.Rows(i)
                Dim backColor As BaseColor = If(i Mod 2 = 0, BaseColor.White, altCellBackColor)

                For j As Integer = 0 To columns.Count - 1
                    Dim columnName As String = columns(j)
                    If columnName <> "OrderCancellationID" Then
                        Dim cellValue As String = FormatCellValue(row, columnName)

                        Dim cell As New PdfPCell(New Phrase(cellValue, cellFont))
                        cell.BackgroundColor = backColor

                        ' Set appropriate alignment based on column type
                        If columnName = "CancelQuantity" Then
                            cell.HorizontalAlignment = Element.ALIGN_CENTER
                        ElseIf columnName = "AddToInventory" Then
                            cell.HorizontalAlignment = Element.ALIGN_CENTER
                        ElseIf columnName = "CancelDate" Then
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = Element.ALIGN_LEFT
                        End If

                        cell.VerticalAlignment = Element.ALIGN_MIDDLE
                        cell.Padding = 5
                        table.AddCell(cell)
                    End If
                Next
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
        Dim dt As DataTable = CType(dgvVoidedTrasaction.DataSource, DataTable)

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
        Dim dt As DataTable = CType(dgvVoidedTrasaction.DataSource, DataTable)

        ' Page title and header
        Dim titleFont As New System.Drawing.Font("Arial", 16, FontStyle.Bold)
        Dim headerFont As New System.Drawing.Font("Arial", 11, FontStyle.Bold)
        Dim normalFont As New System.Drawing.Font("Arial", 9)
        Dim smallFont As New System.Drawing.Font("Arial", 8)

        ' Print margins and positions - optimize for landscape
        Dim leftMargin As Single = e.MarginBounds.Left + 20 ' Add padding from edge
        Dim rightMargin As Single = e.MarginBounds.Right - 20
        Dim topMargin As Single = e.MarginBounds.Top + 10
        Dim width As Single = rightMargin - leftMargin
        Dim currentY As Single = topMargin

        ' Print store name and report title
        Dim titleText As String = "SHIENNA'S MINI GROCERY STORE"
        Dim titleSize As SizeF = e.Graphics.MeasureString(titleText, titleFont)
        e.Graphics.DrawString(titleText, titleFont, Brushes.Black,
                           leftMargin + (width / 2) - (titleSize.Width / 2), currentY)
        currentY += 30

        Dim reportText As String = "Voided Transactions Report"
        Dim reportSize As SizeF = e.Graphics.MeasureString(reportText, headerFont)
        e.Graphics.DrawString(reportText, headerFont, Brushes.Black,
                           leftMargin + (width / 2) - (reportSize.Width / 2), currentY)
        currentY += 25

        ' Print report date
        Dim dateText As String = $"Report Date: {DateTime.Now.ToString("MMMM dd, yyyy")}"
        Dim dateSize As SizeF = e.Graphics.MeasureString(dateText, normalFont)
        e.Graphics.DrawString(dateText, normalFont, Brushes.Black,
                           leftMargin + (width / 2) - (dateSize.Width / 2), currentY)
        currentY += 30 ' Increased spacing

        ' Get columns (excluding OrderCancellationID)
        Dim columns As New List(Of String)
        For Each column As DataColumn In dt.Columns
            If column.ColumnName <> "OrderCancellationID" Then
                columns.Add(column.ColumnName)
            End If
        Next

        ' Define column widths based on content - using relative proportions optimized for landscape
        Dim columnWidths As New Dictionary(Of String, Single)
        Dim totalWidthUnits As Single = 12.0F ' Total units to distribute

        ' Set column width ratios for landscape
        columnWidths.Add("TransactionNumber", 2.0F)
        columnWidths.Add("ProductName", 1.6F)
        columnWidths.Add("CancelledBy", 1.6F)
        columnWidths.Add("VoidBy", 1.6F)
        columnWidths.Add("CancelQuantity", 0.8F)
        columnWidths.Add("AddToInventory", 0.8F)
        columnWidths.Add("Reasons", 2.1F)
        columnWidths.Add("CancelDate", 1.5F)

        ' Calculate actual widths
        Dim columnPositions As New Dictionary(Of String, Single)
        Dim currentX As Single = leftMargin
        columnPositions.Add("Start", currentX)

        For Each columnName In columns
            Dim widthRatio As Single = 1.0F ' Default ratio
            If columnWidths.ContainsKey(columnName) Then
                widthRatio = columnWidths(columnName)
            End If

            Dim columnWidth As Single = (width * widthRatio) / totalWidthUnits
            currentX += columnWidth
            columnPositions.Add(columnName, currentX)
        Next

        ' Table header positions
        Dim cellPadding As Single = 5
        Dim rowHeight As Single = 25

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
            e.Graphics.DrawString(columnName, headerFont, Brushes.White, textX, currentY + cellPadding - 2) ' Adjusted for better vertical centering
        Next
        currentY += rowHeight

        ' Start from rowsPrinted (for pagination)
        Dim rowsPerPage As Integer = 28 ' Increased rows per page for landscape
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
                If columnName = "CancelQuantity" Or columnName = "AddToInventory" Then
                    ' Center align
                    textX = columnStart + (columnWidth / 2) - (textSize.Width / 2)
                ElseIf columnName = "CancelDate" Then
                    ' Right align
                    textX = columnEnd - textSize.Width - cellPadding
                End If

                ' Draw cell text
                e.Graphics.DrawString(cellValue, normalFont, Brushes.Black, textX, currentY + cellPadding)
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

    ' Helper method to safely format cell values for display
    Private Function FormatCellValue(row As DataRow, columnName As String) As String
        If IsDBNull(row(columnName)) Then Return ""

        Try
            Select Case columnName
                Case "CancelDate"
                    ' Format DateTime values
                    If TypeOf row(columnName) Is DateTime Then
                        Return Convert.ToDateTime(row(columnName)).ToString("MM/dd/yyyy HH:mm:ss")
                    Else
                        Return row(columnName).ToString()
                    End If

                Case "AddToInventory"
                    ' Safely handle boolean-like values
                    Dim strValue As String = row(columnName).ToString().Trim().ToLower()

                    If TypeOf row(columnName) Is Boolean Then
                        Return If(CBool(row(columnName)), "Yes", "No")
                    ElseIf strValue = "true" OrElse strValue = "1" OrElse strValue = "yes" Then
                        Return "Yes"
                    ElseIf strValue = "false" OrElse strValue = "0" OrElse strValue = "no" Then
                        Return "No"
                    Else
                        Return row(columnName).ToString()
                    End If

                Case Else
                    ' Default handling for all other columns
                    Return row(columnName).ToString()
            End Select
        Catch ex As Exception
            ' Fallback for any exceptions
            Return If(row(columnName) IsNot Nothing, row(columnName).ToString(), "")
        End Try
    End Function

    Private Sub btnClosePanel_Click(sender As Object, e As EventArgs) Handles btnClosePanel.Click
        Me.Close()
    End Sub
End Class
