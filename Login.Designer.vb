<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login))
        PictureBox1 = New PictureBox()
        Label4 = New Label()
        Panel2 = New Panel()
        Label3 = New Button()
        Label2 = New Label()
        Label1 = New Label()
        txtPassword = New TextBox()
        btnTogglePassword = New CheckBox()
        txtUsername = New TextBox()
        btnLogin = New Button()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        Panel2.SuspendLayout()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(34, 21)
        PictureBox1.Margin = New Padding(2)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(246, 195)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox1.TabIndex = 3
        PictureBox1.TabStop = False
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.BackColor = Color.Transparent
        Label4.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(55, 21)
        Label4.Margin = New Padding(2, 0, 2, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(198, 15)
        Label4.TabIndex = 41
        Label4.Text = "SHIENNA'S MINI GROCERY STORE"
        ' 
        ' Panel2
        ' 
        Panel2.Controls.Add(Label3)
        Panel2.Controls.Add(Label2)
        Panel2.Controls.Add(Label1)
        Panel2.Controls.Add(txtPassword)
        Panel2.Controls.Add(btnTogglePassword)
        Panel2.Controls.Add(txtUsername)
        Panel2.Controls.Add(btnLogin)
        Panel2.Location = New Point(340, -1)
        Panel2.Margin = New Padding(2)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(407, 251)
        Panel2.TabIndex = 42
        ' 
        ' Label3
        ' 
        Label3.BackColor = SystemColors.ActiveCaptionText
        Label3.FlatAppearance.BorderSize = 0
        Label3.FlatStyle = FlatStyle.Flat
        Label3.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.ForeColor = SystemColors.ButtonHighlight
        Label3.Location = New Point(274, 5)
        Label3.Margin = New Padding(2)
        Label3.Name = "Label3"
        Label3.Size = New Size(124, 32)
        Label3.TabIndex = 51
        Label3.Text = "CONNECT"
        Label3.UseVisualStyleBackColor = False
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.ForeColor = SystemColors.ControlLight
        Label2.Location = New Point(132, 97)
        Label2.Margin = New Padding(2, 0, 2, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(73, 19)
        Label2.TabIndex = 49
        Label2.Text = "Password"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.Transparent
        Label1.Font = New Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = SystemColors.ControlLight
        Label1.Location = New Point(132, 45)
        Label1.Margin = New Padding(2, 0, 2, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(80, 19)
        Label1.TabIndex = 48
        Label1.Text = "Username "
        ' 
        ' txtPassword
        ' 
        txtPassword.Font = New Font("Segoe UI", 12F)
        txtPassword.Location = New Point(132, 118)
        txtPassword.Margin = New Padding(2)
        txtPassword.Name = "txtPassword"
        txtPassword.PlaceholderText = "Enter Password"
        txtPassword.Size = New Size(208, 29)
        txtPassword.TabIndex = 45
        ' 
        ' btnTogglePassword
        ' 
        btnTogglePassword.AutoSize = True
        btnTogglePassword.BackColor = Color.Transparent
        btnTogglePassword.ForeColor = SystemColors.ButtonFace
        btnTogglePassword.Location = New Point(132, 151)
        btnTogglePassword.Margin = New Padding(2)
        btnTogglePassword.Name = "btnTogglePassword"
        btnTogglePassword.Size = New Size(108, 19)
        btnTogglePassword.TabIndex = 47
        btnTogglePassword.Text = "Show Password"
        btnTogglePassword.UseVisualStyleBackColor = False
        ' 
        ' txtUsername
        ' 
        txtUsername.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtUsername.Location = New Point(132, 66)
        txtUsername.Margin = New Padding(2)
        txtUsername.Name = "txtUsername"
        txtUsername.PlaceholderText = "Enter Username"
        txtUsername.Size = New Size(208, 29)
        txtUsername.TabIndex = 44
        ' 
        ' btnLogin
        ' 
        btnLogin.BackColor = SystemColors.ActiveCaptionText
        btnLogin.FlatAppearance.BorderSize = 0
        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnLogin.ForeColor = SystemColors.ButtonHighlight
        btnLogin.Location = New Point(133, 174)
        btnLogin.Margin = New Padding(2)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(207, 32)
        btnLogin.TabIndex = 46
        btnLogin.Text = "LOG IN "
        btnLogin.UseVisualStyleBackColor = False
        ' 
        ' Login
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(746, 247)
        Controls.Add(Label4)
        Controls.Add(PictureBox1)
        Controls.Add(Panel2)
        FormBorderStyle = FormBorderStyle.None
        Margin = New Padding(2)
        Name = "Login"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents btnTogglePassword As CheckBox
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents btnLogin As Button
    Friend WithEvents Label3 As Button
End Class
