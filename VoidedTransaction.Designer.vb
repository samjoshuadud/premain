<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class VoidedTransaction
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VoidedTransaction))
        dgvVoidedTrasaction = New DataGridView()
        PanelCategory = New Panel()
        Label2 = New Label()
        btnClosePanel = New Button()
        CType(dgvVoidedTrasaction, ComponentModel.ISupportInitialize).BeginInit()
        PanelCategory.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvVoidedTrasaction
        ' 
        dgvVoidedTrasaction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvVoidedTrasaction.Location = New Point(12, 91)
        dgvVoidedTrasaction.Name = "dgvVoidedTrasaction"
        dgvVoidedTrasaction.RowHeadersWidth = 62
        dgvVoidedTrasaction.Size = New Size(1734, 859)
        dgvVoidedTrasaction.TabIndex = 0
        ' 
        ' PanelCategory
        ' 
        PanelCategory.Controls.Add(Label2)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(348, 75)
        PanelCategory.TabIndex = 36
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(22, 22)
        Label2.Name = "Label2"
        Label2.Size = New Size(288, 32)
        Label2.TabIndex = 0
        Label2.Text = "VOIDED TRANSACTION"
        ' 
        ' btnClosePanel
        ' 
        btnClosePanel.BackColor = Color.Transparent
        btnClosePanel.FlatAppearance.BorderSize = 0
        btnClosePanel.FlatStyle = FlatStyle.Flat
        btnClosePanel.ForeColor = Color.Transparent
        btnClosePanel.Image = CType(resources.GetObject("btnClosePanel.Image"), Image)
        btnClosePanel.Location = New Point(1680, 12)
        btnClosePanel.Name = "btnClosePanel"
        btnClosePanel.Size = New Size(66, 50)
        btnClosePanel.TabIndex = 94
        btnClosePanel.UseVisualStyleBackColor = False
        ' 
        ' VoidedTransaction
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1758, 962)
        Controls.Add(btnClosePanel)
        Controls.Add(PanelCategory)
        Controls.Add(dgvVoidedTrasaction)
        Name = "VoidedTransaction"
        Text = "VoidedTransaction"
        CType(dgvVoidedTrasaction, ComponentModel.ISupportInitialize).EndInit()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvVoidedTrasaction As DataGridView
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents btnClosePanel As Button
End Class
