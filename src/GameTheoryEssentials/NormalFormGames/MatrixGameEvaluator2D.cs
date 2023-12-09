using GameTheoryEssentials.Matrix;

namespace GameTheoryEssentials.NormalFormGames;

public static partial class MatrixGameEvaluator2D
{
    /// <summary>
    /// Computer how much the players could improve if they were to switch to the best response
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="rowPlayerStrategy"></param>
    /// <param name="columnPlayerStrategy"></param>
    /// <returns>value for each of the two players representing how much they can improve their utilities.</returns>
    public static (double, double) ComputeDeltas(Matrix2D matrix , RowStrategy rowPlayerStrategy, ColumnStrategy columnPlayerStrategy)
    {
        var (rowUtility, columnUtility) = EvaluateUtilityForBothPlayers(matrix, -matrix, rowPlayerStrategy, columnPlayerStrategy);
        
        
        var bestRespondingRowStrat = GetBestResponse(matrix, columnPlayerStrategy);
        var bestRespondingColumnStrat = GetBestResponse(matrix, rowPlayerStrategy);

        var (bestResponseRowUtility, _) = EvaluateUtilityForBothPlayers(matrix, -matrix, bestRespondingRowStrat, columnPlayerStrategy);
        var (_, bestResponseColumnUtility) = EvaluateUtilityForBothPlayers(matrix, -matrix, rowPlayerStrategy, bestRespondingColumnStrat);
        

        return (bestResponseRowUtility - rowUtility, bestResponseColumnUtility -columnUtility);
    }

    public static double NashConv(Matrix2D matrix, RowStrategy rowPlayerStrategy, ColumnStrategy columnPlayerStrategy)
    {
        var deltas = ComputeDeltas(matrix, rowPlayerStrategy, columnPlayerStrategy);
        return deltas.Item1 + deltas.Item2; // sum of the deltas
    }

    /// <summary>
    /// Compute exploitability for a zero sum game.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="rowPlayerStrategy"></param>
    /// <param name="columnPlayerStrategy"></param>
    /// <returns></returns>
    public static double ComputeExploitability(Matrix2D matrix, RowStrategy rowPlayerStrategy, ColumnStrategy columnPlayerStrategy)
    {
        return NashConv(matrix, rowPlayerStrategy, columnPlayerStrategy) / 2; // divided by a number of players, in this case we consider only 2
    }
    
    /// <summary>
    /// Computes epsilon as defined for epsilon-Nash equilibrium
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="rowPlayerStrategy"></param>
    /// <param name="columnPlayerStrategy"></param>
    /// <returns></returns>
    public static double ComputeEpsilon(Matrix2D matrix, RowStrategy rowPlayerStrategy, ColumnStrategy columnPlayerStrategy)
    {
        var deltas = ComputeDeltas(matrix, rowPlayerStrategy, columnPlayerStrategy);
        return deltas.Item1 > deltas.Item2 ? deltas.Item1 : deltas.Item2; // return maximum of the deltas, in this case we got only 2 elements.
    }

    public static (double, double) EvaluateUtilityForBothPlayers(Matrix2D rowPlayerUtilityMatrix, Matrix2D columnPlayerUtilityMatrix, RowStrategy rowStrategy, ColumnStrategy columnStrategy)
    {
        var probabilityMatrix = rowStrategy.Multiply(columnStrategy);
        probabilityMatrix.CheckAsProbabilityMatrix();

        var rowUtilityMatrix = rowPlayerUtilityMatrix * probabilityMatrix;
        var columnUtilityMatrix = columnPlayerUtilityMatrix * probabilityMatrix;

        var rowUtility = rowUtilityMatrix.Sum();
        var columnUtility = columnUtilityMatrix.Sum();

        return (rowUtility, columnUtility);
    }
    
    public static RowStrategy GetBestResponse(Matrix2D matrix, ColumnStrategy columnStrat)
    {
        var expectedPayoffs = columnStrat.Multiply(matrix.Transpose());
        int maxPayoffIndex = Array.IndexOf(expectedPayoffs.Data.Flatten(), expectedPayoffs.Data.Flatten().Max());
        var res = CreatePureStrategy(matrix.Data.GetLength(0), maxPayoffIndex);
        return RowStrategy.Create(res);
    }
    
    
    public static ColumnStrategy GetBestResponse(Matrix2D matrix, RowStrategy rowStrat)
    {
        var expectedPayoffs = matrix.Multiply(rowStrat);
        int maxPayoffIndex = Array.IndexOf(expectedPayoffs.Data.Flatten(), expectedPayoffs.Data.Flatten().Max());
        var res = CreatePureStrategy(matrix.Data.GetLength(1), maxPayoffIndex);
        return ColumnStrategy.Create(res);
    }
    
    public static double[,] CreatePureStrategy(int len, int index)
    {
        double[,] response = new double[1, len];

        for (int i = 0; i < len; i++)
        {
            response[0, i] = (i == index) ? 1 : 0;
        }

        return response;
    }
}
