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
        // In source generation, a query is created from the parameters of Execute().
        // Here, the query will match all entities having a LocalTransform, PostTransformMatrix, and RotationSpeed component.
        // (In the scene, the root cube has a non-uniform scale, so it is given a PostTransformMatrix component in baking.)
        void Execute(ref PhysicsVelocity physicsVelocity, ref CarMovementData carData, LocalTransform localTransform)
        {
                float3 jumpDirection = math.normalize(new float3(0, 0, carData.forwardForce));
                physicsVelocity.Linear += jumpDirection;
            
        }
    }
}
