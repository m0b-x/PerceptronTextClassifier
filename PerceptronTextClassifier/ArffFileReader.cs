using System.Globalization;

namespace PerceptronTextClassifier;

public class ArffFileReader
{
    private List<string> _topicList;
    private List<Document> _documents;

    private int _numSamples;
    private int _numAttributes;
    private int _numtopics;

    public List<string> TopicList
    {
        get { return _topicList; }
    }
    
    public int NumberOfSamples
    {
        get { return _numSamples; }
    }
    
    public int NumberOfAttributes
    {
        get { return _numAttributes; }
    }
    
    public int NumberOfTopics
    {
        get { return _numtopics; }
    }

    public List<Document> Documents
    {
        get { return _documents; }
    }
    
    public ArffFileReader(string filename)
    {
        _topicList = new List<string>();
        _documents = new List<Document>();
    
        bool isDataSection = false;
        _numSamples = 0;
        _numAttributes = 0;
        _numtopics = 0;

        using (StreamReader reader = new StreamReader(filename))
        {
            int documentCounter = 0;

            while (reader.ReadLine() is { } line)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                
                if (line.StartsWith('#'))
                {
                    ReadHeader(line);
                }
                else if(line.StartsWith("@a"))//@attribute
                {
                    var lineData = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int attributeName = Int32.Parse(lineData[1]);
                    //file mistakenly lists index as double
                    double attributeIndexDouble = Double.Parse(lineData[2], CultureInfo.InvariantCulture);
                    //subtract 1 to start index at 0
                    int attributeIndex = Convert.ToInt32(attributeIndexDouble) - 1;
                }
                else if(line.StartsWith("@t"))//@topic
                {
                    var lineData = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string topicName = lineData[1];
                    //int topicFrequency = Int32.Parse(lineData[2]);
                    _topicList.Add(topicName);
                }
                else if(line.StartsWith("@d"))//@data
                {
                    isDataSection = true;
                }
                else if (isDataSection)
                {
                    //data
                    var lineData = line.Split('#', StringSplitOptions.RemoveEmptyEntries);
                    var topicNames = lineData[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var unitPair = lineData[0];
                    //Split pairs
                    var pairArrayBeforeTopics = unitPair.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    
                    //For simplicity, we will only keep in mind the first topic
                    string firstTopic = topicNames[0];

                    List<IndexFrequencyPair> termFrequencyPairList = new();
                    foreach (var pairInLine in pairArrayBeforeTopics)
                    {
                        var pairInLineDouble = pairInLine.Split(':');
                        var term = Int32.Parse(pairInLineDouble[0]);
                        var frequency = Int32.Parse(pairInLineDouble[1]);
                        termFrequencyPairList.Add(new IndexFrequencyPair(term, frequency));
                    }
                    _documents.Add(new Document(documentCounter, firstTopic, _numAttributes, termFrequencyPairList));
                    documentCounter++;
                }
            }
        }
        Console.WriteLine($"[Info]: Succesfully read from file {filename}");
    }

    private void ReadHeader(string line)
    {
        // Parse information lines
        string[] parts = line.Split(' ');
        if (parts.Length > 1)
        {
            int value = int.Parse(parts[1]);
            switch (parts[0])
            {
                case "#Samples":
                    _numSamples = value;
                    break;
                case "#Attributes":
                    _numAttributes = value;
                    break;
                case "#Topics":
                    _numtopics = value;
                    break;
            }
        }
    }
}