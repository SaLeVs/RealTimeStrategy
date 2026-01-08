using Unity.Entities;

public struct MoveSpeed : IComponentData
{
    // For create a dots component, we only need to define a struct that implements IComponentData
    // We don't have to add functions or methods on components, just data
    
    public float value; // public fields are ok for ECS components
    
    
}
