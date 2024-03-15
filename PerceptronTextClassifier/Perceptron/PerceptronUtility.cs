using System.Collections;

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
}