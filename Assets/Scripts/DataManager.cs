using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public const string CAN_STEP = "CanStep";
    public const string CAN_TOUCH = "CanTouch";
    public GameObject background;
    public const int CloudLimit = 3;

    private static DataManager _instance;
    private Vector2 bgSize;
    private Vector2 cameraSize;
    private GameObject _mainCamera;
    private int cloudCount = 3;

    public static DataManager Instance
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

    public Vector2 BgSize
    {
        get
        {
            return bgSize;
        }

        private set
        {
            bgSize = value;
        }
    }

    public Vector2 CameraSize
    {
        get
        {
            return cameraSize;
        }

        private set
        {
            cameraSize = value;
        }
    }

    public GameObject MainCamera
    {
        get
        {
            return _mainCamera;
        }

        private set
        {
            _mainCamera = value;
        }
    }

    public int CloudCount
    {
        get
        {
            return cloudCount;
        }

        set
        {
            cloudCount = value;
            UIManager.Instace.UpdateText();
        }
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Instance = this;

        var sprite = background.GetComponent<SpriteRenderer>().sprite;

        //计算背景的实际大小
        Vector2 bounds = sprite.bounds.size;
        Vector2 scale = background.transform.localScale;
        BgSize = new Vector2(bounds.x * scale.x, bounds.y * scale.y);

        //计算相机大小
        var camera = Camera.main;
        Vector2 rightupper = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector2 leftlower = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));

        CameraSize = new Vector2(rightupper.x - leftlower.x, rightupper.y - leftlower.y);

        MainCamera = GameObject.FindWithTag("MainCamera");
    }
}

public class Pool<T> where T : new()
{
    private Queue<T> queue;
    private Action<T> disposeAction;
    private int _count = 0;

    public Pool(Action<T> disposeAction = null)
    {
        queue = new Queue<T>();
        this.disposeAction = disposeAction;
    }

    public int Count
    {
        get
        {
            return _count;
        }

        private set
        {
            _count = value;
        }
    }

    public T Pull()
    {
        T new_obj;
        if (queue.Count == 0)
        {
            new_obj = new T();
        }
        else
        {
            new_obj = queue.Dequeue();
            --Count;
        }

        return new_obj;
    }

    public void Push(T unused)
    {
        if (disposeAction != null)
        {
            disposeAction(unused);
        }
        queue.Enqueue(unused);
        ++Count;
    }
}