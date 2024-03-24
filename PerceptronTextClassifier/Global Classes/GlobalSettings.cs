namespace PerceptronTextClassifier;

public static class GlobalSettings
{
    public static NormalizationTypes NormalizationType =
        NormalizationTypes.With1AndNeg1;
    
    public static int MaxPerceptronEpochs = 100;
}