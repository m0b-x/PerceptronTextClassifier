namespace PerceptronTextClassifier;

public static class GlobalEvaluator
{
    public static int TruePositive { get; set; }
    public static int TrueNegative { get; set; }
    public static int FalsePositive { get; set; }
    public static int FalseNegative { get; set; }

    public static void ResetMetrics()
    {
        TruePositive = 0;
        TrueNegative = 0;
        FalsePositive = 0;
        FalseNegative = 0;
    }

    public static double ComputeAccuracy()
    {
        int total = TruePositive + TrueNegative + FalsePositive + FalseNegative;
        if (total == 0)
            return 0;
        //throw new InvalidOperationException("[Accuracy-Error]: All variables are 0, cannot compute division.");

        double accuracy = (double) (TruePositive + TrueNegative) / total;
        return accuracy;
    }

    public static double ComputeRecall()
    {
        int actualPositive = TruePositive + FalseNegative;
        if (actualPositive == 0)
            return 0;
        //throw new InvalidOperationException("[Recall-Error]: TruePos + FalseNeg is 0, cannot compute division.");

        double recall = (double) TruePositive / actualPositive;
        return recall;
    }

    public static double ComputeSpecificity()
    {
        int actualNegative = TrueNegative + FalsePositive;
        if (actualNegative == 0)
            return 0;
        //throw new InvalidOperationException("[Specificity-Error]: TrueNeg + FalsePoz is 0, cannot compute division.");

        double specificity = (double) TrueNegative / actualNegative;
        return specificity;
    }

    public static double ComputePrecision()
    {
        int predictedPositive = TruePositive + FalsePositive;
        if (predictedPositive == 0)
            return 0;
        //throw new InvalidOperationException("[Precision-Error]: TruePos + FalsePos is 0, cannot compute division.");

        double precision = (double) TruePositive / predictedPositive;
        return precision;
    }

    public static void PrintEvaluationMetrics()
    {
        var accuracy = ComputeAccuracy();
        var precision = ComputePrecision();
        var recall = ComputeRecall();
        var specificity = ComputeSpecificity();
        Console.WriteLine(
            $"Evaluation Metrics:\n Accuracy:\t{accuracy}\n Precision:\t{precision}\n Recall:\t{recall}\n Specificity:\t{specificity}\n");
    }
}
