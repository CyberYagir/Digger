using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandsManager : MonoBehaviour
{
    public static LandsManager instance;
    [SerializeField] GameObject landPrefab;
    public Land[,] lands = new Land[10, 10];
    public List<Land> activeLands = new List<Land>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!SaveLoadManager.haveSave)
            AddLand(new Vector2Int(0, 0));
    }

    public bool CheckPos(Vector2Int pos, Vector2Int dir)
    {
        var next = (pos + dir);
        if (next.x >= 0 && next.y >= 0 && next.x < lands.GetLength(0) && next.y < lands.GetLength(1))
        {
            if (lands[next.x, next.y] == null)
            {
                return true;
            }
        }
        return false;
    }

    public Land AddLand(Vector2Int pos, bool regen = true)
    {
        if (lands[pos.x, pos.y] == null)
        {
            var land = Instantiate(landPrefab, new Vector3(pos.x, 0, pos.y) * 50, Quaternion.identity);
            lands[pos.x, pos.y] = land.GetComponent<Land>();
            lands[pos.x, pos.y].arrayPos = pos;
            if (regen)
            {
                LandRegenerator.instance.RegenLand(new Vector3Int(pos.x, 0, pos.y));
            }
            activeLands.Add(lands[pos.x, pos.y]);
        }
        return lands[pos.x, pos.y];
    }

    public void ActiveLandWithLoading(Vector2Int pos)
    {
        StartCoroutine(waitLoad(pos));
    }

    public IEnumerator waitLoad(Vector2Int pos)
    {
        if (GameManger.canvas == null)
        {
            AddLand(pos);
            yield break;
        }
        else
        {
            UIManager.instance.loadingCanvas.SetActive(true);
            UIManager.instance.loadingCanvas.GetComponent<Animator>().StopPlayback();
            UIManager.instance.loadingCanvas.GetComponent<Animator>().Play("Loading");
            yield return new WaitForSeconds(0.5f);
            AddLand(pos);
            yield return new WaitForSeconds(0.5f);
            UIManager.instance.loadingCanvas.SetActive(false);
        }
    }
}
