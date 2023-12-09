namespace GameTheoryEssentials.Matrix;

public class ColumnStrategy : Matrix2D
{
    private ColumnStrategy(double[,] matrixData) : base(matrixData)
    {
    }

    public static ColumnStrategy Create(double[,] matrixData)
    {
        if (matrixData.GetLength(0) != 1)
        {
            throw new ArgumentException("Column strategy has to have only one row.");
        }
        return new ColumnStrategy(matrixData);
    }
}