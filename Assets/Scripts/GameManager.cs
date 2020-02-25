using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    static GameManager Manager;

    public int Player1Score;
    public int Player2Score;

    public GameObject BlueScore;
    public GameObject RedScore;

    public RectTransform[] BoostChargeDisplay;
    public List<GameObject> MapChuncks;
    public float BreakOffLimit;

    float MapTimer;

    void Start()
    {
        Manager = FindManager();
        
    }

    private void Update()
    {
        MapController();
        UpdateScoreDisplay();
        if (UltimateManager.UM)
        {
            Player1Score = UltimateManager.UM.P1Score;
            Player2Score = UltimateManager.UM.P2Score;
        }
    }

    public void UpdatePlayerBoost(int _PlayerNumber, float _NewHeight)
    {
        BoostChargeDisplay[_PlayerNumber].sizeDelta = new Vector2(100, _NewHeight);
    }
    void UpdateScoreDisplay()
    {
        BlueScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Blue Score: " + Player1Score;
        BlueScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Blue Score: " + Player1Score;

        RedScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Red Score: " + Player2Score;
        RedScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Red Score: " + Player2Score;
    }

    public static GameManager FindManager()
    {
        GameManager ReturningManager;

        GameObject TempManager = GameObject.FindGameObjectWithTag("GameManager");
        if (TempManager == null) ReturningManager = CreateMissingManager().GetComponent<GameManager>();
        else ReturningManager = TempManager.GetComponent<GameManager>();

        return ReturningManager;
        return Manager;
    }
    public static GameObject CreateMissingManager()
    {
        GameObject NewManager = new GameObject();
        NewManager.tag = "GameManager";
        NewManager.AddComponent<GameManager>();
        return NewManager;
    }

    void ResetMapTimer()
    {
        MapTimer = Time.time;
    }
    void PopOffNextMapPiece()
    {
        MapChuncks[MapChuncks.Count - 1].SetActive(false);
        MapChuncks.RemoveAt(MapChuncks.Count - 1);
    }
    void MapController()
    {
        if(Time.time - MapTimer > BreakOffLimit && MapChuncks.Count > 1)
        {
            ResetMapTimer();
            PopOffNextMapPiece();
        }
    }

}
