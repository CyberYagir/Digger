using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public static ParticlesPool instance;
    List<GameObject> pool = new List<GameObject>();


    public void Start()
    {
        instance = this;
        for (int i = 0; i < 20; i++)
        {
            pool.Add(Instantiate(prefab, transform));
            pool[pool.Count - 1].transform.localScale *= 2f;
            pool[pool.Count - 1].SetActive(false);
        }
    }



    public GameObject GetFromPool(Vector3 pos)
    {
        if (pool.Count != 0)
        {
            var particle = pool[0];
            pool.RemoveAt(0);
            particle.transform.position = pos;
            particle.SetActive(true);
            particle.GetComponent<ParticleSystem>().Play();
            particle.gameObject.SetActive(true);
            StartCoroutine(backToPool(particle));
            return particle;
        }
        return null;
    }

    IEnumerator backToPool(GameObject particle)
    {
        yield return new WaitForSeconds(2f);
        particle.SetActive(false);
        pool.Add(particle);
    }
}
