using System.Collections;
using System.Text;

namespace PerceptronTextClassifier;

public static class PerceptronUtility
{
    public static Dictionary<string, BitArray> TopicEncodingDictionary = new();
    
    public static Dictionary<string, BitArray> ComputeTopicEncoding(List<string> topics)
    {
        var numberOfBits = topics.Count;
        int topicCounter = 0;
        foreach (var topic in topics)
        {
            var bitArray = new BitArray(numberOfBits);
            bitArray[topicCounter] = true;
            TopicEncodingDictionary.Add(topic, bitArray);
            topicCounter++;
        }
        return TopicEncodingDictionary;
    }
    
        
    public static StringBuilder BitArrayToStringBuilder(BitArray bitArray)
    {
        if (bitArray == null)
            throw new ArgumentNullException(nameof(bitArray));

        StringBuilder stringBuilder = new StringBuilder(bitArray.Length);
        for (int i = 0; i < bitArray.Length; i++)
        {
            stringBuilder.Append(bitArray[i] ? '1' : '0');
        }
        return stringBuilder;
    }
        
    public static int BitArrayToInteger(BitArray bitArray)
    {
        int integerValue = 0;

        for (int i = 0; i < bitArray.Length; i++)
        {
            if (bitArray.Get(i))
            {
                integerValue += (int)Math.Pow(2, i);
            }
        }

        return integerValue;
    }
    public static int[] ReturnAttributePresenceArrayInt(bool[] attributePresence, NormalizationTypes type)
    {
        if (attributePresence == null)
        {
            throw new ArgumentNullException(nameof(attributePresence), "Presence array is not initialized.");
        }
            
        if (type == NormalizationTypes.With0And1)
        {
            int[] intArray = new int[attributePresence.Length];
            for (int i = 0; i < attributePresence.Length; i++)
            {
                intArray[i] = attributePresence[i] ? 1 : 0;
            }
            return intArray;
        }
        else if (type == NormalizationTypes.With1AndNeg1)
        {
            int[] intArray = new int[attributePresence.Length];
            for (int i = 0; i < attributePresence.Length; i++)
            {
                intArray[i] = attributePresence[i] ? 1 : -1;
            }
            return intArray;
        }

        return null;
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