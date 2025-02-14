Imports System.Text.RegularExpressions
Imports IolMasterDefinitions

Partial Class Form1

    Public dvIn, dvOut As DataView

    Public inOffset As Integer
    Public outOffset As Integer

    Public port As String = ""
    Public inUdtName As String
    Public outUdtName As String
    Public heapItemName As String
    Public mainUdtName As String
    Public subRoutineName As String
    Public inBlockName As String = "inData"
    Public outBlockName As String = "outData"
    Public vendorId As String
    Public deviceId As String
    Public vendorName As String
    Public deviceName As String

    Public inDataBufferName As String
    Public outDataBufferName As String
    Public udtBufferHeapItemName As String

    Public conditionVal As String = ""
    Public conditionDesc As String = ""

    Private deviceNameClean As String
    Private vendorNameClean As String
    Public Sub initBrxExportTab()

        vendorId = ds.Tables("deviceInfo").Rows(0).Item("vendorId")
        deviceId = ds.Tables("deviceInfo").Rows(0).Item("deviceId")
        vendorName = ds.Tables("deviceInfo").Rows(0).Item("vendorName")
        deviceName = ds.Tables("deviceInfo").Rows(0).Item("deviceName")


        Dim reDN As New Regex("[^A-Za-z0-9]")
        deviceNameClean = reDN.Replace(deviceName, "")
        vendorNameClean = reDN.Replace(vendorName, "").ToLower

        Dim devNameLenUse As Integer = Math.Min(13, deviceNameClean.Length)
        Dim venNameAvail As Integer = 16 - 2 - devNameLenUse
        Dim vendorNameLenUse As Integer = Math.Min(venNameAvail, vendorNameClean.Length)
        '  subRoutineName = "m_" & "_" & vendorNameClean.Substring(0, vendorNameLenUse) & deviceNameClean.Substring(0, devNameLenUse)
        subRoutineName = "m_" & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")

        venNameAvail = 16 - 2 - devNameLenUse
        vendorNameLenUse = Math.Min(venNameAvail, vendorNameClean.Length)
        ' mainUdtName = "t_" & vendorNameClean.Substring(0, vendorNameLenUse) & deviceNameClean.Substring(0, devNameLenUse)
        mainUdtName = "t_" & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")

        inUdtName = "tI" & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")
        outUdtName = "tO" & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")

        If Not IsNothing(tscbIolMasterPorts.SelectedItem) Then
            inOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).inByteStart
            outOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).outByteStart
            port = CType(tscbIolMasterPorts.SelectedItem, portInfo).name
        Else
            inOffset = 0
            outOffset = 0
            port = ""
        End If

        nudSourceStartElement.Value = inOffset
        nudTargetStartElement.Value = outOffset

        tbInUdtName.Text = inUdtName
        tbOutUdtName.Text = outUdtName
        tbSubRoutineName.Text = subRoutineName
        heapItemName = "IOL_" & port
        tbUdtHeapItem.Text = heapItemName
        tbMainUdtName.Text = mainUdtName

        inDataBufferName = "IOLBufferDataIn"
        outDataBufferName = "IOLBufferDataOut"

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

            ds.Tables.Add(ds.Tables("processData").DefaultView.ToTable("conditions", True, {"conditionValName", "conditionValue"}))
            cbSelectedOption.DataSource = ds.Tables("conditions")
            cbSelectedOption.ValueMember = "conditionValName"
            cbSelectedOption.DisplayMember = "conditionValName"
            calculateUdts()
            updateTypeAndSubName()
        End If



        tabCtrlBrxExport.Enabled = True
        msBrxExport.Enabled = True

    End Sub
    'unnessecary because the entire IODD file gets reloaded when the port changes -> full re-init
    'Public Sub updateSelectedPortforBrxExport()
    '    If Not IsNothing(tscbIolMasterPorts.SelectedItem) Then
    '        inOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).inByteStart
    '        outOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).outByteStart
    '        port = CType(tscbIolMasterPorts.SelectedItem, portInfo).name
    '    Else
    '        inOffset = 0
    '        outOffset = 0
    '        port = ""
    '    End If

    '    nudSourceStartElement.Value = inOffset
    '    nudTargetStartElement.Value = outOffset
    'End Sub


    Private Sub updateTypeAndSubName()


        Dim devNameLenUse As Integer
        Dim venNameAvail As Integer
        Dim vendorNameLenUse As Integer
        Dim condLen As Integer = conditionVal.Length
        Dim availCondChars As Integer = 16 - 2 - ("v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")).Length
        'If rbExplicitMode.Checked Then
        '    devNameLenUse = Math.Min(13, deviceNameClean.Length)
        '    venNameAvail = 16 - 2 - devNameLenUse - port.Length
        '    vendorNameLenUse = Math.Min(venNameAvail, vendorNameClean.Length)
        '    subRoutineName = "m_" & port.ToUpper & vendorNameClean.Substring(0, vendorNameLenUse) & deviceNameClean.Substring(0, devNameLenUse)
        'Else
        devNameLenUse = Math.Min(13 - condLen, deviceNameClean.Length)
            venNameAvail = 16 - 2 - devNameLenUse - condLen
            vendorNameLenUse = Math.Min(venNameAvail, vendorNameClean.Length)
        subRoutineName = "m_" & conditionVal.Substring(0, Math.Min(availCondChars, conditionVal.Length)) & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")

        udtBufferHeapItemName = "b_" & conditionVal.Substring(0, Math.Min(availCondChars, conditionVal.Length)) & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")




        devNameLenUse = Math.Min(13 - condLen, deviceNameClean.Length)
        venNameAvail = 16 - 2 - devNameLenUse - condLen
        vendorNameLenUse = Math.Min(venNameAvail, vendorNameClean.Length)
        mainUdtName = "t_" & conditionVal.Substring(0, Math.Min(availCondChars, conditionVal.Length)) & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")



        inUdtName = "tI" & conditionVal.Substring(0, Math.Min(availCondChars, conditionVal.Length)) & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")
        outUdtName = "tO" & conditionVal.Substring(0, Math.Min(availCondChars, conditionVal.Length)) & "v" & CInt(vendorId).ToString("X") & "d" & CInt(deviceId).ToString("X")

        tbSubRoutineName.Text = subRoutineName
        tbMainUdtName.Text = mainUdtName

        tbInUdtName.Text = inUdtName
        tbOutUdtName.Text = outUdtName
    End Sub

    Private Sub ExportBRXToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportBRXToolStripMenuItem.Click

        calculateUdts()


        Dim mainCommentString As String = ""
        Dim mainCommentLines As New List(Of String)
        With mainCommentLines
            .Add("This subroutine handles the input and output data from the device")
            .Add("Manufacturer: " & vendorName)
            .Add("Model: " & deviceName)
            .Add("IO-Link vendor ID: " & vendorId)
            .Add("IO-Link device ID: " & deviceId)


            .Add("The input data is gathered starting at memory location " & tbInSourceBlock.Text & nudSourceStartElement.Value)
                .Add("The output data is written starting at memory location " & tbOutTargetBlock.Text & nudTargetStartElement.Value)

            .Add("This subroutine uses data that is copied into buffers upon calling the subroutine")
                .Add("Proper use requires you to create a heapitem of the udt named " & tbMainUdtName.Text & " that is used to read inputs and/or control outputs")
                .Add("when calling the subroutine, enable optional input parameters and copy the following:")
                .Add("[StartElementOfIOLMasterPortINData] to " & tbInSourceBlock.Text & "0 with 32 elements length")
                .Add("[StartElementOfIOLMasterPortOUTData] to " & tbOutTargetBlock.Text & "0 with 32 elements length")
                .Add("[UdtHeapItemInstance created in step 1] to " & tbUdtHeapItem.Text & " with 1 element length")
                .Add("also enable optional output parameters and copy the following:")
                .Add(tbOutTargetBlock.Text & "to [StartElementOfIOLMasterPortOUTData] with 32 elements length")
                .Add("[StartElementOfIOLMasterPortOUTData] to " & tbOutTargetBlock.Text & "0 with 32 elements length")
                .Add(tbUdtHeapItem.Text & " to [UdtHeapItemInstance created in step 1] with 1 element length")

        End With
        For Each str As String In mainCommentLines
            mainCommentString &= """<FONT tsize=2>" & str & "</FONT><br>""" & vbCrLf
        Next

        Dim lstElmDoc As New List(Of String)
        lstElmDoc.Add("#BEGIN ELEMENT_DOC")

        Dim lstRungCommands As New List(Of String)
        Dim lstUDTconfig As New List(Of String)

        lstRungCommands.Add("$LGCMOD " & tbSubRoutineName.Text.Trim)

        For Each cmt As String In {"Vendor: <b>" & vendorName & "</b>", "Device: " & deviceName, "VendorId: " & vendorId, "DeviceId: " & deviceId}
            lstRungCommands.Add("#BEGIN FMT_COMMENT")
            lstRungCommands.Add("""<FONT tsize=2>" & cmt & "</FONT>""")
            lstRungCommands.Add("#End")
            lstRungCommands.Add("NOP")
            lstRungCommands.Add("")
        Next

        If Not String.IsNullOrWhiteSpace(conditionVal) Then
            lstRungCommands.Add("#BEGIN FMT_COMMENT")
            lstRungCommands.Add("""<FONT tsize=2>" & "PD layout: " & conditionDesc & " (" & conditionVal & ")</FONT>""")
            lstRungCommands.Add("#End")
            lstRungCommands.Add("NOP")
            lstRungCommands.Add("")
        End If


        lstRungCommands.Add("#BEGIN FMT_COMMENT")
        lstRungCommands.Add(mainCommentString)
        lstRungCommands.Add("#End")
        lstRungCommands.Add("NOP")
        lstRungCommands.Add("")

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
                lstRungCommands.Add(String.Format("""{2}{3}:{5} -> {0}, Type: {1}, SourceLength: {4}bit""", {rw.Item("fieldName"), rw.Item("fieldType"), inDataBufferName, rw.Item("sourceByteOffset"), rw.Item("sourceBitLength"), rw.Item("sourceBitShift")}))
                lstRungCommands.Add("#End")

                If rw.Item("fieldType") = "BIT" Then
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add(String.Join(" ", {"MATH", udtBufferHeapItemName & ".in." & rw.Item("fieldName"), """" & inDataBufferName & (rw.Item("sourceByteOffset")) & ":" & rw.Item("sourceBitShift") & """"}))
                    lstRungCommands.Add("")
                Else
                    Dim lstBuildCopyCommand As New List(Of String)
                    lstBuildCopyCommand.Add("COPY 0x1 ""4")
                    For idx As Integer = 0 To 3
                        If idx < rw.Item("sourceByteLength") Then
                            lstBuildCopyCommand.Add(String.Format("0x0 {0}{1} IOLRawBuffer0:B{2} 1", {inDataBufferName, rw.Item("sourceByteOffset") + rw.Item("sourceByteLength") - 1 - idx, idx}))
                        Else
                            lstBuildCopyCommand.Add(String.Format("0x0 {0}{1} IOLRawBuffer0:B{2} 1", {"0", "", idx}))
                        End If
                    Next
                    lstBuildCopyCommand.Add("""")
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add(String.Join(" ", lstBuildCopyCommand.ToArray))
                    lstRungCommands.Add(getBRXMathcmdDirect("IOLRawBuffer0", udtBufferHeapItemName & ".in." & rw.Item("fieldName"), rw.Item("fieldType"), rw.Item("sourceBitShift"), rw.Item("sourceBitLength")))
                    lstRungCommands.Add("")
                End If

                Dim splt() As String = CStr(rw.Item("itemValues")).Split({vbCrLf, vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                If splt.Count > 0 Then
                    For i As Integer = 0 To splt.Count - 1
                        splt(i) = """" & splt(i) & """"
                    Next
                    lstElmDoc.Add(String.Join(",", {"""" & udtBufferHeapItemName & ".in." & rw.Item("fieldName") & """", """FLAGS=""", """""", """""", String.Join(",", splt)}))
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
            lstRungCommands.Add("""Stop here if the outputEnable flag is not active.""")
            lstRungCommands.Add("#End")
            lstRungCommands.Add("STRN " & udtBufferHeapItemName & ".enableOutputs")
            lstRungCommands.Add("RETC")
            lstRungCommands.Add("")

            lstRungCommands.Add("#BEGIN FMT_COMMENT")
            lstRungCommands.Add("""Clear UDTBuffer""")
            lstRungCommands.Add("#End")
            lstRungCommands.Add("STR ST1")
            lstRungCommands.Add("MEMCLEAR " & outDataBufferName & "0 32")
            lstRungCommands.Add("")

            For Each rw As DataRow In ds.Tables("outUdt").Rows

                lstRungCommands.Add("#BEGIN FMT_COMMENT")
                lstRungCommands.Add(String.Format("""{0} -> {2}{3}:{5}, Type: {1}, TargetLength: {4}bit""", {rw.Item("fieldName"), rw.Item("fieldType"), outDataBufferName, rw.Item("sourceByteOffset"), rw.Item("sourceBitLength"), rw.Item("sourceBitShift")}))
                lstRungCommands.Add("#End")

                If rw.Item("fieldType") = "BIT" Then
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add(String.Join(" ", {"MATH", outDataBufferName & (rw.Item("sourceByteOffset")) & ":" & rw.Item("sourceBitShift"), """" & udtBufferHeapItemName & ".out." & rw.Item("fieldName") & """"}))
                    lstRungCommands.Add("")
                Else
                    Dim mask As Int32 = 2 ^ rw.Item("sourceBitLength") - 1
                    lstRungCommands.Add("STR ST1")
                    lstRungCommands.Add("MEMCLEAR IOLRawBuffer0 1")
                    lstRungCommands.Add("MATH IOLRawBuffer0 ""(" & udtBufferHeapItemName & ".out." & rw.Item("fieldName") & " & 0x" & mask.ToString("X") & ") << " & rw.Item("sourceBitShift") & """")

                    For i As Integer = 0 To rw.Item("sourceByteLength") - 1
                        Dim targetByte As String = outDataBufferName & rw.Item("sourceByteOffset") + rw.Item("sourceByteLength") - 1 - i

                        lstRungCommands.Add("MATH " & targetByte & " """ & targetByte & " | IOLRawBuffer0:B" & i & """") '" & rw.Item("sourceByteOffset") + rw.Item("sourceByteLength") - 1 - i  & " "(" & tbOutUdtInstanceName.Text.Trim & "." & rw.Item("fieldName") & mask.ToString("X") & ") << " & rw.Item("sourceBitShift") & ")""")

                    Next
                    lstRungCommands.Add("")
                End If

                Dim splt() As String = CStr(rw.Item("itemValues")).Split({vbCrLf, vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                If splt.Count > 0 Then
                    For i As Integer = 0 To splt.Count - 1
                        splt(i) = """" & splt(i) & """"
                    Next
                    lstElmDoc.Add(String.Join(",", {"""" & udtBufferHeapItemName & ".out." & rw.Item("fieldName") & """", """FLAGS=""", """""", """""", String.Join(",", splt)}))
                End If
            Next

            'lstRungCommands.Add("#BEGIN FMT_COMMENT")
            'lstRungCommands.Add("""Copy UDTBuffer to Output""")
            'lstRungCommands.Add("#End")

            'Dim outByteCount As Integer = dvOut.ToTable.Rows(0).Item("pdByteCount")
            'lstRungCommands.Add("STR ST1")
            'lstRungCommands.Add("MEMCOPY IOLUDTOutBuffer0 " & tbOutTargetBlock.Text.Trim & nudTargetStartElement.Value & " 0x0 " & outByteCount)

        End If
#End Region

        lstElmDoc.Add("#END")
        lstRungCommands.Add("RET")
        lstRungCommands.Add("$LGCEND " & tbSubRoutineName.Text.Trim)

        Dim udtOutStart As Integer = ds.Tables("inUdt").Rows.Count
        Dim enableStart As Integer = udtOutStart + ds.Tables("outUdt").Rows.Count

        lstUDTconfig.Add("#BEGIN UDT_CONFIG")
        lstUDTconfig.Add(tbMainUdtName.Text.Trim)
        If ds.Tables("inUdt").Rows.Count > 0 Then lstUDTconfig.Add(String.Join(",", {"in", tbInUdtName.Text.Trim, "0" & ":0", "Read-Write, Native, Short"}))
        If ds.Tables("outUdt").Rows.Count > 0 Then lstUDTconfig.Add(String.Join(",", {"out", tbOutUdtName.Text.Trim, udtOutStart & ":0", "Read-Write, Native, Short"}))
        If ds.Tables("outUdt").Rows.Count > 0 Then lstUDTconfig.Add(String.Join(",", {"enableOutputs", "BIT", enableStart & ":0", "Read-Write, Native, Short"}))
        lstUDTconfig.Add("#END")

        Dim lstRungCommandsPGMmapIOLink As New List(Of String)

        If cbGenerateSRCall.Checked Then
            lstRungCommandsPGMmapIOLink.Add("$PRGRM MapIOLink")
            lstRungCommandsPGMmapIOLink.Add("STR ST1")
            lstRungCommandsPGMmapIOLink.Add(String.Format("CALL {0} 0x1 DST511 ""3 {1}"" ""3 {2}""", {tbSubRoutineName.Text.Trim, String.Join(" ", {tbInSourceBlock.Text.Trim & nudSourceStartElement.Value, inDataBufferName & "0", 32, tbUdtHeapItem.Text.Trim, udtBufferHeapItemName, 1}), String.Join(" ", {udtBufferHeapItemName, tbUdtHeapItem.Text.Trim, 1, outDataBufferName & "0", tbOutTargetBlock.Text.Trim & nudTargetStartElement.Value, 32})}))
            lstRungCommandsPGMmapIOLink.Add("")
            lstRungCommandsPGMmapIOLink.Add("$PGMEND MapIOLink")
        End If

        Dim lstMemConfg As New List(Of String)
        lstMemConfg.Add("#BEGIN MEM_CONFIG")

        lstMemConfg.Add(String.Join(" ", {udtBufferHeapItemName, tbMainUdtName.Text, "0"}))
        lstMemConfg.Add(String.Join(" ", {tbUdtHeapItem.Text.Trim, tbMainUdtName.Text, "0"}))
        lstMemConfg.Add(String.Join(" ", {"IOLRawBuffer", "SDWORD", "1 -1"}))
        lstMemConfg.Add(String.Join(" ", {inDataBufferName, "UBYTE", "32 -1"}))
        lstMemConfg.Add(String.Join(" ", {outDataBufferName, "UBYTE", "32 -1"}))
        ' lstMemConfg.Add(String.Join(" ", {"b_IOLMapDataIn", "UBYTE", "32 -1"}))
        'If rbIndirectMode.Checked Then
        '    lstMemConfg.Add(String.Join(" ", {tbInSourceBlock.Text, "UBYTE", "32 -1"}))
        '    lstMemConfg.Add(String.Join(" ", {tbOutTargetBlock.Text, "UBYTE", "32 -1"}))
        'End If

        lstMemConfg.Add(String.Join(" ", {tbSubRoutineName.Text.Trim, "<Virtual>", "0"}))
        If cbGenerateSRCall.Checked Then lstMemConfg.Add(String.Join(" ", {"MapIOLink", "PROGRAM", "0 -1"}))

        lstMemConfg.Add("#END")

        Dim sfd As New SaveFileDialog
        sfd.AddExtension = True
        sfd.Filter = "text file | *.txt"
        sfd.FileName = "BrxExport_" & vendorNameClean & "_" & deviceNameClean & "_" & tbSubRoutineName.Text.Trim
        If sfd.ShowDialog = DialogResult.OK Then
            Dim lstOut As New List(Of String)
            lstOut.AddRange(lstUDTconfig)
            lstOut.AddRange(lstMemConfg)
            '   lstOut.AddRange(lstElmDoc)
            If cbGenerateSR.Checked Then lstOut.AddRange(lstRungCommands)
            If cbGenerateSRCall.Checked Then lstOut.AddRange(lstRungCommandsPGMmapIOLink)
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
            .Columns.Add("itemValues", GetType(String))

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
            If Not String.IsNullOrWhiteSpace(udtc.fieldType) Then
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
                udtc.itemValues = rw.Item("itemValues")
                posCnt += 1
                ds.Tables("inUdt").Rows.Add(udtc.getValues)
            End If

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
            .Columns.Add("itemValues", GetType(String))

        End With


        posCnt = 0
        For Each rw As DataRow In dvOut.ToTable.Rows
            Dim udtc As New UdtConfig
            udtc.fieldName = rw.Item("udtFieldName").trim
            udtc.fieldType = getBrxType(rw.Item("itemType").trim, rw.Item("itemBitLengthNum"))
            udtc.fieldStartByte = posCnt * 4
            udtc.fieldStartDword = posCnt
            udtc.sourceBlock = tbInSourceBlock.Text
            If Not String.IsNullOrWhiteSpace(udtc.fieldType) Then
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
                udtc.itemValues = rw.Item("itemValues")
                posCnt += 1
                ds.Tables("outUdt").Rows.Add(udtc.getValues)
            End If

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
        Public itemValues As String

        Public Function getValues() As String()
            Return {fieldName, fieldType, fieldStartByte, fieldStartDword, sourceBlock, sourceByteOffset, sourceByteLength, sourceBitShift, sourceType, sourceBitLength, itemValues}
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
            conditionVal = ""
        Else
            dvIn.RowFilter = "pdDir = 'In' AND " & "conditionValName = '" & cbSelectedOption.SelectedValue.ToString & "'"
            dvOut.RowFilter = "pdDir = 'Out' AND " & "conditionValName = '" & cbSelectedOption.SelectedValue.ToString & "'"
            If Not IsDBNull(CType(cbSelectedOption.SelectedItem, DataRowView).Item("conditionValue")) Then
                conditionVal = CType(cbSelectedOption.SelectedItem, DataRowView).Item("conditionValue")
                conditionDesc = cbSelectedOption.SelectedValue.ToString
            Else
                conditionVal = ""
                conditionDesc = ""
            End If

        End If
        updateTypeAndSubName()
        calculateUdts()
    End Sub



    Private Sub cbGenerateSRCall_CheckedChanged(sender As Object, e As EventArgs) Handles cbGenerateSRCall.CheckedChanged
        gbInSourceData.Enabled = cbGenerateSRCall.Checked
        gbOutTargetData.Enabled = cbGenerateSRCall.Checked
        gbDeviceName.Enabled = cbGenerateSRCall.Checked
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

    Public Sub loadHowTo()
        Dim dispString As String = $"
            <html><body style=""max-width: 800px;"">
<h3>Purpose of this utility</h3>
This utility generates the data structures and subroutines required to easily access the inputs and/or outputs of an IO-Link device connected to a BRX PLC through an IO-Link master.
The generated code is exported and saved as a text file and can be imported into Do-More Designer.
This utility provides two 'modes' for the generated output:
<p><b>Direct Mode</b><br>
The Memory block referenced in the EIP scanner is used directly.<br> This requires a dedicated subroutine for each of the IOL-Masters ports, even when the connected devices are the same.
</p>
<p><b>Indirect Mode</b><br>
The subroutine uses a buffer data block that is copied to and from upon calling the sub routine. 
<br>This requires more work when calling the subroutine but alllows for the same subroutine to be used for more then one IOL master port if the connected devices are the same.
</p>
<h3>How to use this dialog</h3>
    <ul>
        <li>If the device has more than one processdata layout option, select the option to use from the drop down list</li> 
        <li>Choose from one of two modes the subroutine can be created:
                <ul>
                <li><b>Direct Mode:</b> The Memory block referenced in the EIP scanner is used directly (this requires a dedicated subroutine for each of the IOL-Masters ports, even when the connected devices are the same.)</li>
                <li><b>Indirect Mode:</b>The subroutine uses a buffer data block that is copied to and from upon calling the sub routine. (This requires more work when calling the subroutine but alllows for the same subroutine to be used for more then one IOL master port if the connected devices are the same.)</li>
                </ul>
        </li> 
        <li>If using the Direct mode 
                <ul>
                <li>enter the names of the input and output data blocks to match what's used in the EIP scanner. If you selected an EIP master and port when loading the IODD file the offsets will be set already. Otherwise consult the manual and enter the offests accordigng to the port used.</li>
                <li>Enter a name for the heap item generated to access the device's In and Out variables.</li>
                </ul>
        </li>
        <li>The subroutine name is already preconfigured based on the device selected. Only change it if keeping it would conflict with an already existing subroutine.</li>
        <li>The deviceUDT name is already preconfigured based on the device selected. Only change it if keeping it would conflict with an already existing subroutine.</li>
    </ul>
<h3>How to import und use the generated code with BRX</h3>

<ul>
<li>With a project loaded in Do-More Designer click 'Tools' / 'Insert Instructiond From File' to open the 'Insert Instructions' dialog</li>
<li>IMPORTANT: In this dialog, make sure to select <b>As Code Blocks</b> next to the file selection dialog</li>
<li>Now select the file and click 'open'</li>
<li>If you used direct mode (as described above):
	<ul>
	<li>use the 'call' instruction to execute the just imported subroutine in any program.</li>
	<li>The data will be available through the imported heap item (at default settings this starts with 'IOL_'</li>
	<li>The input data will immediately be mapped to the variables in the IN structure of the heap item.</li>
	<li>The output will not be copied to the data block until the bit 'enableOutputs' in the heap item is written to TRUE</li>
	</ul>
</li>
<li>If you used indirect mode (as descibed above):
	<ul>
	<li>Generate a heap item using the device UDT that was imported into DmD.</li>
	<li>Add a 'call' instruction to execute the imported subroutine in any program.</li>
	<li>in this 'call' instruction, make sure to enable the optionals input and optional output parameters</li>
	<li>in the input parameter section, copy 
		<ul>
		<li>[StartElementOfIOLMasterPortINData] -> iolMapRawInBuf (32 Elements)</li>
		<li>[StartElementOfIOLMasterPortOUTData] -> iolMapRawOutBuf (32 Elements)</li>		
		<li>[HeapItemCreatedInStep1] -> [bufferElement] (1 Element)  (the name of the buffer element is identical to the sub routine name with m_ replaced for b_)</li>		
		</ul>
	</li>
	<li>in the input parameter section, copy 
		<ul>
		<li>iolMapRawOutBuf -> [StartElementOfIOLMasterPortOUTData] (32 Elements)</li>		
		<li>[bufferElement] -> [HeapItemCreatedInStep1] (1 Element)  (the name of the buffer element is identical to the sub routine name with m_ replaced for b_)</li>		
		</ul>
	</li>
	<li>The data will be available through the heap item created in step 1</li>
	<li>The input data will immediately be mapped to the variables in the IN structure of the heap item.</li>
	<li>The output will not be copied to the data block until the bit 'enableOutputs' in the heap item is written to TRUE</li>
	</ul>


</li>
</ul>
</html></body>
        "

        WebView22.NavigateToString(dispString)
    End Sub
End Class
