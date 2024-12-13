using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
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
            string isGrounded = jumpData.isGrounded == true ? "true" : "false";
            if (jumpData.isGrounded)
            {
                float3 jumpDirection = math.normalize(new float3(0, jumpData.jumpForce, UnityEngine.Random.Range(-jumpData.forwardForce, jumpData.forwardForce)));
                physicsVelocity.Linear += jumpDirection;
            }
            if(jumpData.isTouchingCar)
            {
                Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)(456)); 
                float3 randomForce = new float3(
                    random.NextFloat(-5f, 5f), 
                    random.NextFloat(5f, 15f), 
                    random.NextFloat(-5f, 5f)  
                );

                physicsVelocity.Linear += randomForce;
                jumpData.isTouchingCar = false;
            }
        }
    }
}

/*
 
 */
