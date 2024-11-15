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
}
