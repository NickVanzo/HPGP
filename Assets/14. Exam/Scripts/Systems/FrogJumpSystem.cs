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
        foreach (var (physicsVelocity, jumpData) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<FrogJumpData>>())
        {
            string isGrounded = jumpData.ValueRO.isGrounded == true ? "true" : "false";
            Debug.Log(isGrounded);
        }
    }
}
