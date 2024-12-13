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
            Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)UnityEngine.Time.frameCount);

            for (int i = 0; i < n; i++)
            {
                var e = state.EntityManager.Instantiate(ecbSingleton.prefabTospawn);

                // Generate random coordinates
                float x = random.NextFloat(-100f, 100f); // Adjust the range for your map size
                float z = random.NextFloat(-100f, 100f); // Adjust the range for your map size
                float y = 0.1f; // Almost ground level

                state.EntityManager.SetComponentData(e, LocalTransform.FromPosition(new float3(x, y, z)));
            }
        }
        else
        {
            var ECB = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            state.Dependency = new spawnCubesParallel
            {
                ecb = ECB,
                randomSeed = (uint)UnityEngine.Time.frameCount
            }.ScheduleParallel(state.Dependency);
        }
    }
}



[BurstCompile]
public partial struct spawnCubesParallel : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public uint randomSeed;
    public void Execute([ChunkIndexInQuery] int key, in ECSingletonComponent ecbSingleton)
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random(randomSeed + (uint)key);
        int n = ecbSingleton.spawnAmount;

        for (int i = 0; i < n; i++)
        {
            var e = ecb.Instantiate(key, ecbSingleton.prefabTospawn);

            // Generate random coordinates
            float x = random.NextFloat(-100f, 100f); // Adjust the range for your map size
            float z = random.NextFloat(-100f, 100f); // Adjust the range for your map size
            float y = 0.1f; // Almost ground level

            ecb.AddComponent(key, e, LocalTransform.FromPosition(new float3(x, y, z)));
        }
    }
}
