namespace PAEG1
{
    partial class ElectionForm
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
            components = new System.ComponentModel.Container();
            electionBox = new GroupBox();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // electionBox
            // 
            electionBox.AutoSize = true;
            electionBox.BackColor = Color.LightBlue;
            electionBox.Location = new Point(3, 0);
            electionBox.Name = "electionBox";
            electionBox.Size = new Size(123, 38);
            electionBox.TabIndex = 0;
            electionBox.TabStop = false;
            electionBox.Text = "Оберіть кандидата";
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // ElectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(800, 450);
            Controls.Add(electionBox);
            Name = "ElectionForm";
            Text = "Бланк голосування";
            FormClosed += ElectionForm_FormClosed;
            Load += ElectionForm_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox electionBox;
        private ErrorProvider errorProvider1;
    }
}