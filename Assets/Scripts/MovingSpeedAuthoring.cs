using UnityEngine;
using Unity.Entities;

namespace Frogs
{
    public class MovingSpeedAuthoring : MonoBehaviour
    {
        public float Speed = 2.0f;

        class MovingSpeedBaker : Baker<MovingSpeedAuthoring>
        {
            public override void Bake(MovingSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MovingSpeed
                {
                    Speed = authoring.Speed
                });
            }
        }
    }

    public struct MovingSpeed : IComponentData
    {
        public float Speed;
    }

}
