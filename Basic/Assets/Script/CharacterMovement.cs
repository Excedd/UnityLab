using System.Collections;
using System.Threading;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f; // The speed at which the character moves
    public float jumpForce = 20f; // The force applied when the character jumps
    public float ascentSpeed = 2f; // Adjust the speed of ascent
    public Transform playerBody;
    public Transform cameraTransform;
    private Rigidbody rb; // Reference to the character's Rigidbody component
    private bool isJumping = false; // Flag to check if the character is currently jumping
    
    public float mouseSensitivity = 100f;//"velocidad" de movimiento de nuestra c�mara
    public float clampAngle = 80;//�ngulo para limitar la rotaci�n en el eje X
    Quaternion localRotation;
    float rotY = 0;//para guardarme la rotaci�n en Y
    float rotX = 0;//para guardarme la rotaci�n en X

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        playerBody = rb.transform;
        Cursor.lockState = CursorLockMode.Locked; 
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    private void Update()
    {
        // Read input from the "WASD" keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * speed;

        // Apply the movement to the character's Rigidbody component
        rb.velocity = movement;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Check if the character can jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(Jump());
        }

        rotY += mouseX * mouseSensitivity * Time.deltaTime;//rotaci�n en eje Y de la c�mara a trav�s del mov horizontal del rat�n
        rotX -= mouseY * mouseSensitivity * Time.deltaTime;//rotaci�n en eje X de la c�mara a trav�s del mov vertical del rat�n

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);//l�mite inferior y superior del �ngulo

        localRotation = Quaternion.Euler(rotX, 0, 0);//giro la c�mara en eje X
    }
    void FixedUpdate()
    {
        transform.localRotation = localRotation;
        transform.parent.rotation = Quaternion.Euler(0, rotY, 0);
    }
    private IEnumerator Jump()
    {
        isJumping = true;
        float currentJumpForce = jumpForce;

        while (true)
        {
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            currentJumpForce -= ascentSpeed;

            if (currentJumpForce <= 0)
            {
                break;
            }

            yield return null;
        }

        // Descent
        while (!IsGrounded())
        {
            rb.AddForce(Vector3.down * rb.mass * Physics.gravity.magnitude, ForceMode.Acceleration);
            yield return null;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the character has landed on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}