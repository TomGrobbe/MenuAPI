using System.Collections;

namespace MenuAPI;

public sealed class MenuItemList : IList<string>
{
    private readonly List<string> _items;

    public MenuItemList() => _items = new List<string>();

    public MenuItemList(IEnumerable<string> items) => _items = new List<string>(items);

    public static implicit operator MenuItemList(List<string> items) => new(items);

    public int Count => _items.Count;
    public bool IsReadOnly => false;

    public string this[int index]
    {
        get => _items[index];
        set
        {
            if (_items[index] == value)
            {
                return;
            }

            _items[index] = value;

            MenuNui.Invalidate();
        }
    }

    public void Add(string item)
    {
        _items.Add(item);

        MenuNui.Invalidate();
    }

    public void AddRange(IEnumerable<string> items)
    {
        _items.AddRange(items);

        MenuNui.Invalidate();
    }

    public void Insert(int index, string item)
    {
        _items.Insert(index, item);

        MenuNui.Invalidate();
    }

    public bool Remove(string item)
    {
        if (!_items.Remove(item))
        {
            return false;
        }

        MenuNui.Invalidate();

        return true;
    }

    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);

        MenuNui.Invalidate();
    }

    public void Clear()
    {
        if (_items.Count == 0)
        {
            return;
        }

        _items.Clear();

        MenuNui.Invalidate();
    }

    public void Sort() => Sort(null);

    public void Sort(Comparison<string>? compare)
    {
        if (compare is null)
        {
            _items.Sort();
        }
        else
        {
            _items.Sort(compare);
        }

        MenuNui.Invalidate();
    }

    public void InsertRange(int index, IEnumerable<string> items)
    {
        var before = _items.Count;

        _items.InsertRange(index, items);

        if (_items.Count != before)
        {
            MenuNui.Invalidate();
        }
    }

    public int RemoveAll(Predicate<string> match)
    {
        var removed = _items.RemoveAll(match);

        if (removed > 0)
        {
            MenuNui.Invalidate();
        }

        return removed;
    }

    public void RemoveRange(int index, int count)
    {
        if (count <= 0)
        {
            return;
        }

        _items.RemoveRange(index, count);

        MenuNui.Invalidate();
    }

    public void Reverse()
    {
        if (_items.Count < 2)
        {
            return;
        }

        _items.Reverse();

        MenuNui.Invalidate();
    }

    public bool Contains(string item) => _items.Contains(item);

    public int IndexOf(string item) => _items.IndexOf(item);

    public int LastIndexOf(string item) => _items.LastIndexOf(item);

    public bool Exists(Predicate<string> match) => _items.Exists(match);

    public string? Find(Predicate<string> match) => _items.Find(match);

    public string? FindLast(Predicate<string> match) => _items.FindLast(match);

    public int FindIndex(Predicate<string> match) => _items.FindIndex(match);

    public int FindLastIndex(Predicate<string> match) => _items.FindLastIndex(match);

    public List<string> FindAll(Predicate<string> match) => _items.FindAll(match);

    public bool TrueForAll(Predicate<string> match) => _items.TrueForAll(match);

    public List<TOutput> ConvertAll<TOutput>(Converter<string, TOutput> converter) => _items.ConvertAll(converter);

    public List<string> GetRange(int index, int count) => _items.GetRange(index, count);

    public void ForEach(Action<string> action) => _items.ForEach(action);

    public void CopyTo(string[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public string[] ToArray() => _items.ToArray();

    public List<string> ToList() => new(_items);

    public IEnumerator<string> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
