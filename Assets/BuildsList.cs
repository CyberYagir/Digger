using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildsList : MonoBehaviour
{
    public List<GameObject> builds;
    public int selectedIndex;
    public GameObject preview;
    public Material buildOK;

    private void Update()
    {
        if (preview != null)
        {
            preview.transform.position = Vector3Int.RoundToInt(BuildPoint.instance.transform.position) + (Vector3.one/2f);
        }
    }

    public void NextBuild()
    {
        selectedIndex++;
        if (selectedIndex >= builds.Count)
        {
            selectedIndex = 0;
        }
    }


    public void PrevBuild()
    {
        selectedIndex--;
        if (selectedIndex < 0)
        {
            selectedIndex = builds.Count - 1;
        }
    }

    public void SpawnPreviw()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
        }
        GameManger.player.GetComponent<AutoMine>().enabled = false;
        GameManger.player.GetComponent<MovebleObject>().enabled = false;
        GameManger.player.GetComponent<Player>().joy.gameObject.SetActive(false);
        Camera.main.transform.parent.GetComponent<CameraFollow>().enabled = false;
        Camera.main.transform.parent.GetComponent<CameraBuild>().enabled = true;


        preview = Instantiate(builds[selectedIndex], BuildPoint.instance.transform.position, Quaternion.identity);

        foreach (var item in preview.GetComponentsInChildren<Collider>())
        {
            item.enabled = false;
        }

        foreach (var item in preview.GetComponentsInChildren<Renderer>())
        {
            if (item is SpriteRenderer) { item.gameObject.SetActive(false); continue; }
            if (item.GetComponent<ActionPoint>() != null) { item.transform.parent.gameObject.SetActive(false); continue; };
            item.material = buildOK;
        }
    }
}
