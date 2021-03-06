using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject deathUI, winUI;
    [SerializeField] GameObject tapToPlay, tutor;
    public GameObject loadingCanvas;
    #region Mono
    private void Start()
    {
        instance = this;
        if (!(bool)GameDataObject.GetMain().saves.GetPref(Prefs.Tutorial))
        {
            Instantiate(tutor, transform);
        }
        InitTapToPlay();
    }

    public void InitTapToPlay()
    {
        if (tapToPlay != null)
        {
            if (GameManger.instance.gameStage == GameStage.StartWait)
            {
                tapToPlay.SetActive(true);
                GameManger.TapToPlayUI += () => { Tweaks.AnimationPlayType(tapToPlay, PlayType.Rewind); }; //???????? ? ?????? ????
            }
            else
            {
                tapToPlay.SetActive(false);
            }
        }
    }
    
    private void Update()
    {
        EditorControls();
    }

    #endregion

    #region Buttons
    public void EditorControls()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Loose();
        }
#endif
    }

    public void NextLevel()
    {
        GameManger.NextLevel();
    }

    public void Restart()
    {
        GameManger.Restart();
    }


    #endregion

    #region Evens_Win_Loose
    public void Win()
    {
        if (!winUI.active && !deathUI.active)
        {
            GameManger.OnLevelEnd();
            winUI.SetActive(true);
        }
    }

    public void Loose()
    {
        if (!winUI.active && !deathUI.active)
        {
            GameManger.OnLevelEnd(false);
            deathUI.SetActive(true);
        }
    }

    #endregion


}
