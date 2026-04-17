using CATIA_APP_ITF;
using HybridShapeTypeLib;
using INFITF;
using MECMOD;
using NavigatorTypeLib;
using PARTITF;
using ProductStructureTypeLib;
using SPATypeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETCSharpCOMDLL
{
    public partial class ShowAsmTreeForm : Form
    {
        public INFITF.Application catia;

        public ShowAsmTreeForm()
        {
            InitializeComponent();

            treeViewAssembly.ShowLines = true;
            treeViewAssembly.ShowPlusMinus = true;
            treeViewAssembly.ShowRootLines = true;
        }

        private void ShowAsmTreeForm_Load(object sender, EventArgs e)
        {
            if (catia != null)
            {
                ShowTree();
            }
        }

        private void ShowTree()
        {
            try
            {
                treeViewAssembly.Nodes.Clear();

                Document activeDoc = catia.ActiveDocument;
                ProductDocument productDoc = null;

                try
                {
                    productDoc = (ProductDocument)activeDoc;
                }
                catch (InvalidCastException)
                {
                    MessageBox.Show("Please open an assembly document (.CATProduct) in CATIA first!", "Prompt",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get root product
                Product rootProduct = productDoc.Product;

                // Create root node
                TreeNode rootNode = new TreeNode($"[{rootProduct.get_PartNumber()}] {rootProduct.get_Name()} (Root Assembly)");
                rootNode.Tag = rootProduct;
                treeViewAssembly.Nodes.Add(rootNode);

                // Recursively load sub-assemblies/parts
                LoadProductChildren(rootProduct, rootNode);

                // Expand root node
                rootNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fail：{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductChildren(Product parentProduct, TreeNode parentNode)
        {
            try
            {
                // Get child products collection (most stable property in all CATIA versions)
                Products childProducts = parentProduct.Products;

                // Iterate through all child components
                foreach (Product childProduct in childProducts)
                {
                    string nodeText = $"[{childProduct.get_PartNumber()}] {childProduct.get_Name()}";

                    // Core stable judgment logic (no use of IsLeaf/ReferenceProduct/Reference)
                    // Rule: If product has child products → Sub-assembly; Else → Part
                    if (childProduct.Products.Count > 0)
                    {
                        nodeText += " (Sub-assembly)";
                        // Recursively load child nodes for sub-assembly
                        TreeNode childNode = new TreeNode(nodeText);
                        childNode.Tag = childProduct;
                        parentNode.Nodes.Add(childNode);
                        LoadProductChildren(childProduct, childNode);
                    }
                    else
                    {
                        nodeText += " (Part)";
                        // Directly add part node (no recursion needed)
                        TreeNode childNode = new TreeNode(nodeText);
                        childNode.Tag = childProduct;
                        parentNode.Nodes.Add(childNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load child nodes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

