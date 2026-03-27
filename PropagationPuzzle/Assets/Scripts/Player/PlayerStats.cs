using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Range(0f, 10f)] [SerializeField] float lookSpeed = 1f;
    public float LookSpeed => lookSpeed;

    [Range(0f, 1000f)] [SerializeField] float moveSpeed = 1f;
    public float MoveSpeed => moveSpeed;

    [Range(0f, 10f)][SerializeField] float maxSpeed = 1f;
    public float MaxSpeed => maxSpeed;
}
