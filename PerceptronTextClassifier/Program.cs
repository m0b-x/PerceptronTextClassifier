namespace PerceptronTextClassifier;

class Program
{
    static void Main(string[] args)
    {
        string workingDirectory = Environment.CurrentDirectory;
        string slnDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;

        var trainingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Training_SVM_100.0.arff");
        var testingFilePath = Path.Combine(slnDirectory, "Data/MultiClass_Testing_SVM_100.0.arff");
        
        ArffFileReader trainingReader =  new ArffFileReader(trainingFilePath);
        
        var topicEncoding = PerceptronUtility.ComputeTopicEncoding(trainingReader.TopicList);
        
        foreach (var document in trainingReader.Documents)
        {
            document.TopicEncoding = topicEncoding[document.Topic];
        }
        
        /////////////////
        //Print Documents
        /////////////////
        List<SingleLayerPerceptron> neurons = new List<SingleLayerPerceptron>();
        
        
    }
}