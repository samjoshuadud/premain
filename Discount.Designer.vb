<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Discount
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Discount))
        txtDiscountName = New TextBox()
        txtDiscountRate = New TextBox()
        dgvDiscounts = New DataGridView()
        BtnAdd = New Button()
        btnEdit = New Button()
        btnDelete = New Button()
        btnReset = New Button()
        Label1 = New Label()
        Label2 = New Label()
        PanelCategory = New Panel()
        Label3 = New Label()
        DiscountPanel = New Panel()
        Button1 = New Button()
        Button2 = New Button()
        btnCLose = New Button()
        CType(dgvDiscounts, ComponentModel.ISupportInitialize).BeginInit()
        PanelCategory.SuspendLayout()
        DiscountPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtDiscountName
        ' 
        txtDiscountName.Font = New Font("Segoe UI", 12F)
        txtDiscountName.Location = New Point(214, 69)
        txtDiscountName.Name = "txtDiscountName"
        txtDiscountName.PlaceholderText = "Enter Discount Name"
        txtDiscountName.Size = New Size(530, 39)
        txtDiscountName.TabIndex = 0
        ' 
        ' txtDiscountRate
        ' 
        txtDiscountRate.Font = New Font("Segoe UI", 12F)
        txtDiscountRate.Location = New Point(214, 114)
        txtDiscountRate.Name = "txtDiscountRate"
        txtDiscountRate.PlaceholderText = "Enter Discount Rate"
        txtDiscountRate.Size = New Size(530, 39)
        txtDiscountRate.TabIndex = 1
        ' 
        ' dgvDiscounts
        ' 
        dgvDiscounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDiscounts.Location = New Point(12, 78)
        dgvDiscounts.Name = "dgvDiscounts"
        dgvDiscounts.RowHeadersWidth = 62
        dgvDiscounts.Size = New Size(890, 554)
        dgvDiscounts.TabIndex = 2
        ' 
        ' BtnAdd
        ' 
        BtnAdd.BackColor = SystemColors.ActiveCaptionText
        BtnAdd.FlatStyle = FlatStyle.Flat
        BtnAdd.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        BtnAdd.ForeColor = SystemColors.ButtonHighlight
        BtnAdd.Location = New Point(281, 172)
        BtnAdd.Name = "BtnAdd"
        BtnAdd.Size = New Size(149, 50)
        BtnAdd.TabIndex = 3
        BtnAdd.Text = "ADD"
        BtnAdd.UseVisualStyleBackColor = False
        ' 
        ' btnEdit
        ' 
        btnEdit.BackColor = SystemColors.ActiveCaptionText
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnEdit.ForeColor = SystemColors.ButtonHighlight
        btnEdit.Location = New Point(436, 172)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(149, 50)
        btnEdit.TabIndex = 4
        btnEdit.Text = "EDIT"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' btnDelete
        ' 
        btnDelete.BackColor = SystemColors.ActiveCaptionText
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnDelete.ForeColor = SystemColors.ButtonHighlight
        btnDelete.Location = New Point(126, 172)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(149, 50)
        btnDelete.TabIndex = 5
        btnDelete.Text = "DELETE"
        btnDelete.UseVisualStyleBackColor = False
        ' 
        ' btnReset
        ' 
        btnReset.BackColor = SystemColors.ControlDarkDark
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnReset.ForeColor = SystemColors.ButtonHighlight
        btnReset.Location = New Point(593, 172)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(149, 50)
        btnReset.TabIndex = 6
        btnReset.Text = "RESET"
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label1.Location = New Point(20, 69)
        Label1.Name = "Label1"
        Label1.Size = New Size(183, 30)
        Label1.TabIndex = 7
        Label1.Text = "Discount Name :"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label2.Location = New Point(20, 109)
        Label2.Name = "Label2"
        Label2.Size = New Size(169, 30)
        Label2.TabIndex = 8
        Label2.Text = "Discount Rate :"
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label3)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(235, 60)
        PanelCategory.TabIndex = 38
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.Transparent
        Label3.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label3.ForeColor = SystemColors.ButtonHighlight
        Label3.Location = New Point(17, 18)
        Label3.Name = "Label3"
        Label3.Size = New Size(196, 32)
        Label3.TabIndex = 0
        Label3.Text = "DISCOUNT LIST"
        ' 
        ' DiscountPanel
        ' 
        DiscountPanel.Controls.Add(Button1)
        DiscountPanel.Controls.Add(Label1)
        DiscountPanel.Controls.Add(txtDiscountName)
        DiscountPanel.Controls.Add(btnDelete)
        DiscountPanel.Controls.Add(btnReset)
        DiscountPanel.Controls.Add(BtnAdd)
        DiscountPanel.Controls.Add(Label2)
        DiscountPanel.Controls.Add(txtDiscountRate)
        DiscountPanel.Controls.Add(btnEdit)
        DiscountPanel.Location = New Point(60, 170)
        DiscountPanel.Name = "DiscountPanel"
        DiscountPanel.Size = New Size(771, 247)
        DiscountPanel.TabIndex = 39
        DiscountPanel.Visible = False
        ' 
        ' Button1
        ' 
        Button1.BackColor = Color.Transparent
        Button1.FlatAppearance.BorderSize = 0
        Button1.FlatStyle = FlatStyle.Flat
        Button1.ForeColor = Color.Transparent
        Button1.Image = CType(resources.GetObject("Button1.Image"), Image)
        Button1.Location = New Point(702, 3)
        Button1.Name = "Button1"
        Button1.Size = New Size(66, 50)
        Button1.TabIndex = 95
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Button2
        ' 
        Button2.BackColor = Color.Transparent
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Flat
        Button2.ForeColor = Color.Transparent
        Button2.Image = CType(resources.GetObject("Button2.Image"), Image)
        Button2.Location = New Point(777, 12)
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
        btnCLose.Location = New Point(833, 12)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 93
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' Discount
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(915, 644)
        Controls.Add(Button2)
        Controls.Add(btnCLose)
        Controls.Add(DiscountPanel)
        Controls.Add(PanelCategory)
        Controls.Add(dgvDiscounts)
        FormBorderStyle = FormBorderStyle.None
        Name = "Discount"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Discount"
        CType(dgvDiscounts, ComponentModel.ISupportInitialize).EndInit()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        DiscountPanel.ResumeLayout(False)
        DiscountPanel.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents txtDiscountName As TextBox
    Friend WithEvents txtDiscountRate As TextBox
    Friend WithEvents dgvDiscounts As DataGridView
    Friend WithEvents btn As Button
    Friend WithEvents BtnAdd As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnReset As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents DiscountPanel As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents btnCLose As Button
    Friend WithEvents Button1 As Button

End Class
