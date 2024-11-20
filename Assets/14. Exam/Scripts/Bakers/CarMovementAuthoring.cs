using Unity.Entities;
using UnityEngine;

class CarMovementAuthoring : MonoBehaviour
{
    public float forwardForce = 3.0f;
    public bool isGrounded = false;

    class CarMovementAuthoringBaker : Baker<CarMovementAuthoring>
    {
        public override void Bake(CarMovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new CarMovementData
            {
                forwardForce = authoring.forwardForce,
                isGrounded = authoring.isGrounded
            });
        }
    }

   
}

public struct CarMovementData : IComponentData
{
    public float forwardForce;
    public bool isGrounded;
}
