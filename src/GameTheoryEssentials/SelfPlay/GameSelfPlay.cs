using GameTheoryEssentials.Matrix;
using GameTheoryEssentials.NormalFormGames;
using Python.Runtime;

namespace GameTheoryEssentials.SelfPlay;

/// <summary>
/// Rock Paper Scissors self play
/// </summary>
public class GameSelfPlay
{
    private Matrix2D PayoutMatrix { get; }

    private readonly List<RowStrategy> _rowPlayerActionIndex = new();
    private readonly List<ColumnStrategy> _colPlayerActionIndex = new();
    public GameSelfPlay()
    {
        var matrixData = new double[,]
        {
            {0,1,-1},
            {-1,0,1},
            {1,-1,0}
        };
        PayoutMatrix = Matrix2D.Create(matrixData);
    }
    
    public double[] Play(int numberOfTurns = 20, bool averagedSelfPlay = false)
    {
        var random = new Random();
        var actionIndexLimit = PayoutMatrix.Shape.Rows;
        var randomColStrategy = ColumnStrategy.Create(MatrixGameEvaluator2D.CreatePureStrategy(actionIndexLimit, random.Next(actionIndexLimit)));
        var randomRowStrategy = RowStrategy.Create(MatrixGameEvaluator2D.CreatePureStrategy(actionIndexLimit, random.Next(actionIndexLimit)));
        
        _rowPlayerActionIndex.Add(randomRowStrategy);
        _colPlayerActionIndex.Add(randomColStrategy);

        var exploitabilities = new List<double>();

        for (int i = 0; i < numberOfTurns; i++)
        {
            var rowStrat = MatrixGameEvaluator2D.GetBestResponse(PayoutMatrix, _colPlayerActionIndex.Last());
            var colStrat = MatrixGameEvaluator2D.GetBestResponse(PayoutMatrix, _rowPlayerActionIndex.Last());

            
            var avgRowStrat = AverageRowStrategy(_rowPlayerActionIndex);
            var avgColStrat = AverageColumnStrategy(_colPlayerActionIndex);
            
            if (averagedSelfPlay)
            {
                colStrat = MatrixGameEvaluator2D.GetBestResponse(PayoutMatrix, avgRowStrat);
                rowStrat = MatrixGameEvaluator2D.GetBestResponse(PayoutMatrix, avgColStrat);
            }
            
            _rowPlayerActionIndex.Add(rowStrat);
            _colPlayerActionIndex.Add(colStrat);

            var exploitability = MatrixGameEvaluator2D.ComputeExploitability(PayoutMatrix, avgRowStrat, avgColStrat);
            exploitabilities.Add(exploitability);
        }
        
        return exploitabilities.ToArray();
    }

    static RowStrategy AverageRowStrategy(List<RowStrategy> rowPlayerActionIndex)
    {
        double[] vectorSum = new double[rowPlayerActionIndex[0].Data.Flatten().Length];
        foreach (var strategy in rowPlayerActionIndex)
        {
            for (int i = 0; i < strategy.Data.Flatten().Length; i++)
            {
                vectorSum[i] += strategy.Data.Flatten()[i];
            }
        }

        double[] averageVector = vectorSum.Select(v => v / rowPlayerActionIndex.Count).ToArray();
        var a = averageVector.ConvertTo2DArray(1, 3);
        return RowStrategy.Create(a);
    }

    static ColumnStrategy AverageColumnStrategy(List<ColumnStrategy> rowPlayerActionIndex)
    {
        double[] vectorSum = new double[rowPlayerActionIndex[0].Data.Flatten().Length];
        foreach (var strategy in rowPlayerActionIndex)
        {
            for (int i = 0; i < strategy.Data.Flatten().Length; i++)
            {
                vectorSum[i] += strategy.Data.Flatten()[i];
            }
        }

        double[] averageVector = vectorSum.Select(v => v / rowPlayerActionIndex.Count).ToArray();
        var a = averageVector.ConvertTo2DArray(1 ,3);
        return ColumnStrategy.Create(a);
    }
    
    static void DisplayVectors(List<double[]> vectors)
    {
        foreach (var vector in vectors)
        {
            DisplayVector(vector);
        }
    }

    static void DisplayVector(double[] vector)
    {
        Console.WriteLine("[" + string.Join(", ", vector) + "]");
    }

    static void Plot(dynamic data)
    {
        using (Py.GIL()) // Acquire the Python GIL (Global Interpreter Lock)
        {
            dynamic matplotlib = Py.Import("matplotlib.pyplot");
             data = new double[] { 1, 2, 3, 4, 5 };

            // Plot the data
            matplotlib.plot(data);
            matplotlib.show();
        }
    }
}