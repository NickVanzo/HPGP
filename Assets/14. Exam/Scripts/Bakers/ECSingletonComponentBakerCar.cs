using Unity.Entities;
using UnityEngine;

class ECSingletonAuthoringCar : MonoBehaviour
{
    [Range(1, 200)]
    public int spawnAmount;
    public GameObject prefabToSpawn;
    public SchedulingType schedulingType;

    class baker : Baker<ECSingletonAuthoringCar>
    {
        public override void Bake(ECSingletonAuthoringCar authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ECSingletonComponentCar
            {
                spawnAmount = authoring.spawnAmount,
                prefabTospawn = GetEntity(authoring.prefabToSpawn, TransformUsageFlags.Dynamic),
                schedulingType = authoring.schedulingType
            });
        }
    }
}


