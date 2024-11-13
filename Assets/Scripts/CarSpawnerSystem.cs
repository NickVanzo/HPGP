using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Queries for all CarSpawner components. Uses RefRW because this system wants
        // to read from and write to the component. If the system only needed read-only
        // access, it would use RefRO instead.
        foreach (RefRW<CarSpawner> Spawner in SystemAPI.Query<RefRW<CarSpawner>>())
        {
            ProcessSpawner(ref state, Spawner);
        }
    }

    private void ProcessSpawner(ref SystemState state, RefRW<CarSpawner> spawner)
    {
        // If the next spawn time has passed.
        if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            // Spawns a new entity and positions it at the spawner.
            Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
            // LocalPosition.FromPosition returns a Transform initialized with the given position.
            state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition[0] + spawner.ValueRO.CarNumber, spawner.ValueRO.SpawnPosition[1], spawner.ValueRO.SpawnPosition[2]));

            // Resets the next spawn time.
            spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;

            // increase the nr
            spawner.ValueRW.CarNumber = spawner.ValueRO.CarNumber + 1;
        }
    }
}