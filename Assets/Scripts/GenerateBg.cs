using UnityEngine;

public class GenerateBg : MonoBehaviour
{
    private float bgWidth;

    private Pool<GameObject> bgPool;
    private GameObject preBg;
    private GameObject currentBg;
    private float nextPos;

    private void Start()
    {
        bgWidth = DataManager.Instance.BgSize.x;

        bgPool = new Pool<GameObject>(go => go.SetActive(false));

        currentBg = this.transform.GetChild(0).gameObject;
        preBg = currentBg;

        nextPos = currentBg.transform.position.x + bgWidth / 2;
    }

    private void Update()
    {
        var camera = GameObject.FindWithTag("MainCamera");
        float leftBorder = camera.transform.position.x - DataManager.Instance.CameraSize.x / 2;
        float rightBorder = camera.transform.position.x + DataManager.Instance.CameraSize.x / 2;

        if (rightBorder > nextPos)
        {
            GameObject newBg;
            if (bgPool.Count == 0)
            {
                newBg = Instantiate(DataManager.Instance.background, this.transform);
            }
            else
            {
                newBg = bgPool.Pull();
            }

            newBg.transform.position = new Vector2(nextPos + bgWidth / 2, 0);

            newBg.SetActive(true);
            preBg = currentBg;
            currentBg = newBg;

            nextPos += bgWidth;
        }
        else if (leftBorder > preBg.transform.position.x + bgWidth / 2)
        {
            bgPool.Push(preBg);
            preBg = currentBg;
        }
    }
}