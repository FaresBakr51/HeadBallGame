using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using System.Linq;
public class GameManager : NetworkBehaviour
{

    private static GameManager _instance;
    
    public List<PlayerController> _players = new List<PlayerController>();
   
    public List<Text> _scoreTxt = new List<Text>();
    public bool _gameOver;
    [SyncVar]
    public bool _goal;
    public GameObject[] _spawnPoints;
    public bool _gameStart;
    [SyncVar]
    [SerializeField] public bool _called;
    public struct GameManagerTypes : NetworkMessage
    {
        public List<PlayerController> players;
       public bool goal;
        
    }
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
        if(scene.name != "Offline")
        {
            if (isLocalPlayer)
            {
                Debug.Log("Owned Client");
            }
            //if (!isServer)
            //{
            //    var obj = Instantiate(gameObject);
            //    NetworkServer.Spawn(obj);
            //}
        }
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
       
        _called = false;
        _instance = this;
     //   DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (!_gameStart) return;
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
        foreach(GameObject txt in GameObject.FindGameObjectsWithTag("scoretxt"))
        {
            _scoreTxt.Add(txt.GetComponent<Text>());
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

    IEnumerator WaitPlayers()
    {

        yield return new WaitForSeconds(2f);
        SetPlayersInfo();
    }
    public void CheckAuth(string axis)
    {
        if (!hasAuthority)
        {

        }
        else
        {
            //AddGoaleToClients(axis);
        }
        
    }
   // [Command(requiresAuthority = true)]
  //private void AddGoaleToClients(string axis)
  //  {
  //      AddToOTherClients(axis);

  //  }
  //   [ClientRpc]
  //  private void AddToOTherClients(string axis)
  //  {
       

  //      StartCoroutine(RunReset());
  //  }
   
  //  IEnumerator RunReset()
  //  {
       
  //      yield return new WaitForSeconds(1f);
  //      GetCurrentGameBall().GetComponent<Rigidbody2D>().position = new Vector2(0, 2.5f);
  //      _players.ForEach(x => x.ResetPos());
  //      _goal = false;
  //  }

}
