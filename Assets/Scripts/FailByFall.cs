using UnityEngine;

public class FailByFall : MonoBehaviour
{
    private void Update()
    {
        if (this.transform.position.y < -DataManager.Instance.BgSize.y / 2 - 10)
        {
            UIManager.Instace.GameOver(OverStatus.Lose);
        }
    }
}