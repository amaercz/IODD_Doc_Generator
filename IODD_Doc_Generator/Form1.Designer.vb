﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tscbIolMasters = New System.Windows.Forms.ToolStripComboBox()
        Me.tscbIolMasterPorts = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveHtmlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PdfWithBackgroundToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PdfNoBackgroundColorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.OptionallySelectMasterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadToolStripMenuItem, Me.OptionallySelectMasterToolStripMenuItem, Me.tscbIolMasters, Me.tscbIolMasterPorts, Me.ToolStripMenuItem1, Me.SaveToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(3, 1, 0, 1)
        Me.MenuStrip1.Size = New System.Drawing.Size(962, 25)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(112, 23)
        Me.LoadToolStripMenuItem.Text = "load IODD 1.1 File"
        '
        'tscbIolMasters
        '
        Me.tscbIolMasters.AutoSize = False
        Me.tscbIolMasters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tscbIolMasters.Name = "tscbIolMasters"
        Me.tscbIolMasters.Size = New System.Drawing.Size(180, 23)
        '
        'tscbIolMasterPorts
        '
        Me.tscbIolMasterPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tscbIolMasterPorts.Name = "tscbIolMasterPorts"
        Me.tscbIolMasterPorts.Size = New System.Drawing.Size(121, 23)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(12, 23)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveHtmlToolStripMenuItem, Me.PdfWithBackgroundToolStripMenuItem, Me.PdfNoBackgroundColorToolStripMenuItem})
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(178, 23)
        Me.SaveToolStripMenuItem.Text = "save generatd Documentation"
        '
        'SaveHtmlToolStripMenuItem
        '
        Me.SaveHtmlToolStripMenuItem.Name = "SaveHtmlToolStripMenuItem"
        Me.SaveHtmlToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.SaveHtmlToolStripMenuItem.Text = "save html"
        '
        'PdfWithBackgroundToolStripMenuItem
        '
        Me.PdfWithBackgroundToolStripMenuItem.Name = "PdfWithBackgroundToolStripMenuItem"
        Me.PdfWithBackgroundToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.PdfWithBackgroundToolStripMenuItem.Text = "pdf with background"
        '
        'PdfNoBackgroundColorToolStripMenuItem
        '
        Me.PdfNoBackgroundColorToolStripMenuItem.Name = "PdfNoBackgroundColorToolStripMenuItem"
        Me.PdfNoBackgroundColorToolStripMenuItem.Size = New System.Drawing.Size(206, 22)
        Me.PdfNoBackgroundColorToolStripMenuItem.Text = "pdf no background color"
        '
        'WebView21
        '
        Me.WebView21.AllowExternalDrop = True
        Me.WebView21.CreationProperties = Nothing
        Me.WebView21.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView21.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebView21.Location = New System.Drawing.Point(0, 25)
        Me.WebView21.Margin = New System.Windows.Forms.Padding(2)
        Me.WebView21.Name = "WebView21"
        Me.WebView21.Size = New System.Drawing.Size(962, 527)
        Me.WebView21.TabIndex = 3
        Me.WebView21.ZoomFactor = 1.0R
        '
        'OptionallySelectMasterToolStripMenuItem
        '
        Me.OptionallySelectMasterToolStripMenuItem.Enabled = False
        Me.OptionallySelectMasterToolStripMenuItem.Name = "OptionallySelectMasterToolStripMenuItem"
        Me.OptionallySelectMasterToolStripMenuItem.Size = New System.Drawing.Size(189, 23)
        Me.OptionallySelectMasterToolStripMenuItem.Text = "Optionally select Master/Port-> "
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 552)
        Me.Controls.Add(Me.WebView21)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Form1"
        Me.Text = "IODD Doc Generator"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents LoadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tscbIolMasters As ToolStripComboBox
    Friend WithEvents tscbIolMasterPorts As ToolStripComboBox
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveHtmlToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PdfWithBackgroundToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PdfNoBackgroundColorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents OptionallySelectMasterToolStripMenuItem As ToolStripMenuItem
End Class