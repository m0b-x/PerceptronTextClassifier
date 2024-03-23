using System;

namespace PerceptronTextClassifier;

public class Evaluator
{
    public int TruePositive { get; set; }
    public int TrueNegative { get; set; }
    public int FalsePositive { get; set; }
    public int FalseNegative { get; set; }
    public int NumberOfClasses { get; set; }

    public void InitialiseMetrics(int numberOfClases)
    {
        TruePositive = 0;
        TrueNegative = 0;
        FalsePositive = 0;
        FalseNegative = 0;
        NumberOfClasses = numberOfClases;
    }

    public double ComputeAccuracy()
    {
        int total = TruePositive + TrueNegative + FalsePositive + FalseNegative;
        if (total == 0)
            throw new InvalidOperationException("[Accuracy-Error]: All variables are 0, cannot compute division.");
        
        double accuracy = (double)(TruePositive + TrueNegative) / total;
        return accuracy;
    }

    public double ComputeRecall()
    {
        int actualPositive = TruePositive + FalseNegative;
        if (actualPositive == 0)
            throw new InvalidOperationException("[Recall-Error]: TruePos + FalseNeg is 0, cannot compute division.");
        
        double recall = (double)TruePositive / actualPositive;
        return recall;
    }

    public double ComputeSpecificity()
    {
        int actualNegative = TrueNegative + FalsePositive;
        if (actualNegative == 0)
            throw new InvalidOperationException("[Specificity-Error]: TrueNeg + FalsePoz is 0, cannot compute division.");
        
        double specificity = (double)TrueNegative / actualNegative;
        return specificity;
    }

    public double ComputePrecision()
    {
        int predictedPositive = TruePositive + FalsePositive;
        if (predictedPositive == 0)
            throw new InvalidOperationException("[Precision-Error]: TruePos + FalsePos is 0, cannot compute division.");
        
        double precision = (double)TruePositive / predictedPositive;
        return precision;
    }
}
