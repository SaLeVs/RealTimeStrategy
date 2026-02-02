using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct FindTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        
        NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.Temp);
        
        foreach ((RefRO<LocalTransform> localTransform, RefRW<FindTarget> findTarget, RefRW<Target> target) 
                 in SystemAPI.Query<RefRO<LocalTransform>, RefRW<FindTarget>, RefRW<Target>>())
        {
            findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if (findTarget.ValueRO.timer > 0f)
            {
                // Timer not elapsed
                continue;
            }
            findTarget.ValueRW.timer = findTarget.ValueRO.timerMax;
            
            distanceHits.Clear();
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u, // ~0u mean that we convert all 0 in binary to 1, so the ray belongs to all layers
                CollidesWith = 1u << GameAssets.UNITS_LAYER, // We put 1u << 6 because our units are in layer 6, and we need to shift 1 to the left 6 times to get the correct bitmask
                GroupIndex = 0
            };
            if (collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range,
                    ref distanceHits,
                    collisionFilter))
            {
                foreach (DistanceHit distanceHit in distanceHits)
                {
                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        // Valid target found
                        target.ValueRW.targetEntity = distanceHit.Entity;
                        break;
                    }
                }
            }
        }
        
    }
}
