namespace PerceptronTextClassifier.Perceptron;

public static class PerceptronUtility
{
    public static void TrainPerceptrons(ArffFileReader trainingReader,
    SingleLayerPerceptron[] perceptrons)
    {
        Parallel.For(0, perceptrons.Length, i =>
        {
            foreach (var document in trainingReader.Documents)
                perceptrons[i].TrainWithDocument(document,
                    (perceptrons[i].TopicEncoding == document.TopicEncoding) ? 1 : -1);
        });
    }

    public static void TestPerceptrons(ArffFileReader testingReader,
        SingleLayerPerceptron[] perceptrons)
    {
        Parallel.For(0, perceptrons.Length, i =>
        {
            foreach (var document in testingReader.Documents)
            {
                perceptrons[i].TestWithDocument(document);
            }
        });
    }
    
    public static void PrintMetrics2(SingleLayerPerceptron[] perceptrons)
    {
        int tp = 0, tn = 0, fp = 0, fn = 0;
        for (int i = 0; i < perceptrons.Length; i++)
        {
            tp += perceptrons[i].TP;
            tn += perceptrons[i].TN;
            fp += perceptrons[i].FP;
            fn += perceptrons[i].FN;
        }
        // Compute metrics
        double accuracy = (tp + tn) / (double)(tp + tn + fp + fn);
        double precision = tp / (double)(tp + fp);
        double recall = tp / (double)(tp + fn);
        double specificity = tn / (double)(tn + fp);
    
        // Print metrics
        Console.WriteLine($"Evaluation Metrics:\n\nAccuracy:\t{accuracy}\nPrecision:\t{precision}\nRecall:\t\t{recall}\nSpecificity:\t{specificity}\n\n");
    }
    public static void PrintMetrics(SingleLayerPerceptron[] perceptrons)
    {
        double acc = 0.0, pr = 0.0, rec = 0.0, spec = 0.0;
        for (int i = 0; i < perceptrons.Length; i++)
        {
            acc += perceptrons[i].Evaluator.ComputeAccuracy();
            pr += perceptrons[i].Evaluator.ComputePrecision();
            rec += perceptrons[i].Evaluator.ComputeRecall();
            spec += perceptrons[i].Evaluator.ComputeAccuracy();
        }
                               
        // Compute metrics
        double accuracy = acc / perceptrons.Length;
        double precision = pr / perceptrons.Length;
        double recall = rec / perceptrons.Length;
        double specificity = spec / perceptrons.Length;
        // Print metrics
        Console.WriteLine($"Evaluation Metrics:\n\nAccuracy:\t{accuracy}\nPrecision:\t{precision}\nRecall:\t\t{recall}\nSpecificity:\t{specificity}\n\n");
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
    
}