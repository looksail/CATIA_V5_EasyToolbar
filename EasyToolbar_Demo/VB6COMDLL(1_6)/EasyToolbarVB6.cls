VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "EasyToolbarVB6Class"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True


'Option Explicit

Public Function EasyToolbarCOMDLLFunction(CATIA As Object)

    'Set CATIA = CreateObject("CATIA.Application")
    
    If CATIA Is Nothing Then
        MsgBox "VB6 COMDLL Recived CATIA Parameter Is Nothing"
        Return
    End If
    
     MsgBox "VB6 COMDLL Show CATIA ActiveDocumentName : " + CATIA.ActiveDocument.Name, vbInformation, "COMDLL"
       
    Call CreateSimpleCube(CATIA)
       
End Function


Sub CreateSimpleCube(oCATIA As Object)
    ' Step 1: Initialize CATIA object
    'Dim oCATIA As Object
    
    'Set oCATIA = GetObject(, "CATIA.Application")
    'If oCATIA Is Nothing Then
    ''    Set oCATIA = CreateObject("CATIA.Application")
    '    oCATIA.Visible = True  ' Make CATIA window visible
    'End If
    
    ' Step 2: Create new Part document (fixed API parameter)
    Dim oPartDoc As Object
    Set oPartDoc = oCATIA.Documents.Add("Part")
    Dim oPart As Object
    Set oPart = oPartDoc.part
    
    ' Step 3: Get PartBody (ULTIMATE FIX: PartBody = 1st item in Bodies collection)
    ' Rule: PartBody is ALWAYS the 1st element (index 1) in Bodies (1-based indexing)
    Dim oPartBody As Object
    On Error Resume Next ' Suppress error if Bodies is empty (extremely rare)
    Set oPartBody = oPart.Bodies.Item(1)
    On Error GoTo 0
    
    ' Critical: Check if PartBody was found (defensive check)
    If oPartBody Is Nothing Then
        MsgBox "PartBody not found! Macro exited.", vbCritical, "Error"
        Exit Sub
    End If
    
    ' Step 4: Create sketch on XY Plane (fixed internal property - no language dependency)
    Dim oSketch As Object
    Set oSketch = oPartBody.Sketches.Add(oPart.OriginElements.PlaneXY)
    
    ' Step 5: Open sketch & draw 10mm square (center at origin)
    oPart.InWorkObject = oSketch
    Dim oFactory2D As Object
    Set oFactory2D = oSketch.OpenEdition()
    
    ' Draw closed square (10mm side: -5,-5 to 5,5) - pure numeric logic (no strings)
    Dim oLine1 As Object, oLine2 As Object, oLine3 As Object, oLine4 As Object
    Set oLine1 = oFactory2D.CreateLine(-5, -5, 5, -5)  ' Bottom line
    Set oLine2 = oFactory2D.CreateLine(5, -5, 5, 5)    ' Right line
    Set oLine3 = oFactory2D.CreateLine(5, 5, -5, 5)    ' Top line
    Set oLine4 = oFactory2D.CreateLine(-5, 5, -5, -5)  ' Left line
    
    ' Step 6: Close sketch & extrude to 10mm cube (pad feature)
    oSketch.CloseEdition
    Dim oPad As Object
    Set oPad = oPart.ShapeFactory.AddNewPad(oSketch, 10) ' 10mm extrusion height
    
    ' Step 7: Update part to apply all changes
    oPart.Update
    
    ' Step 8
    Set specsAndGeomWindow1 = oCATIA.ActiveWindow
    Set viewer3D1 = specsAndGeomWindow1.ActiveViewer
    viewer3D1.Reframe
    Set viewpoint3D1 = viewer3D1.Viewpoint3D
    
    ' Step 9 Success message
    MsgBox "10x10x10mm cube created successfully!", vbInformation, "COMDLL Success"
           
End Sub

