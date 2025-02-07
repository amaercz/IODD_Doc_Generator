<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBrxExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBrxExport))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ExportBRXToolStripMenuItem = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbSelectedOption = New System.Windows.Forms.ComboBox()
        Me.gbInPD = New System.Windows.Forms.GroupBox()
        Me.dgvIn = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.gbOutPD = New System.Windows.Forms.GroupBox()
        Me.dgvOut = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.gbInSourceData = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudSourceStartElement = New System.Windows.Forms.NumericUpDown()
        Me.tbInSourceBlock = New System.Windows.Forms.TextBox()
        Me.gbInUdtName = New System.Windows.Forms.GroupBox()
        Me.tbInUdtName = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudTargetStartElement = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbOutTargetBlock = New System.Windows.Forms.TextBox()
        Me.gbOutUdtName = New System.Windows.Forms.GroupBox()
        Me.tbOutUdtName = New System.Windows.Forms.TextBox()
        Me.gbInUdt = New System.Windows.Forms.GroupBox()
        Me.dgvInUdt = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.dgvOutUdt = New System.Windows.Forms.DataGridView()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbDeviceName = New System.Windows.Forms.GroupBox()
        Me.tbUdtHeapItem = New System.Windows.Forms.TextBox()
        Me.gbSubRoutineName = New System.Windows.Forms.GroupBox()
        Me.tbSubRoutineName = New System.Windows.Forms.TextBox()
        Me.gbGlobalUdtName = New System.Windows.Forms.GroupBox()
        Me.tbMainUdtName = New System.Windows.Forms.TextBox()
        Me.gbSettings = New System.Windows.Forms.GroupBox()
        Me.gbAdvancedOptions = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbInPD.SuspendLayout()
        CType(Me.dgvIn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbOutPD.SuspendLayout()
        CType(Me.dgvOut, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbInSourceData.SuspendLayout()
        CType(Me.nudSourceStartElement, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbInUdtName.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.nudTargetStartElement, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbOutUdtName.SuspendLayout()
        Me.gbInUdt.SuspendLayout()
        CType(Me.dgvInUdt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.dgvOutUdt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.gbDeviceName.SuspendLayout()
        Me.gbSubRoutineName.SuspendLayout()
        Me.gbGlobalUdtName.SuspendLayout()
        Me.gbSettings.SuspendLayout()
        Me.gbAdvancedOptions.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportBRXToolStripMenuItem})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(896, 39)
        Me.ToolStrip1.TabIndex = 8
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ExportBRXToolStripMenuItem
        '
        Me.ExportBRXToolStripMenuItem.Image = CType(resources.GetObject("ExportBRXToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExportBRXToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ExportBRXToolStripMenuItem.Name = "ExportBRXToolStripMenuItem"
        Me.ExportBRXToolStripMenuItem.Size = New System.Drawing.Size(77, 36)
        Me.ExportBRXToolStripMenuItem.Text = "Export"
        '
        'GroupBox1
        '
        Me.GroupBox1.CausesValidation = False
        Me.GroupBox1.Controls.Add(Me.cbSelectedOption)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(643, 49)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Process Data Options (if any)"
        '
        'cbSelectedOption
        '
        Me.cbSelectedOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSelectedOption.FormattingEnabled = True
        Me.cbSelectedOption.Location = New System.Drawing.Point(6, 19)
        Me.cbSelectedOption.Name = "cbSelectedOption"
        Me.cbSelectedOption.Size = New System.Drawing.Size(631, 21)
        Me.cbSelectedOption.TabIndex = 0
        '
        'gbInPD
        '
        Me.gbInPD.Controls.Add(Me.dgvIn)
        Me.gbInPD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbInPD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbInPD.Location = New System.Drawing.Point(3, 53)
        Me.gbInPD.Name = "gbInPD"
        Me.gbInPD.Size = New System.Drawing.Size(439, 124)
        Me.gbInPD.TabIndex = 2
        Me.gbInPD.TabStop = False
        Me.gbInPD.Text = "Process Data IN"
        '
        'dgvIn
        '
        Me.dgvIn.AllowUserToAddRows = False
        Me.dgvIn.AllowUserToDeleteRows = False
        Me.dgvIn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIn.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6})
        Me.dgvIn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvIn.Location = New System.Drawing.Point(3, 16)
        Me.dgvIn.Name = "dgvIn"
        Me.dgvIn.RowHeadersWidth = 5
        Me.dgvIn.Size = New System.Drawing.Size(433, 105)
        Me.dgvIn.TabIndex = 3
        '
        'Column1
        '
        Me.Column1.HeaderText = "UDT Field"
        Me.Column1.MinimumWidth = 10
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 80
        '
        'Column2
        '
        Me.Column2.HeaderText = "Type"
        Me.Column2.MinimumWidth = 10
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 56
        '
        'Column3
        '
        Me.Column3.HeaderText = "BitOffset"
        Me.Column3.MinimumWidth = 10
        Me.Column3.Name = "Column3"
        Me.Column3.Width = 72
        '
        'Column4
        '
        Me.Column4.HeaderText = "BitLength"
        Me.Column4.MinimumWidth = 10
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 77
        '
        'Column5
        '
        Me.Column5.HeaderText = "MinValue"
        Me.Column5.MinimumWidth = 10
        Me.Column5.Name = "Column5"
        Me.Column5.Width = 76
        '
        'Column6
        '
        Me.Column6.HeaderText = "MaxValue"
        Me.Column6.MinimumWidth = 10
        Me.Column6.Name = "Column6"
        Me.Column6.Width = 79
        '
        'gbOutPD
        '
        Me.gbOutPD.Controls.Add(Me.dgvOut)
        Me.gbOutPD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbOutPD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbOutPD.Location = New System.Drawing.Point(448, 53)
        Me.gbOutPD.Name = "gbOutPD"
        Me.gbOutPD.Size = New System.Drawing.Size(439, 124)
        Me.gbOutPD.TabIndex = 3
        Me.gbOutPD.TabStop = False
        Me.gbOutPD.Text = "Process Data OUT"
        '
        'dgvOut
        '
        Me.dgvOut.AllowUserToAddRows = False
        Me.dgvOut.AllowUserToDeleteRows = False
        Me.dgvOut.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvOut.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOut.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6})
        Me.dgvOut.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvOut.Location = New System.Drawing.Point(3, 16)
        Me.dgvOut.Name = "dgvOut"
        Me.dgvOut.RowHeadersWidth = 5
        Me.dgvOut.Size = New System.Drawing.Size(433, 105)
        Me.dgvOut.TabIndex = 3
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "UDT Field"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 80
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Type"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Width = 56
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "BitOffset"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Width = 72
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "BitLength"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 77
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "MinValue"
        Me.DataGridViewTextBoxColumn5.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 76
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "MaxValue"
        Me.DataGridViewTextBoxColumn6.MinimumWidth = 10
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.Width = 79
        '
        'gbInSourceData
        '
        Me.gbInSourceData.Controls.Add(Me.Label1)
        Me.gbInSourceData.Controls.Add(Me.Label2)
        Me.gbInSourceData.Controls.Add(Me.nudSourceStartElement)
        Me.gbInSourceData.Controls.Add(Me.tbInSourceBlock)
        Me.gbInSourceData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbInSourceData.Location = New System.Drawing.Point(3, 58)
        Me.gbInSourceData.Name = "gbInSourceData"
        Me.gbInSourceData.Size = New System.Drawing.Size(643, 49)
        Me.gbInSourceData.TabIndex = 0
        Me.gbInSourceData.TabStop = False
        Me.gbInSourceData.Text = "Input data source data block (has to be byte array)"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(167, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(283, 31)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "<- Edit to match scanner input block name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(498, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Starting index"
        '
        'nudSourceStartElement
        '
        Me.nudSourceStartElement.Location = New System.Drawing.Point(578, 19)
        Me.nudSourceStartElement.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudSourceStartElement.Name = "nudSourceStartElement"
        Me.nudSourceStartElement.Size = New System.Drawing.Size(57, 20)
        Me.nudSourceStartElement.TabIndex = 1
        '
        'tbInSourceBlock
        '
        Me.tbInSourceBlock.Location = New System.Drawing.Point(6, 19)
        Me.tbInSourceBlock.Name = "tbInSourceBlock"
        Me.tbInSourceBlock.Size = New System.Drawing.Size(155, 20)
        Me.tbInSourceBlock.TabIndex = 0
        Me.tbInSourceBlock.Text = "InData"
        '
        'gbInUdtName
        '
        Me.gbInUdtName.Controls.Add(Me.tbInUdtName)
        Me.gbInUdtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbInUdtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbInUdtName.Location = New System.Drawing.Point(3, 3)
        Me.gbInUdtName.MinimumSize = New System.Drawing.Size(0, 42)
        Me.gbInUdtName.Name = "gbInUdtName"
        Me.gbInUdtName.Size = New System.Drawing.Size(439, 44)
        Me.gbInUdtName.TabIndex = 2
        Me.gbInUdtName.TabStop = False
        Me.gbInUdtName.Text = "Input UDT Name"
        '
        'tbInUdtName
        '
        Me.tbInUdtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbInUdtName.Location = New System.Drawing.Point(3, 16)
        Me.tbInUdtName.Name = "tbInUdtName"
        Me.tbInUdtName.Size = New System.Drawing.Size(433, 20)
        Me.tbInUdtName.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.nudTargetStartElement)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.tbOutTargetBlock)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(3, 113)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(643, 43)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Output data target data block (has to be byte array)"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(167, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(269, 31)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "<- Edit to match scanner output block name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudTargetStartElement
        '
        Me.nudTargetStartElement.Location = New System.Drawing.Point(578, 15)
        Me.nudTargetStartElement.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudTargetStartElement.Name = "nudTargetStartElement"
        Me.nudTargetStartElement.Size = New System.Drawing.Size(57, 20)
        Me.nudTargetStartElement.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(498, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Starting index"
        '
        'tbOutTargetBlock
        '
        Me.tbOutTargetBlock.Location = New System.Drawing.Point(6, 19)
        Me.tbOutTargetBlock.Name = "tbOutTargetBlock"
        Me.tbOutTargetBlock.Size = New System.Drawing.Size(155, 20)
        Me.tbOutTargetBlock.TabIndex = 0
        Me.tbOutTargetBlock.Text = "OutData"
        '
        'gbOutUdtName
        '
        Me.gbOutUdtName.Controls.Add(Me.tbOutUdtName)
        Me.gbOutUdtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbOutUdtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbOutUdtName.Location = New System.Drawing.Point(448, 3)
        Me.gbOutUdtName.MinimumSize = New System.Drawing.Size(0, 42)
        Me.gbOutUdtName.Name = "gbOutUdtName"
        Me.gbOutUdtName.Size = New System.Drawing.Size(439, 44)
        Me.gbOutUdtName.TabIndex = 2
        Me.gbOutUdtName.TabStop = False
        Me.gbOutUdtName.Text = "Output UDT Name"
        '
        'tbOutUdtName
        '
        Me.tbOutUdtName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbOutUdtName.Location = New System.Drawing.Point(3, 16)
        Me.tbOutUdtName.Name = "tbOutUdtName"
        Me.tbOutUdtName.Size = New System.Drawing.Size(433, 20)
        Me.tbOutUdtName.TabIndex = 1
        '
        'gbInUdt
        '
        Me.gbInUdt.Controls.Add(Me.dgvInUdt)
        Me.gbInUdt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbInUdt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbInUdt.Location = New System.Drawing.Point(3, 183)
        Me.gbInUdt.Name = "gbInUdt"
        Me.gbInUdt.Size = New System.Drawing.Size(439, 125)
        Me.gbInUdt.TabIndex = 6
        Me.gbInUdt.TabStop = False
        Me.gbInUdt.Text = "Input UDT"
        '
        'dgvInUdt
        '
        Me.dgvInUdt.AllowUserToAddRows = False
        Me.dgvInUdt.AllowUserToDeleteRows = False
        Me.dgvInUdt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvInUdt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvInUdt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvInUdt.Location = New System.Drawing.Point(3, 16)
        Me.dgvInUdt.Name = "dgvInUdt"
        Me.dgvInUdt.RowHeadersWidth = 5
        Me.dgvInUdt.Size = New System.Drawing.Size(433, 106)
        Me.dgvInUdt.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.dgvOutUdt)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(448, 183)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(439, 125)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Output UDT"
        '
        'dgvOutUdt
        '
        Me.dgvOutUdt.AllowUserToAddRows = False
        Me.dgvOutUdt.AllowUserToDeleteRows = False
        Me.dgvOutUdt.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvOutUdt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOutUdt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvOutUdt.Location = New System.Drawing.Point(3, 16)
        Me.dgvOutUdt.Name = "dgvOutUdt"
        Me.dgvOutUdt.RowHeadersWidth = 5
        Me.dgvOutUdt.Size = New System.Drawing.Size(433, 106)
        Me.dgvOutUdt.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.GroupBox1)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbInSourceData)
        Me.FlowLayoutPanel1.Controls.Add(Me.GroupBox2)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbDeviceName)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbSubRoutineName)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbGlobalUdtName)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 16)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(890, 334)
        Me.FlowLayoutPanel1.TabIndex = 8
        '
        'gbDeviceName
        '
        Me.gbDeviceName.Controls.Add(Me.tbUdtHeapItem)
        Me.gbDeviceName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbDeviceName.Location = New System.Drawing.Point(3, 162)
        Me.gbDeviceName.Name = "gbDeviceName"
        Me.gbDeviceName.Size = New System.Drawing.Size(643, 49)
        Me.gbDeviceName.TabIndex = 2
        Me.gbDeviceName.TabStop = False
        Me.gbDeviceName.Text = "Device Name (for UDT Heap Item)"
        '
        'tbUdtHeapItem
        '
        Me.tbUdtHeapItem.Location = New System.Drawing.Point(6, 19)
        Me.tbUdtHeapItem.Name = "tbUdtHeapItem"
        Me.tbUdtHeapItem.Size = New System.Drawing.Size(631, 20)
        Me.tbUdtHeapItem.TabIndex = 0
        '
        'gbSubRoutineName
        '
        Me.gbSubRoutineName.Controls.Add(Me.tbSubRoutineName)
        Me.gbSubRoutineName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSubRoutineName.Location = New System.Drawing.Point(3, 217)
        Me.gbSubRoutineName.Name = "gbSubRoutineName"
        Me.gbSubRoutineName.Size = New System.Drawing.Size(643, 48)
        Me.gbSubRoutineName.TabIndex = 3
        Me.gbSubRoutineName.TabStop = False
        Me.gbSubRoutineName.Text = "SubRoutine Name"
        '
        'tbSubRoutineName
        '
        Me.tbSubRoutineName.Location = New System.Drawing.Point(6, 19)
        Me.tbSubRoutineName.Name = "tbSubRoutineName"
        Me.tbSubRoutineName.Size = New System.Drawing.Size(631, 20)
        Me.tbSubRoutineName.TabIndex = 0
        '
        'gbGlobalUdtName
        '
        Me.gbGlobalUdtName.Controls.Add(Me.tbMainUdtName)
        Me.gbGlobalUdtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbGlobalUdtName.Location = New System.Drawing.Point(3, 271)
        Me.gbGlobalUdtName.Name = "gbGlobalUdtName"
        Me.gbGlobalUdtName.Size = New System.Drawing.Size(643, 48)
        Me.gbGlobalUdtName.TabIndex = 4
        Me.gbGlobalUdtName.TabStop = False
        Me.gbGlobalUdtName.Text = "Device Udt Name"
        '
        'tbMainUdtName
        '
        Me.tbMainUdtName.Location = New System.Drawing.Point(6, 19)
        Me.tbMainUdtName.Name = "tbMainUdtName"
        Me.tbMainUdtName.Size = New System.Drawing.Size(631, 20)
        Me.tbMainUdtName.TabIndex = 0
        '
        'gbSettings
        '
        Me.gbSettings.Controls.Add(Me.FlowLayoutPanel1)
        Me.gbSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSettings.Location = New System.Drawing.Point(0, 39)
        Me.gbSettings.Name = "gbSettings"
        Me.gbSettings.Size = New System.Drawing.Size(896, 353)
        Me.gbSettings.TabIndex = 9
        Me.gbSettings.TabStop = False
        Me.gbSettings.Text = "Settings (Edit to match the Input and Output data blocks used in the EIP scanner)" &
    ""
        '
        'gbAdvancedOptions
        '
        Me.gbAdvancedOptions.Controls.Add(Me.TableLayoutPanel1)
        Me.gbAdvancedOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAdvancedOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbAdvancedOptions.Location = New System.Drawing.Point(0, 402)
        Me.gbAdvancedOptions.Name = "gbAdvancedOptions"
        Me.gbAdvancedOptions.Size = New System.Drawing.Size(896, 330)
        Me.gbAdvancedOptions.TabIndex = 10
        Me.gbAdvancedOptions.TabStop = False
        Me.gbAdvancedOptions.Text = "Advanced Options ( Don't change anything unless you know you need to )"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox3, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.gbInUdt, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.gbOutPD, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.gbInPD, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.gbOutUdtName, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.gbInUdtName, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(890, 311)
        Me.TableLayoutPanel1.TabIndex = 11
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 392)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(896, 10)
        Me.Splitter1.TabIndex = 11
        Me.Splitter1.TabStop = False
        '
        'frmBrxExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(896, 732)
        Me.Controls.Add(Me.gbAdvancedOptions)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.gbSettings)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmBrxExport"
        Me.Text = "frmBrxExport"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.gbInPD.ResumeLayout(False)
        CType(Me.dgvIn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbOutPD.ResumeLayout(False)
        CType(Me.dgvOut, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbInSourceData.ResumeLayout(False)
        Me.gbInSourceData.PerformLayout()
        CType(Me.nudSourceStartElement, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbInUdtName.ResumeLayout(False)
        Me.gbInUdtName.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.nudTargetStartElement, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbOutUdtName.ResumeLayout(False)
        Me.gbOutUdtName.PerformLayout()
        Me.gbInUdt.ResumeLayout(False)
        CType(Me.dgvInUdt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.dgvOutUdt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.gbDeviceName.ResumeLayout(False)
        Me.gbDeviceName.PerformLayout()
        Me.gbSubRoutineName.ResumeLayout(False)
        Me.gbSubRoutineName.PerformLayout()
        Me.gbGlobalUdtName.ResumeLayout(False)
        Me.gbGlobalUdtName.PerformLayout()
        Me.gbSettings.ResumeLayout(False)
        Me.gbAdvancedOptions.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cbSelectedOption As ComboBox
    Friend WithEvents gbInPD As GroupBox
    Friend WithEvents gbOutPD As GroupBox
    Friend WithEvents gbInUdtName As GroupBox
    Friend WithEvents tbInUdtName As TextBox
    Friend WithEvents gbInSourceData As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents nudSourceStartElement As NumericUpDown
    Friend WithEvents tbInSourceBlock As TextBox
    Friend WithEvents ExportBRXToolStripMenuItem As ToolStripButton
    Friend WithEvents dgvIn As DataGridView
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
    Friend WithEvents Column6 As DataGridViewTextBoxColumn
    Friend WithEvents dgvOut As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents gbOutUdtName As GroupBox
    Friend WithEvents tbOutUdtName As TextBox
    Friend WithEvents tbOutTargetBlock As TextBox
    Friend WithEvents gbInUdt As GroupBox
    Friend WithEvents dgvInUdt As DataGridView
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents dgvOutUdt As DataGridView
    Friend WithEvents nudTargetStartElement As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents gbSubRoutineName As GroupBox
    Friend WithEvents tbSubRoutineName As TextBox
    Friend WithEvents gbDeviceName As GroupBox
    Friend WithEvents tbUdtHeapItem As TextBox
    Friend WithEvents gbGlobalUdtName As GroupBox
    Friend WithEvents tbMainUdtName As TextBox
    Friend WithEvents gbSettings As GroupBox
    Friend WithEvents gbAdvancedOptions As GroupBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
End Class
