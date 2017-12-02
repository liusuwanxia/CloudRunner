using UnityEngine;

public class Mover : MonoBehaviour
{
    public Vector2 speed;

    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_Rigidbody.velocity = speed;
    }
}