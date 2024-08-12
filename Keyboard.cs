using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace KeyboardScan
{
    public partial class Keyboard : Form
    {
        public Keyboard()
        {
            InitializeComponent();
        }

        private List<Button> clickbtns = new List<Button>();
        Color UnActionColor = System.Drawing.SystemColors.Control;// Color.IndianRed;//Color.FromArgb(224, 224, 224);
        Color ActionColor = Color.FromArgb(255, 255, 192);

        private Button GetButton(string name)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                var control = Controls[i];
                if (control.Name == name) return control as Button;
            }
            return null;
        }

        private void SetButtonColor(Color c)
        {
            foreach (var btn in clickbtns)
            {
                btn.BackColor = c;
            }
        }

        private KeyboradCheck k_hook = new KeyboradCheck();
        private KeyEventHandler KeyEventHandler;

        /// <summary>
        /// 开始监听
        /// </summary>
        public void startListen()
        {
            KeyEventHandler = new KeyEventHandler(hook_KeyDown);
            k_hook.KeyDownEvent += KeyEventHandler;//钩住键按下
            k_hook.KeyUpEvent += K_hook_KeyUpEvent; ;
            k_hook.Start();//安装键盘钩子
        }

        private void K_hook_KeyUpEvent(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"keyup keycode {e.KeyCode} , keyvalue {e.KeyValue} , keysupp {e.SuppressKeyPress} , keydata {e.KeyData} ");

            var k = e.KeyCode.ToString();
            this.lblKey.Text = k;
            this.txtKeyCode.Text = e.KeyValue.ToString();
            var cname = "btn" + k;
            var ctrls = this.Controls.Find(cname, true);
            if (ctrls.Length == 0) return;
            var btn = ctrls[0] as Button;
            if (btn != null)
            {
                this.clickbtns.Add(btn);
                btn.BackColor = ActionColor;
            }
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"keydown keycode {e.KeyCode} , keyvalue {e.KeyValue} , keysupp {e.SuppressKeyPress} , keydata {e.KeyData} ");
            var k = e.KeyCode.ToString();
            this.lblKey.Text = k;
            this.txtKeyCode.Text = e.KeyValue.ToString();

            var cname = "btn" + k;
            var ctrls = this.Controls.Find(cname, true);
            if (ctrls.Length == 0) return;
            var btn = ctrls[0] as Button;
            if (btn != null)
            {
                this.clickbtns.Add(btn);
                btn.BackColor = Color.Orange;
            }
        }

        /// <summary>
        /// 结束监听
        /// </summary>
        public void stopListen()
        {
            if (KeyEventHandler != null)
            {
                k_hook.KeyDownEvent -= KeyEventHandler;//取消按键事件
                KeyEventHandler = null;
                k_hook.Stop();//关闭键盘钩子
            }
        }

        private void lblReset_Click(object sender, EventArgs e)
        {
            this.SetButtonColor(this.UnActionColor);
            this.clickbtns.Clear();
        }
        private void Keyboard_Load(object sender, EventArgs e)
        {
            this.SetButtonColor(UnActionColor);
            this.startListen();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.linkLabel1.Text);
        }
    }
}
