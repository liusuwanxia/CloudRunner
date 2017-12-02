using UnityEngine;

public class DetectJump : MonoBehaviour
{
    public Transform footTransform;
    public float distanceThreshold = 0.5f;
    public float upSpeed = 14f;

    private PlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(footTransform.position, Vector2.down, distanceThreshold, LayerMask.GetMask(DataManager.CAN_STEP));

        if (hit2D.collider == null) return;
        var collider_tag = hit2D.collider.tag;
        if (ItemManager.Instance.CanStep(collider_tag))
        {
            switch (collider_tag)
            {
                case "Bird":
                    CollideBird();
                    break;

                case "Cloud":
                    CollideCloud();
                    break;

                default:
                    break;
            }
            ItemManager.Instance.Reclaim(hit2D.collider.gameObject);
        }
    }

    private void CollideBird()
    {
        movement.Jump(upSpeed);
        var acce = ItemManager.Instance.GetItemImpact("Star") * 2;
        movement.Accelerate(acce);
    }

    private void CollideCloud()
    {
        movement.Jump(upSpeed);
    }
}