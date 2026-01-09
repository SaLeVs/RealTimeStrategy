using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

// System as a suffix is a convention to indicate that this struct is a system
// Partial keyword allows the system to be split across multiple files if needed
// The requirements are partial, struct and implement ISystem

partial struct UnitMoverSystem : ISystem
{
    // We can delete OnCreate and OnDestroy if we don't need them

    [BurstCompile] // Burst compile attribute is for improving performance
    public void OnUpdate(ref SystemState state)
    {
        // RefRW is a class that allows us to convert a value type to a reference type
        
        foreach (RefRW<LocalTransform> localTransform in SystemAPI.Query<RefRW<LocalTransform>>())
        {
            
        }
    }
    
}
