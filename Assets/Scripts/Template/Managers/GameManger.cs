using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStage { StartWait, Game, EndWait };
public class GameManger : MonoBehaviour
{
    public static GameManger instance;
        
    //??????????
    public static LevelManager currentLevel { get; private set; }
    public static GameObject player { get; private set; }
    public static Canvas canvas { get; private set; }

    public GameStage gameStage;

    //??????
    GameDataObject.GDOMain data;
    GameDataObject gdata;

    /// ?????? 
    public static event System.Action StartGame = delegate { }; //????? gameStage ?????????? Game
    public static event System.Action EndGame = delegate { }; //????? gameStage ?????????? EndWait

    public static event System.Action TapToPlayUI = delegate { }; //????? ????? ?????? ? ?????? ??? ??? data.startByTap
    
    

    #region Mono
    public void Awake()
    {
        instance = this;
        StartGame = delegate { };
        EndGame = delegate { };
        TapToPlayUI = delegate { };
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);


        gdata = GameDataObject.GetData();
        data = gdata.main;
        OnLevelStarted(data);
        LoadLevel();
    }
    private void Update()
    {
        if (instance == null) instance = this;
        EditorControls();
        TapToStartCheck();
    }

    #endregion

    #region Gameplay
    public void TapToStartCheck() //???????? ?? ?????? ??? 
    {
        if (data.startByTap)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (gameStage == GameStage.StartWait)
                {
                    StartGameByTap();
                    gameStage = GameStage.Game;
                    TapToPlayUI();
                    StartGame();
                }
            }
        }
    }
    public void LoadLevel() //???????? ?????? 
    {
        var stdData = GameDataObject.GetMain(true);
        if (stdData.saves == null){ Debug.LogError("Yaroslav: Saves Not Found"); return; }
        if (stdData.levelList == null || stdData.levelList.Count == 0) { Debug.LogError("Yaroslav: Levels List in \"" + GameDataObject.GetData(true).name + "\" is empty"); return; }

        stdData.saves.SetLevel((int)stdData.saves.GetPref(Prefs.Level));
        currentLevel = Instantiate(stdData.levelList[(int)stdData.saves.GetPref(Prefs.Level)]);
        //????? ? ??????
        SpawnPlayer();
        SpawnCanvas();
    } 

    public void SpawnPlayer()
    {
        if (data.playerPrefab)
        {
            Vector3 spawnPoint = Vector3.zero;
            if (currentLevel.playerSpawn != null)
            {
                spawnPoint = currentLevel.playerSpawn.transform.position;
                //????????? ??????
            }
            else
            {
                Debug.LogError("Yaroslav: Spawn Not Found");
            }
            player = Instantiate(data.playerPrefab, spawnPoint, Quaternion.identity);
        }
    }

    public void SpawnCanvas()
    {
        if (data.canvas)
            canvas = Instantiate(data.canvas.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Canvas>();
    }

    public void StopGamePlay() //????????? ???? (?????? ? ??.) 
    {
        //?????????? ?????? ? ??.
    }
    public void StartGameByTap() //????????? ??? ???? (?????? ? ??.)
    {
        //????????? ?????????? ? ??.
    }

    #endregion


    #region Editor
    public void EditorControls()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }

#endif
    }
    #endregion

    #region Static
    public static void OnLevelStarted(GameDataObject.GDOMain data)
    {
        if (data.startByTap)
        {
            instance.gameStage = GameStage.StartWait;
            instance.StopGamePlay();
        }
        else
        {
            instance.gameStage = GameStage.Game;
            StartGame();
        }

        //????? ??????? 
    }
    public static void OnLevelEnd(bool win = true)
    {
        instance.StopGamePlay();
        instance.gameStage = GameStage.EndWait;
        EndGame();
        //?????? ??????
        //????? ??????
    }
    public static void Restart()
    {
        OnLevelEnd();
        SceneManager.LoadScene(0);
    }
    public static void NextLevel()
    {
        OnLevelEnd();
        var data = GameDataObject.GetMain(true);
        data.saves.SetPref(Prefs.Level, (int)data.saves.GetPref(Prefs.Level) + 1);
        data.saves.SetLevel((int)data.saves.GetPref(Prefs.Level));
        data.saves.AddToPref(Prefs.CompletedLevels, 1);
        SceneManager.LoadScene(0);
    }
    #endregion
}
