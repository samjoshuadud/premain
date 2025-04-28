Imports Microsoft.Data.SqlClient


Public Class Main
    ' Declare a variable to hold the Full Name
    Private userFullName As String
    Private connectionString As String = AppConfig.ConnectionString
    ' Constructor to accept the Full Name passed from Login form
    Public Sub New(fullName As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Set the Full Name
        userFullName = fullName

        ' Add the event handler for btnMaintenance click event
        AddHandler btnMaintenance.Click, AddressOf btnMaintenance_Click
    End Sub

    ' Initialize ToolTip control
    Dim toolTip As New ToolTip()

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Ipakita ang kasalukuyang oras at petsa sa label
        lblDateTime.Text = Date.Now.ToString("MMMM dd, yyyy hh:mm:ss tt")

    End Sub

    ' Main form load event to initialize UI and start automatic refresh
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize the UI components and load data
        Me.FormBorderStyle = FormBorderStyle.None

        ' Set the window size and position
        SetFormSizeAndPosition()

        ' Set the main panel to be properly anchored and sized
        'MainPanel.Dock = DockStyle.Fill
        'MainPanel.Visible = False

        SetupEventHandlers()
        LoadUI()

        ' Start the timer to automatically refresh the data every 30 seconds (30000 milliseconds)
        Timer2.Interval = 30000 ' Set the interval to 30 seconds
        Timer2.Start()

        ' Check database connection and create admin if needed
        ' ... existing code ...
    End Sub

    ' Method to set the form size and position based on screen resolution
    Private Sub SetFormSizeAndPosition()
        ' Get the primary screen working area (excludes taskbar)
        Dim screen As System.Windows.Forms.Screen = System.Windows.Forms.Screen.PrimaryScreen
        Dim workingArea As Rectangle = screen.WorkingArea

        ' Set the form size to 90% of the screen
        Me.Width = CInt(workingArea.Width * 0.9)
        Me.Height = CInt(workingArea.Height * 0.9)

        ' Center the form on the screen
        Me.Location = New Point(
            CInt((workingArea.Width - Me.Width) / 2),
            CInt((workingArea.Height - Me.Height) / 2))

        ' Ensure form doesn't exceed screen bounds
        If Me.Width > workingArea.Width Then Me.Width = workingArea.Width
        If Me.Height > workingArea.Height Then Me.Height = workingArea.Height
    End Sub

    ' Handle form resize event to maintain proper layout
    Private Sub Main_Resize(sender As Object, e As EventArgs)
        ' Ensure the ButtonPanel fills the form
        ' Removed redundant resizing logic as ButtonPanel is already anchored to all sides
        ButtonPanel.PerformLayout()

        ' If MainPanel is visible, ensure proper layout of its contents
        If MainPanel.Visible AndAlso MainPanel.Controls.Count > 0 Then
            Dim childForm As Form = TryCast(MainPanel.Controls(0), Form)
            If childForm IsNot Nothing Then
                ' Force the child form to redraw
                childForm.PerformLayout()
            End If
        End If
    End Sub

    ' Add the event handler during form initialization
    Private Sub SetupEventHandlers()
        AddHandler Me.Resize, AddressOf Main_Resize
    End Sub

    ' Method to load the UI components and data initially
    Private Sub LoadUI()
        ' Display the Full Name in the label
        lblFullname.Text = "Welcome, " & userFullName

        ' Start the timer for the clock or any other ongoing task
        Timer1.Start()

        ' Load total products count from database
        LoadTotalProducts()

        ' Load total sales for the day
        LoadQuantityInStock()
        LoadCriticalItems()

        ' Set background color
        Me.BackColor = ColorTranslator.FromHtml("#251F1F")

        ' Apply button styles
        ButtonStyler.ApplyStyle(btnDashboard)
        ButtonStyler.ApplyStyle(btnMaintenance)
        ButtonStyler.ApplyStyle(btnReports)
        ButtonStyler.ApplyStyle(btnInventory)
        ButtonStyler.ApplyStyle(btnPOS)
        ButtonStyler.ApplyStyle(btnSettings)
        ButtonStyler.ApplyStyle(btnLogOut)
        ButtonStyler.ApplyStyle(btnChangePassword)

        ' Set tooltips for buttons
        toolTip.SetToolTip(btnDashboard, "Dashboard - Click To View The Dashboard.")
        toolTip.SetToolTip(btnPOS, "Point of Sale (POS) - Click To Open POS.")
        toolTip.SetToolTip(btnInventory, "Inventory - Click To Open Inventory Management.")
        toolTip.SetToolTip(btnSettings, "Settings - Click To Open Settings Options.")
        toolTip.SetToolTip(btnMaintenance, "Maintenance - Click To View Maintenance Options.")
        toolTip.SetToolTip(btnReports, "Reports - Click To View Available Reports.")
        toolTip.SetToolTip(btnLogOut, "Log Out - Click To Log Out From The System.")

        ' Ensure Panel5 is visible initially and MainPanel is hidden
        ' Removed redundant visibility settings as they are already defined in the designer
    End Sub

    ' Method to reload all UI components and data
    Private Sub ReloadUI()
        ' Reload data every time this method is called
        LoadTotalProducts()
        LoadTotalSales()
        LoadQuantityInStock()

        ' Optionally, reset the background color or other UI elements if needed
        Me.BackColor = ColorTranslator.FromHtml("#251F1F") ' Or set it to your desired color again

        ' Reapply button styles if needed
        ButtonStyler.ApplyStyle(btnDashboard)
        ButtonStyler.ApplyStyle(btnMaintenance)
        ButtonStyler.ApplyStyle(btnReports)
        ButtonStyler.ApplyStyle(btnInventory)
        ButtonStyler.ApplyStyle(btnPOS)
        ButtonStyler.ApplyStyle(btnSettings)
        ButtonStyler.ApplyStyle(btnLogOut)
        ButtonStyler.ApplyStyle(btnChangePassword)
    End Sub

    ' Timer Tick event to refresh data automatically every set interval (30 seconds)
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        ' Call ReloadUI to refresh data and UI every time the timer ticks
        ReloadUI()
    End Sub



    ' Load total number of products from the database
    Private Sub LoadTotalProducts()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM Products"
                Using cmd As New SqlCommand(query, conn)
                    Dim totalProducts As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    lblTotalProduct.Text = totalProducts.ToString()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading total products: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Load total sales for today
    Private Sub LoadTotalSales()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' SQL query to sum total sales for today
                Dim query As String = "SELECT SUM(si.TotalPrice) " &
                                  "FROM SaleItems si " &
                                  "JOIN Sales s ON si.SaleID = s.SaleID " &
                                  "WHERE CAST(s.SaleDate AS DATE) = CAST(GETDATE() AS DATE)"

                Using cmd As New SqlCommand(query, conn)
                    ' Execute the query and check if result is NULL
                    Dim result As Object = cmd.ExecuteScalar()

                    ' Check if result is DBNull (null) and set totalSales to 0 if no sales today
                    Dim totalSales As Decimal = If(result Is DBNull.Value OrElse result Is Nothing, 0D, Convert.ToDecimal(result))

                    ' Update the label with total sales
                    lblTotalSales.Text = totalSales.ToString("C")
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading total sales: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Load the quantity in stock
    Private Sub LoadQuantityInStock()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' SQL query to get QuantityInStock from Inventory
                Dim query As String = "SELECT SUM(QuantityInStock) FROM Inventory"

                Using cmd As New SqlCommand(query, conn)
                    ' Execute the query and check if result is NULL
                    Dim result As Object = cmd.ExecuteScalar()

                    ' Check if result is DBNull (null) and set QuantityInStock to 0 if no stock data
                    Dim quantityInStock As Integer = 0
                    If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                        quantityInStock = Convert.ToInt32(result)
                    End If

                    ' Update the label with the quantity in stock
                    lblQuantityStock.Text = quantityInStock.ToString()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading quantity in stock: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadCriticalItems()
        Try
            Using conn As New SqlConnection(connectionString)
                conn.Open()

                ' SQL query to count products with 5 or fewer items in stock
                Dim query As String = "SELECT COUNT(*) FROM Inventory WHERE QuantityInStock <= 5"

                Using cmd As New SqlCommand(query, conn)
                    ' Execute the query and check if result is NULL
                    Dim result As Object = cmd.ExecuteScalar()

                    ' Check if result is DBNull (null) and set CriticalItemsCount to 0 if no products match
                    Dim criticalItemsCount As Integer = 0
                    If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                        criticalItemsCount = Convert.ToInt32(result)
                    End If

                    ' Update the label with the count of critical items
                    lblCriticalItems.Text = criticalItemsCount.ToString()
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading critical items: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub









    ' Function to show a form inside a panel with proper resizing
    Private Sub ShowFormInPanel(childForm As Form)
        ' Clear previous form inside the panel
        MainPanel.Controls.Clear()

        ' Ensure panel is properly sized before adding the form
        ' This is crucial for different screen resolutions
        MainPanel.Visible = True
        MainPanel.Dock = DockStyle.Fill
        MainPanel.BringToFront()

        ' Remove Panel5 to prevent overlap issues
        If Panel5.Visible Then
            Panel5.Visible = False
        End If

        ' Set form properties for proper display
        childForm.TopLevel = False
        childForm.FormBorderStyle = FormBorderStyle.None
        childForm.Dock = DockStyle.Fill

        ' Add to panel and show
        MainPanel.Controls.Add(childForm)
        childForm.BringToFront()
        childForm.Show()

        ' Force layout update
        MainPanel.PerformLayout()
        Application.DoEvents()
    End Sub


    ' Menu item event handlers
    Private Sub userMenuItem_Click(sender As Object, e As EventArgs)
        ShowFormInPanel(New User)
        MainPanel.Visible = True
    End Sub


    Private Sub productMenuItem_Click(sender As Object, e As EventArgs)
        ShowFormInPanel(New Product)
        MainPanel.Visible = True

    End Sub

    Private Sub categoryMenuItem_Click(sender As Object, e As EventArgs)
        ShowFormInPanel(New Category)
        MainPanel.Visible = True

    End Sub

    Private Sub SupplierMenuItem_Click(sender As Object, e As EventArgs)
        ShowFormInPanel(New Supplier)
        MainPanel.Visible = True

    End Sub

    Private Sub vatMenuItem_Click(sender As Object, e As EventArgs)
        ShowFormInPanel(New Vat)
        MainPanel.Visible = True

    End Sub

    Private Sub discountMenuItem_Click(sender As Object, e As EventArgs)
        ShowFormInPanel(New Discount)
        MainPanel.Visible = True

    End Sub










































    '============================FOR UI==================================================== 

    'FOR PANEL
    Private Sub UserPanel_Paint(sender As Object, e As PaintEventArgs) Handles UserPanel.Paint
        ' Enable anti-aliasing for smoother edges
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Define the corner radius
        Dim cornerRadius As Integer = 20

        ' Create a rounded rectangle path
        Dim path As New Drawing2D.GraphicsPath()
        Dim rect As New Rectangle(0, 0, UserPanel.Width, UserPanel.Height)

        ' Add rounded corners
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' Apply rounded region to the panel
        UserPanel.Region = New Region(path)

        ' Fill the background (optional)
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#FFFFFF"))
            g.FillPath(brush, path)
        End Using

        ' Draw the border (optional)
        Using pen As New Pen(ColorTranslator.FromHtml("#FFFFFF"), 2)
            g.DrawPath(pen, path)
        End Using
    End Sub

    Private Sub ButtonPanel_Paint(sender As Object, e As PaintEventArgs) Handles ButtonPanel.Paint
        ' Set up the Graphics object with anti-aliasing for smooth edges
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias ' Enable anti-aliasing for smoother edges

        ' Create a path for rounded corners
        Dim path As New Drawing2D.GraphicsPath()
        Dim cornerRadius As Integer = 20 ' Adjust this value for more or less rounded corners
        Dim rect As New Rectangle(0, 0, ButtonPanel.Width, ButtonPanel.Height)

        ' Adjusting arc positions to prevent artifacts
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' **Set region to make the panel truly rounded**
        ButtonPanel.Region = New Region(path)

        ' Fill the background color
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#B0B0B0"))
            g.FillPath(brush, path) ' White background
        End Using

        ' Draw the border
        Using pen As New Pen(ColorTranslator.FromHtml("#B0B0B0"), 2)
            g.DrawPath(pen, path) ' Dark border
        End Using
    End Sub



    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs) Handles Panel5.Paint
        ' Set up the Graphics object with anti-aliasing for smooth edges
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias ' Enable anti-aliasing for smoother edges

        ' Create a path for rounded corners
        Dim path As New Drawing2D.GraphicsPath()
        Dim cornerRadius As Integer = 20 ' Adjust this value for more or less rounded corners
        Dim rect As New Rectangle(0, 0, Panel5.Width, Panel5.Height) ' Use Panel5 dimensions

        ' Adjusting arc positions to prevent artifacts
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' **Set region to make the panel truly rounded**
        Panel5.Region = New Region(path)

        ' Fill the background color
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#FFFFFF"))
            g.FillPath(brush, path) ' White background
        End Using

        ' Draw the border
        Using pen As New Pen(ColorTranslator.FromHtml("#FFFFFF"), 2)
            g.DrawPath(pen, path) ' Border
        End Using
    End Sub

    Private Sub PanelTotalSales_Paint(sender As Object, e As PaintEventArgs) Handles PanelTotalSales.Paint
        ' Set up the Graphics object with anti-aliasing for smooth edges
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias ' Enable anti-aliasing for smoother edges

        ' Create a path for rounded corners
        Dim path As New Drawing2D.GraphicsPath
        Dim cornerRadius = 20 ' Adjust this value for more or less rounded corners
        Dim rect As New Rectangle(0, 0, PanelTotalSales.Width, PanelTotalSales.Height) ' Use PanelTotalSales dimensions

        ' Adjusting arc positions to prevent artifacts
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' **Set region to make the panel truly rounded**
        PanelTotalSales.Region = New Region(path)

        ' Fill the background color
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#60B5FF"))
            g.FillPath(brush, path) ' Fill the background
        End Using

        ' Draw the border
        Using pen As New Pen(ColorTranslator.FromHtml("#60B5FF"), 2)
            g.DrawPath(pen, path) ' Draw the border
        End Using
    End Sub

    Private Sub PanelQuantityStock_Paint(sender As Object, e As PaintEventArgs) Handles PanelQuantityStock.Paint
        ' Set up the Graphics object with anti-aliasing for smooth edges
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias ' Enable anti-aliasing for smoother edges

        ' Create a path for rounded corners
        Dim path As New Drawing2D.GraphicsPath
        Dim cornerRadius = 20 ' Adjust this value for more or less rounded corners
        Dim rect As New Rectangle(0, 0, PanelQuantityStock.Width, PanelQuantityStock.Height) ' Use PanelQuantityStock dimensions

        ' Adjusting arc positions to prevent artifacts
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' **Set region to make the panel truly rounded**
        PanelQuantityStock.Region = New Region(path)

        ' Fill the background color
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#FEC260"))
            g.FillPath(brush, path) ' Fill the background
        End Using

        ' Draw the border
        Using pen As New Pen(ColorTranslator.FromHtml("#FEC260"), 2)
            g.DrawPath(pen, path) ' Draw the border
        End Using
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ' Set up the Graphics object with anti-aliasing for smooth edges
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias ' Enable anti-aliasing for smoother edges

        ' Create a path for rounded corners
        Dim path As New Drawing2D.GraphicsPath
        Dim cornerRadius = 20 ' Adjust this value for more or less rounded corners
        Dim rect As New Rectangle(0, 0, Panel1.Width, Panel1.Height) ' Use Panel1 dimensions

        ' Adjusting arc positions to prevent artifacts
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' **Set region to make the panel truly rounded**
        Panel1.Region = New Region(path)

        ' Fill the background color
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#1DCD9F"))
            g.FillPath(brush, path) ' Fill the background
        End Using

        ' Draw the border
        Using pen As New Pen(ColorTranslator.FromHtml("#1DCD9F"), 2)
            g.DrawPath(pen, path) ' Draw the border
        End Using
    End Sub


    '========================= FOR BUTTONS =====================================

    'FOR BUTTON
    Public Class ButtonStyler
        ' Function para i-apply ang styling sa buttons
        Public Shared Sub ApplyStyle(btn As Button)
            ' Default Button Style
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0
            btn.BackColor = ColorTranslator.FromHtml("#251F1F") ' Default background color
            btn.ForeColor = Color.White ' Default text color
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent ' No default hover effect

            ' Add hover effect
            AddHandler btn.MouseEnter, AddressOf ButtonHover
            AddHandler btn.MouseLeave, AddressOf ButtonLeave
        End Sub

        ' Kapag naka-hover (Mouse Enter)
        Private Shared Sub ButtonHover(sender As Object, e As EventArgs)
            Dim btn As Button = DirectCast(sender, Button)
            btn.BackColor = ColorTranslator.FromHtml("#251F1F") ' Darker hover effect
        End Sub

        ' Kapag iniwan ang hover (Mouse Leave)
        Private Shared Sub ButtonLeave(sender As Object, e As EventArgs)
            Dim btn As Button = DirectCast(sender, Button)
            btn.BackColor = ColorTranslator.FromHtml("#251F1F") ' Balik sa default color
        End Sub
    End Class



    '================= MAIN BUTTONS ==============================

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        ' Hide MainPanel and show the dashboard panel
        MainPanel.Visible = False
        MainPanel.Controls.Clear()
        Panel5.Visible = True
        Panel5.BringToFront()
        ReloadUI()
    End Sub

    Private Sub btnMaintenance_Click(sender As Object, e As EventArgs) Handles btnMaintenance.Click
        ' Kunin ang screen position ng btnMaintenance at itulak ang ContextMenu sa kanan ng button
        Dim btnPosition As Point = btnMaintenance.PointToScreen(New Point(btnMaintenance.Width, 0))

        ' Ipakita ang ContextMenuStripMaintenance sa tabi ng button
        ContextMenuStripMaintenance.Show(btnPosition)
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        ' Kunin ang screen position ng btnReports at itulak ang ContextMenu sa kanan ng button
        Dim btnPosition As Point = btnReports.PointToScreen(New Point(btnReports.Width, 0))

        ' Ipakita ang ContextMenuReports sa tabi ng button
        ContextMenuStripReports.Show(btnPosition)
    End Sub


    ' If the constructor expects a string userRole
    Private Sub btnPOS_Click(sender As Object, e As EventArgs) Handles btnPOS.Click
        ShowFormInPanel(New POS("Admin"))  ' Pass the required parameter (userRole) to the constructor
        MainPanel.Visible = True
    End Sub

    Private Sub btnInventory_Click(sender As Object, e As EventArgs) Handles btnInventory.Click
        ShowFormInPanel(New Inventory)
        MainPanel.Visible = True
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        ' I-set ang posisyon ng ContextMenu sa kanan ng button
        Dim btnPosition = btnSettings.PointToScreen(New Point(btnSettings.Width, 0))

        ' Ipakita ang ContextMenuSettings sa tabi ng button
        ContextMenuStripSettings.Show(btnPosition)
    End Sub

    Private Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        ' Retrieve the actual user's role and full name from the stored session-like data
        Dim userRole = role ' Get the role from the session-like data
        Dim userFullName = fullName ' Get the full name from the session-like data

        ' Define the action that occurred (user logged out)
        Dim action = "User logged Out"

        ' Call the LogHistoryEntry method to log this action in the database
        LogHistoryEntry(userRole, userFullName, action)

        ' Restart the application after logging out
        Application.Restart()
    End Sub



    ' ============= FOR MENU STRIP ============
    Private Sub FastMovingProductsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FastMovingProductsToolStripMenuItem.Click
        ShowFormInPanel(New FastMovingProduct)
        MainPanel.Visible = True
    End Sub

    Private Sub ExpirationReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpirationReportsToolStripMenuItem.Click
        ShowFormInPanel(New ExpirationReports)
        MainPanel.Visible = True
    End Sub

    Private Sub SalesReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalesReportsToolStripMenuItem.Click
        ShowFormInPanel(New SalesReports)
        MainPanel.Visible = True
    End Sub



    Private Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click
        ShowFormInPanel(New User)
        MainPanel.Visible = True
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierToolStripMenuItem.Click
        ShowFormInPanel(New Supplier)
        MainPanel.Visible = True
    End Sub

    Private Sub CategoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CategoryToolStripMenuItem.Click
        ShowFormInPanel(New Category)
        MainPanel.Visible = True
    End Sub

    Private Sub DiscountToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DiscountToolStripMenuItem.Click
        Dim Discount As New Discount()

        Discount.Show()
    End Sub

    Private Sub VatToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VatToolStripMenuItem.Click
        Dim Vat As New Vat()

        Vat.Show()
    End Sub

    Private Sub ProductToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductToolStripMenuItem.Click
        ShowFormInPanel(New Product)
        MainPanel.Visible = True
    End Sub

    Private Sub AuditTrailToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AuditTrailToolStripMenuItem.Click
        ShowFormInPanel(New AuditTrail)
        MainPanel.Visible = True
    End Sub

    Private Sub LogHistoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogHistoryToolStripMenuItem.Click
        ShowFormInPanel(New LogHistory)
        MainPanel.Visible = True
    End Sub

    Private Sub VoidedTransactionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VoidedTransactionToolStripMenuItem.Click
        ShowFormInPanel(New VoidedTransaction)
        MainPanel.Visible = True
    End Sub
    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        ' Declare the new form instance (Dim)
        Dim changePasswordForm As New ChangePassword()

        ' Show the form (you can choose when to show it, or add further logic to show it later)
        changePasswordForm.Show()
    End Sub



    '============ UI ================
    Private Sub PanelTotalProduct_Paint(sender As Object, e As PaintEventArgs) Handles PanelTotalProduct.Paint
        ' Set up the Graphics object with anti-aliasing for smooth edges
        Dim g = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias ' Enable anti-aliasing for smoother edges

        ' Create a path for rounded corners
        Dim path As New Drawing2D.GraphicsPath
        Dim cornerRadius = 20 ' Adjust this value for more or less rounded corners
        Dim rect As New Rectangle(0, 0, PanelTotalProduct.Width, PanelTotalProduct.Height) ' Use the correct panel dimensions

        ' Adjusting arc positions to prevent artifacts
        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90) ' Top-left corner
        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90) ' Top-right corner
        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90) ' Bottom-right corner
        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90) ' Bottom-left corner
        path.CloseFigure()

        ' **Set region to make the panel truly rounded**
        PanelTotalProduct.Region = New Region(path)

        ' Fill the background color
        Using brush As New SolidBrush(ColorTranslator.FromHtml("#E78895"))
            g.FillPath(brush, path) ' Fill the background
        End Using

        ' Draw the border
        Using pen As New Pen(ColorTranslator.FromHtml("#E78895"), 2)
            g.DrawPath(pen, path) ' Draw the border
        End Using
    End Sub




    Private Sub SalesReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalesReportsToolStripMenuItem.Click
        ShowFormInPanel(New SalesReports)
        MainPanel.Visible = True
    End Sub

    ' Mag-create ng method para sa pag-backup ng database
    'Public Sub BackupDB()
    '    ' I-setup ang connection string (palitan ng tamang server, database, at credentials)
    '    Dim connectionString As String = "Data Source=LAPTOP-HC3L03IC\SQLEXPRESS;Initial Catalog=smgs;Integrated Security=True;Trust Server Certificate=True"

    '    ' Ang path kung saan mo gustong i-save ang backup file (tandaan ang .bak extension)
    '    Dim backupFilePath As String = "C:\Users\Aspire 5\Desktop\Backup"

    '    ' SQL query para mag-backup ng database
    '    Dim backupQuery As String = "BACKUP DATABASE [smgs] TO DISK = '" & backupFilePath & "'"

    '    ' Gumawa ng koneksyon sa database
    '    Using connection As New SqlConnection(connectionString)
    '        Try
    '            ' I-open ang connection
    '            connection.Open()

    '            ' Gumawa ng command para mag-execute ng backup query
    '            Using command As New SqlCommand(backupQuery, connection)
    '                ' I-execute ang backup query
    '                command.ExecuteNonQuery()

    '                MessageBox.Show("Database backup successful!")
    '            End Using
    '        Catch ex As Exception
    '            ' I-handle ang errors
    '            MessageBox.Show("Error: " & ex.Message)
    '        End Try
    '    End Using
    'End Sub


    '' Event handler ng button click
    'Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
    '        ' I-trigger ang backup method kapag ni-click ang button
    '        BackupDB()
    '    End Sub


    Private Sub LogHistoryEntry(ByVal Role As String, ByVal FullName As String, ByVal Action As String)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim cmd As New SqlCommand("INSERT INTO loghistory (Role, FullName, Action, Date) VALUES (@Role, @FullName, @Action, GETDATE())", connection)
                cmd.Parameters.AddWithValue("@Role", Role)
                cmd.Parameters.AddWithValue("@FullName", FullName)
                cmd.Parameters.AddWithValue("@Action", Action)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error logging action: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MainPanel_Paint(sender As Object, e As PaintEventArgs) Handles MainPanel.Paint
        MainPanel.Size = New Size(1761, 956)
    End Sub

    Private Sub StockInReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StockInReportsToolStripMenuItem.Click
        Dim Vat As New DeliveryList()

        DeliveryList.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Close()
    End Sub


End Class
