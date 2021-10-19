using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildsList : MonoBehaviour
{
    public List<GameObject> builds;
    public int selectedIndex;
    public GameObject preview;
    Vector3 previewPos;
    int previewYRot;
    public Material buildOK, buildNo;
    public GameObject mainCanvas, placeCanvas;

    bool allNormal;
    private void Start()
    {
        DesactiveMainCanvas();
    }

    private void Update()
    {
        if (preview != null)
        {
            previewPos = new Vector3(previewPos.x, 0, previewPos.z);
            preview.transform.position = Vector3.Lerp(preview.transform.position, previewPos, 10 * Time.deltaTime);
            preview.transform.rotation = Quaternion.Lerp(preview.transform.rotation, Quaternion.Euler(0, previewYRot, 0), 10 * Time.deltaTime);

            if (!Input.GetKey(KeyCode.Mouse1) && !Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider != null)
                        {
                            var m_PointerEventData = new PointerEventData(EventSystem.current);
                            //Set the Pointer Event Position to that of the mouse position
                            m_PointerEventData.position = Input.mousePosition;

                            //Create a list of Raycast Results
                            List<RaycastResult> results = new List<RaycastResult>();

                            //Raycast using the Graphics Raycaster and mouse click position
                            placeCanvas.GetComponent<GraphicRaycaster>().Raycast(m_PointerEventData, results);

                            if (results.Count == 0)
                            {
                                previewPos = Vector3Int.RoundToInt(new Vector3(hit.point.x, 0, hit.point.z)) + new Vector3(0.5f, 0, 0.5f);

                                allNormal = true;
                                var oldPos = preview.transform.position;
                                preview.transform.position = previewPos;

                                foreach (var item in preview.GetComponent<Building>().parts)
                                {
                                    if (Physics.Raycast(item.transform.position + new Vector3(0, 2, 0), Vector3.down, out RaycastHit pHit, Mathf.Infinity, LayerMask.GetMask("Default")))
                                    {
                                        if (pHit.collider == null)
                                        {
                                            allNormal = false;
                                            break;
                                        }
                                        else
                                        {
                                            if (pHit.transform.tag != "Grass")
                                            {
                                                allNormal = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        allNormal = false;
                                        break;
                                    }
                                }
                                preview.transform.position = oldPos;

                                foreach (var item in preview.GetComponentsInChildren<Renderer>())
                                {
                                    if (item is SpriteRenderer) { item.gameObject.SetActive(false); continue; }
                                    if (item.GetComponent<ActionPoint>() != null) { item.transform.parent.gameObject.SetActive(false); continue; };
                                    item.material = allNormal ? buildOK : buildNo;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void AddRotation()
    {
        previewYRot += 45;
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

    public void ActiveMainCanvas()
    {
        mainCanvas.SetActive(true);
        placeCanvas.SetActive(false);
    }
    public void DesactiveMainCanvas()
    {
        mainCanvas.SetActive(false);
        placeCanvas.SetActive(false);
    }

    public void CancelBuild()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
        }
        previewPos = new Vector3();
        previewYRot = 0;
        GameManger.player.GetComponent<AutoMine>().enabled = true;
        GameManger.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        GameManger.player.GetComponent<MovebleObject>().enabled = true;
        GameManger.player.GetComponent<Player>().joy.gameObject.SetActive(true);
        Camera.main.transform.parent.GetComponent<CameraFollow>().enabled = true;
        Camera.main.transform.parent.GetComponent<CameraBuild>().enabled = false;


        QualitySettings.shadowDistance = 30;
        QualitySettings.shadowResolution = ShadowResolution.Medium;

        DesactiveMainCanvas();
    }

    public void SpawnBuild()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
        }
        previewPos = new Vector3();
        previewYRot = 0;
        GameManger.player.GetComponent<AutoMine>().enabled = false;
        GameManger.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GameManger.player.GetComponent<MovebleObject>().enabled = false;
        GameManger.player.GetComponent<Player>().joy.gameObject.SetActive(false);
        Camera.main.transform.parent.GetComponent<CameraFollow>().enabled = false;
        Camera.main.transform.parent.GetComponent<CameraBuild>().enabled = true;

        QualitySettings.shadowDistance = 60;
        QualitySettings.shadowResolution = ShadowResolution.Low;

        preview = Instantiate(builds[selectedIndex], previewPos, Quaternion.identity);
        foreach (var item in preview.GetComponentsInChildren<Collider>(true))
        {
            Destroy(item);
        }
        mainCanvas.SetActive(false);
        placeCanvas.SetActive(true);

        preview.GetComponent<Building>().status = BuildingType.Preview;
    }


    public void PlaceBuild()
    {
        if (allNormal)
        {
            var build = Instantiate(builds[selectedIndex], previewPos, Quaternion.Euler(0, previewYRot, 0));
            build.GetComponent<Building>().status = BuildingType.InConstruction;
            CancelBuild();
        }
    }
}
