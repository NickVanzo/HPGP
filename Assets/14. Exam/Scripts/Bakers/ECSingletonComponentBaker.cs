using Unity.Entities;
using UnityEngine;

class ECSingletonAuthoring : MonoBehaviour
{
    [Range(1, 200)]
    public int spawnAmount;
    public GameObject prefabToSpawn;
    class baker : Baker<ECSingletonAuthoring>
    {
        public override void Bake(ECSingletonAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ECSingletonComponent
            {
                spawnAmount = authoring.spawnAmount,
                prefabTospawn = GetEntity(authoring.prefabToSpawn, TransformUsageFlags.Dynamic)
            });
        }
    }
}


