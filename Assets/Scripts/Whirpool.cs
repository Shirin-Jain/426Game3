//using UnityEngine;

//public class Whirlpool : MonoBehaviour
//{
//    [Header("Whirlpool Settings")]
//    public float pullStrength = 20f;
//    public float requiredEntrySpeed = 6f;
//    public float maxPullDistance = 5f;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player"))
//            return;

//        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
//        if (rb == null)
//            return;

//        float entrySpeed = rb.linearVelocity.magnitude;

//        // Player is too slow → trapped
//        if (entrySpeed < requiredEntrySpeed)
//        {
//            PlayerWhirlpoolState state = other.GetComponent<PlayerWhirlpoolState>();
//            if (state != null)
//            {
//                state.EnterWhirlpool(transform, pullStrength, maxPullDistance);
//            }
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (!other.CompareTag("Player"))
//            return;

//        PlayerWhirlpoolState state = other.GetComponent<PlayerWhirlpoolState>();
//        if (state != null)
//        {
//            state.ExitWhirlpool();
//        }
//    }
//}



using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    public float pullStrength = 10f;
    public string playerTag = "Boat";
    public float maxHorizontalForce = 2;

    //[Header("Spin Settings")]
    //public float spinStrength = 200f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector2 directionToCenter = (Vector2)transform.position - rb.position;

        if(directionToCenter.x > maxHorizontalForce)
        {
            directionToCenter.x = maxHorizontalForce;
        }
        float distance = directionToCenter.magnitude;

        // Pull toward center
        Vector2 pullForce = directionToCenter.normalized * pullStrength;
        rb.AddForce(pullForce);

        // Optional: clamp max speed so it doesn't go crazy
        //rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxPullSpeed);

        //// Spin around the center
        //Vector2 tangent = new Vector2(-directionToCenter.y, directionToCenter.x).normalized;
        //rb.AddForce(tangent * spinStrength * Time.fixedDeltaTime);
    }
}
