using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
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
        // RW stands for Read/Write
        // RO stands for Read Only
        // It's good to choice the RO when we don't need to write to the component for better performance
        
        foreach (RefRW<LocalTransform> localTransform in SystemAPI.Query<RefRW<LocalTransform>>())
        {
            // Look that we use RW for modify and RO for read only
            // DOTS have a special delta time that is different from UnityEngine.Time
            
            localTransform.ValueRW.Position = localTransform.ValueRO.Position + new float3(1, 0, 0) * SystemAPI.Time.DeltaTime;
        }
    }
    
}
