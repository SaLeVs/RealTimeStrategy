using Unity.Collections;
using Unity.Entities;
using UnityEngine;

// Managers is a common naming convention for MonoBehaviour classes and Systems that handle DOTS logic
public class UnitSelectionManager : MonoBehaviour
{
    private Vector3 _mouseWorldPosition;
    private EntityManager _entityManager;
    private EntityQuery _entityQuery;
    private NativeArray<UnitMover> _unitMovers;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _mouseWorldPosition = MouseWorldPosition.Instance.GetPosition(); // This get an error because it's a mono-behavior, so will work, but not whit best performance in ECS
            
            // SystemAPI is not accessible from MonoBehaviour, only from Systems
            // So we need to create an EntityQuery manually
            // Pay attention for use allocator because memory leaks
            // Temp is shorter than other allocators, so we don't need to dispose it

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; // World.DefaultGameObjectInjectionWorld is the default world where all systems and entities are created
            _entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover>().Build(_entityManager); 
            
            _unitMovers = _entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            
            for(int i = 0; i < _unitMovers.Length; i++)
            {
                UnitMover unitMover = _unitMovers[i];
                unitMover.targetPosition = _mouseWorldPosition;
            }
        }
    }
}
