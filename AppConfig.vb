Public Class AppConfig
    ' Database connection string
    Public Shared ReadOnly Property ConnectionString As String
        Get
            Return "Data Source=192.168.55.105,1433;Initial Catalog=updated;User ID=oreo123;Password=oreo12345;Encrypt=True;TrustServerCertificate=True"
        End Get
    End Property

    ' Resources folder path
    'Public Shared ReadOnly Property ResourcesPath As String
    '    Get
    '        'Return "C:\Users\Caleb\Desktop\BACK UP\smgsisbstp2025\Resources"
    '    End Get
    'End Property
End Class 