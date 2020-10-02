using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingReader : MonoBehaviour
{
    private static DrawingReader instance;
    Dictionary<int, Vector3[]> dic;
    public static DrawingReader Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            dic = new Dictionary<int, Vector3[]>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveDrawing()
    {
    }

    public void LoadDrawing()
    {
    }
}
