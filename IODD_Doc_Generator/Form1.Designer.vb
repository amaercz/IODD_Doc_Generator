<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionallySelectMasterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tscbIolMasters = New System.Windows.Forms.ToolStripComboBox()
        Me.tscbIolMasterPorts = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveHtmlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PdfWithBackgroundToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PdfNoBackgroundColorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tstbVer = New System.Windows.Forms.ToolStripTextBox()
        Me.WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.WebView21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadToolStripMenuItem, Me.OptionallySelectMasterToolStripMenuItem, Me.tscbIolMasters, Me.tscbIolMasterPorts, Me.ToolStripMenuItem1, Me.SaveToolStripMenuItem, Me.tstbVer})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(3848, 96)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(189, 88)
        Me.LoadToolStripMenuItem.Text = "load IODD File"
        '
        'OptionallySelectMasterToolStripMenuItem
        '
        Me.OptionallySelectMasterToolStripMenuItem.Enabled = False
        Me.OptionallySelectMasterToolStripMenuItem.Name = "OptionallySelectMasterToolStripMenuItem"
        Me.OptionallySelectMasterToolStripMenuItem.Size = New System.Drawing.Size(376, 88)
        Me.OptionallySelectMasterToolStripMenuItem.Text = "Optionally select Master/Port-> "
        '
        'tscbIolMasters
        '
        Me.tscbIolMasters.AutoSize = False
        Me.tscbIolMasters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tscbIolMasters.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tscbIolMasters.Name = "tscbIolMasters"
        Me.tscbIolMasters.Size = New System.Drawing.Size(356, 40)
        '
        'tscbIolMasterPorts
        '
        Me.tscbIolMasterPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tscbIolMasterPorts.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tscbIolMasterPorts.Name = "tscbIolMasterPorts"
        Me.tscbIolMasterPorts.Size = New System.Drawing.Size(238, 88)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(20, 88)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveHtmlToolStripMenuItem, Me.PdfWithBackgroundToolStripMenuItem, Me.PdfNoBackgroundColorToolStripMenuItem})
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(370, 88)
        Me.SaveToolStripMenuItem.Text = "save generated Documentation"
        '
        'SaveHtmlToolStripMenuItem
        '
        Me.SaveHtmlToolStripMenuItem.Name = "SaveHtmlToolStripMenuItem"
        Me.SaveHtmlToolStripMenuItem.Size = New System.Drawing.Size(412, 44)
        Me.SaveHtmlToolStripMenuItem.Text = "save html"
        '
        'PdfWithBackgroundToolStripMenuItem
        '
        Me.PdfWithBackgroundToolStripMenuItem.Name = "PdfWithBackgroundToolStripMenuItem"
        Me.PdfWithBackgroundToolStripMenuItem.Size = New System.Drawing.Size(412, 44)
        Me.PdfWithBackgroundToolStripMenuItem.Text = "pdf with background"
        '
        'PdfNoBackgroundColorToolStripMenuItem
        '
        Me.PdfNoBackgroundColorToolStripMenuItem.Name = "PdfNoBackgroundColorToolStripMenuItem"
        Me.PdfNoBackgroundColorToolStripMenuItem.Size = New System.Drawing.Size(412, 44)
        Me.PdfNoBackgroundColorToolStripMenuItem.Text = "pdf no background color"
        '
        'tstbVer
        '
        Me.tstbVer.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tstbVer.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tstbVer.Enabled = False
        Me.tstbVer.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tstbVer.Name = "tstbVer"
        Me.tstbVer.ReadOnly = True
        Me.tstbVer.Size = New System.Drawing.Size(80, 88)
        Me.tstbVer.Text = "1.0.4b1"
        Me.tstbVer.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'WebView21
        '
        Me.WebView21.AllowExternalDrop = True
        Me.WebView21.CreationProperties = Nothing
        Me.WebView21.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WebView21.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebView21.Location = New System.Drawing.Point(0, 192)
        Me.WebView21.Margin = New System.Windows.Forms.Padding(4)
        Me.WebView21.Name = "WebView21"
        Me.WebView21.Size = New System.Drawing.Size(3848, 1932)
        Me.WebView21.TabIndex = 3
        Me.WebView21.ZoomFactor = 1.0R
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1924, 1062)
        Me.Controls.Add(Me.WebView21)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
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
    Friend WithEvents tstbVer As ToolStripTextBox
End Class
