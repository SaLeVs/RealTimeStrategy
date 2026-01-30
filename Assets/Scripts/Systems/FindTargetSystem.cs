using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

partial struct FindTargetSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        
        NativeList<DistanceHit> distanceHits = new NativeList<DistanceHit>(Allocator.Temp);
        
        
        
        foreach ((RefRO<LocalTransform> localTransform, RefRO<FindTarget> findTarget) 
                 in SystemAPI.Query<RefRO<LocalTransform>, RefRO<FindTarget>>())
        {
            distanceHits.Clear();
            //collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range, ref distanceHits,   )
        }
        
    }
}
