
namespace Tester
{
    partial class TesterForm
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
            initBtn = new System.Windows.Forms.Button();
            startQueueBtn = new System.Windows.Forms.Button();
            stopQueueBtn = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // initBtn
            // 
            initBtn.Location = new System.Drawing.Point(69, 63);
            initBtn.Name = "initBtn";
            initBtn.Size = new System.Drawing.Size(150, 46);
            initBtn.TabIndex = 0;
            initBtn.Text = "Initialize";
            initBtn.UseVisualStyleBackColor = true;
            initBtn.Click += Init_Click;
            // 
            // startQueueBtn
            // 
            startQueueBtn.Enabled = false;
            startQueueBtn.Location = new System.Drawing.Point(351, 63);
            startQueueBtn.Name = "startQueueBtn";
            startQueueBtn.Size = new System.Drawing.Size(204, 46);
            startQueueBtn.TabIndex = 1;
            startQueueBtn.Text = "Start Queuing";
            startQueueBtn.UseVisualStyleBackColor = true;
            startQueueBtn.Click += StartQueue_Click;
            // 
            // stopQueueBtn
            // 
            stopQueueBtn.Enabled = false;
            stopQueueBtn.Location = new System.Drawing.Point(351, 183);
            stopQueueBtn.Name = "stopQueueBtn";
            stopQueueBtn.Size = new System.Drawing.Size(204, 46);
            stopQueueBtn.TabIndex = 2;
            stopQueueBtn.Text = "Stop Queuing";
            stopQueueBtn.UseVisualStyleBackColor = true;
            stopQueueBtn.Click += StopQueue_Click;
            // 
            // TesterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(stopQueueBtn);
            Controls.Add(startQueueBtn);
            Controls.Add(initBtn);
            Name = "TesterForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button initBtn;
        private System.Windows.Forms.Button startQueueBtn;
        private System.Windows.Forms.Button stopQueueBtn;
    }
}

