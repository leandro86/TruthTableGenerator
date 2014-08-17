namespace TruthTableGenerator.Ui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.expressionInput = new System.Windows.Forms.TextBox();
            this.generateTruthTableButton = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.Panel();
            this.generateTreeButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // expressionInput
            // 
            this.expressionInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expressionInput.Location = new System.Drawing.Point(6, 33);
            this.expressionInput.Name = "expressionInput";
            this.expressionInput.Size = new System.Drawing.Size(389, 30);
            this.expressionInput.TabIndex = 0;
            this.expressionInput.TextChanged += new System.EventHandler(this.ExpressionInputTextChanged);
            this.expressionInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExpressionInputKeyDown);
            // 
            // generateTruthTableButton
            // 
            this.generateTruthTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateTruthTableButton.Enabled = false;
            this.generateTruthTableButton.Location = new System.Drawing.Point(401, 33);
            this.generateTruthTableButton.Name = "generateTruthTableButton";
            this.generateTruthTableButton.Size = new System.Drawing.Size(132, 30);
            this.generateTruthTableButton.TabIndex = 1;
            this.generateTruthTableButton.Text = "Generate Truth Table";
            this.generateTruthTableButton.UseVisualStyleBackColor = true;
            this.generateTruthTableButton.Click += new System.EventHandler(this.GenerateTruthTableButtonClick);
            // 
            // canvas
            // 
            this.canvas.AutoScroll = true;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(3, 16);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(695, 458);
            this.canvas.TabIndex = 2;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasPaint);
            // 
            // generateTreeButton
            // 
            this.generateTreeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateTreeButton.Enabled = false;
            this.generateTreeButton.Location = new System.Drawing.Point(539, 33);
            this.generateTreeButton.Name = "generateTreeButton";
            this.generateTreeButton.Size = new System.Drawing.Size(152, 30);
            this.generateTreeButton.TabIndex = 3;
            this.generateTreeButton.Text = "Generate Expression Tree";
            this.generateTreeButton.UseVisualStyleBackColor = true;
            this.generateTreeButton.Click += new System.EventHandler(this.GenerateTreeButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.expressionInput);
            this.groupBox1.Controls.Add(this.generateTreeButton);
            this.groupBox1.Controls.Add(this.generateTruthTableButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 82);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Expression";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.canvas);
            this.groupBox2.Location = new System.Drawing.Point(12, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(701, 477);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Truth Table";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 589);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Truth Table Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox expressionInput;
        private System.Windows.Forms.Button generateTruthTableButton;
        private System.Windows.Forms.Panel canvas;
        private System.Windows.Forms.Button generateTreeButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

