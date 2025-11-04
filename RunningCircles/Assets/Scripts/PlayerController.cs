using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _marginPercent = 0.01f;


    private Rigidbody2D _rb;
    private Camera _camera;
    private Vector2 _moveInput;
    private float _minX, _maxX, _minY, _maxY;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _camera = Camera.main;
        CalculateCameraBounds();
    }

    public void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    public void OnCancel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        Vector2 dir = _moveInput.normalized;
        Vector2 newPos = _rb.position + dir * _moveSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, _minX, _maxX);
        newPos.y = Mathf.Clamp(newPos.y, _minY, _maxY);

        _rb.MovePosition(newPos);
    }

    private void CalculateCameraBounds()
    {
        Vector3 bl = _camera.ViewportToWorldPoint(new Vector3(_marginPercent, _marginPercent, 0));
        Vector3 tr = _camera.ViewportToWorldPoint(new Vector3(1f - _marginPercent, 1f - _marginPercent, 0));
        _minX = bl.x;
        _minY = bl.y;
        _maxX = tr.x;
        _maxY = tr.y;
    }
}
