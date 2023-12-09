namespace GameTheoryEssentials.Matrix;

public static class Matrix2DExtensions
{
    public static Matrix2D Multiply(this Matrix2D matrix1, Matrix2D matrix2)
    {
        int rowsA = matrix1.Data.GetLength(0);
        int colsA = matrix1.Data.GetLength(1);
        int rowsB = matrix2.Data.GetLength(0);
        int colsB = matrix2.Data.GetLength(1);

        if (colsA != rowsB)
        {
            throw new InvalidOperationException("Matrix dimensions are not suitable for multiplication.");
        }

        var result = new double[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                double sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += matrix1.Data[i, k] * matrix2.Data[k, j];
                }
                result[i, j] = sum;
            }
        }

        return Matrix2D.Create(result);
    }
    
    public static Matrix2D MultiplyElementWise(this Matrix2D matrix1, Matrix2D matrix2)
    {
        int rowsA = matrix1.Data.GetLength(0);
        int colsA = matrix1.Data.GetLength(1);
        int rowsB = matrix2.Data.GetLength(0);
        int colsB = matrix2.Data.GetLength(1);

        if (rowsA != rowsB || colsA != colsB)
        {
            throw new InvalidOperationException("Matrix dimensions are not suitable for element-wise multiplication.");
        }

        var result = new double[rowsA, colsA];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsA; j++)
            {
                result[i, j] = matrix1.Data[i, j] * matrix2.Data[i, j];
            }
        }

        return Matrix2D.Create(result);
    }
    
    
    public static double[] Flatten(this double[,] matrix)
    {
        if (matrix == null)
        {
            throw new ArgumentNullException(nameof(matrix), "Input matrix cannot be null.");
        }

        int numRows = matrix.GetLength(0);
        int numCols = matrix.GetLength(1);

        var flattenedArray = new double[numRows * numCols];

        int index = 0;
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                flattenedArray[index++] = matrix[row, col];
            }
        }

        return flattenedArray;
    }
    
    
    
    public static double[,] ConvertTo2DArray(this double[] inputArray, int numRows, int numCols)
    {
        if (inputArray.Length != numRows * numCols)
        {
            throw new ArgumentException("The dimensions do not match the length of the input array.");
        }

        double[,] resultArray = new double[numRows, numCols];

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                resultArray[i, j] = inputArray[i * numCols + j];
            }
        }

        return resultArray;
    }
}