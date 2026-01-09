using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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
        
        foreach ((
                     RefRW<LocalTransform> localTransform, 
                     RefRO<MoveSpeed> moveSpeed,
                     RefRW<PhysicsVelocity> physicsVelocity) 
                        in SystemAPI.Query<
                           RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>())
        {
            // Look that we use RW for modify and RO for read only
            // DOTS have a special delta time that is different from UnityEngine.Time
            // Float3 means a vector with 3 float values (x, y, z)
            
            float3 targetPosition = localTransform.ValueRO.Position + new float3(10f, 0f, 0f);
            float3 moveDirection = targetPosition - localTransform.ValueRO.Position;
            
            moveDirection = math.normalize(moveDirection);
            
            localTransform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up()); // Math.up() is (0, 1, 0)

            physicsVelocity.ValueRW.Linear = moveDirection * moveSpeed.ValueRO.value;
            physicsVelocity.ValueRW.Angular = float3.zero; // We add this to avoid unwanted rotation
            // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
        }
    }
    
}
