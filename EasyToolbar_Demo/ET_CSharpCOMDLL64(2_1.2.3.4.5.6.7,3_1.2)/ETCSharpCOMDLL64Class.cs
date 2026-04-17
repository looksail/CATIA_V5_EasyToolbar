
using CATIA_APP_ITF;
using ETCSharpCOMDLL;
using HybridShapeTypeLib;
using INFITF;
using MECMOD;
using NavigatorTypeLib;
using PARTITF;
using ProductStructureTypeLib;
using SPATypeLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ETCSharpCOMDLL64
{
    [Guid("7C8F2C4A-9876-4532-8B1E-9D876543210F")]
    [ComVisible(true)]
    public interface IETCSharpCOMDLL64
    {
        bool LoginPLM(ref object CATIA);
        void LogoutPLM(ref object CATIA);
        void ShowActiveDocName(ref object CATIA);
        void CreateSimpleCube(ref object CATIA);
        void CreateCylinder(ref object CATIA);
        void ShowAsmTree(ref object CATIA);
        void AddUserProperty(ref object CATIA);
        void RenamePartNumber(ref object CATIA);
        void OpenAndCLoseFile(ref object CATIA);

        //void SingleSelectEdgeFillet(ref object CATIA);
        //void MultiSelectEdgeFillet(ref object CATIA);
        //void CheckConflict(ref object CATIA);
    }

    [Guid("6B29FC40-CA47-1067-B31D-00DD010662DA")]
    [ProgId("ETCSharpCOMDLL64.ETCSharpCOMDLL64Class")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]

    public class ETCSharpCOMDLL64Class : IETCSharpCOMDLL64
    {
        public ETCSharpCOMDLL64Class() { }
        ~ETCSharpCOMDLL64Class() { }

        public bool LoginPLM(ref object CATIA)
        {
            MessageBox.Show("LoginPLM OK");
            return true;
        }
        public void LogoutPLM(ref object CATIA)
        {
        }
        public void ShowActiveDocName(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 ShowActiveDocName: Input CATIA parameter is null");
                return;
            }

            try
            {
                INFITF.Application catia = (INFITF.Application)CATIA;
                MessageBox.Show("C# COMDLL64 ShowActiveDocName \r\n\r\n " + catia.ActiveDocument.FullName);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 ShowActiveDocName \r\n\r\n CATIA has no open documents.\r\nError:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 ShowActiveDocName \r\n\r\n Error：" + ex.Message);
            }
        }
        public void CreateSimpleCube(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 CreateSimpleCube: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;

            CreateSimpleCubeFunction(ref catia, 20.0);
        }
        public void CreateCylinder(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 CreateCylinder: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;

            double diameter = 10;
            double height = 10;
            using (CylinderForm inputForm = new CylinderForm())
            {
                if (inputForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                diameter = inputForm.diameter;
                height = inputForm.height;
            }
            CreateCylinderFunction(ref catia, diameter, height);
        }
        public void ShowAsmTree(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 ShowAsmTree: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;

            using (ShowAsmTreeForm ShowForm = new ShowAsmTreeForm())
            {
                ShowForm.catia = catia;
                ShowForm.ShowDialog();
            }
        }
        public void AddUserProperty(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 AddUserProperty: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;

            string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
            if (extension != ".catproduct")
            {
                MessageBox.Show("Please switch to the Product document to add custom properties.");
                return;
            }

            ProductDocument prodDoc = catia.ActiveDocument as ProductDocument;
            if (prodDoc == null)
            {
                MessageBox.Show("An open Product document is required to perform this operation.");
                return;
            }
            Product product = prodDoc.Product;
            product.UserRefProperties.CreateString("TestUserProp", "TestUserValue");

            MessageBox.Show("Custom properties added. Right-click the root Product to view them.");
        }
        public void RenamePartNumber(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 Rename: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;

            string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
            if (extension != ".catproduct")
            {
                MessageBox.Show("Please switch to the Product document to rename.");
                return;
            }

            ProductDocument prodDoc = catia.ActiveDocument as ProductDocument;
            if (prodDoc == null)
            {
                MessageBox.Show("An open Product document is required to perform this operation.");
                return;
            }
            Product product = prodDoc.Product;

            // Rename the product component with "_New" suffix
            string newpartnumber = product.get_PartNumber() + "_New";
            product.set_PartNumber(newpartnumber); 
            product.Update();
        }
        public void OpenAndCLoseFile(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 ShowActiveDocName: Input CATIA parameter is null");
                return;
            }

            try
            {
                // 1. Connect to the running CATIA instance
                INFITF.Application catia = (INFITF.Application)CATIA;

                // 2. Add: Pop up file selection dialog to let user select file
                string filePath = string.Empty;
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Set dialog title
                    openFileDialog.Title = "Select CATIA File to Open";
                    // Set initial directory (can be changed to your common directory, e.g. Desktop)
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    // Set file filter (only show CATIA supported formats, *.* shows all files)
                    openFileDialog.Filter = "CATIA Files (*.CATPart;*.CATProduct;*.CATDrawing)|*.CATPart;*.CATProduct;*.CATDrawing|All Files (*.*)|*.*";
                    // Allow only single file selection
                    openFileDialog.Multiselect = false;
                    // Check if file exists (avoid selecting non-existent files)
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;

                    // Show dialog and check if user confirms selection
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Get the full path of the selected file
                        filePath = openFileDialog.FileName;
                        Console.WriteLine($"File selected by user: {filePath}");
                    }
                    else
                    {
                        // User cancelled selection, exit directly
                        return;
                    }
                }

                // 3. Open the file selected by user
                Documents documents = catia.Documents;
                Document catiaDoc = null;
                catiaDoc = documents.Open(filePath);           
                MessageBox.Show($"Successfully opened file: {catiaDoc.get_Name()}");

                // 4. Do something with the opened document

                // 5. Close the file (choose one of the two methods)
                //if (catiaDoc != null)
                //{
                //    catiaDoc.Close();
                //    MessageBox.Show("File closed successfully");
                //}
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 OpenAndCLoseFile COM Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 OpenAndCLoseFile \r\n\r\n Error：" + ex.Message);
            }
        }
        private void CreateCylinderFunction(ref INFITF.Application catia, double diameter,double height)
        {
            /* Create a cylinder (custom height and diameter) */
            // Add a new part named "Cylinder" to the current product

            string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
            if (extension != ".catproduct")
            {
                MessageBox.Show("Please switch to the Product document to create a Cylinder.");
                return;
            }

            ProductDocument prodDoc = catia.ActiveDocument as ProductDocument;
            if (prodDoc == null)
            {
                MessageBox.Show("An open Product document is required to create the cylinder");
                return;
            }

            // Validate parameters (diameter/height > 0 required)
            if (diameter <= 0)
            {
                MessageBox.Show("Cylinder diameter must be greater than 0 mm!");
                return;
            }
            if (height <= 0)
            {
                MessageBox.Show("Cylinder height must be greater than 0 mm!");
                return;
            }

            Product product = prodDoc.Product.Products.AddNewComponent("Part", "Cylinder");
            if (product == null)
            {
                MessageBox.Show("Failed to add new Cylinder part to Product!");
                return;
            }

            PartDocument document = catia.Documents.Item("Cylinder.CATPart") as PartDocument;
            if (document == null)
            {
                MessageBox.Show("Failed to get Cylinder.CATPart document!");
                return;
            }

            Part part = document.Part as Part;
            if (part == null)
            {
                MessageBox.Show("Failed to get Part from Cylinder.CATPart!");
                return;
            }

            // Step 1: Get PartBody step by step + multi-language compatibility + create Reference explicitly
            Bodies bodies = part.Bodies as Bodies;
            if (bodies == null)
            {
                MessageBox.Show("Failed to get Bodies collection from Part!");
                return;
            }

            Body partBody = null;
            try
            {
                partBody = bodies.Item(1) as Body;
            }
            catch
            {
                MessageBox.Show("Failed to get PartBody!");
                return;
            }
            if (partBody == null)
            {
                MessageBox.Show("PartBody is null!");
                return;
            }

            OriginElements originElements = part.OriginElements as OriginElements;
            if (originElements == null)
            {
                MessageBox.Show("Failed to get OriginElements from Part!");
                return;
            }
            Plane xyPlane = originElements.PlaneXY as Plane;
            if (xyPlane == null)
            {
                MessageBox.Show("Failed to get XY Plane from OriginElements!");
                return;
            }

            Reference xyPlaneRef = part.CreateReferenceFromObject(xyPlane) as Reference;
            if (xyPlaneRef == null)
            {
                MessageBox.Show("Failed to create Reference for XY Plane!");
                return;
            }

            Sketches sketches = partBody.Sketches as Sketches;
            if (sketches == null)
            {
                MessageBox.Show("Failed to get Sketches collection from PartBody!");
                return;
            }
            Sketch sketch = sketches.Add(xyPlaneRef) as Sketch;
            if (sketch == null)
            {
                MessageBox.Show("Failed to create Sketch on XY Plane!");
                return;
            }

            // Step 2: Skip Axis2D, open Sketch editing mode directly (core modification)
            part.InWorkObject = sketch;
            Factory2D factory = sketch.OpenEdition() as Factory2D;
            if (factory == null)
            {
                MessageBox.Show("Failed to open Sketch for edition!");
                return;
            }

            // Step 3: Draw a circle based on diameter (core modification: radius = diameter / 2)
            // Core: The default origin of CATIA Sketch is (0,0), calculate radius from diameter
            double radius = diameter / 2.0; 
            Circle2D circle = factory.CreateClosedCircle(0, 0, radius) as Circle2D;
            if (circle == null)
            {
                MessageBox.Show("Failed to create circle in Sketch!");
                return;
            }
            // No need to set Axis2D ReportName, it does not affect cylinder geometry creation

            // Step 4: Close Sketch and update part
            sketch.CloseEdition();
            part.Update();

            // Step 5: Create pad (extrusion) feature to set cylinder height
            ShapeFactory shapeFactory = part.ShapeFactory as ShapeFactory;
            if (shapeFactory == null)
            {
                MessageBox.Show("Failed to get ShapeFactory from Part!");
                return;
            }
            shapeFactory.AddNewPad(sketch, height);
            part.Update();

            MessageBox.Show($"Cylinder (height: {height}mm, diameter: {diameter}mm, radius: {radius}mm) created successfully!");
        }
        static void CreateSimpleCubeFunction(ref INFITF.Application catia, double cubeSideLength)
        {
            // Step 0: Input validation
            if (cubeSideLength <= 0)
            {
                MessageBox.Show("CreateSimpleCube:Invalid cube side length! Must be greater than 0 mm.");
                return;
            }

            //Application catia = null;
            //// Step 1: Get or create CATIA application instance
            //try
            //{
            //    catia = (Application)Marshal.GetActiveObject("CATIA.Application");
            //}
            //catch (COMException)
            //{
            //    Type catiaType = Type.GetTypeFromProgID("CATIA.Application");
            //    catia = (Application)Activator.CreateInstance(catiaType);
            //    catia.Visible = true;
            //}
            //if (catia == null)
            //{
            //    MessageBox.Show("CreateSimpleCube:Failed to start or connect to CATIA!");
            //    return;
            //}

            // Step 2: Create new Part document
            Documents documents = catia.Documents as Documents;
            if (documents == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get CATIA Documents collection!");
                return;
            }

            PartDocument partDoc = documents.Add("Part") as PartDocument;
            if (partDoc == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to create new Part document!");
                return;
            }

            Part part = partDoc.Part as Part;
            if (part == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get Part from PartDocument!");
                return;
            }

            // Step 3: Get PartBody
            Bodies bodies = part.Bodies as Bodies;
            if (bodies == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get Bodies collection from Part!");
                return;
            }

            Body partBody = null;
            try
            {
                partBody = bodies.Item(1) as Body;
            }
            catch (Exception)
            {
                MessageBox.Show("CreateSimpleCube:PartBody not found! Program exited.");
                return;
            }

            if (partBody == null)
            {
                MessageBox.Show("CreateSimpleCube:PartBody is null! Program exited.");
                return;
            }

            // Step 4: Create sketch on XY Plane (fixed CS0266 + CS1053)
            OriginElements originElements = part.OriginElements as OriginElements;
            if (originElements == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get OriginElements from Part!");
                return;
            }

            Plane xyPlane = originElements.PlaneXY as Plane;
            if (xyPlane == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get XY Plane from origin elements!");
                return;
            }

            // Create Reference for XY Plane (required by Sketches.Add)
            Reference xyPlaneRef = part.CreateReferenceFromObject(xyPlane) as Reference;
            if (xyPlaneRef == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to create Reference for XY Plane!");
                return;
            }

            Sketches sketches = partBody.Sketches as Sketches;
            if (sketches == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get Sketches collection from PartBody!");
                return;
            }

            Sketch sketch = sketches.Add(xyPlaneRef) as Sketch;
            if (sketch == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to create sketch on XY Plane!");
                return;
            }

            // Step 5: Open sketch and draw square (dynamic size)
            part.InWorkObject = sketch;
            Factory2D factory2D = sketch.OpenEdition() as Factory2D;
            if (factory2D == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to open sketch for edition!");
                return;
            }

            double halfSide = cubeSideLength / 2;
            Line2D line1 = factory2D.CreateLine(-halfSide, -halfSide, halfSide, -halfSide) as Line2D;
            Line2D line2 = factory2D.CreateLine(halfSide, -halfSide, halfSide, halfSide) as Line2D;
            Line2D line3 = factory2D.CreateLine(halfSide, halfSide, -halfSide, halfSide) as Line2D;
            Line2D line4 = factory2D.CreateLine(-halfSide, halfSide, -halfSide, -halfSide) as Line2D;

            // Step 6: Close sketch and create pad feature (fixed CS0266 for ShapeFactory)
            sketch.CloseEdition();
            ShapeFactory shapeFactory = part.ShapeFactory as ShapeFactory;
            if (shapeFactory == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to get ShapeFactory from Part!");
                return;
            }

            Pad pad = shapeFactory.AddNewPad(sketch, cubeSideLength) as Pad;
            if (pad == null)
            {
                MessageBox.Show("CreateSimpleCube:Failed to create Pad (extrusion) feature!");
                return;
            }

            // Step 7: Update part
            part.Update();

            // Step 8: Adjust view
            Window activeWindow = catia.ActiveWindow as Window;
            if (activeWindow != null)
            {
                Viewer3D activeViewer = activeWindow.ActiveViewer as Viewer3D;
                if (activeViewer != null)
                {
                    activeViewer.Reframe();
                    Viewpoint3D viewpoint3D = activeViewer.Viewpoint3D as Viewpoint3D;
                    Marshal.ReleaseComObject(viewpoint3D);
                    Marshal.ReleaseComObject(activeViewer);
                }
                Marshal.ReleaseComObject(activeWindow);
            }

            // Step 9: Success message
            MessageBox.Show($"{cubeSideLength}x{cubeSideLength}x{cubeSideLength}mm cube created successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Release all COM objects to avoid memory leaks
            Marshal.ReleaseComObject(pad);
            Marshal.ReleaseComObject(shapeFactory);
            Marshal.ReleaseComObject(line4);
            Marshal.ReleaseComObject(line3);
            Marshal.ReleaseComObject(line2);
            Marshal.ReleaseComObject(line1);
            Marshal.ReleaseComObject(factory2D);
            Marshal.ReleaseComObject(sketch);
            Marshal.ReleaseComObject(xyPlaneRef);
            Marshal.ReleaseComObject(xyPlane);
            Marshal.ReleaseComObject(originElements);
            Marshal.ReleaseComObject(partBody);
            Marshal.ReleaseComObject(bodies);
            Marshal.ReleaseComObject(part);
            Marshal.ReleaseComObject(partDoc);
            Marshal.ReleaseComObject(documents);
            //Marshal.ReleaseComObject(catia);
        }

        //This function has been moved to Project ET_CSharpEXE2. Testing has verified 
        //that CATIA's automation mechanism cannot execute interactive functions within the same process,
        //even though no issues were found when validating with ET_CSharpCOMDLL_Test alone.
        /*
        public void SingleSelectEdgeFillet(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 SelectEdgeFilletFunction: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;
            SingleSelectEdgeFilletFunction(ref catia);
        }
        public void MultiSelectEdgeFillet(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 SelectEdgeFilletFunction: Input CATIA parameter is null");
                return;
            }

            INFITF.Application catia = (INFITF.Application)CATIA;
            MultiSelectEdgeFilletFunction(ref catia);
        }
      
        private void SingleSelectEdgeFilletFunction(ref INFITF.Application catia)
        {
            PartDocument partDoc = null;
            Part part = null;
            Selection sel = null;
            ShapeFactory shapeFactory = null;
            double Radius = 5.0;

            try
            {
                string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
                if (extension != ".catpart")
                {
                    MessageBox.Show("Please switch to the Part document to fillet");
                    return;
                }

                partDoc = (PartDocument)catia.ActiveDocument;
                part = partDoc.Part;
                sel = partDoc.Selection;
                sel.Clear();

                object[] filter = new object[] { "Edge" };
                string status = sel.SelectElement2(filter, "Please select one edge(press Esc to cancel).", true);

                if (status == "Normal")
                {
                    Reference edgeRef = sel.Item(1).Reference;

                    ShapeFactory shapefactory = (ShapeFactory)part.ShapeFactory;
                    EdgeFillet edgeFillet = shapefactory.AddNewEdgeFilletWithConstantRadius(
                                       edgeRef, CatFilletEdgePropagation.catTangencyFilletEdgePropagation, Radius);

                    part.Update();
                    MessageBox.Show($"Success! Created fillet for 1 edge(s) (Radius: {Radius}).");
                }
                else
                {
                    MessageBox.Show("No edge selected or selection canceled.");
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 SelectEdgeFilletFunction \r\n\r\n COM Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 SelectEdgeFilletFunction \r\n\r\n Error：" + ex.Message);
            }
            finally
            {
                if (shapeFactory != null) Marshal.ReleaseComObject(shapeFactory);
                if (sel != null) Marshal.ReleaseComObject(sel);
                if (part != null) Marshal.ReleaseComObject(part);
                if (partDoc != null) Marshal.ReleaseComObject(partDoc);
            }
        }
        private void MultiSelectEdgeFilletFunction(ref INFITF.Application catia)
        {
            PartDocument partDoc = null;
            Part part = null;
            Selection sel = null;
            ShapeFactory shapeFactory = null;
            double Radius = 5.0;

            try
            {
                string extension = Path.GetExtension(catia.ActiveDocument.FullName).ToLower();
                if (extension != ".catpart")
                {
                    MessageBox.Show("Please switch to the Part document to fillet");
                    return;
                }

                partDoc = (PartDocument)catia.ActiveDocument;
                part = partDoc.Part;
                sel = partDoc.Selection;
                sel.Clear(); 

                object[] filter = new object[] { "Edge" };
                string status = sel.SelectElement3(filter, "Please select one or more edges (hold Ctrl to multi-select).",
                                                   true, CATMultiSelectionMode.CATMultiSelTriggWhenUserValidatesSelection, true);
                if (status == "Normal")
                {
                    int selectedCount = sel.Count;
                    if (selectedCount == 0)
                    {
                        MessageBox.Show("No edge selected or selection canceled.");
                        return;
                    }

                    shapeFactory = (ShapeFactory)part.ShapeFactory;

                    for (int i = 1; i <= selectedCount; i++) 
                    {
                        Reference edgeRef = sel.Item(i).Reference;

                        EdgeFillet edgeFillet = shapeFactory.AddNewEdgeFilletWithConstantRadius(
                            edgeRef,
                            CatFilletEdgePropagation.catTangencyFilletEdgePropagation,
                            Radius);
                    }

                    part.Update();
                    MessageBox.Show($"Success! Created fillet for {selectedCount} edge(s) (Radius: {Radius}).");
                }
                else
                {
                    MessageBox.Show("No edge selected or selection canceled.");
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 MultiSelectEdgeFilletFunction \r\n\r\n COM Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 MultiSelectEdgeFilletFunction \r\n\r\n Error：" + ex.Message);
            }
            finally
            {
                if (shapeFactory != null) Marshal.ReleaseComObject(shapeFactory);
                if (sel != null) Marshal.ReleaseComObject(sel);
                if (part != null) Marshal.ReleaseComObject(part);
                if (partDoc != null) Marshal.ReleaseComObject(partDoc);
            }
        }
        */

        //This function failed testing, with an error thrown at the following line of code:
        //Groups groups = prodDoc.Product.GetTechnologicalObject("Groups") as Groups;
        /*
        public void CheckConflict(ref object CATIA)
        {
            if (CATIA == null)
            {
                MessageBox.Show("C# COMDLL64 ShowActiveDocName: Input CATIA parameter is null");
                return;
            }

            try
            {
                INFITF.Application catia = (INFITF.Application)CATIA;

                Selection selection = catia.ActiveDocument.Selection;
                if (selection.Count != 2)
                {
                    MessageBox.Show("Please first hold Ctrl and select two Products from the specification tree on the left, then click this button to perform the interference check.");
                    return;
                }

                Product product1, product2;
                product1 = (Product)selection.Item2(1).Reference;
                product2 = (Product)selection.Item2(2).Reference;
                if (product1 == null || product2 == null)
                {
                    MessageBox.Show("You have not selected two valid Products.");
                    return;
                }

                int nCheck = ConflictCheckFunction(ref catia, product1, product2);
                if (nCheck > 0)
                {
                    MessageBox.Show($"There are {nCheck} conflicts between the two Products.");
                }
                else
                {
                    MessageBox.Show("No conflicts detected.");
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("C# COMDLL64 CheckConflict \r\n\r\nCOM Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("C# COMDLL64 CheckConflict \r\n\r\n Error：" + ex.Message);
            }
        }
        private int ConflictCheckFunction(ref INFITF.Application catia,Product product1, Product product2)
        {
            ProductDocument prodDoc = catia.ActiveDocument as ProductDocument;

            Groups groups = prodDoc.Product.GetTechnologicalObject("Groups") as Groups;
            Group first = groups.Add();
            Group second = groups.Add();
            first.AddExplicit(product1);
            second.AddExplicit(product2);

            Clash clash = (prodDoc.Product.GetTechnologicalObject("Clashes") as Clashes).Add();
            clash.ComputationType = CatClashComputationType.catClashComputationTypeBetweenTwo;
            clash.InterferenceType = CatClashInterferenceType.catClashInterferenceTypeContact;
            clash.FirstGroup = first;
            clash.SecondGroup = second;
            clash.Compute();
            Conflicts conflicts = clash.Conflicts;

            groups.Remove(first.get_Name());
            groups.Remove(second.get_Name());

            return conflicts.Count;
        }
        */
    }
}


