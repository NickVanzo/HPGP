using Unity.Entities;

public struct ECSingletonComponentCar : IComponentData
{
    public int spawnAmount;
    public Entity prefabTospawn;
    public SchedulingType schedulingType;
}
