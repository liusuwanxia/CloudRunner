using UnityEngine;

/// <summary>
/// 被添加到所有的Item上, 用于检测边界碰撞并销毁自身
/// </summary>
public class DestroyByBound : MonoBehaviour
{
    private DataManager gm;
    private Transform transformCamera;

    private void Start()
    {
        gm = DataManager.Instance;
        transformCamera = gm.MainCamera.transform;
    }

    private void Update()
    {
        //考虑在GameManager中计算并提供接口获取
        var bound = transformCamera.position.x - gm.CameraSize.x / 2;

        if (this.transform.position.x < bound)
        {
            ItemManager.Instance.Reclaim(this.gameObject);
        }
    }
}