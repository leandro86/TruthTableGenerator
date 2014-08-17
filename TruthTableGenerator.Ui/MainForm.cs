using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TruthTableGenerator.Logic;
using TruthTableGenerator.Logic.ExpressionParser;
using TruthTableGenerator.Logic.ExpressionParser.Exceptions;

namespace TruthTableGenerator.Ui
{
    public partial class MainForm : Form
    {
        private LogicalExpression _logicalExpression;
        private readonly ToolTip _errorTooltip;

        private const int TruthTableOffsetX = 70;
        private const int TruthTableOffsetY = 20;

        private const int SpaceBetweenTokens = 10;
        private const int SpaceBetweenImplications = 30;
        private const int SpaceBetweenRows = 30;

        private const string FontName = "Tahoma";
        private const int FontSize = 13;

        public MainForm()
        {
            InitializeComponent();

            _errorTooltip = new ToolTip
            {
                UseFading = true,
                UseAnimation = true,
                ToolTipIcon = ToolTipIcon.Error,
                AutoPopDelay = 5000,
                InitialDelay = 1000,
                ReshowDelay = 500,
            };
        }

        private void GenerateTruthTableButtonClick(object sender, EventArgs e)
        {
            GenerateTruthTable();
        }

        private void GenerateTruthTable()
        {
            if (string.IsNullOrEmpty(expressionInput.Text))
            {
                return;
            }

            _errorTooltip.Hide(expressionInput);

            try
            {
                _logicalExpression = new LogicalExpression(expressionInput.Text);
                canvas.Invalidate();
            }
            catch (ParsingException ex)
            {
                string expectedTokens = "";

                string delimiter = "";
                foreach (TokenType t in ex.ExpectedTokenTypes)
                {
                    expectedTokens += delimiter + t.ToString();
                    delimiter = ", ";
                }

                string toolTipCaption = "";
                string msg = "";

                switch (ex.ExceptionType)
                {
                    case ParserExceptionType.UnknownToken:
                    {
                        toolTipCaption = "Unknown token";
                        msg = string.Format("Expected: {0}.", expectedTokens);
                        break;
                    }
                    case ParserExceptionType.ExpectedToken:
                    {
                        toolTipCaption = "Unexpected token";
                        msg = string.Format("Expected: {0}.", expectedTokens);
                        break;
                    }
                    case ParserExceptionType.IllegalInitialToken:
                    {
                        toolTipCaption = "Invalid initial token";
                        msg = string.Format("An expression must start with: {0}.", expectedTokens);
                        break;
                    }
                    case ParserExceptionType.IllegalEndToken:
                    {
                        toolTipCaption = "Invalid end token";
                        msg = string.Format("Expected: {0}.", expectedTokens);
                        break;
                    }
                    case ParserExceptionType.MissingOpenBracket:
                    {
                        toolTipCaption = "Matching bracket error";
                        msg = string.Format("Missing open bracket.");
                        break;
                    }
                    case ParserExceptionType.MissingCloseBracket:
                    {
                        toolTipCaption = "Matching bracket error";
                        msg = string.Format("Missing close bracket.");
                        break;
                    }
                }

                HighlightExpressionError(ex.Token.Position);
                ShowErrorToolTip(toolTipCaption, msg);
            }
        }

        private void HighlightExpressionError(int position)
        {
            expressionInput.Focus();
            expressionInput.Select(position, 1);
        }

        private void ShowErrorToolTip(string title, string message)
        {
            _errorTooltip.ToolTipTitle = title;
            _errorTooltip.Show(message, expressionInput, 0, 37, 7000);
        }

        private void CanvasPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.ResetTransform();
            e.Graphics.Clear(canvas.BackColor);

            e.Graphics.TranslateTransform(canvas.AutoScrollPosition.X, canvas.AutoScrollPosition.Y);

            if (_logicalExpression != null)
            {
                Tuple<int, int> endTablePos = DrawTruthTable(e.Graphics);
                DrawLinesForTable(e.Graphics, endTablePos.Item1, endTablePos.Item2);

                canvas.AutoScrollMinSize = new Size(endTablePos.Item1 + 50, endTablePos.Item2 + 40);
            }
        }

        private Tuple<int, int> DrawTruthTable(Graphics g)
        {
            int x = TruthTableOffsetX - SpaceBetweenTokens + 10;
            int y;
            int[] drawingPositionsX = new int[_logicalExpression.Expression.Length];
            var tokens = _logicalExpression.GetInfixTokens().ToArray();

            using (Font font = new Font(FontName, FontSize))
            {
                var previousTokenType = tokens[0].Item1;

                foreach (var token in tokens)
                {
                    TokenType tokenType = token.Item1;
                    char tokenValue = token.Item2;
                    int tokenPosition = token.Item3;

                    if (tokenType == TokenType.Implication || tokenType == TokenType.Conjunction || tokenType == TokenType.Disjunction ||
                        previousTokenType == TokenType.Implication || previousTokenType == TokenType.Conjunction || 
                        previousTokenType == TokenType.Disjunction)
                    {
                        x += SpaceBetweenImplications;
                    }
                    else
                    {
                        x += SpaceBetweenTokens;
                    }

                    g.DrawString(tokenValue.ToString(), font, Brushes.Blue, x, TruthTableOffsetY);
                    drawingPositionsX[tokenPosition] = x;

                    previousTokenType = tokenType;
                }

                y = TruthTableOffsetY;

                for (int row = 0; row < _logicalExpression.TruthTable.Length; row++)
                {
                    string result = _logicalExpression.TruthTable[row];

                    y += SpaceBetweenRows;
                    for (int i = 0; i < result.Length; i++)
                    {
                        Brush brush = i == _logicalExpression.TruthTable.ResultPosition ? Brushes.Red : Brushes.Black;
                        g.DrawString(result[i].ToString(), font, brush, drawingPositionsX[i], y);
                    }
                }
            }

            // Returns position of last column and row of the truth table in order to know how long the truth table's lines have to be.
            return new Tuple<int, int>(x, y);
        }

        private void DrawLinesForTable(Graphics g, int lastColumnPosition, int lastRowPosition)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                // Draw vertical line
                g.DrawLine(pen,
                           TruthTableOffsetX,
                           TruthTableOffsetY,
                           TruthTableOffsetX,
                           TruthTableOffsetY + lastRowPosition);

                // Draw horizontal line
                g.DrawLine(pen,
                           TruthTableOffsetX - 65,
                           TruthTableOffsetY + SpaceBetweenRows,
                           TruthTableOffsetX - 40 + lastColumnPosition,
                           TruthTableOffsetY + SpaceBetweenRows);
            }

            // Draw row number
            using (Font font = new Font(FontName, FontSize, FontStyle.Bold))
            {
                for (int i = 0; i < _logicalExpression.TruthTable.Length; i++)
                {
                    g.DrawString(string.Format("{0, 2}", i + 1),
                                 font,
                                 Brushes.DarkGray,
                                 TruthTableOffsetX - 50,
                                 TruthTableOffsetY + SpaceBetweenRows + (i * SpaceBetweenRows));
                }
            }
        }

        private void GenerateTreeButtonClick(object sender, EventArgs e)
        {
            BinaryNode rootNode = _logicalExpression.GenerateExpressionTree();
            new BinaryTreeForm(rootNode).ShowDialog();
        }

        private void ExpressionInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && expressionInput.Text != "")
            {
                GenerateTruthTable();
            }
        }

        private void ExpressionInputTextChanged(object sender, EventArgs e)
        {
            generateTruthTableButton.Enabled = expressionInput.Text != "";
            generateTreeButton.Enabled = generateTruthTableButton.Enabled;
        }
    }
}