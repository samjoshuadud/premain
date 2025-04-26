Namespace GlobalSession
    Module SessionData
        ' Stores the ID of the currently logged-in user
        Public CurrentUserId As Integer

        ' Stores the role of the currently logged-in user (e.g., "Cashier", "Admin")
        Public role As String

        ' Stores the full name of the logged-in user
        Public fullName As String



        Public Class SessionData
            Public Shared CurrentUserId As Integer
            Public Shared Role As String
            Public Shared FullName As String
            Public Shared CurrentUsername As String ' ✅ Add this line
        End Class

    End Module
End Namespace
