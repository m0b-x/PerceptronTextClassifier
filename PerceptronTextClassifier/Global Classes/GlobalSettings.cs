namespace PerceptronTextClassifier;

public static class GlobalSettings
{
    public static NormalizationTypes NormalizationType =
        NormalizationTypes.With1And0;
    
    public static int MaxPerceptronIterations = 25;
    public static double DefaultNeuronBias = 1.0;
}