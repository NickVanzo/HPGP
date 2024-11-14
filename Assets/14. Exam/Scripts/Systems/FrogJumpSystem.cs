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
        foreach (var (physicsVelocity, jumpData, localTransform) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<FrogJumpData>, RefRO<LocalTransform>>())
        {
            string isGrounded = jumpData.ValueRO.isGrounded == true ? "true" : "false";
            if(jumpData.ValueRO.isGrounded)
            {
                float3 jumpDirection = math.normalize(new float3(0, jumpData.ValueRO.jumpForce, jumpData.ValueRO.forwardForce));
                physicsVelocity.ValueRW.Linear += jumpDirection;
            }
        }
    }
}
