
using UnityEngine;
using Unity.Entities;

namespace Frogs
{
    public class FrogJumpAuthoring : MonoBehaviour
    {
        public float jumpForce = 10.0f;
        public float forwardForce = 3.0f;
        public bool isGrounded = false;

        class FrogJumpBaker : Baker<FrogJumpAuthoring>
        {
            public override void Bake(FrogJumpAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new FrogJump
                {
                    jumpForce = authoring.jumpForce,
                    forwardForce = authoring.forwardForce,
                    isGrounded = true
                }); 
            }
        }
    }

    public struct FrogJump : IComponentData
    {
        public float jumpForce;
        public float forwardForce;
        public bool isGrounded;
    }
}


