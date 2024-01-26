using System.ComponentModel;
using System.Drawing.Text;

public class GameOfFifteenBoard
{
  public static int GridSize = 16;
  private static Bitmap bitmap;
  private static readonly int width = 401;

  //for winforms

  public static void Draw(int[] positions, string message = "")
  {
    Guard(positions);

    bitmap = new Bitmap(width, width + 20);
    

    using var graphics = Graphics.FromImage(bitmap);
    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

    DrawGameBoard(graphics, width);

    int root = (int)Math.Sqrt(positions.Length);

    for (int y = 0, k = 0; y < root; y++)
      for (int x = 0; x < root; x++)
        DrawNumberedTile(graphics, positions[k++].ToString(), x, y, width / root);

    if (message != "")
      graphics.DrawString(message, exploredFont, Brushes.Green, new Point(0, 401));

    //Util.ClearResults();
    //Console.WriteLine(Environment.NewLine);
    //bitmap.Dump();

    if (OnDraw != null)
      OnDraw(bitmap, EventArgs.Empty);

  }

  public static EventHandler? OnDraw { get; set; }



  //experimental
  public static void DrawNumberedTile(int[] positions, int k, int x, int y)
  {
    if (bitmap == null)
      throw new ArgumentNullException("bitmap");

    using var graphics = Graphics.FromImage(bitmap);
    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

    int root = (int)Math.Sqrt(positions.Length);

    DrawNumberedTile(graphics, positions[k].ToString(), x, y, width / root);
    //Util.ClearResults();
    bitmap.Dump();

  }


  static void DrawGameBoard(Graphics graphics, int width) => graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, width - 1, width - 1));

  static void DrawNumberedTile(Graphics graphics, string text, int x, int y, int w)
  {
    DrawRectangle(graphics, text, x, y, w);
    DrawString(graphics, text, x, y, w);
  }

  static void DrawString(Graphics graphics, string text, int x, int y, int w) => graphics.DrawString(text, font, Brushes.Black,
                  GetTextPoint(x, y, w));

  static void DrawRectangle(Graphics graphics, string text, int x, int y, int w)
  {
    var rect = GetRectangle(x, y, w);
    var brush = text == "0" || text == "" ? Brushes.LightGreen : Brushes.White;
    graphics.FillRectangle(brush, rect);
    graphics.DrawRectangle(Pens.Black, GetRectangle(x, y, w));
  }


  static Rectangle GetRectangle(int x, int y, int w) => new Rectangle(GetX(x, w), GetY(y, w), w, w);
  static PointF GetTextPoint(int x, int y, int w) => new Point(GetTextX(x, w), GetTextY(y, w));


  private static int GetX(int x, int width) => GetPosition(x, width);
  private static int GetY(int y, int width) => GetPosition(y, width);

  static int GetPosition(int k, int width)
  {
    return k * width;
  }

  private static int GetTextX(int x, int width) => GetTextPosition(x, width);
  private static int GetTextY(int y, int width) => GetTextPosition(y, width);

  static int GetTextPosition(int k, int width)
  {
    return k * width + width / 3;
  }

  private static Font font = new Font(new FontFamily("Arial"), 16);
  private static Font exploredFont = new Font(new FontFamily("Arial"), 8);

  static void Guard(int[] positions)
  {
    Guard(IsPerfectSquare(positions.Length), () => throw new ArgumentOutOfRangeException("positions", "Not a perfect square"));
  }
  static void Guard(bool test, Action action)
  {
    if (test == false)
      action();
  }

  private static bool IsPerfectSquare(int number)
  {
    return Math.Sqrt(number) % 1 == 0;
  }

  [Description("Solvable in three (3)")]
  public static int[] SimplyPopulate8()
  {
    return new int[] { 1, 2, 3, 0, 4, 5, 7, 8, 6 };
  }

  [Description("Seemed to find at 177,000 or almost max using DFS")]

  public static int[] SimplyPopulate8_WithSolution()
  {
    return new int[] { 8, 0, 6, 5, 4, 7, 2, 3, 1 };
  }

  public static int[] SimplyPopulate8_WithSolution2()
  {
    return new int[] { 0, 1, 3, 4, 2, 5, 7, 8, 6 };
  }

  public static int[] SimplyPopulate15()
  {
    return new int[] { 5, 1, 3, 4, 2, 0, 7, 8, 9, 6, 19, 12, 13, 14, 11, 15 };
  }

  public static int[] SimplyPopulate15_withSolution()
  {
    return new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0, 13, 14, 15 };
    //return new int[] { 15, 14, 8, 12, 10, 11, 9, 13, 2, 6, 5, 1, 3, 7, 4, 0};
    //return new int[] { 9, 2, 8, 11, 0, 5, 13, 7, 15, 1, 4, 10, 3, 14, 6, 12};
    //return new int[] { 12, 1, 2, 15, 11, 6, 5, 8, 7, 10, 9, 4, 0, 13, 14, 3};
    //return new int[] { 6, 13, 7, 10, 8, 9, 11, 0, 15, 2, 12, 5, 14, 3, 1, 4};
    //return new int[] { 5, 2, 12, 0, 8, 11, 13, 3, 14, 1, 10, 15, 7, 6, 4, 9 };
    //return new int[] { 13, 2, 10, 3, 1, 12, 8, 4, 5, 0, 9, 6, 15, 14, 11, 7};
  }

  public static int[] SimplyPopulate15_withSolution2()
  {
    return new int[] { 2, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 };
  }

  public static void Shuffle<T>(Random random, T[] array)
  {
    int n = array.Length;
    while (n > 1)
    {
      int k = random.Next(n--);
      T temp = array[n];
      array[n] = array[k];
      array[k] = temp;
    }
  }

  public static int[] RandomlyPopulate()
  {
    return SimplyPopulate15_withSolution();
    var list = new List<int>();
    list.AddRange(Enumerable.Range(1, GridSize - 1));

    var result = list.Select(x => x).Cast<int>().ToList();
    result.Add(0);

    int[] numbers = result.ToArray();
    Shuffle(new Random(DateTime.Now.Millisecond), numbers);
    return numbers;
  }

  public static int[] RandomlyPopulate_Old()
  {
    var random = new Random(DateTime.Now.Millisecond);
    var positions = CreatePositions();
    var exclusionSet = new HashSet<int>();

    var i = 0;
    while (i < GridSize - 1)
    {
      var next = random.Next(1, GridSize);
      if (exclusionSet.Contains(next) == false)
      {
        i++;
        positions.Add(next);
        exclusionSet.Add(next);
      }
    }

    positions.Add(0);

    InitializeGrid(positions.ToArray());

    return positions.ToArray();
  }

  static void InitializeGrid(int[] positions)
  {
    Draw(positions);
  }

  public static List<int> CreatePositions() => new List<int>(GridSize);
}

