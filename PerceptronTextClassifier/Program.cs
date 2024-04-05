using System.Diagnostics;
using PerceptronTextClassifier.Perceptron;

namespace PerceptronTextClassifier;

class Program
{
    static void Main()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        
        string workingDirectory = Environment.CurrentDirectory;
        string slnDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;

        var trainingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Training_SVM_1309.0.arff");
        var testingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Testing_SVM_1309.0.arff");
        
        ArffFileReader trainingReader =  new ArffFileReader(trainingFilePath);
        ArffFileReader testingReader =  new ArffFileReader(testingFilePath);

        ///////////////////
        //Encode the topics
        ///////////////////
        
        Dictionary<string, int> topicEncodingDictionary = new(trainingFilePath.Length + testingFilePath.Length);
        
        PerceptronUtility.DoStringEncodings(trainingReader, topicEncodingDictionary, testingReader);
        
        Console.WriteLine($"\n\nTiming Metrics:\n\nTopics were encoded at {stopwatch.Elapsed}.");

        ////////////////////
        //Create Perceptrons
        ////////////////////
        
        HashSet<string> topics = new();
        foreach (var document in trainingReader.Documents)
        {
            topics.Add(document.Topic);
        }
        
        SingleLayerPerceptron[] perceptrons =
            new SingleLayerPerceptron[topics.Count];

        int perceptronCt = 0;
        foreach (var topic in topics)
        {
            var perceptron = new SingleLayerPerceptron(
                trainingReader.NumberOfAttributes,
                learningRate: 0.1,
                topic,
                topicEncodingDictionary[topic]);
            perceptrons[perceptronCt] = perceptron;
            
            perceptronCt++;
        }
        Console.WriteLine($"Perceptrons were created at {stopwatch.Elapsed}.");
        
        ///////////////////
        //Train Perceptrons
        ///////////////////
        
        PerceptronUtility.TrainPerceptrons(
            trainingReader: trainingReader,
            perceptrons: perceptrons);
        Console.WriteLine($"Perceptrons were trained at {stopwatch.Elapsed}.");
        
        //////////////////
        //Test Perceptrons
        //////////////////

        PerceptronUtility.TestPerceptrons(
            testingReader: testingReader,
            perceptrons: perceptrons);
        Console.WriteLine($"Perceptrons were tested at {stopwatch.Elapsed}.");
        
        //Stop stopwatch
        stopwatch.Stop();
        Console.WriteLine($"Stopwatch stopped at: {stopwatch.Elapsed}\n");
        
        //////////////////
        //Print Metrics
        //////////////////

        for (int i = 0; i < perceptrons.Length; i++)
        {
            perceptrons[i].PrintConfusionMatrix();
        }
        
        PerceptronUtility.PrintMetrics2(perceptrons);

    }
}