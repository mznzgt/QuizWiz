namespace QuizWiz.Web.Services
{
    public class ErrorHandlerService
    {
        public event Action<string> OnError;

        public void TriggerError(string message)
        {
            OnError?.Invoke(message);
        }

        public void ClearError()
        {
            OnError?.Invoke(null);
        }
    }
}
