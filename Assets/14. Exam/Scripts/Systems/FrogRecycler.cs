using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct FrogRecycler : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ECSingletonComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var (triggerComponent, jumpData, transform, entity) in SystemAPI
                    .Query< RefRO<FloorDetectionTriggerComponent>, RefRW<FrogJumpData>, RefRW<LocalTransform>>()
                    .WithEntityAccess()
                    )
        {
            if(transform.ValueRO.Position.y < -2.0f)
                ecb.DestroyEntity(entity);  
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

