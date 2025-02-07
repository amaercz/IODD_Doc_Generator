Imports System.Security.Cryptography.X509Certificates
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frmBrxExport
    Public ds As New DataSet
    Public dvIn, dvOut As DataView

    Public inOffset As Integer
    Public outOffset As Integer
    Public port As String
    Public inUdtName As String
    Public outUdtName As String
    Public mainUdtName As String
    Public subRoutineName As String




    Private Sub frmBrxExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        nudSourceStartElement.Value = inOffset
        nudTargetStartElement.Value = outOffset

        tbInUdtName.Text = inUdtName
        tbOutUdtName.Text = outUdtName
        tbSubRoutineName.Text = subRoutineName
        tbUdtHeapItem.Text = "IOL_" & port
        tbMainUdtName.Text = mainUdtName
        If Not IsNothing(ds) AndAlso ds.Tables.Contains("processData") Then
            If Not ds.Tables("processData").Columns.Contains("udtFieldName") Then
                ds.Tables("processData").Columns.Add("udtFieldName", GetType(String))
                For Each dr As DataRow In ds.Tables("processData").Rows
                    Dim fn As String = dr.Item("itemName")
                    Dim re As New Regex("\W")
                    fn = re.Replace(fn, "")
                    If fn.Length > 16 Then
                        re = New Regex("[^a-zA-z0-9\s]")
                        fn = re.Replace(dr.Item("itemName").trim, "")
                        Dim words() = fn.Split(" ")
                        fn = ""
                        Dim cntPerWord As Integer = (16 - (16 Mod words.Count)) / words.Count
                        For Each word As String In words
                            If word.Length > 1 And cntPerWord > 1 Then
                                fn = fn & word.Substring(0, 1).ToUpper & word.Substring(1, Math.Min(cntPerWord - 1, word.Length - 1))
                            Else
                                fn = fn & word.Substring(0, 1).ToUpper
                            End If
                        Next
                    End If
                    dr.Item("udtFieldName") = fn.Substring(0, Math.Min(fn.Length, 16))
                Next
            End If
            For Each dgv As DataGridView In {dgvIn, dgvOut}
                dgv.DataSource = Nothing
                dgv.Columns.Clear()
                dgv.AutoGenerateColumns = False
                Dim col As DataGridViewTextBoxColumn
                col = New DataGridViewTextBoxColumn : col.HeaderText = "udtFieldName" : col.DataPropertyName = "udtFieldName" : dgv.Columns.Add(col)
                col = New DataGridViewTextBoxColumn : col.HeaderText = "itemName" : col.DataPropertyName = "itemName" : dgv.Columns.Add(col)
                col = New DataGridViewTextBoxColumn : col.HeaderText = "itemType" : col.DataPropertyName = "itemType" : dgv.Columns.Add(col)
                col = New DataGridViewTextBoxColumn : col.HeaderText = "itemBitOffset" : col.DataPropertyName = "itemBitOffset" : dgv.Columns.Add(col)
                col = New DataGridViewTextBoxColumn : col.HeaderText = "itemBitLength" : col.DataPropertyName = "itemBitLength" : dgv.Columns.Add(col)
            Next
            dvIn = New DataView(ds.Tables("processData"), "pdDir = 'In'", "pdID ASC, pdDir ASC, PILsbByte ASC, PILsbBit ASC", DataViewRowState.CurrentRows)
            dvOut = New DataView(ds.Tables("processData"), "pdDir = 'Out'", "pdID ASC, pdDir ASC, PILsbByte ASC, PILsbBit ASC", DataViewRowState.CurrentRows)
            dgvIn.DataSource = dvIn
            dgvOut.DataSource = dvOut

            ds.Tables.Add(ds.Tables("processData").DefaultView.ToTable("conditions", True, "conditionValName"))
            cbSelectedOption.DataSource = ds.Tables("conditions")
            cbSelectedOption.ValueMember = "conditionValName"
            cbSelectedOption.DisplayMember = "conditionValName"
            calculateUdts()
        End If

    End Sub


    Private Sub ExportBRXToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportBRXToolStripMenuItem.Click

        calculateUdts()

        Dim lstRungCommands As New List(Of String)
        Dim lstUDTconfig As New List(Of String)

        lstRungCommands.Add("$LGCMOD " & tbSubRoutineName.Text.Trim)

#Region "IN"
        If ds.Tables("inUdt").Rows.Count > 0 Then
            'splitting UDT config an rung commands as things were growing to be unwieldy
            ''UDT config
            lstUDTconfig.Add("#BEGIN UDT_CONFIG")
            lstUDTconfig.Add(tbInUdtName.Text.Trim)
            For Each rw As DataRow In ds.Tables("inUdt").Rows
                lstUDTconfig.Add(String.Join(",", {rw.Item("fieldName"), rw.Item("fieldType"), rw.Item("fieldStartDword") & ":0", "Read-Write, Native, Short"}))
            Next
            lstUDTconfig.Add("#END")

            ''Rungs



            For Each rw As DataRow In ds.Tables("inUdt").Rows

                lstRungCommands.Add("#BEGIN FMT_COMMENT")
                lstRungCommands.Add(String.Format("""{2}{3}:{5} -> {0}, Type: {1}, SourceLength: {4}bit""", {rw.Item("fieldName"), rw.Item("fieldType"), tbInSourceBlock.Text, nudSourceStartElement.Value + rw.Item("sourceByteOffset"), rw.Item("sourceBitLength"), rw.Item("sourceBitShift")}))
                lstRungCommands.Add("#End")

                If rw.Item("fieldType") = "BIT" Then
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add(String.Join(" ", {"MATH", tbUdtHeapItem.Text.Trim & ".in." & rw.Item("fieldName"), """" & tbInSourceBlock.Text & (nudSourceStartElement.Value + rw.Item("sourceByteOffset")) & ":" & rw.Item("sourceBitShift") & """"}))
                    lstRungCommands.Add("")
                Else
                    Dim lstBuildCopyCommand As New List(Of String)
                    lstBuildCopyCommand.Add("COPY 0x1 ""4")
                    For idx As Integer = 0 To 3
                        If idx < rw.Item("sourceByteLength") Then
                            lstBuildCopyCommand.Add(String.Format("0x0 {0}{1} IOLRawBuffer0:B{2} 1", {tbInSourceBlock.Text, nudSourceStartElement.Value + rw.Item("sourceByteOffset") + rw.Item("sourceByteLength") - 1 - idx, idx}))
                        Else
                            lstBuildCopyCommand.Add(String.Format("0x0 {0}{1} IOLRawBuffer0:B{2} 1", {"0", "", idx}))
                        End If
                    Next
                    lstBuildCopyCommand.Add("""")
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add(String.Join(" ", lstBuildCopyCommand.ToArray))
                    lstRungCommands.Add(getBRXMathcmdDirect("IOLRawBuffer0", tbUdtHeapItem.Text.Trim & ".in." & rw.Item("fieldName"), rw.Item("fieldType"), rw.Item("sourceBitShift"), rw.Item("sourceBitLength")))
                    lstRungCommands.Add("")
                End If
            Next


        End If

#End Region
#Region "Out"
        If ds.Tables("outUdt").Rows.Count > 0 Then
            'splitting UDT config an rung commands as things were growing to be unwieldy
            ''UDT config
            lstUDTconfig.Add("#BEGIN UDT_CONFIG")
            lstUDTconfig.Add(tbOutUdtName.Text.Trim)
            For Each rw As DataRow In ds.Tables("outUdt").Rows
                lstUDTconfig.Add(String.Join(",", {rw.Item("fieldName"), rw.Item("fieldType"), rw.Item("fieldStartDword") & ":0", "Read-Write, Native, Short"}))
            Next
            lstUDTconfig.Add("#END")

            ''Rungs

            lstRungCommands.Add("#BEGIN FMT_COMMENT")
            lstRungCommands.Add("""Clear UDTBuffer""")
            lstRungCommands.Add("#End")
            lstRungCommands.Add("STR ST1")
            lstRungCommands.Add("MEMCLEAR IOLUDTOutBuffer0 32")
            lstRungCommands.Add("")

            For Each rw As DataRow In ds.Tables("outUdt").Rows

                lstRungCommands.Add("#BEGIN FMT_COMMENT")
                lstRungCommands.Add(String.Format("""{0} -> {2}{3}:{5}, Type: {1}, TargetLength: {4}bit""", {rw.Item("fieldName"), rw.Item("fieldType"), tbOutTargetBlock.Text, nudTargetStartElement.Value + rw.Item("sourceByteOffset"), rw.Item("sourceBitLength"), rw.Item("sourceBitShift")}))
                lstRungCommands.Add("#End")

                If rw.Item("fieldType") = "BIT" Then
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add(String.Join(" ", {"MATH", "IOLUDTOutBuffer" & (rw.Item("sourceByteOffset")) & ":" & rw.Item("sourceBitShift"), """" & tbUdtHeapItem.Text.Trim & ".out." & rw.Item("fieldName") & """"}))
                    lstRungCommands.Add("")
                Else
                    Dim mask As Int32 = 2 ^ rw.Item("sourceBitLength") - 1
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add("MEMCLEAR IOLRawBuffer0 1")
                    lstRungCommands.Add("MATH IOLRawBuffer0 ""(" & tbUdtHeapItem.Text.Trim & ".out." & rw.Item("fieldName") & " & 0x" & mask.ToString("X") & ") << " & rw.Item("sourceBitShift") & """")

                    For i As Integer = 0 To rw.Item("sourceByteLength") - 1
                        Dim targetByte As String = "IOLUDTOutBuffer" & rw.Item("sourceByteOffset") + rw.Item("sourceByteLength") - 1 - i

                        lstRungCommands.Add("MATH " & targetByte & " """ & targetByte & " | IOLRawBuffer0:B" & i & """") '" & rw.Item("sourceByteOffset") + rw.Item("sourceByteLength") - 1 - i  & " "(" & tbOutUdtInstanceName.Text.Trim & "." & rw.Item("fieldName") & mask.ToString("X") & ") << " & rw.Item("sourceBitShift") & ")""")

                    Next
                    lstRungCommands.Add("")
                End If
            Next

            lstRungCommands.Add("#BEGIN FMT_COMMENT")
            lstRungCommands.Add("""Copy UDTBuffer to Output""")
            lstRungCommands.Add("#End")

            Dim outByteCount As Integer = dvOut.ToTable.Rows(0).Item("pdByteCount")
            lstRungCommands.Add("STR ST1")
            lstRungCommands.Add("MEMCOPY IOLUDTOutBuffer0 " & tbOutTargetBlock.Text.Trim & nudTargetStartElement.Value & " 0x0 " & outByteCount)

        End If
#End Region

        lstRungCommands.Add("RET")
        lstRungCommands.Add("$LGCEND " & tbSubRoutineName.Text.Trim)

        Dim udtOutStart As Integer = ds.Tables("inUdt").Rows.Count + 1

        lstUDTconfig.Add("#BEGIN UDT_CONFIG")
        lstUDTconfig.Add(tbMainUdtName.Text.Trim)
        If ds.Tables("inUdt").Rows.Count > 0 Then lstUDTconfig.Add(String.Join(",", {"in", tbInUdtName.Text.Trim, "0" & ":0", "Read-Write, Native, Short"}))
        If ds.Tables("outUdt").Rows.Count > 0 Then lstUDTconfig.Add(String.Join(",", {"out", tbOutUdtName.Text.Trim, udtOutStart & ":0", "Read-Write, Native, Short"}))
        lstUDTconfig.Add("#END")



        Dim lstMemConfg As New List(Of String)
        lstMemConfg.Add("#BEGIN MEM_CONFIG")

        lstMemConfg.Add(String.Join(" ", {tbUdtHeapItem.Text.Trim, tbMainUdtName.Text, "0"}))
        lstMemConfg.Add(String.Join(" ", {"IOLRawBuffer", "SDWORD", "1 -1"}))
        lstMemConfg.Add(String.Join(" ", {"IOLUDTOutBuffer", "UBYTE", "32 -1"}))
        lstMemConfg.Add(String.Join(" ", {tbSubRoutineName.Text.Trim, "<Virtual>", "0"}))

        lstMemConfg.Add("#END")

        Dim sfd As New SaveFileDialog
        If sfd.ShowDialog = DialogResult.OK Then
            Dim lstOut As New List(Of String)
            lstOut.AddRange(lstUDTconfig)
            lstOut.AddRange(lstMemConfg)
            lstOut.AddRange(lstRungCommands)
            System.IO.File.WriteAllLines(sfd.FileName, lstOut.ToArray)
        End If
    End Sub

    Private Function getBRXMathcmd(srcBlock As String, trgtBlock As String, byteOffset As Integer, tp As String, bo As String, Optional bl As String = "") As String
        Dim ret As String = ""
        Select Case tp
            Case "BIT"
                ret = String.Join(" ", {"MATH", trgtBlock & byteOffset & ":SD", """" & srcBlock & ":" & bo & """"})
            Case "SWORD", "SBYTE", "SDWORD"
                Dim maskVal As UInt32 = (2 ^ (CInt(bl) - 1) - 1)
                Dim maskSign As UInt32 = (2 ^ (CInt(bl) - 1))
                Dim signFill As UInt32 = ((2 ^ 32) - 1) Xor maskVal

                Dim expr As String = String.Format("(({0} >> {1}) & 0x{2}) | (0x{4} * (({0} & 0x{5}) >> {3}))", {srcBlock, CInt(bo), maskVal.ToString("X"), (CInt(bo) + CInt(bl) - 1), signFill.ToString("X"), maskSign.ToString("X")}) '   srcBlock & ":W >> " & CInt(bo) 
                ret = String.Join(" ", {"MATH", trgtBlock & byteOffset & ":SD", """" & expr & """"})
            Case "UWORD", "UBYTE"
                Dim maskVal As UInt16 = (2 ^ (CInt(bl)) - 1)
                Dim expr As String = String.Format("(({0} >> {1}) & 0x{2})", {srcBlock, CInt(bo), maskVal.ToString("X")}) '   srcBlock & ":W >> " & CInt(bo) 
                ret = String.Join(" ", {"MATH", trgtBlock & byteOffset & ":SD", """" & expr & """"})

            Case Else
                ret = ""
        End Select
        Return ret
    End Function

    Private Function getBRXMathcmdDirect(srcBlock As String, trgtBlock As String, tp As String, bo As String, Optional bl As String = "") As String
        Dim ret As String = ""
        Select Case tp
            Case "BIT"
                ret = String.Join(" ", {"MATH", trgtBlock, """" & srcBlock & ":" & bo & """"})
            Case "SWORD", "SBYTE", "SDWORD"
                Dim maskVal As UInt32 = (2 ^ (CInt(bl) - 1) - 1)
                Dim maskSign As UInt32 = (2 ^ (CInt(bl) - 1))
                Dim signFill As UInt32 = ((2 ^ 32) - 1) Xor maskVal

                Dim expr As String = String.Format("(({0} >> {1}) & 0x{2}) | (0x{4} * (({0} & 0x{5}) >> {3}))", {srcBlock, CInt(bo), maskVal.ToString("X"), (CInt(bo) + CInt(bl) - 1), signFill.ToString("X"), maskSign.ToString("X")}) '   srcBlock & ":W >> " & CInt(bo) 
                ret = String.Join(" ", {"MATH", trgtBlock, """" & expr & """"})
            Case "UWORD", "UBYTE"
                Dim maskVal As UInt16 = (2 ^ (CInt(bl)) - 1)
                Dim expr As String = String.Format("(({0} >> {1}) & 0x{2})", {srcBlock, CInt(bo), maskVal.ToString("X")}) '   srcBlock & ":W >> " & CInt(bo) 
                ret = String.Join(" ", {"MATH", trgtBlock, """" & expr & """"})
            Case "REAL"
                ret = String.Join(" ", {"MATH", trgtBlock, """" & srcBlock & """"})
            Case Else
                ret = ""
        End Select
        Return ret
    End Function

    Private Sub calculateUdts()
        dgvInUdt.DataSource = Nothing
        If ds.Tables.Contains("inUdt") Then ds.Tables.Remove("inUdt")
        If ds.Tables.Contains("outUdt") Then ds.Tables.Remove("outUdt")

        With ds.Tables.Add("inUdt")
            .Columns.Add("fieldName", GetType(String))
            .Columns.Add("fieldType", GetType(String))
            .Columns.Add("fieldStartByte", GetType(Integer))
            .Columns.Add("fieldStartDword", GetType(Integer))
            .Columns.Add("sourceBlock", GetType(String))
            .Columns.Add("sourceByteOffset", GetType(Integer))
            .Columns.Add("sourceByteLength", GetType(Integer))
            .Columns.Add("sourceBitShift", GetType(Integer))
            .Columns.Add("sourceType", GetType(String))
            .Columns.Add("sourceBitLength", GetType(Integer))
        End With


        Dim posCnt As Integer
        posCnt = 0
        For Each rw As DataRow In dvIn.ToTable.Rows
            Dim udtc As New UdtConfig
            udtc.fieldName = rw.Item("udtFieldName").trim
            udtc.fieldType = getBrxType(rw.Item("itemType").trim, rw.Item("itemBitLengthNum"))
            udtc.fieldStartByte = posCnt * 4
            udtc.fieldStartDword = posCnt
            udtc.sourceBlock = tbInSourceBlock.Text

            Dim bit, bt As Integer
            bit = rw.Item("itemBitOffset") Mod 8
            If rw.Item("itemBitLength") <> "" Then
                bt = rw.Item("pdByteCount") - ((rw.Item("itemBitOffset") - bit + CInt(rw.Item("itemBitLengthNum")) - 1) / 8)
            Else
                bt = rw.Item("pdByteCount") - ((rw.Item("itemBitOffset") - bit) / 8)
            End If
            udtc.sourceByteOffset = rw.Item("PIMsbByte") - 1
            udtc.sourceByteLength = (rw.Item("itemBitLengthNum") - (rw.Item("itemBitLengthNum") Mod 8)) / 8
            If rw.Item("itemBitLengthNum") Mod 8 > 0 Then udtc.sourceByteLength += 1
            udtc.sourceBitShift = ((rw.Item("itemBitOffset")) Mod 8)
            udtc.sourceType = rw.Item("itemType")
            udtc.sourceBitLength = rw.Item("itemBitLengthNum")

            posCnt += 1
            ds.Tables("inUdt").Rows.Add(udtc.getValues)
        Next

        dgvInUdt.DataSource = ds.Tables("inUdt")


        With ds.Tables.Add("outUdt")
            .Columns.Add("fieldName", GetType(String))
            .Columns.Add("fieldType", GetType(String))
            .Columns.Add("fieldStartByte", GetType(Integer))
            .Columns.Add("fieldStartDword", GetType(Integer))
            .Columns.Add("sourceBlock", GetType(String))
            .Columns.Add("sourceByteOffset", GetType(Integer))
            .Columns.Add("sourceByteLength", GetType(Integer))
            .Columns.Add("sourceBitShift", GetType(Integer))
            .Columns.Add("sourceType", GetType(String))
            .Columns.Add("sourceBitLength", GetType(Integer))
        End With


        posCnt = 0
        For Each rw As DataRow In dvOut.ToTable.Rows
            Dim udtc As New UdtConfig
            udtc.fieldName = rw.Item("udtFieldName").trim
            udtc.fieldType = getBrxType(rw.Item("itemType").trim, rw.Item("itemBitLengthNum"))
            udtc.fieldStartByte = posCnt * 4
            udtc.fieldStartDword = posCnt
            udtc.sourceBlock = tbInSourceBlock.Text

            Dim bit, bt As Integer
            bit = rw.Item("itemBitOffset") Mod 8
            If rw.Item("itemBitLength") <> "" Then
                bt = rw.Item("pdByteCount") - ((rw.Item("itemBitOffset") - bit + CInt(rw.Item("itemBitLengthNum")) - 1) / 8)
            Else
                bt = rw.Item("pdByteCount") - ((rw.Item("itemBitOffset") - bit) / 8)
            End If
            udtc.sourceByteOffset = rw.Item("PIMsbByte") - 1
            udtc.sourceByteLength = (rw.Item("itemBitLengthNum") - (rw.Item("itemBitLengthNum") Mod 8)) / 8
            If rw.Item("itemBitLengthNum") Mod 8 > 0 Then udtc.sourceByteLength += 1
            udtc.sourceBitShift = ((rw.Item("itemBitOffset")) Mod 8)
            udtc.sourceType = rw.Item("itemType")
            udtc.sourceBitLength = rw.Item("itemBitLengthNum")

            posCnt += 1
            ds.Tables("outUdt").Rows.Add(udtc.getValues)
        Next
        dgvOutUdt.DataSource = ds.Tables("outUdt")
    End Sub

    Structure UdtConfig
        Public fieldName As String
        Public fieldType As String
        Public fieldStartByte As Integer
        Public fieldStartDword As Integer
        Public sourceBlock As String
        Public sourceByteOffset As Integer
        Public sourceByteLength As Integer
        Public sourceBitShift As Integer
        Public sourceType As String
        Public sourceBitLength As Integer

        Public Function getValues() As String()
            Return {fieldName, fieldType, fieldStartByte, fieldStartDword, sourceBlock, sourceByteOffset, sourceByteLength, sourceBitShift, sourceType, sourceBitLength}
        End Function
    End Structure


    Private Function getBrxType(str As String, bitlength As Integer) As String
        Select Case str
            Case "BooleanT"
                Return "BIT"
            Case "IntegerT"
                If bitlength > 16 Then
                    Return "SDWORD"
                ElseIf bitlength > 8 Then
                    Return "SWORD"
                Else
                    Return "SBYTE"
                End If
            Case "UIntegerT"
                If bitlength > 16 Then
                    Return "SDWORD"
                ElseIf bitlength > 8 Then
                    Return "UWORD"
                Else
                    Return "UBYTE"
                End If
            Case "Float32T"
                Return "REAL"
            Case Else
                Return ""
        End Select
    End Function


    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbSelectedOption.SelectedValueChanged
        '   updateProcessData()

        If IsNothing(cbSelectedOption.SelectedValue) OrElse String.IsNullOrWhiteSpace(cbSelectedOption.SelectedValue.ToString) Then
            dvIn.RowFilter = "pdDir = 'In'"
            dvOut.RowFilter = "pdDir = 'Out'"
        Else
            dvIn.RowFilter = "pdDir = 'In' AND " & "conditionValName = '" & cbSelectedOption.SelectedValue.ToString & "'"
            dvOut.RowFilter = "pdDir = 'Out' AND " & "conditionValName = '" & cbSelectedOption.SelectedValue.ToString & "'"
        End If
        calculateUdts()
    End Sub



    Sub updateProcessData()
        dgvIn.Rows.Clear()
        dgvOut.Rows.Clear()
        If ds.Tables("processData").Rows.Count <= 0 Then Return
        Dim drs() As DataRow
        If IsNothing(cbSelectedOption.SelectedValue) Then
            drs = ds.Tables("processData").Select("", "pdID ASC, pdDir ASC, PILsbByte ASC, PILsbBit ASC")
        Else
            drs = ds.Tables("processData").Select("conditionValName = '" & cbSelectedOption.SelectedValue.ToString & "'", "pdID ASC, pdDir ASC, PILsbByte ASC, PILsbBit ASC")
        End If
        For Each rw As DataRow In drs
            If rw.Item("pdDir") = "In" Then
                dgvIn.Rows.Add({rw.Item("itemName").replace(" ", ""), rw.Item("itemType"), rw.Item("itemBitOffset"), rw.Item("itemBitLength"), rw.Item("itemMin"), rw.Item("itemMax")})
            ElseIf rw.Item("pdDir") = "Out" Then
                dgvOut.Rows.Add({rw.Item("itemName").replace(" ", ""), rw.Item("itemType"), rw.Item("itemBitOffset"), rw.Item("itemBitLength"), rw.Item("itemMin"), rw.Item("itemMax")})
            End If
        Next
    End Sub
End Class