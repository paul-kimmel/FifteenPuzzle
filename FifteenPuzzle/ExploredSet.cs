public class ExploredSet : List<int[]>
{
  public bool ContainsState(int[] state)
  {
    var enumerator = this.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (enumerator.Current.SequenceEqual(state)) return true;
    }

    return false;
  }
}

