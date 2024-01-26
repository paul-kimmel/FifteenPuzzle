using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FifteenPuzzle
{
  public partial class Form1 : Form
  {
    private TextBoxWriter? textBoxWriter = null;

    public Form1()
    {
      InitializeComponent();

      textBoxWriter = new TextBoxWriter(textBox1);

      GameOfFifteenBoard.OnDraw += OnDraw;
    }

    private void OnDraw(object sender, EventArgs e)
    {
      DrawPicture(sender as Bitmap);
    }

    private void DrawPicture(Bitmap? bitmap)
    {
      if (InvokeRequired)
      {
        Invoke(() =>
        {
          Clipboard.SetImage(bitmap);
          pictureBox1.Image = bitmap;
          this.Refresh();
        });
      }

    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void pictureBox1_VisibleChanged(object sender, EventArgs e)
    {

    }


    private void button1_Click(object sender, EventArgs e)
    {
      BackgroundWorker worker = new BackgroundWorker();
      worker.DoWork += Worker_DoWork;
      worker.RunWorkerAsync();

    }

    private void Worker_DoWork(object? sender, DoWorkEventArgs e)
    {
      GameOfFifteen.ManualScramble();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }
  }

  public class TextBoxWriter : TextWriter
  {
    private TextBoxBase control;
    private StringBuilder? builder;
    private TextWriter? oldWriter;

    public TextBoxWriter(TextBox control)
    {
      this.control = control;
      //control.HandleCreated += new EventHandler(control_HandleCreated);
      control.HandleCreated += Control_HandleCreated;
      oldWriter = Console.Out;
      Console.SetOut(this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (oldWriter != null)
        {
          Console.SetOut(oldWriter);
        }
      }

    }
    public override void Write(char ch)
    {
      Write(ch.ToString());
      if (oldWriter != null)
        oldWriter.Write(ch);
    }

    public override void Write(string? s)
    {
      if (control.IsHandleCreated)
        AppendText(s);
      else
        BufferText(s);

      if (oldWriter != null)
        oldWriter.Write(s);
    }

    public override void WriteLine(string? s)
    {
      Write(s + Environment.NewLine);
      if (oldWriter != null)
        oldWriter.WriteLine(s);
    }

    private void BufferText(string s)
    {
      if (builder == null)
        builder = new StringBuilder();
      builder.Append(s);
    }

    private void Control_HandleCreated(object? sender, EventArgs e)
    {
      if (builder != null)
      {
        AppendText(builder.ToString());
        builder = null;
      }
    }

    private delegate void AppendTextInvoker(string s);
    private void AppendText(string s)
    {
      if (control.InvokeRequired)
        control.Invoke(new AppendTextInvoker(InternalAppendText), s);
      else
        InternalAppendText(s);
    }

    private void InternalAppendText(string s)
    {
      if (builder != null)
      {
        control.AppendText(builder.ToString());
        builder = null;
      }

      control.AppendText(s);
      control.Refresh();
    }
    public override Encoding Encoding => Encoding.Default;
  }
}