using Unity.Entities;
using UnityEngine;

class FloorAuthoring : MonoBehaviour
{
    class FloorAuthoringBaker : Baker<FloorAuthoring>
    {
        public override void Bake(FloorAuthoring authoring)
        {
            Entity triggerAuthoring = GetEntity(TransformUsageFlags.None);
            AddComponent(triggerAuthoring, new FloorTag());
        }
    }
}


