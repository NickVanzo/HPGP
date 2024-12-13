using Unity.Entities;
using UnityEngine;

class FrogJumpAuthoring : MonoBehaviour
{
    public float jumpForce;
    public float forwardForce;
    public bool isGrounded = false;
    public bool isTouchingCar = false;

    class FrogJumpAuthoringBaker : Baker<FrogJumpAuthoring>
    {
        public override void Bake(FrogJumpAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new FrogJumpData
            {
                jumpForce = authoring.jumpForce,
                forwardForce = authoring.forwardForce,
                isGrounded = authoring.isGrounded,
                isTouchingCar = authoring.isTouchingCar
            });
        }

    }

   
}

public struct FrogJumpData : IComponentData
{
    public float jumpForce;
    public float forwardForce;
    public bool isGrounded;
    public bool isTouchingCar;
}
