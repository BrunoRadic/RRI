using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;

    [Header("Jump & Gravity")]
    public float jumpForce = 6f;
    public float gravity = -20f;

    [Header("Crouch")]
    public float standingHeight = 2.0f;
    public float crouchHeight = 1.0f;
    public float crouchLerpSpeed = 12f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;
    public float staminaRunMin = 10f;

    [Header("Mouse")]
    public float sensitivityX = 2f;

    public PlayerState CurrentState { get; private set; }
    public float CurrentStamina { get; private set; }
    public float CurrentEyeHeight { get; private set; }
    public float MaxStamina => maxStamina;
    public bool IsCrouching => _isCrouching;

    CharacterController _cc;
    float _yVelocity = 0f;

    bool _isCrouching = false;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        CurrentStamina = maxStamina;

        _cc.height = standingHeight;
        _cc.center = Vector3.zero;

        CurrentEyeHeight = standingHeight * 0.85f;
    }

    void Update()
    {
        DoYaw();
        DoCrouch();
        DoMovement();
        DoState();
    }

    void DoYaw()
    {
        transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityX, 0f);
    }

    void DoCrouch()
    {
        bool wantsCrouch = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        if (!wantsCrouch && _isCrouching)
        {
            // top of capsule
            Vector3 castOrigin = transform.position
                + Vector3.up * (_cc.center.y + _cc.height * 0.5f - _cc.skinWidth);
            float clearance = standingHeight - _cc.height;
            if (clearance > 0.01f && Physics.Raycast(castOrigin, Vector3.up, clearance,
                    ~0, QueryTriggerInteraction.Ignore))
                wantsCrouch = true; // ceiling blocks stand-up
        }

        _isCrouching = wantsCrouch;

        float targetH = _isCrouching ? crouchHeight : standingHeight;
        float newH = Mathf.Lerp(_cc.height, targetH, Time.deltaTime * crouchLerpSpeed);

        _cc.height = newH;
        _cc.center = new Vector3(0f, newH * 0.5f, 0f);

        CurrentEyeHeight = newH * 0.85f - standingHeight * 0.5f;
    }

    Vector3 GetMoveDirection(float h, float v)
    {
        Quaternion yaw = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        Vector3 forward = yaw * Vector3.forward;
        Vector3 right = yaw * Vector3.right;

        return (right * h + forward * v).normalized;
    }

    void DoMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = GetMoveDirection(h, v);

        bool wantRun = Input.GetKey(KeyCode.LeftShift) && !_isCrouching;
        bool canRun = CurrentStamina > staminaRunMin;
        bool running = wantRun && canRun && dir.magnitude > 0.05f;

        if (running)
            CurrentStamina -= staminaDrain * Time.deltaTime;
        else
            CurrentStamina += staminaRegen * Time.deltaTime;

        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, maxStamina);

        float speed = _isCrouching ? crouchSpeed : (running ? runSpeed : walkSpeed);

        if (_cc.isGrounded && _yVelocity < 0f)
            _yVelocity = -0.5f;

        if (Input.GetButtonDown("Jump") && _cc.isGrounded && !_isCrouching)
            _yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);

        _yVelocity += gravity * Time.deltaTime;
        Vector3 move = dir * speed;
        move.y = _yVelocity;
        _cc.Move(move * Time.deltaTime);
    }

    void DoState()
    {
        bool grounded = _cc.isGrounded;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool moving = new Vector2(h, v).magnitude > 0.05f;

        bool running = Input.GetKey(KeyCode.LeftShift) && moving
                       && !_isCrouching
                       && CurrentStamina > staminaRunMin;

        if (!grounded && _yVelocity > 0.5f) CurrentState = PlayerState.Jump;
        else if (!grounded && _yVelocity < -1.5f) CurrentState = PlayerState.Fall;
        else if (_isCrouching) CurrentState = PlayerState.Crouch;
        else if (running) CurrentState = PlayerState.Run;
        else if (moving) CurrentState = PlayerState.Walk;
        else CurrentState = PlayerState.Idle;
    }
}