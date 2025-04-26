<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Supplier
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Supplier))
        dgvSuppliers = New DataGridView()
        txtCompanyName = New TextBox()
        lblSupplierName = New Label()
        txtContactName = New TextBox()
        lblContactName = New Label()
        txtContactNumber = New TextBox()
        lblContactNumber = New Label()
        txtEmail = New TextBox()
        lblEmail = New Label()
        txtAddress = New TextBox()
        lblAddress = New Label()
        btnAdd = New Button()
        btnDelete = New Button()
        btnEdit = New Button()
        PanelSupplier = New Panel()
        Button1 = New Button()
        btnReset = New Button()
        PanelCategory = New Panel()
        Label2 = New Label()
        txtSearch = New TextBox()
        btnCLose = New Button()
        Button2 = New Button()
        CType(dgvSuppliers, ComponentModel.ISupportInitialize).BeginInit()
        PanelSupplier.SuspendLayout()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvSuppliers
        ' 
        dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvSuppliers.Location = New Point(16, 82)
        dgvSuppliers.Margin = New Padding(5, 6, 5, 6)
        dgvSuppliers.Name = "dgvSuppliers"
        dgvSuppliers.RowHeadersWidth = 62
        dgvSuppliers.Size = New Size(1740, 865)
        dgvSuppliers.TabIndex = 0
        ' 
        ' txtCompanyName
        ' 
        txtCompanyName.Font = New Font("Segoe UI", 10F)
        txtCompanyName.Location = New Point(235, 71)
        txtCompanyName.Margin = New Padding(5, 6, 5, 6)
        txtCompanyName.Name = "txtCompanyName"
        txtCompanyName.Size = New Size(623, 34)
        txtCompanyName.TabIndex = 2
        ' 
        ' lblSupplierName
        ' 
        lblSupplierName.AutoSize = True
        lblSupplierName.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblSupplierName.Location = New Point(23, 73)
        lblSupplierName.Margin = New Padding(5, 0, 5, 0)
        lblSupplierName.Name = "lblSupplierName"
        lblSupplierName.Size = New Size(184, 30)
        lblSupplierName.TabIndex = 1
        lblSupplierName.Text = "Company Name:"
        ' 
        ' txtContactName
        ' 
        txtContactName.Font = New Font("Segoe UI", 10F)
        txtContactName.Location = New Point(235, 117)
        txtContactName.Margin = New Padding(5, 6, 5, 6)
        txtContactName.Name = "txtContactName"
        txtContactName.Size = New Size(623, 34)
        txtContactName.TabIndex = 4
        ' 
        ' lblContactName
        ' 
        lblContactName.AutoSize = True
        lblContactName.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblContactName.Location = New Point(23, 117)
        lblContactName.Margin = New Padding(5, 0, 5, 0)
        lblContactName.Name = "lblContactName"
        lblContactName.Size = New Size(167, 30)
        lblContactName.TabIndex = 3
        lblContactName.Text = "Contact Name:"
        ' 
        ' txtContactNumber
        ' 
        txtContactNumber.Font = New Font("Segoe UI", 10F)
        txtContactNumber.Location = New Point(235, 253)
        txtContactNumber.Margin = New Padding(5, 6, 5, 6)
        txtContactNumber.Name = "txtContactNumber"
        txtContactNumber.Size = New Size(623, 34)
        txtContactNumber.TabIndex = 6
        ' 
        ' lblContactNumber
        ' 
        lblContactNumber.AutoSize = True
        lblContactNumber.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblContactNumber.Location = New Point(23, 248)
        lblContactNumber.Margin = New Padding(5, 0, 5, 0)
        lblContactNumber.Name = "lblContactNumber"
        lblContactNumber.Size = New Size(192, 30)
        lblContactNumber.TabIndex = 5
        lblContactNumber.Text = "Contact Number:"
        ' 
        ' txtEmail
        ' 
        txtEmail.Font = New Font("Segoe UI", 10F)
        txtEmail.Location = New Point(235, 292)
        txtEmail.Margin = New Padding(5, 6, 5, 6)
        txtEmail.Name = "txtEmail"
        txtEmail.Size = New Size(623, 34)
        txtEmail.TabIndex = 8
        ' 
        ' lblEmail
        ' 
        lblEmail.AutoSize = True
        lblEmail.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblEmail.Location = New Point(23, 294)
        lblEmail.Margin = New Padding(5, 0, 5, 0)
        lblEmail.Name = "lblEmail"
        lblEmail.Size = New Size(157, 30)
        lblEmail.TabIndex = 7
        lblEmail.Text = "Email Adress :"
        ' 
        ' txtAddress
        ' 
        txtAddress.Font = New Font("Segoe UI", 10F)
        txtAddress.Location = New Point(235, 163)
        txtAddress.Margin = New Padding(5, 6, 5, 6)
        txtAddress.Multiline = True
        txtAddress.Name = "txtAddress"
        txtAddress.Size = New Size(623, 78)
        txtAddress.TabIndex = 10
        ' 
        ' lblAddress
        ' 
        lblAddress.AutoSize = True
        lblAddress.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblAddress.Location = New Point(23, 165)
        lblAddress.Margin = New Padding(5, 0, 5, 0)
        lblAddress.Name = "lblAddress"
        lblAddress.Size = New Size(103, 30)
        lblAddress.TabIndex = 9
        lblAddress.Text = "Address:"
        ' 
        ' btnAdd
        ' 
        btnAdd.BackColor = SystemColors.ActiveCaptionText
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnAdd.ForeColor = SystemColors.ButtonHighlight
        btnAdd.Location = New Point(237, 338)
        btnAdd.Margin = New Padding(5, 6, 5, 6)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(149, 50)
        btnAdd.TabIndex = 11
        btnAdd.Text = "ADD"
        btnAdd.UseVisualStyleBackColor = False
        ' 
        ' btnDelete
        ' 
        btnDelete.BackColor = SystemColors.ActiveCaptionText
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnDelete.ForeColor = SystemColors.ButtonHighlight
        btnDelete.Location = New Point(550, 338)
        btnDelete.Margin = New Padding(5, 6, 5, 6)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(149, 50)
        btnDelete.TabIndex = 12
        btnDelete.Text = "DELETE"
        btnDelete.UseVisualStyleBackColor = False
        ' 
        ' btnEdit
        ' 
        btnEdit.BackColor = SystemColors.ActiveCaptionText
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnEdit.ForeColor = SystemColors.ButtonHighlight
        btnEdit.Location = New Point(396, 338)
        btnEdit.Margin = New Padding(5, 6, 5, 6)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(149, 50)
        btnEdit.TabIndex = 13
        btnEdit.Text = "EDIT"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' PanelSupplier
        ' 
        PanelSupplier.Controls.Add(Button1)
        PanelSupplier.Controls.Add(btnReset)
        PanelSupplier.Controls.Add(lblSupplierName)
        PanelSupplier.Controls.Add(btnEdit)
        PanelSupplier.Controls.Add(lblContactName)
        PanelSupplier.Controls.Add(btnDelete)
        PanelSupplier.Controls.Add(lblAddress)
        PanelSupplier.Controls.Add(btnAdd)
        PanelSupplier.Controls.Add(txtCompanyName)
        PanelSupplier.Controls.Add(txtEmail)
        PanelSupplier.Controls.Add(txtAddress)
        PanelSupplier.Controls.Add(txtContactNumber)
        PanelSupplier.Controls.Add(lblEmail)
        PanelSupplier.Controls.Add(txtContactName)
        PanelSupplier.Controls.Add(lblContactNumber)
        PanelSupplier.Location = New Point(16, 144)
        PanelSupplier.Name = "PanelSupplier"
        PanelSupplier.Size = New Size(884, 420)
        PanelSupplier.TabIndex = 14
        PanelSupplier.Visible = False
        ' 
        ' Button1
        ' 
        Button1.BackColor = Color.Transparent
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Flat
        Button1.ForeColor = Color.Transparent
        Button1.Image = CType(resources.GetObject("Button1.Image"), Image)
        Button1.Location = New Point(792, 3)
        Button1.Name = "Button1"
        Button1.Size = New Size(66, 50)
        Button1.TabIndex = 93
        Button1.UseVisualStyleBackColor = False
        ' 
        ' btnReset
        ' 
        btnReset.BackColor = SystemColors.ControlDarkDark
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnReset.ForeColor = SystemColors.ButtonHighlight
        btnReset.Location = New Point(709, 338)
        btnReset.Margin = New Padding(5, 6, 5, 6)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(149, 50)
        btnReset.TabIndex = 14
        btnReset.Text = "RESET"
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' PanelCategory
        ' 
        PanelCategory.BackColor = Color.FromArgb(CByte(51), CByte(153), CByte(255))
        PanelCategory.Controls.Add(Label2)
        PanelCategory.Location = New Point(16, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(215, 61)
        PanelCategory.TabIndex = 35
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label2.ForeColor = SystemColors.ButtonHighlight
        Label2.Location = New Point(10, 13)
        Label2.Name = "Label2"
        Label2.Size = New Size(185, 32)
        Label2.TabIndex = 0
        Label2.Text = "SUPPLIER LIST"
        ' 
        ' txtSearch
        ' 
        txtSearch.Font = New Font("Segoe UI", 12F)
        txtSearch.Location = New Point(1358, 41)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = " 🔍 Search Supplier"
        txtSearch.Size = New Size(277, 39)
        txtSearch.TabIndex = 0
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(1697, 30)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 91
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' Button2
        ' 
        Button2.BackColor = Color.Transparent
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Flat
        Button2.ForeColor = Color.Transparent
        Button2.Image = CType(resources.GetObject("Button2.Image"), Image)
        Button2.Location = New Point(1641, 30)
        Button2.Name = "Button2"
        Button2.Size = New Size(66, 50)
        Button2.TabIndex = 92
        Button2.UseVisualStyleBackColor = False
        ' 
        ' Supplier
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1770, 962)
        Controls.Add(Button2)
        Controls.Add(btnCLose)
        Controls.Add(txtSearch)
        Controls.Add(PanelCategory)
        Controls.Add(PanelSupplier)
        Controls.Add(dgvSuppliers)
        FormBorderStyle = FormBorderStyle.None
        Margin = New Padding(5, 6, 5, 6)
        Name = "Supplier"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Supplier Management"
        CType(dgvSuppliers, ComponentModel.ISupportInitialize).EndInit()
        PanelSupplier.ResumeLayout(False)
        PanelSupplier.PerformLayout()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvSuppliers As System.Windows.Forms.DataGridView
    Friend WithEvents txtCompanyName As System.Windows.Forms.TextBox
    Friend WithEvents lblSupplierName As System.Windows.Forms.Label
    Friend WithEvents txtContactName As System.Windows.Forms.TextBox
    Friend WithEvents lblContactName As System.Windows.Forms.Label
    Friend WithEvents txtContactNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblContactNumber As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents PanelSupplier As Panel
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnReset As Button
    Friend WithEvents btnCLose As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button

End Class
