using Unity.Entities;
using UnityEngine;



public struct MoveSpeed : IComponentData
{
    // For create a dots component, we only need to define a struct that implements IComponentData
    // We don't have to add functions or methods on components, just data
    
    public float value; // public fields are ok for ECS components
    
    
}

public class MoveSpeedAuthoring : MonoBehaviour
{
    public float value;

    public class Baker : Baker<MoveSpeedAuthoring>
    {
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new MoveSpeed
            {
                value = authoring.value
            });
        }
    }
}



