namespace PerceptronTextClassifier;

public class SingleLayerPerceptron
{
    private int _inputSize;
    private double[] _weights;
    private double _bias;
    private double _learningRate;
    

    public SingleLayerPerceptron(int inputSize, double learningRate)
    {
        this._inputSize = inputSize;
        this._learningRate = learningRate;

        _weights = new double[inputSize];
        InitializeWeights(inputSize);
        _bias = 0;
    }

    private void InitializeWeights(int inputSize)
    {
        Random rand = new Random();
        double range = 1.0 / inputSize;

        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = rand.NextDouble() * (2 * range) - range;
        }
    }

    private int Activate(double sum)
    {
        return sum >= 0 ? 1 : 0; 
    }

    public int Predict(double[] inputs)
    {
        if (inputs.Length != _inputSize)
        {
            throw new ArgumentException("Input size mismatch");
        }

        double sum = _bias;
        for (int i = 0; i < _inputSize; i++)
        {
            sum += inputs[i] * _weights[i];
        }

        return Activate(sum);
    }

    public void Train(double[][] inputSet, int[] labels, int epochs)
    {
        if (inputSet.Length != labels.Length)
        {
            throw new ArgumentException("Input size mismatch");
        }

        for (int epoch = 0; epoch < epochs; epoch++)
        {
            for (int i = 0; i < inputSet.Length; i++)
            {
                int prediction = Predict(inputSet[i]);
                int error = labels[i] - prediction;

                // Update weights and bias
                for (int j = 0; j < _inputSize; j++)
                {
                    _weights[j] += _learningRate * error * inputSet[i][j];
                }
                _bias += _learningRate * error;
            }
        }
    }
}