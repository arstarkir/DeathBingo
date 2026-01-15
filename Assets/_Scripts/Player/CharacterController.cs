using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : Singleton<CharacterController>
{
    public float speed = 5;
    public float sprintSpeed = 7.5f;
    public float jumpVelocity = 8f; // default jump speed
    public float playerGravity = -8f; // default gravity speed (only for player, also unitys gravity is turned off)
    public bool grounded; // true if touching the ground

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
        rb.AddForce(0, playerGravity, 0, ForceMode.Acceleration);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
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

    public void OnJump()
    {
        if (!grounded) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        grounded = false;
    }
    #endregion
}
