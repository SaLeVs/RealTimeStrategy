using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

// System as a suffix is a convention to indicate that this struct is a system
// Partial keyword allows the system to be split across multiple files if needed
// The requirements are partial, struct and implement ISystem

partial struct UnitMoverSystem : ISystem
{
    // We can delete OnCreate and OnDestroy if we don't need them

    [BurstCompile] // Burst compile attribute is for improving performance
    public void OnUpdate(ref SystemState state)
    {
        UnitMoverJob unitMoverJob = new UnitMoverJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        unitMoverJob.ScheduleParallel();

        /*
        // RefRW is a class that allows us to convert a value type to a reference type
        // RW stands for Read/Write
        // RO stands for Read Only
        // It's good to choice the RO when we don't need to write to the component for better performance

        foreach ((
                     RefRW<LocalTransform> localTransform,
                     RefRO<UnitMover> unitMover,
                     RefRW<PhysicsVelocity> physicsVelocity)
                        in SystemAPI.Query<
                           RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        {
            // Look that we use RW for modify and RO for read only

        } */

    }
    
}


public partial struct UnitMoverJob : IJobEntity
{
    // In DOTS documentation, the recommended way to pass data to jobs is using in and ref keywords
    // in is for read-only data
    // ref is for read and write data
    
    public float deltaTime;
    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {
        // DOTS have a special delta time that is different from UnityEngine.Time
        // Float3 means a vector with 3 float values (x, y, z)
            
        float3 moveDirection = unitMover.targetPosition - localTransform.Position;
            
        moveDirection = math.normalize(moveDirection);
            
        localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(moveDirection, math.up()), deltaTime * unitMover.rotationSpeed); // Math.up() is (0, 1, 0)

        physicsVelocity.Linear = moveDirection * unitMover.moveSpeed;
        physicsVelocity.Angular = float3.zero; // We add this to avoid unwanted rotation
        // localTransform.ValueRW.Position += moveDirection * moveSpeed.ValueRO.value * SystemAPI.Time.DeltaTime;
    }
}
