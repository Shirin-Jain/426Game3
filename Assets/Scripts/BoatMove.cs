using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMove : MonoBehaviour
{
    public float maxHorizontalSpeed;
    public float horizontalAccel;
    public float horizontalDeccel;
    public float bounceDeccel;

    public float verticalDeccel;
    public float currentAccel = 15;

    public string currentTag = "Current";
    public bool onCurrent = false;

    public Rigidbody2D rb;
    private bool isDead = false;

    public bool isBouncing = false;
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
      
    }

    void Update()
    {
        if (isDead) return;

        float horizotnalVelocity = rb.linearVelocity.x;
        float verticalVelocity = rb.linearVelocity.y;

        if (!isBouncing)
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                horizotnalVelocity += horizontalAccel * Time.deltaTime;
            }
            else
            {
                horizotnalVelocity -= horizontalDeccel * Time.deltaTime;
            }

            horizotnalVelocity = Mathf.Clamp(horizotnalVelocity, 0, maxHorizontalSpeed);
        }
        else
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                horizotnalVelocity += horizontalAccel * Time.deltaTime;
            }
            else
            {
                horizotnalVelocity += bounceDeccel * Time.deltaTime;

            }

            if (horizotnalVelocity >= 0)
            {
                isBouncing = false;
            }

        }



        if (!onCurrent && verticalVelocity != 0)
        {
            float verticalDirection = verticalVelocity > 0 ? 1f : -1f;
            verticalVelocity += -verticalDirection * verticalDeccel * Time.deltaTime;

            if (verticalDirection == -1f && verticalVelocity > 0f)
                verticalVelocity = 0f;
            else if (verticalDirection == 1f && verticalVelocity < 0f)
                verticalVelocity = 0f;
        }

        rb.linearVelocity = new Vector2(horizotnalVelocity, verticalVelocity);

    }

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
            onCurrent = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(currentTag) || isDead)
            return;

        float radians = other.transform.rotation.z;

        Vector2 direction = new Vector2(
            Mathf.Cos(radians),
            Mathf.Sin(radians)
        );

        rb.linearVelocity += direction * currentAccel * Time.fixedDeltaTime;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
            onCurrent = false;
    }

    public void Bounce(float impact, Vector2 impactSpeed)
    {
        
        Debug.Log("Vector: " + impactSpeed.x + ", " + impactSpeed.y);
        rb.linearVelocity = new Vector2 (-impactSpeed.x, impactSpeed.y);
        isBouncing = true;
    }
}
