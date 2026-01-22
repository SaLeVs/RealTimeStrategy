using System;
using UnityEngine;

public class UnitSelectionManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform selectionAreaRectTransform;
    [SerializeField] private UnitSelectionManager unitSelectionManager;
    
    [SerializeField] private Canvas canvas;
    
    
    private void Awake()
    {
        unitSelectionManager.OnSelectionStart += UnitSelectionManager_OnSelectionStart;
        unitSelectionManager.OnSelectionEnd += UnitSelectionManager_OnSelectionEnd;
        
        selectionAreaRectTransform.gameObject.SetActive(false);
        
    }

    private void UnitSelectionManager_OnSelectionStart()
    {
        selectionAreaRectTransform.gameObject.SetActive(true);
        UpdateVisual();
    }
    
    private void UnitSelectionManager_OnSelectionEnd()
    {
        selectionAreaRectTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (selectionAreaRectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }
    
    private void UpdateVisual()
    {
        Rect selectionAreaRect = unitSelectionManager.GetSelectionAreaRect();
        
        float canvasScale = canvas.transform.localScale.x;
        
        selectionAreaRectTransform.anchoredPosition = new Vector2(selectionAreaRect.x, selectionAreaRect.y ) / canvasScale;
        selectionAreaRectTransform.sizeDelta = new Vector2(selectionAreaRect.width, selectionAreaRect.height) / canvasScale;
    }
    
}
