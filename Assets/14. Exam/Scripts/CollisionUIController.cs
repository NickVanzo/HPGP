using UnityEngine;
using System.Collections;
//using TMPro;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;

public class CollisionUIController : MonoBehaviour
{
    //[SerializeField] TMP_Text text;

    private Entity counterEntity;
    private EntityManager entityManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //Wait for subscene to load
        yield return new WaitForSeconds(0.2f);
        counterEntity = entityManager.CreateEntityQuery(typeof(CounterTag)).GetSingletonEntity();
    }

    // Update is called once per frame
    void Update()
    {
        int _counter = entityManager.GetComponentData<CounterData>(counterEntity).value;
        //text = $"Frogs Hit: {_counter}";
    }
}
