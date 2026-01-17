using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CharacterController : Singleton<CharacterController>
{
    public float speed = 5;
    public float sprintSpeed = 7.5f;
    public float jumpVelocity = 8f; // default jump speed
    public float playerGravity = -8f; // default gravity speed (only for player, also unitys gravity is turned off)
    bool grounded; // true if touching the ground
    [SerializeField] float groundDistance = 1.1f; // grace distance for grounded state (how close you have to be to be "grounded")
    // ground distance is high because I'm factoring in player height
    public float rollSpeed = 20f; // speed during a dodge roll
    public float rollDuration = 0.15f; // how long they player should move fast while rolling
    public float rollCooldown = 1f; // how long the player has to wait after a roll
    bool rolling; // true when rolling
    bool cooldown; // true if in cooldown
    Vector3 rollDir; // direciton of roll (so you can't change direction mid-roll)
    [SerializeField] LayerMask groundLayer; // which layer makes the player grounded

    [HideInInspector] public Vector2 inputVec;
    bool isSprinting = false;
    [HideInInspector] public Rigidbody rb;

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
        if (rolling)
        {
            moveVel = new Vector3(rollDir.x * rollSpeed, 0, rollDir.z * rollSpeed);
        }
        rb.linearVelocity = moveVel + influenceVelocity + new Vector3(0,rb.linearVelocity[1],0);
        influenceVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        rb.AddForce(0, playerGravity, 0, ForceMode.Acceleration);
        Vector3 pos = rb.position;
        Vector3 minBounds = new Vector3(-10,0,-10);
        Vector3 maxBounds = new Vector3(10, 100, 10);
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        pos.z = Mathf.Clamp(pos.z, minBounds.z, maxBounds.z);
        rb.position = pos;
    }

    // raycast for ground layer down
    void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }

    // rolling state (fast dash move)
    IEnumerator Rolling()
    {
        rolling = true;
        float timer = rollDuration;
        while (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rolling = false;
        cooldown = true;
        yield return new WaitForSeconds(rollCooldown);
        cooldown = false;
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

    public void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!(!ctx.performed && ctx.started)) return;
        if (!grounded || rolling || cooldown) return;

        if (inputVec.sqrMagnitude > 0.01f) // if the player is moving dash goes in that direction
        {
            rollDir = new Vector3(inputVec.x, 0, inputVec.y).normalized;

        }
        else // if they aren't, it's just whatever direction they are facing (which I don't actually think changes at all as of writing)
        {
            rollDir = transform.forward;
        }
        StartCoroutine(Rolling());
    }
    #endregion
}
