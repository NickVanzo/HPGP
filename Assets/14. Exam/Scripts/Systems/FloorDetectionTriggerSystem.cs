using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

partial struct FloorDetectionTriggerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        EntityManager entityManager = state.EntityManager;

        Entity carEntity = Entity.Null;
        foreach (var (carTag, transform, entity) in SystemAPI.Query<RefRO<CarTag>, RefRW<LocalTransform>>().WithEntityAccess())
        {
               carEntity = entity;
        }

        var carTransform = entityManager.GetComponentData<LocalTransform>(carEntity);
        float3 carPosition = carTransform.Position;

        foreach (var (triggerComponent, jumpData, transform) in SystemAPI
                     .Query<RefRO<FloorDetectionTriggerComponent>, RefRW<FrogJumpData>, RefRW<LocalTransform>>())
        {
            PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            var raycastInput = new RaycastInput
            {
                Start = transform.ValueRO.Position,
                End = transform.ValueRO.Position - new float3(0.0f, 10.0f, 0.0f), 
                Filter = CollisionFilter.Default
            };

            Debug.DrawRay(transform.ValueRO.Position, new float3(0.0f, -10.0f, 0.0f), Color.red);

            var hit = physicsWorld.CastRay(
                    raycastInput,
                    out var rayResult
                );

            jumpData.ValueRW.isGrounded = hit && entityManager.HasComponent<FloorTag>(rayResult.Entity);


            float3 entityPosition = transform.ValueRO.Position;

            float distance = math.distance(entityPosition, carPosition);

            if(distance < 2.0f)
            {
                jumpData.ValueRW.isTouchingCar = true;
                var soundManager = GameObject.FindAnyObjectByType<SoundManager>();
                soundManager.PlayJumpSound();
            } else
            {
                jumpData.ValueRW.isTouchingCar = false;
            }
        }
    }
}
