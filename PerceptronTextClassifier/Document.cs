using System.Collections;
using System.Text;

namespace PerceptronTextClassifier
{
    public class Document
    {
        private int _id;
        
        private string _topic;
        private int _topicEncoding;
        
        private List<IndexFrequencyPair> _indexFrequencyPairs;
        private int[] _normalisedAttributePresence;

        public Document(int id, string topic, int numAttributes, List<IndexFrequencyPair> indexFrequencyPairs)
        {
            _id = id;
            _topic = topic;
            _indexFrequencyPairs = indexFrequencyPairs;
            
            //Add normalization data layer
            InitialiseNormalisedPresenceArray(numAttributes);
        }

        private void InitialiseNormalisedPresenceArray(int numAttributes)
        {
            _normalisedAttributePresence = new int[numAttributes];
            
            if (GlobalSettings.NormalizationType.Equals(NormalizationTypes.With1AndNeg1))
            {
                for (int i = 0; i < numAttributes; ++i)
                {
                    _normalisedAttributePresence[i] = -1;
                }
            }
            // no need to to something for 1 and 0 normalisation
            // since int array values are initialised to 0 by default 
            
            foreach (var pair in _indexFrequencyPairs)
            {
                _normalisedAttributePresence[pair.Index] = 1;
            }
        }

        public int[] NormalisedAttributePresence
        {
            get { return _normalisedAttributePresence; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
        public List<IndexFrequencyPair> IndexFrequencyPairs
        {
            get { return _indexFrequencyPairs; }
            set { _indexFrequencyPairs = value; }
        }

    }
}