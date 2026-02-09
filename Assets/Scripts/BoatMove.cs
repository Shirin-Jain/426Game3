using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class BoatMove : MonoBehaviour
{
    public float maxHorizontalSpeed;
    public float horizontalAccel;
    public float horizontalDeccel;

    public float verticalDeccel;

    public float currentAccel = 15;

    public string currentTag = "Current";


    public bool onCurrent = false;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        float horizotnalVelocity = rb.linearVelocity.x;
        float verticalVelocity = rb.linearVelocity.y;


        if (Keyboard.current.spaceKey.isPressed)
        {

            horizotnalVelocity += horizontalAccel * Time.deltaTime;

        }
        else
        {
            horizotnalVelocity -= horizontalDeccel * Time.deltaTime;

        }

        horizotnalVelocity = Mathf.Clamp(horizotnalVelocity, 0, maxHorizontalSpeed);
        // should this be break or reverse (reverse more freedom but break mkaes more sense)

        if (!onCurrent)
        {
            if(verticalVelocity != 0)
            {

                float verticalDirection = 0;

                if (verticalVelocity > 0)
                {
                     verticalDirection = 1f;
                }
                else
                {
                     verticalDirection = -1f;
                }

                verticalVelocity += - verticalDirection *  verticalDeccel * Time.deltaTime;

                if (verticalDirection == -1f)
                {
                    if (verticalVelocity > 0f) verticalVelocity = 0f;

                }
                else
                {
                    if (verticalVelocity < 0f) verticalVelocity = 0f;
                }

            }

        }


        rb.linearVelocity = new Vector2 (horizotnalVelocity, verticalVelocity);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
        {
            onCurrent = true;
        }
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(currentTag))
            return;

        float radians = other.transform.rotation.z;

        Vector2 direction = new Vector2(
            Mathf.Cos(radians),
            Mathf.Sin(radians)
        );

        //Debug.Log("Degrees:" + radians + " X: " + direction.x + "Y: " + direction.y);


        rb.linearVelocity = rb.linearVelocity + (direction * currentAccel * Time.fixedDeltaTime);


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
        {
            onCurrent = false;
        }
    }
   }
