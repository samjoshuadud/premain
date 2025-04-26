<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DeliverySaveProduct
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
        dgvItems = New DataGridView()
        btnVoid = New Button()
        CType(dgvItems, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvItems
        ' 
        dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvItems.Location = New Point(12, 12)
        dgvItems.Name = "dgvItems"
        dgvItems.RowHeadersWidth = 62
        dgvItems.Size = New Size(776, 225)
        dgvItems.TabIndex = 0
        ' 
        ' btnVoid
        ' 
        btnVoid.Location = New Point(12, 243)
        btnVoid.Name = "btnVoid"
        btnVoid.Size = New Size(112, 34)
        btnVoid.TabIndex = 1
        btnVoid.Text = "Button1"
        btnVoid.UseVisualStyleBackColor = True
        ' 
        ' DeliverySaveProduct
        ' 
        AutoScaleDimensions = New SizeF(10.0F, 25.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(btnVoid)
        Controls.Add(dgvItems)
        Name = "DeliverySaveProduct"
        StartPosition = FormStartPosition.CenterScreen
        Text = "DeliverySaveProduct"
        CType(dgvItems, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvItems As DataGridView
    Friend WithEvents btnVoid As Button
End Class
