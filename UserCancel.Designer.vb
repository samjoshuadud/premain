<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserCancel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserCancel))
        txtUsername = New TextBox()
        txtPassword = New TextBox()
        btnVoid = New Button()
        Panel1 = New Panel()
        btnCLose = New Button()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtUsername
        ' 
        txtUsername.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        txtUsername.Location = New Point(152, 97)
        txtUsername.Name = "txtUsername"
        txtUsername.PlaceholderText = "Enter Username"
        txtUsername.Size = New Size(280, 39)
        txtUsername.TabIndex = 0
        ' 
        ' txtPassword
        ' 
        txtPassword.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold)
        txtPassword.Location = New Point(152, 148)
        txtPassword.Name = "txtPassword"
        txtPassword.PlaceholderText = "Enter Password"
        txtPassword.Size = New Size(280, 39)
        txtPassword.TabIndex = 1
        ' 
        ' btnVoid
        ' 
        btnVoid.BackColor = SystemColors.ActiveCaptionText
        btnVoid.FlatStyle = FlatStyle.Flat
        btnVoid.ForeColor = SystemColors.ButtonHighlight
        btnVoid.Location = New Point(152, 193)
        btnVoid.Name = "btnVoid"
        btnVoid.Size = New Size(280, 52)
        btnVoid.TabIndex = 2
        btnVoid.Text = "VOID"
        btnVoid.UseVisualStyleBackColor = False
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnCLose)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(595, 60)
        Panel1.TabIndex = 3
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(529, 3)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 69
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' UserCancel
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ButtonHighlight
        ClientSize = New Size(595, 302)
        Controls.Add(Panel1)
        Controls.Add(btnVoid)
        Controls.Add(txtPassword)
        Controls.Add(txtUsername)
        FormBorderStyle = FormBorderStyle.None
        Name = "UserCancel"
        StartPosition = FormStartPosition.CenterScreen
        Text = "UserCancel"
        Panel1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtUsername As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents btnVoid As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnCLose As Button
End Class
