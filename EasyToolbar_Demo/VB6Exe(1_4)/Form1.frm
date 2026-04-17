VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3015
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   4560
   LinkTopic       =   "Form1"
   ScaleHeight     =   3015
   ScaleWidth      =   4560
   StartUpPosition =   3  '´°¿ÚÈ±Ê¡
   Begin VB.CommandButton CommandCreate 
      Caption         =   "CreateSimpleCube"
      Height          =   495
      Left            =   960
      TabIndex        =   0
      Top             =   1440
      Width           =   3015
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

' CATIA VBA Macro: Language-Agnostic Simple Cube (10x10x10mm)
' Final Fix: Avoid "IsGeometricalSet" (use PartBody = Bodies(1) - fixed rule for all CATIA V5)
' Compatibility: All CATIA V5 versions/languages (English/Chinese/French/German/R20-R32)
Sub CreateSimpleCube()
    ' Step 1: Initialize CATIA object
    Dim oCATIA As Object
    
    Set oCATIA = GetObject(, "CATIA.Application")
    If oCATIA Is Nothing Then
        Set oCATIA = CreateObject("CATIA.Application")
        oCATIA.Visible = True  ' Make CATIA window visible
    End If
    
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
    MsgBox "10x10x10mm cube created successfully!", vbInformation, "Success"
           
End Sub

Private Sub CommandCreate_Click()

Call CreateSimpleCube

End Sub
