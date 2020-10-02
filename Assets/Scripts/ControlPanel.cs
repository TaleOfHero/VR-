using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    public Button undoButton = null;
    public GameObject ColorContent = null;
    public Button[] colorButton = null;
    public Button saveButton = null;
    public Button loadButton = null;
    void Start()
    {
        colorButton = ColorContent.GetComponentsInChildren<Button>();
        for(int i=0; i<colorButton.Length;i++)
        {
            Color buttonColor = colorButton[i].gameObject.GetComponent<Image>().color;
            colorButton[i].onClick.AddListener(
                ()=>DrawingPanel.Instance.ChangeLineColor(buttonColor));
        }
        undoButton.onClick.AddListener(DrawingPanel.Instance.UndoLine);
        saveButton.onClick.AddListener(DrawingPanel.Instance.SaveLine);
        loadButton.onClick.AddListener(DrawingPanel.Instance.LoadLine);
    }
}
