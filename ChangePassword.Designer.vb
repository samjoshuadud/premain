<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ChangePassword
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChangePassword))
        txtUsername = New TextBox()
        txtOldPassword = New TextBox()
        txtNewPassword = New TextBox()
        btnChangePassword = New Button()
        PanelCategory = New Panel()
        Label2 = New Label()
        btnClosePanel = New Button()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtUsername
        ' 
        txtUsername.Font = New Font("Segoe UI", 12F)
        txtUsername.Location = New Point(158, 119)
        txtUsername.Name = "txtUsername"
        txtUsername.PlaceholderText = "Enter Username"
        txtUsername.Size = New Size(249, 39)
        txtUsername.TabIndex = 0
        ' 
        ' txtOldPassword
        ' 
        txtOldPassword.Font = New Font("Segoe UI", 12F)
        txtOldPassword.Location = New Point(158, 169)
        txtOldPassword.Name = "txtOldPassword"
        txtOldPassword.PlaceholderText = "Enter Old Password"
        txtOldPassword.Size = New Size(249, 39)
        txtOldPassword.TabIndex = 1
        txtOldPassword.UseSystemPasswordChar = True
        ' 
        ' txtNewPassword
        ' 
        txtNewPassword.Font = New Font("Segoe UI", 12F)
        txtNewPassword.Location = New Point(158, 219)
        txtNewPassword.Name = "txtNewPassword"
        txtNewPassword.PlaceholderText = "Enter New Password"
        txtNewPassword.Size = New Size(249, 39)
        txtNewPassword.TabIndex = 2
        txtNewPassword.UseSystemPasswordChar = True
        ' 
        ' btnChangePassword
        ' 
        btnChangePassword.BackColor = SystemColors.ActiveCaptionText
        btnChangePassword.FlatStyle = FlatStyle.Flat
        btnChangePassword.ForeColor = SystemColors.ButtonHighlight
        btnChangePassword.Location = New Point(158, 269)
        btnChangePassword.Name = "btnChangePassword"
        btnChangePassword.Size = New Size(249, 40)
        btnChangePassword.TabIndex = 3
        btnChangePassword.Text = "Change Password"
        btnChangePassword.UseVisualStyleBackColor = False
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label2)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(293, 68)
        PanelCategory.TabIndex = 38
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label2.ForeColor = SystemColors.ButtonHighlight
        Label2.Location = New Point(18, 17)
        Label2.Name = "Label2"
        Label2.Size = New Size(261, 32)
        Label2.TabIndex = 0
        Label2.Text = "CHANGE PASSWORD"
        ' 
        ' btnClosePanel
        ' 
        btnClosePanel.BackColor = Color.Transparent
        btnClosePanel.FlatAppearance.BorderSize = 0
        btnClosePanel.FlatStyle = FlatStyle.Flat
        btnClosePanel.ForeColor = Color.Transparent
        btnClosePanel.Image = CType(resources.GetObject("btnClosePanel.Image"), Image)
        btnClosePanel.Location = New Point(498, 2)
        btnClosePanel.Name = "btnClosePanel"
        btnClosePanel.Size = New Size(66, 50)
        btnClosePanel.TabIndex = 94
        btnClosePanel.UseVisualStyleBackColor = False
        ' 
        ' ChangePassword
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(565, 346)
        Controls.Add(btnClosePanel)
        Controls.Add(PanelCategory)
        Controls.Add(btnChangePassword)
        Controls.Add(txtNewPassword)
        Controls.Add(txtOldPassword)
        Controls.Add(txtUsername)
        FormBorderStyle = FormBorderStyle.None
        Name = "ChangePassword"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Change Password"
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Private WithEvents txtUsername As System.Windows.Forms.TextBox
    Private WithEvents txtOldPassword As System.Windows.Forms.TextBox
    Private WithEvents txtNewPassword As System.Windows.Forms.TextBox
    Private WithEvents btnChangePassword As System.Windows.Forms.Button
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents btnClosePanel As Button
End Class
