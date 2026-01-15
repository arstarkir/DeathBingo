using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : Singleton<CharacterController>
{
    public float speed = 5;
    public float sprintSpeed = 7.5f;
    public float jumpVelocity = 8f; // default jump speed
    public float playerGravity = -8f; // default gravity speed (only for player, also unitys gravity is turned off)
    bool grounded; // true if touching the ground
    [SerializeField] float groundDistance = 1.1f; // grace distance for grounded state (how close you have to be to be "grounded")
    // ground distance is high because I'm factoring in player height
    [SerializeField] LayerMask groundLayer; // which layer makes the player grounded

    [HideInInspector] public Vector2 inputVec;
    bool isSprinting = false;
    Rigidbody rb;

    public bool isInteractable = true;
    [HideInInspector] public Vector3 influenceVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isInteractable)
            return;

        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 moveVel = new Vector3(inputVec.x, 0, inputVec.y) * (isSprinting ? sprintSpeed : speed);
        rb.linearVelocity = moveVel + influenceVelocity + new Vector3(0,rb.linearVelocity[1],0);
        influenceVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        rb.AddForce(0, playerGravity, 0, ForceMode.Acceleration);
    }

    // raycast for ground layer down
    void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }

    #region Input System Callbacks
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isInteractable)
            return;
        inputVec = ctx.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        isSprinting = !isSprinting;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!(!ctx.performed && ctx.started)) return; // this line makes it so you only jump on button press and not release, funky new unity input stuff
        if (!grounded) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        grounded = false;
    }
    #endregion
}
