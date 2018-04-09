#if NETFRAMEWORK4_X

namespace Microshaoft
{
    using System;
    using System.Drawing;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;
    public class ProcessWaitingCancelableDialog : Form, IDialogResultForm
    {
        private IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(208, 132);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "取消(&C)";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(25, 132);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "确定(&O)";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(112, 166);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "重试(&R)";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // ProcessWaitingCancelableDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ProcessWaitingCancelableDialog";
            this.ResumeLayout(false);

        }

        private Button button2;
        private Button button3;
        private Button button1;
        public Button CancelWaitButton
        {
            get
            {
                return button1;
            }
        }
        public ProcessWaitingCancelableDialog()
        {
            InitializeComponent();
            button1.Click += DialogResultButtonClick;
            button2.Click += DialogResultButtonClick;
            
        }
        void DialogResultButtonClick(object sender, EventArgs e)
        {

            Button dialogResultButton = (Button)sender;

            dialogResultButton.Click -= DialogResultButtonClick;
            Close();
        }

        public void SetDialogResultProcess(params DialogResult[] results)
        {
            var action = new Action
            (
                    () =>
                    {
                        foreach (var result in results)
                        {
                            if (result == DialogResult.Cancel)
                            {
                                button1.Enabled = true;
                            }
                            if (result == DialogResult.OK)
                            {
                                button2.Enabled = true;
                            }
                            if (result == DialogResult.Retry)
                            {
                                button3.Enabled = true;
                            }
                        }
                    }
            );

            if (this.IsHandleCreated && this.IsHandleCreated)
            {
                this.Invoke
                        (
                            action
                        );
            }
        }
    }
}

#endif