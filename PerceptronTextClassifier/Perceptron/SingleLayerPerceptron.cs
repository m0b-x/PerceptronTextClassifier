using System.Collections;
using System.Text;

namespace PerceptronTextClassifier;

public class SingleLayerPerceptron
{
    //dynamic input size
    
    private double[] _weights;
    
    private double _bias;
    
    private double _learningRate;
    
    private double _treshold = 0.0;
    
    private string _topic;
    private BitArray _topicEncoding;
    private StringBuilder _topicEncodingString;
    private int _topicEncodingInt;
    
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
        
    public BitArray TopicEncoding
    {
        get { return _topicEncoding; }
        set { _topicEncoding = value;
            _topicEncodingString = PerceptronUtility.BitArrayToStringBuilder(_topicEncoding);
            _topicEncodingInt = PerceptronUtility.BitArrayToInteger(_topicEncoding);
        }
    }
        
    public StringBuilder TopicEncodingString
    {
        get { return _topicEncodingString; }
    }
    
    public int TopicEncodingInt
    {
        get { return _topicEncodingInt; }
    }

    public SingleLayerPerceptron(int inputSize, double learningRate)
    {
        this._learningRate = learningRate;

        _weights = new double[inputSize];
        _bias = 0;
    }
    //Initialize Weights Between -1/inputSize and 1/inputSize
    private void InitializeWeights()
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
        if (_weights == null)
        {
            throw new ArgumentNullException(nameof(_weights), "Weight array is not initialized.");
        }
        
        bool outcomesMatch = false;
        
        NormalizationTypes normalization = NormalizationTypes.With0And1;
        
        int[] presenceArray =
            PerceptronUtility.ReturnAttributePresenceArrayInt(document.AttributePresenceArray, normalization);

        //Stopping criterion =  outcomes Match
        while (!outcomesMatch)
        {
            double sum = 0.0;

            sum = NormalizeAndSum(document, sum, normalization);

            double activationResult =
                PerceptronUtility.ApplyActivationFunction(
                    sum: sum,
                    activationType: ActivationFunctions.SignFunction,
                    threshold: 0.0
                );

            //Update the weights if the presented output is not what we want
            if (!activationResult.Equals(expectedOutcome))
            {
                for (int i = 0; i < _weights.Length; ++i)
                {
                    _weights[i] = _weights[i] - _learningRate * (expectedOutcome - sum) * presenceArray[i];
                }
            }
            else
            {
                outcomesMatch = true;
            }
        }
    }

    private double NormalizeAndSum(Document document, double sum, NormalizationTypes normalization)
    {
        if (normalization.Equals(NormalizationTypes.With0And1))
        {
            foreach (var pair in document.IndexFrequencyPairs)
            {
                var index = pair.Index;
                var frequency = pair.Frequency;
                
                sum += _weights[index]; // * frequency;
            }
        }
        else if (normalization.Equals(NormalizationTypes.With1AndNeg1))
        {
            HashSet<int> indexes = new HashSet<int>(document.IndexFrequencyPairs.Count);
            foreach (var pair in document.IndexFrequencyPairs)
            {
                var index = pair.Index;
                var frequency = pair.Frequency;
                indexes.Add(index);
                sum += _weights[index];
            }

            List<int> missingIndexes = new List<int>(_weights.Length);
            for (var presenceIndex = 0; presenceIndex < _weights.Length; ++presenceIndex)
            {
                if (!indexes.Contains(presenceIndex))
                {
                    missingIndexes.Add(presenceIndex);
                }
            }

            foreach (var missingIndex in missingIndexes)
            {
                sum -= _weights[missingIndex];
            }
        }
        return sum;
    }
}