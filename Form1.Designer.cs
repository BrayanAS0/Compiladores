namespace Compliadores_Form
{
    partial class Form1
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
            dataGridView1 = new DataGridView();
            Position = new DataGridViewTextBoxColumn();
            Token = new DataGridViewTextBoxColumn();
            Type = new DataGridViewTextBoxColumn();
            textBox1 = new TextBox();
            button1 = new Button();
            label2 = new Label();
            textBox2 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Position, Token, Type });
            dataGridView1.GridColor = SystemColors.ActiveBorder;
            dataGridView1.Location = new Point(36, 342);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(521, 381);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Position
            // 
            Position.HeaderText = "Position";
            Position.MinimumWidth = 8;
            Position.Name = "Position";
            Position.Width = 150;
            // 
            // Token
            // 
            Token.HeaderText = "Token";
            Token.MinimumWidth = 8;
            Token.Name = "Token";
            Token.Width = 150;
            // 
            // Type
            // 
            Type.HeaderText = "Type";
            Type.MinimumWidth = 8;
            Type.Name = "Type";
            Type.Width = 150;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(36, 40);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(521, 271);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(594, 136);
            button1.Name = "button1";
            button1.Size = new Size(196, 34);
            button1.TabIndex = 3;
            button1.Text = "Abrir archivo";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(761, 57);
            label2.Name = "label2";
            label2.Size = new Size(139, 25);
            label2.TabIndex = 4;
            label2.Text = "Ruta del archivo";
            label2.Click += label2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(584, 85);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(467, 31);
            textBox2.TabIndex = 5;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // button2
            // 
            button2.Location = new Point(819, 138);
            button2.Name = "button2";
            button2.Size = new Size(220, 34);
            button2.TabIndex = 6;
            button2.Text = "Guardar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(683, 386);
            button3.Name = "button3";
            button3.Size = new Size(254, 186);
            button3.TabIndex = 7;
            button3.Text = "Analizar";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1063, 762);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Position;
        private DataGridViewTextBoxColumn Token;
        private DataGridViewTextBoxColumn Type;
        private TextBox textBox1;
        private Button button1;
        private Label label2;
        private TextBox textBox2;
        private Button button2;
        private Button button3;
    }
}
