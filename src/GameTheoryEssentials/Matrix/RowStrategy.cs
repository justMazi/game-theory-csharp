namespace GameTheoryEssentials.Matrix;

public class RowStrategy : Matrix2D
{
    private RowStrategy(double[,] matrixData) : base(matrixData)
    {
    }

    public static RowStrategy Create(double[,] matrixData)
    {
        
        var transpose = Matrix2D.Create(matrixData).Transpose();
        
        if (transpose.Data.GetLength(1) != 1)
        {
            throw new ArgumentException("Row strategy has to have only one column.");
        }
        return new RowStrategy(transpose.Data);
    }
}