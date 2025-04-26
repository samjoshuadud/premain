<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TodaySales
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TodaySales))
        dgvTodaysSales = New DataGridView()
        txtTransactionNumber = New TextBox()
        txtProductInfo = New TextBox()
        txtQuantity = New TextBox()
        txtUnitPrice = New TextBox()
        txtTotalPrice = New TextBox()
        txtDiscount = New TextBox()
        cmbAddToInventory = New ComboBox()
        lblAddToInventory = New Label()
        lblCancelQuantity = New Label()
        lblReasons = New Label()
        txtCancelQuantity = New TextBox()
        txtReasons = New TextBox()
        lblTransactionNumber = New Label()
        lblProductInfo = New Label()
        lblQuantity = New Label()
        lblUnitPrice = New Label()
        lblTotalPrice = New Label()
        lblDiscount = New Label()
        btnCancelOrder = New Button()
        txtProductID = New TextBox()
        cmbCancelledby = New ComboBox()
        txtVoidby = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        Panel1 = New Panel()
        btnCLose = New Button()
        CType(dgvTodaysSales, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvTodaysSales
        ' 
        dgvTodaysSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvTodaysSales.Location = New Point(12, 334)
        dgvTodaysSales.Name = "dgvTodaysSales"
        dgvTodaysSales.RowHeadersWidth = 62
        dgvTodaysSales.Size = New Size(1421, 298)
        dgvTodaysSales.TabIndex = 21
        ' 
        ' txtTransactionNumber
        ' 
        txtTransactionNumber.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtTransactionNumber.Location = New Point(213, 86)
        txtTransactionNumber.Name = "txtTransactionNumber"
        txtTransactionNumber.Size = New Size(368, 34)
        txtTransactionNumber.TabIndex = 10
        ' 
        ' txtProductInfo
        ' 
        txtProductInfo.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtProductInfo.Location = New Point(213, 160)
        txtProductInfo.Name = "txtProductInfo"
        txtProductInfo.Size = New Size(368, 34)
        txtProductInfo.TabIndex = 11
        ' 
        ' txtQuantity
        ' 
        txtQuantity.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtQuantity.Location = New Point(213, 196)
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(368, 34)
        txtQuantity.TabIndex = 12
        ' 
        ' txtUnitPrice
        ' 
        txtUnitPrice.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtUnitPrice.Location = New Point(213, 122)
        txtUnitPrice.Name = "txtUnitPrice"
        txtUnitPrice.Size = New Size(368, 34)
        txtUnitPrice.TabIndex = 13
        ' 
        ' txtTotalPrice
        ' 
        txtTotalPrice.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtTotalPrice.Location = New Point(213, 236)
        txtTotalPrice.Name = "txtTotalPrice"
        txtTotalPrice.Size = New Size(368, 34)
        txtTotalPrice.TabIndex = 14
        ' 
        ' txtDiscount
        ' 
        txtDiscount.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtDiscount.Location = New Point(213, 276)
        txtDiscount.Name = "txtDiscount"
        txtDiscount.Size = New Size(368, 34)
        txtDiscount.TabIndex = 15
        ' 
        ' cmbAddToInventory
        ' 
        cmbAddToInventory.DropDownStyle = ComboBoxStyle.DropDownList
        cmbAddToInventory.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        cmbAddToInventory.Location = New Point(773, 83)
        cmbAddToInventory.Name = "cmbAddToInventory"
        cmbAddToInventory.Size = New Size(368, 36)
        cmbAddToInventory.TabIndex = 16
        ' 
        ' lblAddToInventory
        ' 
        lblAddToInventory.AutoSize = True
        lblAddToInventory.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblAddToInventory.Location = New Point(596, 86)
        lblAddToInventory.Name = "lblAddToInventory"
        lblAddToInventory.Size = New Size(171, 25)
        lblAddToInventory.TabIndex = 7
        lblAddToInventory.Text = "Add To Inventory :"
        ' 
        ' lblCancelQuantity
        ' 
        lblCancelQuantity.AutoSize = True
        lblCancelQuantity.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblCancelQuantity.Location = New Point(596, 207)
        lblCancelQuantity.Name = "lblCancelQuantity"
        lblCancelQuantity.Size = New Size(158, 25)
        lblCancelQuantity.TabIndex = 8
        lblCancelQuantity.Text = "Cancel Quantity :"
        ' 
        ' lblReasons
        ' 
        lblReasons.AutoSize = True
        lblReasons.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblReasons.Location = New Point(596, 169)
        lblReasons.Name = "lblReasons"
        lblReasons.Size = New Size(92, 25)
        lblReasons.TabIndex = 9
        lblReasons.Text = "Reasons :"
        ' 
        ' txtCancelQuantity
        ' 
        txtCancelQuantity.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtCancelQuantity.Location = New Point(773, 207)
        txtCancelQuantity.Name = "txtCancelQuantity"
        txtCancelQuantity.Size = New Size(368, 34)
        txtCancelQuantity.TabIndex = 17
        ' 
        ' txtReasons
        ' 
        txtReasons.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtReasons.Location = New Point(773, 167)
        txtReasons.Name = "txtReasons"
        txtReasons.Size = New Size(368, 34)
        txtReasons.TabIndex = 18
        ' 
        ' lblTransactionNumber
        ' 
        lblTransactionNumber.AutoSize = True
        lblTransactionNumber.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTransactionNumber.Location = New Point(12, 83)
        lblTransactionNumber.Name = "lblTransactionNumber"
        lblTransactionNumber.Size = New Size(195, 25)
        lblTransactionNumber.TabIndex = 1
        lblTransactionNumber.Text = "Transaction Number :"
        ' 
        ' lblProductInfo
        ' 
        lblProductInfo.AutoSize = True
        lblProductInfo.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblProductInfo.Location = New Point(12, 160)
        lblProductInfo.Name = "lblProductInfo"
        lblProductInfo.Size = New Size(129, 25)
        lblProductInfo.TabIndex = 2
        lblProductInfo.Text = "Product Info :"
        ' 
        ' lblQuantity
        ' 
        lblQuantity.AutoSize = True
        lblQuantity.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblQuantity.Location = New Point(12, 196)
        lblQuantity.Name = "lblQuantity"
        lblQuantity.Size = New Size(97, 25)
        lblQuantity.TabIndex = 3
        lblQuantity.Text = "Quantity :"
        ' 
        ' lblUnitPrice
        ' 
        lblUnitPrice.AutoSize = True
        lblUnitPrice.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblUnitPrice.Location = New Point(12, 120)
        lblUnitPrice.Name = "lblUnitPrice"
        lblUnitPrice.Size = New Size(105, 25)
        lblUnitPrice.TabIndex = 4
        lblUnitPrice.Text = "Unit Price :"
        ' 
        ' lblTotalPrice
        ' 
        lblTotalPrice.AutoSize = True
        lblTotalPrice.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTotalPrice.Location = New Point(12, 236)
        lblTotalPrice.Name = "lblTotalPrice"
        lblTotalPrice.Size = New Size(111, 25)
        lblTotalPrice.TabIndex = 5
        lblTotalPrice.Text = "Total Price :"
        ' 
        ' lblDiscount
        ' 
        lblDiscount.AutoSize = True
        lblDiscount.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblDiscount.Location = New Point(12, 276)
        lblDiscount.Name = "lblDiscount"
        lblDiscount.Size = New Size(97, 25)
        lblDiscount.TabIndex = 6
        lblDiscount.Text = "Discount :"
        ' 
        ' btnCancelOrder
        ' 
        btnCancelOrder.BackColor = SystemColors.ActiveCaptionText
        btnCancelOrder.FlatStyle = FlatStyle.Flat
        btnCancelOrder.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnCancelOrder.ForeColor = SystemColors.ButtonHighlight
        btnCancelOrder.Location = New Point(1226, 260)
        btnCancelOrder.Name = "btnCancelOrder"
        btnCancelOrder.Size = New Size(182, 50)
        btnCancelOrder.TabIndex = 20
        btnCancelOrder.Text = "CANCEL ORDER"
        btnCancelOrder.UseVisualStyleBackColor = False
        ' 
        ' txtProductID
        ' 
        txtProductID.Location = New Point(213, 46)
        txtProductID.Name = "txtProductID"
        txtProductID.Size = New Size(150, 31)
        txtProductID.TabIndex = 14
        txtProductID.Visible = False
        ' 
        ' cmbCancelledby
        ' 
        cmbCancelledby.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCancelledby.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        cmbCancelledby.Location = New Point(773, 125)
        cmbCancelledby.Name = "cmbCancelledby"
        cmbCancelledby.Size = New Size(368, 36)
        cmbCancelledby.TabIndex = 19
        ' 
        ' txtVoidby
        ' 
        txtVoidby.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        txtVoidby.Location = New Point(773, 247)
        txtVoidby.Name = "txtVoidby"
        txtVoidby.Size = New Size(368, 34)
        txtVoidby.TabIndex = 16
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label1.Location = New Point(601, 247)
        Label1.Name = "Label1"
        Label1.Size = New Size(87, 25)
        Label1.TabIndex = 22
        Label1.Text = "Void By :"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label2.Location = New Point(596, 128)
        Label2.Name = "Label2"
        Label2.Size = New Size(105, 25)
        Label2.TabIndex = 23
        Label2.Text = "Cancel By :"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnCLose)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1445, 56)
        Panel1.TabIndex = 24
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(1367, 3)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 68
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' TodaySales
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Gainsboro
        ClientSize = New Size(1445, 644)
        Controls.Add(Panel1)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(txtVoidby)
        Controls.Add(cmbCancelledby)
        Controls.Add(txtProductID)
        Controls.Add(btnCancelOrder)
        Controls.Add(txtReasons)
        Controls.Add(lblReasons)
        Controls.Add(txtCancelQuantity)
        Controls.Add(lblCancelQuantity)
        Controls.Add(lblAddToInventory)
        Controls.Add(cmbAddToInventory)
        Controls.Add(txtDiscount)
        Controls.Add(lblDiscount)
        Controls.Add(txtTotalPrice)
        Controls.Add(lblTotalPrice)
        Controls.Add(txtUnitPrice)
        Controls.Add(lblUnitPrice)
        Controls.Add(txtQuantity)
        Controls.Add(lblQuantity)
        Controls.Add(txtProductInfo)
        Controls.Add(lblProductInfo)
        Controls.Add(txtTransactionNumber)
        Controls.Add(lblTransactionNumber)
        Controls.Add(dgvTodaysSales)
        FormBorderStyle = FormBorderStyle.None
        Name = "TodaySales"
        StartPosition = FormStartPosition.CenterScreen
        Text = "TodaySales"
        CType(dgvTodaysSales, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvTodaysSales As DataGridView
    Friend WithEvents txtTransactionNumber As TextBox
    Friend WithEvents txtProductInfo As TextBox
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents txtUnitPrice As TextBox
    Friend WithEvents txtTotalPrice As TextBox
    Friend WithEvents txtDiscount As TextBox
    Friend WithEvents cmbAddToInventory As ComboBox ' ComboBox for Add to Inventory
    Friend WithEvents lblAddToInventory As Label ' Label for Add to Inventory
    Friend WithEvents lblCancelQuantity As Label ' Label for Cancel Quantity
    Friend WithEvents txtCancelQuantity As TextBox ' TextBox for Cancel Quantity
    Friend WithEvents lblReasons As Label ' Label for Reasons
    Friend WithEvents txtReasons As TextBox ' TextBox for Reasons

    ' New labels
    Friend WithEvents lblTransactionNumber As Label
    Friend WithEvents lblProductInfo As Label
    Friend WithEvents lblQuantity As Label
    Friend WithEvents lblUnitPrice As Label
    Friend WithEvents lblTotalPrice As Label
    Friend WithEvents lblDiscount As Label
    Friend WithEvents btnCancelOrder As Button
    Friend WithEvents txtProductID As TextBox
    Friend WithEvents cmbCancelledby As ComboBox
    Friend WithEvents txtVoidby As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnCLose As Button

End Class
