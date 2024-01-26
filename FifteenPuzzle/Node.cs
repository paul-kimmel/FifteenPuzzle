#define SHOW_PAINT

public class Node : IComparable<Node>
{
  public int[] State;
  public int G { get; set; }
  public int H { get; set; }
  public Node Parent { get; set; }
  public Move Action { get; set; }
  public string ActionText { get; set; }


  private static readonly Node _empty = new Node() { State = null, G = 0, H = 0, Parent = null, Action = Move.None, ActionText = "None" };

  public static Node Empty { get { return _empty; } }

  public int CompareTo(Node other)
  {
    return (G + H).CompareTo(other.G + other.H);
    
  }
}
