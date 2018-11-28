namespace WindowsFormsApp1
{
    partial class MachineAddForm
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
            this.userControl_Machine1 = new WindowsFormsApp1.UserControl_Machine();
            this.SuspendLayout();
            // 
            // userControl_Machine1
            // 
            this.userControl_Machine1.Location = new System.Drawing.Point(12, 12);
            this.userControl_Machine1.Name = "userControl_Machine1";
            this.userControl_Machine1.Size = new System.Drawing.Size(471, 220);
            this.userControl_Machine1.TabIndex = 0;
            // 
            // MachineAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 238);
            this.Controls.Add(this.userControl_Machine1);
            this.Name = "MachineAddForm";
            this.Text = "MachineAddForm";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControl_Machine userControl_Machine1;
    }
}