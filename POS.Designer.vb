<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class POS
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

    ' Windows Form Designer generated code
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(POS))
        Panel2 = New Panel()
        lblProductInformation = New Label()
        lblVatUI = New Label()
        lblProductInfo = New Label()
        Label10 = New Label()
        lblTotalItems = New Label()
        Label11 = New Label()
        lblChange = New Label()
        Panel12 = New Panel()
        Label7 = New Label()
        Panel11 = New Panel()
        Panel10 = New Panel()
        Panel3 = New Panel()
        lblTotal = New Label()
        Label5 = New Label()
        lblDiscount = New Label()
        Label8 = New Label()
        Label9 = New Label()
        lblSubtotal = New Label()
        lblVAT = New Label()
        lblTransactionNumber = New Label()
        dgvCart = New DataGridView()
        Label1 = New Label()
        pnlCashierItemScan = New Panel()
        lblDate = New Label()
        txtBarcode = New TextBox()
        PanelPay = New Panel()
        btnDot = New Button()
        btnClear2 = New Button()
        btn0 = New Button()
        btn1 = New Button()
        btn4 = New Button()
        btn34 = New Button()
        btn2 = New Button()
        Button1 = New Button()
        btn7 = New Button()
        btn8 = New Button()
        btn3 = New Button()
        btn6 = New Button()
        btnProcessPayment = New Button()
        Panel6 = New Panel()
        Button4 = New Button()
        Label2 = New Label()
        txtAmountPaid = New TextBox()
        btn9 = New Button()
        DiscountPanel = New Panel()
        Panel4 = New Panel()
        Button5 = New Button()
        Button2 = New Button()
        Label3 = New Label()
        cmbDiscount = New ComboBox()
        PanelQuantity = New Panel()
        Label4 = New Label()
        Panel5 = New Panel()
        Button6 = New Button()
        txtQuantity = New TextBox()
        btnClearcart = New Button()
        Panel1 = New Panel()
        Label6 = New Label()
        lblCashier = New Label()
        Button3 = New Button()
        Panel9 = New Panel()
        btnCancelOrder = New Button()
        Button7 = New Button()
        Button8 = New Button()
        Panel7 = New Panel()
        btnSelectProduct = New Button()
        txtProduct = New TextBox()
        Panel2.SuspendLayout()
        CType(dgvCart, ComponentModel.ISupportInitialize).BeginInit()
        pnlCashierItemScan.SuspendLayout()
        PanelPay.SuspendLayout()
        Panel6.SuspendLayout()
        DiscountPanel.SuspendLayout()
        Panel4.SuspendLayout()
        PanelQuantity.SuspendLayout()
        Panel5.SuspendLayout()
        Panel1.SuspendLayout()
        Panel7.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.White
        Panel2.BorderStyle = BorderStyle.Fixed3D
        Panel2.Controls.Add(lblProductInformation)
        Panel2.Controls.Add(lblVatUI)
        Panel2.Controls.Add(lblProductInfo)
        Panel2.Controls.Add(Label10)
        Panel2.Controls.Add(lblTotalItems)
        Panel2.Controls.Add(Label11)
        Panel2.Controls.Add(lblChange)
        Panel2.Controls.Add(Panel12)
        Panel2.Controls.Add(Label7)
        Panel2.Controls.Add(Panel11)
        Panel2.Controls.Add(Panel10)
        Panel2.Controls.Add(Panel3)
        Panel2.Controls.Add(lblTotal)
        Panel2.Controls.Add(Label5)
        Panel2.Controls.Add(lblDiscount)
        Panel2.Controls.Add(Label8)
        Panel2.Controls.Add(Label9)
        Panel2.Controls.Add(lblSubtotal)
        Panel2.Controls.Add(lblVAT)
        Panel2.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        Panel2.Location = New Point(870, 4)
        Panel2.Margin = New Padding(2, 2, 2, 2)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(376, 497)
        Panel2.TabIndex = 16
        ' 
        ' lblProductInformation
        ' 
        lblProductInformation.AutoSize = True
        lblProductInformation.Location = New Point(10, 311)
        lblProductInformation.Margin = New Padding(2, 0, 2, 0)
        lblProductInformation.Name = "lblProductInformation"
        lblProductInformation.Size = New Size(197, 25)
        lblProductInformation.TabIndex = 71
        lblProductInformation.Text = "Product Information"
        ' 
        ' lblVatUI
        ' 
        lblVatUI.AutoSize = True
        lblVatUI.BackColor = Color.Transparent
        lblVatUI.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblVatUI.ForeColor = SystemColors.ActiveCaptionText
        lblVatUI.Location = New Point(246, 129)
        lblVatUI.Margin = New Padding(2, 0, 2, 0)
        lblVatUI.Name = "lblVatUI"
        lblVatUI.Size = New Size(50, 25)
        lblVatUI.TabIndex = 20
        lblVatUI.Text = "12%"
        lblVatUI.Visible = False
        ' 
        ' lblProductInfo
        ' 
        lblProductInfo.AutoSize = True
        lblProductInfo.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductInfo.Location = New Point(10, 336)
        lblProductInfo.Margin = New Padding(2, 0, 2, 0)
        lblProductInfo.Name = "lblProductInfo"
        lblProductInfo.Size = New Size(50, 15)
        lblProductInfo.TabIndex = 70
        lblProductInfo.Text = "Label12"
        lblProductInfo.Visible = False
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.BackColor = Color.Transparent
        Label10.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label10.ForeColor = Color.Black
        Label10.Location = New Point(20, 235)
        Label10.Margin = New Padding(2, 0, 2, 0)
        Label10.Name = "Label10"
        Label10.Size = New Size(56, 13)
        Label10.TabIndex = 20
        Label10.Text = "CHANGE "
        ' 
        ' lblTotalItems
        ' 
        lblTotalItems.AutoSize = True
        lblTotalItems.BackColor = Color.Transparent
        lblTotalItems.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblTotalItems.ForeColor = Color.Black
        lblTotalItems.Location = New Point(246, 194)
        lblTotalItems.Margin = New Padding(2, 0, 2, 0)
        lblTotalItems.Name = "lblTotalItems"
        lblTotalItems.Size = New Size(23, 25)
        lblTotalItems.TabIndex = 5
        lblTotalItems.Text = "0"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.BackColor = Color.Transparent
        Label11.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label11.ForeColor = Color.Black
        Label11.Location = New Point(20, 203)
        Label11.Margin = New Padding(2, 0, 2, 0)
        Label11.Name = "Label11"
        Label11.Size = New Size(72, 13)
        Label11.TabIndex = 21
        Label11.Text = "TOTAL ITEM "
        ' 
        ' lblChange
        ' 
        lblChange.AutoSize = True
        lblChange.BackColor = Color.Transparent
        lblChange.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblChange.ForeColor = Color.Black
        lblChange.Location = New Point(246, 227)
        lblChange.Margin = New Padding(2, 0, 2, 0)
        lblChange.Name = "lblChange"
        lblChange.Size = New Size(23, 25)
        lblChange.TabIndex = 5
        lblChange.Text = "0"
        ' 
        ' Panel12
        ' 
        Panel12.BackColor = Color.LightGray
        Panel12.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Panel12.Location = New Point(10, 165)
        Panel12.Margin = New Padding(2, 2, 2, 2)
        Panel12.Name = "Panel12"
        Panel12.Size = New Size(350, 2)
        Panel12.TabIndex = 25
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.BackColor = Color.Transparent
        Label7.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label7.ForeColor = SystemColors.ActiveCaptionText
        Label7.Location = New Point(20, 136)
        Label7.Margin = New Padding(2, 0, 2, 0)
        Label7.Name = "Label7"
        Label7.Size = New Size(29, 13)
        Label7.TabIndex = 17
        Label7.Text = "VAT "
        ' 
        ' Panel11
        ' 
        Panel11.BackColor = Color.LightGray
        Panel11.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        Panel11.Location = New Point(10, 290)
        Panel11.Margin = New Padding(2, 2, 2, 2)
        Panel11.Name = "Panel11"
        Panel11.Size = New Size(350, 2)
        Panel11.TabIndex = 24
        ' 
        ' Panel10
        ' 
        Panel10.BackColor = Color.LightGray
        Panel10.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Panel10.Location = New Point(10, 122)
        Panel10.Margin = New Padding(2, 2, 2, 2)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(350, 2)
        Panel10.TabIndex = 23
        ' 
        ' Panel3
        ' 
        Panel3.BackColor = Color.LightGray
        Panel3.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Panel3.Location = New Point(10, 82)
        Panel3.Margin = New Padding(2, 2, 2, 2)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(350, 2)
        Panel3.TabIndex = 22
        ' 
        ' lblTotal
        ' 
        lblTotal.AutoSize = True
        lblTotal.BackColor = Color.Transparent
        lblTotal.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblTotal.ForeColor = Color.Red
        lblTotal.Location = New Point(246, 261)
        lblTotal.Margin = New Padding(2, 0, 2, 0)
        lblTotal.Name = "lblTotal"
        lblTotal.Size = New Size(23, 25)
        lblTotal.TabIndex = 4
        lblTotal.Text = "0"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.BackColor = Color.Transparent
        Label5.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label5.ForeColor = Color.Red
        Label5.Location = New Point(20, 268)
        Label5.Margin = New Padding(2, 0, 2, 0)
        Label5.Name = "Label5"
        Label5.Size = New Size(40, 13)
        Label5.TabIndex = 15
        Label5.Text = "TOTAL"
        ' 
        ' lblDiscount
        ' 
        lblDiscount.AutoSize = True
        lblDiscount.BackColor = Color.Transparent
        lblDiscount.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblDiscount.ForeColor = SystemColors.ActiveCaptionText
        lblDiscount.Location = New Point(246, 88)
        lblDiscount.Margin = New Padding(2, 0, 2, 0)
        lblDiscount.Name = "lblDiscount"
        lblDiscount.Size = New Size(23, 25)
        lblDiscount.TabIndex = 3
        lblDiscount.Text = "0"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.BackColor = Color.Transparent
        Label8.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label8.ForeColor = SystemColors.ActiveCaptionText
        Label8.Location = New Point(20, 98)
        Label8.Margin = New Padding(2, 0, 2, 0)
        Label8.Name = "Label8"
        Label8.Size = New Size(65, 13)
        Label8.TabIndex = 18
        Label8.Text = "DISCOUNT "
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.BackColor = Color.Transparent
        Label9.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label9.ForeColor = SystemColors.ActiveCaptionText
        Label9.Location = New Point(20, 56)
        Label9.Margin = New Padding(2, 0, 2, 0)
        Label9.Name = "Label9"
        Label9.Size = New Size(61, 13)
        Label9.TabIndex = 19
        Label9.Text = "SUBTOTAL"
        ' 
        ' lblSubtotal
        ' 
        lblSubtotal.AutoSize = True
        lblSubtotal.BackColor = Color.Transparent
        lblSubtotal.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblSubtotal.ForeColor = SystemColors.ActiveCaptionText
        lblSubtotal.Location = New Point(246, 48)
        lblSubtotal.Margin = New Padding(2, 0, 2, 0)
        lblSubtotal.Name = "lblSubtotal"
        lblSubtotal.Size = New Size(23, 25)
        lblSubtotal.TabIndex = 1
        lblSubtotal.Text = "0"
        ' 
        ' lblVAT
        ' 
        lblVAT.AutoSize = True
        lblVAT.BackColor = Color.Transparent
        lblVAT.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        lblVAT.ForeColor = SystemColors.ActiveCaptionText
        lblVAT.Location = New Point(246, 126)
        lblVAT.Margin = New Padding(2, 0, 2, 0)
        lblVAT.Name = "lblVAT"
        lblVAT.Size = New Size(23, 25)
        lblVAT.TabIndex = 2
        lblVAT.Text = "0"
        lblVAT.Visible = False
        ' 
        ' lblTransactionNumber
        ' 
        lblTransactionNumber.AutoSize = True
        lblTransactionNumber.BackColor = Color.Transparent
        lblTransactionNumber.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        lblTransactionNumber.ForeColor = Color.Red
        lblTransactionNumber.Location = New Point(6, 5)
        lblTransactionNumber.Margin = New Padding(2, 0, 2, 0)
        lblTransactionNumber.Name = "lblTransactionNumber"
        lblTransactionNumber.Size = New Size(161, 15)
        lblTransactionNumber.TabIndex = 4
        lblTransactionNumber.Text = "TRANSACTION NUMBER :"
        ' 
        ' dgvCart
        ' 
        dgvCart.AllowUserToAddRows = False
        dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvCart.BackgroundColor = SystemColors.ControlLight
        dgvCart.ColumnHeadersHeight = 34
        dgvCart.EnableHeadersVisualStyles = False
        dgvCart.GridColor = SystemColors.GrayText
        dgvCart.Location = New Point(2, 4)
        dgvCart.Margin = New Padding(2, 2, 2, 2)
        dgvCart.Name = "dgvCart"
        dgvCart.ReadOnly = True
        dgvCart.RowHeadersWidth = 62
        dgvCart.Size = New Size(864, 455)
        dgvCart.TabIndex = 0
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        Label1.Location = New Point(6, 33)
        Label1.Margin = New Padding(2, 0, 2, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(72, 15)
        Label1.TabIndex = 6
        Label1.Text = "BARCODE :"
        ' 
        ' pnlCashierItemScan
        ' 
        pnlCashierItemScan.BackColor = Color.White
        pnlCashierItemScan.BorderStyle = BorderStyle.FixedSingle
        pnlCashierItemScan.Controls.Add(lblDate)
        pnlCashierItemScan.Location = New Point(443, 13)
        pnlCashierItemScan.Margin = New Padding(2, 2, 2, 2)
        pnlCashierItemScan.Name = "pnlCashierItemScan"
        pnlCashierItemScan.Size = New Size(29, 15)
        pnlCashierItemScan.TabIndex = 0
        pnlCashierItemScan.Visible = False
        ' 
        ' lblDate
        ' 
        lblDate.AutoSize = True
        lblDate.Font = New Font("Arial", 12F)
        lblDate.ForeColor = Color.FromArgb(CByte(13), CByte(71), CByte(161))
        lblDate.Location = New Point(440, 2)
        lblDate.Margin = New Padding(2, 0, 2, 0)
        lblDate.Name = "lblDate"
        lblDate.Size = New Size(201, 18)
        lblDate.TabIndex = 1
        lblDate.Text = "Date: YYYY-MM-DD HH:MM"
        ' 
        ' txtBarcode
        ' 
        txtBarcode.Font = New Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtBarcode.Location = New Point(88, 28)
        txtBarcode.Margin = New Padding(2, 2, 2, 2)
        txtBarcode.Name = "txtBarcode"
        txtBarcode.PlaceholderText = "Scan Barcode"
        txtBarcode.Size = New Size(258, 29)
        txtBarcode.TabIndex = 1
        ' 
        ' PanelPay
        ' 
        PanelPay.BorderStyle = BorderStyle.Fixed3D
        PanelPay.Controls.Add(btnDot)
        PanelPay.Controls.Add(btnClear2)
        PanelPay.Controls.Add(btn0)
        PanelPay.Controls.Add(btn1)
        PanelPay.Controls.Add(btn4)
        PanelPay.Controls.Add(btn34)
        PanelPay.Controls.Add(btn2)
        PanelPay.Controls.Add(Button1)
        PanelPay.Controls.Add(btn7)
        PanelPay.Controls.Add(btn8)
        PanelPay.Controls.Add(btn3)
        PanelPay.Controls.Add(btn6)
        PanelPay.Controls.Add(btnProcessPayment)
        PanelPay.Controls.Add(Panel6)
        PanelPay.Controls.Add(Label2)
        PanelPay.Controls.Add(txtAmountPaid)
        PanelPay.Controls.Add(btn9)
        PanelPay.Location = New Point(10, 4)
        PanelPay.Margin = New Padding(2, 2, 2, 2)
        PanelPay.Name = "PanelPay"
        PanelPay.Size = New Size(392, 336)
        PanelPay.TabIndex = 16
        PanelPay.Visible = False
        ' 
        ' btnDot
        ' 
        btnDot.BackColor = Color.WhiteSmoke
        btnDot.FlatStyle = FlatStyle.Flat
        btnDot.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btnDot.ForeColor = SystemColors.ActiveCaptionText
        btnDot.Location = New Point(170, 272)
        btnDot.Margin = New Padding(2, 2, 2, 2)
        btnDot.Name = "btnDot"
        btnDot.Size = New Size(78, 48)
        btnDot.TabIndex = 23
        btnDot.Text = "."
        btnDot.UseVisualStyleBackColor = False
        ' 
        ' btnClear2
        ' 
        btnClear2.BackColor = Color.WhiteSmoke
        btnClear2.FlatStyle = FlatStyle.Flat
        btnClear2.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btnClear2.ForeColor = SystemColors.ActiveCaptionText
        btnClear2.Location = New Point(5, 272)
        btnClear2.Margin = New Padding(2, 2, 2, 2)
        btnClear2.Name = "btnClear2"
        btnClear2.Size = New Size(78, 48)
        btnClear2.TabIndex = 22
        btnClear2.Text = "C"
        btnClear2.UseVisualStyleBackColor = False
        ' 
        ' btn0
        ' 
        btn0.BackColor = Color.WhiteSmoke
        btn0.FlatStyle = FlatStyle.Flat
        btn0.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn0.ForeColor = SystemColors.ActiveCaptionText
        btn0.Location = New Point(88, 272)
        btn0.Margin = New Padding(2, 2, 2, 2)
        btn0.Name = "btn0"
        btn0.Size = New Size(78, 48)
        btn0.TabIndex = 21
        btn0.Text = "0"
        btn0.UseVisualStyleBackColor = False
        ' 
        ' btn1
        ' 
        btn1.BackColor = Color.WhiteSmoke
        btn1.FlatStyle = FlatStyle.Flat
        btn1.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn1.ForeColor = SystemColors.ActiveCaptionText
        btn1.Location = New Point(170, 222)
        btn1.Margin = New Padding(2, 2, 2, 2)
        btn1.Name = "btn1"
        btn1.Size = New Size(78, 48)
        btn1.TabIndex = 19
        btn1.Text = "1"
        btn1.UseVisualStyleBackColor = False
        ' 
        ' btn4
        ' 
        btn4.BackColor = Color.WhiteSmoke
        btn4.FlatStyle = FlatStyle.Flat
        btn4.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn4.ForeColor = SystemColors.ActiveCaptionText
        btn4.Location = New Point(170, 170)
        btn4.Margin = New Padding(2, 2, 2, 2)
        btn4.Name = "btn4"
        btn4.Size = New Size(78, 48)
        btn4.TabIndex = 18
        btn4.Text = "4"
        btn4.UseVisualStyleBackColor = False
        ' 
        ' btn34
        ' 
        btn34.BackColor = Color.WhiteSmoke
        btn34.FlatStyle = FlatStyle.Flat
        btn34.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn34.ForeColor = SystemColors.ActiveCaptionText
        btn34.Location = New Point(170, 119)
        btn34.Margin = New Padding(2, 2, 2, 2)
        btn34.Name = "btn34"
        btn34.Size = New Size(78, 48)
        btn34.TabIndex = 17
        btn34.Text = "7"
        btn34.UseVisualStyleBackColor = False
        ' 
        ' btn2
        ' 
        btn2.BackColor = Color.WhiteSmoke
        btn2.FlatStyle = FlatStyle.Flat
        btn2.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn2.ForeColor = SystemColors.ActiveCaptionText
        btn2.Location = New Point(88, 222)
        btn2.Margin = New Padding(2, 2, 2, 2)
        btn2.Name = "btn2"
        btn2.Size = New Size(78, 48)
        btn2.TabIndex = 15
        btn2.Text = "2"
        btn2.UseVisualStyleBackColor = False
        ' 
        ' Button1
        ' 
        Button1.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        Button1.Image = CType(resources.GetObject("Button1.Image"), Image)
        Button1.ImageAlign = ContentAlignment.MiddleLeft
        Button1.Location = New Point(255, 237)
        Button1.Margin = New Padding(2, 2, 2, 2)
        Button1.Name = "Button1"
        Button1.Size = New Size(125, 32)
        Button1.TabIndex = 12
        Button1.Text = "         DISCOUNT"
        ' 
        ' btn7
        ' 
        btn7.BackColor = Color.WhiteSmoke
        btn7.FlatStyle = FlatStyle.Flat
        btn7.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn7.ForeColor = SystemColors.ActiveCaptionText
        btn7.Location = New Point(88, 170)
        btn7.Margin = New Padding(2, 2, 2, 2)
        btn7.Name = "btn7"
        btn7.Size = New Size(78, 48)
        btn7.TabIndex = 14
        btn7.Text = "5"
        btn7.UseVisualStyleBackColor = False
        ' 
        ' btn8
        ' 
        btn8.BackColor = Color.WhiteSmoke
        btn8.FlatStyle = FlatStyle.Flat
        btn8.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn8.ForeColor = SystemColors.ActiveCaptionText
        btn8.Location = New Point(89, 119)
        btn8.Margin = New Padding(2, 2, 2, 2)
        btn8.Name = "btn8"
        btn8.Size = New Size(78, 48)
        btn8.TabIndex = 13
        btn8.Text = "8"
        btn8.UseVisualStyleBackColor = False
        ' 
        ' btn3
        ' 
        btn3.BackColor = Color.WhiteSmoke
        btn3.FlatStyle = FlatStyle.Flat
        btn3.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn3.ForeColor = SystemColors.ActiveCaptionText
        btn3.Location = New Point(5, 222)
        btn3.Margin = New Padding(2, 2, 2, 2)
        btn3.Name = "btn3"
        btn3.Size = New Size(78, 48)
        btn3.TabIndex = 11
        btn3.Text = "3"
        btn3.UseVisualStyleBackColor = False
        ' 
        ' btn6
        ' 
        btn6.BackColor = Color.WhiteSmoke
        btn6.FlatStyle = FlatStyle.Flat
        btn6.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn6.ForeColor = SystemColors.ActiveCaptionText
        btn6.Location = New Point(5, 170)
        btn6.Margin = New Padding(2, 2, 2, 2)
        btn6.Name = "btn6"
        btn6.Size = New Size(78, 48)
        btn6.TabIndex = 10
        btn6.Text = "6"
        btn6.UseVisualStyleBackColor = False
        ' 
        ' btnProcessPayment
        ' 
        btnProcessPayment.BackColor = Color.FromArgb(CByte(0), CByte(192), CByte(0))
        btnProcessPayment.FlatStyle = FlatStyle.Flat
        btnProcessPayment.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnProcessPayment.ForeColor = SystemColors.ButtonHighlight
        btnProcessPayment.Location = New Point(253, 273)
        btnProcessPayment.Margin = New Padding(2, 2, 2, 2)
        btnProcessPayment.Name = "btnProcessPayment"
        btnProcessPayment.Size = New Size(127, 46)
        btnProcessPayment.TabIndex = 3
        btnProcessPayment.Text = "PROCESS PAYMENT"
        btnProcessPayment.UseVisualStyleBackColor = False
        ' 
        ' Panel6
        ' 
        Panel6.BackColor = Color.Gainsboro
        Panel6.Controls.Add(Button4)
        Panel6.Dock = DockStyle.Top
        Panel6.Location = New Point(0, 0)
        Panel6.Margin = New Padding(2, 2, 2, 2)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(388, 31)
        Panel6.TabIndex = 8
        ' 
        ' Button4
        ' 
        Button4.BackColor = Color.Transparent
        Button4.FlatAppearance.BorderSize = 0
        Button4.FlatStyle = FlatStyle.Flat
        Button4.Image = CType(resources.GetObject("Button4.Image"), Image)
        Button4.Location = New Point(342, 2)
        Button4.Margin = New Padding(2, 2, 2, 2)
        Button4.Name = "Button4"
        Button4.Size = New Size(43, 25)
        Button4.TabIndex = 67
        Button4.UseVisualStyleBackColor = False
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(5, 30)
        Label2.Margin = New Padding(2, 0, 2, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(200, 32)
        Label2.TabIndex = 7
        Label2.Text = "AMOUNT PAID :"
        ' 
        ' txtAmountPaid
        ' 
        txtAmountPaid.BackColor = SystemColors.GradientInactiveCaption
        txtAmountPaid.Font = New Font("Arial", 28F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtAmountPaid.Location = New Point(5, 67)
        txtAmountPaid.Margin = New Padding(2, 2, 2, 2)
        txtAmountPaid.Name = "txtAmountPaid"
        txtAmountPaid.Size = New Size(376, 50)
        txtAmountPaid.TabIndex = 2
        txtAmountPaid.TextAlign = HorizontalAlignment.Center
        ' 
        ' btn9
        ' 
        btn9.BackColor = Color.WhiteSmoke
        btn9.FlatStyle = FlatStyle.Flat
        btn9.Font = New Font("Segoe UI Black", 9F, FontStyle.Bold)
        btn9.ForeColor = SystemColors.ActiveCaptionText
        btn9.Location = New Point(5, 119)
        btn9.Margin = New Padding(2, 2, 2, 2)
        btn9.Name = "btn9"
        btn9.Size = New Size(78, 48)
        btn9.TabIndex = 9
        btn9.Text = "9"
        btn9.UseVisualStyleBackColor = False
        ' 
        ' DiscountPanel
        ' 
        DiscountPanel.Controls.Add(Panel4)
        DiscountPanel.Controls.Add(Label3)
        DiscountPanel.Controls.Add(cmbDiscount)
        DiscountPanel.Location = New Point(428, 25)
        DiscountPanel.Margin = New Padding(2, 2, 2, 2)
        DiscountPanel.Name = "DiscountPanel"
        DiscountPanel.Size = New Size(344, 129)
        DiscountPanel.TabIndex = 24
        DiscountPanel.Visible = False
        ' 
        ' Panel4
        ' 
        Panel4.BackColor = Color.Gainsboro
        Panel4.Controls.Add(Button5)
        Panel4.Controls.Add(Button2)
        Panel4.Dock = DockStyle.Top
        Panel4.Location = New Point(0, 0)
        Panel4.Margin = New Padding(2, 2, 2, 2)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(344, 34)
        Panel4.TabIndex = 13
        ' 
        ' Button5
        ' 
        Button5.BackColor = Color.Transparent
        Button5.FlatAppearance.BorderSize = 0
        Button5.FlatStyle = FlatStyle.Flat
        Button5.Image = CType(resources.GetObject("Button5.Image"), Image)
        Button5.Location = New Point(299, 4)
        Button5.Margin = New Padding(2, 2, 2, 2)
        Button5.Name = "Button5"
        Button5.Size = New Size(43, 25)
        Button5.TabIndex = 68
        Button5.UseVisualStyleBackColor = False
        ' 
        ' Button2
        ' 
        Button2.BackColor = Color.Transparent
        Button2.FlatAppearance.BorderSize = 0
        Button2.FlatStyle = FlatStyle.Flat
        Button2.Image = CType(resources.GetObject("Button2.Image"), Image)
        Button2.Location = New Point(342, 1)
        Button2.Margin = New Padding(2, 2, 2, 2)
        Button2.Name = "Button2"
        Button2.Size = New Size(43, 25)
        Button2.TabIndex = 67
        Button2.UseVisualStyleBackColor = False
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(22, 46)
        Label3.Margin = New Padding(2, 0, 2, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(204, 32)
        Label3.TabIndex = 12
        Label3.Text = "Select Discount :"
        ' 
        ' cmbDiscount
        ' 
        cmbDiscount.Font = New Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbDiscount.FormattingEnabled = True
        cmbDiscount.Location = New Point(22, 76)
        cmbDiscount.Margin = New Padding(2, 2, 2, 2)
        cmbDiscount.Name = "cmbDiscount"
        cmbDiscount.Size = New Size(300, 40)
        cmbDiscount.TabIndex = 10
        ' 
        ' PanelQuantity
        ' 
        PanelQuantity.Controls.Add(Label4)
        PanelQuantity.Controls.Add(Panel5)
        PanelQuantity.Controls.Add(txtQuantity)
        PanelQuantity.Location = New Point(428, 208)
        PanelQuantity.Margin = New Padding(2, 2, 2, 2)
        PanelQuantity.Name = "PanelQuantity"
        PanelQuantity.Size = New Size(267, 140)
        PanelQuantity.TabIndex = 14
        PanelQuantity.Visible = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.BackColor = Color.Transparent
        Label4.Font = New Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(10, 52)
        Label4.Margin = New Padding(2, 0, 2, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(124, 29)
        Label4.TabIndex = 11
        Label4.Text = "Quantity :"
        ' 
        ' Panel5
        ' 
        Panel5.BackColor = Color.Gainsboro
        Panel5.Controls.Add(Button6)
        Panel5.Dock = DockStyle.Top
        Panel5.Location = New Point(0, 0)
        Panel5.Margin = New Padding(2, 2, 2, 2)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(267, 35)
        Panel5.TabIndex = 69
        ' 
        ' Button6
        ' 
        Button6.BackColor = Color.Transparent
        Button6.FlatAppearance.BorderSize = 0
        Button6.FlatStyle = FlatStyle.Flat
        Button6.Image = CType(resources.GetObject("Button6.Image"), Image)
        Button6.Location = New Point(220, 4)
        Button6.Margin = New Padding(2, 2, 2, 2)
        Button6.Name = "Button6"
        Button6.Size = New Size(43, 25)
        Button6.TabIndex = 67
        Button6.UseVisualStyleBackColor = False
        ' 
        ' txtQuantity
        ' 
        txtQuantity.BackColor = SystemColors.GradientInactiveCaption
        txtQuantity.Font = New Font("Arial", 72F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtQuantity.Location = New Point(6, 88)
        txtQuantity.Margin = New Padding(2, 2, 2, 2)
        txtQuantity.Multiline = True
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(255, 45)
        txtQuantity.TabIndex = 9
        txtQuantity.TextAlign = HorizontalAlignment.Center
        ' 
        ' btnClearcart
        ' 
        btnClearcart.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        btnClearcart.Image = CType(resources.GetObject("btnClearcart.Image"), Image)
        btnClearcart.ImageAlign = ContentAlignment.MiddleLeft
        btnClearcart.Location = New Point(812, 503)
        btnClearcart.Margin = New Padding(2, 2, 2, 2)
        btnClearcart.Name = "btnClearcart"
        btnClearcart.Size = New Size(141, 65)
        btnClearcart.TabIndex = 4
        btnClearcart.Text = " NEW TRANSAC" & vbCrLf & "          ( F3 )" & vbCrLf
        btnClearcart.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.Transparent
        Panel1.BorderStyle = BorderStyle.Fixed3D
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(pnlCashierItemScan)
        Panel1.Controls.Add(txtBarcode)
        Panel1.Controls.Add(lblTransactionNumber)
        Panel1.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Panel1.ForeColor = SystemColors.ActiveCaptionText
        Panel1.Location = New Point(8, 505)
        Panel1.Margin = New Padding(2, 2, 2, 2)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(508, 64)
        Panel1.TabIndex = 11
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Label6.Location = New Point(76, 295)
        Label6.Margin = New Padding(2, 0, 2, 0)
        Label6.Name = "Label6"
        Label6.Size = New Size(50, 21)
        Label6.TabIndex = 16
        Label6.Text = "USER"
        Label6.Visible = False
        ' 
        ' lblCashier
        ' 
        lblCashier.AutoSize = True
        lblCashier.Font = New Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCashier.ForeColor = Color.Black
        lblCashier.Location = New Point(8, 326)
        lblCashier.Margin = New Padding(2, 0, 2, 0)
        lblCashier.Name = "lblCashier"
        lblCashier.Size = New Size(102, 30)
        lblCashier.TabIndex = 2
        lblCashier.Text = "Cashier: "
        lblCashier.Visible = False
        ' 
        ' Button3
        ' 
        Button3.BackColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        Button3.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        Button3.ForeColor = Color.Black
        Button3.Image = CType(resources.GetObject("Button3.Image"), Image)
        Button3.ImageAlign = ContentAlignment.MiddleLeft
        Button3.Location = New Point(958, 503)
        Button3.Margin = New Padding(2, 2, 2, 2)
        Button3.Name = "Button3"
        Button3.Size = New Size(141, 65)
        Button3.TabIndex = 17
        Button3.Text = "         PAY" & vbCrLf & "        ( F4 )"
        Button3.UseVisualStyleBackColor = False
        ' 
        ' Panel9
        ' 
        Panel9.Location = New Point(10, 397)
        Panel9.Margin = New Padding(2, 2, 2, 2)
        Panel9.Name = "Panel9"
        Panel9.Size = New Size(735, 56)
        Panel9.TabIndex = 22
        ' 
        ' btnCancelOrder
        ' 
        btnCancelOrder.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        btnCancelOrder.Image = CType(resources.GetObject("btnCancelOrder.Image"), Image)
        btnCancelOrder.ImageAlign = ContentAlignment.MiddleLeft
        btnCancelOrder.Location = New Point(667, 503)
        btnCancelOrder.Margin = New Padding(2, 2, 2, 2)
        btnCancelOrder.Name = "btnCancelOrder"
        btnCancelOrder.Size = New Size(141, 65)
        btnCancelOrder.TabIndex = 23
        btnCancelOrder.Text = "DAILY SALES" & vbCrLf & "                        ( F2 )" & vbCrLf
        btnCancelOrder.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Button7
        ' 
        Button7.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        Button7.Image = CType(resources.GetObject("Button7.Image"), Image)
        Button7.ImageAlign = ContentAlignment.MiddleLeft
        Button7.Location = New Point(529, 503)
        Button7.Margin = New Padding(2, 2, 2, 2)
        Button7.Name = "Button7"
        Button7.Size = New Size(134, 65)
        Button7.TabIndex = 24
        Button7.Text = "                  CLEAR CART" & vbCrLf & "                        ( F1 )" & vbCrLf
        Button7.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' Button8
        ' 
        Button8.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        Button8.Image = CType(resources.GetObject("Button8.Image"), Image)
        Button8.ImageAlign = ContentAlignment.MiddleLeft
        Button8.Location = New Point(1097, 503)
        Button8.Margin = New Padding(2, 2, 2, 2)
        Button8.Name = "Button8"
        Button8.Size = New Size(141, 65)
        Button8.TabIndex = 25
        Button8.Text = "               EXIT SALES" & vbCrLf & "                   ( F5 )"
        ' 
        ' Panel7
        ' 
        Panel7.Controls.Add(btnSelectProduct)
        Panel7.Controls.Add(txtProduct)
        Panel7.Location = New Point(2, 462)
        Panel7.Margin = New Padding(2, 2, 2, 2)
        Panel7.Name = "Panel7"
        Panel7.Size = New Size(864, 40)
        Panel7.TabIndex = 26
        ' 
        ' btnSelectProduct
        ' 
        btnSelectProduct.Font = New Font("Segoe UI Black", 8F, FontStyle.Bold)
        btnSelectProduct.ImageAlign = ContentAlignment.MiddleLeft
        btnSelectProduct.Location = New Point(736, 5)
        btnSelectProduct.Margin = New Padding(2, 2, 2, 2)
        btnSelectProduct.Name = "btnSelectProduct"
        btnSelectProduct.Size = New Size(125, 32)
        btnSelectProduct.TabIndex = 24
        btnSelectProduct.Text = "SELECT PRODUCT"
        ' 
        ' txtProduct
        ' 
        txtProduct.Font = New Font("Arial", 14F)
        txtProduct.Location = New Point(8, 5)
        txtProduct.Margin = New Padding(2, 2, 2, 2)
        txtProduct.Name = "txtProduct"
        txtProduct.Size = New Size(725, 29)
        txtProduct.TabIndex = 0
        ' 
        ' POS
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.White
        ClientSize = New Size(1557, 882)
        Controls.Add(Panel7)
        Controls.Add(PanelQuantity)
        Controls.Add(Button8)
        Controls.Add(Button7)
        Controls.Add(PanelPay)
        Controls.Add(DiscountPanel)
        Controls.Add(btnCancelOrder)
        Controls.Add(Button3)
        Controls.Add(btnClearcart)
        Controls.Add(Panel1)
        Controls.Add(Panel2)
        Controls.Add(dgvCart)
        Controls.Add(Label6)
        Controls.Add(lblCashier)
        Controls.Add(Panel9)
        FormBorderStyle = FormBorderStyle.None
        Margin = New Padding(2, 2, 2, 2)
        Name = "POS"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Point of Sale"
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        CType(dgvCart, ComponentModel.ISupportInitialize).EndInit()
        pnlCashierItemScan.ResumeLayout(False)
        pnlCashierItemScan.PerformLayout()
        PanelPay.ResumeLayout(False)
        PanelPay.PerformLayout()
        Panel6.ResumeLayout(False)
        DiscountPanel.ResumeLayout(False)
        DiscountPanel.PerformLayout()
        Panel4.ResumeLayout(False)
        PanelQuantity.ResumeLayout(False)
        PanelQuantity.PerformLayout()
        Panel5.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel7.ResumeLayout(False)
        Panel7.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents PanelQuantity As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents txtQuantity As TextBox
    Private WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbDiscount As ComboBox
    Private WithEvents lblTransactionNumber As Label
    Private WithEvents txtBarcode As TextBox
    Private WithEvents pnlCashierItemScan As Panel
    Private WithEvents lblDate As Label
    Private WithEvents Button1 As Button
    Friend WithEvents btnProcessPayment As Button
    Private WithEvents btnClearcart As Button
    Private WithEvents dgvCart As DataGridView
    Friend WithEvents Panel1 As Panel
    Private WithEvents Label10 As Label
    Private WithEvents lblChange As Label
    Private WithEvents Label6 As Label
    Private WithEvents lblTotalItems As Label
    Private WithEvents lblCashier As Label
    Friend WithEvents PanelPay As Panel
    Friend WithEvents Panel6 As Panel
    Private WithEvents Label2 As Label
    Private WithEvents txtAmountPaid As TextBox
    Friend WithEvents Button4 As Button
    Private WithEvents Button3 As Button
    Private WithEvents Label11 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Button6 As Button
    Friend WithEvents Panel9 As Panel
    Private WithEvents btnCancelOrder As Button
    Private WithEvents Label7 As Label
    Private WithEvents lblVatUI As Label
    Private WithEvents Label8 As Label
    Private WithEvents lblVAT As Label
    Private WithEvents Label9 As Label
    Private WithEvents lblSubtotal As Label
    Private WithEvents lblDiscount As Label
    Private WithEvents Label5 As Label
    Private WithEvents lblTotal As Label
    Friend WithEvents btn1 As Button
    Friend WithEvents btn4 As Button
    Friend WithEvents btn34 As Button
    Friend WithEvents btn2 As Button
    Friend WithEvents btn7 As Button
    Friend WithEvents btn8 As Button
    Friend WithEvents btn3 As Button
    Friend WithEvents btn6 As Button
    Friend WithEvents btn9 As Button
    Friend WithEvents btn0 As Button
    Friend WithEvents btnClear2 As Button
    Friend WithEvents btnDot As Button
    Friend WithEvents DiscountPanel As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Button5 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Panel10 As Panel
    Private WithEvents Button7 As Button
    Private WithEvents Button8 As Button
    Friend WithEvents lblProductInfo As Label
    Friend WithEvents Panel12 As Panel
    Friend WithEvents lblProductInformation As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents txtProduct As TextBox
    Private WithEvents btnSelectProduct As Button




End Class

