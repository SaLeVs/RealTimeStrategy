using Unity.Entities;
using UnityEngine;



public struct UnitMover : IComponentData
{
    // For create a dots component, we only need to define a struct that implements IComponentData
    // We don't have to add functions or methods on components, just data
    public float value; // public fields are ok for ECS components
}

public class UnityMoverAuthoring : MonoBehaviour
{
    public float value;

    public class Baker : Baker<UnityMoverAuthoring>
    {
        public override void Bake(UnityMoverAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new UnitMover
            {
                value = authoring.value
            });
        }
    }
}



