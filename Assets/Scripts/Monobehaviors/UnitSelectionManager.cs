using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SocialPlatforms;

// Managers is a common naming convention for MonoBehaviour classes and Systems that handle DOTS logic
public class UnitSelectionManager : MonoBehaviour
{
    public event Action OnSelectionStart;
    public event Action OnSelectionEnd;
    
    private Vector3 _mouseWorldPosition;
    private EntityManager _entityManager;
    private EntityQuery _entityQuery;
    
    private NativeArray<LocalTransform> _localTransformArray;
    private NativeArray<UnitMover> _unitMoverArray;
    private NativeArray<Entity> _entityArray;
    
    private Vector2 _startMousePosition;
    private Vector2 _endMousePosition;
    
    private Camera _mainCamera;
    private float _minSelectionAreaSize;
    private bool _isMultipleSelection;


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = Input.mousePosition;
            OnSelectionStart?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endMousePosition = Input.mousePosition;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager; 
            
            _entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(_entityManager); 
            _entityArray = _entityQuery.ToEntityArray(Allocator.Temp);
            
            for(int i = 0; i < _entityArray.Length; i++)
            {
                _entityManager.SetComponentEnabled<Selected>(_entityArray[i], false);
            }
            
            Rect selectionAreaRect = GetSelectionAreaRect();
            float selectionAreaSize = selectionAreaRect.width + selectionAreaRect.height;
            _minSelectionAreaSize = 40f;
            
            _isMultipleSelection = selectionAreaSize > _minSelectionAreaSize;
            if (_isMultipleSelection)
            {
                _entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>().WithPresent<Selected>().Build(_entityManager); 
            
                _entityArray = _entityQuery.ToEntityArray(Allocator.Temp);
                _localTransformArray = _entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
                
                for(int i = 0; i < _localTransformArray.Length; i++)
                {
                    LocalTransform localTransform = _localTransformArray[i];
                    Vector2 unitScreenPosition = _mainCamera.WorldToScreenPoint(localTransform.Position);

                    if (selectionAreaRect.Contains(unitScreenPosition))
                    {
                        _entityManager.SetComponentEnabled<Selected>(_entityArray[i], true);
                    }
                }
            }
            else
            {
                // Single selection
                _entityQuery = _entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = _entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
                
                UnityEngine.Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
                
                RaycastInput rayCastInput = new RaycastInput
                {
                    Start = cameraRay.GetPoint(0f), // 0f is the near plane
                    End = cameraRay.GetPoint(10000f), // 10000f is a far distance
                    Filter = new CollisionFilter
                    {
                        BelongsTo = 
                    }
                };
                collisionWorld.CastRay()

            }
            
            
            OnSelectionEnd?.Invoke();
            
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

    public Rect GetSelectionAreaRect()
    {
        Vector2 selectionEndMousePosition = Input.mousePosition;
        
        Vector2 lowerLeftCorner = new Vector2(
            Mathf.Min(_startMousePosition.x, selectionEndMousePosition.x), 
            Mathf.Min(_startMousePosition.y, selectionEndMousePosition.y));
        
        Vector2 upperRightCorner = new Vector2(
            Mathf.Max(_startMousePosition.x, selectionEndMousePosition.x),
            Mathf.Max(_startMousePosition.y, selectionEndMousePosition.y));
        
        return new  Rect(
            lowerLeftCorner.x, 
            lowerLeftCorner.y, 
            upperRightCorner.x - lowerLeftCorner.x,
            upperRightCorner.y - lowerLeftCorner.y);
    }
}
