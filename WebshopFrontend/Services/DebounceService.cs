namespace WebshopFrontend.Services;

public class DebounceService : IDebounceService
{
    private readonly Dictionary<object, CancellationTokenSource> _cancellationTokens = new();

    public void Debounce(int delayMilliseconds, Action action, object key)
    {
        Cancel(key); 
        
        var cts = new CancellationTokenSource();
        _cancellationTokens[key] = cts;

        Task.Delay(delayMilliseconds, cts.Token)
            .ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    action(); 
                }
                
                lock (_cancellationTokens)
                {
                    _cancellationTokens.Remove(key);
                }
            }, cts.Token);
    }

    public void Cancel(object key)
    {
        lock (_cancellationTokens)
        {
            if (!_cancellationTokens.TryGetValue(key, out var cts))
            {
                return;
            }
            
            cts.Cancel();
            
            _cancellationTokens.Remove(key);
        }
    }
}