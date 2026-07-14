using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Camera Rotation")]
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    [Header("Ground Movement")]
    public float MoveSpeed = 5f;
    private CharacterController controller;
    private float moveHorizontal;
    private float moveForward;

    [Header("Jumping & Gravity")]
    public float jumpForce = 2f;
    public float gravity = -19.62f;
    public LayerMask groundLayer;

    private Vector3 velocity;
    private RaycastHit groundHit;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Sounds")]
    public AudioSource jump;
    public AudioClip[] jumpSounds;
    public AudioSource walk;

    // Inicialización
    void Start() {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Actualización de cada frame
    void Update() {
        MovePlayer();
        HandleAnimations();
        HandleWalkingSound();
    }

    void LateUpdate() {
        RotateCamera();
    }

    private void MovePlayer() {
        bool isRealGround = IsActuallyGrounded();

        if (isRealGround && velocity.y < 0) {
            float slopeAngle = Vector3.Angle(Vector3.up, groundHit.normal);
            if (slopeAngle > 0.1f) {
                velocity.y = -MoveSpeed * 2f;
            } else {
                velocity.y = -2f;
            }
        } else {
            velocity.y += gravity * Time.deltaTime;
        }

        // Input
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");
        Vector3 horizontalMove = (transform.right * moveHorizontal + transform.forward * moveForward).normalized * MoveSpeed;

        // Salto
        if (Input.GetButtonDown("Jump") && isRealGround) {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            PlayJumpSound();
        }

        Vector3 finalMovement = horizontalMove + (Vector3.up * velocity.y);
        controller.Move(finalMovement * Time.deltaTime);
    }

    // Esta en el suelo
    private bool IsActuallyGrounded() {
        Vector3 sphereOrigin = transform.position + controller.center;
        float radius = controller.radius * 0.9f;
        float distance = (controller.height / 2f) + 0.4f;

        if (Physics.SphereCast(sphereOrigin, radius, Vector3.down, out groundHit, distance, groundLayer)) {
            if (Vector3.Angle(Vector3.up, groundHit.normal) <= controller.slopeLimit) {
                return true;
            }
        }
        return false;
    }

    private void RotateCamera() {
        if (Cursor.lockState != CursorLockMode.Locked) { return; }
        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // Animaciones
    private void HandleAnimations() {
        if (animator != null) {
            animator.SetFloat("MoveX", moveHorizontal);
            animator.SetFloat("MoveZ", moveForward);

            bool isRealGround = IsActuallyGrounded();
            animator.SetBool("IsJumping", !isRealGround && velocity.y > 0);
            animator.SetBool("IsFalling", !isRealGround && velocity.y < 0);
        }
    }

    // Sonidos
    private void HandleWalkingSound() {
        if (IsActuallyGrounded() && (Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveForward) > 0.1f)) {
            if (walk != null && !walk.isPlaying) { walk.Play(); }
        } else {
            if (walk != null) { walk.Stop(); }
        }
    }

    private void PlayJumpSound() {
        if (jumpSounds.Length > 0 && jump != null) {
            jump.clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
            jump.pitch = Random.Range(0.9f, 1.1f);
            jump.Play();
        }
    }

    // Al desactivar el script
    private void OnDisable() {
        moveHorizontal = 0f;
        moveForward = 0f;
        if (animator != null) {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", 0f);
        }
        if (walk != null && walk.isPlaying) { walk.Stop(); }
    }
}