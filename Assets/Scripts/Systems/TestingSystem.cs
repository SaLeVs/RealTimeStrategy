using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct TestingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        int unitCount = 0;
        
        foreach (RefRW<Zombies> zombies in SystemAPI.Query<RefRW<Zombies>>()) 
        {
            unitCount++;
        } 
      
        Debug.Log($"UnitCount: {unitCount}"); 
       
       
    }

    
}
