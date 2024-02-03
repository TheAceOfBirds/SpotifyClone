using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using SpotifyClone.Services;
using System.Configuration;
namespace Tester
{
    public partial class TesterForm : Form
    {
        QueuingTesterService queuingTesterService;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        // Task will hold the logic
        private Task _task;
        static TesterForm form;
        public TesterForm()
        {
            InitializeComponent();
            form = this;
        }

        private void EventLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                queuingTesterService.ResetQueueAndLog();
                queuingTesterService.ShuffleQueue();
            }
            SetButtonEnabled(true, startQueueBtn);
            _cts = new CancellationTokenSource();

        }
        private void Init_Click(object sender, EventArgs e)
        {
            queuingTesterService = new QueuingTesterService(ConfigurationManager.AppSettings["clientId"],
                                                            ConfigurationManager.AppSettings["clientSecret"],
                                                            ConfigurationManager.AppSettings["playlistId"],
                                                            ConfigurationManager.AppSettings["excelPath"]);
            SetButtonEnabled(false, initBtn);
            Thread.Sleep(1000);
            if (queuingTesterService != null)
            {
                SetButtonEnabled(true, startQueueBtn);
            }
        }

        private void StartQueue_Click(object sender, EventArgs e)
        {
            _task = Task.Factory.StartNew(() => EventLoop(_cts.Token), _cts.Token);
            SetButtonEnabled(false, startQueueBtn);
            SetButtonEnabled(true, stopQueueBtn);
        }

        private void StopQueue_Click(object sender, EventArgs e)
        {
            SetButtonEnabled(false, stopQueueBtn);
            _cts.Cancel();
        }
        delegate void SetTextCallback(string text, TextBox textBox, bool append);
        public static void SetText(string text, TextBox textBox, bool append)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (textBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                form.Invoke(d, new object[] { text, textBox, append });
            }
            else
            {
                if (append)
                {
                    textBox.AppendText(text);
                    textBox.AppendText(Environment.NewLine);
                }
                else
                {
                    textBox.Text = text;
                }
            }
        }

        delegate void SetButtonEnabledCallback(bool enabled, Button button);
        public static void SetButtonEnabled(bool enabled, Button button)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (button.InvokeRequired)
            {
                SetButtonEnabledCallback d = new SetButtonEnabledCallback(SetButtonEnabled);
                form.Invoke(d, new object[] { enabled, button });
            }
            else
            {
                button.Enabled = enabled;
            }
        }
    }
}
