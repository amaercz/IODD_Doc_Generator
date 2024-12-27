Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.Json

Public Class clsIolMasterDefinitions
    Public masters As List(Of IOLMaster)

    Public Sub New()

    End Sub

    Public Sub New(definitionsDir As String, Optional includeNoneMaster As Boolean = False)
        masters = New List(Of IOLMaster)
        If includeNoneMaster Then masters.Add(New IOLMaster("none", ""))
        If Directory.Exists(definitionsDir) Then
            Dim files() As String = System.IO.Directory.GetFiles(definitionsDir, "*.iolmstr", SearchOption.AllDirectories)
            For Each file As String In files
                Try
                    masters.Add(JsonSerializer.Deserialize(System.IO.File.ReadAllText(file), GetType(IOLMaster)))
                Catch ex As Exception
                    MsgBox(System.IO.Path.GetFileName(file) & vbCrLf & ex.InnerException.Message)
                End Try
            Next
        End If
    End Sub
End Class

Public Class IOLMaster
    Public Sub New()
        ports = New List(Of portInfo)
    End Sub

    Public Sub New(manufacturer As String, model As String)
        ports = New List(Of portInfo)
        Me.manufacturer = manufacturer
        Me.model = model
    End Sub


    Property manufacturer As String
    Property model As String

    Property ports As List(Of portInfo)
    Public Overrides Function tostring() As String
        Dim str() As String = IIf(ports.Count > 0, {manufacturer, model, ports.Count & " ports"}.ToArray, IIf(String.IsNullOrWhiteSpace(model), {manufacturer}.ToArray, {manufacturer, model}.ToArray))
        Return String.Join(", ", str)
    End Function
End Class


Public Class portInfo
    Property name As String
    Property inByteCount As Integer
    Property inByteStart As Integer
    Property outByteCount As Integer
    Property outByteStart As Integer

    Public Overrides Function tostring() As String
        Return IIf(Not String.IsNullOrWhiteSpace(name), name, "no name yet")
    End Function

End Class