
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net.Security
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Policy
Imports System.Text.Encodings.Web
Imports System.Xml
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.Logging
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Imports IolMasterDefinitions

Public Class Form1

    Dim ds As DataSet
    Private iolMasters As clsIolMasterDefinitions
    Private ioddpath As String
    Private cssStyling As String

    Private currentOutput As String

    Private currentFile As String

    Private frmBrxExp As New frmBrxExport

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim userDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\IODD_XML_parser"
        Dim env = CoreWebView2Environment.CreateAsync(Nothing, userDataFolder)
        env.Wait()
        Await WebView21.EnsureCoreWebView2Async(env.Result)
        Await WebView22.EnsureCoreWebView2Async(env.Result)
        loadHowTo()
        Dim path As String = AppDomain.CurrentDomain.BaseDirectory
        iolMasters = New clsIolMasterDefinitions(path & "\IOLDef\", True)
        If System.IO.File.Exists(path & "\styling.css") Then cssStyling = System.IO.File.ReadAllText(path & "\styling.css")
        tscbIolMasters.Items.AddRange(iolMasters.masters.ToArray) : tscbIolMasters.SelectedIndex = 0
    End Sub
    Private Sub LoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadToolStripMenuItem.Click

        Dim ofd As New OpenFileDialog
        If ofd.ShowDialog = DialogResult.OK Then
            loadIODD(ofd.FileName)
        End If
    End Sub
    Private Sub loadIODD(file As String)
        If Not System.IO.File.Exists(file) Then Return
        currentFile = file
        If file.EndsWith("IODD1.1.xml") Then
            Try
                Dim iodd11 As New IODD11Parser.clsIODD11parser(file, cssStyling, tscbIolMasterPorts.SelectedItem, tscbIolMasters.SelectedItem)
                currentOutput = iodd11.currentOutput
                WebView21.NavigateToString(currentOutput)
                ds = New DataSet
                ds.Merge(iodd11.ds)
                initBrxExportTab()
            Catch ex As Exception
                MsgBox("Can't load file " & System.IO.Path.GetFileName(file))
            End Try
        Else
            Try
                Dim iodd10 As New IODD10parser.clsIODD10parser(file, cssStyling, tscbIolMasterPorts.SelectedItem, tscbIolMasters.SelectedItem)
                currentOutput = iodd10.currentOutput
                WebView21.NavigateToString(currentOutput)
                ds = New DataSet
                ds.Merge(iodd10.ds)
                initBrxExportTab()
            Catch ex As Exception
                MsgBox("Can't load file " & System.IO.Path.GetFileName(file))
            End Try
        End If
    End Sub

    Private Sub tscbIolMasters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tscbIolMasters.SelectedIndexChanged
        tscbIolMasterPorts.Items.Clear()
        If CType(tscbIolMasters.SelectedItem, IOLMaster).ports.Count > 0 Then
            tscbIolMasterPorts.Items.AddRange(CType(tscbIolMasters.SelectedItem, IOLMaster).ports.ToArray)
            tscbIolMasterPorts.SelectedIndex = 0
        Else
            If (Not String.IsNullOrWhiteSpace(currentFile)) Then loadIODD(currentFile)
        End If
    End Sub

    Private Sub tscbIolMasterPorts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tscbIolMasterPorts.SelectedIndexChanged
        If (Not String.IsNullOrWhiteSpace(currentFile)) Then loadIODD(currentFile)
    End Sub


    Private Sub PdfWithBackgroundToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PdfWithBackgroundToolStripMenuItem.Click
        Dim sfd As New SaveFileDialog()
        sfd.InitialDirectory = ioddpath
        sfd.FileName = WebView21.CoreWebView2.DocumentTitle & ".pdf"
        If sfd.ShowDialog = DialogResult.OK Then
            Dim ps As CoreWebView2PrintSettings = WebView21.CoreWebView2.Environment.CreatePrintSettings
            ps.ColorMode = CoreWebView2PrintColorMode.Color
            ps.ShouldPrintBackgrounds = True
            WebView21.CoreWebView2.PrintToPdfAsync(sfd.FileName, ps)
        End If
    End Sub

    Private Sub SaveHtmlToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveHtmlToolStripMenuItem.Click
        Dim sfd As New SaveFileDialog()
        sfd.InitialDirectory = ioddpath
        sfd.FileName = WebView21.CoreWebView2.DocumentTitle & ".html"
        If sfd.ShowDialog = DialogResult.OK Then
            System.IO.File.WriteAllText(sfd.FileName, currentOutput)
        End If
    End Sub

    Private Sub PdfNoBackgroundColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PdfNoBackgroundColorToolStripMenuItem.Click
        Dim sfd As New SaveFileDialog()
        sfd.InitialDirectory = ioddpath
        sfd.FileName = WebView21.CoreWebView2.DocumentTitle & "_NoBackground.pdf"
        If sfd.ShowDialog = DialogResult.OK Then
            Dim ps As CoreWebView2PrintSettings = WebView21.CoreWebView2.Environment.CreatePrintSettings
            ps.ColorMode = CoreWebView2PrintColorMode.Grayscale
            ps.ShouldPrintBackgrounds = False
            WebView21.CoreWebView2.PrintToPdfAsync(sfd.FileName, ps)
        End If
    End Sub





    'Private Sub ExportBRXToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportBRXToolStripMenuItem.Click
    '    If IsNothing(ds) Then Return
    '    frmBrxExp = New frmBrxExport
    '    frmBrxExp.ds = New DataSet
    '    frmBrxExp.ds.Merge(ds)
    '    If Not IsNothing(tscbIolMasterPorts.SelectedItem) Then
    '        frmBrxExp.inOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).inByteStart
    '        frmBrxExp.outOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).outByteStart
    '        frmBrxExp.port = CType(tscbIolMasterPorts.SelectedItem, portInfo).name
    '    End If

    '    frmBrxExp.ShowDialog()
    'End Sub
End Class