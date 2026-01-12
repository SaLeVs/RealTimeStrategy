using UnityEngine;

// Managers is a common naming convention for MonoBehaviour classes and Systems that handle DOTS logic
public class UnitSelectionManager : MonoBehaviour
{
    private Vector3 _mouseWorldPosition;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();
        }
    }
}
