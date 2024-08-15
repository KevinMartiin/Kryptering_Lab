namespace GruppLabKryptering
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
            EncryptButton = new Button();
            decryptButton = new Button();
            textBoxInput = new TextBox();
            textBoxOutput = new TextBox();
            SuspendLayout();
            // 
            // EncryptButton
            // 
            EncryptButton.Location = new Point(81, 262);
            EncryptButton.Name = "EncryptButton";
            EncryptButton.Size = new Size(112, 34);
            EncryptButton.TabIndex = 0;
            EncryptButton.Text = "Encrypt";
            EncryptButton.UseVisualStyleBackColor = true;
            EncryptButton.Click += EncryptButton_Click;
            // 
            // decryptButton
            // 
            decryptButton.Location = new Point(621, 262);
            decryptButton.Name = "decryptButton";
            decryptButton.Size = new Size(112, 34);
            decryptButton.TabIndex = 1;
            decryptButton.Text = "Decrypt";
            decryptButton.UseVisualStyleBackColor = true;
            decryptButton.Click += decryptButton_Click;
            // 
            // textBoxInput
            // 
            textBoxInput.Location = new Point(81, 36);
            textBoxInput.Name = "textBoxInput";
            textBoxInput.Size = new Size(652, 31);
            textBoxInput.TabIndex = 2;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Location = new Point(81, 157);
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.Size = new Size(652, 31);
            textBoxOutput.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxOutput);
            Controls.Add(textBoxInput);
            Controls.Add(decryptButton);
            Controls.Add(EncryptButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button EncryptButton;
        private Button decryptButton;
        private TextBox textBoxInput;
        private TextBox textBoxOutput;
    }
}
