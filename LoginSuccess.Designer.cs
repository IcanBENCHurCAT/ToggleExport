namespace TogglExport
{
    partial class LoginSuccess
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Generate = new System.Windows.Forms.Button();
            this.dt_Begin = new System.Windows.Forms.DateTimePicker();
            this.dt_End = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_Loading = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Generate Summary Report";
            // 
            // btn_Generate
            // 
            this.btn_Generate.Location = new System.Drawing.Point(370, 188);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(83, 27);
            this.btn_Generate.TabIndex = 1;
            this.btn_Generate.Text = "Generate";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
            // 
            // dt_Begin
            // 
            this.dt_Begin.Location = new System.Drawing.Point(17, 114);
            this.dt_Begin.Name = "dt_Begin";
            this.dt_Begin.Size = new System.Drawing.Size(200, 20);
            this.dt_Begin.TabIndex = 2;
            // 
            // dt_End
            // 
            this.dt_End.Location = new System.Drawing.Point(17, 194);
            this.dt_End.Name = "dt_End";
            this.dt_End.Size = new System.Drawing.Size(200, 20);
            this.dt_End.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Begin Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "End Date";
            // 
            // lbl_Loading
            // 
            this.lbl_Loading.AutoSize = true;
            this.lbl_Loading.Location = new System.Drawing.Point(367, 218);
            this.lbl_Loading.Name = "lbl_Loading";
            this.lbl_Loading.Size = new System.Drawing.Size(54, 13);
            this.lbl_Loading.TabIndex = 6;
            this.lbl_Loading.Text = "Loading...";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(370, 114);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(83, 30);
            this.btn_Save.TabIndex = 7;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // LoginSuccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 258);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.lbl_Loading);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dt_End);
            this.Controls.Add(this.dt_Begin);
            this.Controls.Add(this.btn_Generate);
            this.Controls.Add(this.label1);
            this.Name = "LoginSuccess";
            this.Text = "LoginSuccess";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.DateTimePicker dt_Begin;
        private System.Windows.Forms.DateTimePicker dt_End;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_Loading;
        private System.Windows.Forms.Button btn_Save;
    }
}