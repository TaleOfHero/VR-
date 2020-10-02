using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DrawingPanel : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public PenMode.PAINTMODE curPaintMode = PenMode.PAINTMODE.PEN;

    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public bool isPointerDown = false;
    public List<Vector2> fingerPosition;
    
    public List<GameObject> lineList;
    public List<GameObject> saveLineList;



    public Transform hoverSphereTransform = null;
    private Color lineColor;
    //싱글턴
    #region
    private static DrawingPanel instance = null;

    public static DrawingPanel Instance
    {
        get 
        {
            if(null ==instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion //싱글턴 
    void Awake()
    {
        if(Instance ==null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            lineList = new List<GameObject>();
            saveLineList = new List<GameObject>();
            lineColor = Color.black;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    void Update()
    {
        if (currentLine != null)
        {
            currentLine.transform.localPosition =
               new Vector3(currentLine.transform.localPosition.x, currentLine.transform.localPosition.y, -0.1f);
        }
        //처음 눌릴때
        if (Input.GetMouseButtonDown(0)&&isPointerDown)
        {
            CreateLine();
        }
        //눌릴대 판정
        if(Input.GetMouseButton(0) && isPointerDown)
        {
            Vector3 hoverPosition = new Vector3(hoverSphereTransform.transform.position.x, hoverSphereTransform.transform.position.y, 0);
            Vector2 tempFingerPos = hoverPosition;
            if(Vector2.Distance(tempFingerPos,fingerPosition[fingerPosition.Count-1]) > 0.05f)
            {
                UpdateLine(tempFingerPos);
            }
        }
    }

    public void SaveLine()
    {
        for (int i = 0; i < saveLineList.Count; i++)
            Destroy(saveLineList[i].gameObject);
        saveLineList.Clear();
        
        for(int i= 0;i<lineList.Count;i++)
        {
            saveLineList.Add(Instantiate(lineList[i], Vector3.zero, Quaternion.identity));
            saveLineList[i].transform.SetParent(this.gameObject.transform);
            saveLineList[i].gameObject.transform.localPosition =
               new Vector3(lineList[i].gameObject.transform.localPosition.x, lineList[i].gameObject.transform.localPosition.y, -0.1f);
            saveLineList[i].gameObject.transform.localScale =
               new Vector3(lineList[i].gameObject.transform.localScale.x, lineList[i].gameObject.transform.localScale.y, lineList[i].gameObject.transform.localScale.z);
            saveLineList[i].gameObject.SetActive(false);
        }
    }
    public void LoadLine()
    {
        ResetLine();
        for (int i = 0; i < saveLineList.Count; i++)
        {
            lineList.Add(Instantiate(saveLineList[i]));
            lineList[i].transform.SetParent(this.gameObject.transform);
            lineList[i].gameObject.transform.localPosition =
               new Vector3(saveLineList[i].gameObject.transform.localPosition.x, saveLineList[i].gameObject.transform.localPosition.y, -0.1f);
            lineList[i].gameObject.transform.localScale =
               new Vector3(saveLineList[i].gameObject.transform.localScale.x, saveLineList[i].gameObject.transform.localScale.y, saveLineList[i].gameObject.transform.localScale.z);
            lineList[i].gameObject.SetActive(true);
            lineList[i].gameObject.name = "line";
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
       
        currentLine.gameObject.transform.SetParent(this.gameObject.transform);

        lineRenderer = currentLine.GetComponent<LineRenderer>();

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        fingerPosition.Clear();

        fingerPosition.Add(hoverSphereTransform.transform.position);
        fingerPosition.Add(hoverSphereTransform.transform.position);

        lineRenderer.SetPosition(0, fingerPosition[0]);
        lineRenderer.SetPosition(1, fingerPosition[1]);
    }
   
    void UpdateLine(Vector2 newFingerPos)
    {
        fingerPosition.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    }

    public void UndoLine()// ctrl z
    {
        if (lineList != null)
        {
            Destroy(lineList[lineList.Count - 1]);
            lineList.RemoveAt(lineList.Count - 1);
        }
    }

    public void ChangeLineColor(Color color)
    {
        lineColor = color;
    }

    public void ResetLine()
    {
        for(int i = 0; i<lineList.Count;i++)
            Destroy(lineList[i]);
        lineList.Clear();
    }

    public void setPaintMode(int paintMode)
    {
        switch (paintMode)
        {
            case (int)PenMode.PAINTMODE.PEN:
                {
                    curPaintMode = PenMode.PAINTMODE.PEN;
                    break;
                }
            case (int)PenMode.PAINTMODE.ERASER:
                {
                    curPaintMode = PenMode.PAINTMODE.ERASER;
                    break;
                }

            default:
                Debug.LogError("존재하지 않는 펜");
                break;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        switch (curPaintMode)
        {
            case PenMode.PAINTMODE.PEN:
                break;
            case PenMode.PAINTMODE.ERASER:
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        lineList.Add(currentLine);
    }
}
