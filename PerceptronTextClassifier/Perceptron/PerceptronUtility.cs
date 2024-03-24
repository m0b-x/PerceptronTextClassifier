using System.Collections;
using System.Text;

namespace PerceptronTextClassifier;

public static class PerceptronUtility
{
    public static void TrainPerceptrons(ArffFileReader trainingReader,
    SingleLayerPerceptron[] perceptrons, NormalizationTypes normalizationType)
    {
        if (normalizationType.Equals(NormalizationTypes.With0And1))
        {
            Parallel.For(0, perceptrons.Length, i =>
            {
                foreach(var document in trainingReader.Documents)
                    perceptrons[i].TrainWithDocument(document,
                        (perceptrons[i].TopicEncoding == document.TopicEncoding) ? 1 : 0);
            });
        }
        else if(normalizationType.Equals(NormalizationTypes.With1AndNeg1))
        {
            Parallel.For(0, perceptrons.Length, i =>
            {
                foreach(var document in trainingReader.Documents)
                    perceptrons[i].TrainWithDocument(document,
                        (perceptrons[i].TopicEncoding == document.TopicEncoding) ? 1 : -1);
            });
        }
    }

    public static void TestPerceptrons(ArffFileReader testingReader,
        SingleLayerPerceptron[] perceptrons, NormalizationTypes normalizationType)
    {
        for (int i = 0; i < perceptrons.Length; ++i)
        {
            foreach (var document in testingReader.Documents)
            {
                perceptrons[i].TestWithDocument(document);
            }
        }
    }
    
    public static void DoStringEncodings(ArffFileReader trainingReader, Dictionary<string, int> topicEncodingDictionary,
        ArffFileReader testingReader)
    {
        int ct = 1;
        foreach (var topic in trainingReader.TopicList)
        {
            if (!topicEncodingDictionary.ContainsKey(topic))
            {
                topicEncodingDictionary.Add(topic, ct);
                ct++;
            }
        }
        foreach (var topic in testingReader.TopicList)
        {
            if (!topicEncodingDictionary.ContainsKey(topic))
            {
                topicEncodingDictionary.Add(topic, ct);
                ct++;
            }
        }
        
        foreach (var document in trainingReader.Documents)
        {
            document.TopicEncoding = topicEncodingDictionary[document.Topic];
        }
        foreach (var document in testingReader.Documents)
        {
            document.TopicEncoding = topicEncodingDictionary[document.Topic];
        }
    }
    public static double ApplyActivationFunction(double sum, ActivationFunctions activationType, double threshold = 0.0, double alpha = 1.0)
    {
        switch (activationType)
        {
            case ActivationFunctions.StepFunction:
                return ActivationFunctionsImpl.StepFunction(sum, threshold);
            case ActivationFunctions.SignFunction:
                return ActivationFunctionsImpl.SignFunction(sum);
            case ActivationFunctions.Sigmoid:
                return ActivationFunctionsImpl.Sigmoid(sum);
            case ActivationFunctions.Tanh:
                return ActivationFunctionsImpl.Tanh(sum);
            case ActivationFunctions.ReLU:
                return ActivationFunctionsImpl.ReLU(sum);
            case ActivationFunctions.ELU:
                return ActivationFunctionsImpl.ELU(sum, alpha);
            default:
                throw new ArgumentException("Invalid activation function type.");
        }
    }
}