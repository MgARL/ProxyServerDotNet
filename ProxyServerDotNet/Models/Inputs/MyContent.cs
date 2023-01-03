using System.Runtime.Serialization;

namespace ProxyServerDotNet.Models.Inputs
{
    [DataContract]
    public class MyContent
    {
        [DataMember]
        public string Result { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
