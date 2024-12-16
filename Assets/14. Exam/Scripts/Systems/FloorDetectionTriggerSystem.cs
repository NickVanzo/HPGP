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
        var ecbSingleton = SystemAPI.GetSingleton<ECSingletonComponent>();
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        EntityManager entityManager = state.EntityManager;

        float3 carPosition = float3.zero;
        foreach (var (carTag, transform) in SystemAPI.Query<RefRO<CarTag>, RefRW<LocalTransform>>())
        {
            carPosition = transform.ValueRO.Position;
        }

        if (ecbSingleton.schedulingType == SchedulingType.Multithread)
        {
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            var floorTagLookup = state.GetComponentLookup<FloorTag>(isReadOnly: true);

            state.Dependency = new FrogDetectionJob
            {
                collisionWorld = physicsWorld.CollisionWorld,
                floorTagLookup = floorTagLookup,
                carPosition = carPosition
            }.ScheduleParallel(state.Dependency);
        } else
        {
            Entity carEntity = Entity.Null;
            foreach (var (carTag, transform, entity) in SystemAPI.Query<RefRO<CarTag>, RefRW<LocalTransform>>().WithEntityAccess())
            {
                carEntity = entity;
            }

            var carTransform = entityManager.GetComponentData<LocalTransform>(carEntity);

            foreach (var (triggerComponent, jumpData, transform) in SystemAPI
                         .Query<RefRO<FloorDetectionTriggerComponent>, RefRW<FrogJumpData>, RefRW<LocalTransform>>())
            {

                var raycastInput = new RaycastInput
                {
                    Start = transform.ValueRO.Position,
                    End = transform.ValueRO.Position - new float3(0.0f, 10.0f, 0.0f),
                    Filter = CollisionFilter.Default
                };

                var hit = physicsWorldSingleton.CastRay(
                        raycastInput,
                        out var rayResult
                    );

                jumpData.ValueRW.isGrounded = hit && entityManager.HasComponent<FloorTag>(rayResult.Entity);


                float3 entityPosition = transform.ValueRO.Position;

                float distance = math.distance(entityPosition, carPosition);

                if (distance < 2.0f)
                {
                    jumpData.ValueRW.isTouchingCar = true;
                    //var soundManager = GameObject.FindAnyObjectByType<SoundManager>();
                    //soundManager.PlayJumpSound();
                }
                else
                {
                    jumpData.ValueRW.isTouchingCar = false;
                }
            }
        }
         
    }

    [BurstCompile]
    partial struct FrogDetectionJob : IJobEntity
    {
        [ReadOnly] public ComponentLookup<FloorTag> floorTagLookup;
        [ReadOnly] public CollisionWorld collisionWorld;
        public float3 carPosition;


        void Execute(
           in FloorDetectionTriggerComponent triggerComponent,
           ref FrogJumpData jumpData,
           ref LocalTransform transform
           )
        {
            var raycastInput = new RaycastInput
            {
                Start = transform.Position,
                End = transform.Position - new float3(0.0f, 10.0f, 0.0f),
                Filter = CollisionFilter.Default
            };
            var hit = collisionWorld.CastRay(raycastInput, out var rayResult);
            jumpData.isGrounded = hit && floorTagLookup.HasComponent(rayResult.Entity);

            float3 entityPosition = transform.Position;
            float distance = math.distance(entityPosition, carPosition);

            jumpData.isTouchingCar = distance < 2.0f;
        }
    }

}
