namespace GameTheoryEssentials.MathHelpers;

public class Equality
{
    public static bool AreEqualWithTolerance(double a, double b, double epsilon = 1e-9)
    {
        return Math.Abs(a - b) < epsilon;
    }
}