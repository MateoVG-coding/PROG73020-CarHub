namespace WebAPI.Messages
{
    public class SessionManager
    {
        private string _sessionId = string.Empty; 

        public string getSessionId() { return _sessionId; }

        public void setSessionId(string sessionId) { _sessionId = sessionId; }

        public void deleteSessionId() { _sessionId = null ; }
    }
}
