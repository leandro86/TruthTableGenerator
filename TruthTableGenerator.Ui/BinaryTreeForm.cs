using System.Drawing;
using System.Windows.Forms;
using TruthTableGenerator.Logic;
using TruthTableGenerator.Logic.ExpressionParser;

namespace TruthTableGenerator.Ui
{
    public partial class BinaryTreeForm : Form
    {
        private readonly BinaryNode _rootNode;

        private const int CircleDiameter = 30;
        private const string FontName = "Verdana";
        private const int NodesFontSize = 15;

        public BinaryTreeForm(BinaryNode rootNode)
        {
            InitializeComponent();

            _rootNode = rootNode;
        }

        private void CanvasPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(canvas.BackColor);

            int screenCenterX = canvas.Width / 2 - CircleDiameter / 2;
            DrawBinaryNode(_rootNode, screenCenterX, 25, 200, 75, e.Graphics);
        }

        private void DrawBinaryNode(BinaryNode node, int x, int y, int widthBetweenNodes, int heightBetweenNodes, Graphics g)
        {
            int newNodeY = y + heightBetweenNodes;
            int newLineX = x + 12;
            int newLineY = y + 5;
            int newHeightBetweenNodes = y + 5 + heightBetweenNodes;
            int newWidthBetweenNodes = (int)(widthBetweenNodes * 0.55);

            if (node.Left != null)
            {
                g.DrawLine(Pens.Green, newLineX, newLineY, x + 12 - widthBetweenNodes, newHeightBetweenNodes);
                DrawBinaryNode(node.Left, x - widthBetweenNodes, newNodeY, newWidthBetweenNodes, heightBetweenNodes, g);
            }

            if (node.Right != null)
            {
                g.DrawLine(Pens.Green, newLineX, newLineY, x + 12 + widthBetweenNodes, newHeightBetweenNodes);
                DrawBinaryNode(node.Right, x + widthBetweenNodes, newNodeY, newWidthBetweenNodes, heightBetweenNodes, g);
            }
            
            g.FillEllipse(Brushes.DarkSeaGreen, x, y, CircleDiameter, CircleDiameter);

            using (Font font = new Font(FontName, NodesFontSize, FontStyle.Bold))
            {
                g.DrawString(node.Value.ToString(), font, Brushes.Yellow, x + 5, y);
            }
        }
    }
}