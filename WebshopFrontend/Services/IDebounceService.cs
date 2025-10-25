namespace WebshopFrontend.Services;

public interface IDebounceService
{
    void Debounce(int delayMilliseconds, Action action, object key);
    
    void Cancel(object key);
}