using System.Diagnostics;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;
partial struct FrogJumpSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (physicsVelocity, jumpData) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<FrogJumpData>>())
        {
            
        }
    }
}
