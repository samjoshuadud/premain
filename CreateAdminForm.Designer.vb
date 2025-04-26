<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CreateAdminForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        lblTitle = New Label()
        lblUsername = New Label()
        txtUsername = New TextBox()
        lblPassword = New Label()
        txtPassword = New TextBox()
        lblConfirmPassword = New Label()
        txtConfirmPassword = New TextBox()
        lblFullName = New Label()
        txtFullName = New TextBox()
        lblEmail = New Label()
        txtEmail = New TextBox()
        btnSave = New Button()
        btnCancel = New Button()
        lblInstructions = New Label()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        lblTitle.Location = New Point(42, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(369, 32)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Create Administrator Account"
        ' 
        ' lblInstructions
        ' 
        lblInstructions.Location = New Point(42, 51)
        lblInstructions.Name = "lblInstructions"
        lblInstructions.Size = New Size(369, 50)
        lblInstructions.TabIndex = 1
        lblInstructions.Text = "No administrator account was found. Please create one to continue using the system."
        ' 
        ' lblUsername
        ' 
        lblUsername.AutoSize = True
        lblUsername.Location = New Point(42, 111)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New Size(75, 20)
        lblUsername.TabIndex = 2
        lblUsername.Text = "Username:"
        ' 
        ' txtUsername
        ' 
        txtUsername.Location = New Point(180, 108)
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(231, 27)
        txtUsername.TabIndex = 3
        ' 
        ' lblPassword
        ' 
        lblPassword.AutoSize = True
        lblPassword.Location = New Point(42, 151)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(73, 20)
        lblPassword.TabIndex = 4
        lblPassword.Text = "Password:"
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(180, 148)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordChar = "*"c
        txtPassword.Size = New Size(231, 27)
        txtPassword.TabIndex = 5
        ' 
        ' lblConfirmPassword
        ' 
        lblConfirmPassword.AutoSize = True
        lblConfirmPassword.Location = New Point(42, 191)
        lblConfirmPassword.Name = "lblConfirmPassword"
        lblConfirmPassword.Size = New Size(132, 20)
        lblConfirmPassword.TabIndex = 6
        lblConfirmPassword.Text = "Confirm Password:"
        ' 
        ' txtConfirmPassword
        ' 
        txtConfirmPassword.Location = New Point(180, 188)
        txtConfirmPassword.Name = "txtConfirmPassword"
        txtConfirmPassword.PasswordChar = "*"c
        txtConfirmPassword.Size = New Size(231, 27)
        txtConfirmPassword.TabIndex = 7
        ' 
        ' lblFullName
        ' 
        lblFullName.AutoSize = True
        lblFullName.Location = New Point(42, 231)
        lblFullName.Name = "lblFullName"
        lblFullName.Size = New Size(79, 20)
        lblFullName.TabIndex = 8
        lblFullName.Text = "Full Name:"
        ' 
        ' txtFullName
        ' 
        txtFullName.Location = New Point(180, 228)
        txtFullName.Name = "txtFullName"
        txtFullName.Size = New Size(231, 27)
        txtFullName.TabIndex = 9
        ' 
        ' lblEmail
        ' 
        lblEmail.AutoSize = True
        lblEmail.Location = New Point(42, 271)
        lblEmail.Name = "lblEmail"
        lblEmail.Size = New Size(49, 20)
        lblEmail.TabIndex = 10
        lblEmail.Text = "Email:"
        ' 
        ' txtEmail
        ' 
        txtEmail.Location = New Point(180, 268)
        txtEmail.Name = "txtEmail"
        txtEmail.Size = New Size(231, 27)
        txtEmail.TabIndex = 11
        ' 
        ' btnSave
        ' 
        btnSave.BackColor = Color.FromArgb(CByte(0), CByte(64), CByte(0))
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.ForeColor = Color.White
        btnSave.Location = New Point(180, 320)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(109, 36)
        btnSave.TabIndex = 12
        btnSave.Text = "Save"
        btnSave.UseVisualStyleBackColor = False
        ' 
        ' btnCancel
        ' 
        btnCancel.BackColor = Color.Maroon
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.ForeColor = Color.White
        btnCancel.Location = New Point(302, 320)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(109, 36)
        btnCancel.TabIndex = 13
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' CreateAdminForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(454, 380)
        Controls.Add(btnCancel)
        Controls.Add(btnSave)
        Controls.Add(txtEmail)
        Controls.Add(lblEmail)
        Controls.Add(txtFullName)
        Controls.Add(lblFullName)
        Controls.Add(txtConfirmPassword)
        Controls.Add(lblConfirmPassword)
        Controls.Add(txtPassword)
        Controls.Add(lblPassword)
        Controls.Add(txtUsername)
        Controls.Add(lblUsername)
        Controls.Add(lblInstructions)
        Controls.Add(lblTitle)
        Name = "CreateAdminForm"
        Text = "Create Administrator Account"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblConfirmPassword As Label
    Friend WithEvents txtConfirmPassword As TextBox
    Friend WithEvents lblFullName As Label
    Friend WithEvents txtFullName As TextBox
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblInstructions As Label
End Class 