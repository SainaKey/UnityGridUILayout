using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GridUILayout
{
    public class GridBackGroundManager : MonoBehaviour
    {
        [Header("UserSettings")]
        [SerializeField] private Transform guisPool;

        [SerializeField] private bool isGridMode = true;

        [SerializeField] private Canvas parentCanvas;
        [SerializeField] private RectTransform parentRectTransform;
        
        [SerializeField] private GameObject gridImagePrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        [SerializeField] private Vector2 gridSize;
        
    private List<GridBlock> gridBlocks = new List<GridBlock>();
    private List<GridContent> gridContents = new List<GridContent>();
    private Vector2 bgSize;
    private int total;
    float x_count;
    float y_count;

    private GameObject cloneObj;

    public List<GridContent> GridContents
    {
        get { return this.gridContents; }
    }
    public Vector2 GridSize
    {
        get { return this.gridSize; }
    }
    public Canvas ParentCanvas
    {
        get { return this.parentCanvas; }
    }
    public List<GridBlock> GridBlocks
    {
        get { return this.gridBlocks; }
    }

    public Transform GUIsPool
    {
        get { return this.guisPool; }
    }

    private void Start()
    {
        bgSize = parentRectTransform.sizeDelta;
        gridLayoutGroup.cellSize = gridSize;

        x_count = (int)bgSize.x / (int)gridSize.x;
        y_count = (int)bgSize.y / (int)gridSize.y;

        total = (int) (x_count * y_count);
        for (int i = 0; i <total; i++)
        {
            cloneObj = Instantiate(gridImagePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, gridLayoutGroup.transform);
            cloneObj.transform.localPosition = new Vector3(0, 0, 0);
            gridBlocks.Add(cloneObj.GetComponent<GridBlock>());
        }
    }

    private void Update()
    {
        if (isGridMode)
        {
            gridLayoutGroup.gameObject.SetActive(true);
        }
        else
        {
            gridLayoutGroup.gameObject.SetActive(false);
        }
        
        if (bgSize != parentRectTransform.sizeDelta)
        {
            float x_countTMP = (int)bgSize.x / (int)gridSize.x;
            float y_countTMP = (int)bgSize.y / (int)gridSize.y;
            
            int totalTMP = ((int)x_countTMP * (int)y_countTMP);

            int differenceX = (int)x_count - (int)x_countTMP;
            int differenceY = (int)x_count - (int)x_countTMP;
            
            int difference = total - totalTMP;
            if (difference < 0)
            {
                //Increased
                for (int i = 0; i <Mathf.Abs(difference); i++)
                {
                    cloneObj = Instantiate(gridImagePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, gridLayoutGroup.transform);
                    cloneObj.transform.localPosition = new Vector3(0, 0, 0);
                    gridBlocks.Add(cloneObj.GetComponent<GridBlock>());
                }
            }
            else
            {
                //Decreased
                for (int i = 0; i <Mathf.Abs(difference); i++)
                {
                    GameObject deleteTarget = gridLayoutGroup.transform.GetChild(i).gameObject;
                    int index = gridBlocks.IndexOf(deleteTarget.GetComponent<GridBlock>());
                    gridBlocks.RemoveAt(index);
                    Destroy(deleteTarget);
                }
            }
            
            x_count = x_countTMP;
            y_count = y_countTMP;
            total = totalTMP;
        }
        bgSize = parentRectTransform.sizeDelta;
    }
}
}

