<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SalesReports
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'Note: The following procedure is required by the Windows Form Designer.
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SalesReports))
        dtpStartDate = New DateTimePicker()
        dtpEndDate = New DateTimePicker()
        btnGenerateReport = New Button()
        dgvSalesReport = New DataGridView()
        pnlSummary = New Panel()
        lblNetTotal = New Label()
        lblTotalVAT = New Label()
        lblTotalDiscount = New Label()
        lblTotalSales = New Label()
        PanelCategory = New Panel()
        Label2 = New Label()
        Label1 = New Label()
        btnClosePanel = New Button()
        CType(dgvSalesReport, ComponentModel.ISupportInitialize).BeginInit()
        pnlSummary.SuspendLayout()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' dtpStartDate
        ' 
        dtpStartDate.CalendarFont = New Font("Segoe UI", 12F)
        dtpStartDate.Format = DateTimePickerFormat.Short
        dtpStartDate.Location = New Point(141, 91)
        dtpStartDate.Name = "dtpStartDate"
        dtpStartDate.Size = New Size(207, 31)
        dtpStartDate.TabIndex = 0
        ' 
        ' dtpEndDate
        ' 
        dtpEndDate.CalendarFont = New Font("Segoe UI", 12F)
        dtpEndDate.Format = DateTimePickerFormat.Short
        dtpEndDate.Location = New Point(354, 91)
        dtpEndDate.Name = "dtpEndDate"
        dtpEndDate.Size = New Size(207, 31)
        dtpEndDate.TabIndex = 1
        ' 
        ' btnGenerateReport
        ' 
        btnGenerateReport.BackColor = SystemColors.ActiveCaptionText
        btnGenerateReport.FlatStyle = FlatStyle.Flat
        btnGenerateReport.ForeColor = SystemColors.ControlLightLight
        btnGenerateReport.Location = New Point(567, 87)
        btnGenerateReport.Name = "btnGenerateReport"
        btnGenerateReport.Size = New Size(155, 42)
        btnGenerateReport.TabIndex = 2
        btnGenerateReport.Text = "Generate Report"
        btnGenerateReport.UseVisualStyleBackColor = False
        ' 
        ' dgvSalesReport
        ' 
        dgvSalesReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvSalesReport.Location = New Point(12, 135)
        dgvSalesReport.Name = "dgvSalesReport"
        dgvSalesReport.RowHeadersWidth = 62
        dgvSalesReport.RowTemplate.Height = 24
        dgvSalesReport.Size = New Size(1745, 713)
        dgvSalesReport.TabIndex = 3
        ' 
        ' pnlSummary
        ' 
        pnlSummary.Controls.Add(lblNetTotal)
        pnlSummary.Controls.Add(lblTotalVAT)
        pnlSummary.Controls.Add(lblTotalDiscount)
        pnlSummary.Controls.Add(lblTotalSales)
        pnlSummary.Font = New Font("Microsoft Sans Serif", 8.25F)
        pnlSummary.Location = New Point(12, 854)
        pnlSummary.Name = "pnlSummary"
        pnlSummary.Size = New Size(1712, 96)
        pnlSummary.TabIndex = 4
        ' 
        ' lblNetTotal
        ' 
        lblNetTotal.AutoSize = True
        lblNetTotal.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblNetTotal.Location = New Point(1386, 46)
        lblNetTotal.Name = "lblNetTotal"
        lblNetTotal.Size = New Size(120, 30)
        lblNetTotal.TabIndex = 3
        lblNetTotal.Text = "Net Total: "
        ' 
        ' lblTotalVAT
        ' 
        lblTotalVAT.AutoSize = True
        lblTotalVAT.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblTotalVAT.Location = New Point(1384, 16)
        lblTotalVAT.Name = "lblTotalVAT"
        lblTotalVAT.Size = New Size(122, 30)
        lblTotalVAT.TabIndex = 2
        lblTotalVAT.Text = "Total VAT: "
        ' 
        ' lblTotalDiscount
        ' 
        lblTotalDiscount.AutoSize = True
        lblTotalDiscount.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblTotalDiscount.Location = New Point(1038, 46)
        lblTotalDiscount.Name = "lblTotalDiscount"
        lblTotalDiscount.Size = New Size(173, 30)
        lblTotalDiscount.TabIndex = 1
        lblTotalDiscount.Text = "Total Discount: "
        ' 
        ' lblTotalSales
        ' 
        lblTotalSales.AutoSize = True
        lblTotalSales.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblTotalSales.Location = New Point(1038, 16)
        lblTotalSales.Name = "lblTotalSales"
        lblTotalSales.Size = New Size(134, 30)
        lblTotalSales.TabIndex = 0
        lblTotalSales.Text = "Total Sales: "
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label2)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(249, 56)
        PanelCategory.TabIndex = 37
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label2.Location = New Point(20, 13)
        Label2.Name = "Label2"
        Label2.Size = New Size(201, 32)
        Label2.TabIndex = 0
        Label2.Text = "SALES REPORTS"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(12, 90)
        Label1.Name = "Label1"
        Label1.Size = New Size(123, 32)
        Label1.TabIndex = 4
        Label1.Text = "FILTER BY"
        ' 
        ' btnClosePanel
        ' 
        btnClosePanel.BackColor = Color.Transparent
        btnClosePanel.FlatAppearance.BorderSize = 0
        btnClosePanel.FlatStyle = FlatStyle.Flat
        btnClosePanel.ForeColor = Color.Transparent
        btnClosePanel.Image = CType(resources.GetObject("btnClosePanel.Image"), Image)
        btnClosePanel.Location = New Point(1691, 12)
        btnClosePanel.Name = "btnClosePanel"
        btnClosePanel.Size = New Size(66, 50)
        btnClosePanel.TabIndex = 95
        btnClosePanel.UseVisualStyleBackColor = False
        ' 
        ' SalesReports
        ' 
        ClientSize = New Size(1769, 962)
        Controls.Add(btnClosePanel)
        Controls.Add(Label1)
        Controls.Add(PanelCategory)
        Controls.Add(pnlSummary)
        Controls.Add(dgvSalesReport)
        Controls.Add(btnGenerateReport)
        Controls.Add(dtpEndDate)
        Controls.Add(dtpStartDate)
        FormBorderStyle = FormBorderStyle.None
        Name = "SalesReports"
        Text = "Sales Reports"
        CType(dgvSalesReport, ComponentModel.ISupportInitialize).EndInit()
        pnlSummary.ResumeLayout(False)
        pnlSummary.PerformLayout()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    ' Declare controls
    Friend WithEvents dtpStartDate As DateTimePicker
    Friend WithEvents dtpEndDate As DateTimePicker
    Friend WithEvents btnGenerateReport As Button
    Friend WithEvents dgvSalesReport As DataGridView
    Friend WithEvents pnlSummary As Panel
    Friend WithEvents lblTotalSales As Label
    Friend WithEvents lblTotalDiscount As Label
    Friend WithEvents lblTotalVAT As Label
    Friend WithEvents lblNetTotal As Label
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnClosePanel As Button

End Class
