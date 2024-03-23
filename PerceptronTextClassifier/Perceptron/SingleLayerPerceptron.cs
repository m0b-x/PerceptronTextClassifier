using System.Collections;
using System.Text;

namespace PerceptronTextClassifier;

public class SingleLayerPerceptron
{
    //dynamic input size
    public static int MaxEpochs = 100;
    
    private double[] _weights;
    
    private double _bias;
    
    private double _learningRate;
    
    private double _treshold = 0.0;
    
    private string _topic;
    
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

    public SingleLayerPerceptron(int inputSize, double learningRate, string topicToLearn)
    {
        _learningRate = learningRate;
        _weights = new double[inputSize];
        InitializeWeights();
        _bias = 0;
        _topic = topicToLearn;
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
    public void TrainWithDocument(Document document, int expectedOutcome, NormalizationTypes normalization)
    {
        if (_weights == null)
        {
            throw new ArgumentNullException(nameof(_weights), "Weight array is not initialized.");
        }
        
        bool outcomesMatch = false;
        int curentEpoch = 0;
        
        int[] presenceArray =
            PerceptronUtility.ReturnAttributePresenceArrayInt(document.AttributePresenceArray, normalization);

        //Stopping criterion =  outcomes Match
        while (!outcomesMatch && curentEpoch < MaxEpochs)
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
                    _weights[i] = _weights[i] - _learningRate * (expectedOutcome - activationResult) * presenceArray[i];
                }
            }
            else
            {
                outcomesMatch = true;
            }

            curentEpoch++;
        }
    }
    public bool TestWithDocument(Document document, NormalizationTypes normalization)
    {
        if (_weights == null)
        {
            throw new ArgumentNullException(nameof(_weights), "Weight array is not initialized.");
        }
        
        double sum = 0.0;

        sum = NormalizeAndSum(document, sum, normalization);

        double activationResult =
            PerceptronUtility.ApplyActivationFunction(
                sum: sum,
                activationType: ActivationFunctions.SignFunction,
                threshold: 0.0
                );
        
        Console.WriteLine($"Sum: {sum}, activationResult:{activationResult}");
        
        // Correctly predicts that the document is part of class
        if (activationResult > 0 && _topic.Equals(document.Topic))
        {
            // Increment TP
            // Increment TN for all other classes
            return true;
        }

        // Correctly predicts that the document is not part of class
        if (activationResult <= 0 && !_topic.Equals(document.Topic))
        {
            // Increment TN
            return true;
        }

        // Incorrectly predicts that the document is part of class (it is not)
        if (activationResult > 0 && !_topic.Equals(document.Topic))
        {
            // Increment FP
            return true;
        }

        // Incorrectly predicts that the document is not part of class (it is)
        if (activationResult <= 0 && _topic.Equals(document.Topic))
        {
            // Increment FN
            return true;
        }



        return false;
    }

    private double NormalizeAndSum(Document document, double sum, NormalizationTypes normalization)
    {
        if (normalization.Equals(NormalizationTypes.With0And1))
        {
            foreach (var pair in document.IndexFrequencyPairs)
            {
                var index = pair.Index;
                var frequency = pair.Frequency;
                
                sum += _weights[index];
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