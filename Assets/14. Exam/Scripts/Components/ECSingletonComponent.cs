using Unity.Entities;

public struct ECSingletonComponent : IComponentData
{
    public int spawnAmount;
    public Entity prefabTospawn;
    public SchedulingType schedulingType;
}

public enum SchedulingType{
    Mainthread,
    Multithread,
}
