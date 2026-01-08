using Unity.Entities;
using UnityEngine;

public class MoveSpeedAuthoring : MonoBehaviour
{
    public float value;

    public class Baker : Baker<MoveSpeedAuthoring>
    {
        
        public override void Bake(MoveSpeedAuthoring authoring)
        {
            
        }
        
    }
    
}

