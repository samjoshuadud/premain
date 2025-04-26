<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Inventory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Inventory))
        Button1 = New Button()
        dgvInventory = New DataGridView()
        txtCriticalLevel = New TextBox()
        BtnSave = New Button()
        lblCriticalMessage = New Label()
        pnlCriticalStock = New Panel()
        PictureBox1 = New PictureBox()
        Label6 = New Label()
        Label5 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Panel1 = New Panel()
        Label7 = New Label()
        txtWholeSalePrice = New TextBox()
        Label1 = New Label()
        Button2 = New Button()
        PanelCategory = New Panel()
        Label2 = New Label()
        Button3 = New Button()
        Button4 = New Button()
        CType(dgvInventory, ComponentModel.ISupportInitialize).BeginInit()
        pnlCriticalStock.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.BackColor = SystemColors.ControlDarkDark
        Button1.FlatStyle = FlatStyle.Flat
        Button1.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Button1.ForeColor = SystemColors.ButtonHighlight
        Button1.ImageAlign = ContentAlignment.MiddleLeft
        Button1.Location = New Point(1535, 123)
        Button1.Name = "Button1"
        Button1.Size = New Size(213, 58)
        Button1.TabIndex = 0
        Button1.Text = "RECIEVED DELIVERY"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' dgvInventory
        ' 
        dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvInventory.Location = New Point(12, 186)
        dgvInventory.Name = "dgvInventory"
        dgvInventory.RowHeadersWidth = 62
        dgvInventory.Size = New Size(1736, 764)
        dgvInventory.TabIndex = 2
        ' 
        ' txtCriticalLevel
        ' 
        txtCriticalLevel.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtCriticalLevel.Location = New Point(220, 50)
        txtCriticalLevel.Name = "txtCriticalLevel"
        txtCriticalLevel.Size = New Size(354, 34)
        txtCriticalLevel.TabIndex = 3
        ' 
        ' BtnSave
        ' 
        BtnSave.BackColor = SystemColors.ActiveCaptionText
        BtnSave.FlatStyle = FlatStyle.Flat
        BtnSave.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        BtnSave.ForeColor = SystemColors.ButtonHighlight
        BtnSave.Location = New Point(580, 37)
        BtnSave.Name = "BtnSave"
        BtnSave.Size = New Size(112, 47)
        BtnSave.TabIndex = 5
        BtnSave.Text = "UPDATE"
        BtnSave.UseVisualStyleBackColor = False
        ' 
        ' lblCriticalMessage
        ' 
        lblCriticalMessage.AutoSize = True
        lblCriticalMessage.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCriticalMessage.Location = New Point(137, 76)
        lblCriticalMessage.Name = "lblCriticalMessage"
        lblCriticalMessage.Size = New Size(81, 30)
        lblCriticalMessage.TabIndex = 6
        lblCriticalMessage.Text = "Label1"
        ' 
        ' pnlCriticalStock
        ' 
        pnlCriticalStock.BackColor = Color.Transparent
        pnlCriticalStock.Controls.Add(PictureBox1)
        pnlCriticalStock.Controls.Add(Label6)
        pnlCriticalStock.Controls.Add(lblCriticalMessage)
        pnlCriticalStock.Controls.Add(Label5)
        pnlCriticalStock.Controls.Add(Label4)
        pnlCriticalStock.Controls.Add(Label3)
        pnlCriticalStock.Location = New Point(1218, 779)
        pnlCriticalStock.Name = "pnlCriticalStock"
        pnlCriticalStock.Size = New Size(530, 171)
        pnlCriticalStock.TabIndex = 7
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(22, 36)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(96, 96)
        PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize
        PictureBox1.TabIndex = 7
        PictureBox1.TabStop = False
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label6.Location = New Point(424, 139)
        Label6.Name = "Label6"
        Label6.Size = New Size(71, 25)
        Label6.TabIndex = 13
        Label6.Text = "Critical"
        Label6.Visible = False
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(424, 107)
        Label5.Name = "Label5"
        Label5.Size = New Size(95, 25)
        Label5.TabIndex = 12
        Label5.Text = "Warning !"
        Label5.Visible = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.BackColor = Color.Yellow
        Label4.ForeColor = Color.Yellow
        Label4.Location = New Point(326, 107)
        Label4.Name = "Label4"
        Label4.Size = New Size(92, 25)
        Label4.TabIndex = 10
        Label4.Text = "00000000"
        Label4.Visible = False
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.Red
        Label3.ForeColor = Color.Red
        Label3.Location = New Point(326, 141)
        Label3.Name = "Label3"
        Label3.Size = New Size(92, 25)
        Label3.TabIndex = 11
        Label3.Text = "00000000"
        Label3.Visible = False
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Button4)
        Panel1.Controls.Add(Label7)
        Panel1.Controls.Add(txtWholeSalePrice)
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(BtnSave)
        Panel1.Controls.Add(txtCriticalLevel)
        Panel1.Location = New Point(12, 86)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1070, 94)
        Panel1.TabIndex = 9
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label7.Location = New Point(14, 13)
        Label7.Name = "Label7"
        Label7.Size = New Size(200, 25)
        Label7.TabIndex = 12
        Label7.Text = "WholeSale Discounts :"
        ' 
        ' txtWholeSalePrice
        ' 
        txtWholeSalePrice.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtWholeSalePrice.Location = New Point(220, 13)
        txtWholeSalePrice.Name = "txtWholeSalePrice"
        txtWholeSalePrice.Size = New Size(354, 34)
        txtWholeSalePrice.TabIndex = 11
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(14, 50)
        Label1.Name = "Label1"
        Label1.Size = New Size(130, 25)
        Label1.TabIndex = 7
        Label1.Text = "Critical Level :"
        ' 
        ' Button2
        ' 
        Button2.BackColor = SystemColors.ActiveCaptionText
        Button2.FlatStyle = FlatStyle.Flat
        Button2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Button2.ForeColor = SystemColors.ButtonHighlight
        Button2.Location = New Point(1355, 122)
        Button2.Name = "Button2"
        Button2.Size = New Size(174, 58)
        Button2.TabIndex = 10
        Button2.Text = "DELIVERY LIST"
        Button2.UseVisualStyleBackColor = False
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label2)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(199, 68)
        PanelCategory.TabIndex = 37
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label2.ForeColor = SystemColors.ButtonHighlight
        Label2.Location = New Point(18, 17)
        Label2.Name = "Label2"
        Label2.Size = New Size(155, 32)
        Label2.TabIndex = 0
        Label2.Text = "INVENTORY"
        ' 
        ' Button3
        ' 
        Button3.BackColor = Color.Transparent
        Button3.FlatAppearance.BorderSize = 0
        Button3.FlatStyle = FlatStyle.Flat
        Button3.ForeColor = Color.Transparent
        Button3.Image = CType(resources.GetObject("Button3.Image"), Image)
        Button3.Location = New Point(1682, 11)
        Button3.Name = "Button3"
        Button3.Size = New Size(66, 50)
        Button3.TabIndex = 97
        Button3.UseVisualStyleBackColor = False
        ' 
        ' Button4
        ' 
        Button4.BackColor = SystemColors.ActiveCaptionText
        Button4.FlatStyle = FlatStyle.Flat
        Button4.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Button4.ForeColor = SystemColors.ButtonHighlight
        Button4.Location = New Point(698, 39)
        Button4.Name = "Button4"
        Button4.Size = New Size(112, 47)
        Button4.TabIndex = 13
        Button4.Text = "RELOAD"
        Button4.UseVisualStyleBackColor = False
        ' 
        ' Inventory
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1774, 962)
        Controls.Add(Button3)
        Controls.Add(PanelCategory)
        Controls.Add(Button2)
        Controls.Add(pnlCriticalStock)
        Controls.Add(dgvInventory)
        Controls.Add(Panel1)
        Controls.Add(Button1)
        FormBorderStyle = FormBorderStyle.None
        Name = "Inventory"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Inventory"
        CType(dgvInventory, ComponentModel.ISupportInitialize).EndInit()
        pnlCriticalStock.ResumeLayout(False)
        pnlCriticalStock.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents dgvInventory As DataGridView
    Friend WithEvents txtCriticalLevel As TextBox
    Friend WithEvents BtnSave As Button
    Friend WithEvents lblCriticalMessage As Label
    Friend WithEvents pnlCriticalStock As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtWholeSalePrice As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
End Class
