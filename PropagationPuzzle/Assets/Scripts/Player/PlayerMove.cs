using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public PlayerStats playerStats;
    public Rigidbody rigid;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector2 movementInput = Rotate(PlayerInput.instance.PlayerMovement.normalized, -transform.rotation.eulerAngles.y);

        Move(movementInput * playerStats.MoveSpeed * Time.deltaTime);
    }
    void Move(Vector2 movement)
    {
        Vector2 movingSpeed = new Vector2(rigid.linearVelocity.x + movement.x, rigid.linearVelocity.z + movement.y) ;

        if (movingSpeed.magnitude <= playerStats.MaxSpeed)
        {
            Vector3 accVector = new Vector3(movement.x, 0, movement.y);
            rigid.AddForce(accVector, ForceMode.Acceleration);
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
