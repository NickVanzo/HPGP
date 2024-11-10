using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Frogs.MainThread
{
    public partial struct MovingFrogsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach(var (transform, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovingSpeed>>())
            {
                float3 direction = new float3(0.0f, 0.0f, deltaTime * speed.ValueRO.Speed);
                transform.ValueRW = transform.ValueRO.Translate(direction);
            } 
        }
    }
}
