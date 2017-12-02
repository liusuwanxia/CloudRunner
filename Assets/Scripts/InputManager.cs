using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const string CloudName = "Cloud";
    private DataManager dm;

    private void Start()
    {
        dm = DataManager.Instance;
    }

    private void Update()
    {
        CaptureCloud();
    }

    private void CaptureCloud()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(clickPosition, LayerMask.GetMask(DataManager.CAN_TOUCH));
            if (collider == null)
            {
                if (dm.CloudCount > 0)
                {
                    //在ItemManager定义一个接口: ComsumeItem(name, position, rotation)
                    ItemManager.Instance.Consume(CloudName, clickPosition);
                    --dm.CloudCount;
                }
            }
            else
            {
                if (dm.CloudCount < DataManager.CloudLimit)
                {
                    ItemManager.Instance.Reclaim(collider.transform.parent.gameObject);
                    ++dm.CloudCount;
                }
            }
        }
    }
}