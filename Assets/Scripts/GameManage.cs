using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;
using MirrorMatchMaking;
    public class GameManage : NetworkBehaviour
    {
        [Header("InGameProp")]
        public float _timer;
        [SerializeField] private Text _timertxt;
        [SerializeField] public int _playersCount;
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _ball;
        [SerializeField] public Text[] _scoreTxt;
    [SerializeField] GameObject[] _players;
  
        void Start()
        {
            _timer = 90;

             _players = GameObject.FindGameObjectsWithTag("playercard");
            if (isLocalPlayer)
            {
                Debug.Log("local");
            }
            else
            {
                Debug.Log("notmine");
            }
          //  ActiveBall();
        }

    //[Client]
    //private void ActiveBall()
    //{
    //    foreach(GameObject pl in _players)
    //    {
    //        var obj = Instantiate(_player);
    //        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("GamePlay"));
    //        AddAuth(obj.GetComponent<NetworkIdentity>());
    //        NetworkServer.Spawn(obj);
    //    }
      
    //}
    //[Command]
    //private void AddAuth(NetworkIdentity id)
    //{
    //    CmdPickupItem(id);
    //}
    //void CmdPickupItem(NetworkIdentity item)
    //{
    //    item.AssignClientAuthority(connectionToClient);
    //}

    private void Update()
        {
      
            if (_playersCount >= 2 && !GameManager.Instance.Goal())
            {
                HandleTimer();
            }

        }
        private void HandleTimer()
        {

            if (_timer > 0) { _timer -= Time.deltaTime; _timertxt.text = ((int)_timer).ToString(); }
            if (_timer <= 0 && !GameManager.Instance.IsGameOver())
            {
                GameManager.Instance._gameOver = true;

            }
        }

    }

