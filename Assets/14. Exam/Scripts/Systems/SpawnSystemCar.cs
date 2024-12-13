using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct SpawnSystemCar : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ECSingletonComponentCar>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var ecbSingleton = SystemAPI.GetSingleton<ECSingletonComponentCar>();

        if (ecbSingleton.schedulingType == SchedulingType.Mainthread)
        {
            int n = ecbSingleton.spawnAmount;
            for (int i = 0; i < n * n; i++)
            {
                var e = state.EntityManager.Instantiate(ecbSingleton.prefabTospawn);
                float x = 2f;
                float y = 2f;
                float z = -500f;

                state.EntityManager.SetComponentData(e, LocalTransform.FromPositionRotationScale(
                    new float3(x, y, z),
                    quaternion.identity,
                    1f
                    ));
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

