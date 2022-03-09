using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace GridUILayout
{
    [RequireComponent(typeof(BoxCollider))]
    public class GridContent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler ,IPointerDownHandler , IPointerUpHandler
    {
        [Header("UserSettings")] 
        [SerializeField] private int blockSize;

        [Header("DeveloperSettings")] 
        private Vector3 old_pos;


        [Header("RunTime")] 
        private BoxCollider boxCollider;
        private List<GridBlock> gridBlocks = new List<GridBlock>();
        private List<GridBlock> usedGridBlocks = new List<GridBlock>();
        private Vector2 gridSize;
        private GridBackGroundManager gridBackGroundManager;
        private RectTransform rectTransform;
        private Canvas uiCanvas;
        private Transform guisPool;
        private GridBlock firstGridBlock;
        private bool isClick;
        

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            boxCollider = GetComponent<BoxCollider>();
            Transform parent = rectTransform.parent;
            while (!parent.TryGetComponent(out GridBackGroundManager gridMG))
            {
                parent = parent.parent;
            }

            gridBackGroundManager = parent.GetComponent<GridBackGroundManager>();
            uiCanvas = gridBackGroundManager.ParentCanvas;
            guisPool = gridBackGroundManager.GUIsPool;
            gridBackGroundManager.GridContents.Add(GetComponent<GridContent>());

            blockSize = (int)rectTransform.sizeDelta.x / (int)gridBackGroundManager.GridSize.x;
            blockSize += (int)rectTransform.sizeDelta.y / (int)gridBackGroundManager.GridSize.y;

            rectTransform.anchorMax = new Vector2(0,1);
            rectTransform.anchorMin = new Vector2(0,1);
            boxCollider.size = rectTransform.sizeDelta;
        }

        private void Update()
        {
            if (!isClick)
            {
                return;
            }
            
            if (Input.GetMouseButton(0))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Vector2 oldPivot = rectTransform.pivot;
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);

                    if (rectTransform.localEulerAngles.z <= 360 + 10)
                        rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x,
                            rectTransform.localEulerAngles.y, rectTransform.localEulerAngles.z + 90);

                    else
                        rectTransform.localEulerAngles = Vector3.zero;

                    rectTransform.pivot = oldPivot;
                }
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isClick = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isClick = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            rectTransform.SetAsLastSibling();
            boxCollider.enabled = true;
            foreach (GridBlock gridBlock in usedGridBlocks)
            {
                gridBlock.isUsed = false;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / uiCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            int count = 0;
            foreach (GridBlock gridBlock in gridBackGroundManager.GridBlocks)
            {
                if(gridBlock.isUsed)
                    continue;
                
                
                if (gridBlock.onGridContent)
                {
                    if (count == 0)
                    {
                        firstGridBlock = gridBlock;
                    }
                    count++;
                }
            }

            if (count >= blockSize)
            {
                rectTransform.SetParent(firstGridBlock.transform,false);
                rectTransform.localScale = new Vector3(1,1,1);
                if (rectTransform.localEulerAngles.z <= 0 + 45)
                {
                    rectTransform.localPosition = new Vector3(rectTransform.sizeDelta.x / 2 - 25,-rectTransform.sizeDelta.y / 2 + 25,0);
                }
                else if (rectTransform.localEulerAngles.z <= 90 + 45)
                {
                    rectTransform.localPosition = new Vector3(rectTransform.sizeDelta.y / 2 - 25,-rectTransform.sizeDelta.x / 2 + 25,0);
                }
                else if (rectTransform.localEulerAngles.z <= 180 + 45)
                {
                    rectTransform.localPosition = new Vector3(rectTransform.sizeDelta.x / 2 - 25,-rectTransform.sizeDelta.y / 2 + 25,0);
                }
                else if (rectTransform.localEulerAngles.z <= 360 + 45)
                {
                    rectTransform.localPosition = new Vector3(rectTransform.sizeDelta.y / 2 - 25,-rectTransform.sizeDelta.x / 2 + 25,0);
                }
                
                rectTransform.SetParent(guisPool,true);

                old_pos = rectTransform.localPosition;
            }
            else
            {
                rectTransform.localPosition = old_pos;
                return;
            }
            
            
            usedGridBlocks.Clear();
            foreach (GridBlock gridBlock in gridBackGroundManager.GridBlocks)
            {
                if (gridBlock.onGridContent)
                {
                    gridBlock.isUsed = true;
                    usedGridBlocks.Add(gridBlock);
                }
            }
            
            boxCollider.enabled = false;
        }
    }
}

