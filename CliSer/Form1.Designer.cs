namespace WindowsFormsApp1
{
    partial class ClientForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.hostname = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hostname
            // 
            this.hostname.AutoSize = true;
            this.hostname.Location = new System.Drawing.Point(751, 11);
            this.hostname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.hostname.Name = "hostname";
            this.hostname.Size = new System.Drawing.Size(70, 17);
            this.hostname.TabIndex = 0;
            this.hostname.Text = "hostname";
            this.hostname.Click += new System.EventHandler(this.hostname_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 550);
            this.Controls.Add(this.hostname);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ClientForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label hostname;
    }
}

