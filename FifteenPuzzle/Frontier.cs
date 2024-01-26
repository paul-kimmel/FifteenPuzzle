//depth first search (Stack) or breadth first (Queue)
using System.Collections;

public class Frontier<T> : ICollection<T>
{
  private Stack<T> stack { get; set; } = null;
  private Queue<T> queue { get; set; } = null;

  public Frontier(Stack<T> stack)
  {   //depth first implementation
    this.stack = stack;
  }

  public Frontier(Queue<T> queue)
  {
    //breadth first implementation
    this.queue = queue;
  }

  public void Clear()
  {
    if (stack != null)
      stack.Clear();
    else
      queue.Clear();
  }

  public bool IsReadOnly => false;

  public int Count => stack != null ? stack.Count : queue.Count;

  public void Add(T item)
  {
    if (stack != null)
      stack.Push(item);
    else
      queue.Enqueue(item);
  }

  public bool IsEmpty()
  {
    return stack != null ? stack.Count == 0 : queue.Count == 0;
  }

  public T Next()
  {
    return stack != null ? stack.Pop() : queue.Dequeue();

  }

  public bool Remove(T item)
  {
    return stack != null ? stack.Peek() != null : queue.Peek() != null;
  }

  public bool Contains(T item)
  {
    var enumerator = GetEnumerator();

    while (enumerator.MoveNext())
    {
      var o = enumerator.Current;
      if (o.Equals(item))
        return true;
    }

    return false;
  }

  public void CopyTo(T[] array, int arrayIndex)
  {
    throw new NotImplementedException();
  }

  public IEnumerator<T> GetEnumerator()
  {
    return stack != null ? stack.GetEnumerator() : queue.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return stack != null ? ((IEnumerable)stack).GetEnumerator() :
        ((IEnumerable)queue).GetEnumerator();
  }

 
}

