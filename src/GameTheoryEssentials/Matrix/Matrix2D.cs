namespace GameTheoryEssentials.Matrix;

public class Matrix2D
{
    public Shape2D Shape { get; }
    public double[,] Data { get; }
    

    protected Matrix2D(double[,] matrixData)
    {
        Data = matrixData;
        Shape = new Shape2D(matrixData.GetLength(0), matrixData.GetLength(1));
    }

    public static Matrix2D Create(double[,] matrixData)
    {
        return new Matrix2D(matrixData);
    }

    
    public static Matrix2D operator -(Matrix2D matrix)
    {
        return matrix.Negate();
    }
    
    public static Matrix2D operator *(Matrix2D matrix1, Matrix2D matrix2)
    {
        return matrix1.MultiplyElementWise(matrix2);
    }
    
    public Matrix2D Transpose()
    {
        var newData = new double[Shape.Columns, Shape.Rows];

        for (int i = 0; i < Shape.Rows; i++)
        {
            for (int j = 0; j < Shape.Columns; j++)
            {
                newData[j, i] = Data[i, j];
            }
        }

        return new Matrix2D(newData);
    }
    
    
    
    public static Matrix2D Add(Matrix2D matrix1, Matrix2D matrix2)
    {
        if (matrix1.Shape.Rows != matrix2.Shape.Rows || matrix1.Shape.Rows != matrix2.Shape.Columns)
        {
            throw new ArgumentException("Matrix dimensions do not match for addition");
        }

        var newData = new double[matrix1.Shape.Rows,matrix1.Shape.Columns];
        
        for (int i = 0; i < matrix1.Shape.Rows; i++)
        {
            for (int j = 0; j < matrix1.Shape.Columns; j++)
            {
                newData[i, j] = matrix1.Data[i, j] + matrix2.Data[i, j];
            }
        }

        return new Matrix2D(newData);
    }

    public static Matrix2D Subtract(Matrix2D matrix1, Matrix2D matrix2)
    {
        if (matrix1.Shape.Rows != matrix2.Shape.Rows || matrix1.Shape.Columns != matrix2.Shape.Columns)
        {
            throw new ArgumentException("Matrix dimensions do not match for subtraction");
        }

        var newData = new double[matrix1.Shape.Rows,matrix1.Shape.Columns];

        for (int i = 0; i < matrix1.Shape.Rows; i++)
        {
            for (int j = 0; j < matrix1.Shape.Columns; j++)
            {
                newData[i, j] = matrix1.Data[i, j] - matrix2.Data[i, j];
            }
        }

        return new Matrix2D(newData);
    }

    public Matrix2D Negate()
    {
        var newData = new double[Shape.Rows, Shape.Columns];

        for (int i = 0; i < Shape.Rows; i++)
        {
            for (int j = 0; j < Shape.Columns; j++)
            {
                newData[i, j] = -Data[i, j];
            }
        }

        return new Matrix2D(newData);
    }
    
    public double Sum()
    {
        double res = 0;
        for (int i = 0; i < Shape.Rows; i++)
        {
            for (int j = 0; j < Shape.Columns; j++)
            {
                res += Data[i, j];
            }
        }

        return res;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if sum is not 1 including the tolerance.</exception>
    public Matrix2D CheckAsProbabilityMatrix(double epsilon = 1e-9)
    {
        double res = 0;
        for (int i = 0; i < Shape.Rows; i++)
        {
            for (int j = 0; j < Shape.Columns; j++)
            {
                res += Data[i, j];
            }
        }

        if (!MathHelpers.Equality.AreEqualWithTolerance(Sum(), 1, epsilon))
        {
            throw new ArgumentException($"Sum of values of matrix representing probability is '{res}' instead of '1'");
        }

        return this;
    }
    
    public void Display()
    {
        for (int i = 0; i < Shape.Rows; i++)
        {
            for (int j = 0; j < Shape.Columns; j++)
            {
                Console.Write(Data[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}