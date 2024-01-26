namespace FifteenPuzzle
{
  partial class Form1
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
      pictureBox1 = new PictureBox();
      button1 = new Button();
      textBox1 = new TextBox();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      SuspendLayout();
      // 
      // pictureBox1
      // 
      pictureBox1.Location = new Point(38, 28);
      pictureBox1.Margin = new Padding(3, 2, 3, 2);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(444, 374);
      pictureBox1.TabIndex = 0;
      pictureBox1.TabStop = false;
      pictureBox1.VisibleChanged += pictureBox1_VisibleChanged;
      // 
      // button1
      // 
      button1.Location = new Point(505, 28);
      button1.Margin = new Padding(3, 2, 3, 2);
      button1.Name = "button1";
      button1.Size = new Size(82, 22);
      button1.TabIndex = 1;
      button1.Text = "Play";
      button1.UseVisualStyleBackColor = true;
      button1.Click += button1_Click;
      // 
      // textBox1
      // 
      textBox1.AcceptsReturn = true;
      textBox1.AcceptsTab = true;
      textBox1.Dock = DockStyle.Bottom;
      textBox1.Location = new Point(0, 441);
      textBox1.Margin = new Padding(3, 2, 3, 2);
      textBox1.Multiline = true;
      textBox1.Name = "textBox1";
      textBox1.ScrollBars = ScrollBars.Vertical;
      textBox1.Size = new Size(743, 104);
      textBox1.TabIndex = 2;
      textBox1.TextChanged += textBox1_TextChanged;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(743, 545);
      Controls.Add(textBox1);
      Controls.Add(button1);
      Controls.Add(pictureBox1);
      Margin = new Padding(3, 2, 3, 2);
      Name = "Form1";
      Text = "Fifteen Puzzle";
      FormClosing += Form1_FormClosing;
      Load += Form1_Load;
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private PictureBox pictureBox1;
    private Button button1;
    private TextBox textBox1;
  }
}