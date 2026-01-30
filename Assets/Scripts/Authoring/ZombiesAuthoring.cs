using Unity.Entities;
using UnityEngine;

public class ZombiesAuthoring : MonoBehaviour
{
    
}
public class ZombiesAuthoringBaker : Baker<ZombiesAuthoring>
{
    public override void Bake(ZombiesAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new Zombies());
    }
}


public struct Zombies : IComponentData
{
    
}