
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TypesSelector
{
    partial class TypesSelectorForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypesSelectorForm));
            this.exitButton = new System.Windows.Forms.Button();
            this.unitTypeIdentifierGroupBox = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelTypeIdentifierGroupBox = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.cancelButton = new System.Windows.Forms.Button();
            this.allUTIButton = new System.Windows.Forms.Button();
            this.allPTIButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.unitTypeIdentifierGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelTypeIdentifierGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(356, 733);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(79, 46);
            this.exitButton.TabIndex = 0;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // unitTypeIdentifierGroupBox
            // 
            this.unitTypeIdentifierGroupBox.Controls.Add(this.dataGridView1);
            this.unitTypeIdentifierGroupBox.Location = new System.Drawing.Point(464, 18);
            this.unitTypeIdentifierGroupBox.Name = "unitTypeIdentifierGroupBox";
            this.unitTypeIdentifierGroupBox.Size = new System.Drawing.Size(378, 783);
            this.unitTypeIdentifierGroupBox.TabIndex = 1;
            this.unitTypeIdentifierGroupBox.TabStop = false;
            this.unitTypeIdentifierGroupBox.Text = "Elenco degli Unit Identifier";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(30, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(321, 727);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_selectedRowsButton_Click);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // panelTypeIdentifierGroupBox
            // 
            this.panelTypeIdentifierGroupBox.Controls.Add(this.dataGridView2);
            this.panelTypeIdentifierGroupBox.Location = new System.Drawing.Point(18, 18);
            this.panelTypeIdentifierGroupBox.Name = "panelTypeIdentifierGroupBox";
            this.panelTypeIdentifierGroupBox.Size = new System.Drawing.Size(417, 370);
            this.panelTypeIdentifierGroupBox.TabIndex = 2;
            this.panelTypeIdentifierGroupBox.TabStop = false;
            this.panelTypeIdentifierGroupBox.Text = "Elenco dei Panel Type Identifier";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToOrderColumns = true;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.GridColor = System.Drawing.Color.Black;
            this.dataGridView2.Location = new System.Drawing.Point(29, 40);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(360, 296);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_ColumnHeaderMouseClick);
            this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView2_DataBindingComplete);
            this.dataGridView2.Click += new System.EventHandler(this.dataGridView2_selectedRowsButton_Click);
            this.dataGridView2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView2_KeyUp);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.cancelButton.Location = new System.Drawing.Point(140, 130);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(127, 62);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancella le selezioni";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // allUTIButton
            // 
            this.allUTIButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.allUTIButton.Location = new System.Drawing.Point(240, 40);
            this.allUTIButton.Name = "allUTIButton";
            this.allUTIButton.Size = new System.Drawing.Size(127, 62);
            this.allUTIButton.TabIndex = 4;
            this.allUTIButton.Text = "Colora tutti gli Unit Type Identifier";
            this.allUTIButton.UseVisualStyleBackColor = false;
            this.allUTIButton.Click += new System.EventHandler(this.allUTIButton_Click);
            // 
            // allPTIButton
            // 
            this.allPTIButton.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.allPTIButton.Location = new System.Drawing.Point(45, 40);
            this.allPTIButton.Name = "allPTIButton";
            this.allPTIButton.Size = new System.Drawing.Size(127, 62);
            this.allPTIButton.TabIndex = 5;
            this.allPTIButton.Text = "Colora tutti i Panel Type Identifier";
            this.allPTIButton.UseVisualStyleBackColor = false;
            this.allPTIButton.Click += new System.EventHandler(this.allPTIButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.allUTIButton);
            this.groupBox2.Controls.Add(this.allPTIButton);
            this.groupBox2.Controls.Add(this.cancelButton);
            this.groupBox2.Location = new System.Drawing.Point(18, 407);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 221);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Utilities";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Yellow;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 675);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(10);
            this.pictureBox1.Size = new System.Drawing.Size(156, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // TypesSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 813);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panelTypeIdentifierGroupBox);
            this.Controls.Add(this.unitTypeIdentifierGroupBox);
            this.Controls.Add(this.exitButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TypesSelectorForm";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.Text = "BOLD - Visualizzatore di Tipologie";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TypesSelector_FormClosing);
            this.Load += new System.EventHandler(this.TypesSelectorForm_Load);
            this.unitTypeIdentifierGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelTypeIdentifierGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Button exitButton;
        private GroupBox unitTypeIdentifierGroupBox;
        private DataGridView dataGridView1;
        private GroupBox panelTypeIdentifierGroupBox;
        private DataGridView dataGridView2;
        private Button cancelButton;
        private Button allUTIButton;
        private Button allPTIButton;
        private GroupBox groupBox2;
        private PictureBox pictureBox1;
    }
}