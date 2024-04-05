namespace PerceptronTextClassifier;

public class SingleLayerPerceptron
{
    private Random RandomGenerator;
    
    private int _maxIterations;
    
    private double[] _weights;
    
    private double _bias;
    
    private double _learningRate;
    
    private string _topic;
    private int _topicEncoding;

    private Evaluator _evaluator;

    public int TP
    {
        get { return _evaluator.TruePositive; }
        set { _evaluator.TruePositive = value; }
    }
    public int TN
    {
        get { return _evaluator.TrueNegative; }
        set { _evaluator.TrueNegative = value; }
    }
    public int FP
    {
        get { return _evaluator.FalsePositive; }
        set { _evaluator.FalsePositive = value; }
    }
    public int FN
    {
        get { return _evaluator.FalseNegative; }
        set { _evaluator.FalseNegative = value; }
    }

    public Evaluator Evaluator
    {
        get { return _evaluator; }
    }
    
    public string Topic
    {
        get { return _topic; }
        set { _topic = value; }
    }
    public int TopicEncoding
    {
        get { return _topicEncoding; }
        set { _topicEncoding = value; }
    }


    public SingleLayerPerceptron(int inputSize, double learningRate, string topicToLearn, int topicEncoding)
    {
        _learningRate = learningRate;
        _weights = new double[inputSize];
        _bias = 0;
        _topic = topicToLearn;
        _topicEncoding = topicEncoding;
        _maxIterations = GlobalSettings.MaxPerceptronIterations;
        _evaluator = new Evaluator();
        RandomGenerator = new Random();
        InitializeWeights();
    }
    //Initialize Weights Between -1/inputSize and 1/inputSize
    public void InitializeWeights()
    {
        double range = 1.0 / _weights.Length;

        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = RandomGenerator.NextDouble() * (2 * range) - range;
        }
    }

    //expectedOutcome should be either -1 or 1
    public void TrainWithDocument(Document document, int expectedOutcome)
    {
        int curentIteration = 0;
        //Stopping criterion =  outcomes Match
        while (curentIteration < _maxIterations)
        {
            double sum = _bias;

            sum = ApplySummingFunction(document, sum);

            int activationResult =
                ActivationFunctionsImpl.SignFunction(sum);
            
            //Update the weights if the presented output is not what we want
            if (activationResult != expectedOutcome)
            {
                double differenceBetweenOutcomes = _learningRate * (expectedOutcome - activationResult);
                
                for (int i = 0; i < _weights.Length; ++i)
                {
                    //Original formula
                    //_weights[i] = _weights[i] + _learningRate * (expectedOutcome - activationResult) * document.NormalisedAttributePresence[i];
                    
                    _weights[i] += differenceBetweenOutcomes * document.NormalisedAttributePresence[i];
                }
            }
            else
            {
                break;
            }
            curentIteration++;
        }
    }
    public double TestWithDocument(Document document)
    {
        double sum = _bias;

        sum = ApplySummingFunction(document, sum);

        double activationResult =
            ActivationFunctionsImpl.SignFunction(sum);
        
        bool isTopicMatch = TopicEncoding == document.TopicEncoding;
        if (activationResult >= 0)
        {
            //Correctly predicts that the documentis part of class
            if (isTopicMatch)
            {
                _evaluator.TruePositive++;
            }
            //Incorrectly predicts that the documentis part of class (it is not)
            else
            {
                _evaluator.FalsePositive++;
            }
        }
        else
        {
            // Incorrectly predicts that the document is part of class (it is)
            if (isTopicMatch)
            {
                _evaluator.FalseNegative++;
            }
            // Correctly predicts that the document is not part of class
            else
            {
                _evaluator.TrueNegative++;
            }
        }

        return sum;
    }

    private double ApplySummingFunction(Document document, double sum)
    {
        for (int i = 0; i < document.NormalisedAttributePresence.Length; ++i)
        {
            sum += _weights[i] * document.NormalisedAttributePresence[i];
        }
        return sum;
    }

    public void PrintConfusionMatrix()
    {
        Console.WriteLine(
            $"Confusion Matrix for perceptron {Topic}\nTP:{TP}, FN:{FN},\nFP:{FP}, TN:{TN}\n");
    }
}