using Unity.Entities;
using UnityEngine;

class FloorTriggerAuthoring : MonoBehaviour
{
    public float size = 1.0f;
    class FloorTriggerAuthoringBaker : Baker<FloorTriggerAuthoring>
    {
        public override void Bake(FloorTriggerAuthoring authoring)
        {
            Entity triggerAuthoring = GetEntity(TransformUsageFlags.None);
            AddComponent(triggerAuthoring, new FloorDetectionTriggerComponent
            {
                size = authoring.size
            });
        }
    }
}

