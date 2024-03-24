using System.Diagnostics;

namespace PerceptronTextClassifier;

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        
        string workingDirectory = Environment.CurrentDirectory;
        string slnDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;

        var trainingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Training_SVM_100.0.arff");
        var testingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Testing_SVM_100.0.arff");
        
        ArffFileReader trainingReader =  new ArffFileReader(trainingFilePath);
        ArffFileReader testingReader =  new ArffFileReader(testingFilePath);
        
        var topicEncodings = PerceptronUtility.ComputeTopicEncoding(trainingReader.TopicList);
        
        foreach (var document in trainingReader.Documents)
        {
            document.TopicEncoding = topicEncodings[document.Topic];
        }
        
        ///////////////////
        //Train Perceptrons
        ///////////////////
        SingleLayerPerceptron[] perceptrons =
            new SingleLayerPerceptron[topicEncodings.Keys.Count];

        int perceptronCt = 0;
        Dictionary<string, int> trainingDataIndexDictionary = new(topicEncodings.Keys.Count);
        foreach (var topic in topicEncodings.Keys)
        {
            var perceptron = new SingleLayerPerceptron(
                trainingReader.NumberOfAttributes,
                learningRate: 0.1,
                topic);
            perceptrons[perceptronCt] = perceptron;
            
            trainingDataIndexDictionary[topic] = perceptronCt;
            perceptronCt++;
        }
        
        //Training
        PerceptronUtility.TrainPerceptrons(
            trainingReader: trainingReader,
            perceptrons: perceptrons,
            normalizationType: GlobalSettings.NormalizationType);
        
        //Testing
        PerceptronUtility.TestPerceptrons(
            testingReader: testingReader,
            perceptrons: perceptrons,
            normalizationType: GlobalSettings.NormalizationType);
        Console.WriteLine("Perceptrons tested.");
        
        //Print Metrics
        GlobalEvaluator.PrintEvaluationMetrics();
        
        stopwatch.Stop();
        Console.WriteLine($"Stopwatch Time: {stopwatch.Elapsed}");
    }
}