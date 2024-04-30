using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class represents a Sudoku board. It encapsulates the values for each position on the board along with methods
/// to manipulate them and validate the state of the board.
/// </summary>
public class Sudoku
{
	private readonly int[,] values;

	/// <summary>
	/// Initializes a new empty Sudoku.
	/// </summary>
	public Sudoku() : this(new int[9, 9])
	{
	}

	/// <summary>
	/// Initializes a new Sudoku using the given values.
	/// </summary>
	public Sudoku(int[,] values)
	{
		if (values.GetLength(0) != 9 || values.GetLength(1) != 9)
			throw new InvalidOperationException("A sudoku board must be 9x9.");

		this.values = values;
	}

	/// <summary>
	/// Returns an identical copy of the sudoku instance.
	/// </summary>
	public Sudoku Clone()
	{
		var sudoku = new Sudoku();
		
		for (int row = 0; row < 9; row++)
		for (int column = 0; column < 9; column++)
			sudoku.values[row, column] = values[row, column];
		
		return sudoku;
	}

	/// <summary>
	/// Returns true when the sudoku is complete, i.e. all values are valid and non-zero.
	/// </summary>
	public bool IsComplete()
	{
		return IsValid() && AllValues.All(x => x != 0);
	}

	/// <summary>
	/// Returns the current value at the specified row and column.
	/// </summary>
	public int GetValue(int row, int column) => values[row, column];

	/// <summary>
	/// Updates the current value at the specified row and column.
	/// Note: value must be either 0 (representing null) or between 1 and 9.
	///
	/// Use <see cref="IsValidAfterChange"/> before calling this method to avoid making a change that results in an invalid Sudoku.
	/// </summary>
	public void SetValue(int row, int column, int value)
	{
		if (!IsValueAllowed(value))
			throw new InvalidOperationException();

		values[row, column] = value;
	}

	/// <summary>
	/// Returns all values in the specified row (left to right).
	/// </summary>
	public IEnumerable<int> GetRow(int row)
	{
		for (int i = 0; i < 9; i++)
			yield return values[row, i];
	}

	/// <summary>
	/// Returns all values in the specified column (top to bottom).
	/// </summary>
	public IEnumerable<int> GetColumn(int column)
	{
		for (int i = 0; i < 9; i++)
			yield return values[i, column];
	}

	/// <summary>
	/// Returns all values in the specified block (row-wise, top to bottom).
	/// </summary>
	public IEnumerable<int> GetBlock(int blockRow, int blockColumn)
	{
		for (int row = blockRow * 3; row < blockRow * 3 + 3; row++)
		for (int column = blockColumn * 3; column < blockColumn * 3 + 3; column++)
			yield return values[row, column];
	}

	/// <summary>
	/// Returns true if setting the specified value at the given position would result in a valid sudoku.
	/// Does not modify the sudoku in any way.
	/// </summary>
	public bool IsValidAfterChange(int row, int column, int newValue)
	{
		HashSet<int> validationSet = new HashSet<int>();

		var currentValue = GetValue(row, column);

		var modifiedRow = GetRow(row).RemoveOne(currentValue).Append(newValue);
		// Note: It is important that we only remove at most one instance of the current value (there could be duplicates!)

		if (!ValidateGroup(modifiedRow, validationSet)) return false;

		var modifiedColumn = GetColumn(column).RemoveOne(currentValue).Append(newValue);
		if (!ValidateGroup(modifiedColumn, validationSet)) return false;

		var modifiedBlock = GetBlock(row / 3, column / 3).RemoveOne(currentValue).Append(newValue);
		if (!ValidateGroup(modifiedBlock, validationSet)) return false;

		return true;
	}

	/// <summary>
	/// Returns the set of values (excluding 0) which are valid for the given position, i.e. values that are not currently
	/// contained in the given row, column, or block.
	/// </summary>
	public ISet<int> GetValidValuesForPosition(int row, int column)
	{
		var possibleValues = new HashSet<int>(Enumerable.Range(1, 9));
		possibleValues.ExceptWith(GetRow(row));
		possibleValues.ExceptWith(GetColumn(column));
		possibleValues.ExceptWith(GetBlock(row / 3, column / 3));
		return possibleValues;
	}

	/// <summary>
	/// Returns all values in the sudoku.
	/// </summary>
	public IEnumerable<int> AllValues
	{
		get
		{
			for (int i = 0; i < 9; i++)
			for (int j = 0; j < 9; j++)
				yield return values[i, j];
		}
	}

	/// <summary>
	/// Returns true if the current configuration is valid according to the rules of sudoku.
	/// </summary>
	public bool IsValid()
	{
		if (AllValues.Any(value => !IsValueAllowed(value)))
			return false;

		HashSet<int> validationSet = new HashSet<int>();

		for (int i = 0; i < 9; i++)
		{
			if (!ValidateGroup(GetRow(i), validationSet)) return false;
			if (!ValidateGroup(GetColumn(i), validationSet)) return false;
			if (!ValidateGroup(GetBlock(i / 3, i % 3), validationSet)) return false;
		}

		return true;
	}

	public override string ToString()
	{
		var stringBuilder = new System.Text.StringBuilder();

		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
				stringBuilder.Append(values[i, j]).Append(" ");

			stringBuilder.Append("\n");
		}

		return stringBuilder.ToString();
	}

	/// <summary>
	/// Checks whether the specified group of values contains any non-zero duplicates by adding them to the provided hashset.
	/// </summary>
	static bool ValidateGroup(IEnumerable<int> group, HashSet<int> validationSet)
	{
		validationSet.Clear();

		foreach (var value in group)
			if (value != 0 && !validationSet.Add(value))
				return false; // Duplicate value.

		return true;
	}

	/// <summary>
	/// Returns true if the specified value can be inserted in a sudoku.
	/// A value of 0 is allowed and represents null.
	/// </summary>
	public static bool IsValueAllowed(int value) => value >= 0 && value <= 9;

	private static readonly Random random = new Random();

	/// <summary>
	/// Generates a fully solved sudoku by following a simple method of cyclically permuting a sequence containing
	/// the numbers 1 through 9 in random order across the rows.
	/// </summary>
	static int[,] GenerateTrivialSolution()
	{
		var seed = Enumerable.Range(1, 9).OrderBy(x => random.Next()).ToArray();

		var values = new int[9, 9];

		for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			values[i, j] = seed[(j + 3 * i + i / 3) % 9];

		/*
		 * This is what it looks like if seed is { 1, 2, 3, 4, 5, 6, 7, 8, 9 }:
		 * 
		 * 1 2 3 4 5 6 7 8 9
		 * 4 5 6 7 8 9 1 2 3
		 * 7 8 9 1 2 3 4 5 6
		 * 2 3 4 5 6 7 8 9 1
		 * 5 6 7 8 9 1 2 3 4
		 * 8 9 1 2 3 4 5 6 7
		 * 3 4 5 6 7 8 9 1 2
		 * 6 7 8 9 1 2 3 4 5
		 * 9 1 2 3 4 5 6 7 8
		 */

		return values;
	}

	/// <summary>
	/// Reused to avoid allocations.
	/// </summary>
	[ThreadStatic] private static int[] rowPermutations;

	/// <summary>
	/// Reused to avoid allocations.
	/// </summary>
	[ThreadStatic] private static int[] columnPermutations;

	/// <summary>
	/// Generates a random Sudoku with the specified number of revealed values.
	/// Note: the generated Sudoku is not guaranteed to have a unique solution, i.e. there may be more than one possible
	/// final configuration where all values have been set. Real Sudokus should have a unique solution.
	/// </summary>
	public static Sudoku Generate(int revealedValues)
	{
		if (revealedValues > 81 || revealedValues < 0)
			throw new InvalidOperationException($"{nameof(revealedValues)} must be between 0 and 81");

		var trivialSolution = GenerateTrivialSolution();

		// Permute the trivial solution randomly
		InitializeRandomIndexPermutationArray(ref rowPermutations);
		InitializeRandomIndexPermutationArray(ref columnPermutations);

		var permutedSolution = new int[9, 9];
		for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			permutedSolution[i, j] = trivialSolution[rowPermutations[i], columnPermutations[j]];

		// Remove values until only the specified amount remains.
		var amountToRemove = 81 - revealedValues;
		var positionsToErase = GetAllPositions() // Returns a sequence containing all positions on the board
			.OrderBy(x => random.Next()) // Orders them randomly
			.Take(amountToRemove); // Takes {amountToRemove} elements from the start of the sequence.

		foreach (var position in positionsToErase)
			permutedSolution[position.row, position.column] = 0;

		return new Sudoku(permutedSolution);
	}

	/// <summary>
	/// Initializes the target array with a randomly generated permutation for rows or columns which preserves the validity of the sudoku.
	/// The value of the i:th element in the array is the new index of the i:th row or column.
	/// </summary>
	private static void InitializeRandomIndexPermutationArray(ref int[] array)
	{
		if (array == null)
			array = new int[9];

		for (int i = 0; i < 3; i++)
			GetRandomPartialPermutationArray().CopyTo(array, i * 3);

		var blockPermutation = GetRandomPartialPermutationArray();

		for (int i = 0; i < 9; i++)
			array[i] += blockPermutation[i / 3] * 3;
	}

	/// <summary>
	/// Returns a random permutation of the numbers 0, 1 and 2.
	/// </summary>
	private static int[] GetRandomPartialPermutationArray() => partialPermutationArrays[random.Next(6)];

	/// <summary>
	/// All permutations of the numbers 0, 1 and 2.
	/// </summary>
	private static readonly int[][] partialPermutationArrays =
	{
		new[] { 0, 1, 2 },
		new[] { 1, 2, 0 },
		new[] { 2, 0, 1 },
		new[] { 1, 0, 2 },
		new[] { 0, 2, 1 },
		new[] { 2, 1, 0 }
	};

	/// <summary>
	/// Returns a sequence containing every possible coordinate on a sudoku board.
	/// </summary>
	public static IEnumerable<(int row, int column)> GetAllPositions()
	{
		for (int row = 0; row < 9; row++)
		for (int column = 0; column < 9; column++)
			yield return (row, column);
	}
}