namespace Auto_QTE
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
            start_btn = new Button();
            bw_run = new System.ComponentModel.BackgroundWorker();
            list_text = new ListBox();
            SuspendLayout();
            // 
            // start_btn
            // 
            start_btn.Font = new Font("Microsoft JhengHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 136);
            start_btn.Location = new Point(66, 12);
            start_btn.Name = "start_btn";
            start_btn.Size = new Size(156, 132);
            start_btn.TabIndex = 0;
            start_btn.Text = "button1";
            start_btn.UseVisualStyleBackColor = true;
            start_btn.Click += start_btn_Click;
            // 
            // bw_run
            // 
            bw_run.DoWork += bw_run_DoWork;
            // 
            // list_text
            // 
            list_text.FormattingEnabled = true;
            list_text.HorizontalScrollbar = true;
            list_text.ItemHeight = 15;
            list_text.Location = new Point(-1, 161);
            list_text.MultiColumn = true;
            list_text.Name = "list_text";
            list_text.Size = new Size(297, 139);
            list_text.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(297, 299);
            Controls.Add(list_text);
            Controls.Add(start_btn);
            Name = "Form1";
            Text = "Auto_QTE";
            ResumeLayout(false);
        }

        #endregion

        private Button start_btn;
        private System.ComponentModel.BackgroundWorker bw_run;
        private ListBox list_text;
    }
}
