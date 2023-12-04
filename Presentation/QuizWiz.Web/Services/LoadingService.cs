namespace QuizWiz.Web.Services;

public class LoadingService
{
    public bool IsLoading { get; private set; }

    public event Action OnLoadingChanged;

    public void Show()
    {
        IsLoading = true;
        OnLoadingChanged?.Invoke();
    }

    public void Hide()
    {
        IsLoading = false;
        OnLoadingChanged?.Invoke();
    }
}
