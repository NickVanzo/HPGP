using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
using Debug = UnityEngine.Debug;

partial struct CarMovementSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new CarMovementJob();
        job.Schedule();
    }

    [BurstCompile]
    partial struct CarMovementJob : IJobEntity
    {
        void Execute(ref PhysicsVelocity physicsVelocity, ref CarMovementData carData, LocalTransform localTransform)
        {
                float3 jumpDirection = math.normalize(new float3(0, 0, carData.forwardForce));
                physicsVelocity.Linear += jumpDirection;
            
        }
    }
}
