using System.Windows.Forms;
using TcpTubes;


public partial class FormTubeTerminal : Form
{
    public FormTubeTerminal()
    {
        InitializeComponent();
        ClientPipe.SetPipeType(typeof(TcpClientHub));
        ServerPipe.SetPipeType(typeof(TcpServerHub));
    }
}

