using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public PlayerStats playerStats;
    public Rigidbody rigid;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 movementInput = Rotate(PlayerInput.instance.PlayerMovement.normalized, -transform.rotation.eulerAngles.y);

        Move(movementInput * playerStats.MoveSpeed * Time.fixedDeltaTime);
    }
    void Move(Vector2 movement)
    {
           
       
            
        Vector3 accVector = new Vector3(movement.x, 0, movement.y);
        Debug.Log(accVector);
        rigid.AddForce(accVector, ForceMode.Acceleration);

        Vector2 movingSpeed = new Vector2(rigid.linearVelocity.x, rigid.linearVelocity.z);
        if (movingSpeed.magnitude > playerStats.MaxSpeed)
        {
            Vector2 movingDirection = movingSpeed.normalized;
            Vector2 movingSpeedCapped = movingDirection * playerStats.MaxSpeed;
            rigid.linearVelocity = new Vector3(movingSpeedCapped.x, rigid.linearVelocity.y, movingSpeedCapped.y);
        }

    }
    public static Vector2 Rotate(Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }
}
