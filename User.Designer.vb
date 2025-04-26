<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class User
    Inherits System.Windows.Forms.Form

    ' Form overrides dispose to clean up the component list.
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

    ' Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Windows Form Designer
    ' It can be modified using the Windows Form Designer.
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(User))
        dgvUsers = New DataGridView()
        ToolTip1 = New ToolTip(components)
        Label2 = New Label()
        lblAddress = New Label()
        lblEmail = New Label()
        lblRole = New Label()
        lblRequiredFields = New Label()
        txtAge = New TextBox()
        txtFirstName = New TextBox()
        txtMiddleInitial = New TextBox()
        lblFirstName = New Label()
        lblMiddleInitial = New Label()
        lblLastName = New Label()
        txtLastName = New TextBox()
        lblAge = New Label()
        lblGender = New Label()
        cbGender = New ComboBox()
        UserAddPanel = New Panel()
        txtAddress = New TextBox()
        txtEmailAddress = New TextBox()
        btnDelete = New Button()
        btnReset = New Button()
        btnEdit = New Button()
        btnAdd = New Button()
        Label4 = New Label()
        cbRole = New ComboBox()
        txtConfirmPassword = New TextBox()
        txtPassword = New TextBox()
        Label5 = New Label()
        Label1 = New Label()
        txtUsername = New TextBox()
        txtPhoneNumber = New TextBox()
        Panel2 = New Panel()
        btnClosePanel = New Button()
        txtSearch = New TextBox()
        btnCLose = New Button()
        btnAddUser = New Button()
        Label3 = New Label()
        Panel1 = New Panel()
        CType(dgvUsers, ComponentModel.ISupportInitialize).BeginInit()
        UserAddPanel.SuspendLayout()
        Panel2.SuspendLayout()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvUsers
        ' 
        dgvUsers.AllowUserToAddRows = False
        dgvUsers.AllowUserToDeleteRows = False
        dgvUsers.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvUsers.BackgroundColor = Color.Silver
        dgvUsers.BorderStyle = BorderStyle.None
        dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvUsers.Location = New Point(13, 84)
        dgvUsers.Margin = New Padding(4, 5, 4, 5)
        dgvUsers.Name = "dgvUsers"
        dgvUsers.ReadOnly = True
        dgvUsers.RowHeadersWidth = 62
        dgvUsers.Size = New Size(1732, 872)
        dgvUsers.TabIndex = 0
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label2.ForeColor = SystemColors.ActiveCaptionText
        Label2.Location = New Point(28, 400)
        Label2.Margin = New Padding(4, 0, 4, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(250, 40)
        Label2.TabIndex = 73
        Label2.Text = "Phone # :"
        ' 
        ' lblAddress
        ' 
        lblAddress.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblAddress.BackColor = Color.Transparent
        lblAddress.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblAddress.ForeColor = SystemColors.ActiveCaptionText
        lblAddress.Location = New Point(28, 444)
        lblAddress.Margin = New Padding(4, 0, 4, 0)
        lblAddress.Name = "lblAddress"
        lblAddress.Size = New Size(463, 40)
        lblAddress.TabIndex = 72
        lblAddress.Text = "Username :"
        ' 
        ' lblEmail
        ' 
        lblEmail.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblEmail.BackColor = Color.Transparent
        lblEmail.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblEmail.ForeColor = SystemColors.ActiveCaptionText
        lblEmail.Location = New Point(30, 315)
        lblEmail.Margin = New Padding(4, 0, 4, 0)
        lblEmail.Name = "lblEmail"
        lblEmail.Size = New Size(463, 40)
        lblEmail.TabIndex = 70
        lblEmail.Text = "Email :"
        ' 
        ' lblRole
        ' 
        lblRole.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblRole.BackColor = Color.Transparent
        lblRole.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblRole.ForeColor = SystemColors.ActiveCaptionText
        lblRole.Location = New Point(28, 355)
        lblRole.Margin = New Padding(4, 0, 4, 0)
        lblRole.Name = "lblRole"
        lblRole.Size = New Size(463, 40)
        lblRole.TabIndex = 71
        lblRole.Text = "Role :"
        ' 
        ' lblRequiredFields
        ' 
        lblRequiredFields.AutoSize = True
        lblRequiredFields.Font = New Font("Segoe UI", 9F, FontStyle.Italic)
        lblRequiredFields.ForeColor = Color.Red
        lblRequiredFields.Location = New Point(258, 58)
        lblRequiredFields.Margin = New Padding(4, 0, 4, 0)
        lblRequiredFields.Name = "lblRequiredFields"
        lblRequiredFields.Size = New Size(286, 25)
        lblRequiredFields.TabIndex = 58
        lblRequiredFields.Text = "* Fields marked with * are required"
        ' 
        ' txtAge
        ' 
        txtAge.BorderStyle = BorderStyle.FixedSingle
        txtAge.Font = New Font("Segoe UI", 10F)
        txtAge.Location = New Point(256, 229)
        txtAge.Margin = New Padding(4, 5, 4, 5)
        txtAge.MaxLength = 3
        txtAge.Name = "txtAge"
        txtAge.Size = New Size(625, 34)
        txtAge.TabIndex = 56
        ' 
        ' txtFirstName
        ' 
        txtFirstName.BorderStyle = BorderStyle.FixedSingle
        txtFirstName.Font = New Font("Segoe UI", 10F)
        txtFirstName.Location = New Point(257, 99)
        txtFirstName.Margin = New Padding(4, 5, 4, 5)
        txtFirstName.Name = "txtFirstName"
        txtFirstName.Size = New Size(625, 34)
        txtFirstName.TabIndex = 50
        ' 
        ' txtMiddleInitial
        ' 
        txtMiddleInitial.BorderStyle = BorderStyle.FixedSingle
        txtMiddleInitial.Font = New Font("Segoe UI", 10F)
        txtMiddleInitial.Location = New Point(256, 187)
        txtMiddleInitial.Margin = New Padding(4, 5, 4, 5)
        txtMiddleInitial.Name = "txtMiddleInitial"
        txtMiddleInitial.Size = New Size(625, 34)
        txtMiddleInitial.TabIndex = 52
        ' 
        ' lblFirstName
        ' 
        lblFirstName.BackColor = Color.Transparent
        lblFirstName.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblFirstName.ForeColor = SystemColors.ActiveCaptionText
        lblFirstName.Location = New Point(29, 99)
        lblFirstName.Margin = New Padding(4, 0, 4, 0)
        lblFirstName.Name = "lblFirstName"
        lblFirstName.Size = New Size(157, 40)
        lblFirstName.TabIndex = 57
        lblFirstName.Text = "First Name :"
        ' 
        ' lblMiddleInitial
        ' 
        lblMiddleInitial.BackColor = Color.Transparent
        lblMiddleInitial.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblMiddleInitial.ForeColor = SystemColors.ActiveCaptionText
        lblMiddleInitial.Location = New Point(30, 188)
        lblMiddleInitial.Margin = New Padding(4, 0, 4, 0)
        lblMiddleInitial.Name = "lblMiddleInitial"
        lblMiddleInitial.Size = New Size(119, 40)
        lblMiddleInitial.TabIndex = 49
        lblMiddleInitial.Text = "M.I. :"
        ' 
        ' lblLastName
        ' 
        lblLastName.BackColor = Color.Transparent
        lblLastName.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblLastName.ForeColor = SystemColors.ActiveCaptionText
        lblLastName.Location = New Point(30, 143)
        lblLastName.Margin = New Padding(4, 0, 4, 0)
        lblLastName.Name = "lblLastName"
        lblLastName.Size = New Size(157, 40)
        lblLastName.TabIndex = 48
        lblLastName.Text = "Last Name :"
        ' 
        ' txtLastName
        ' 
        txtLastName.BorderStyle = BorderStyle.FixedSingle
        txtLastName.Font = New Font("Segoe UI", 10F)
        txtLastName.Location = New Point(257, 143)
        txtLastName.Margin = New Padding(4, 5, 4, 5)
        txtLastName.Name = "txtLastName"
        txtLastName.Size = New Size(625, 34)
        txtLastName.TabIndex = 51
        ' 
        ' lblAge
        ' 
        lblAge.BackColor = Color.Transparent
        lblAge.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblAge.ForeColor = SystemColors.ActiveCaptionText
        lblAge.Location = New Point(30, 228)
        lblAge.Margin = New Padding(4, 0, 4, 0)
        lblAge.Name = "lblAge"
        lblAge.Size = New Size(157, 40)
        lblAge.TabIndex = 54
        lblAge.Text = "Age :"
        ' 
        ' lblGender
        ' 
        lblGender.BackColor = Color.Transparent
        lblGender.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        lblGender.ForeColor = SystemColors.ActiveCaptionText
        lblGender.Location = New Point(30, 275)
        lblGender.Margin = New Padding(4, 0, 4, 0)
        lblGender.Name = "lblGender"
        lblGender.Size = New Size(157, 40)
        lblGender.TabIndex = 53
        lblGender.Text = "Gender :"
        ' 
        ' cbGender
        ' 
        cbGender.DropDownStyle = ComboBoxStyle.DropDownList
        cbGender.Font = New Font("Segoe UI", 10F)
        cbGender.FormattingEnabled = True
        cbGender.Items.AddRange(New Object() {"Male", "Female"})
        cbGender.Location = New Point(256, 273)
        cbGender.Margin = New Padding(4, 5, 4, 5)
        cbGender.Name = "cbGender"
        cbGender.Size = New Size(624, 36)
        cbGender.TabIndex = 55
        ' 
        ' UserAddPanel
        ' 
        UserAddPanel.Controls.Add(txtAddress)
        UserAddPanel.Controls.Add(txtEmailAddress)
        UserAddPanel.Controls.Add(btnDelete)
        UserAddPanel.Controls.Add(btnReset)
        UserAddPanel.Controls.Add(btnEdit)
        UserAddPanel.Controls.Add(btnAdd)
        UserAddPanel.Controls.Add(lblRequiredFields)
        UserAddPanel.Controls.Add(Label4)
        UserAddPanel.Controls.Add(cbGender)
        UserAddPanel.Controls.Add(lblGender)
        UserAddPanel.Controls.Add(lblAge)
        UserAddPanel.Controls.Add(txtLastName)
        UserAddPanel.Controls.Add(cbRole)
        UserAddPanel.Controls.Add(lblLastName)
        UserAddPanel.Controls.Add(txtConfirmPassword)
        UserAddPanel.Controls.Add(lblMiddleInitial)
        UserAddPanel.Controls.Add(lblFirstName)
        UserAddPanel.Controls.Add(txtPassword)
        UserAddPanel.Controls.Add(txtMiddleInitial)
        UserAddPanel.Controls.Add(Label5)
        UserAddPanel.Controls.Add(txtFirstName)
        UserAddPanel.Controls.Add(txtAge)
        UserAddPanel.Controls.Add(Label1)
        UserAddPanel.Controls.Add(txtUsername)
        UserAddPanel.Controls.Add(txtPhoneNumber)
        UserAddPanel.Controls.Add(Label2)
        UserAddPanel.Controls.Add(lblAddress)
        UserAddPanel.Controls.Add(lblRole)
        UserAddPanel.Controls.Add(lblEmail)
        UserAddPanel.Controls.Add(Panel2)
        UserAddPanel.Location = New Point(90, 127)
        UserAddPanel.Name = "UserAddPanel"
        UserAddPanel.Size = New Size(913, 677)
        UserAddPanel.TabIndex = 78
        UserAddPanel.Visible = False
        ' 
        ' txtAddress
        ' 
        txtAddress.BorderStyle = BorderStyle.FixedSingle
        txtAddress.Font = New Font("Segoe UI", 10F)
        txtAddress.Location = New Point(256, 482)
        txtAddress.Margin = New Padding(4, 5, 4, 5)
        txtAddress.MaxLength = 3
        txtAddress.Name = "txtAddress"
        txtAddress.Size = New Size(627, 34)
        txtAddress.TabIndex = 87
        ' 
        ' txtEmailAddress
        ' 
        txtEmailAddress.BorderStyle = BorderStyle.FixedSingle
        txtEmailAddress.Font = New Font("Segoe UI", 10F)
        txtEmailAddress.Location = New Point(258, 316)
        txtEmailAddress.Margin = New Padding(4, 5, 4, 5)
        txtEmailAddress.MaxLength = 3
        txtEmailAddress.Name = "txtEmailAddress"
        txtEmailAddress.Size = New Size(625, 34)
        txtEmailAddress.TabIndex = 99
        ' 
        ' btnDelete
        ' 
        btnDelete.BackColor = SystemColors.ActiveCaptionText
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnDelete.ForeColor = SystemColors.ButtonHighlight
        btnDelete.Location = New Point(575, 608)
        btnDelete.Margin = New Padding(5, 6, 5, 6)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(149, 50)
        btnDelete.TabIndex = 97
        btnDelete.Text = "DELETE"
        btnDelete.UseVisualStyleBackColor = False
        ' 
        ' btnReset
        ' 
        btnReset.BackColor = SystemColors.ControlDarkDark
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnReset.ForeColor = SystemColors.ButtonHighlight
        btnReset.Location = New Point(734, 608)
        btnReset.Margin = New Padding(5, 6, 5, 6)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(149, 50)
        btnReset.TabIndex = 96
        btnReset.Text = "RESET"
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' btnEdit
        ' 
        btnEdit.BackColor = SystemColors.ActiveCaptionText
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnEdit.ForeColor = SystemColors.ButtonHighlight
        btnEdit.Location = New Point(416, 608)
        btnEdit.Margin = New Padding(5, 6, 5, 6)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(149, 50)
        btnEdit.TabIndex = 95
        btnEdit.Text = "EDIT"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' btnAdd
        ' 
        btnAdd.BackColor = SystemColors.ActiveCaptionText
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnAdd.ForeColor = SystemColors.ButtonHighlight
        btnAdd.Location = New Point(257, 608)
        btnAdd.Margin = New Padding(5, 6, 5, 6)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(149, 50)
        btnAdd.TabIndex = 94
        btnAdd.Text = "ADD"
        btnAdd.UseVisualStyleBackColor = False
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label4.BackColor = Color.Transparent
        Label4.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label4.ForeColor = SystemColors.ActiveCaptionText
        Label4.Location = New Point(28, 483)
        Label4.Margin = New Padding(4, 0, 4, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(463, 40)
        Label4.TabIndex = 84
        Label4.Text = "Address :"
        ' 
        ' cbRole
        ' 
        cbRole.DropDownStyle = ComboBoxStyle.DropDownList
        cbRole.Font = New Font("Segoe UI", 10F)
        cbRole.FormattingEnabled = True
        cbRole.Items.AddRange(New Object() {"Admin" & vbTab, "Staff", "Cashier"})
        cbRole.Location = New Point(258, 359)
        cbRole.Margin = New Padding(4, 5, 4, 5)
        cbRole.Name = "cbRole"
        cbRole.Size = New Size(624, 36)
        cbRole.TabIndex = 89
        ' 
        ' txtConfirmPassword
        ' 
        txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle
        txtConfirmPassword.Font = New Font("Segoe UI", 10F)
        txtConfirmPassword.Location = New Point(257, 563)
        txtConfirmPassword.Margin = New Padding(4, 5, 4, 5)
        txtConfirmPassword.MaxLength = 3
        txtConfirmPassword.Name = "txtConfirmPassword"
        txtConfirmPassword.Size = New Size(626, 34)
        txtConfirmPassword.TabIndex = 88
        ' 
        ' txtPassword
        ' 
        txtPassword.BorderStyle = BorderStyle.FixedSingle
        txtPassword.Font = New Font("Segoe UI", 10F)
        txtPassword.Location = New Point(256, 523)
        txtPassword.Margin = New Padding(4, 5, 4, 5)
        txtPassword.MaxLength = 3
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(627, 34)
        txtPassword.TabIndex = 86
        ' 
        ' Label5
        ' 
        Label5.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label5.BackColor = Color.Transparent
        Label5.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label5.ForeColor = SystemColors.ActiveCaptionText
        Label5.Location = New Point(30, 564)
        Label5.Margin = New Padding(4, 0, 4, 0)
        Label5.Name = "Label5"
        Label5.Size = New Size(463, 40)
        Label5.TabIndex = 85
        Label5.Text = "Confirm Passowrd :"
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Label1.BackColor = Color.Transparent
        Label1.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        Label1.ForeColor = SystemColors.ActiveCaptionText
        Label1.Location = New Point(28, 524)
        Label1.Margin = New Padding(4, 0, 4, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(463, 40)
        Label1.TabIndex = 83
        Label1.Text = "Password :"
        ' 
        ' txtUsername
        ' 
        txtUsername.BorderStyle = BorderStyle.FixedSingle
        txtUsername.Font = New Font("Segoe UI", 10F)
        txtUsername.Location = New Point(258, 444)
        txtUsername.Margin = New Padding(4, 5, 4, 5)
        txtUsername.MaxLength = 3
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(624, 34)
        txtUsername.TabIndex = 82
        ' 
        ' txtPhoneNumber
        ' 
        txtPhoneNumber.BorderStyle = BorderStyle.FixedSingle
        txtPhoneNumber.Font = New Font("Segoe UI", 10F)
        txtPhoneNumber.Location = New Point(257, 405)
        txtPhoneNumber.Margin = New Padding(4, 5, 4, 5)
        txtPhoneNumber.MaxLength = 3
        txtPhoneNumber.Name = "txtPhoneNumber"
        txtPhoneNumber.Size = New Size(625, 34)
        txtPhoneNumber.TabIndex = 81
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(btnClosePanel)
        Panel2.Location = New Point(0, 3)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(913, 52)
        Panel2.TabIndex = 98
        ' 
        ' btnClosePanel
        ' 
        btnClosePanel.BackColor = Color.Transparent
        btnClosePanel.FlatAppearance.BorderSize = 0
        btnClosePanel.FlatStyle = FlatStyle.Flat
        btnClosePanel.ForeColor = Color.Transparent
        btnClosePanel.Image = CType(resources.GetObject("btnClosePanel.Image"), Image)
        btnClosePanel.Location = New Point(844, 0)
        btnClosePanel.Name = "btnClosePanel"
        btnClosePanel.Size = New Size(66, 50)
        btnClosePanel.TabIndex = 93
        btnClosePanel.UseVisualStyleBackColor = False
        ' 
        ' txtSearch
        ' 
        txtSearch.Font = New Font("Segoe UI", 12F)
        txtSearch.Location = New Point(1324, 37)
        txtSearch.Name = "txtSearch"
        txtSearch.PlaceholderText = " 🔍 Search User"
        txtSearch.Size = New Size(277, 39)
        txtSearch.TabIndex = 91
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(1679, 26)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 95
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' btnAddUser
        ' 
        btnAddUser.BackColor = Color.Transparent
        btnAddUser.FlatAppearance.BorderSize = 0
        btnAddUser.FlatStyle = FlatStyle.Flat
        btnAddUser.ForeColor = Color.Transparent
        btnAddUser.Image = CType(resources.GetObject("btnAddUser.Image"), Image)
        btnAddUser.Location = New Point(1607, 26)
        btnAddUser.Name = "btnAddUser"
        btnAddUser.Size = New Size(66, 50)
        btnAddUser.TabIndex = 96
        btnAddUser.UseVisualStyleBackColor = False
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label3.ForeColor = Color.White
        Label3.Location = New Point(4, 15)
        Label3.Margin = New Padding(4, 0, 4, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(262, 32)
        Label3.TabIndex = 0
        Label3.Text = "USER MANAGEMENT"
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.FromArgb(CByte(51), CByte(153), CByte(255))
        Panel1.Controls.Add(Label3)
        Panel1.Location = New Point(13, 14)
        Panel1.Margin = New Padding(4, 5, 4, 5)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(271, 60)
        Panel1.TabIndex = 37
        ' 
        ' User
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.WhiteSmoke
        ClientSize = New Size(1758, 1018)
        Controls.Add(btnAddUser)
        Controls.Add(btnCLose)
        Controls.Add(txtSearch)
        Controls.Add(UserAddPanel)
        Controls.Add(Panel1)
        Controls.Add(dgvUsers)
        ForeColor = SystemColors.ActiveCaptionText
        FormBorderStyle = FormBorderStyle.None
        Margin = New Padding(4, 5, 4, 5)
        MinimumSize = New Size(1705, 1018)
        Name = "User"
        StartPosition = FormStartPosition.CenterScreen
        Text = "User Management"
        CType(dgvUsers, ComponentModel.ISupportInitialize).EndInit()
        UserAddPanel.ResumeLayout(False)
        UserAddPanel.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    ' Declare Controls
    Friend WithEvents dgvUsers As System.Windows.Forms.DataGridView
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Label2 As Label
    Friend WithEvents lblAddress As Label
    Friend WithEvents lblEmail As Label
    Friend WithEvents lblRole As Label
    Friend WithEvents lblRequiredFields As Label
    Friend WithEvents txtAge As TextBox
    Friend WithEvents txtFirstName As TextBox
    Friend WithEvents txtMiddleInitial As TextBox
    Friend WithEvents lblFirstName As Label
    Friend WithEvents lblMiddleInitial As Label
    Friend WithEvents lblLastName As Label
    Friend WithEvents txtLastName As TextBox
    Friend WithEvents lblAge As Label
    Friend WithEvents lblGender As Label
    Friend WithEvents cbGender As ComboBox
    Friend WithEvents UserAddPanel As Panel
    Friend WithEvents txtPhoneNumber As TextBox
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtAddress As TextBox
    Friend WithEvents txtConfirmPassword As TextBox
    Friend WithEvents cbRole As ComboBox
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnClosePanel As Button
    Friend WithEvents btnCLose As Button
    Friend WithEvents btnAddUser As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnReset As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtEmailAddress As TextBox

End Class