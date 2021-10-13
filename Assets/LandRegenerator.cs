using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandRegenerator : MonoBehaviour
{
    public static LandRegenerator instance;

    [SerializeField] Vector2 range;
    EntityManager entityManager;
    [SerializeField] List<GameObject> resources;

    private void Start()
    {
        instance = this;
        entityManager = GetComponent<EntityManager>();
    }
    public void Regen()
    {
        if (entityManager.entities.FindAll(x=>x != null).Count < 30)
        {
            entityManager.ResetData();
            RaycastHit hit;
            Vector3 pos = new Vector3(Random.Range(-range.x, range.x)/2f, 100, Random.Range(-range.y, range.y)/2f);
            Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
            int trys = 0;
            while (hit.transform.tag != "Grass" && Vector3.Distance(GameManger.player.transform.position, hit.point) < 25)
            {
                pos = new Vector3(Random.Range(-range.x, range.x) / 2f, 100, Random.Range(-range.y, range.y) / 2f);
                Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
                trys++;
                if (trys > 100) return;
            }
            var res = Random.Range(0, resources.Count);
            var m = Instantiate(resources[res], hit.point, Quaternion.identity, GameManger.currentLevel.treesHolder);
            m.transform.localScale = resources[res].transform.localScale;
            entityManager.entities.Add(m.GetComponent<Entity>());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector2.zero, new Vector3(range.x, 5, range.y));
    }
}
