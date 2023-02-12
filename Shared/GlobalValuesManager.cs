public class GlobalValuesManager
{

    public Action OnThemeChanged;
    public GlobalValuesManager()
    {

    }

    private bool _isDarkMode = true;
    public bool IsDarkMode{ 
        get
        {
            return _isDarkMode;
        }
        set
        {
            _isDarkMode = value;
            OnThemeChanged?.Invoke();
        }
    }

}