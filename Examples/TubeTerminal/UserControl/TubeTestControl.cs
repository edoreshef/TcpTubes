using System;
using System.Text;
using TcpTubes;

namespace TubeTerminal.UserControl
{
    public partial class TubeTestControl : System.Windows.Forms.UserControl
    {
        private Type m_PipeType;
        private Hub  m_Pipe;

        public TubeTestControl()
        {
            InitializeComponent();
        }

        public void UpdateUI()
        {
            if (m_Pipe == null)
            {
                btnStartStop.Text = "Open";
                txtLocalId.Enabled = true;
                txtAddress.Enabled = true;
            }
            else
            {
                btnStartStop.Text = "Close";
                txtLocalId.Enabled = false;
                txtAddress.Enabled = false;
            }
        }

        public void SetPipeType(Type pipeType)
        {
            // Update UI according to pipe type (server or client)
            m_PipeType = pipeType;
            lblAddressLabel.Text      = pipeType == typeof(TcpServerHub) ? "Listen on" : "Connect to";
            lblClientServerTitle.Text = pipeType == typeof(TcpServerHub) ? "Server" : "Client";
            txtAddress.Text           = pipeType == typeof(TcpServerHub) ? "0.0.0.0:1222" : "127.0.0.1:1222";
            txtLocalId.Text           = pipeType == typeof(TcpServerHub) ? "99" : "10";
            UpdateUI();
        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            // Choose open or close
            if (m_Pipe == null)
            {
                // Get id,address and port from gui
                var id = uint.Parse(txtLocalId.Text);
                var ipParts = txtAddress.Text.Split(':');
                var port = ipParts.Length > 1 ? int.Parse(ipParts[1]) : 1222;

                // Create server or client
                if (m_PipeType == typeof(TcpServerHub))
                    m_Pipe = new TcpServerHub(id, ipParts[0], port);
                else
                    m_Pipe = new TcpClientHub(id, ipParts[0], port);
            }
            else
            {
                // Close will cause a graceful disconnection from all clients. 
                // When termination is completed an '#terminated' message will be reported.
                m_Pipe.Close();
            }

            UpdateUI();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            // Do we have a running pipe?
            if (m_Pipe != null)
            {
                // Is there pending messages to receive?
                uint sourceId;
                string messageId;
                byte[] data;
                if (m_Pipe.GetMessage(out sourceId, out messageId, out data))
                {
                    // Message recevived, start by logging it
                    txtLogWindow.AppendText($"{sourceId} > '{messageId}' (data.Length = {data?.Length})\r\n");

                    // Process messages
                    switch (messageId)
                    {
                        case "#terminated":
                            m_Pipe = null;
                            UpdateUI();
                            break;

                        case "#connected":
                            lstConnections.Items.Add(sourceId);
                            break;

                        case "#disconnected":
                            lstConnections.Items.Remove(sourceId);
                            break;
                    }
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            m_Pipe?.SendMessage(txtClientSendMsg.Text, Encoding.ASCII.GetBytes(txtClientSendData.Text));
        }
    }
}
