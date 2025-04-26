<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FastMovingProduct
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

    'UI Controls Declaration
    Private WithEvents dgvFastMovingProduct As DataGridView
    Private WithEvents btnGenerateReport As Button
    Private startDatePicker As DateTimePicker
    Private endDatePicker As DateTimePicker
    Private lblStartDate As Label
    Private lblEndDate As Label

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvFastMovingProduct = New DataGridView()
        lblStartDate = New Label()
        startDatePicker = New DateTimePicker()
        lblEndDate = New Label()
        endDatePicker = New DateTimePicker()
        btnGenerateReport = New Button()
        CType(dgvFastMovingProduct, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvFastMovingProduct
        ' 
        dgvFastMovingProduct.ColumnHeadersHeight = 34
        dgvFastMovingProduct.Location = New Point(12, 103)
        dgvFastMovingProduct.Name = "dgvFastMovingProduct"
        dgvFastMovingProduct.RowHeadersWidth = 62
        dgvFastMovingProduct.Size = New Size(1723, 791)
        dgvFastMovingProduct.TabIndex = 0
        ' 
        ' lblStartDate
        ' 
        lblStartDate.Location = New Point(649, 28)
        lblStartDate.Name = "lblStartDate"
        lblStartDate.Size = New Size(100, 23)
        lblStartDate.TabIndex = 1
        lblStartDate.Text = "Start Date:"
        ' 
        ' startDatePicker
        ' 
        startDatePicker.Format = DateTimePickerFormat.Short
        startDatePicker.Location = New Point(755, 28)
        startDatePicker.Name = "startDatePicker"
        startDatePicker.Size = New Size(200, 31)
        startDatePicker.TabIndex = 2
        ' 
        ' lblEndDate
        ' 
        lblEndDate.Location = New Point(961, 26)
        lblEndDate.Name = "lblEndDate"
        lblEndDate.Size = New Size(100, 23)
        lblEndDate.TabIndex = 3
        lblEndDate.Text = "End Date:"
        ' 
        ' endDatePicker
        ' 
        endDatePicker.Format = DateTimePickerFormat.Short
        endDatePicker.Location = New Point(1067, 26)
        endDatePicker.Name = "endDatePicker"
        endDatePicker.Size = New Size(200, 31)
        endDatePicker.TabIndex = 4
        ' 
        ' btnGenerateReport
        ' 
        btnGenerateReport.Location = New Point(1273, 23)
        btnGenerateReport.Name = "btnGenerateReport"
        btnGenerateReport.Size = New Size(160, 40)
        btnGenerateReport.TabIndex = 5
        btnGenerateReport.Text = "Generate Report"
        ' 
        ' FastMovingProduct
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1747, 906)
        Controls.Add(dgvFastMovingProduct)
        Controls.Add(lblStartDate)
        Controls.Add(startDatePicker)
        Controls.Add(lblEndDate)
        Controls.Add(endDatePicker)
        Controls.Add(btnGenerateReport)
        Name = "FastMovingProduct"
        Text = "FastMovingProduct"
        CType(dgvFastMovingProduct, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub


End Class
