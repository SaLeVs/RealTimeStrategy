using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Debug.Log(GetPosition());
    }
    
    private Vector3 GetPosition()
    {
        Ray mouseCameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        
        if (plane.Raycast(mouseCameraRay, out float distance))
        {
            return mouseCameraRay.GetPoint(distance);
        }
        else
        {
            return Vector3.zero;
        }
    }
    
}
