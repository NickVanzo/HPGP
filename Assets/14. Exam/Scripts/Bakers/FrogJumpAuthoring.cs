using Unity.Entities;
using UnityEngine;

class FrogJumpAuthoring : MonoBehaviour
{
    public float jumpForce = 10.0f;
    public float forwardForce = 3.0f;
    public bool isGrounded = false;

    class FrogJumpAuthoringBaker : Baker<FrogJumpAuthoring>
    {
        public override void Bake(FrogJumpAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new FrogJumpData
            {
                jumpForce = authoring.jumpForce,
                forwardForce = authoring.forwardForce,
                isGrounded = authoring.isGrounded
            });
        }
    }

   
}

public struct FrogJumpData : IComponentData
{
    public float jumpForce;
    public float forwardForce;
    public bool isGrounded;
}
