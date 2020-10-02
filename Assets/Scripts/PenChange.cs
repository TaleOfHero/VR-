using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenChange : MonoBehaviour
{
    private Button btn;
    public PenMode.PAINTMODE curPaintMode;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetPen);
    }


    private void SetPen()
    {
        DrawingPanel.Instance.setPaintMode((int)curPaintMode);
    }
}
