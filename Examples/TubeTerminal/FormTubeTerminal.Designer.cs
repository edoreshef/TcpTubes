
using TubeTerminal.UserControl;

partial class FormTubeTerminal
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
        this.ClientPipe = new TubeTestControl();
        this.ServerPipe = new TubeTestControl();
        this.SuspendLayout();
        // 
        // ClientPipe
        // 
        this.ClientPipe.Location = new System.Drawing.Point(12, 12);
        this.ClientPipe.Name = "ClientPipe";
        this.ClientPipe.Size = new System.Drawing.Size(394, 449);
        this.ClientPipe.TabIndex = 0;
        // 
        // SeverPipe
        // 
        this.ServerPipe.Location = new System.Drawing.Point(412, 12);
        this.ServerPipe.Name = "ServerPipe";
        this.ServerPipe.Size = new System.Drawing.Size(394, 449);
        this.ServerPipe.TabIndex = 1;
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(802, 455);
        this.Controls.Add(this.ServerPipe);
        this.Controls.Add(this.ClientPipe);
        this.Name = "FormTubeTerminal";
        this.Text = "PipeTester";
        this.ResumeLayout(false);

    }

    #endregion

    private TubeTestControl ClientPipe;
    private TubeTestControl ServerPipe;
}


