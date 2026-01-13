using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;



public struct UnitMover : IComponentData
{
    // For create a dots component, we only need to define a struct that implements IComponentData
    // We don't have to add functions or methods on components, just data
    public float moveSpeed; // public fields are ok for ECS components
    public float rotationSpeed;
    public float3 targetPosition;
}

public class UnityMoverAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public class Baker : Baker<UnityMoverAuthoring>
    {
        public override void Bake(UnityMoverAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new UnitMover
            {
                moveSpeed = authoring.moveSpeed,
                rotationSpeed = authoring.moveSpeed
            });
        }
    }
}



