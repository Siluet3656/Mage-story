using System.Collections;
using UnityEngine;
using UI;

namespace PlayerStaff
{ 
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerUI))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private SpeedType _playerDefaultSpeed;

    [Header("Blink")] 
    [SerializeField] private LayerMask _blinkRaycastMask;
    [SerializeField, Range(0.1f, 10f)] private float _blinkDistance = 5f;
    [SerializeField, Range(0.1f,1f)] private float _raycastOffset = 0.4f;
    [SerializeField, Range(0.1f,1f)] private float _distanceFactor = 0.8f;
    [SerializeField] private float _blinkCooldown = 3f;

    private PlayerUI _ui;
    private Rigidbody2D _rb;
    private PlayerInputHandler _playerInputHandler;
    private Vector2 _movementInput;
    private bool _isBlinkOnCooldown;
    private bool _isMovementEnabled;
    private float _currentSpeed;
    private void Awake()
    {
        _ui = GetComponent<PlayerUI>();
        _rb = GetComponent<Rigidbody2D>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        
        _currentSpeed = SpeedTypeData.GetDataByType(_playerDefaultSpeed);

        _playerInputHandler.BlinkButtonPressed += OnBlink;
        
        _isBlinkOnCooldown = false;
        _isMovementEnabled = true;
    }

    private void FixedUpdate()
    {
        if (_isMovementEnabled)
        {
            _rb.MovePosition(_rb.position + _movementInput * (_currentSpeed * Time.fixedDeltaTime));
        }
    }

    public void SetSpeed(float speed) => _currentSpeed = speed;

    public void SetMovementInput(Vector2 value) => _movementInput = value;
    public Vector2 GetMovementInput() => _movementInput;

    private void OnBlink()
    {
        if (_movementInput.magnitude > 0 && !_isBlinkOnCooldown)
        {
            PerformBlink();
        }
    }

    private void PerformBlink()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y - _raycastOffset), 
            _movementInput, 
            _blinkDistance, 
            _blinkRaycastMask
        );

        _rb.position += hit.collider != null ? 
              _movementInput * hit.distance * _distanceFactor 
            : _movementInput * _blinkDistance;

        StartCoroutine(BlinkCooldown());
    }

    private IEnumerator BlinkCooldown()
    {
        _isBlinkOnCooldown = true;
        
        _ui.SetBlinkRefreshBarFillAmount(0);
        float timer = 0;
        
        while (timer < _blinkCooldown)
        {
            timer += Time.deltaTime;
            _ui.SetBlinkRefreshBarFillAmount(timer / _blinkCooldown);
            yield return null;
        }
        
        _isBlinkOnCooldown = false;
    }

    public void EnableMovement() => _isMovementEnabled = true;
    public void DisableMovement() => _isMovementEnabled = false;
}
}