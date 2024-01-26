public static class FrontierExtension
{
  public static bool ContainsState(this Frontier<Node> frontier, int[] state)
  {
    var enumerator = frontier.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (enumerator.Current.State.SequenceEqual(state)) return true;
    }
    return false;
  }
}

