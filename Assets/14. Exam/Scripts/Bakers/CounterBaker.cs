using Unity.Entities;
using UnityEngine;

class CounterBaker : MonoBehaviour
{
    public int value = 0;
}

class CounterBakerBaker : Baker<CounterBaker>
{
    public override void Bake(CounterBaker authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new CounterData
        {
            value = authoring.value
        });

        AddComponent(entity, new CounterTag());
    }
}
