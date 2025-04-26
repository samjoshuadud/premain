<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogHistory
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
        dgvLogHistory = New DataGridView()
        PanelCategory = New Panel()
        Label2 = New Label()
        CType(dgvLogHistory, ComponentModel.ISupportInitialize).BeginInit()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvLogHistory
        ' 
        dgvLogHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvLogHistory.Location = New Point(12, 74)
        dgvLogHistory.Name = "dgvLogHistory"
        dgvLogHistory.RowHeadersWidth = 62
        dgvLogHistory.Size = New Size(1754, 876)
        dgvLogHistory.TabIndex = 0
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label2)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(179, 56)
        PanelCategory.TabIndex = 39
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI Black", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(17, 12)
        Label2.Name = "Label2"
        Label2.Size = New Size(148, 28)
        Label2.TabIndex = 0
        Label2.Text = "LOG HISTORY"
        ' 
        ' LogHistory
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1778, 962)
        Controls.Add(PanelCategory)
        Controls.Add(dgvLogHistory)
        Name = "LogHistory"
        Text = "LogHistory"
        CType(dgvLogHistory, ComponentModel.ISupportInitialize).EndInit()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvLogHistory As DataGridView
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label2 As Label
End Class
