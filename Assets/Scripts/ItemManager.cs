using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public float generateDistance = 4f;
    public float offset = 0.5f;
    public Item[] itemList;

    private static ItemManager _instance;
    private GameObject mainCamera;
    private DataManager gm;
    private float nextGeneratePos;
    private int total;
    private Dictionary<string, Item> itemMap;
    private Dictionary<string, int> itemPriority;
    private Dictionary<string, Pool<GameObject>> itemPools;
    private float camHeight;
    private float camWidth;

    public static ItemManager Instance
    {
        get
        {
            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gm = DataManager.Instance;
        mainCamera = gm.MainCamera;
        nextGeneratePos = mainCamera.transform.position.x + generateDistance;
        camHeight = gm.CameraSize.y;
        camWidth = gm.CameraSize.x;

        total = 0;
        itemMap = new Dictionary<string, Item>();
        itemPriority = new Dictionary<string, int>();
        itemPools = new Dictionary<string, Pool<GameObject>>();

        if (itemList != null)
        {
            foreach (var item in itemList)
            {
                total += item.weight;
                itemMap.Add(item.name, item);
                itemPriority.Add(item.name, total);
                itemPools.Add(item.name, new Pool<GameObject>(go => go.SetActive(false)));
            }
        }
    }

    private void Update()
    {
        var camPos = mainCamera.transform.position;
        if (camPos.x > nextGeneratePos)
        {
            int random = Random.Range(0, total);

            var iter = itemPriority.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current.Key;
                var priority = iter.Current.Value;
                if (random < priority)
                {
                    //使用 Pool 优化
                    Spawn(item, RandomSpawnPos(item, camPos));
                    break;
                }
            }

            nextGeneratePos += generateDistance;
        }
    }

    private Vector2 RandomSpawnPos(string itemName, Vector2 camPos)
    {
        //float posX = nextGeneratePos + camWidth / 2 + offset;
        //float posY = Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2);
        //var generatePos = new Vector2(posX, posY);
        Vector2 spawnPos = new Vector2
        {
            x = nextGeneratePos + camWidth / 2 + offset
        };

        switch (itemName)
        {
            case "Bird":
            case "Cloud":
                if (Random.value > 0.333333333f)
                {
                    spawnPos.y = Random.Range(camPos.y - camHeight / 2, camPos.y);
                }
                else
                {
                    spawnPos.y = Random.Range(camPos.y, camPos.y - 1 + camHeight / 2);
                }
                break;

            case "Star":
                if (Random.value < 0.333333333f)
                {
                    spawnPos.y = Random.Range(camPos.y + 1 - camHeight / 2, camPos.y);
                }
                else
                {
                    spawnPos.y = Random.Range(camPos.y, camPos.y - 1 + camHeight / 2);
                }
                break;

            default:
                break;
        }

        return spawnPos;
    }

    private void Spawn(string itemName, Vector2 pos)
    {
        Pool<GameObject> pool;
        if (itemPools.TryGetValue(itemName, out pool))
        {
            GameObject go;
            Item item = itemMap[itemName];

            if (pool.Count == 0)
            {
                go = Instantiate(item.prefab, pos, Quaternion.identity);
                go.AddComponent<DestroyByBound>();
                go.name = item.name;
                go.tag = item.name;
                if (item.canStep)
                {
                    go.layer = LayerMask.NameToLayer(DataManager.CAN_STEP);
                }
            }
            else
            {
                go = pool.Pull();
                go.transform.position = pos;
                go.transform.rotation = Quaternion.identity;
                go.SetActive(true);
            }
        }
    }

    public void Consume(string itemName, Vector2 pos)
    {
        if (itemMap.ContainsKey(itemName))
        {
            Spawn(itemName, pos);
        }
    }

    public float GetItemImpact(string itemName)
    {
        return itemMap[itemName].impact;
    }

    public bool HasItem(string tag)
    {
        return itemMap.ContainsKey(tag);
    }

    public bool CanStep(string tag)
    {
        if (HasItem(tag))
        {
            return itemMap[tag].canStep;
        }
        return false;
    }

    /// <summary>
    /// 由生成的Item物体调用, 用于销毁自身
    /// </summary>
    /// <param name="go"></param>
    public void Reclaim(GameObject go)
    {
        if (itemMap.ContainsKey(go.name))
        {
            var pool = itemPools[go.name];
            pool.Push(go);
        }
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public GameObject prefab;
    public int weight;
    public float impact;
    public bool canStep = true;
}