<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DeliveryList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DeliveryList))
        dgvDeliveryList = New DataGridView()
        Panel1 = New Panel()
        Label2 = New Label()
        btnClosePanel = New Button()
        CType(dgvDeliveryList, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvDeliveryList
        ' 
        dgvDeliveryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDeliveryList.Location = New Point(12, 79)
        dgvDeliveryList.Name = "dgvDeliveryList"
        dgvDeliveryList.RowHeadersWidth = 62
        dgvDeliveryList.Size = New Size(1603, 620)
        dgvDeliveryList.TabIndex = 0
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = SystemColors.MenuHighlight
        Panel1.Controls.Add(Label2)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(220, 61)
        Panel1.TabIndex = 37
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold)
        Label2.ForeColor = SystemColors.ButtonHighlight
        Label2.Location = New Point(13, 12)
        Label2.Name = "Label2"
        Label2.Size = New Size(185, 32)
        Label2.TabIndex = 0
        Label2.Text = "DELIVERY LIST"
        ' 
        ' btnClosePanel
        ' 
        btnClosePanel.BackColor = Color.Transparent
        btnClosePanel.FlatAppearance.BorderSize = 0
        btnClosePanel.FlatStyle = FlatStyle.Flat
        btnClosePanel.ForeColor = Color.Transparent
        btnClosePanel.Image = CType(resources.GetObject("btnClosePanel.Image"), Image)
        btnClosePanel.Location = New Point(1549, 6)
        btnClosePanel.Name = "btnClosePanel"
        btnClosePanel.Size = New Size(66, 50)
        btnClosePanel.TabIndex = 94
        btnClosePanel.UseVisualStyleBackColor = False
        ' 
        ' DeliveryList
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.ControlLight
        ClientSize = New Size(1627, 720)
        Controls.Add(btnClosePanel)
        Controls.Add(Panel1)
        Controls.Add(dgvDeliveryList)
        FormBorderStyle = FormBorderStyle.None
        Name = "DeliveryList"
        StartPosition = FormStartPosition.CenterScreen
        Text = "DeliveryList"
        CType(dgvDeliveryList, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvDeliveryList As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents btnClosePanel As Button
End Class
