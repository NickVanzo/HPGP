using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

partial struct FrogJumpSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new FrogJumpJob();
        job.Schedule();

    }

    [BurstCompile]
    partial struct FrogJumpJob : IJobEntity
    {
        // In source generation, a query is created from the parameters of Execute().
        // Here, the query will match all entities having a LocalTransform, PostTransformMatrix, and RotationSpeed component.
        // (In the scene, the root cube has a non-uniform scale, so it is given a PostTransformMatrix component in baking.)
        void Execute(ref PhysicsVelocity physicsVelocity, ref FrogJumpData jumpData, LocalTransform localTransform)
        {
            if (jumpData.isGrounded || jumpData.isTouchingCar)
            {
                float3 randomForce = new float3(
                   UnityEngine.Random.Range(-1f * jumpData.forwardForce, jumpData.forwardForce),
                   UnityEngine.Random.Range(0, jumpData.jumpForce),
                   UnityEngine.Random.Range(-1f * jumpData.forwardForce, jumpData.forwardForce)
               );

                physicsVelocity.Linear += randomForce;
                jumpData.isTouchingCar = false;
            }
        }
    }
}

/*
 
 */
