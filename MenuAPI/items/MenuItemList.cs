using System.Collections;

namespace MenuAPI;

/// <summary>
/// The values of a <see cref="MenuListItem"/>. Used like a <see cref="List{T}"/> of strings, but every change redraws the open menu straight away.
/// </summary>
public sealed class MenuItemList : IList<string>
{
    private readonly List<string> _items;

    /// <summary>
    /// Creates an empty list.
    /// </summary>
    public MenuItemList() => _items = new List<string>();

    /// <summary>
    /// Creates a list holding a copy of the given values.
    /// </summary>
    /// <param name="items">The values to copy.</param>
    public MenuItemList(IEnumerable<string> items) => _items = new List<string>(items);

    /// <summary>
    /// Lets a <see cref="List{T}"/> of strings be assigned to a <see cref="MenuItemList"/>. The values are copied, so later changes to the original list do not reach the menu.
    /// </summary>
    /// <param name="items">The values to copy.</param>
    public static implicit operator MenuItemList(List<string> items) => new(items);

    /// <summary>
    /// The number of values in the list.
    /// </summary>
    public int Count => _items.Count;
    /// <summary>
    /// Always false, the list can be changed.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// The value at the given position. Setting a different value redraws the menu.
    /// </summary>
    /// <param name="index">The zero based position.</param>
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

    /// <summary>
    /// Adds a value to the end of the list.
    /// </summary>
    /// <param name="item">The value to add.</param>
    public void Add(string item)
    {
        _items.Add(item);

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Adds several values to the end of the list.
    /// </summary>
    /// <param name="items">The values to add.</param>
    public void AddRange(IEnumerable<string> items)
    {
        _items.AddRange(items);

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Inserts a value at the given position.
    /// </summary>
    /// <param name="index">The zero based position to insert at.</param>
    /// <param name="item">The value to insert.</param>
    public void Insert(int index, string item)
    {
        _items.Insert(index, item);

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Removes the first occurrence of a value.
    /// </summary>
    /// <param name="item">The value to remove.</param>
    /// <returns>True when the value was found and removed.</returns>
    public bool Remove(string item)
    {
        if (!_items.Remove(item))
        {
            return false;
        }

        MenuNui.Invalidate();

        return true;
    }

    /// <summary>
    /// Removes the value at the given position.
    /// </summary>
    /// <param name="index">The zero based position to remove.</param>
    public void RemoveAt(int index)
    {
        _items.RemoveAt(index);

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Removes every value. Keep at least one value in a list item, see <see cref="MenuListItem.ListItems"/>.
    /// </summary>
    public void Clear()
    {
        if (_items.Count == 0)
        {
            return;
        }

        _items.Clear();

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Sorts the values alphabetically using the default string comparer.
    /// </summary>
    public void Sort() => Sort(null);

    /// <summary>
    /// Sorts the values with your own comparison.
    /// </summary>
    /// <param name="compare">How two values compare. Null uses the default string comparer.</param>
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

    /// <summary>
    /// Inserts several values at the given position.
    /// </summary>
    /// <param name="index">The zero based position to insert at.</param>
    /// <param name="items">The values to insert.</param>
    public void InsertRange(int index, IEnumerable<string> items)
    {
        var before = _items.Count;

        _items.InsertRange(index, items);

        if (_items.Count != before)
        {
            MenuNui.Invalidate();
        }
    }

    /// <summary>
    /// Removes every value that matches.
    /// </summary>
    /// <param name="match">Returns true for values to remove.</param>
    /// <returns>The number of values removed.</returns>
    public int RemoveAll(Predicate<string> match)
    {
        var removed = _items.RemoveAll(match);

        if (removed > 0)
        {
            MenuNui.Invalidate();
        }

        return removed;
    }

    /// <summary>
    /// Removes a range of values.
    /// </summary>
    /// <param name="index">The zero based position of the first value to remove.</param>
    /// <param name="count">How many values to remove.</param>
    public void RemoveRange(int index, int count)
    {
        if (count <= 0)
        {
            return;
        }

        _items.RemoveRange(index, count);

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Reverses the order of the values.
    /// </summary>
    public void Reverse()
    {
        if (_items.Count < 2)
        {
            return;
        }

        _items.Reverse();

        MenuNui.Invalidate();
    }

    /// <summary>
    /// Checks whether the list holds a value.
    /// </summary>
    /// <param name="item">The value to look for.</param>
    /// <returns>True when the value is in the list.</returns>
    public bool Contains(string item) => _items.Contains(item);

    /// <summary>
    /// Finds the position of the first occurrence of a value.
    /// </summary>
    /// <param name="item">The value to look for.</param>
    /// <returns>The zero based position, or -1 when it is not found.</returns>
    public int IndexOf(string item) => _items.IndexOf(item);

    /// <summary>
    /// Finds the position of the last occurrence of a value.
    /// </summary>
    /// <param name="item">The value to look for.</param>
    /// <returns>The zero based position, or -1 when it is not found.</returns>
    public int LastIndexOf(string item) => _items.LastIndexOf(item);

    /// <summary>
    /// Checks whether any value matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>True when at least one value matches.</returns>
    public bool Exists(Predicate<string> match) => _items.Exists(match);

    /// <summary>
    /// Finds the first value that matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>The first match, or null when nothing matches.</returns>
    public string? Find(Predicate<string> match) => _items.Find(match);

    /// <summary>
    /// Finds the last value that matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>The last match, or null when nothing matches.</returns>
    public string? FindLast(Predicate<string> match) => _items.FindLast(match);

    /// <summary>
    /// Finds the position of the first value that matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>The zero based position, or -1 when nothing matches.</returns>
    public int FindIndex(Predicate<string> match) => _items.FindIndex(match);

    /// <summary>
    /// Finds the position of the last value that matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>The zero based position, or -1 when nothing matches.</returns>
    public int FindLastIndex(Predicate<string> match) => _items.FindLastIndex(match);

    /// <summary>
    /// Finds every value that matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>A new list with the matches. Changing it does not change the menu.</returns>
    public List<string> FindAll(Predicate<string> match) => _items.FindAll(match);

    /// <summary>
    /// Checks whether every value matches.
    /// </summary>
    /// <param name="match">Returns true for a matching value.</param>
    /// <returns>True when all values match, or when the list is empty.</returns>
    public bool TrueForAll(Predicate<string> match) => _items.TrueForAll(match);

    /// <summary>
    /// Turns every value into something else.
    /// </summary>
    /// <typeparam name="TOutput">The type to convert to.</typeparam>
    /// <param name="converter">Converts one value.</param>
    /// <returns>A new list with the converted values.</returns>
    public List<TOutput> ConvertAll<TOutput>(Converter<string, TOutput> converter) => _items.ConvertAll(converter);

    /// <summary>
    /// Copies a range of values.
    /// </summary>
    /// <param name="index">The zero based position of the first value.</param>
    /// <param name="count">How many values to copy.</param>
    /// <returns>A new list with the values. Changing it does not change the menu.</returns>
    public List<string> GetRange(int index, int count) => _items.GetRange(index, count);

    /// <summary>
    /// Runs an action for every value.
    /// </summary>
    /// <param name="action">The action to run.</param>
    public void ForEach(Action<string> action) => _items.ForEach(action);

    /// <summary>
    /// Copies the values into an array.
    /// </summary>
    /// <param name="array">The array to copy into.</param>
    /// <param name="arrayIndex">The position in the array to start at.</param>
    public void CopyTo(string[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    /// <summary>
    /// Copies the values into a new array.
    /// </summary>
    /// <returns>A new array with the values.</returns>
    public string[] ToArray() => _items.ToArray();

    /// <summary>
    /// Copies the values into a new list.
    /// </summary>
    /// <returns>A new list with the values. Changing it does not change the menu.</returns>
    public List<string> ToList() => new(_items);

    /// <summary>
    /// Returns an enumerator that goes through the values.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<string> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
