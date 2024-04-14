using System.Collections.Generic;

public static class EnumerableExtensions
{
	/// <summary>
	/// Removes the first occurrence of the specified value from the provided sequence, if it exists.
	/// </summary>
	public static IEnumerable<T> RemoveOne<T>(this IEnumerable<T> enumerable, T value)
	{
		bool found = false;
		foreach (var element in enumerable)
		{
			if (!found && EqualityComparer<T>.Default.Equals(element, value))
				found = true;
			else
				yield return element;
		}
	}
}