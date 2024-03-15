namespace PerceptronTextClassifier
{
    public class IndexFrequencyPair
    {
        private int _index;
        private int _frequency;

        public IndexFrequencyPair(int index, int frequency)
        {
            Index = index;
            Frequency = frequency;
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public int Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }
    }
}