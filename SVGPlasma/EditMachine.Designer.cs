namespace SVGPlasma
{
    partial class EditMachine
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBeginCode = new System.Windows.Forms.TextBox();
            this.txtEndCode = new System.Windows.Forms.TextBox();
            this.txtPlasmaOnCode = new System.Windows.Forms.TextBox();
            this.txtPlasmaOffCode = new System.Windows.Forms.TextBox();
            this.txtCutWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCutWidth)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtBeginCode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtEndCode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtPlasmaOnCode, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPlasmaOffCode, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtCutWidth, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 403);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtName.Location = new System.Drawing.Point(127, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(452, 22);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 72);
            this.label2.TabIndex = 2;
            this.label2.Text = "Begin Code:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBeginCode
            // 
            this.txtBeginCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBeginCode.Location = new System.Drawing.Point(127, 31);
            this.txtBeginCode.MinimumSize = new System.Drawing.Size(100, 66);
            this.txtBeginCode.Multiline = true;
            this.txtBeginCode.Name = "txtBeginCode";
            this.txtBeginCode.Size = new System.Drawing.Size(452, 66);
            this.txtBeginCode.TabIndex = 3;
            // 
            // txtEndCode
            // 
            this.txtEndCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEndCode.Location = new System.Drawing.Point(127, 103);
            this.txtEndCode.MinimumSize = new System.Drawing.Size(100, 66);
            this.txtEndCode.Multiline = true;
            this.txtEndCode.Name = "txtEndCode";
            this.txtEndCode.Size = new System.Drawing.Size(452, 66);
            this.txtEndCode.TabIndex = 4;
            // 
            // txtPlasmaOnCode
            // 
            this.txtPlasmaOnCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPlasmaOnCode.Location = new System.Drawing.Point(127, 175);
            this.txtPlasmaOnCode.MinimumSize = new System.Drawing.Size(100, 66);
            this.txtPlasmaOnCode.Multiline = true;
            this.txtPlasmaOnCode.Name = "txtPlasmaOnCode";
            this.txtPlasmaOnCode.Size = new System.Drawing.Size(452, 66);
            this.txtPlasmaOnCode.TabIndex = 5;
            // 
            // txtPlasmaOffCode
            // 
            this.txtPlasmaOffCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPlasmaOffCode.Location = new System.Drawing.Point(127, 247);
            this.txtPlasmaOffCode.MinimumSize = new System.Drawing.Size(100, 66);
            this.txtPlasmaOffCode.Multiline = true;
            this.txtPlasmaOffCode.Name = "txtPlasmaOffCode";
            this.txtPlasmaOffCode.Size = new System.Drawing.Size(452, 66);
            this.txtPlasmaOffCode.TabIndex = 6;
            // 
            // txtCutWidth
            // 
            this.txtCutWidth.Location = new System.Drawing.Point(127, 319);
            this.txtCutWidth.Name = "txtCutWidth";
            this.txtCutWidth.Size = new System.Drawing.Size(120, 22);
            this.txtCutWidth.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 72);
            this.label3.TabIndex = 8;
            this.label3.Text = "End Code:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 72);
            this.label4.TabIndex = 9;
            this.label4.Text = "Plasma On Code:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 72);
            this.label5.TabIndex = 10;
            this.label5.Text = "Plasma Off Code:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 28);
            this.label6.TabIndex = 11;
            this.label6.Text = "Cut Width (mm):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cmdSave);
            this.flowLayoutPanel1.Controls.Add(this.cmdCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(127, 347);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(452, 53);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(3, 3);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 0;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(84, 3);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // EditMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 403);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "EditMachine";
            this.Text = "Edit Machine";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCutWidth)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBeginCode;
        private System.Windows.Forms.TextBox txtEndCode;
        private System.Windows.Forms.TextBox txtPlasmaOnCode;
        private System.Windows.Forms.TextBox txtPlasmaOffCode;
        private System.Windows.Forms.NumericUpDown txtCutWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdCancel;
    }
}