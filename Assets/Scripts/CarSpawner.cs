using Unity.Entities;
using Unity.Mathematics;

public struct CarSpawner : IComponentData
{
    public Entity Prefab;
    public float3 SpawnPosition;
    public float NextSpawnTime;
    public float SpawnRate;
    public float CarNumber;
}
