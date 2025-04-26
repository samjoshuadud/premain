<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Category
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

    'NOTE: The following procedure is required by the Windows Form Designer.
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Category))
        dgvCategories = New DataGridView()
        txtCategoryName = New TextBox()
        txtDescription = New TextBox()
        btnAdd = New Button()
        btnEdit = New Button()
        btnDelete = New Button()
        CategoryPanel = New Panel()
        Button1 = New Button()
        cboCategoryType = New ComboBox()
        btnReset = New Button()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        PanelCategory = New Panel()
        Button2 = New Button()
        btnCLose = New Button()
        txtSearch = New TextBox()
        CType(dgvCategories, ComponentModel.ISupportInitialize).BeginInit()
        CategoryPanel.SuspendLayout()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvCategories
        ' 
        dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCategories.Location = New Point(12, 104)
        dgvCategories.Name = "dgvCategories"
        dgvCategories.RowHeadersWidth = 62
        dgvCategories.Size = New Size(1734, 846)
        dgvCategories.TabIndex = 0
        ' 
        ' txtCategoryName
        ' 
        txtCategoryName.Font = New Font("Segoe UI", 10F)
        txtCategoryName.Location = New Point(228, 71)
        txtCategoryName.Name = "txtCategoryName"
        txtCategoryName.Size = New Size(614, 34)
        txtCategoryName.TabIndex = 3
        ' 
        ' txtDescription
        ' 
        txtDescription.Location = New Point(228, 149)
        txtDescription.Multiline = True
        txtDescription.Name = "txtDescription"
        txtDescription.Size = New Size(614, 34)
        txtDescription.TabIndex = 5
        ' 
        ' btnAdd
        ' 
        btnAdd.BackColor = SystemColors.ActiveCaptionText
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnAdd.ForeColor = SystemColors.ButtonHighlight
        btnAdd.Location = New Point(228, 189)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(149, 50)
        btnAdd.TabIndex = 6
        btnAdd.Text = "ADD"
        btnAdd.UseVisualStyleBackColor = False
        ' 
        ' btnEdit
        ' 
        btnEdit.BackColor = SystemColors.ActiveCaptionText
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnEdit.ForeColor = SystemColors.ButtonHighlight
        btnEdit.Location = New Point(383, 189)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(149, 50)
        btnEdit.TabIndex = 7
        btnEdit.Text = "EDIT"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' btnDelete
        ' 
        btnDelete.BackColor = SystemColors.ActiveCaptionText
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnDelete.ForeColor = SystemColors.ButtonHighlight
        btnDelete.Location = New Point(538, 189)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(149, 50)
        btnDelete.TabIndex = 8
        btnDelete.Text = "DELETE"
        btnDelete.UseVisualStyleBackColor = False
        ' 
        ' CategoryPanel
        ' 
        CategoryPanel.BackColor = SystemColors.ControlLightLight
        CategoryPanel.Controls.Add(Button1)
        CategoryPanel.Controls.Add(cboCategoryType)
        CategoryPanel.Controls.Add(btnReset)
        CategoryPanel.Controls.Add(Label4)
        CategoryPanel.Controls.Add(Label3)
        CategoryPanel.Controls.Add(Label2)
        CategoryPanel.Controls.Add(txtCategoryName)
        CategoryPanel.Controls.Add(btnDelete)
        CategoryPanel.Controls.Add(btnEdit)
        CategoryPanel.Controls.Add(txtDescription)
        CategoryPanel.Controls.Add(btnAdd)
        CategoryPanel.Location = New Point(75, 190)
        CategoryPanel.Name = "CategoryPanel"
        CategoryPanel.Size = New Size(866, 258)
        CategoryPanel.TabIndex = 3
        CategoryPanel.Visible = False
        ' 
        ' Button1
        ' 
        Button1.BackColor = Color.Transparent
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Flat
        Button1.ForeColor = Color.Transparent
        Button1.Image = CType(resources.GetObject("Button1.Image"), Image)
        Button1.Location = New Point(797, 3)
        Button1.Name = "Button1"
        Button1.Size = New Size(66, 50)
        Button1.TabIndex = 96
        Button1.UseVisualStyleBackColor = False
        ' 
        ' cboCategoryType
        ' 
        cboCategoryType.FormattingEnabled = True
        cboCategoryType.Items.AddRange(New Object() {"Perishable Goods", "Non-Perishable Goods"})
        cboCategoryType.Location = New Point(228, 112)
        cboCategoryType.Name = "cboCategoryType"
        cboCategoryType.Size = New Size(614, 33)
        cboCategoryType.TabIndex = 4
        ' 
        ' btnReset
        ' 
        btnReset.BackColor = SystemColors.ControlDarkDark
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnReset.ForeColor = SystemColors.ButtonHighlight
        btnReset.Location = New Point(693, 189)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(149, 50)
        btnReset.TabIndex = 9
        btnReset.Text = "RESET"
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label4.Location = New Point(16, 149)
        Label4.Name = "Label4"
        Label4.Size = New Size(154, 32)
        Label4.TabIndex = 2
        Label4.Text = "Discription :"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label3.Location = New Point(16, 112)
        Label3.Name = "Label3"
        Label3.Size = New Size(193, 32)
        Label3.TabIndex = 1
        Label3.Text = "Category Type :"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label2.Location = New Point(16, 71)
        Label2.Name = "Label2"
        Label2.Size = New Size(206, 32)
        Label2.TabIndex = 0
        Label2.Text = "Category Name :"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.Transparent
        Label1.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label1.ForeColor = SystemColors.ButtonHighlight
        Label1.Location = New Point(13, 19)
        Label1.Name = "Label1"
        Label1.Size = New Size(199, 32)
        Label1.TabIndex = 0
        Label1.Text = "CATEGORY LIST"
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label1)
        PanelCategory.Location = New Point(12, 16)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(230, 71)
        PanelCategory.TabIndex = 0
        ' 
        ' Button2
        ' 
        Button2.BackColor = Color.Transparent
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Flat
        Button2.ForeColor = Color.Transparent
        Button2.Image = CType(resources.GetObject("Button2.Image"), Image)
        Button2.Location = New Point(1623, 48)
        Button2.Name = "Button2"
        Button2.Size = New Size(66, 50)
        Button2.TabIndex = 95
        Button2.UseVisualStyleBackColor = False
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(1679, 48)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 94
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' txtSearch
        ' 
        txtSearch.Font = New Font("Segoe UI", 12F)
        txtSearch.Location = New Point(1340, 59)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = " 🔍 Search Supplier"
        txtSearch.Size = New Size(277, 39)
        txtSearch.TabIndex = 93
        ' 
        ' Category
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1758, 1018)
        Controls.Add(Button2)
        Controls.Add(btnCLose)
        Controls.Add(txtSearch)
        Controls.Add(CategoryPanel)
        Controls.Add(dgvCategories)
        Controls.Add(PanelCategory)
        FormBorderStyle = FormBorderStyle.None
        Name = "Category"
        Text = "Category Management"
        CType(dgvCategories, ComponentModel.ISupportInitialize).EndInit()
        CategoryPanel.ResumeLayout(False)
        CategoryPanel.PerformLayout()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvCategories As DataGridView
    Friend WithEvents txtCategoryName As TextBox
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents CategoryPanel As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnReset As Button
    Friend WithEvents cboCategoryType As ComboBox
    Friend WithEvents Button2 As Button
    Friend WithEvents btnCLose As Button
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Button1 As Button
End Class
