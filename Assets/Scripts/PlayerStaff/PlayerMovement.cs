using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace PlayerStaff
{ 
    
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private SpeedType _playerDefaultSpeed;
    [Header("Blink")]
    [SerializeField, Range(0.1f, 10f)] private float _blinkDistance = 5f;
    [SerializeField] private float _blinkCooldown = 3f;
    [SerializeField] private Image _blinkRefreshBar;
    [SerializeField, Range(0.1f,1f)] private float _raycastOffset = 0.4f;
    [SerializeField, Range(0.1f,1f)] private float _distanceFactor = 0.8f;
    private Rigidbody2D _rb;
    //private PlayerInputActions _inputActions;
    private Vector2 _movementInput;
    private bool _isBlinkOnCooldown = false;
    private bool _isMovementEnabled = true;
    private float _currentSpeed;
    private Animator anim;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_inputActions = new PlayerInputActions();
        
        //_inputActions.Player.Blink.started += OnBlink;

        _currentSpeed = SpeedTypeData.GetDataByType(_playerDefaultSpeed);
        //Нужно выкинуть в отделный VIEW класс
        anim = GetComponent<Animator>();
    }

    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    private void Update()
    {
        if (_isMovementEnabled)
        {
            _movementInput = _inputActions.Player.Movement.ReadValue<Vector2>();
            anim.SetFloat("MoveX", _movementInput.x);
            anim.SetFloat("MoveY",  _movementInput.y);
        }
        else
        {
            _movementInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movementInput * (_currentSpeed * Time.fixedDeltaTime));
    }

    public void SetSpeed(float speed) => _currentSpeed = speed;

    private void OnBlink(InputAction.CallbackContext context)
    {
        if (_movementInput.magnitude > 0 && !_isBlinkOnCooldown)
        {
            PerformBlink();
        }
    }

    private void PerformBlink()
    {
        LayerMask wallMask = LayerMask.GetMask("Walls");
        
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y - _raycastOffset), 
            _movementInput, 
            _blinkDistance, 
            wallMask
        );

        _rb.position += hit.collider != null ? 
              _movementInput * hit.distance * _distanceFactor 
            : _movementInput * _blinkDistance;

        StartCoroutine(BlinkCooldown());
    }

    private IEnumerator BlinkCooldown()
    {
        _isBlinkOnCooldown = true;
        
        _blinkRefreshBar.fillAmount = 0;
        float timer = 0;
        
        while (timer < _blinkCooldown)
        {
            timer += Time.deltaTime;
            _blinkRefreshBar.fillAmount = timer / _blinkCooldown;
            yield return null;
        }
        
        _isBlinkOnCooldown = false;
    }

    public void EnableMovement() => _isMovementEnabled = true;
    public void DisableMovement() => _isMovementEnabled = false;
}
}