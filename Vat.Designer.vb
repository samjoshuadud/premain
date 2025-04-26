<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Vat
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Vat))
        dgvVat = New DataGridView()
        txtVatRate = New TextBox()
        btnEdit = New Button()
        btnDelete = New Button()
        btnAdd = New Button()
        VatPanel = New Panel()
        Button3 = New Button()
        Button1 = New Button()
        Label1 = New Label()
        DateTimePicker1 = New DateTimePicker()
        lblSupplierName = New Label()
        Button2 = New Button()
        btnCLose = New Button()
        Panel1 = New Panel()
        Label3 = New Label()
        CType(dgvVat, ComponentModel.ISupportInitialize).BeginInit()
        VatPanel.SuspendLayout()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvVat
        ' 
        dgvVat.BackgroundColor = SystemColors.ButtonHighlight
        dgvVat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvVat.Location = New Point(12, 78)
        dgvVat.Name = "dgvVat"
        dgvVat.RowHeadersWidth = 62
        dgvVat.Size = New Size(871, 617)
        dgvVat.TabIndex = 0
        ' 
        ' txtVatRate
        ' 
        txtVatRate.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtVatRate.Location = New Point(168, 68)
        txtVatRate.Name = "txtVatRate"
        txtVatRate.Size = New Size(530, 39)
        txtVatRate.TabIndex = 1
        ' 
        ' btnEdit
        ' 
        btnEdit.BackColor = SystemColors.ActiveCaptionText
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.ForeColor = SystemColors.ButtonHighlight
        btnEdit.Location = New Point(390, 116)
        btnEdit.Margin = New Padding(5, 6, 5, 6)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(149, 50)
        btnEdit.TabIndex = 16
        btnEdit.Text = "EDIT"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' btnDelete
        ' 
        btnDelete.BackColor = SystemColors.ActiveCaptionText
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.ForeColor = SystemColors.ButtonHighlight
        btnDelete.Location = New Point(8, 317)
        btnDelete.Margin = New Padding(5, 6, 5, 6)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(149, 50)
        btnDelete.TabIndex = 15
        btnDelete.Text = "DELETE"
        btnDelete.UseVisualStyleBackColor = False
        ' 
        ' btnAdd
        ' 
        btnAdd.BackColor = SystemColors.ActiveCaptionText
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.ForeColor = SystemColors.ButtonHighlight
        btnAdd.Location = New Point(231, 116)
        btnAdd.Margin = New Padding(5, 6, 5, 6)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(149, 50)
        btnAdd.TabIndex = 14
        btnAdd.Text = "ADD"
        btnAdd.UseVisualStyleBackColor = False
        ' 
        ' VatPanel
        ' 
        VatPanel.BackColor = Color.White
        VatPanel.Controls.Add(Button3)
        VatPanel.Controls.Add(Button1)
        VatPanel.Controls.Add(Label1)
        VatPanel.Controls.Add(DateTimePicker1)
        VatPanel.Controls.Add(lblSupplierName)
        VatPanel.Controls.Add(btnEdit)
        VatPanel.Controls.Add(txtVatRate)
        VatPanel.Controls.Add(btnAdd)
        VatPanel.Controls.Add(btnDelete)
        VatPanel.Location = New Point(48, 105)
        VatPanel.Name = "VatPanel"
        VatPanel.Size = New Size(734, 194)
        VatPanel.TabIndex = 18
        VatPanel.Visible = False
        ' 
        ' Button3
        ' 
        Button3.BackColor = Color.Transparent
        Button3.FlatAppearance.BorderSize = 0
        Button3.FlatStyle = FlatStyle.Flat
        Button3.ForeColor = Color.Transparent
        Button3.Image = CType(resources.GetObject("Button3.Image"), Image)
        Button3.Location = New Point(665, 3)
        Button3.Name = "Button3"
        Button3.Size = New Size(66, 50)
        Button3.TabIndex = 96
        Button3.UseVisualStyleBackColor = False
        ' 
        ' Button1
        ' 
        Button1.BackColor = SystemColors.ControlDarkDark
        Button1.FlatStyle = FlatStyle.Flat
        Button1.ForeColor = SystemColors.ButtonHighlight
        Button1.Location = New Point(549, 116)
        Button1.Margin = New Padding(5, 6, 5, 6)
        Button1.Name = "Button1"
        Button1.Size = New Size(149, 50)
        Button1.TabIndex = 20
        Button1.Text = "RESET"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label1.Location = New Point(849, 23)
        Label1.Margin = New Padding(5, 0, 5, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(170, 30)
        Label1.TabIndex = 19
        Label1.Text = "Effective Date :"
        Label1.Visible = False
        ' 
        ' DateTimePicker1
        ' 
        DateTimePicker1.Font = New Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        DateTimePicker1.Location = New Point(1035, 18)
        DateTimePicker1.Name = "DateTimePicker1"
        DateTimePicker1.Size = New Size(374, 37)
        DateTimePicker1.TabIndex = 18
        DateTimePicker1.Visible = False
        ' 
        ' lblSupplierName
        ' 
        lblSupplierName.AutoSize = True
        lblSupplierName.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblSupplierName.Location = New Point(23, 71)
        lblSupplierName.Margin = New Padding(5, 0, 5, 0)
        lblSupplierName.Name = "lblSupplierName"
        lblSupplierName.Size = New Size(137, 30)
        lblSupplierName.TabIndex = 17
        lblSupplierName.Text = "Vat Rate % :"
        ' 
        ' Button2
        ' 
        Button2.BackColor = Color.Transparent
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Flat
        Button2.ForeColor = Color.Transparent
        Button2.Image = CType(resources.GetObject("Button2.Image"), Image)
        Button2.Location = New Point(762, 12)
        Button2.Name = "Button2"
        Button2.Size = New Size(66, 50)
        Button2.TabIndex = 94
        Button2.UseVisualStyleBackColor = False
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(818, 12)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 93
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label3)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(169, 60)
        Panel1.TabIndex = 95
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.Transparent
        Label3.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label3.ForeColor = SystemColors.ButtonHighlight
        Label3.Location = New Point(21, 14)
        Label3.Name = "Label3"
        Label3.Size = New Size(121, 32)
        Label3.TabIndex = 0
        Label3.Text = "VAT LIST"
        ' 
        ' Vat
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ControlLight
        ClientSize = New Size(900, 707)
        Controls.Add(Panel1)
        Controls.Add(Button2)
        Controls.Add(btnCLose)
        Controls.Add(VatPanel)
        Controls.Add(dgvVat)
        FormBorderStyle = FormBorderStyle.None
        Name = "Vat"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Vat"
        CType(dgvVat, ComponentModel.ISupportInitialize).EndInit()
        VatPanel.ResumeLayout(False)
        VatPanel.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvVat As DataGridView
    Friend WithEvents txtVatRate As TextBox
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents VatPanel As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents lblSupplierName As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents btnCLose As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Button3 As Button
End Class
