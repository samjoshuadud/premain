<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectProduct
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
        dgvProducts = New DataGridView()
        CType(dgvProducts, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvProducts
        ' 
        dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvProducts.Location = New Point(12, 108)
        dgvProducts.Name = "dgvProducts"
        dgvProducts.RowHeadersWidth = 62
        dgvProducts.Size = New Size(1325, 447)
        dgvProducts.TabIndex = 0
        ' 
        ' SelectProduct
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1349, 567)
        Controls.Add(dgvProducts)
        Name = "SelectProduct"
        Text = "SelectProduct"
        CType(dgvProducts, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvProducts As DataGridView
End Class
