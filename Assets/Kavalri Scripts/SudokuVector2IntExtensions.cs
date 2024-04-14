using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides extension methods for working with the Sudoku class in terms of Unity's Vector2Int for convenience.
/// </summary>
public static class SudokuVector2IntExtensions
{
	/// <inheritdoc cref="Sudoku.GetValue"/>
	public static int GetValue(this Sudoku sudoku, Vector2Int position)
	{
		return sudoku.GetValue(position.x, position.y);
	}

	/// <inheritdoc cref="Sudoku.SetValue"/>
	public static void SetValue(this Sudoku sudoku, Vector2Int position, int value)
	{
		sudoku.SetValue(position.x, position.y, value);
	}
	
	/// <inheritdoc cref="Sudoku.IsValidAfterChange"/>
	public static bool IsValidAfterChange(this Sudoku sudoku, Vector2Int position, int value)
	{
		return sudoku.IsValidAfterChange(position.x, position.y, value);
	}
	
	/// <inheritdoc cref="Sudoku.GetAllPositions"/>
	public static IEnumerable<Vector2Int> GetAllPositions(this Sudoku sudoku)
	{
		foreach (var position in Sudoku.GetAllPositions())
			yield return new Vector2Int(position.row, position.column);
	}

	/// <inheritdoc cref="Sudoku.GetValidValuesForPosition"/>
	public static ISet<int> GetValidValuesForPosition(this Sudoku sudoku, Vector2Int position)
	{
		return sudoku.GetValidValuesForPosition(position.x, position.y);
	}
}