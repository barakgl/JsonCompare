using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace WindowsFormsApp2
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Json file comaparer";
        } 

        private void scottPlotUC1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///  select file 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse JSON Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "json files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
        /// <summary>
        ///     select file 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse JSON Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "json files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog2.FileName;
            }
        }

        /// <summary>
        ///     Take
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            /* check if not empty*/
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Must select files.");
                return;
            }
            String file1 = "";
            String file2 = "";
            try
            {
                 file1 = File.ReadAllText(textBox1.Text);
                 file2 = File.ReadAllText(textBox2.Text);
            }
            catch ( FileNotFoundException )
            {
                MessageBox.Show("Bad file. Select again.");
            }
 
            /* use api to deserialize and compare files */
            JsonCmp json = new JsonCmp();
            var json1 = json.DeserializeJson(file1);
            var json2 = json.DeserializeJson(file2);
            Boolean res = json.cmp(json1, json2);

            // Add a root node to assign the customer nodes to.
            TreeNode rootNode = new TreeNode();
            rootNode.Text = "best_path";
            // Add a main root treenode.
            treeView1.Nodes.Add(rootNode);
            int idx = 0;
            // Add a root treenode for each 'Customer' object in the ArrayList.
            foreach (var node in json1.best_path)
            {
                // Add a child treenode for each Order object.
                TreeNode[] myTreeNodeArray = new TreeNode[2];
                myTreeNodeArray[0] = new TreeNode($"id: {node.id.ToString()}");
                myTreeNodeArray[1] = new TreeNode($"index: {node.index.ToString()}");

                var err = node.getErrors();
                if (err[0] == true)
                {
                    myTreeNodeArray[0].ForeColor = Color.Red;
                }
                if (err[1] == true)
                {
                    myTreeNodeArray[1].ForeColor = Color.Red;
                }

                TreeNode bestPath = new TreeNode($"path #{idx}",
                  myTreeNodeArray);
                // Display the customer names with and Orange font.
                bestPath.ForeColor = Color.Orange;
                // Store the Customer object in the Tag property of the TreeNode.

                treeView1.Nodes[0].Nodes.Add(bestPath);
                idx++;
            }

            TreeNode rootNode2 = new TreeNode();
            rootNode2.Text = "nodes_list";
            // Add a main root treenode.
            treeView1.Nodes.Add(rootNode2);

            idx = 0;
            foreach (var node in json1.nodes_list)
            {
                // Add a child treenode for each Order object.
                TreeNode[] myTreeNodeArray = new TreeNode[3];
                myTreeNodeArray[0] = new TreeNode($"id: {node.id.ToString()}");
                myTreeNodeArray[1] = new TreeNode($"index: {node.index.ToString()}");
                myTreeNodeArray[2] = new TreeNode("edges: ");

                var err = node.getErrors();
                if (err[0] == true)
                {
                    myTreeNodeArray[0].ForeColor = Color.Red;
                }
                if (err[1] == true)
                {
                    myTreeNodeArray[1].ForeColor = Color.Red;
                }

                foreach (var edge in node.edges)
                {
                    if (edge==null)
                    {
                        myTreeNodeArray[2] = new TreeNode("[]");
                        break;
                    }
                    // Add a child treenode for each Order object.
                    TreeNode[] myTreeNodeArray2 = new TreeNode[4];
                    myTreeNodeArray2[0] = new TreeNode($"edge_0: {edge.edge_nodes[0].ToString()}");
                    myTreeNodeArray2[1] = new TreeNode($"edge_1: {edge.edge_nodes[1].ToString()}");
                    myTreeNodeArray2[2] = new TreeNode($"weight_1: {edge.weight_1.ToString()}");
                    myTreeNodeArray2[3] = new TreeNode($"weight_2: {edge.weight_2.ToString()}");

                    var edgeErr = edge.getErrors();
                    if (edgeErr[0] == true)
                    {
                        myTreeNodeArray2[0].ForeColor = Color.Red;
                    }
                    if (edgeErr[1] == true)
                    {
                        myTreeNodeArray2[1].ForeColor = Color.Red;
                    }
                    if (edgeErr[2] == true)
                    {
                        myTreeNodeArray2[2].ForeColor = Color.Red;
                    }
                    if (edgeErr[3] == true)
                    {
                        myTreeNodeArray2[3].ForeColor = Color.Red;
                    }

                    TreeNode customerEdgeNode = new TreeNode("edge",
                    myTreeNodeArray2);
                    // Display the customer names with and Orange font.
                    customerEdgeNode.ForeColor = Color.Orange;
                    // Store the Customer object in the Tag property of the TreeNode.

                    myTreeNodeArray[2] = customerEdgeNode;
                    
                }

                TreeNode customerNode = new TreeNode($"node #{idx}",
                  myTreeNodeArray);
                // Display the customer names with and Orange font.
                customerNode.ForeColor = Color.Orange;
                // Store the Customer object in the Tag property of the TreeNode.

                treeView1.Nodes[1].Nodes.Add(customerNode);
                idx++;
            }

            // Add a root node to assign the customer nodes to.
            TreeNode rootNode3 = new TreeNode();
            rootNode3.Text = "best_path";
            // Add a main root treenode.
            treeView2.Nodes.Add(rootNode3);
            idx = 0;
            // Add a root treenode for each 'Customer' object in the ArrayList.
            foreach (var node in json2.best_path)
            {
                // Add a child treenode for each Order object.
                TreeNode[] myTreeNodeArray = new TreeNode[2];
                myTreeNodeArray[0] = new TreeNode($"id: {node.id.ToString()}");
                myTreeNodeArray[1] = new TreeNode($"index: {node.index.ToString()}");

                var err = node.getErrors();
                if (err[0] == true)
                {
                    myTreeNodeArray[0].ForeColor = Color.Red;
                }
                if (err[1] == true)
                {
                    myTreeNodeArray[1].ForeColor = Color.Red;
                }

                TreeNode bestPath = new TreeNode($"path #{idx}",
                  myTreeNodeArray);
                // Display the customer names with and Orange font.
                bestPath.ForeColor = Color.Orange;
                // Store the Customer object in the Tag property of the TreeNode.

                treeView2.Nodes[0].Nodes.Add(bestPath);
                idx++;
            }

            TreeNode rootNode4 = new TreeNode();
            rootNode4.Text = "nodes_list";
            // Add a main root treenode.
            treeView2.Nodes.Add(rootNode4);

            idx = 0;
            foreach (var node in json2.nodes_list)
            {
                // Add a child treenode for each Order object.
                TreeNode[] myTreeNodeArray = new TreeNode[3];
                myTreeNodeArray[0] = new TreeNode($"id: {node.id.ToString()}");
                myTreeNodeArray[1] = new TreeNode($"index: {node.index.ToString()}");
                myTreeNodeArray[2] = new TreeNode("edges: ");

                var err = node.getErrors();
                if (err[0] == true)
                {
                    myTreeNodeArray[0].ForeColor = Color.Red;
                }
                if (err[1] == true)
                {
                    myTreeNodeArray[1].ForeColor = Color.Red;
                }

                foreach (var edge in node.edges)
                {
                    if (edge == null)
                    {
                        myTreeNodeArray[2] = new TreeNode("[]");
                        break;
                    }
                    // Add a child treenode for each Order object.
                    TreeNode[] myTreeNodeArray2 = new TreeNode[4];
                    myTreeNodeArray2[0] = new TreeNode($"edge_0: {edge.edge_nodes[0].ToString()}");
                    myTreeNodeArray2[1] = new TreeNode($"edge_1: {edge.edge_nodes[1].ToString()}");
                    myTreeNodeArray2[2] = new TreeNode($"weight_1: {edge.weight_1.ToString()}");
                    myTreeNodeArray2[3] = new TreeNode($"weight_2: {edge.weight_2.ToString()}");

                    var edgeErr = edge.getErrors();
                    if (edgeErr[0] == true)
                    {
                        myTreeNodeArray2[0].ForeColor = Color.Red;
                    }
                    if (edgeErr[1] == true)
                    {
                        myTreeNodeArray2[1].ForeColor = Color.Red;
                    }
                    if (edgeErr[2] == true)
                    {
                        myTreeNodeArray2[2].ForeColor = Color.Red;
                    }
                    if (edgeErr[3] == true)
                    {
                        myTreeNodeArray2[3].ForeColor = Color.Red;
                    }

                    TreeNode customerEdgeNode = new TreeNode("edge",
                    myTreeNodeArray2);
                    // Display the customer names with and Orange font.
                    customerEdgeNode.ForeColor = Color.Orange;
                    // Store the Customer object in the Tag property of the TreeNode.

                    myTreeNodeArray[2] = customerEdgeNode;

                }

                TreeNode customerNode = new TreeNode($"node #{idx}",
                  myTreeNodeArray);
                // Display the customer names with and Orange font.
                customerNode.ForeColor = Color.Orange;
                // Store the Customer object in the Tag property of the TreeNode.

                treeView2.Nodes[1].Nodes.Add(customerNode);
                idx++;
            }

            treeView1.ExpandAll();
            treeView2.ExpandAll();
            var sum = json.summ();
            String summery = "";
            if (sum[0] != 0 || sum[1] != 0 || sum[2] != 0)
                summery += "Files Do not match";
            summery = $"Number of id missmatches: {sum[0]}\nNumber of value missmatches: {sum[1]}\nNumber of missing elements: {sum[2]}";
            richTextBox1.Text = summery;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
        }
    }
}
