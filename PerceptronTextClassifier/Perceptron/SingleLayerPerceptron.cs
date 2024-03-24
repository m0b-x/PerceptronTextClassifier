using System.Collections;
using System.Text;

namespace PerceptronTextClassifier;

public class SingleLayerPerceptron
{
    //dynamic input size
    private int _maxEpochs;
    
    private double[] _weights;
    
    private double _bias;
    
    private double _learningRate;
    
    private double _treshold = 0.0;
    
    private string _topic;
    private int _topicEncoding;
    
    public double Treshold
    {
        get { return _treshold; }
        set { _treshold = value; }
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
        InitializeWeights();
        _bias = 0;
        _topic = topicToLearn;
        _topicEncoding = topicEncoding;
        _maxEpochs = GlobalSettings.MaxPerceptronEpochs;
    }
    //Initialize Weights Between -1/inputSize and 1/inputSize
    public void InitializeWeights()
    {
        Random rand = new Random();
        double range = 1.0 / _weights.Length;

        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = rand.NextDouble() * (2 * range) - range;
        }
    }

    //expectedOutcome should be either -1 or 1
    public void TrainWithDocument(Document document, int expectedOutcome)
    {
        bool outcomesMatch = false;
        int curentEpoch = 0;
        //Stopping criterion =  outcomes Match
        while (!outcomesMatch && curentEpoch < _maxEpochs)
        {
            double sum = _bias;

            sum = ApplySummingFunction(document, sum);

            int activationResult =
                ActivationFunctionsImpl.SignFunction(sum);

            //Update the weights if the presented output is not what we want
            if (activationResult != expectedOutcome)
            {
                double differenceBetweenOutcomes = _learningRate* (expectedOutcome - activationResult);
                for (int i = 0; i < _weights.Length; ++i)
                {
                    //Original formula
                    //_weights[i] = _weights[i] - _learningRate * (expectedOutcome - activationResult) * document.NormalisedAttributePresence[i];
                    
                    _weights[i] = _weights[i] - differenceBetweenOutcomes * document.NormalisedAttributePresence[i];
                }
            }
            else
            {
                outcomesMatch = true;
                break;
            }

            curentEpoch++;
        }
    }
    public void TestWithDocument(Document document)
    {
        double sum = _bias;

        sum = ApplySummingFunction(document, sum);

        double activationResult =
            ActivationFunctionsImpl.SignFunction(sum);
        
        bool isTopicMatch = TopicEncoding == document.TopicEncoding;

        if (activationResult > 0)
        {
            //Correctly predicts that the documentis part of class
            if (isTopicMatch)
            {
                GlobalEvaluator.TruePositive++;
            }
            //Incorrectly predicts that the documentis part of class (it is not)
            else
            {
                GlobalEvaluator.FalsePositive++;
            }
        }
        else
        {
            // Incorrectly predicts that the document is part of class (it is)
            if (isTopicMatch)
            {
                GlobalEvaluator.FalseNegative++;
            }
            // Correctly predicts that the document is not part of class
            else
            {
                GlobalEvaluator.TrueNegative++;
            }
        }
    }

    private double ApplySummingFunction(Document document, double sum)
    {
        for (int i = 0; i < document.NormalisedAttributePresence.Length; ++i)
        {
            sum += _weights[i] * document.NormalisedAttributePresence[i];
        }
        return sum;
    }
}