using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalSpeed = 5f;
    public float thrust;
    public float fallSpeedLimit;

    private const string STAR_DESTROY = "Destroy";
    private const float SPEED_X_MIN = 5;
    private const float SPEED_X_MAX = 10;

    private Rigidbody2D m_Rigidbody;
    private readonly int starAnimParaDestroy = Animator.StringToHash(STAR_DESTROY);

    private GameObject _toReclaim;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.velocity = new Vector2(horizontalSpeed, 0);

        UIManager.Instace.UpdateSpeed(m_Rigidbody.velocity.x);
    }

    public void Jump(float upSpeed)
    {
        var newSpeed = m_Rigidbody.velocity;
        newSpeed.y = upSpeed;
        m_Rigidbody.velocity = newSpeed;
    }

    private void FixedUpdate()
    {
        if (m_Rigidbody.velocity.y > -fallSpeedLimit)
        {
            m_Rigidbody.AddForce(Vector2.down * thrust);
        }
    }

    public void Accelerate(float xSpeed)
    {
        var velocity = m_Rigidbody.velocity;
        var controlledSpeed = Mathf.Clamp(velocity.x + xSpeed, SPEED_X_MIN, SPEED_X_MAX);
        if (controlledSpeed == SPEED_X_MAX)
        {
            UIManager.Instace.GameOver(OverStatus.Win);
        }
        velocity.x = controlledSpeed;

        m_Rigidbody.velocity = velocity;

        UIManager.Instace.UpdateSpeed(m_Rigidbody.velocity.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ItemManager.Instance.HasItem(collision.tag)) return;
        var impact = ItemManager.Instance.GetItemImpact(collision.tag);

        Accelerate(impact);

        if (collision.tag == "Star")
        {
            var starGo = collision.gameObject;
            var animator = starGo.GetComponentInChildren<Animator>();
            animator.SetTrigger(starAnimParaDestroy);

            _toReclaim = starGo;

            Invoke("DelayReclaim", 0.57f);
        }
        else
        {
            ItemManager.Instance.Reclaim(collision.gameObject);
        }
    }

    private void DelayReclaim()
    {
        ItemManager.Instance.Reclaim(_toReclaim);
    }
}