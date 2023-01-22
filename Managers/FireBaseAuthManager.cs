

using Firebase.Auth;

namespace ProxyServerDotNet.Managers
{
    public class FireBaseAuthManager
    {
        static public FirebaseAuthProvider initialConfig()
        {
            var config = new FirebaseConfig(Environment.GetEnvironmentVariable("AuthSecret"));
            var authProvider = new FirebaseAuthProvider(config);

            return authProvider;
        }
    }
}
