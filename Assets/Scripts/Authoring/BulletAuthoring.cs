using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float speed;
    public int damageAmount;
}

public class BulletAuthoringBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new BulletData
        {
            speed = authoring.speed,
            damageAmount = authoring.damageAmount
        });
    }
}

public struct BulletData : IComponentData
{
    public float speed;
    public int damageAmount;
}