using UnityEngine;
using Unity.Entities;

class CarSpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float SpawnRate;
    public float CarNumber;
}

class CarSpawnerBaker : Baker<CarSpawnerAuthoring>
{

    public override void Bake(CarSpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new CarSpawner
        {
            // By default, each authoring GameObject turns into an Entity.
            // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.transform.position,
            NextSpawnTime = 0.0f,
            SpawnRate = authoring.SpawnRate,
            CarNumber = authoring.CarNumber
        });
    }
}
