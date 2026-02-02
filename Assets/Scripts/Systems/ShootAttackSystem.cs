using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct ShootAttackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<ShootAttack> shootAttack, RefRO<Target> target) in 
                 SystemAPI.Query<RefRW<ShootAttack>, RefRO<Target>>())
        {
            if (target.ValueRO.targetEntity == Entity.Null) // If Entity.null is true, we don't have a target
            {
                continue;
            }
            
            shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            
            if (shootAttack.ValueRO.timer > 0f)
            {
                continue;
            }
            
            shootAttack.ValueRW.timer = shootAttack.ValueRO.timerMax;
            Debug.Log("Shoot");
        }

        
    }
}
