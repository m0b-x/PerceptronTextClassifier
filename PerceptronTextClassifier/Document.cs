using System.Collections;
using System.Text;

namespace PerceptronTextClassifier
{
    public class Document
    {
        private int _id;
        private string _topic;
        private BitArray _topicEncoding;
        private StringBuilder _topicEncodingString;
        private int _topicEncodingInt;
        private List<IndexFrequencyPair> _indexFrequencyPairs;
        private bool []_attributePresence;

        public Document(int id, string topic, int numAttributes, List<IndexFrequencyPair> indexFrequencyPairs)
        {
            _id = id;
            _topic = topic;
            _indexFrequencyPairs = indexFrequencyPairs;
            
            //Add normalization data layer
            _attributePresence = new bool[numAttributes];
            foreach (var pair in _indexFrequencyPairs)
            {
                _attributePresence[pair.Index] = true;
            }
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


        public List<IndexFrequencyPair> IndexFrequencyPairs
        {
            get { return _indexFrequencyPairs; }
            set { _indexFrequencyPairs = value; }
        }

        public bool[] AttributePresenceArray
        {
            get { return _attributePresence; }
        }
    }
}