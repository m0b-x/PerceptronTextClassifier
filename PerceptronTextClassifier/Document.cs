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
        
        private List<IndexFrequencyPair> _indexFrequencyPairs;

        public Document(int id, string topic, List<IndexFrequencyPair> indexFrequencyPairs)
        {
            _id = id;
            _topic = topic;
            _indexFrequencyPairs = indexFrequencyPairs;
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
                _topicEncodingString = BitArrayToStringBuilder(_topicEncoding);
            }
        }
        
        public StringBuilder TopicEncodingString
        {
            get { return _topicEncodingString; }
        }

        public List<IndexFrequencyPair> IndexFrequencyPairs
        {
            get { return _indexFrequencyPairs; }
            set { _indexFrequencyPairs = value; }
        }
        
        
        public static StringBuilder BitArrayToStringBuilder(BitArray bitArray)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            StringBuilder stringBuilder = new StringBuilder(bitArray.Length);
            for (int i = 0; i < bitArray.Length; i++)
            {
                stringBuilder.Append(bitArray[i] ? '1' : '0');
            }
            return stringBuilder;
        }
    }
}