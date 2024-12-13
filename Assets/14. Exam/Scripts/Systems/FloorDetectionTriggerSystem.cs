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
            NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
            NativeList<ColliderCastHit> hitsForCarCollision = new NativeList<ColliderCastHit>(Allocator.Temp);

            physicsWorld.SphereCastAll(
                transform.ValueRO.Position,
                triggerComponent.ValueRO.size / 2,
                new float3(0, -1, 0),
                1,
                ref hits,
                CollisionFilter.Default
            );

            physicsWorld.SphereCastAll(
               transform.ValueRO.Position,
               3,
               new float3(0, -1, 0),
               1,
               ref hitsForCarCollision,
               CollisionFilter.Default
           );
           
            foreach (ColliderCastHit hit in hits)
            {
                if (entityManager.HasComponent<FloorTag>(hit.Entity))
                {
                    jumpData.ValueRW.isGrounded = true;
                    break;
                }
                else
                {
                    jumpData.ValueRW.isGrounded = false;
                }
            }

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
        }
    }
}
