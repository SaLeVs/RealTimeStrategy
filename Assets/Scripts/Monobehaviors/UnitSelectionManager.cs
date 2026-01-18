using Unity.Collections;
using Unity.Entities;
using UnityEngine;

// Managers is a common naming convention for MonoBehaviour classes and Systems that handle DOTS logic
public class UnitSelectionManager : MonoBehaviour
{
    private Vector3 _mouseWorldPosition;
    private EntityManager _entityManager;
    private EntityQuery _entityQuery;
    
    private NativeArray<UnitMover> _unitMoverArray;
    private NativeArray<Entity> _entityArray;
    
    private Vector2 _startMousePosition;
    private Vector2 _endMousePosition;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = Input.mousePosition;
            Debug.Log($"Start mouse position: {_startMousePosition}");
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endMousePosition = Input.mousePosition;
            Debug.Log($"End mouse position: {_endMousePosition}");
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            _mouseWorldPosition = MouseWorldPosition.Instance.GetPosition(); // This get an error because it's a mono-behavior, so will work, but not whit best performance in ECS
            
            // SystemAPI is not accessible from MonoBehaviour, only from Systems
            // So we need to create an EntityQuery manually
            // Pay attention for use allocator because memory leaks
            // Temp is shorter than other allocators, so we don't need to dispose it

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // World.DefaultGameObjectInjectionWorld is the default world where all systems and entities are created
            _entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(_entityManager); 
            
            _unitMoverArray = _entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            _entityArray = _entityQuery.ToEntityArray(Allocator.Temp);
            
            for(int i = 0; i < _unitMoverArray.Length; i++)
            {
                UnitMover unitMover = _unitMoverArray[i];
                unitMover.targetPosition = _mouseWorldPosition;
                _unitMoverArray[i] = unitMover;
            }
            
            _entityQuery.CopyFromComponentDataArray(_unitMoverArray);
        }
    }
}
