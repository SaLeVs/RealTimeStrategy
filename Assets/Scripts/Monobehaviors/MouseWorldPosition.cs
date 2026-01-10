using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    private Camera _mainCamera;
    
    public static MouseWorldPosition Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }
    
    
    // This is a good method for plane terrains, if you have hills or 3d objects prefer physics raycast
    public Vector3 GetPosition()
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
