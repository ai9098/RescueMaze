using UnityEngine;

public class StraightEnemy : EnemyBase
{
    [SerializeField] private float moveSpeed = 3.0f;  // ’Êí‚ÌˆÚ“®‘¬“x

    // ’Êí‚ÌˆÚ“®ˆ—‚ğã‘‚«
    protected override void NormalMovement()
    {
        // ’¼i‚É–ß‚é
        Vector3 move = transform.forward * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        // •Ç‚É‚Ô‚Â‚©‚Á‚½‚ç”½‘Î•ûŒü‚É
        if (collision.gameObject.CompareTag("Wall"))
        {
            // y²‰ñ“]
            transform.Rotate(0, 180, 0);
        }
    }
}
