using Frogs;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

partial struct FrogJumpSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float gravity = 9.81f;
        float jumpAngle = math.radians(45f);
        
        foreach (var (physicsVelocity, jumpData) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<FrogJump>>())
        {
            if (jumpData.ValueRW.isGrounded)
            {
                // Calculate jump speed based on force and gravity
                float jumpSpeed = math.sqrt(2 * gravity * jumpData.ValueRO.jumpForce / math.sin(2 * jumpAngle));

                // Determine the velocity in the jump direction
                float3 jumpDirection = new float3(math.cos(jumpAngle), math.sin(jumpAngle), 0);
                float3 jumpVelocity = jumpDirection * jumpSpeed;

                // Apply the jump velocity to the entity's physics velocity
                physicsVelocity.ValueRW.Linear += jumpVelocity;

                // Mark as airborne after jump
                jumpData.ValueRW.isGrounded = false;
            }
        }
    }
}
