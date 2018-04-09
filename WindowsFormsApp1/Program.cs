namespace Test2
{
    using System;
    using System.Windows.Forms;
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
namespace Test2
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using Microshaoft;
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Button button1;
    }
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private object _locker = new object();
        private void button1_Click(object sender, EventArgs e)
        {

            //测试样例代码
            DialogResult result = DialogResult.None;
            var pwcd = new ProcessWaitingCancelableDialog();
            while (1==1)
            {
                if (result == DialogResult.None || result == DialogResult.Retry)
                {
                    result = TaskWaitingProcessorHelper
                                .ProcessWaitingShowDialog
                                        (
                                            this
                                            , pwcd
                                            , (dialog) =>
                                            {
                                                lock (_locker)
                                                {
                                                    // 发指令序列
                                                    for (var i = 0; i < 5; i++)
                                                    {
                                                        TaskWaitingProcessorHelper
                                                            .TrySafeInvokeFormAction
                                                                 (
                                                                        dialog
                                                                        , (d) =>
                                                                        {
                                                                            d.Text = DateTime.Now.ToString();
                                                                        }
                                                                        , null
                                                                );
                                                        Thread.Sleep(1 * 1000);
                                                    }
                                                }
                                                //模拟异常
                                                //throw new Exception();
                                                //正常执行完成后 设置 DialogResultForm
                                                IDialogResultForm drf = dialog as IDialogResultForm;
                                                drf.SetDialogResultProcess
                                                            (
                                                                DialogResult.OK
                                                                , DialogResult.Cancel
                                                            );
                                            }
                                            , (x, d) => //捕获到异常
                                            {
                                                TaskWaitingProcessorHelper
                                                            .TrySafeInvokeFormAction
                                                                 (
                                                                        d
                                                                        , (dd) =>
                                                                        {
                                                                            dd.Text = "Exception " + DateTime.Now.ToString();
                                                                        }
                                                                        , null
                                                                );
                                                IDialogResultForm drf = d as IDialogResultForm;
                                                drf.SetDialogResultProcess
                                                        (
                                                            DialogResult.Retry
                                                            , DialogResult.Cancel
                                                        );
                                                return DialogResult.Retry;
                                            }
                                        );
                }
                if (result == DialogResult.Cancel)
                {
                    var thread = new Thread
                        (
                            () =>
                            {
                                lock (_locker)
                                {
                                    Console.WriteLine("begin: " + result);
                                    Thread.Sleep(2000);
                                    Console.WriteLine("end: " + result);
                                }

                            }
                        );
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    break;
                }
                else if (result == DialogResult.OK)
                {
                    break;
                }
                Console.WriteLine("loop in {0}" , result);
            }
            Console.WriteLine("loop out: {0}", result);

        }
    }
}

