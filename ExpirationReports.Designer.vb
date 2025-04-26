<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExpirationReports
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExpirationReports))
        dgvExpirationReport = New DataGridView()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        PanelCategory = New Panel()
        Label7 = New Label()
        btnClosePanel = New Button()
        Panel1 = New Panel()
        Label8 = New Label()
        CType(dgvExpirationReport, ComponentModel.ISupportInitialize).BeginInit()
        PanelCategory.SuspendLayout()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' dgvExpirationReport
        ' 
        dgvExpirationReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvExpirationReport.Location = New Point(5, 197)
        dgvExpirationReport.Name = "dgvExpirationReport"
        dgvExpirationReport.RowHeadersWidth = 62
        dgvExpirationReport.Size = New Size(1752, 753)
        dgvExpirationReport.TabIndex = 0
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.Yellow
        Label1.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label1.ForeColor = Color.Yellow
        Label1.Location = New Point(15, 91)
        Label1.Name = "Label1"
        Label1.Size = New Size(82, 21)
        Label1.TabIndex = 1
        Label1.Text = "00000000"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Red
        Label2.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label2.ForeColor = Color.Red
        Label2.Location = New Point(15, 125)
        Label2.Name = "Label2"
        Label2.Size = New Size(82, 21)
        Label2.TabIndex = 2
        Label2.Text = "00000000"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label3.Location = New Point(113, 125)
        Label3.Name = "Label3"
        Label3.Size = New Size(68, 21)
        Label3.TabIndex = 3
        Label3.Text = "Expired"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label4.Location = New Point(113, 91)
        Label4.Name = "Label4"
        Label4.Size = New Size(111, 21)
        Label4.TabIndex = 4
        Label4.Text = "Expired Soon"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.BackColor = Color.Green
        Label5.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label5.ForeColor = Color.Green
        Label5.Location = New Point(15, 53)
        Label5.Name = "Label5"
        Label5.Size = New Size(82, 21)
        Label5.TabIndex = 5
        Label5.Text = "00000000"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label6.Location = New Point(113, 53)
        Label6.Name = "Label6"
        Label6.Size = New Size(51, 21)
        Label6.TabIndex = 6
        Label6.Text = "Good"
        ' 
        ' PanelCategory
        ' 
        PanelCategory.BackColor = SystemColors.MenuHighlight
        PanelCategory.Controls.Add(Label7)
        PanelCategory.Location = New Point(12, 12)
        PanelCategory.Name = "PanelCategory"
        PanelCategory.Size = New Size(235, 53)
        PanelCategory.TabIndex = 37
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.BackColor = Color.Transparent
        Label7.Font = New Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label7.ForeColor = SystemColors.ButtonHighlight
        Label7.Location = New Point(3, 12)
        Label7.Name = "Label7"
        Label7.Size = New Size(228, 28)
        Label7.TabIndex = 0
        Label7.Text = "EXPIRATION REPORTS "
        ' 
        ' btnClosePanel
        ' 
        btnClosePanel.BackColor = Color.Transparent
        btnClosePanel.FlatAppearance.BorderSize = 0
        btnClosePanel.FlatStyle = FlatStyle.Flat
        btnClosePanel.ForeColor = Color.Transparent
        btnClosePanel.Image = CType(resources.GetObject("btnClosePanel.Image"), Image)
        btnClosePanel.Location = New Point(1691, 12)
        btnClosePanel.Name = "btnClosePanel"
        btnClosePanel.Size = New Size(66, 50)
        btnClosePanel.TabIndex = 94
        btnClosePanel.UseVisualStyleBackColor = False
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label8)
        Panel1.Controls.Add(Label5)
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(Label2)
        Panel1.Controls.Add(Label6)
        Panel1.Controls.Add(Label3)
        Panel1.Controls.Add(Label4)
        Panel1.Location = New Point(1321, 785)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(436, 165)
        Panel1.TabIndex = 95
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        Label8.Location = New Point(15, 16)
        Label8.Name = "Label8"
        Label8.Size = New Size(75, 21)
        Label8.TabIndex = 7
        Label8.Text = "STATUS :"
        ' 
        ' ExpirationReports
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1769, 962)
        Controls.Add(Panel1)
        Controls.Add(btnClosePanel)
        Controls.Add(PanelCategory)
        Controls.Add(dgvExpirationReport)
        FormBorderStyle = FormBorderStyle.None
        Name = "ExpirationReports"
        StartPosition = FormStartPosition.CenterScreen
        Text = "ExpirationReports"
        CType(dgvExpirationReport, ComponentModel.ISupportInitialize).EndInit()
        PanelCategory.ResumeLayout(False)
        PanelCategory.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvExpirationReport As DataGridView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents PanelCategory As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents btnClosePanel As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label8 As Label
End Class
