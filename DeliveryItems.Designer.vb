<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DeliveryItems
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
        txtQuantity = New TextBox()
        dgvPendingItems = New DataGridView()
        btnAddItem = New Button()
        btnRemoveItem = New Button()
        txtBarcodeScan = New TextBox()
        lblTransactionNumber = New Label()
        lblBarcode = New Label()
        lblQuantity = New Label()
        lblUnitPrice = New Label()
        lblTotalPrice = New Label()
        lblSupplier = New Label()
        lblProduct = New Label()
        lblBatchNumber = New Label()
        lblCostPrice = New Label()
        dpExpirationDate = New DateTimePicker()
        lblRetailPrice = New Label()
        Label1 = New Label()
        Panel1 = New Panel()
        btnConfirm = New Button()
        txtMinStockLevel = New TextBox()
        txtReorder = New TextBox()
        CType(dgvPendingItems, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtQuantity
        ' 
        txtQuantity.Location = New Point(150, 127)
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(280, 31)
        txtQuantity.TabIndex = 2
        ' 
        ' dgvPendingItems
        ' 
        dgvPendingItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPendingItems.Location = New Point(450, 30)
        dgvPendingItems.Name = "dgvPendingItems"
        dgvPendingItems.RowHeadersWidth = 62
        dgvPendingItems.Size = New Size(600, 200)
        dgvPendingItems.TabIndex = 5
        ' 
        ' btnAddItem
        ' 
        btnAddItem.Location = New Point(150, 367)
        btnAddItem.Name = "btnAddItem"
        btnAddItem.Size = New Size(280, 35)
        btnAddItem.TabIndex = 7
        btnAddItem.Text = "Add Item"
        btnAddItem.UseVisualStyleBackColor = True
        ' 
        ' btnRemoveItem
        ' 
        btnRemoveItem.Location = New Point(150, 407)
        btnRemoveItem.Name = "btnRemoveItem"
        btnRemoveItem.Size = New Size(280, 35)
        btnRemoveItem.TabIndex = 8
        btnRemoveItem.Text = "Remove Item"
        btnRemoveItem.UseVisualStyleBackColor = True
        ' 
        ' txtBarcodeScan
        ' 
        txtBarcodeScan.Location = New Point(150, 60)
        txtBarcodeScan.Name = "txtBarcodeScan"
        txtBarcodeScan.Size = New Size(280, 31)
        txtBarcodeScan.TabIndex = 1
        ' 
        ' lblTransactionNumber
        ' 
        lblTransactionNumber.AutoSize = True
        lblTransactionNumber.Location = New Point(20, 30)
        lblTransactionNumber.Name = "lblTransactionNumber"
        lblTransactionNumber.Size = New Size(120, 25)
        lblTransactionNumber.TabIndex = 9
        lblTransactionNumber.Text = "Transaction #:"
        ' 
        ' lblBarcode
        ' 
        lblBarcode.AutoSize = True
        lblBarcode.Location = New Point(20, 60)
        lblBarcode.Name = "lblBarcode"
        lblBarcode.Size = New Size(80, 25)
        lblBarcode.TabIndex = 10
        lblBarcode.Text = "Barcode:"
        ' 
        ' lblQuantity
        ' 
        lblQuantity.AutoSize = True
        lblQuantity.Location = New Point(20, 120)
        lblQuantity.Name = "lblQuantity"
        lblQuantity.Size = New Size(84, 25)
        lblQuantity.TabIndex = 11
        lblQuantity.Text = "Quantity:"
        ' 
        ' lblUnitPrice
        ' 
        lblUnitPrice.AutoSize = True
        lblUnitPrice.Location = New Point(20, 195)
        lblUnitPrice.Name = "lblUnitPrice"
        lblUnitPrice.Size = New Size(90, 25)
        lblUnitPrice.TabIndex = 12
        lblUnitPrice.Text = "Unit Price:"
        ' 
        ' lblTotalPrice
        ' 
        lblTotalPrice.AutoSize = True
        lblTotalPrice.Location = New Point(20, 255)
        lblTotalPrice.Name = "lblTotalPrice"
        lblTotalPrice.Size = New Size(95, 25)
        lblTotalPrice.TabIndex = 13
        lblTotalPrice.Text = "Total Price:"
        ' 
        ' lblSupplier
        ' 
        lblSupplier.AutoSize = True
        lblSupplier.Location = New Point(20, 90)
        lblSupplier.Name = "lblSupplier"
        lblSupplier.Size = New Size(81, 25)
        lblSupplier.TabIndex = 14
        lblSupplier.Text = "Supplier:"
        ' 
        ' lblProduct
        ' 
        lblProduct.AutoSize = True
        lblProduct.Location = New Point(26, 145)
        lblProduct.Name = "lblProduct"
        lblProduct.Size = New Size(78, 25)
        lblProduct.TabIndex = 15
        lblProduct.Text = "Product:"
        ' 
        ' lblBatchNumber
        ' 
        lblBatchNumber.AutoSize = True
        lblBatchNumber.Location = New Point(18, 297)
        lblBatchNumber.Name = "lblBatchNumber"
        lblBatchNumber.Size = New Size(129, 25)
        lblBatchNumber.TabIndex = 16
        lblBatchNumber.Text = "Batch Number:"
        ' 
        ' lblCostPrice
        ' 
        lblCostPrice.AutoSize = True
        lblCostPrice.Location = New Point(20, 170)
        lblCostPrice.Name = "lblCostPrice"
        lblCostPrice.Size = New Size(94, 25)
        lblCostPrice.TabIndex = 17
        lblCostPrice.Text = "Cost Price:"
        ' 
        ' dpExpirationDate
        ' 
        dpExpirationDate.Location = New Point(141, 17)
        dpExpirationDate.Name = "dpExpirationDate"
        dpExpirationDate.Size = New Size(280, 31)
        dpExpirationDate.TabIndex = 9
        ' 
        ' lblRetailPrice
        ' 
        lblRetailPrice.AutoSize = True
        lblRetailPrice.Location = New Point(24, 230)
        lblRetailPrice.Name = "lblRetailPrice"
        lblRetailPrice.Size = New Size(90, 25)
        lblRetailPrice.TabIndex = 18
        lblRetailPrice.Text = "Unit Price:"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(20, 23)
        Label1.Name = "Label1"
        Label1.Size = New Size(63, 25)
        Label1.TabIndex = 19
        Label1.Text = "Label1"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnConfirm)
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(dpExpirationDate)
        Panel1.Location = New Point(577, 274)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(434, 118)
        Panel1.TabIndex = 20
        ' 
        ' btnConfirm
        ' 
        btnConfirm.Location = New Point(309, 72)
        btnConfirm.Name = "btnConfirm"
        btnConfirm.Size = New Size(112, 34)
        btnConfirm.TabIndex = 20
        btnConfirm.Text = "Confirm"
        btnConfirm.UseVisualStyleBackColor = True
        ' 
        ' txtMinStockLevel
        ' 
        txtMinStockLevel.Location = New Point(204, 236)
        txtMinStockLevel.Name = "txtMinStockLevel"
        txtMinStockLevel.Size = New Size(280, 31)
        txtMinStockLevel.TabIndex = 21
        ' 
        ' txtReorder
        ' 
        txtReorder.Location = New Point(204, 273)
        txtReorder.Name = "txtReorder"
        txtReorder.Size = New Size(280, 31)
        txtReorder.TabIndex = 22
        ' 
        ' DeliveryItems
        ' 
        ClientSize = New Size(1083, 500)
        Controls.Add(txtReorder)
        Controls.Add(txtMinStockLevel)
        Controls.Add(Panel1)
        Controls.Add(lblRetailPrice)
        Controls.Add(lblBatchNumber)
        Controls.Add(lblProduct)
        Controls.Add(lblSupplier)
        Controls.Add(lblTotalPrice)
        Controls.Add(lblUnitPrice)
        Controls.Add(lblQuantity)
        Controls.Add(lblBarcode)
        Controls.Add(lblTransactionNumber)
        Controls.Add(txtBarcodeScan)
        Controls.Add(btnRemoveItem)
        Controls.Add(btnAddItem)
        Controls.Add(dgvPendingItems)
        Controls.Add(txtQuantity)
        Controls.Add(lblCostPrice)
        Name = "DeliveryItems"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Delivery Items"
        CType(dgvPendingItems, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    ' Declare the controls used in the form
    Private WithEvents txtQuantity As TextBox
    Private WithEvents dgvPendingItems As DataGridView
    Private WithEvents btnAddItem As Button
    Private WithEvents btnRemoveItem As Button
    Private WithEvents txtBarcodeScan As TextBox
    Private WithEvents lblTransactionNumber As Label
    Private WithEvents lblBarcode As Label
    Private WithEvents lblQuantity As Label
    Private WithEvents lblUnitPrice As Label
    Private WithEvents lblTotalPrice As Label
    Private WithEvents lblSupplier As Label
    Private WithEvents lblProduct As Label
    Private WithEvents lblBatchNumber As Label
    Private WithEvents lblCostPrice As Label
    Private WithEvents dpExpirationDate As DateTimePicker ' New DateTimePicker control
    Private WithEvents lblRetailPrice As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnConfirm As Button
    Private WithEvents txtMinStockLevel As TextBox
    Private WithEvents txtReorder As TextBox
End Class
