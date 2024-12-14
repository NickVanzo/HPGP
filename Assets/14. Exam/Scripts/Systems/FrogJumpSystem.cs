using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
using Debug = UnityEngine.Debug;

partial struct FrogJumpSystem : ISystem
{
    // Random instance (stored as part of the system's unmanaged state)
    private Random classRandom;

    public void OnCreate(ref SystemState state)
    {
        // Initialize Random with a non-zero seed
        classRandom = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Create a local copy of the random instance for thread safety
        Random jobRandom = classRandom;

        // Schedule the job
        var job = new FrogJumpJob
        {
            random = jobRandom
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);

        // Update the random instance's state for the next frame
        classRandom = job.random;
        job.Schedule();

    }

    [BurstCompile]
    partial struct FrogJumpJob : IJobEntity
    {
        // In source generation, a query is created from the parameters of Execute().
        // Here, the query will match all entities having a LocalTransform, PostTransformMatrix, and RotationSpeed component.
        // (In the scene, the root cube has a non-uniform scale, so it is given a PostTransformMatrix component in baking.)
        public Random random;

        void Execute(ref PhysicsVelocity physicsVelocity, ref FrogJumpData jumpData, in LocalTransform localTransform)
        {
            if (jumpData.isGrounded)
            {
                // Generate random direction for jump
                float3 jumpDirection = math.normalize(new float3(
                    random.NextFloat(-1f * jumpData.forwardForce, jumpData.forwardForce),
                    jumpData.jumpForce,
                    random.NextFloat(-1f * jumpData.forwardForce, jumpData.forwardForce)
                ));

                physicsVelocity.Linear += jumpDirection;
            }

            if (jumpData.isTouchingCar)
            {
                // Generate random force when touching a car
                float3 randomForce = new float3(
                    random.NextFloat(-1f * jumpData.forwardForce, jumpData.forwardForce),
                    random.NextFloat(0, jumpData.jumpForce),
                    random.NextFloat(-1f * jumpData.forwardForce, jumpData.forwardForce)
                );

                physicsVelocity.Linear += randomForce;
                jumpData.isTouchingCar = false; // Reset flag
            }
        }
    }
}

