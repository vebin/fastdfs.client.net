
namespace fastdfs.client.net
{
    /// <summary>
    /// author zhouyh
    /// version 1.0
    /// </summary>
    public class NameValuePair
    {
        protected string name;
        protected string value;

        public NameValuePair()
        {
        }

        public NameValuePair(string name)
        {
            this.name = name;
        }

        public NameValuePair(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string _Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string _Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
