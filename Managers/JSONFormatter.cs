using Newtonsoft.Json;

namespace ProxyServerDotNet.Managers
{
    public class JSONFormatter
    {
        public void GenerateFirebaseConfigJsonFile()
        {
            string filePath = @".\firebase-config.json";

            //setting up JSON File
            var fireBaseConfig = new FireBaseConfig
            {
                type = Environment.GetEnvironmentVariable("type"),
                project_id = Environment.GetEnvironmentVariable("project_id"),
                private_key_id = Environment.GetEnvironmentVariable("private_key_id"),
                private_key = Environment.GetEnvironmentVariable("private_key"),
                client_email = Environment.GetEnvironmentVariable("client_email"),
                client_id = Environment.GetEnvironmentVariable("client_id"),
                auth_uri = Environment.GetEnvironmentVariable("auth_uri"),
                token_uri = Environment.GetEnvironmentVariable("token_uri"),
                auth_provider_x509_cert_url = Environment.GetEnvironmentVariable("auth_provider_x509_cert_url"),
                client_x509_cert_url = Environment.GetEnvironmentVariable("client_x509_cert_url")
            };

            var fireBaseConfigJson = JsonConvert.SerializeObject(fireBaseConfig, Formatting.Indented);
            File.WriteAllText(filePath, fireBaseConfigJson);
        }
    }


    public class FireBaseConfig
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
    }

}
