using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;

    //public float heightLimit = 3.9f;
    public float smooth = 1f;

    public Vector2 offset;

    private float VerticalLimit;

    private void Start()
    {
        VerticalLimit = (DataManager.Instance.BgSize.y - DataManager.Instance.CameraSize.y) / 2;
    }

    private void Update()
    {
        Vector2 targetPos = target.position;

        if (Vector2.Distance(transform.position, targetPos + offset) > 0.001f)
        {
            var position = Vector2.Lerp(this.transform.position, targetPos + offset, smooth * Time.deltaTime);
            position.y = Mathf.Clamp(position.y, -VerticalLimit, VerticalLimit);
            this.transform.position = new Vector3(position.x, position.y, -10);
        }
    }
}