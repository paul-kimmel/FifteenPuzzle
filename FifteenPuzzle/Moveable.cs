public class Moveable
{
  public int X { get; set; }
  public int Y { get; set; }
  public int Value { get; set; }
  public Move Move { get; set; }

  private static readonly Moveable _empty = new Moveable() { X = -1, Y = -1, Value = -1, Move = Move.None };
  public static Moveable Empty { get { return _empty; } }
}
