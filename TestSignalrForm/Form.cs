using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace TestSignalrForm
{
    public partial class Form : System.Windows.Forms.Form
    {
        private readonly HubConnection _hub;
        private readonly IConfiguration _config;
        public Form()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            InitializeComponent();
            _hub = new HubConnectionBuilder().WithUrl(_config["serverurl"]).WithAutomaticReconnect().Build();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                txtResult.SelectionAlignment = HorizontalAlignment.Left;
                _hub.On<Info>("Receive", x =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{x.name}: {x.message}");
                    sb.AppendLine($"{DateTime.Now}");
                    sb.AppendLine();
                    txtResult.SelectionAlignment = HorizontalAlignment.Left;
                    txtResult.AppendText(sb.ToString());
                    txtResult.Focus();
                    txtResult.Select(txtResult.TextLength, 0);
                    txtResult.ScrollToCaret();
                    txtMessage.Focus();
                });
                lblName.Visible = false;
                txtName.Visible = false;
                btnLogin.Visible = false;
                btnSend.Enabled = true;
                txtResult.Enabled = true;
                btnSend.Enabled = true;
                txtMessage.Enabled = true;
            }
            else
            {
                txtName.Focus();
            }
        }

        private async void Form_Load(object sender, System.EventArgs e)
        {
            await _hub.StartAsync();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            var msg = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                var info = new Info
                {
                    name = txtName.Text,
                    message = msg
                };
                await _hub.InvokeAsync("Send", info);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"{txtName.Text}: {msg}");
                sb.AppendLine($"{DateTime.Now}");
                sb.AppendLine();
                txtResult.SelectionAlignment = HorizontalAlignment.Right;
                txtResult.AppendText(sb.ToString());
                txtResult.Focus();
                txtResult.Select(txtResult.TextLength, 0);
                txtResult.ScrollToCaret();
                txtMessage.Focus();
                txtMessage.Text = null;
            }
            else
            {
                txtMessage.Focus();
            }
        }
    }
}
