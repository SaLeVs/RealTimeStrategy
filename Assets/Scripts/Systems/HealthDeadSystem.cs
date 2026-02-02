using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthDeadSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach ((RefRO<Health> health, Entity entity)
            in SystemAPI.Query<RefRO<Health>>().WithEntityAccess()) // We can add .WithEntityAccess() to get the entity along with the component data
        {
            if (health.ValueRO.healthAmount <= 0)
            {
                entityCommandBuffer.DestroyEntity(entity);
                // state.EntityManager.DestroyEntity(entity);  We cant destroy entities inside a foreach loop using EntityManager, this make a structural change
            }
        }
    }
}
