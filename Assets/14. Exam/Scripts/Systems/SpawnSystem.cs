using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct SpawnSystem : ISystem
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
        var ecbSingleton = SystemAPI.GetSingleton<ECSingletonComponent>();

        if (ecbSingleton.schedulingType == SchedulingType.Mainthread)
        {
            int n = ecbSingleton.spawnAmount;
            for (int i = 0; i < n * n; i++)
            {
                var e = state.EntityManager.Instantiate(ecbSingleton.prefabTospawn);
                float x = (i % n) * 2f;
                float y = 2f;
                float z = (i / (n * n)) * 2f;

                state.EntityManager.SetComponentData(e, LocalTransform.FromPosition(new float3(x, y, z)));
            };
        }
        else
        {
            var ECB = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            state.Dependency = new spawnCubesParallel
            {
                ecb = ECB
            }.ScheduleParallel(state.Dependency);
        }
    }
}



[BurstCompile]
public partial struct spawnCubesParallel : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public void Execute([ChunkIndexInQuery] int key, in ECSingletonComponent ecbSingleton)
    {
        int n = ecbSingleton.spawnAmount;
        for (int i = 0; i < n*n ; i++)
        {
            var e = ecb.Instantiate(key, ecbSingleton.prefabTospawn);
            float x = (i % n) * 2f;
            float y = (i / n) * 2f;
            float z = (i / (n * n)) * 2f;

            ecb.AddComponent(key,e, LocalTransform.FromPosition(new float3(x, y, z)));
        }
    }
}
