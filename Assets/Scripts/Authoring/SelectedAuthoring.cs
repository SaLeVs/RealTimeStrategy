using Unity.Entities;
using UnityEngine;

// In dots, we avoid structural changes as much as possible (Expensive operations)
// For avoid to remove and add components frequently, we can use IEnableableComponent interface
// This interface allow us to enable or disable a component without remove it from the entity
public struct Selected : IComponentData, IEnableableComponent
{
       public Entity selectedVisualEntity;
       public float showVisualScale;
       public bool onSelected;
       public bool onDeselected;
}

public class SelectedAuthoring : MonoBehaviour
{
    public GameObject selectedVisualGameObject;
    public float showVisualScale;
    
    public class SelectedAuthoringBaker : Baker<SelectedAuthoring>
    {
        public override void Bake(SelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new Selected
            {
                selectedVisualEntity = GetEntity(authoring.selectedVisualGameObject, TransformUsageFlags.Dynamic),
                showVisualScale = authoring.showVisualScale
            });
            
            SetComponentEnabled<Selected>(entity, false);
        }
    }
}



