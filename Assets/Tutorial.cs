using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
[System.Serializable]
public class Quest
{
    public string message, targetText;
    public bool started, ended;
}

public class Tutorial : MonoBehaviour
{
    public int quest;
    [SerializeField] GameObject back;
    [SerializeField] TMP_Text messageT, targetT;
    [SerializeField] List<Quest> quests;
    [SerializeField] Transform arrow, arrowTarget;
    bool endTutorial;
    private void Start()
    {
        NewQuest(0);
    }
    void Update()
    {
        arrow.gameObject.SetActive(arrowTarget != null);
        arrow.transform.position = GameManger.player.transform.position;
        if (arrowTarget)
            arrow.LookAt(new Vector3(arrowTarget.position.x, arrow.position.y, arrowTarget.position.z));


        switch (quest)
        {
            case 0:
                if (arrowTarget == null && !quests[quest].started)
                {
                    var nt = EntityManager.entityManager.entities.OrderBy(x => Vector3.Distance(x.transform.position, GameManger.player.transform.position)).ToList()[0];
                    if (nt != null)
                    {
                        arrowTarget = nt.transform;
                        quests[quest].started = true;
                    }
                }
                if (GameManger.player.GetComponent<StackManager>().GetStack().Count != 0)
                {
                    NewQuest();
                }
                break;
            case 1:
                if (!quests[quest].started)
                {
                    arrowTarget = GameObject.FindGameObjectWithTag("BotsSpawn").transform;
                    quests[quest].started = true;
                }
                if (PlayersManager.instance.players.Count > 1)
                {
                    NewQuest();
                }
                break;
            case 2:
                if (!quests[quest].started)
                {
                    arrowTarget = FindObjectOfType<Builder>().transform;
                    quests[quest].started = true;
                }
                var build = FindObjectOfType<Building>();
                if (build != null)
                {
                    if (build.status == BuildingType.Finished)
                    {
                        NewQuest();
                    }
                }
                break;
            case 3:
                if (!endTutorial)
                {
                    GameDataObject.GetMain().saves.SetPref(Prefs.Tutorial, true);
                    endTutorial = true;
                }
                break;
        }
    }
    public void LoadLevel()
    {
        if (quest == 3 && endTutorial)
            Application.LoadLevel(0);
    }

    public void NewQuest(int addQ = 1)
    {
        if (quest < quests.Count)
        {
            quest += addQ;
            GetComponent<Animator>().Play("TutorialOn");
            messageT.text = quests[quest].message;
            targetT.text = quests[quest].targetText;
            targetT.gameObject.SetActive(false);
            arrowTarget = null;
        }
    }
}
