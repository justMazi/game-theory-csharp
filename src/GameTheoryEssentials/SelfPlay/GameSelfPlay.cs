﻿using GameTheoryEssentials.Matrix;
using GameTheoryEssentials.NormalFormGames;

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
    
    public void Play(int numberOfTurns = 700)
    {
        var random = new Random();
        var actionIndexLimit = PayoutMatrix.Shape.Rows;
        var randomColStrategy = ColumnStrategy.Create(MatrixGameEvaluator2D.CreatePureStrategy(actionIndexLimit, random.Next(actionIndexLimit)));
        var randomRowStrategy = RowStrategy.Create(MatrixGameEvaluator2D.CreatePureStrategy(actionIndexLimit, random.Next(actionIndexLimit)));
        
        _rowPlayerActionIndex.Add(randomRowStrategy);
        _colPlayerActionIndex.Add(randomColStrategy);

        for (int i = 0; i < numberOfTurns; i++)
        {
            var naiveRowStrat = MatrixGameEvaluator2D.GetBestResponse(PayoutMatrix, _colPlayerActionIndex.Last());
            var naiveColStrat = MatrixGameEvaluator2D.GetBestResponse(PayoutMatrix, _rowPlayerActionIndex.Last());

            _rowPlayerActionIndex.Add(naiveRowStrat);
            _colPlayerActionIndex.Add(naiveColStrat);
            
            var avgRowStrat = AverageRowStrategy(_rowPlayerActionIndex);
            var avgColStrat = AverageColumnStrategy(_colPlayerActionIndex);
            var exploitability = MatrixGameEvaluator2D.ComputeExploitability(PayoutMatrix, avgRowStrat, avgColStrat);
            Console.WriteLine(exploitability);
        }
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
}