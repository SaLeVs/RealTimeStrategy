using Unity.Entities;
using UnityEngine;

public class FindTargetAuthoring : MonoBehaviour
{
    public float range;
}


public class FindTargetAuthoringBaker : Baker<FindTargetAuthoring>
{
    
    public override void Bake(FindTargetAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new FindTarget
        {
            range = authoring.range
        });
    }
    
}


public struct FindTarget : IComponentData
{
    public float range;
}