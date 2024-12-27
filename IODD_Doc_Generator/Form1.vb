
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

Public Class Form1

    Dim ds As DataSet

    Private iodev As IODevice
    Private dctVariables As Dictionary(Of String, VariableCollectionTVariable)
    Private dctTexts As Dictionary(Of String, String)
    Private lstProcessData As List(Of ProcessDataT)
    Private dctDatatypes As Dictionary(Of String, DatatypeT)
    Public dctColWidth As New Dictionary(Of String, Single)
    Private iolMasters As clsIolMasterDefinitions
    Private ioddpath As String
    Private cssStyling As String
    Private devInfo As deviceInfo
    Private currentOutput As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim userDataFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\IODD_XML_parser"
        Dim env = CoreWebView2Environment.CreateAsync(Nothing, userDataFolder)
        env.Wait()
        WebView21.EnsureCoreWebView2Async(env.Result)
        Dim path As String = AppDomain.CurrentDomain.BaseDirectory
        iolMasters = New clsIolMasterDefinitions(path & "\IOLDef\", True)
        If System.IO.File.Exists(path & "\styling.css") Then cssStyling = System.IO.File.ReadAllText(path & "\styling.css")
        tscbIolMasters.Items.AddRange(iolMasters.masters.ToArray) : tscbIolMasters.SelectedIndex = 0
    End Sub
    Private Sub LoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadToolStripMenuItem.Click
        loadIODD()
    End Sub
    Private Sub loadIODD()
        Dim ofd As New OpenFileDialog
        If ofd.ShowDialog = DialogResult.OK Then
            Dim serializer As New XmlSerializer(GetType(IODevice))
            Dim reader As New IO.StreamReader(ofd.FileName)
            iodev = serializer.Deserialize(reader)
            reader.Close()
            reader.Dispose()
            ioddpath = System.IO.Path.GetDirectoryName(ofd.FileName)

            populateDictionaries()
            populateDeviceInfo(System.IO.Path.GetFileName(ofd.FileName))
            generateTables()
            generateVariableTable()
            getSvgByteMapExtended()
        End If
    End Sub
    Private Sub populateDictionaries()
        dctVariables = New Dictionary(Of String, VariableCollectionTVariable)

        For Each var As VariableCollectionTVariable In iodev.ProfileBody.DeviceFunction.VariableCollection.Variable
            dctVariables.Add(var.id, var)
        Next

        dctTexts = New Dictionary(Of String, String)
        For Each text As TextDefinitionT In iodev.ExternalTextCollection.PrimaryLanguage.Items
            dctTexts.Add(text.id, text.value)
        Next

        lstProcessData = New List(Of ProcessDataT)
        For Each pd As ProcessDataT In iodev.ProfileBody.DeviceFunction.ProcessDataCollection.ProcessData
            lstProcessData.Add(pd)
        Next

        dctDatatypes = New Dictionary(Of String, DatatypeT)
        If Not IsNothing(iodev.ProfileBody.DeviceFunction.DatatypeCollection) Then
            For Each dt As DatatypeT In iodev.ProfileBody.DeviceFunction.DatatypeCollection.Datatype
                dctDatatypes.Add(dt.id, dt)
            Next
        End If

    End Sub

    Private Sub populateDeviceInfo(filename As String)
        With devInfo
            .filename = filename
            .version = iodev.DocumentInfo.version
            .releaseDate = iodev.DocumentInfo.releaseDate
            .vendorId = iodev.ProfileBody.DeviceIdentity.vendorId
            .vendorName = iodev.ProfileBody.DeviceIdentity.vendorName
            .deviceId = iodev.ProfileBody.DeviceIdentity.deviceId
            .deviceName = getTextRef(iodev.ProfileBody.DeviceIdentity.DeviceName.textId)
            .description = getTextRef(iodev.ProfileBody.DeviceIdentity.DeviceVariantCollection.DeviceVariant(0).Description.textId)

            If Not IsNothing(iodev.ProfileBody.DeviceIdentity.GetType.GetProperty("VendorLogo")) AndAlso Not IsNothing(iodev.ProfileBody.DeviceIdentity.VendorLogo) Then
                .logoFile = ioddpath & "\" & iodev.ProfileBody.DeviceIdentity.VendorLogo.name
                If System.IO.File.Exists(.logoFile) Then .logo = Convert.ToBase64String(System.IO.File.ReadAllBytes(devInfo.logoFile))
            Else
                .logoFile = "" : .logo = ""
            End If
            If Not IsNothing(iodev.ProfileBody.DeviceIdentity.DeviceVariantCollection.DeviceVariant(0).GetType.GetProperty("deviceSymbol")) AndAlso Not IsNothing(iodev.ProfileBody.DeviceIdentity.DeviceVariantCollection.DeviceVariant(0).deviceSymbol) Then
                .deviceSymbolFile = ioddpath & "\" & iodev.ProfileBody.DeviceIdentity.DeviceVariantCollection.DeviceVariant(0).deviceSymbol
                If System.IO.File.Exists(.deviceSymbolFile) Then .deviceSymbol = Convert.ToBase64String(System.IO.File.ReadAllBytes(devInfo.deviceSymbolFile))
            Else
                .deviceSymbolFile = "" : .deviceSymbol = ""
            End If
        End With
    End Sub
    Private Sub generateTables()
        ds = New DataSet
        ds.Tables.Add("processData")
        With ds.Tables("processData")
            .Columns.Add("pdID", GetType(String))
            .Columns.Add("conditionVarName", GetType(String))
            .Columns.Add("conditionValName", GetType(String))
            .Columns.Add("conditionISDU", GetType(String))
            .Columns.Add("conditionValue", GetType(String))
            .Columns.Add("conditionDefaultVal", GetType(String))
            .Columns.Add("conditionDefaultName", GetType(String))
            .Columns.Add("pdDir", GetType(String))
            .Columns.Add("pdByteCount", GetType(Integer))
            .Columns.Add("pdName", GetType(String))
            .Columns.Add("itemName", GetType(String))
            .Columns.Add("itemBitOffset", GetType(Integer))
            .Columns.Add("itemBitLength", GetType(String))
            .Columns.Add("itemType", GetType(String))
            .Columns.Add("itemValues", GetType(String))
            .Columns.Add("itemMin", GetType(String))
            .Columns.Add("itemMax", GetType(String))
            .Columns.Add("itemBitLengthNum", GetType(Integer), "IIF(IsNull(itemBitLength, '') <> '',Convert(itemBitLength,'System.Int16'),1)")
            .Columns.Add("PIMsbBit", GetType(Integer), "(itemBitOffset + itemBitLengthNum -1) % 8")
            .Columns.Add("PIMsbByte", GetType(Integer), "pdByteCount - ((itemBitOffset + itemBitLengthNum -1 - PIMsbBit ) / 8)")
            .Columns.Add("PILsbBit", GetType(Integer), "(itemBitOffset) % 8")
            .Columns.Add("PILsbByte", GetType(Integer), "pdByteCount - ((itemBitOffset - PILsbBit ) / 8)")
        End With

        Dim pdID As Integer = 0

        For Each pd As ProcessDataT In lstProcessData

            'handle possible conditions
            Dim cVarName, cValName, cISDU, cValue, cDefaultVal, cDefaultName As String
            If Not IsNothing(pd.Condition) Then
                If dctVariables.ContainsKey((pd.Condition.variableId)) Then
                    Dim var As VariableCollectionTVariable = dctVariables((pd.Condition.variableId))
                    cVarName = getTextRef(var.Name.textId)
                    If Not IsNothing(var.Item.GetType.GetProperty("Items")) AndAlso Not IsNothing(var.Item.Items) Then
                        For Each val As Object In var.Item.items
                            Dim pi As PropertyInfo = val.GetType().GetProperty("value")
                            If Not IsNothing(pi) Then
                                If val.value = pd.Condition.value Then
                                    cValName = getTextRef(val.Name.textId)
                                    cISDU = var.index
                                    cDefaultVal = var.defaultValue
                                End If

                                If val.value = var.defaultValue Then
                                    cDefaultName = getTextRef(val.Name.textId)
                                End If
                            End If
                        Next
                    End If
                End If
                cValue = pd.Condition.value
            End If

            Dim pdDir, pdBC, pdName As String
            Dim pdits As New Dictionary(Of String, ProcessDataItemT)

            If Not IsNothing(pd.ProcessDataIn) Then pdits.Add("In", pd.ProcessDataIn)
            If Not IsNothing(pd.ProcessDataOut) Then pdits.Add("Out", pd.ProcessDataOut)

            For Each dir As String In pdits.Keys

                Dim pdit As ProcessDataItemT = pdits(dir)
                pdDir = dir
                pdName = getTextRef(pdit.Name.textId)
                pdBC = Math.Ceiling(pdit.bitLength / 8)

                Dim ti As TypeInfo
                If Not IsNothing(pdit.Item.GetType.GetProperty("RecordItem")) Then
                    For Each pdItm In pdit.Item.RecordItem
                        ti = getTableVarItemDataRecordItem(pdItm)
                        ds.Tables("processData").Rows.Add({pdID, cVarName, cValName, cISDU, cValue, cDefaultVal, cDefaultName, pdDir, pdBC, pdName, ti.itemName, ti.itemBO, ti.itemBL, ti.itemType, ti.itemVals, ti.itemMin, ti.itemMax})

                    Next
                Else
                    If pdit.Item.GetType = GetType(RecordItemT) Then
                        ti = getTableVarItemDataRecordItem(pdit.Item)
                        ds.Tables("processData").Rows.Add({pdID, cVarName, cValName, cISDU, cValue, cDefaultVal, cDefaultName, pdDir, pdBC, pdName, ti.itemName, ti.itemBO, ti.itemBL, ti.itemType, ti.itemVals, ti.itemMin, ti.itemMax})

                    ElseIf pdit.Item.GetType = GetType(OctetStringT) Then
                        ti.itemBL = pdit.Item.fixedLength * 8
                        ti.itemType = GetType(OctetStringT).Name
                        ds.Tables("processData").Rows.Add({pdID, cVarName, cValName, cISDU, cValue, cDefaultVal, cDefaultName, pdDir, pdBC, pdName, ti.itemName, ti.itemBO, ti.itemBL, ti.itemType, ti.itemVals, ti.itemMin, ti.itemMax})
                    Else

                        If Not IsNothing(pdit.Item.GetType.GetProperty("bitlength")) Then ti.itemBL = pdit.Item.bitlength

                        ti.itemType = pdit.Item.GetType.Name
                        ds.Tables("processData").Rows.Add({pdID, cVarName, cValName, cISDU, cValue, cDefaultVal, cDefaultName, pdDir, pdBC, pdName, ti.itemName, ti.itemBO, ti.itemBL, ti.itemType, ti.itemVals, ti.itemMin, ti.itemMax})

                    End If
                End If
            Next

            pdID += 1
        Next

        ds.Tables("processData").DefaultView.Sort = "pdID ASC, pdDir ASC, PILsbByte ASC, PILsbBit ASC"
        ds.Tables.Add(ds.Tables("processData").DefaultView.ToTable("conditions", True, "conditionValName"))

    End Sub

    Private Sub generateVariableTable()
        With ds.Tables.Add("variables")
            .Columns.Add("id", GetType(String))
            .Columns.Add("index", GetType(Integer))
            .Columns.Add("accessRights", GetType(String))
            .Columns.Add("name", GetType(String))
            .Columns.Add("desc", GetType(String))
            .Columns.Add("subindex", GetType(String))
            .Columns.Add("bitOffset", GetType(String))
            .Columns.Add("bitLength", GetType(String))
            .Columns.Add("siName", GetType(String))
            .Columns.Add("type", GetType(String))
            .Columns.Add("values", GetType(String))

            For Each var As VariableCollectionTVariable In dctVariables.Values

                Dim vals() As String = {var.id, var.index, var.accessRights.ToString, getTextRef(var.Name.textId), getTextRef(getTextIdFailsafe(var.Description))}
                If var.Item.GetType.Name = "RecordT" Then

                    For Each ri As RecordItemT In CType(var.Item, RecordT).RecordItem
                        Dim ti As TypeInfo = getTableVarItemDataRecordItem(ri)
                        Dim lst As New List(Of String)
                        lst.AddRange(vals)
                        lst.AddRange({ri.subindex, ri.bitOffset, ti.itemBL, getTextRef(ri.Name.textId), ti.itemType, ti.itemVals})
                        .Rows.Add(lst.ToArray)
                    Next

                ElseIf var.Item.GetType.Name = "ArrayT" Then
                    Dim ti As TypeInfo
                    If var.Item.item.GetType = GetType(DatatypeRefT) AndAlso dctDatatypes.ContainsKey(var.Item.item.datatypeId) Then
                        ti = getTableVarItemDataObj(dctDatatypes(var.Item.item.datatypeId))
                    Else
                        ti = getTableVarItemDataObj(var.Item.item)
                    End If


                    ti.itemName = getTextRef(var.Name.textId)
                    ti.itemType = "Array" & vbCrLf & "Count: " & var.Item.count & vbCrLf & ti.itemType
                    Dim lst As New List(Of String)
                    lst.AddRange(vals)
                    lst.AddRange({"-", "", ti.itemBL, getTextRef(var.Name.textId), ti.itemType, ti.itemVals})
                    .Rows.Add(lst.ToArray)

                Else

                    Dim ti As TypeInfo = getTableVarItemDataSingelVar(var)
                    Dim lst As New List(Of String)
                    lst.AddRange(vals)
                    lst.AddRange({"-", "", ti.itemBL, getTextRef(var.Name.textId), ti.itemType, ti.itemVals})
                    .Rows.Add(lst.ToArray)
                End If

            Next
        End With
    End Sub

    Public Function getTextIdFailsafe(var As Object) As String
        If IsNothing(var) Then
            Return "Nothing"
        Else
            If IsNothing(var.textId) Then
                Return "Nothing"
            Else
                Return var.textId
            End If
        End If
    End Function

    Private Function getTableVarItemDataRecordItem(pd As RecordItemT) As TypeInfo
        Dim ti As New TypeInfo
        If IsNothing(pd) Then Return New TypeInfo
        If pd.Item.GetType = GetType(DatatypeRefT) AndAlso dctDatatypes.ContainsKey(pd.Item.datatypeId) Then
            ti = getTableVarItemDataObj(dctDatatypes(pd.Item.datatypeId))
        Else
            ti = getTableVarItemDataObj(pd.Item)
        End If
        ti.itemName = getTextRef(pd.Name.textId)
        ti.itemBO = pd.bitOffset
        Return ti
    End Function

    Private Function getTableVarItemDataObj(itm As Object) As TypeInfo
        Dim ti As New TypeInfo

        If IsNothing(itm) Then Return ti

        If Not IsNothing(itm.GetType.GetProperty("bitLength")) AndAlso Not IsNothing(itm.bitLength) Then
            ti.itemBL = itm.bitLength
        Else
            If itm.GetType.Name.ToUpper = "FLOAT32T" Then
                ti.itemBL = "32"
            Else
                ti.itemBL = ""
            End If
        End If

        ti.itemType = itm.GetType.Name

        ti.itemMin = "" 'reset to empty and set if applicable below
        ti.itemMax = ""

        Dim lstVal As New List(Of String)
        If Not IsNothing(itm.GetType.GetProperty("SingleValue")) AndAlso Not IsNothing(itm.SingleValue) Then
            For Each sv As Object In itm.SingleValue
                Dim str As String = sv.value.ToString
                If Not IsNothing(sv.name) Then str &= " = " & getTextRef(sv.name.textid)
                lstVal.Add(str)
            Next
        ElseIf Not IsNothing(itm.GetType.GetProperty("Items")) AndAlso Not IsNothing(itm.Items) Then

            For Each item As Object In itm.Items
                Dim str As String = ""
                If Not IsNothing(item.GetType.GetProperty("lowerValue")) And Not IsNothing(item.GetType.GetProperty("upperValue")) AndAlso Not IsNothing(item.lowerValue) AndAlso Not IsNothing(item.upperValue) Then
                    ti.itemMin = item.lowerValue
                    ti.itemMax = item.upperValue
                    str = String.Format("{0} .. {1}", {item.lowerValue, item.upperValue})
                ElseIf Not IsNothing(item.GetType.GetProperty("value")) AndAlso Not IsNothing(item.value) Then
                    str = item.value.ToString
                End If
                If Not IsNothing(item.Name) AndAlso Not IsNothing(item.name.GetType.GetProperty("textId")) Then str &= " = " & getTextRef(item.Name.textId)
                lstVal.Add(str)
            Next
        End If

        ti.itemVals = String.Join(vbCrLf, lstVal.ToArray)
        Return ti
    End Function

    Private Function getTableVarItemDataSingelVar(pd As VariableCollectionTVariable) As TypeInfo
        Dim ti As New TypeInfo

        If IsNothing(pd) Then Return ti

        If pd.Item.GetType = GetType(DatatypeRefT) AndAlso dctDatatypes.ContainsKey(pd.Item.datatypeId) Then
            ti = getTableVarItemDataObj(dctDatatypes(pd.Item.datatypeId))
        Else
            ti = getTableVarItemDataObj(pd.Item)
        End If

        ti.itemName = getTextRef(pd.Name.textId)
        ti.itemBO = ""

        Return ti
    End Function

    Private Function getTextRef(key As String, Optional def As String = "") As String
        If dctTexts.ContainsKey(key) Then Return dctTexts(key) Else Return def
    End Function

    Private Function htmlTd(content As String, Optional colspan As Integer = 1, Optional rowspan As Integer = 1, Optional paddingVertical As Integer = 0, Optional paddingHorizontal As Integer = 0, Optional center As Boolean = False, Optional color As String = "", Optional omitBorder As Boolean = False) As String
        Dim sw As New StringWriter
        Dim setts As New XmlWriterSettings
        setts.Indent = True
        setts.ConformanceLevel = ConformanceLevel.Fragment
        Dim xmlw As XmlWriter = XmlWriter.Create(sw, setts)
        '  If startRow Then xmlw.WriteStartElement("tr")
        xmlw.WriteStartElement("td")
        If colspan > 1 Then xmlw.WriteAttributeString("colspan", colspan)
        If rowspan > 1 Then xmlw.WriteAttributeString("rowspan", rowspan)
        Dim lstStyles As New List(Of String)
        lstStyles.Add("padding: " & paddingVertical & "px " & paddingHorizontal & "px")
        If center Then lstStyles.Add("text-align: center")
        If color <> "" Then lstStyles.Add("background: " & color)
        If omitBorder Then lstStyles.Add("border:none")
        xmlw.WriteAttributeString("style", String.Join(";", lstStyles.ToArray))
        xmlw.WriteRaw(content.Replace(vbCrLf, "<br>"))
        xmlw.WriteEndElement()
        xmlw.Flush()
        Return sw.ToString
    End Function

    Private Structure byteMapExtVals
        Dim dir As String
        Dim [byte] As Integer
        Dim bit As Integer
        Dim showAsEllipsis As Boolean
        Dim isMsb As Boolean
        Dim isLsb As Boolean
        Dim text As String
        Dim rowId As Integer
        Dim pdItemId As Integer
        Dim color As String
        Dim pdItemBo_Bl As String
        Dim pdItemType As String
        Dim pdItemValues As String
        Dim pdDir_Name_BC As String
        Dim pdBitLength As Integer
        Dim conditionstring As String
        Dim pdId As Integer

        Public Sub New(vals() As Object)
            If vals.Count > 1 Then
                For i As Integer = 0 To vals.Count - 1
                    Select Case i
                        Case 0 : dir = vals(i)
                        Case 1 : [byte] = vals(i)
                        Case 2 : bit = vals(i)
                        Case 3 : showAsEllipsis = vals(i)
                        Case 4 : isMsb = vals(i)
                        Case 5 : isLsb = vals(i)
                        Case 6 : text = vals(i)
                        Case 7 : rowId = vals(i)
                        Case 8 : pdItemId = vals(i)
                        Case 9 : color = vals(i)
                        Case 10 : pdItemBo_Bl = vals(i)
                        Case 11 : pdItemType = vals(i)
                        Case 12 : pdItemValues = vals(i)
                        Case 13 : pdDir_Name_BC = vals(i)
                        Case 14 : pdBitLength = vals(i)
                        Case 15 : conditionstring = vals(i)
                        Case 16 : pdId = vals(i)
                    End Select
                Next
            End If
        End Sub
        Public Function getValArray() As Object()
            Return {dir, [byte], bit, showAsEllipsis, isMsb, isLsb, text, rowId, pdItemId, color, pdItemBo_Bl, pdItemType, pdItemValues, pdDir_Name_BC, pdBitLength, conditionstring, pdId}
        End Function
    End Structure
    Private Sub getSvgByteMapExtended()
        If ds.Tables.Contains("byteMapExt") Then ds.Tables.Remove("byteMapExt")
        ds.Tables.Add("byteMapExt")
        With ds.Tables("byteMapExt").Columns
            .Add("dir", GetType(String))
            .Add("byte", GetType(Integer))
            .Add("bit", GetType(Integer))
            .Add("showAsEllipsis", GetType(Boolean))
            .Add("isMsb", GetType(Boolean))
            .Add("isLsb", GetType(Boolean))
            .Add("text", GetType(String))
            .Add("rowId", GetType(Integer))
            .Add("pdItemId", GetType(Integer))
            .Add("color", GetType(String))
            .Add("pdItemBo_Bl", GetType(String))
            .Add("pdItemType", GetType(String))
            .Add("pdItemValues", GetType(String))
            .Add("pdDir_Name_BC", GetType(String))
            .Add("pdBitLength", GetType(Integer))
            .Add("conditionString", GetType(String))
            .Add("pdId", GetType(Integer))

        End With
        Dim colInput() As String = {"#e1eefc", "#f5faff"}
        Dim colOutput() As String = {"#fce1e1", "#fcf7f7"}
        Dim cols As New colorSchemer({"#f5fcff", "#ffffba"})

        Dim conditionVarName As String = ""
        Dim conditionVarISDU As String = ""
        Dim conditionVarDefault As String = ""



        Dim masterPortSelected As Boolean = Not IsNothing(tscbIolMasterPorts.SelectedItem)
        Dim masterPort As String = ""
        Dim InOffset As Integer
        Dim OutOffset As Integer
        If masterPortSelected Then
            InOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).inByteStart
            OutOffset = CType(tscbIolMasterPorts.SelectedItem, portInfo).outByteStart
            masterPort = "ProcessImage Byte" & vbCrLf & "using master" & vbCrLf & tscbIolMasters.SelectedItem.manufacturer & " - " & tscbIolMasters.SelectedItem.model & vbCrLf & "device connected to" & vbCrLf & tscbIolMasterPorts.SelectedItem.ToString
        Else
            InOffset = -1
            OutOffset = -1
        End If
        Dim dstPdDir As DataTable = ds.Tables("processData").DefaultView.ToTable(True, {"pdDir", "pdId"})
        dstPdDir.DefaultView.Sort = "pdId ASC, pdDir ASC"

        Dim pdItemId As Integer = 0
        For Each drw As DataRow In dstPdDir.DefaultView.ToTable.Rows


            Dim rwId As Integer = 0

            Dim dir As String = drw.Item("pdDir")
            ds.Tables("processData").DefaultView.RowFilter = "pdDir = '" & dir & "' AND pdId = '" & drw.Item("pdId") & "'"
            ds.Tables("processData").DefaultView.Sort = "pdDir ASC, itemBitOffset ASC"
            For Each rw As DataRow In ds.Tables("processData").DefaultView.ToTable.Rows

                ' Dim col As String = IIf(dir.ToLower = "in", colsIn.getColor, colsOut.getColor)

                If Not IsDBNull(rw.Item("conditionVarName")) Then conditionVarName = rw.Item("conditionVarName")
                If Not IsDBNull(rw.Item("conditionISDU")) Then conditionVarISDU = rw.Item("conditionISDU")
                If Not IsDBNull(rw.Item("conditionDefaultVal")) Then conditionVarDefault = rw.Item("conditionDefaultVal") & " "
                If Not IsDBNull(rw.Item("conditionDefaultName")) Then conditionVarDefault += "(" & rw.Item("conditionDefaultName") & ")"
                Dim bit, bt As Integer
                bit = rw.Item("itemBitOffset") Mod 8
                If rw.Item("itemBitLength") <> "" Then
                    bt = rw.Item("pdByteCount") - ((rw.Item("itemBitOffset") - bit + CInt(rw.Item("itemBitLength")) - 1) / 8)
                Else
                    bt = rw.Item("pdByteCount") - ((rw.Item("itemBitOffset") - bit) / 8)
                End If
                Dim posString As String = String.Format("Byte {0} Bit {1} ~ {0}.{1}", {bt, bit})


                If rw.Item("itemBitLengthNum") > 1 Then
                    posString = String.Format("LSB: Byte {0} Bit {1} ~ {0}.{1}" & vbCrLf & "MSB: Byte {2} Bit {3} ~ {2}.{3}" & vbCrLf & "IODD Bit-Offset: " & rw.Item("itemBitOffset") & vbCrLf & "Bit-Length: " & rw.Item("itemBitLength"), {rw.Item("PILsbByte"), rw.Item("PILsbBit"), rw.Item("PIMsbByte"), rw.Item("PIMsbBit")})
                Else
                    posString = String.Format("Byte {0} Bit {1} ~ {0}.{1}" & vbCrLf & "IODD Bit-Offset: " & rw.Item("itemBitOffset"), {rw.Item("PILsbByte"), rw.Item("PILsbBit"), rw.Item("PIMsbByte"), rw.Item("PIMsbBit")})
                End If
                Dim conditionstring As String = String.Format("Condition: {0} = {1} ~ ISDU: {2} value: {3} ~ default: {4} ({5})", rw.Item("conditionVarName"), rw.Item("conditionValName"), rw.Item("conditionISDU"), rw.Item("conditionValue"), rw.Item("conditionDefaultVal"), rw.Item("conditionDefaultName"))
                Dim vals As New byteMapExtVals({dir, 0, 0, False, False, False, rw.Item("itemName"), rwId, pdItemId, "", posString, "DataType: " & rw.Item("itemType"), rw.Item("itemValues"), "Direction: " & rw.Item("pdDir") & " ~ " & "Name: " & rw.Item("pdName") & " ~ " & "Bytecount: " & rw.Item("pdByteCount"), rw.Item("itemBitLengthNum"), conditionstring, drw.Item("pdId")})
                If rw.Item("itemBitLengthNum") = 1 Then
                    vals.byte = rw.Item("PILsbByte") : vals.bit = rw.Item("PILsbBit") : vals.showAsEllipsis = False : vals.isMsb = False : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)

                ElseIf rw.Item("itemBitLengthNum") >= 4 Then
                    vals.byte = rw.Item("PILsbByte") : vals.bit = rw.Item("PILsbBit") : vals.showAsEllipsis = False : vals.isMsb = False : vals.isLsb = True : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)

                    rwId += 1
                    Dim byteOffset As Integer = 0
                    Dim curBit As Integer = rw.Item("PILsbBit") + 1
                    Dim ellipsisSet As Boolean = False
                    For i As Integer = 1 To rw.Item("itemBitLengthNum") - 2
                        If curBit = 8 Then
                            byteOffset -= 1 'in relation to the IODD bitoffset the process bytes move in negative direction
                            curBit = 0
                            vals.byte = rw.Item("PILsbByte") + byteOffset : vals.bit = curBit : vals.showAsEllipsis = False : vals.isMsb = False : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)

                            rwId += 1
                            ellipsisSet = False
                        ElseIf curBit = 7 Then
                            vals.byte = rw.Item("PILsbByte") + byteOffset : vals.bit = curBit : vals.showAsEllipsis = False : vals.isMsb = False : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)
                            rwId += 1
                            ellipsisSet = False
                        Else
                            If Not ellipsisSet Then 'there's at least two consecutive rows left that can be ellipsed
                                ellipsisSet = True
                                vals.byte = rw.Item("PILsbByte") + byteOffset : vals.bit = curBit : vals.showAsEllipsis = True : vals.isMsb = False : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)
                                rwId += 1
                            End If
                        End If
                        curBit += 1

                    Next
                    vals.byte = rw.Item("PIMsbByte") : vals.bit = rw.Item("PIMsbBit") : vals.showAsEllipsis = False : vals.isMsb = True : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)
                    rwId += 1
                Else
                    vals.byte = rw.Item("PILsbByte") : vals.bit = rw.Item("PILsbBit") : vals.showAsEllipsis = False : vals.isMsb = False : vals.isLsb = True : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)
                    rwId += 1
                    If rw.Item("itemBitLengthNum") > 2 Then
                        Dim curBit As Integer = 0
                        Dim byteOffset As Integer = 0
                        curBit = rw.Item("PILsbBit") + 1
                        If curBit = 8 Then curBit = 0 : byteOffset = 1
                        vals.byte = rw.Item("PILsbByte") + byteOffset : vals.bit = curBit : vals.showAsEllipsis = False : vals.isMsb = False : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)
                        rwId += 1
                    End If

                    vals.byte = rw.Item("PIMsbByte") : vals.bit = rw.Item("PIMsbBit") : vals.showAsEllipsis = False : vals.isMsb = True : vals.isLsb = False : vals.rowId = rwId : ds.Tables("byteMapExt").Rows.Add(vals.getValArray)
                End If
                rwId += 1
                pdItemId += 1
            Next
        Next
        ds.Tables("processData").DefaultView.RowFilter = ""
        ds.Tables("byteMapExt").DefaultView.Sort = "pdId ASC"

        Dim dctPdIdTables As New Dictionary(Of String, String)
        Dim setts As New XmlWriterSettings : setts.Indent = True : setts.OmitXmlDeclaration = True
        Dim dv As New DataView(ds.Tables("byteMapExt"))
        For Each pdIdRw As DataRow In dv.ToTable(True, {"pdId", "conditionString"}).Rows
            ds.Tables("byteMapExt").DefaultView.Sort = "dir ASC, rowId DESC"
            ds.Tables("byteMapExt").DefaultView.RowFilter = "pdId = " & pdIdRw.Item("pdId")
            Dim htmlsw As New StringWriter : Dim htmlw As XmlWriter = XmlWriter.Create(htmlsw, setts) : htmlw.WriteStartElement("html") : htmlw.WriteStartElement("body")

            For Each drw As DataRow In ds.Tables("byteMapExt").DefaultView.ToTable(True, {"dir"}).Rows
                htmlw.WriteStartElement("table") : htmlw.WriteAttributeString("border", 1)


                ds.Tables("byteMapExt").DefaultView.RowFilter = "dir = '" & drw.Item("dir") & "' AND pdId = " & pdIdRw.Item("pdId")

                Dim curByte As Integer = -1
                Dim curPdItemId As Integer = -1
                Dim lastWasMsb As Boolean = False


                Dim dthtml As DataTable = ds.Tables("byteMapExt").DefaultView.ToTable()
                Dim colorScheme As colorSchemer
                colorScheme = IIf(dthtml.Rows(0).Item("dir").tolower = "in", New colorSchemer(colInput), New colorSchemer(colOutput))

                htmlw.WriteStartElement("caption")
                htmlw.WriteAttributeString("style", "background: " & colorScheme.getBaseColor)
                htmlw.WriteRaw(dthtml.Rows(0).Item("pdDir_Name_BC"))
                htmlw.WriteEndElement()

                htmlw.WriteStartElement("thead")
                htmlw.WriteStartElement("tr")
                htmlw.WriteAttributeString("style", "background: #f7f7f7")

                If masterPortSelected Then htmlw.WriteRaw(htmlTd(masterPort, , 2,, 5, True))
                htmlw.WriteRaw(htmlTd("IO-Link Port" & vbCrLf & "ProcessImage", 2, ,, 5, True))
                htmlw.WriteRaw(htmlTd("Value" & vbCrLf & "Bit",, 2,, 5, True))
                htmlw.WriteRaw(htmlTd("Name",, 2,, 5, True))
                htmlw.WriteRaw(htmlTd("ProcessImagePos",, 2,, 5, True))
                htmlw.WriteRaw(htmlTd("Datatype",, 2,, 5, True))
                htmlw.WriteRaw(htmlTd("Values",, 2,, 5, True))
                htmlw.WriteEndElement()
                htmlw.WriteStartElement("tr")
                htmlw.WriteAttributeString("style", "background: #f7f7f7")
                htmlw.WriteRaw(htmlTd("Byte",, ,, 5, True))
                htmlw.WriteRaw(htmlTd("Bit",, ,, 5, True))
                htmlw.WriteEndElement()
                htmlw.WriteEndElement()
                Dim pdColor As String = ""
                For Each rw As DataRow In dthtml.Rows
                    If rw.Item("byte") <> curByte Then
                        curByte = rw.Item("byte")
                        Dim cnt = dthtml.Select("byte = " & rw.Item("byte")).Count
                        htmlw.WriteStartElement("tr")


                        If masterPortSelected Then htmlw.WriteRaw(htmlTd(curByte - 1 + IIf(rw.Item("dir") = "In", InOffset, OutOffset),, cnt,, 10, True, colorScheme.getBaseColor))
                        htmlw.WriteRaw(htmlTd(curByte,, cnt,, 10, True, colorScheme.getBaseColor))
                    Else
                        htmlw.WriteStartElement("tr")

                    End If

                    If rw.Item("pdItemId") <> curPdItemId Then
                        pdColor = colorScheme.getColor
                    End If


                    If rw.Item("showAsEllipsis") Then
                        htmlw.WriteRaw(htmlTd("...",,, 2, 10, True, pdColor))
                    Else
                        htmlw.WriteRaw(htmlTd(rw.Item("bit"),,, 2,, True, pdColor))
                    End If

                    If rw.Item("isMsb") Then
                        htmlw.WriteRaw(htmlTd(rw.Item("pdBitLength") - 1 & " ~ MSB",,, 2, 10, True, pdColor))
                        lastWasMsb = True
                    Else
                        If rw.Item("isLsb") Then
                            htmlw.WriteRaw(htmlTd("0 ~ LSB",,, 2,, True, pdColor))
                        Else

                            Dim cnt = dthtml.Select("pdItemId = " & rw.Item("pdItemId")).Count
                            If cnt > 2 Then
                                If lastWasMsb Then
                                    htmlw.WriteRaw(htmlTd("...",, cnt - 2, 2, 10, True, pdColor))
                                End If
                            Else
                                htmlw.WriteRaw(htmlTd("",,, 2, 10, True, pdColor))
                            End If

                        End If
                        lastWasMsb = False
                    End If

                    If rw.Item("pdItemId") <> curPdItemId Then
                        curPdItemId = rw.Item("pdItemId")
                        Dim cnt = dthtml.Select("pdItemId = " & rw.Item("pdItemId")).Count
                        htmlw.WriteRaw(htmlTd(rw.Item("text"),, cnt, 2, 10, True, pdColor))
                        htmlw.WriteRaw(htmlTd(rw.Item("pdItemBo_Bl"),, cnt, 2, 10,, pdColor))
                        htmlw.WriteRaw(htmlTd(rw.Item("pdItemType"),, cnt, 2, 10, True, pdColor))
                        htmlw.WriteRaw(htmlTd(rw.Item("pdItemValues"),, cnt, 2, 10,, pdColor))
                    Else

                    End If
                    htmlw.WriteEndElement()
                Next

                htmlw.WriteEndElement()
            Next

            htmlw.Flush()

            dctPdIdTables.Add(pdIdRw.Item("conditionString"), htmlsw.ToString)
        Next

        Dim htmlsw2 As New StringWriter : Dim htmlw2 As XmlWriter = XmlWriter.Create(htmlsw2, setts)
        htmlw2.WriteStartElement("html")
        htmlw2.WriteStartElement("head")
        If masterPortSelected Then
            htmlw2.WriteElementString("title", "IODD_info_" & System.IO.Path.GetFileNameWithoutExtension(devInfo.filename) & "_" & tscbIolMasters.SelectedItem.manufacturer & "_" & tscbIolMasters.SelectedItem.model & "_" & tscbIolMasterPorts.SelectedItem.ToString)
        Else
            htmlw2.WriteElementString("title", "IODD_info_" & System.IO.Path.GetFileNameWithoutExtension(devInfo.filename))
        End If
        htmlw2.WriteStartElement("style")
        htmlw2.WriteRaw(cssStyling)
        htmlw2.WriteEndElement()
        htmlw2.WriteEndElement()
        htmlw2.WriteStartElement("body")


        htmlw2.WriteStartElement("details")
        htmlw2.WriteAttributeString("open", "")
        htmlw2.WriteElementString("summary", "Device Info")
        htmlw2.WriteRaw(generateDeviceInfoTable)
        htmlw2.WriteEndElement()

        If dctPdIdTables.Count > 1 Then
            htmlw2.WriteStartElement("div")
            htmlw2.WriteAttributeString("class", "roundedCorners")
            htmlw2.WriteRaw("<p>This device allows for different process data layouts based on the configuration of variable <b>" & conditionVarName & "</b>.<br>The default setting for this variable is <b>" & conditionVarDefault & ".</b></p><p>Select a configuration below to show the process data layout.</p>")
            htmlw2.WriteEndElement()
        End If

        htmlw2.WriteStartElement("details")
        htmlw2.WriteAttributeString("open", "")
        htmlw2.WriteAttributeString("class", "break-after")
        htmlw2.WriteElementString("summary", "Process Data")
        For Each pdTable As String In dctPdIdTables.Keys
            If dctPdIdTables.Count = 1 Then
                htmlw2.WriteRaw(dctPdIdTables(pdTable))
            Else
                htmlw2.WriteStartElement("details")
                htmlw2.WriteAttributeString("open", "")
                htmlw2.WriteAttributeString("class", "detailsSummaryAccentBgColor break-after")

                htmlw2.WriteStartElement("summary")
                htmlw2.WriteAttributeString("class", "detailsSummaryAccentBgColor")
                htmlw2.WriteRaw(pdTable)
                htmlw2.WriteEndElement()

                htmlw2.WriteRaw(dctPdIdTables(pdTable))
                htmlw2.WriteEndElement()
            End If
        Next
        htmlw2.WriteEndElement()

        htmlw2.WriteStartElement("details")
        htmlw2.WriteAttributeString("open", "")
        htmlw2.WriteElementString("summary", "IO-Link Config Variables")
        htmlw2.WriteRaw(generateVariableInfoTable)
        htmlw2.WriteEndElement()

        htmlw2.WriteEndElement() : htmlw2.WriteEndElement() : htmlw2.Flush()
        currentOutput = htmlsw2.ToString

        WebView21.NavigateToString(htmlsw2.ToString)

        ds.Tables("byteMapExt").DefaultView.RowFilter = ""
    End Sub

    Private Function generateDeviceInfoTable() As String
        Dim setts As New XmlWriterSettings : setts.Indent = True : setts.OmitXmlDeclaration = True
        Dim htmlsw As New StringWriter : Dim htmlw As XmlWriter = XmlWriter.Create(htmlsw, setts) : htmlw.WriteStartElement("html") : htmlw.WriteStartElement("body")

        htmlw.WriteStartElement("table") : htmlw.WriteAttributeString("border", 1) ': htmlw.WriteAttributeString("style", "border-collapse: collapse; font-family: sans-serif; font-size:12px;  padding: 10px;")
        htmlw.WriteStartElement("tr")
        htmlw.WriteRaw(htmlTd("Manufacturer and device info", 2,,, 5, True))
        htmlw.WriteRaw(htmlTd("<img src=data:image/png;base64," & devInfo.logo & "></img>", , 9,, 5, True))
        htmlw.WriteRaw(htmlTd("<img src=data:image/png;base64," & devInfo.deviceSymbol & "></img>", , 9,, 5, True))

        htmlw.WriteEndElement()
        htmlw.WriteRaw(getTrWithTds({"Filename", devInfo.filename}))
        htmlw.WriteRaw(getTrWithTds({"File version", devInfo.version}))
        htmlw.WriteRaw(getTrWithTds({"Release date", devInfo.releaseDate}))
        htmlw.WriteRaw(getTrWithTds({"Vendor name", devInfo.vendorName}))
        htmlw.WriteRaw(getTrWithTds({"Device name", devInfo.deviceName}))
        htmlw.WriteRaw(getTrWithTds({"Description", devInfo.description}))
        htmlw.WriteRaw(getTrWithTds({"VendorId", devInfo.vendorId}))
        htmlw.WriteRaw(getTrWithTds({"DeviceId", devInfo.deviceId}))
        htmlw.WriteEndElement()

        htmlw.Flush()
        Return htmlsw.ToString
    End Function

    Private Function generateVariableInfoTable() As String

        Dim colRw() As String = {"#e6fff3", "#fafff3"}
        Dim colRo() As String = {"#ffffe6", "#fffffa"}
        Dim cols As New colorSchemer({"#e6ffff", "#faffff"})

        Dim dtDst As DataTable = ds.Tables("variables").DefaultView.ToTable(True, {"index"})

        Dim setts As New XmlWriterSettings : setts.Indent = True : setts.OmitXmlDeclaration = True
        Dim htmlsw As New StringWriter : Dim htmlw As XmlWriter = XmlWriter.Create(htmlsw, setts) : htmlw.WriteStartElement("html") : htmlw.WriteStartElement("body")
        htmlw.WriteStartElement("table") : htmlw.WriteAttributeString("border", 1) ': htmlw.WriteAttributeString("style", "border-collapse: collapse; font-family: sans-serif; font-size:12px;  padding: 10px;")

        htmlw.WriteStartElement("thead")
        htmlw.WriteStartElement("tr")
        htmlw.WriteAttributeString("style", "background: #f7f7f7")
        htmlw.WriteRaw(htmlTd("Index",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("r/w",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("Name" & vbCrLf & "Description",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("SubIndex",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("BitOffset",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("Sub name",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("Type",, ,, 5, True))
        htmlw.WriteRaw(htmlTd("Values",, ,, 5, True))
        htmlw.WriteEndElement()
        htmlw.WriteEndElement()

        For Each rw As DataRow In dtDst.Rows
            ds.Tables("variables").DefaultView.RowFilter = "index = " & rw.Item("index")
            Dim lastAccessRights As String = ""

            Dim cnt As Integer = ds.Tables("variables").DefaultView.ToTable.Rows.Count
            Dim isFirst = True
            For Each drw As DataRow In ds.Tables("variables").DefaultView.ToTable.Rows
                If lastAccessRights <> drw.Item("accessRights") Then
                    lastAccessRights = drw.Item("accessRights")
                    cols = New colorSchemer(IIf(drw.Item("accessRights").toupper = "RW", colRw, colRo))
                End If
                htmlw.WriteStartElement("tr")
                If isFirst Then
                    htmlw.WriteRaw(htmlTd(drw.Item("index"),, cnt, 2, 5, True, cols.getBaseColor))
                    htmlw.WriteRaw(htmlTd(drw.Item("accessRights"),, cnt, 2, 5, True, cols.getBaseColor))
                    If (drw.Item("desc") <> "") And (drw.Item("desc") <> drw.Item("name")) Then
                        htmlw.WriteRaw(htmlTd(drw.Item("name") & vbCrLf & vbCrLf & drw.Item("desc"),, cnt, 2, 5, True, cols.getBaseColor))
                    Else
                        htmlw.WriteRaw(htmlTd(drw.Item("name"),, cnt, 2, 5, True, cols.getBaseColor))
                    End If

                    isFirst = False
                End If
                Dim col As String = cols.getColor
                htmlw.WriteRaw(htmlTd(drw.Item("subIndex"),,, 2, 5, True, col))
                htmlw.WriteRaw(htmlTd(drw.Item("bitOffset"),,, 2, 5, True, col))
                htmlw.WriteRaw(htmlTd(drw.Item("siName"),,, 2, 5, True, col))
                If drw.Item("bitLength") <> "" Then
                    htmlw.WriteRaw(htmlTd(drw.Item("type") & vbCrLf & "BitLength: " & drw.Item("bitLength"),,, 2, 5, True, col))
                Else
                    htmlw.WriteRaw(htmlTd(drw.Item("type"),,, 2, 5, True, col))
                End If
                htmlw.WriteRaw(htmlTd(drw.Item("values"),,, 2, 5, , col))
                htmlw.WriteEndElement()
            Next
        Next
        ds.Tables("variables").DefaultView.RowFilter = ""
        htmlw.Flush()
        Return htmlsw.ToString
    End Function

    Private Function getTrWithTds(cells() As String) As String
        Dim setts As New XmlWriterSettings : setts.Indent = True : setts.OmitXmlDeclaration = True
        Dim htmlsw As New StringWriter : Dim htmlw As XmlWriter = XmlWriter.Create(htmlsw, setts) : htmlw.WriteStartElement("html") : htmlw.WriteStartElement("body")
        htmlw.WriteStartElement("tr")
        For Each str As String In cells
            htmlw.WriteElementString("td", str)
        Next
        htmlw.WriteEndElement()
        htmlw.Flush()
        Return htmlsw.ToString
    End Function

    Private Sub tscbIolMasters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tscbIolMasters.SelectedIndexChanged
        tscbIolMasterPorts.Items.Clear()
        If CType(tscbIolMasters.SelectedItem, IOLMaster).ports.Count > 0 Then
            tscbIolMasterPorts.Items.AddRange(CType(tscbIolMasters.SelectedItem, IOLMaster).ports.ToArray)
            tscbIolMasterPorts.SelectedIndex = 0
        Else
            If Not IsNothing(ds) AndAlso ds.Tables.Contains("byteMapExt") Then getSvgByteMapExtended()
        End If

    End Sub

    Private Sub tscbIolMasterPorts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tscbIolMasterPorts.SelectedIndexChanged
        If Not IsNothing(ds) AndAlso ds.Tables.Contains("byteMapExt") Then getSvgByteMapExtended()
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
End Class

Public Class colorSchemer
    Private lst As List(Of String)
    Private idx As Integer = 0

    Public Sub New(clrs() As String)
        lst = New List(Of String)
        lst.AddRange(clrs)
    End Sub

    Public Function getColor() As String
        Dim col As String = lst(idx)
        If idx = lst.Count - 1 Then idx = 0 Else idx += 1
        Return col
    End Function

    Public Function getBaseColor() As String
        Return lst(0)
    End Function
End Class


Public Structure deviceInfo
    Public filename As String
    Public version As String
    Public releaseDate As String
    Public vendorId As String
    Public deviceId As String
    Public vendorName As String
    Public deviceName As String
    Public logoFile As String
    Public logo As String
    Public deviceSymbolFile As String
    Public deviceSymbol As String
    Public description As String

End Structure

Public Structure TypeInfo
    Public itemName As String
    Public itemBO As String
    Public itemBL As String
    Public itemType As String
    Public itemVals As String
    Public itemMin As String
    Public itemMax As String
End Structure

