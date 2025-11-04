using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIScreen : MonoBehaviour
{
    [SerializeField] Selectable _firstSelected;
    Canvas _canvas;
    CanvasGroup _canvasGroup;
    UIManager _uiManager;
    InputAction _cancelAction;
    bool _isOpen = false;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
        _cancelAction = InputSystem.actions.FindAction("Cancel");
    }

    public void Open()
    {
        if (_isOpen) 
            return;
        if (_canvas != null)
            _canvas.enabled = true;
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        if (_firstSelected != null)
        {
            _firstSelected.Select();
        }
        _cancelAction.performed += ctx => GoBack();
        _isOpen = true;
    }

    public void Close()
    {
        if (_canvas != null)
            _canvas.enabled = false;
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        _cancelAction.performed -= ctx => GoBack();
        _isOpen = false;
    }

    public void SwitchToScreen(UIScreen newScreen)
    {
        if (_uiManager != null)
        {
            _uiManager.SwitchScreen(newScreen);
        }
    }

    public void GoBack()
    {
        if (_uiManager != null && _isOpen)
        {
            _uiManager.GoBack();
        }
    }
}
