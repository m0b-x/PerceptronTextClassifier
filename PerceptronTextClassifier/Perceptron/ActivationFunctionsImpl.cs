namespace PerceptronTextClassifier;

public static class ActivationFunctionsImpl
{
    //Binary activation function -unipolar
    public static int StepFunction(double x, double threshold)
    {
        return x >= threshold ? 1 : 0;
    }
    
    //Binary activation function -multipolar
    public static int SignFunction(double x)
    {
        if (x < 0)
        {
            return -1;
        }
        else if (x > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    
    // Sigmoid function
    public static double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }

    // Hyperbolic tangent (tanh) function
    public static double Tanh(double x)
    {
        return Math.Tanh(x);
    }

    // Rectified Linear Unit (ReLU) function
    public static double ReLU(double x)
    {
        return Math.Max(0, x);
    }

    // Exponential Linear Unit (ELU) function
    public static double ELU(double x, double alpha)
    {
        return x >= 0 ? x : alpha * (Math.Exp(x) - 1);
    }
    
}