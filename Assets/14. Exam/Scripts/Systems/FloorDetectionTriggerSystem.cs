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

            Debug.DrawRay(transform.ValueRO.Position, transform.ValueRO.Position - new float3(0.0f, 10.0f, 0.0f));

                var hit = physicsWorld.CastRay(
                    raycastInput,
                    out var rayResult
                );

            jumpData.ValueRW.isGrounded = hit && entityManager.HasComponent<FloorTag>(rayResult.Entity);

/*
                foreach (ColliderCastHit hit in hitsForCarCollision)
                {
                    if (entityManager.HasComponent<CarTag>(hit.Entity))
                    {
                        jumpData.ValueRW.isTouchingCar = true;
                        var soundManager = GameObject.FindAnyObjectByType<SoundManager>();
                        soundManager.PlayJumpSound();
                        break;
                    }
                    else
                    {
                        jumpData.ValueRW.isTouchingCar = false;
                    }
                }
*/
        }
    }
}
