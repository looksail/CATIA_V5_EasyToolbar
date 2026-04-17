Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports INFITF
Imports HybridShapeTypeLib
Imports MECMOD
Imports NavigatorTypeLib
Imports PARTITF
Imports ProductStructureTypeLib
Imports SPATypeLib


<Guid("9E5E5F0B-8D1A-4B2C-8E3D-4F5A6B7C8D9E")>
<InterfaceType(ComInterfaceType.InterfaceIsDual)>
Public Interface IET_VBNETCOMDLL64
    Sub ShowActiveDocName(CATIA As Object)
    Sub CreateSimpleCube(ByRef CATIA As Object)

End Interface

<Guid("1A2B3C4D-5E6F-7A8B-9C0D-1E2F3A4B5C6D")>
<ClassInterface(ClassInterfaceType.None)>
<ProgId("ET_VBNETCOMDLL64.ET_VBNETCOMDLL64Class")>
Public Class ET_VBNETCOMDLL64Class
    Implements IET_VBNETCOMDLL64

    Public Sub ShowActiveDocName(CATIA As Object) Implements IET_VBNETCOMDLL64.ShowActiveDocName

        Dim catiaApp As INFITF.Application = CATIA
        MessageBox.Show(catiaApp.ActiveDocument.Name)

    End Sub

    Public Sub CreateSimpleCube(ByRef CATIA As Object) Implements IET_VBNETCOMDLL64.CreateSimpleCube
        If CATIA Is Nothing Then
            MessageBox.Show("VB.NET COMDLL64 CreateSimpleCube: Input CATIA parameter is null")
            Return
        End If

        Dim catiaApp As INFITF.Application = CType(CATIA, INFITF.Application)
        CreateSimpleCubeFunction(catiaApp, 20.0)
    End Sub

    Private Shared Sub CreateSimpleCubeFunction(ByRef catiaApp As INFITF.Application, ByVal cubeSideLength As Double)
        ' Step 0: Input validation
        If cubeSideLength <= 0 Then
            MessageBox.Show("CreateSimpleCube:Invalid cube side length! Must be greater than 0 mm.")
            Return
        End If

        ' Step 2: Create new Part document
        Dim documents As Documents = CType(catiaApp.Documents, Documents)
        If documents Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get CATIA Documents collection!")
            Return
        End If

        Dim partDoc As PartDocument = CType(documents.Add("Part"), PartDocument)
        If partDoc Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to create new Part document!")
            Return
        End If

        Dim part As Part = CType(partDoc.Part, Part)
        If part Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get Part from PartDocument!")
            Return
        End If

        ' Step 3: Get PartBody
        Dim bodies As Bodies = CType(part.Bodies, Bodies)
        If bodies Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get Bodies collection from Part!")
            Return
        End If

        Dim partBody As Body = Nothing
        Try
            partBody = CType(bodies.Item(1), Body)
        Catch ex As Exception
            MessageBox.Show("CreateSimpleCube:PartBody not found! Program exited.")
            Return
        End Try

        If partBody Is Nothing Then
            MessageBox.Show("CreateSimpleCube:PartBody is null! Program exited.")
            Return
        End If

        ' Step 4: Create sketch on XY Plane
        Dim originElements As OriginElements = CType(part.OriginElements, OriginElements)
        If originElements Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get OriginElements from Part!")
            Return
        End If

        Dim xyPlane As Plane = CType(originElements.PlaneXY, Plane)
        If xyPlane Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get XY Plane from origin elements!")
            Return
        End If

        ' Create Reference for XY Plane (required by Sketches.Add)
        Dim xyPlaneRef As INFITF.Reference = CType(part.CreateReferenceFromObject(xyPlane), INFITF.Reference)
        If xyPlaneRef Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to create Reference for XY Plane!")
            Return
        End If

        Dim sketches As Sketches = CType(partBody.Sketches, Sketches)
        If sketches Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get Sketches collection from PartBody!")
            Return
        End If

        Dim sketch As Sketch = CType(sketches.Add(xyPlaneRef), Sketch)
        If sketch Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to create sketch on XY Plane!")
            Return
        End If

        ' Step 5: Open sketch and draw square (dynamic size)
        part.InWorkObject = sketch
        Dim factory2D As Factory2D = CType(sketch.OpenEdition(), Factory2D)
        If factory2D Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to open sketch for edition!")
            Return
        End If

        Dim halfSide As Double = cubeSideLength / 2
        Dim line1 As Line2D = CType(factory2D.CreateLine(-halfSide, -halfSide, halfSide, -halfSide), Line2D)
        Dim line2 As Line2D = CType(factory2D.CreateLine(halfSide, -halfSide, halfSide, halfSide), Line2D)
        Dim line3 As Line2D = CType(factory2D.CreateLine(halfSide, halfSide, -halfSide, halfSide), Line2D)
        Dim line4 As Line2D = CType(factory2D.CreateLine(-halfSide, halfSide, -halfSide, -halfSide), Line2D)

        ' Step 6: Close sketch and create pad feature
        sketch.CloseEdition()
        Dim shapeFactory As ShapeFactory = CType(part.ShapeFactory, ShapeFactory)
        If shapeFactory Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to get ShapeFactory from Part!")
            Return
        End If

        Dim pad As Pad = CType(shapeFactory.AddNewPad(sketch, cubeSideLength), Pad)
        If pad Is Nothing Then
            MessageBox.Show("CreateSimpleCube:Failed to create Pad (extrusion) feature!")
            Return
        End If

        ' Step 7: Update part
        part.Update()

        ' Step 8: Adjust view
        Dim activeWindow As INFITF.Window = CType(catiaApp.ActiveWindow, INFITF.Window)
        If activeWindow IsNot Nothing Then
            Dim activeViewer As INFITF.Viewer3D = CType(activeWindow.ActiveViewer, INFITF.Viewer3D)
            If activeViewer IsNot Nothing Then
                activeViewer.Reframe()
                Dim viewpoint3D As INFITF.Viewpoint3D = CType(activeViewer.Viewpoint3D, INFITF.Viewpoint3D)
                Marshal.ReleaseComObject(viewpoint3D)
                Marshal.ReleaseComObject(activeViewer)
            End If
            Marshal.ReleaseComObject(activeWindow)
        End If

        ' Step 9: Success message
        MessageBox.Show($"{cubeSideLength}x{cubeSideLength}x{cubeSideLength}mm cube created successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Release all COM objects to avoid memory leaks
        Marshal.ReleaseComObject(pad)
        Marshal.ReleaseComObject(shapeFactory)
        Marshal.ReleaseComObject(line4)
        Marshal.ReleaseComObject(line3)
        Marshal.ReleaseComObject(line2)
        Marshal.ReleaseComObject(line1)
        Marshal.ReleaseComObject(factory2D)
        Marshal.ReleaseComObject(sketch)
        Marshal.ReleaseComObject(xyPlaneRef)
        Marshal.ReleaseComObject(xyPlane)
        Marshal.ReleaseComObject(originElements)
        Marshal.ReleaseComObject(partBody)
        Marshal.ReleaseComObject(bodies)
        Marshal.ReleaseComObject(part)
        Marshal.ReleaseComObject(partDoc)
        Marshal.ReleaseComObject(documents)
    End Sub
End Class