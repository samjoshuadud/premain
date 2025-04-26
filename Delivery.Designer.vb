<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Delivery
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

    'Note: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim btnClear As Button
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Delivery))
        lblTransactionNumber = New Label()
        lblSupplierID = New Label()
        cmbSupplierID = New ComboBox()
        lblProductID = New Label()
        cmbProductID = New ComboBox()
        lblQuantity = New Label()
        txtQuantity = New TextBox()
        lblSellingPrice = New Label()
        txtSellingPrice = New TextBox()
        lblCostPrice = New Label()
        txtCostPrice = New TextBox()
        lblExpirationDate = New Label()
        dtpExpirationDate = New DateTimePicker()
        lblDeliveryDate = New Label()
        dtpDeliveryDate = New DateTimePicker()
        btnConfirm = New Button()
        dgvPendingItems = New DataGridView()
        txtBarcode = New TextBox()
        lblBarcodeScan = New Label()
        btnSave = New Button()
        cmbUserFullName = New ComboBox()
        btnEdit = New Button()
        Label1 = New Label()
        btnCLose = New Button()
        prodtxt = New TextBox()
        txtReceivedBy = New TextBox()
        Button1 = New Button()
        btnClear = New Button()
        CType(dgvPendingItems, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnClear
        ' 
        btnClear.BackColor = SystemColors.ControlDarkDark
        btnClear.FlatStyle = FlatStyle.Flat
        btnClear.ForeColor = SystemColors.ButtonHighlight
        btnClear.Location = New Point(880, 422)
        btnClear.Margin = New Padding(2)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(104, 30)
        btnClear.TabIndex = 19
        btnClear.Text = "CLEAR"
        btnClear.UseVisualStyleBackColor = False
        ' 
        ' lblTransactionNumber
        ' 
        lblTransactionNumber.AutoSize = True
        lblTransactionNumber.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTransactionNumber.Location = New Point(8, 5)
        lblTransactionNumber.Margin = New Padding(2, 0, 2, 0)
        lblTransactionNumber.Name = "lblTransactionNumber"
        lblTransactionNumber.Size = New Size(170, 21)
        lblTransactionNumber.TabIndex = 0
        lblTransactionNumber.Text = "Transaction Number:"
        lblTransactionNumber.Visible = False
        ' 
        ' lblSupplierID
        ' 
        lblSupplierID.AutoSize = True
        lblSupplierID.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblSupplierID.Location = New Point(7, 106)
        lblSupplierID.Margin = New Padding(2, 0, 2, 0)
        lblSupplierID.Name = "lblSupplierID"
        lblSupplierID.Size = New Size(56, 13)
        lblSupplierID.TabIndex = 4
        lblSupplierID.Text = "Supplier :"
        ' 
        ' cmbSupplierID
        ' 
        cmbSupplierID.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSupplierID.FormattingEnabled = True
        cmbSupplierID.Location = New Point(109, 106)
        cmbSupplierID.Margin = New Padding(2)
        cmbSupplierID.Name = "cmbSupplierID"
        cmbSupplierID.Size = New Size(176, 23)
        cmbSupplierID.TabIndex = 5
        ' 
        ' lblProductID
        ' 
        lblProductID.AutoSize = True
        lblProductID.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblProductID.Location = New Point(8, 130)
        lblProductID.Margin = New Padding(2, 0, 2, 0)
        lblProductID.Name = "lblProductID"
        lblProductID.Size = New Size(54, 13)
        lblProductID.TabIndex = 6
        lblProductID.Text = "Product :"
        ' 
        ' cmbProductID
        ' 
        cmbProductID.DropDownStyle = ComboBoxStyle.DropDownList
        cmbProductID.FormattingEnabled = True
        cmbProductID.Location = New Point(109, 130)
        cmbProductID.Margin = New Padding(2)
        cmbProductID.Name = "cmbProductID"
        cmbProductID.Size = New Size(176, 23)
        cmbProductID.TabIndex = 7
        ' 
        ' lblQuantity
        ' 
        lblQuantity.AutoSize = True
        lblQuantity.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblQuantity.Location = New Point(295, 38)
        lblQuantity.Margin = New Padding(2, 0, 2, 0)
        lblQuantity.Name = "lblQuantity"
        lblQuantity.Size = New Size(55, 13)
        lblQuantity.TabIndex = 8
        lblQuantity.Text = "Quantity:"
        ' 
        ' txtQuantity
        ' 
        txtQuantity.Location = New Point(397, 38)
        txtQuantity.Margin = New Padding(2)
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(176, 23)
        txtQuantity.TabIndex = 9
        ' 
        ' lblSellingPrice
        ' 
        lblSellingPrice.AutoSize = True
        lblSellingPrice.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblSellingPrice.Location = New Point(296, 62)
        lblSellingPrice.Margin = New Padding(2, 0, 2, 0)
        lblSellingPrice.Name = "lblSellingPrice"
        lblSellingPrice.Size = New Size(60, 13)
        lblSellingPrice.TabIndex = 10
        lblSellingPrice.Text = "Unit Price:"
        ' 
        ' txtSellingPrice
        ' 
        txtSellingPrice.Location = New Point(397, 62)
        txtSellingPrice.Margin = New Padding(2)
        txtSellingPrice.Name = "txtSellingPrice"
        txtSellingPrice.Size = New Size(176, 23)
        txtSellingPrice.TabIndex = 11
        ' 
        ' lblCostPrice
        ' 
        lblCostPrice.AutoSize = True
        lblCostPrice.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblCostPrice.Location = New Point(296, 86)
        lblCostPrice.Margin = New Padding(2, 0, 2, 0)
        lblCostPrice.Name = "lblCostPrice"
        lblCostPrice.Size = New Size(61, 13)
        lblCostPrice.TabIndex = 12
        lblCostPrice.Text = "Cost Price:"
        ' 
        ' txtCostPrice
        ' 
        txtCostPrice.Location = New Point(397, 86)
        txtCostPrice.Margin = New Padding(2)
        txtCostPrice.Name = "txtCostPrice"
        txtCostPrice.Size = New Size(176, 23)
        txtCostPrice.TabIndex = 13
        ' 
        ' lblExpirationDate
        ' 
        lblExpirationDate.AutoSize = True
        lblExpirationDate.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblExpirationDate.Location = New Point(296, 110)
        lblExpirationDate.Margin = New Padding(2, 0, 2, 0)
        lblExpirationDate.Name = "lblExpirationDate"
        lblExpirationDate.Size = New Size(90, 13)
        lblExpirationDate.TabIndex = 14
        lblExpirationDate.Text = "Expiration Date:"
        ' 
        ' dtpExpirationDate
        ' 
        dtpExpirationDate.Format = DateTimePickerFormat.Short
        dtpExpirationDate.Location = New Point(397, 110)
        dtpExpirationDate.Margin = New Padding(2)
        dtpExpirationDate.Name = "dtpExpirationDate"
        dtpExpirationDate.Size = New Size(176, 23)
        dtpExpirationDate.TabIndex = 15
        dtpExpirationDate.Value = New Date(2025, 4, 25, 0, 0, 0, 0)
        ' 
        ' lblDeliveryDate
        ' 
        lblDeliveryDate.AutoSize = True
        lblDeliveryDate.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblDeliveryDate.Location = New Point(296, 134)
        lblDeliveryDate.Margin = New Padding(2, 0, 2, 0)
        lblDeliveryDate.Name = "lblDeliveryDate"
        lblDeliveryDate.Size = New Size(79, 13)
        lblDeliveryDate.TabIndex = 16
        lblDeliveryDate.Text = "Delivery Date:"
        ' 
        ' dtpDeliveryDate
        ' 
        dtpDeliveryDate.Format = DateTimePickerFormat.Short
        dtpDeliveryDate.Location = New Point(397, 134)
        dtpDeliveryDate.Margin = New Padding(2)
        dtpDeliveryDate.Name = "dtpDeliveryDate"
        dtpDeliveryDate.Size = New Size(176, 23)
        dtpDeliveryDate.TabIndex = 17
        ' 
        ' btnConfirm
        ' 
        btnConfirm.BackColor = SystemColors.ActiveCaptionText
        btnConfirm.FlatStyle = FlatStyle.Flat
        btnConfirm.ForeColor = SystemColors.ButtonHighlight
        btnConfirm.Location = New Point(583, 121)
        btnConfirm.Margin = New Padding(2)
        btnConfirm.Name = "btnConfirm"
        btnConfirm.Size = New Size(104, 30)
        btnConfirm.TabIndex = 18
        btnConfirm.Text = "Confirm"
        btnConfirm.UseVisualStyleBackColor = False
        ' 
        ' dgvPendingItems
        ' 
        dgvPendingItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPendingItems.Location = New Point(8, 157)
        dgvPendingItems.Margin = New Padding(2)
        dgvPendingItems.Name = "dgvPendingItems"
        dgvPendingItems.RowHeadersWidth = 62
        dgvPendingItems.Size = New Size(976, 262)
        dgvPendingItems.TabIndex = 20
        ' 
        ' txtBarcode
        ' 
        txtBarcode.Location = New Point(109, 38)
        txtBarcode.Margin = New Padding(2)
        txtBarcode.Name = "txtBarcode"
        txtBarcode.Size = New Size(176, 23)
        txtBarcode.TabIndex = 1
        ' 
        ' lblBarcodeScan
        ' 
        lblBarcodeScan.AutoSize = True
        lblBarcodeScan.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        lblBarcodeScan.Location = New Point(8, 38)
        lblBarcodeScan.Margin = New Padding(2, 0, 2, 0)
        lblBarcodeScan.Name = "lblBarcodeScan"
        lblBarcodeScan.Size = New Size(55, 13)
        lblBarcodeScan.TabIndex = 21
        lblBarcodeScan.Text = "Barcode :"
        ' 
        ' btnSave
        ' 
        btnSave.BackColor = SystemColors.ActiveCaptionText
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.ForeColor = SystemColors.ButtonHighlight
        btnSave.Location = New Point(663, 422)
        btnSave.Margin = New Padding(2)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(104, 30)
        btnSave.TabIndex = 22
        btnSave.Text = "SAVE"
        btnSave.UseVisualStyleBackColor = False
        btnSave.Visible = False
        ' 
        ' cmbUserFullName
        ' 
        cmbUserFullName.FormattingEnabled = True
        cmbUserFullName.Location = New Point(109, 83)
        cmbUserFullName.Margin = New Padding(2)
        cmbUserFullName.Name = "cmbUserFullName"
        cmbUserFullName.Size = New Size(176, 23)
        cmbUserFullName.TabIndex = 23
        cmbUserFullName.Visible = False
        ' 
        ' btnEdit
        ' 
        btnEdit.BackColor = SystemColors.ActiveCaptionText
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.ForeColor = SystemColors.ButtonHighlight
        btnEdit.Location = New Point(771, 422)
        btnEdit.Margin = New Padding(2)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(104, 30)
        btnEdit.TabIndex = 24
        btnEdit.Text = "EDIT"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        Label1.Location = New Point(8, 83)
        Label1.Margin = New Padding(2, 0, 2, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(75, 13)
        Label1.TabIndex = 25
        Label1.Text = "Received By :"
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(1194, 5)
        btnCLose.Margin = New Padding(2)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(46, 30)
        btnCLose.TabIndex = 70
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' prodtxt
        ' 
        prodtxt.Location = New Point(609, 38)
        prodtxt.Margin = New Padding(2)
        prodtxt.Name = "prodtxt"
        prodtxt.Size = New Size(176, 23)
        prodtxt.TabIndex = 71
        ' 
        ' txtReceivedBy
        ' 
        txtReceivedBy.Location = New Point(109, 83)
        txtReceivedBy.Margin = New Padding(2)
        txtReceivedBy.Name = "txtReceivedBy"
        txtReceivedBy.Size = New Size(176, 23)
        txtReceivedBy.TabIndex = 72
        ' 
        ' Button1
        ' 
        Button1.BackgroundImage = My.Resources.Resources.icons8_x_button_48
        Button1.Location = New Point(952, 14)
        Button1.Name = "Button1"
        Button1.Size = New Size(49, 47)
        Button1.TabIndex = 73
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Delivery
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1013, 458)
        Controls.Add(Button1)
        Controls.Add(txtReceivedBy)
        Controls.Add(prodtxt)
        Controls.Add(btnCLose)
        Controls.Add(Label1)
        Controls.Add(btnEdit)
        Controls.Add(cmbUserFullName)
        Controls.Add(btnSave)
        Controls.Add(txtBarcode)
        Controls.Add(lblBarcodeScan)
        Controls.Add(dgvPendingItems)
        Controls.Add(btnClear)
        Controls.Add(btnConfirm)
        Controls.Add(dtpDeliveryDate)
        Controls.Add(lblDeliveryDate)
        Controls.Add(dtpExpirationDate)
        Controls.Add(lblExpirationDate)
        Controls.Add(txtCostPrice)
        Controls.Add(lblCostPrice)
        Controls.Add(txtSellingPrice)
        Controls.Add(lblSellingPrice)
        Controls.Add(txtQuantity)
        Controls.Add(lblQuantity)
        Controls.Add(cmbProductID)
        Controls.Add(lblProductID)
        Controls.Add(cmbSupplierID)
        Controls.Add(lblSupplierID)
        Controls.Add(lblTransactionNumber)
        FormBorderStyle = FormBorderStyle.None
        Margin = New Padding(2)
        Name = "Delivery"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Delivery"
        CType(dgvPendingItems, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    'Form Controls
    Private WithEvents lblTransactionNumber As Label
    Private WithEvents lblSupplierID As Label
    Private WithEvents cmbSupplierID As ComboBox
    Private WithEvents lblProductID As Label
    Private WithEvents cmbProductID As ComboBox
    Private WithEvents lblQuantity As Label
    Private WithEvents txtQuantity As TextBox
    Private WithEvents lblSellingPrice As Label
    Private WithEvents txtSellingPrice As TextBox
    Private WithEvents lblCostPrice As Label
    Private WithEvents txtCostPrice As TextBox
    Private WithEvents lblExpirationDate As Label
    Private WithEvents dtpExpirationDate As DateTimePicker
    Private WithEvents lblDeliveryDate As Label
    Private WithEvents dtpDeliveryDate As DateTimePicker
    Private WithEvents btnConfirm As Button
    Private WithEvents btnClear As Button
    Friend WithEvents dgvPendingItems As DataGridView
    Private WithEvents txtBarcode As TextBox
    Private WithEvents lblBarcodeScan As Label
    Private WithEvents btnSave As Button
    Friend WithEvents cmbUserFullName As ComboBox
    Private WithEvents btnEdit As Button
    Private WithEvents Label1 As Label
    Friend WithEvents btnCLose As Button
    Private WithEvents prodtxt As TextBox
    Private WithEvents txtReceivedBy As TextBox
    Friend WithEvents Button1 As Button

End Class
