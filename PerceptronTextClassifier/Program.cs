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

        var trainingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Training_SVM_1309.0.arff");
        var testingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Testing_SVM_1309.0.arff");
        
        ArffFileReader trainingReader =  new ArffFileReader(trainingFilePath);
        ArffFileReader testingReader =  new ArffFileReader(testingFilePath);

        Dictionary<string, int> topicEncodingDictionary = new(trainingFilePath.Length + testingFilePath.Length);
        
        ///////////////////
        //Encode the topics
        ///////////////////
        PerceptronUtility.DoStringEncodings(trainingReader, topicEncodingDictionary, testingReader);
        
        Console.WriteLine("Topics Encoded");
        
        ///////////////////
        //Train Perceptrons
        ///////////////////
        SingleLayerPerceptron[] perceptrons =
            new SingleLayerPerceptron[trainingReader.TopicList.Count];

        int perceptronCt = 0;
        foreach (var topic in trainingReader.TopicList)
        {
            var perceptron = new SingleLayerPerceptron(
                trainingReader.NumberOfAttributes,
                learningRate: 0.1,
                topic,
                topicEncodingDictionary[topic]);
            perceptrons[perceptronCt] = perceptron;
            
            perceptronCt++;
        }
        Console.WriteLine($"Perceptrons were creeated at {stopwatch.Elapsed}.");
        
        //Training
        PerceptronUtility.TrainPerceptrons(
            trainingReader: trainingReader,
            perceptrons: perceptrons,
            normalizationType: GlobalSettings.NormalizationType);
        Console.WriteLine($"Perceptrons were trained at {stopwatch.Elapsed}.");
        
        //Testing
        PerceptronUtility.TestPerceptrons(
            testingReader: testingReader,
            perceptrons: perceptrons,
            normalizationType: GlobalSettings.NormalizationType);
        Console.WriteLine($"Perceptrons were tested at {stopwatch.Elapsed}.");
        
        //Stop stopwatch
        stopwatch.Stop();
        Console.WriteLine($"Stopwatch stopped at: {stopwatch.Elapsed}");
        
        //Print Metrics
        GlobalEvaluator.PrintEvaluationMetrics();
        ;
    }
}