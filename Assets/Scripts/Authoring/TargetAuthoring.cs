using Unity.Entities;
using UnityEngine;

public class TargetAuthoring : MonoBehaviour
{
    public GameObject target;
}

public class TargetAuthoringBaker : Baker<TargetAuthoring>
{
    public override void Bake(TargetAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new Target
        {
            targetEntity = GetEntity(authoring.target, TransformUsageFlags.Dynamic)
        });
    }
}

public struct Target : IComponentData
{
       public Entity targetEntity;
}