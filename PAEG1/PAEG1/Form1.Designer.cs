namespace PAEG1
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
            components = new System.ComponentModel.Container();
            loginInput = new TextBox();
            passwordInput = new TextBox();
            loginButton = new Button();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // loginInput
            // 
            loginInput.Location = new Point(73, 56);
            loginInput.Name = "loginInput";
            loginInput.PlaceholderText = "Login";
            loginInput.Size = new Size(213, 27);
            loginInput.TabIndex = 0;
            // 
            // passwordInput
            // 
            passwordInput.Location = new Point(73, 120);
            passwordInput.Name = "passwordInput";
            passwordInput.PasswordChar = '*';
            passwordInput.PlaceholderText = "Password";
            passwordInput.Size = new Size(213, 27);
            passwordInput.TabIndex = 1;
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.LightBlue;
            loginButton.Location = new Point(97, 192);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(144, 45);
            loginButton.TabIndex = 2;
            loginButton.Text = "Увійти";
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += button1_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(341, 294);
            Controls.Add(loginButton);
            Controls.Add(passwordInput);
            Controls.Add(loginInput);
            Name = "Form1";
            Text = "Авторизація";
            Load += Form1_Load;
            VisibleChanged += Form1_VisibleChanged;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox loginInput;
        private TextBox passwordInput;
        private Button loginButton;
        private ErrorProvider errorProvider1;
    }
}