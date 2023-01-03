using System.Runtime.Serialization;

namespace ProxyServerDotNet.Models.Inputs
{
    [DataContract]
    public class AuthInput
    {
        [DataMember]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
