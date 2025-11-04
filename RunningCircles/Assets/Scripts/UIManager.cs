using UnityEngine;


public class UIManager : MonoBehaviour
{
    [SerializeField] UIScreen _initialScreen;

    UIScreen _currentScreen;
    UIScreen _previousScreen;

    private void Awake()
    {
        UIScreen[] screens = GetComponentsInChildren<UIScreen>(true);
        foreach (var screen in screens)
        {
            screen.Initialize(this);
        }
    }

    private void Start()
    {
        if (_initialScreen != null)
        {
            _initialScreen.Open();
            _currentScreen = _initialScreen;
        }
    }
    public void SwitchScreen(UIScreen newScreen)
    {
        if (_currentScreen != null)
        {
            _currentScreen.Close();
            _previousScreen = _currentScreen;
        }
        if (newScreen != null)
        {
            newScreen.Open();
            _currentScreen = newScreen;
        }
    }
    public void GoBack()
    {
        if (_previousScreen != null)
        {
            if (_currentScreen != null)
            {
                _currentScreen.Close();
            }
            _previousScreen.Open();
            _currentScreen = _previousScreen;
            _previousScreen = null;
        }
    }
}

