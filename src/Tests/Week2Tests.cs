using GameTheoryEssentials.MathHelpers;
using GameTheoryEssentials.Matrix;
using GameTheoryEssentials.NormalFormGames;

namespace Tests;

public class Week2Tests
{
    [Fact]
    public void Week2()
    {    
        /*
        matrix = np.array([[0, 1, -1], [-1, 0, 1], [1, -1, 0]])
        row_strategy = np.array([[0.1, 0.2, 0.7]])
        column_strategy = np.array([[0.3, 0.2, 0.5]]).transpose()

        delta_row, delta_column = week3.compute_deltas(matrix=matrix, row_strategy=row_strategy, column_strategy=column_strategy)

        assert delta_row == pytest.approx(0.12)
        assert delta_column == pytest.approx(0.68)
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
        
        var (rowPlayerDelta, columnPlayerDelta) = MatrixGameEvaluator2D.ComputeDeltas(matrix, rowPlayerStrategy, columnPlayerStrategy);
        Assert.True(Equality.AreEqualWithTolerance(0.68, rowPlayerDelta));
        Assert.True(Equality.AreEqualWithTolerance(0.12, columnPlayerDelta));
    }
}