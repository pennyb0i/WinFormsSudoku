namespace WinFormsSudoku
{
    partial class Form1
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
            this.gBoxSudoku = new System.Windows.Forms.GroupBox();
            this.btnGenerateClear = new System.Windows.Forms.Button();
            this.btnLoadSave = new System.Windows.Forms.Button();
            this.btnStartSolve = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gBoxSudoku
            // 
            this.gBoxSudoku.Location = new System.Drawing.Point(28, 12);
            this.gBoxSudoku.Name = "gBoxSudoku";
            this.gBoxSudoku.Size = new System.Drawing.Size(375, 375);
            this.gBoxSudoku.TabIndex = 0;
            this.gBoxSudoku.TabStop = false;
            // 
            // btnGenerateClear
            // 
            this.btnGenerateClear.Location = new System.Drawing.Point(19, 430);
            this.btnGenerateClear.Name = "btnGenerateClear";
            this.btnGenerateClear.Size = new System.Drawing.Size(126, 44);
            this.btnGenerateClear.TabIndex = 1;
            this.btnGenerateClear.Text = "Generate puzzle";
            this.btnGenerateClear.UseVisualStyleBackColor = true;
            this.btnGenerateClear.Click += new System.EventHandler(this.btnGenerateClear_Click);
            // 
            // btnLoadSave
            // 
            this.btnLoadSave.Location = new System.Drawing.Point(151, 430);
            this.btnLoadSave.Name = "btnLoadSave";
            this.btnLoadSave.Size = new System.Drawing.Size(126, 44);
            this.btnLoadSave.TabIndex = 2;
            this.btnLoadSave.Text = "Load puzzle";
            this.btnLoadSave.UseVisualStyleBackColor = true;
            this.btnLoadSave.Click += new System.EventHandler(this.btnLoadSave_Click);
            // 
            // btnStartSolve
            // 
            this.btnStartSolve.Location = new System.Drawing.Point(283, 430);
            this.btnStartSolve.Name = "btnStartSolve";
            this.btnStartSolve.Size = new System.Drawing.Size(126, 44);
            this.btnStartSolve.TabIndex = 3;
            this.btnStartSolve.Text = "Start game";
            this.btnStartSolve.UseVisualStyleBackColor = true;
            this.btnStartSolve.Click += new System.EventHandler(this.btnStartSolve_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(131, 390);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(162, 20);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(428, 486);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStartSolve);
            this.Controls.Add(this.btnLoadSave);
            this.Controls.Add(this.btnGenerateClear);
            this.Controls.Add(this.gBoxSudoku);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "Form1";
            this.Text = "Sudokerino";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox gBoxSudoku;
        private System.Windows.Forms.Button btnGenerateClear;
        private System.Windows.Forms.Button btnLoadSave;
        private System.Windows.Forms.Button btnStartSolve;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.Button btnSave;

        #endregion
    }
}