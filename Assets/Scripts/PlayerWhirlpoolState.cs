using UnityEngine;

public class PlayerWhirlpoolState : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform whirlpoolCenter;

    private bool trapped = false;
    private float pullStrength;
    private float maxPullDistance;
    private bool inWhirlpool = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void EnterWhirlpool(Transform center, float strength, float maxDistance)
    {
        whirlpoolCenter = center;
        pullStrength = strength;
        maxPullDistance = maxDistance;
        trapped = true;

    }

    public void ExitWhirlpool()
    {
        trapped = false;
        whirlpoolCenter = null;
    }

    private void FixedUpdate()
    {
        if (!trapped || whirlpoolCenter == null)
            return;

        Debug.Log("Trapped: " + trapped + "In whirpool:" + inWhirlpool);

        Vector2 direction = (whirlpoolCenter.position - transform.position);
        float distance = direction.magnitude;

      

        direction.Normalize();

        // Stronger pull closer to center
        float forceMultiplier = 1f - (distance / maxPullDistance);
        Vector2 force = direction * pullStrength * forceMultiplier;


        Debug.Log("Force : " + force.x + ", " + force.y);

        //if(force.x > 2)
        //{
        //    force.x = 0;
        //}
        rb.AddForce(force, ForceMode2D.Force);
    }
}
