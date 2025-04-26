<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AuditTrail
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
        dgvAuditTrail = New DataGridView()
        Panel1 = New Panel()
        Label2 = New Label()
        CType(dgvAuditTrail, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvAuditTrail
        ' 
        dgvAuditTrail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAuditTrail.Location = New Point(12, 78)
        dgvAuditTrail.Name = "dgvAuditTrail"
        dgvAuditTrail.RowHeadersWidth = 62
        dgvAuditTrail.Size = New Size(1738, 872)
        dgvAuditTrail.TabIndex = 1
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label2)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(201, 60)
        Panel1.TabIndex = 38
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label2.ForeColor = SystemColors.ButtonHighlight
        Label2.Location = New Point(13, 14)
        Label2.Name = "Label2"
        Label2.Size = New Size(168, 32)
        Label2.TabIndex = 0
        Label2.Text = "AUDIT TRAIL"
        ' 
        ' AuditTrail
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1778, 962)
        Controls.Add(Panel1)
        Controls.Add(dgvAuditTrail)
        Name = "AuditTrail"
        Text = "AuditTrail"
        CType(dgvAuditTrail, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvAuditTrail As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
End Class
