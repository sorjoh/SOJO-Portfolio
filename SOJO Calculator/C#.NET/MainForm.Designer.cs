namespace SOJO_Calculator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnZero = new Button();
            btnDot = new Button();
            btnToggle = new Button();
            btnThree = new Button();
            btnTwo = new Button();
            btnOne = new Button();
            btnSix = new Button();
            btnFive = new Button();
            btnFour = new Button();
            btnNine = new Button();
            btnEight = new Button();
            btnSeven = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            btnDivide = new Button();
            btnMultiply = new Button();
            btnSubtract = new Button();
            btnAdd = new Button();
            btnEqual = new Button();
            btnReciprocal = new Button();
            btnSquare = new Button();
            btnSquareRoot = new Button();
            btnModulo = new Button();
            btnPercent = new Button();
            btmRnd = new Button();
            btnPi = new Button();
            btnCubeRoot = new Button();
            btnCube = new Button();
            btnPower = new Button();
            btnMemorySub = new Button();
            btnMemoryAdd = new Button();
            btnMemoryStore = new Button();
            btnMemoryRecall = new Button();
            btnMemoryClear = new Button();
            tblResult = new TableLayoutPanel();
            lblPreviousResult = new Label();
            lblCurrentResult = new Label();
            lblMemoryStatus = new Label();
            tblResult.SuspendLayout();
            SuspendLayout();
            // 
            // btnZero
            // 
            btnZero.BackColor = Color.Black;
            btnZero.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnZero.ForeColor = Color.White;
            btnZero.Location = new Point(3, 484);
            btnZero.Name = "btnZero";
            btnZero.Size = new Size(84, 54);
            btnZero.TabIndex = 0;
            btnZero.Text = "0";
            btnZero.UseVisualStyleBackColor = false;
            btnZero.Click += NumberButton_Click;
            // 
            // btnDot
            // 
            btnDot.BackColor = Color.Black;
            btnDot.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnDot.ForeColor = Color.White;
            btnDot.Location = new Point(91, 484);
            btnDot.Name = "btnDot";
            btnDot.Size = new Size(84, 54);
            btnDot.TabIndex = 1;
            btnDot.Text = ".";
            btnDot.UseVisualStyleBackColor = false;
            btnDot.Click += NumberButton_Click;
            // 
            // btnToggle
            // 
            btnToggle.BackColor = Color.Black;
            btnToggle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnToggle.ForeColor = Color.White;
            btnToggle.Location = new Point(179, 484);
            btnToggle.Name = "btnToggle";
            btnToggle.Size = new Size(84, 54);
            btnToggle.TabIndex = 2;
            btnToggle.Text = "+/-";
            btnToggle.UseVisualStyleBackColor = false;
            btnToggle.Click += btnToggle_Click;
            // 
            // btnThree
            // 
            btnThree.BackColor = Color.Black;
            btnThree.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnThree.ForeColor = Color.White;
            btnThree.Location = new Point(179, 426);
            btnThree.Name = "btnThree";
            btnThree.Size = new Size(84, 54);
            btnThree.TabIndex = 5;
            btnThree.Text = "3";
            btnThree.UseVisualStyleBackColor = false;
            btnThree.Click += NumberButton_Click;
            // 
            // btnTwo
            // 
            btnTwo.BackColor = Color.Black;
            btnTwo.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnTwo.ForeColor = Color.White;
            btnTwo.Location = new Point(91, 426);
            btnTwo.Name = "btnTwo";
            btnTwo.Size = new Size(84, 54);
            btnTwo.TabIndex = 4;
            btnTwo.Text = "2";
            btnTwo.UseVisualStyleBackColor = false;
            btnTwo.Click += NumberButton_Click;
            // 
            // btnOne
            // 
            btnOne.BackColor = Color.Black;
            btnOne.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnOne.ForeColor = Color.White;
            btnOne.Location = new Point(3, 426);
            btnOne.Name = "btnOne";
            btnOne.Size = new Size(84, 54);
            btnOne.TabIndex = 3;
            btnOne.Text = "1";
            btnOne.UseVisualStyleBackColor = false;
            btnOne.Click += NumberButton_Click;
            // 
            // btnSix
            // 
            btnSix.BackColor = Color.Black;
            btnSix.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnSix.ForeColor = Color.White;
            btnSix.Location = new Point(179, 368);
            btnSix.Name = "btnSix";
            btnSix.Size = new Size(84, 54);
            btnSix.TabIndex = 8;
            btnSix.Text = "6";
            btnSix.UseVisualStyleBackColor = false;
            btnSix.Click += NumberButton_Click;
            // 
            // btnFive
            // 
            btnFive.BackColor = Color.Black;
            btnFive.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnFive.ForeColor = Color.White;
            btnFive.Location = new Point(91, 368);
            btnFive.Name = "btnFive";
            btnFive.Size = new Size(84, 54);
            btnFive.TabIndex = 7;
            btnFive.Text = "5";
            btnFive.UseVisualStyleBackColor = false;
            btnFive.Click += NumberButton_Click;
            // 
            // btnFour
            // 
            btnFour.BackColor = Color.Black;
            btnFour.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnFour.ForeColor = Color.White;
            btnFour.Location = new Point(3, 368);
            btnFour.Name = "btnFour";
            btnFour.Size = new Size(84, 54);
            btnFour.TabIndex = 6;
            btnFour.Text = "4";
            btnFour.UseVisualStyleBackColor = false;
            btnFour.Click += NumberButton_Click;
            // 
            // btnNine
            // 
            btnNine.BackColor = Color.Black;
            btnNine.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnNine.ForeColor = Color.White;
            btnNine.Location = new Point(179, 310);
            btnNine.Name = "btnNine";
            btnNine.Size = new Size(84, 54);
            btnNine.TabIndex = 11;
            btnNine.Text = "9";
            btnNine.UseVisualStyleBackColor = false;
            btnNine.Click += NumberButton_Click;
            // 
            // btnEight
            // 
            btnEight.BackColor = Color.Black;
            btnEight.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnEight.ForeColor = Color.White;
            btnEight.Location = new Point(91, 310);
            btnEight.Name = "btnEight";
            btnEight.Size = new Size(84, 54);
            btnEight.TabIndex = 10;
            btnEight.Text = "8";
            btnEight.UseVisualStyleBackColor = false;
            btnEight.Click += NumberButton_Click;
            // 
            // btnSeven
            // 
            btnSeven.BackColor = Color.Black;
            btnSeven.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnSeven.ForeColor = Color.White;
            btnSeven.Location = new Point(3, 310);
            btnSeven.Name = "btnSeven";
            btnSeven.Size = new Size(84, 54);
            btnSeven.TabIndex = 9;
            btnSeven.Text = "7";
            btnSeven.UseVisualStyleBackColor = false;
            btnSeven.Click += NumberButton_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Blue;
            btnDelete.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(267, 310);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(84, 54);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "DEL";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.Blue;
            btnClear.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(355, 310);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(84, 54);
            btnClear.TabIndex = 13;
            btnClear.Text = "CE/C";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // btnDivide
            // 
            btnDivide.BackColor = Color.Gainsboro;
            btnDivide.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            btnDivide.ForeColor = Color.Black;
            btnDivide.Location = new Point(355, 368);
            btnDivide.Name = "btnDivide";
            btnDivide.Size = new Size(84, 54);
            btnDivide.TabIndex = 15;
            btnDivide.Text = "÷";
            btnDivide.UseVisualStyleBackColor = false;
            btnDivide.Click += OperatorButton_Click;
            // 
            // btnMultiply
            // 
            btnMultiply.BackColor = Color.Gainsboro;
            btnMultiply.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            btnMultiply.ForeColor = Color.Black;
            btnMultiply.Location = new Point(267, 368);
            btnMultiply.Name = "btnMultiply";
            btnMultiply.Size = new Size(84, 54);
            btnMultiply.TabIndex = 14;
            btnMultiply.Text = "×";
            btnMultiply.UseVisualStyleBackColor = false;
            btnMultiply.Click += OperatorButton_Click;
            // 
            // btnSubtract
            // 
            btnSubtract.BackColor = Color.Gainsboro;
            btnSubtract.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            btnSubtract.ForeColor = Color.Black;
            btnSubtract.Location = new Point(355, 426);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.Size = new Size(84, 54);
            btnSubtract.TabIndex = 17;
            btnSubtract.Text = "−";
            btnSubtract.UseVisualStyleBackColor = false;
            btnSubtract.Click += OperatorButton_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.Gainsboro;
            btnAdd.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            btnAdd.ForeColor = Color.Black;
            btnAdd.Location = new Point(267, 426);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(84, 54);
            btnAdd.TabIndex = 16;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += OperatorButton_Click;
            // 
            // btnEqual
            // 
            btnEqual.BackColor = Color.Silver;
            btnEqual.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            btnEqual.ForeColor = Color.Black;
            btnEqual.Location = new Point(267, 484);
            btnEqual.Name = "btnEqual";
            btnEqual.Size = new Size(173, 54);
            btnEqual.TabIndex = 18;
            btnEqual.Text = "=";
            btnEqual.UseVisualStyleBackColor = false;
            btnEqual.Click += btnEqual_Click;
            // 
            // btnReciprocal
            // 
            btnReciprocal.AllowDrop = true;
            btnReciprocal.BackColor = Color.Tan;
            btnReciprocal.Font = new Font("Segoe UI", 12F);
            btnReciprocal.ForeColor = Color.Black;
            btnReciprocal.Location = new Point(3, 268);
            btnReciprocal.Name = "btnReciprocal";
            btnReciprocal.Size = new Size(84, 38);
            btnReciprocal.TabIndex = 19;
            btnReciprocal.Text = "1/x";
            btnReciprocal.UseVisualStyleBackColor = false;
            btnReciprocal.Click += OperatorButton_Click;
            // 
            // btnSquare
            // 
            btnSquare.AllowDrop = true;
            btnSquare.BackColor = Color.Tan;
            btnSquare.Font = new Font("Segoe UI", 12F);
            btnSquare.ForeColor = Color.Black;
            btnSquare.Location = new Point(91, 268);
            btnSquare.Name = "btnSquare";
            btnSquare.Size = new Size(84, 38);
            btnSquare.TabIndex = 20;
            btnSquare.Text = "x²";
            btnSquare.UseVisualStyleBackColor = false;
            btnSquare.Click += OperatorButton_Click;
            // 
            // btnSquareRoot
            // 
            btnSquareRoot.AllowDrop = true;
            btnSquareRoot.BackColor = Color.Tan;
            btnSquareRoot.Font = new Font("Segoe UI", 12F);
            btnSquareRoot.ForeColor = Color.Black;
            btnSquareRoot.Location = new Point(179, 268);
            btnSquareRoot.Name = "btnSquareRoot";
            btnSquareRoot.Size = new Size(84, 38);
            btnSquareRoot.TabIndex = 21;
            btnSquareRoot.Text = "√";
            btnSquareRoot.UseVisualStyleBackColor = false;
            btnSquareRoot.Click += OperatorButton_Click;
            // 
            // btnModulo
            // 
            btnModulo.AllowDrop = true;
            btnModulo.BackColor = Color.Tan;
            btnModulo.Font = new Font("Segoe UI", 12F);
            btnModulo.ForeColor = Color.Black;
            btnModulo.Location = new Point(267, 268);
            btnModulo.Name = "btnModulo";
            btnModulo.Size = new Size(84, 38);
            btnModulo.TabIndex = 22;
            btnModulo.Text = "mod";
            btnModulo.UseVisualStyleBackColor = false;
            btnModulo.Click += OperatorButton_Click;
            // 
            // btnPercent
            // 
            btnPercent.AllowDrop = true;
            btnPercent.BackColor = Color.Tan;
            btnPercent.Font = new Font("Segoe UI", 12F);
            btnPercent.ForeColor = Color.Black;
            btnPercent.Location = new Point(355, 268);
            btnPercent.Name = "btnPercent";
            btnPercent.Size = new Size(84, 38);
            btnPercent.TabIndex = 23;
            btnPercent.Text = "%";
            btnPercent.UseVisualStyleBackColor = false;
            btnPercent.Click += OperatorButton_Click;
            // 
            // btmRnd
            // 
            btmRnd.AllowDrop = true;
            btmRnd.BackColor = Color.Tan;
            btmRnd.Font = new Font("Segoe UI", 12F);
            btmRnd.ForeColor = Color.Black;
            btmRnd.Location = new Point(355, 226);
            btmRnd.Name = "btmRnd";
            btmRnd.Size = new Size(84, 38);
            btmRnd.TabIndex = 28;
            btmRnd.Text = "rnd";
            btmRnd.UseVisualStyleBackColor = false;
            btmRnd.Click += btnRnd_Click;
            // 
            // btnPi
            // 
            btnPi.AllowDrop = true;
            btnPi.BackColor = Color.Tan;
            btnPi.Font = new Font("Segoe UI", 12F);
            btnPi.ForeColor = Color.Black;
            btnPi.Location = new Point(267, 226);
            btnPi.Name = "btnPi";
            btnPi.Size = new Size(84, 38);
            btnPi.TabIndex = 27;
            btnPi.Text = "π";
            btnPi.UseVisualStyleBackColor = false;
            btnPi.Click += btnPi_Click;
            // 
            // btnCubeRoot
            // 
            btnCubeRoot.AllowDrop = true;
            btnCubeRoot.BackColor = Color.Tan;
            btnCubeRoot.Font = new Font("Segoe UI", 12F);
            btnCubeRoot.ForeColor = Color.Black;
            btnCubeRoot.Location = new Point(179, 226);
            btnCubeRoot.Name = "btnCubeRoot";
            btnCubeRoot.Size = new Size(84, 38);
            btnCubeRoot.TabIndex = 26;
            btnCubeRoot.Text = "∛";
            btnCubeRoot.UseVisualStyleBackColor = false;
            btnCubeRoot.Click += OperatorButton_Click;
            // 
            // btnCube
            // 
            btnCube.AllowDrop = true;
            btnCube.BackColor = Color.Tan;
            btnCube.Font = new Font("Segoe UI", 12F);
            btnCube.ForeColor = Color.Black;
            btnCube.Location = new Point(91, 226);
            btnCube.Name = "btnCube";
            btnCube.Size = new Size(84, 38);
            btnCube.TabIndex = 25;
            btnCube.Text = "x³";
            btnCube.UseVisualStyleBackColor = false;
            btnCube.Click += OperatorButton_Click;
            // 
            // btnPower
            // 
            btnPower.AllowDrop = true;
            btnPower.BackColor = Color.Tan;
            btnPower.Font = new Font("Segoe UI", 12F);
            btnPower.ForeColor = Color.Black;
            btnPower.Location = new Point(3, 226);
            btnPower.Name = "btnPower";
            btnPower.Size = new Size(84, 38);
            btnPower.TabIndex = 24;
            btnPower.Text = "^";
            btnPower.UseVisualStyleBackColor = false;
            btnPower.Click += OperatorButton_Click;
            // 
            // btnMemorySub
            // 
            btnMemorySub.AllowDrop = true;
            btnMemorySub.BackColor = Color.Beige;
            btnMemorySub.Font = new Font("Segoe UI", 12F);
            btnMemorySub.ForeColor = Color.Black;
            btnMemorySub.Location = new Point(355, 184);
            btnMemorySub.Name = "btnMemorySub";
            btnMemorySub.Size = new Size(84, 38);
            btnMemorySub.TabIndex = 33;
            btnMemorySub.Text = "M-";
            btnMemorySub.UseVisualStyleBackColor = false;
            btnMemorySub.Click += MemoryButton_Click;
            // 
            // btnMemoryAdd
            // 
            btnMemoryAdd.AllowDrop = true;
            btnMemoryAdd.BackColor = Color.Beige;
            btnMemoryAdd.Font = new Font("Segoe UI", 12F);
            btnMemoryAdd.ForeColor = Color.Black;
            btnMemoryAdd.Location = new Point(267, 184);
            btnMemoryAdd.Name = "btnMemoryAdd";
            btnMemoryAdd.Size = new Size(84, 38);
            btnMemoryAdd.TabIndex = 32;
            btnMemoryAdd.Text = "M+";
            btnMemoryAdd.UseVisualStyleBackColor = false;
            btnMemoryAdd.Click += MemoryButton_Click;
            // 
            // btnMemoryStore
            // 
            btnMemoryStore.AllowDrop = true;
            btnMemoryStore.BackColor = Color.Beige;
            btnMemoryStore.Font = new Font("Segoe UI", 12F);
            btnMemoryStore.ForeColor = Color.Black;
            btnMemoryStore.Location = new Point(179, 184);
            btnMemoryStore.Name = "btnMemoryStore";
            btnMemoryStore.Size = new Size(84, 38);
            btnMemoryStore.TabIndex = 31;
            btnMemoryStore.Text = "MS";
            btnMemoryStore.UseVisualStyleBackColor = false;
            btnMemoryStore.Click += MemoryButton_Click;
            // 
            // btnMemoryRecall
            // 
            btnMemoryRecall.AllowDrop = true;
            btnMemoryRecall.BackColor = Color.Beige;
            btnMemoryRecall.Font = new Font("Segoe UI", 12F);
            btnMemoryRecall.ForeColor = Color.Black;
            btnMemoryRecall.Location = new Point(91, 184);
            btnMemoryRecall.Name = "btnMemoryRecall";
            btnMemoryRecall.Size = new Size(84, 38);
            btnMemoryRecall.TabIndex = 30;
            btnMemoryRecall.Text = "MR";
            btnMemoryRecall.UseVisualStyleBackColor = false;
            btnMemoryRecall.Click += MemoryButton_Click;
            // 
            // btnMemoryClear
            // 
            btnMemoryClear.AllowDrop = true;
            btnMemoryClear.BackColor = Color.Beige;
            btnMemoryClear.Font = new Font("Segoe UI", 12F);
            btnMemoryClear.ForeColor = Color.Black;
            btnMemoryClear.Location = new Point(3, 184);
            btnMemoryClear.Name = "btnMemoryClear";
            btnMemoryClear.Size = new Size(84, 38);
            btnMemoryClear.TabIndex = 29;
            btnMemoryClear.Text = "MC";
            btnMemoryClear.UseVisualStyleBackColor = false;
            btnMemoryClear.Click += MemoryButton_Click;
            // 
            // tblResult
            // 
            tblResult.ColumnCount = 1;
            tblResult.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblResult.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblResult.Controls.Add(lblPreviousResult, 0, 0);
            tblResult.Controls.Add(lblCurrentResult, 0, 1);
            tblResult.Location = new Point(3, 2);
            tblResult.Name = "tblResult";
            tblResult.RowCount = 2;
            tblResult.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblResult.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblResult.Size = new Size(436, 178);
            tblResult.TabIndex = 34;
            // 
            // lblPreviousResult
            // 
            lblPreviousResult.AutoSize = true;
            lblPreviousResult.BackColor = Color.Black;
            lblPreviousResult.Dock = DockStyle.Fill;
            lblPreviousResult.Font = new Font("Segoe UI Semibold", 21.75F, FontStyle.Bold);
            lblPreviousResult.ForeColor = Color.White;
            lblPreviousResult.Location = new Point(0, 0);
            lblPreviousResult.Margin = new Padding(0);
            lblPreviousResult.Name = "lblPreviousResult";
            lblPreviousResult.Size = new Size(436, 89);
            lblPreviousResult.TabIndex = 2;
            lblPreviousResult.TextAlign = ContentAlignment.BottomRight;
            // 
            // lblCurrentResult
            // 
            lblCurrentResult.AutoSize = true;
            lblCurrentResult.BackColor = Color.Black;
            lblCurrentResult.Dock = DockStyle.Fill;
            lblCurrentResult.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold);
            lblCurrentResult.ForeColor = Color.White;
            lblCurrentResult.Location = new Point(0, 89);
            lblCurrentResult.Margin = new Padding(0);
            lblCurrentResult.Name = "lblCurrentResult";
            lblCurrentResult.Size = new Size(436, 89);
            lblCurrentResult.TabIndex = 1;
            lblCurrentResult.Text = "0";
            lblCurrentResult.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblMemoryStatus
            // 
            lblMemoryStatus.AutoSize = true;
            lblMemoryStatus.BackColor = Color.Black;
            lblMemoryStatus.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold);
            lblMemoryStatus.ForeColor = Color.Red;
            lblMemoryStatus.Location = new Point(3, 3);
            lblMemoryStatus.Name = "lblMemoryStatus";
            lblMemoryStatus.Size = new Size(37, 31);
            lblMemoryStatus.TabIndex = 35;
            lblMemoryStatus.Text = "M";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(442, 542);
            Controls.Add(lblMemoryStatus);
            Controls.Add(tblResult);
            Controls.Add(btnMemorySub);
            Controls.Add(btnMemoryAdd);
            Controls.Add(btnMemoryStore);
            Controls.Add(btnMemoryRecall);
            Controls.Add(btnMemoryClear);
            Controls.Add(btmRnd);
            Controls.Add(btnPi);
            Controls.Add(btnCubeRoot);
            Controls.Add(btnCube);
            Controls.Add(btnPower);
            Controls.Add(btnPercent);
            Controls.Add(btnModulo);
            Controls.Add(btnSquareRoot);
            Controls.Add(btnSquare);
            Controls.Add(btnReciprocal);
            Controls.Add(btnEqual);
            Controls.Add(btnSubtract);
            Controls.Add(btnAdd);
            Controls.Add(btnDivide);
            Controls.Add(btnMultiply);
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnNine);
            Controls.Add(btnEight);
            Controls.Add(btnSeven);
            Controls.Add(btnSix);
            Controls.Add(btnFive);
            Controls.Add(btnFour);
            Controls.Add(btnThree);
            Controls.Add(btnTwo);
            Controls.Add(btnOne);
            Controls.Add(btnToggle);
            Controls.Add(btnDot);
            Controls.Add(btnZero);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            KeyPreview = true;
            Margin = new Padding(1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SOJO Calculator";
            Load += MainForm_Load;
            tblResult.ResumeLayout(false);
            tblResult.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnZero;
        private Button btnDot;
        private Button btnToggle;
        private Button btnThree;
        private Button btnTwo;
        private Button btnOne;
        private Button btnSix;
        private Button btnFive;
        private Button btnFour;
        private Button btnNine;
        private Button btnEight;
        private Button btnSeven;
        private Button btnDelete;
        private Button btnClear;
        private Button btnDivide;
        private Button btnMultiply;
        private Button btnSubtract;
        private Button btnAdd;
        private Button btnEqual;
        private Button btnReciprocal;
        internal Button btnSquare;
        internal Button btnSquareRoot;
        internal Button btnModulo;
        internal Button btnPercent;
        internal Button btmRnd;
        internal Button btnPi;
        internal Button btnCubeRoot;
        internal Button btnCube;
        private Button btnPower;
        internal Button btnMemorySub;
        internal Button btnMemoryAdd;
        internal Button btnMemoryStore;
        internal Button btnMemoryRecall;
        private Button btnMemoryClear;
        private TableLayoutPanel tblResult;
        internal Label lblCurrentResult;
        internal Label lblPreviousResult;
        private Label lblMemoryStatus;
    }
}
