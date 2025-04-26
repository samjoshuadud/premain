<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DatabaseLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DatabaseLogin))
        txtip = New TextBox()
        txtuser = New TextBox()
        txtpass = New TextBox()
        Button1 = New Button()
        btnCLose = New Button()
        SuspendLayout()
        ' 
        ' txtip
        ' 
        txtip.Font = New Font("Segoe UI", 12F)
        txtip.Location = New Point(90, 81)
        txtip.Name = "txtip"
        txtip.Size = New Size(303, 39)
        txtip.TabIndex = 0
        ' 
        ' txtuser
        ' 
        txtuser.Font = New Font("Segoe UI", 12F)
        txtuser.Location = New Point(90, 126)
        txtuser.Name = "txtuser"
        txtuser.Size = New Size(303, 39)
        txtuser.TabIndex = 1
        ' 
        ' txtpass
        ' 
        txtpass.Font = New Font("Segoe UI", 12F)
        txtpass.Location = New Point(90, 171)
        txtpass.Name = "txtpass"
        txtpass.Size = New Size(303, 39)
        txtpass.TabIndex = 2
        ' 
        ' Button1
        ' 
        Button1.FlatStyle = FlatStyle.Flat
        Button1.Location = New Point(90, 229)
        Button1.Name = "Button1"
        Button1.Size = New Size(303, 58)
        Button1.TabIndex = 3
        Button1.Text = "CONNECT"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' btnCLose
        ' 
        btnCLose.BackColor = Color.Transparent
        btnCLose.FlatAppearance.BorderSize = 0
        btnCLose.FlatStyle = FlatStyle.Flat
        btnCLose.ForeColor = Color.Transparent
        btnCLose.Image = CType(resources.GetObject("btnCLose.Image"), Image)
        btnCLose.Location = New Point(421, 2)
        btnCLose.Name = "btnCLose"
        btnCLose.Size = New Size(66, 50)
        btnCLose.TabIndex = 96
        btnCLose.UseVisualStyleBackColor = False
        ' 
        ' DatabaseLogin
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(490, 386)
        Controls.Add(btnCLose)
        Controls.Add(Button1)
        Controls.Add(txtpass)
        Controls.Add(txtuser)
        Controls.Add(txtip)
        FormBorderStyle = FormBorderStyle.None
        Name = "DatabaseLogin"
        StartPosition = FormStartPosition.CenterScreen
        Text = "DatabaseLogin"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtip As TextBox
    Friend WithEvents txtuser As TextBox
    Friend WithEvents txtpass As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents btnCLose As Button
End Class
