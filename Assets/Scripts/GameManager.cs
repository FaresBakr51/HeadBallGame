using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
public class GameManager : NetworkBehaviour
{

    private static GameManager _instance;
    public List<PlayerController> _players = new List<PlayerController>();
    public bool _gameOver;
    public bool _goal;
    public GameObject[] _spawnPoints;
    public bool _gameStart;
    [SyncVar]
    private bool _called;
    public static GameManager Instance
    {

        get
        {
            if (_instance is null) {

                _instance = new GameManager();
            
            }
            return _instance;
        }
    }
    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       if(scene.name == "GamePlay")
        {
            GameModes._PvPMode = true;
            Debug.Log("Getting Players and Timer");
            GetSpawnpoints();
            StartCoroutine(WaitPlayers());
            
        }
    }
  
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (!_gameStart) return;
     //   if(_players.Count < 2) { return; }
        if (Goal() && !_called)
        {
            CmdWaitForGoal();
            _called = true;
        }
    }
    private void GetSpawnpoints()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");

    }
    public GameObject GetCurrentGameBall()
    {
        var ball = GameObject.FindGameObjectWithTag("Ball");
        return ball;
    }
    
  
    private void SetPlayersInfo()
    {
        foreach(GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
        {
            _players.Add(pl.GetComponent<PlayerController>());
        }

    }
    public bool IsGameOver()
    {
        return _gameOver;
    }

    public bool Goal()
    {
        return _goal;
    }
    public virtual GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn)
    {
        
        return null;
    }

    IEnumerator WaitPlayers()
    {

        yield return new WaitForSeconds(2f);
        SetPlayersInfo();
    }
   
    private void CmdWaitForGoal()
    {
        StartCoroutine(WaitGoal());
    }
    [Client]
    IEnumerator WaitGoal()
    {
        Debug.Log("Waiting Goal");
        yield return new WaitForSeconds(0.5f);
        GetCurrentGameBall().transform.position = new Vector2(0, 2f);
        RestPlayer();
        _goal = false;
        _called = false;
    }
   
    private void RestPlayer()
    {
        foreach(PlayerController obj in _players)
        {
            obj.ResetPos();

        }
    }

}
