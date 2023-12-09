using GameTheoryEssentials.MathHelpers;
using GameTheoryEssentials.Matrix;
using GameTheoryEssentials.NormalFormGames;

namespace Tests;

public class Week1Tests
{
    [Fact]
    public void Week1()
    {    
        /*
        matrix = np.array([[0, 1, -1], [-1, 0, 1], [1, -1, 0]])
        row_strategy = np.array([[0.1, 0.2, 0.7]])
        column_strategy = np.array([[0.3, 0.2, 0.5]]).transpose()

        rowValue, columnValue = week1.ComputeValuesFor_TwoPlayer_NonZeroSumGame(matrix, -matrix, row_strategy, column_strategy)
        assert rowValue == pytest.approx(0.08)

        br_value_row = week1.BestResponse_Value_To_Row(matrix=matrix, row_strategy=row_strategy)
        br_value_column = week1.BestResponse_Value_To_Column(matrix=matrix, column_strategy=column_strategy)
        assert br_value_row == pytest.approx(-0.6)
        assert br_value_column == pytest.approx(-0.2)
        */
        
        var matrixData = new double[,]
        {
            {0,1,-1},
            {-1,0,1},
            {1,-1,0}
        };
        var matrix = Matrix2D.Create(matrixData);

        var rowStrategyValues = new[,]
        {
            {0.3, 0.2, 0.5}
        };
        var rowPlayerStrategy = RowStrategy.Create(rowStrategyValues);
        
        var columnStrategyValues = new[,]
        {
            {0.1, 0.2, 0.7}
        };
        var columnPlayerStrategy = ColumnStrategy.Create(columnStrategyValues);

        var (rowUtility, _) = MatrixGameEvaluator2D.EvaluateUtilityForBothPlayers(matrix, -matrix, rowPlayerStrategy, columnPlayerStrategy);
        Assert.Equal(-0.08, rowUtility);

        var bestRespondingRowStrat = MatrixGameEvaluator2D.GetBestResponse(matrix, columnPlayerStrategy);
        var bestRespondingColumnStrat = MatrixGameEvaluator2D.GetBestResponse(matrix, rowPlayerStrategy);

        var (_, againstBestResponseColumnUtility) = MatrixGameEvaluator2D.EvaluateUtilityForBothPlayers(matrix, -matrix, bestRespondingRowStrat, columnPlayerStrategy);
        Assert.True(Equality.AreEqualWithTolerance(-0.6, againstBestResponseColumnUtility));

        var (againstBestResponseRowUtility, _) = MatrixGameEvaluator2D.EvaluateUtilityForBothPlayers(matrix, -matrix, rowPlayerStrategy, bestRespondingColumnStrat);
        Assert.True(Equality.AreEqualWithTolerance(-0.2, againstBestResponseRowUtility));
    }
}