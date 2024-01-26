using System.Diagnostics;

public class GameOfFifteen
{
  private int gridSize = 16;
  private static readonly int LIMIT = 4;

  private readonly int[] Goal = new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0};

  //private int[] Goal = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0};

  public static void Run()
  {
    var game = new GameOfFifteen(16);
    //Console.WriteLine();
    game.Runner();
  }

  public static void ManualScramble()
  {
    var game = new GameOfFifteen(16);
    //Console.WriteLine();
    game.Draw();
    game.ManualScramble(25);
    game.Runner();
  }

  private void ManualScramble(int iterations)
  {
    var random = new Random(DateTime.Now.Millisecond);
    
    for (var i = 0; i < iterations; i++)
    {
      var moves = GetTilesThatCanMove();
      var move = moves[random.Next(0, moves.Count)];
      Positions = this.GetFutureState(move.X, move.Y, move.Move);
      Draw(Positions);
      //Thread.Sleep(200);
    }
  }


  private void SlowAndVerbose<T>(int numberExplored, SortedSet<T> frontier, ExploredSet explored)
  {
#if SHOW_PAINT
    if (numberExplored > 500000 || numberExplored % 500 == 0)
#endif
    {
      Draw($"Tried: {numberExplored}, Frontier: {frontier.Count}, Explored: {explored.Count}");
      Thread.Sleep(100);
    }
  }

  
  private void Runner()
  {
    var timer = Stopwatch.StartNew();
    try
    {
      var solution = FindSolution();
      //ObjectDumper.Dump(solution);
      Console.WriteLine("Solution:");
      foreach (var item in solution.Item3)
      {
        Console.WriteLine(item);
      }
      //solution.Dump();
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
    finally
    {
      timer.Stop();
      Console.WriteLine($"Processing took {timer.Elapsed}");
    }
  }

  private (List<int[]>, List<Move>, List<string>) FindSolution()
  {
    //var frontier = new Frontier<Node>(new Queue<Node>());
    var frontier = new SortedSet<Node>(); //need this (sorted) because of heuristic

    WriteSolvability();

    frontier.Add(new Node() { State = GetState(), G = 0, H = ManhattanOrTaxiCabDistance(GetState(), Goal), Action = Move.None, Parent = Node.Empty });

    var explored = new ExploredSet();
    var numberExplored = 0;

    while (true)
    {
      IsTruthy(frontier.Count == 0, () => throw new Exception("No solution"));

      var node = frontier.First();
      frontier.Remove(node);

      Positions = node.State;
      numberExplored += 1;

      SlowAndVerbose(numberExplored, frontier, explored);

      if (IsGoal(Positions))
      {
        //refactor reconstruct
          Draw($"Goal @{numberExplored}, Frontier: {frontier.Count}, Explored: {explored.Count}");
        
        return ReconstructPath(node);
      }

      explored.Add(DeepCopy(node.State));

      foreach (var item in GetTilesThatCanMove())
      {
        var state = this.GetFutureState(item.X, item.Y, item.Move);

        int g = node.G + 1;
        int h = ManhattanOrTaxiCabDistance(state, Goal);

        Console.WriteLine($"Heuristic check: {g + h < node.G + node.H}");

        if (explored.ContainsState(state) == false || g + h < node.G + node.H)
        {
          frontier.Add(new Node()
          {
            State = DeepCopy(state),
            G = g,
            H = h,
            Parent = node,
            Action = item.Move,
            ActionText = $"{item.Value} moves {item.Move}"
          });
          explored.Add(DeepCopy(state));
        }
      }

    }
  }

  private int[] DeepCopy(int[] state)
  {
    var result = new int[state.Length];
    for(var i = 0;i <state.Length; i++)
    {
      result[i] = state[i];
    }

    return result;
  }

  private void WriteSolvability()
  {
    Console.WriteLine($"State: {GetState().Dump().Replace("\r\n", "|")} is solvable: {IsSolvable(GetState())}");
  }

  private bool IsSolvable(int[] state)
  {
    // https://www.geeksforgeeks.org/check-instance-15-puzzle-solvable/
    if (LIMIT % 2 == 0) //even
    {
      int index = Array.IndexOf(state, 0);
      if (NIsOdd(index))
        return NumberOfInversions(state) % 2 == 0;
      else if (NIsEven(index))
        return NumberOfInversions(state) % 2 == 1;
          
    }
    else //odd
    {
      return NumberOfInversions(state) % 2 == 0;
    }

    return false;
  }
   

  private bool NIsOdd(int index)
  {
    return new List<int>() { 4, 5, 6, 7, 12, 13, 14, 15 }.Contains(index);
  }

  private bool NIsEven(int index)
  {
    return new List<int>() { 0, 1, 2, 3, 8, 9, 10, 11}.Contains(index);
  }

  private int NumberOfInversions(int[] state)
  {
    int inversions = 0;
    for (int i = 0; i < state.Length - 1; i++)
    {
      if (state[i] > state[i + 1] && state[i+1] != 0)
        inversions++;
    }
    return inversions;
  }

  (List<int[]>, List<Move>, List<string>) ReconstructPath(Node node)
  {

    var actions = new List<Move>();
    var states = new List<int[]>();
    var moves = new List<string>();

    while (node.Parent != Node.Empty)
    {
      states.Add(node.State);
      actions.Add(node.Action);
      moves.Add(node.ActionText);
      node = node.Parent;
    }
    actions.Reverse();
    states.Reverse();
    moves.Reverse();
    return (states, actions, moves);

  }

  /* Think of this as cross streets and north south streets, or how many over and up to be in the right position */
  int ManhattanOrTaxiCabDistance(int[] state, int[] goal)
  {
    
    int h = 0;
    for (int i = 0; i < state.Length; i++)
    {
      int value = state[i];
      if (value != 0 && value != goal[i])
      {
        int x1 = i % LIMIT;
        int y1 = i / LIMIT;
        int x2 = Array.IndexOf(goal, value) % LIMIT;
        int y2 = Array.IndexOf(goal, value) / LIMIT;
        h += Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
      }
    }

    Debug.Assert(goal.SequenceEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 }));

    return h;
  }

  public GameOfFifteen(int size)
  {
    gridSize = size;
    GameOfFifteenBoard.GridSize = gridSize;
    Positions = GameOfFifteenBoard.RandomlyPopulate();

    Goal = new int[gridSize];
    for (var i = 0; i < gridSize; i++)
      Goal[i] = i + 1;

    Goal[gridSize - 1] = 0;
  }

  public bool IsGoal()
  {
    return Positions.SequenceEqual(Goal);
  }

  public bool IsGoal(int[] positions)
  {
    return Goal.SequenceEqual(positions);
  }


  private void DebugWriteLine(Exception ex)
  {
#if KUNK
        Debug.WriteLine(ex.Message);
#endif
  }

  

  public int[] Positions { get; protected set; }

  public int[] GetFutureState(int x, int y, Move move)
  {
    var clone = GetState();
    var result = (int[])MoveTile(x, y, move, false);
    result = DeepCopy(result);

    for (int i = 0; i < clone.Count(); i++)
    {
      Positions[i] = clone[i];
    }

    return result;
  }

  public int[] MoveTile(int x, int y, Move move, bool draw = true)
  {
    GuardX(x);
    GuardY(y);
    int width = GetWidth(gridSize);

    //right now any move whether its blank or not
    switch (move)
    {
      case Move.Up:
        MoveUp(x, y, width, draw);
        break;
      case Move.Down:
        MoveDown(x, y, width, draw);
        break;
      case Move.Left:
        MoveLeft(x, y, width, draw);
        break;
      case Move.Right:
        MoveRight(x, y, width, draw);
        break;

      default:
        Debug.WriteLine($"MoveTitle: {move}");
        break;
    }

    return Positions;
  }

  public List<Moveable> GetTilesThatCanMove()
  {
    int width = GetWidth();

    var list = new List<Moveable>();
    for (int y = 0; y < width; y++)
      for (int x = 0; x < width; x++)
      {
        var result = CanMove(x, y);
        if (result != Moveable.Empty)
          list.Add(result);
      }

    return list;
  }

  public Moveable CanMove(int x, int y)
  {
    int width = GetWidth();
    int value = GetValue(x, y);

    if (CanMoveDown(x, y, width))
      return new Moveable() { X = x, Y = y, Value = value, Move = Move.Down };
    if (CanMoveUp(x, y, width))
      return new Moveable() { X = x, Y = y, Value = value, Move = Move.Up };
    if (CanMoveLeft(x, y, width))
      return new Moveable() { X = x, Y = y, Value = value, Move = Move.Left };
    if (CanMoveRight(x, y, width))
      return new Moveable() { X = x, Y = y, Value = value, Move = Move.Right };

    return Moveable.Empty;
  }

  public int GetValue(int x, int y)
  {
    try
    {
      int k = GetWidth() * y + x;
      return Positions[k];
    }
    catch (Exception ex)
    {
      DebugWriteLine(ex);
      return Int32.MinValue;
    }
  }

  public bool CanMoveDown(int x, int y, int width)
  {
    return y + 1 < width && CanMoveTo(width * (y + 1) + x);
  }


  void MoveDown(int x, int y, int width, bool draw = true)
  {
    int oldX = width * y + x;
    int newX = width * (y + 1) + x;
    PositionChange(oldX, newX, "↓");
    
    if (y + 1 >= 0 && CanMoveTo(newX))
    {
      Swap(oldX, newX);
      if (draw) Draw();
    }
    else
    {
      Debug.WriteLine("Target tile is not empty");
    }
  }

  public bool CanMoveUp(int x, int y, int width)
  {
    return y - 1 >= 0 && CanMoveTo(width * (y - 1) + x);
  }

  private void PositionChange(int oldX, int newX, string direction)
  {
    Console.WriteLine($"{Positions[oldX]} {direction} {Positions[newX]}");
  }

  void MoveUp(int x, int y, int width, bool draw = true)
  {
    int oldX = width * y + x;
    int newX = width * (y - 1) + x;

    PositionChange(oldX, newX, "↑");

    if (y - 1 >= 0 && CanMoveTo(newX))
    {
      Swap(oldX, newX);
      if (draw) Draw();
    }
    else
    {
      Debug.WriteLine("Target tile is not empty");
    }
  }

  public bool CanMoveLeft(int x, int y, int width)
  {
    return x - 1 >= 0 && CanMoveTo(width * y + x - 1);
  }

  public bool CanMoveTo(int newX)
  {
    try
    {
      return Positions[newX] == 0;
    }
    catch (Exception ex)
    {
      DebugWriteLine(ex);
      return false;
    }
  }

  void MoveLeft(int x, int y, int width, bool draw = true)
  {
    int oldX = width * y + x;
    int newX = width * y + x - 1;

    PositionChange(oldX, newX, "←");
    if (x - 1 >= 0 && CanMoveTo(newX))
    {
      Swap(oldX, newX);
      if (draw) Draw();
    }
    else
    {
      Debug.WriteLine("Target tile is not empty");
    }
  }

  public bool CanMoveRight(int x, int y, int width)
  {
    return x + 1 < width && CanMoveTo(width * y + x + 1);
  }


  void MoveRight(int x, int y, int width, bool draw = true)
  {
    int oldX = width * y + x;
    int newX = width * y + x + 1;
    PositionChange(oldX, newX, "→");
    if (x + 1 < width && CanMoveTo(newX))
    {
      Swap(oldX, newX);
      if (draw) Draw();
    }
    else
    {
      Debug.WriteLine("Target tile is not empty");
    }
  }

  public void DrawNumberedTile(int x, int y)
  {
    int k = GetWidth() * y + x;
    GameOfFifteenBoard.DrawNumberedTile(Positions, k, x, y);
  }

  public void DrawGoalState()
  {
    GameOfFifteenBoard.Draw(Goal);
  }


  public void Draw(string message = "")
  {
    GameOfFifteenBoard.Draw(Positions, message);
  }


  public void Draw(int[] positions)
  {
    GameOfFifteenBoard.Draw(positions);
  }

  int[] Swap(int oldX, int newX)
  {
    int temp = Positions[oldX];
    Positions[oldX] = Positions[newX];
    Positions[newX] = temp;
    return Positions;
  }


  void IsTruthy(bool test, Action action)
  {
    if (test)
      action();
  }


  void GuardY(int y)
  {
    GuardXY(y, "y out of range");
  }

  void GuardX(int x)
  {
    GuardXY(x, "x out of range");
  }

  int GetWidth()
  {
    return (int)Math.Sqrt(gridSize);
  }

  int GetWidth(int length)
  {
    return (int)Math.Sqrt(length);
  }

  void GuardXY(int k, string message)
  {
    Guard(k >= 0 && k < GetWidth(Positions.Length), () => throw new ArgumentException(message, "k"));
  }

  public int[] GetState()
  {
    return DeepCopy(Positions);
  }


  void Guard(bool test, Action action)
  {
    if (test == false)
      action();
  }

}

