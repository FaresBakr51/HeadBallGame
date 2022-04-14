using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MirrorMatchMaking;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _mycanavas;

    private PlayerCard _myCard;
    [Header("MoveProp")]
    [SerializeField] private float _speed;
    [SyncVar]
    private float horizontalAxis;
    [SyncVar]
    [SerializeField] private bool _movingLeft;
    [SyncVar]
    [SerializeField] private bool _movingRight;
    private Rigidbody2D _rig;


    [Header("GameMange")]
    public Text _goalTxt;
    public int _goals;
   
    [SyncVar]
    public GameManage _gamemanage;
    [SyncVar]
    public GameManager _gameManager;
   [SerializeField]  public Vector2 _mypos;

    [Header("ShotProp")]
    public bool _canShot;



    [Header("JumpProp")]

    [SyncVar]
    [SerializeField] private bool _grounded;



    [SerializeField] private LayerMask _groundLAYER;
    [SerializeField] private Transform _checkGround;
    [SerializeField] private float _jumpForce;
   
    void Start()
    {
        
        _myCard = GetComponent<PlayerCard>();
        
        _rig = GetComponent<Rigidbody2D>();
      
    }
    [Client]
    public void StartPorp()
    {
      
        _rig.WakeUp();
        if (!isLocalPlayer)
        {
            _mycanavas.SetActive(false);
        }
        else
        {
            _mycanavas.SetActive(true);
        }
        _gameManager = GetComponent<GameManager>();
        _gamemanage = FindObjectOfType<GameManage>();
        _gamemanage._playersCount++;
        CheckMyProp();
      //  _mypos = this._rig.position;
    }
    [Client]
    public void ResetPos()
    {
        _rig.Sleep();
        this._rig.position = new Vector2(_mypos.x,_mypos.y +1);
    }
    public void CheckMyProp()
    {
        if(transform.localRotation.eulerAngles.y > 0)
        {
            _goalTxt = _gamemanage._scoreTxt[1];
        }
        else
        {
            _goalTxt = _gamemanage._scoreTxt[0];
        }
    }
    void FixedUpdate()
    {
         if (!isLocalPlayer) return;
        if (!hasAuthority) return;
        if (GameManager.Instance.IsGameOver()) return;
        if (GameManager.Instance.Goal()) return;
         if(_movingLeft || _movingRight) { MovingAxis(); }
        _grounded = Physics2D.OverlapCircle(_checkGround.position, 0.2f, _groundLAYER);
    } 
    [Command]
    private void MovingAxis()
    {
        
        MovingCliet();
    }
    [ClientRpc]
    private void MovingCliet()
    {
        if (_movingLeft)
        {
            _rig.velocity = new Vector2(Time.fixedDeltaTime * _speed * -1, _rig.velocity.y);
        }else if (_movingRight)
        {
            _rig.velocity = new Vector2(Time.fixedDeltaTime * _speed * 1, _rig.velocity.y);
        }
    }
    [Command]
    public void MoveAxis(bool val)
    {
        _movingLeft = val;
        _movingRight = !val;
        MoveClient(val);
    }
    [ClientRpc]
    private void MoveClient(bool vale) {
        _movingLeft = vale;
        _movingRight = !vale;
    
    }
   
    public void StopMove()
    {
        _movingLeft = false;
        _movingRight = false;
        horizontalAxis = 0;

    }
   
    public void NormalKick()
    {
        if (!isLocalPlayer) return;

        if (hasAuthority)
        {

            if (_canShot)
            {

                var ball = GameManager.Instance.GetCurrentGameBall().GetComponent<Rigidbody2D>();
                var auth = ball.GetComponent<NetworkIdentity>();
                CmdPickupItem(auth);
                if (transform.localRotation.y == 0)
                {
                    ball.AddForce(new Vector2(-400, 0));

                }
                else { ball.AddForce(new Vector2(400, 0)); }
            }
        }
    }
   
    
    public void HighKick()
    {  
       
        if (!isLocalPlayer) return;
        if (hasAuthority)
        {
            if (_canShot)

            {
                var ball = GameManager.Instance.GetCurrentGameBall().GetComponent<Rigidbody2D>();
                var auth = ball.GetComponent<NetworkIdentity>();
                CmdPickupItem(auth);
                if (transform.localRotation.y == 0)
                {
                    Debug.Log("Iam in the right");
                    ball.AddForce(new Vector2(-400, 500));
                }
                else
                {
                    Debug.Log("Iam in the left");
                    ball.AddForce(new Vector2(400, 500));
                }


            }
        }
    }
    [Command]
    public void CmdPickupItem(NetworkIdentity item)
    {
        item.RemoveClientAuthority();
        item.AssignClientAuthority(connectionToClient);
    }
   [Command]
    public void Jump()
    {

        ClientJump();
    }
    [ClientRpc]
    private void ClientJump()
    {
        if (GameManager.Instance.Goal()) return;
        if (_grounded) { _rig.velocity = new Vector2(_rig.velocity.x, _jumpForce); }
    }


    [Command]
    public void AddGoal(string axis)
    {
        // if (!GameManager.Instance._goal) return;
       
        AddGoalClients(axis);
   //     GameManager.Instance._goal = true;
     //   StartCoroutine(WaitGoal());
    }
    [ClientRpc]
    private void AddGoalClients(string axis)
    {

        _goals += 1;
        _goalTxt.text = _goals.ToString();
        var allgamemaanger = GameObject.FindObjectsOfType<GameManager>().ToList();
        allgamemaanger.ForEach(x => x._goal = true);
       // _gameManager._goal = true;
        AddGoaleToClients(axis);

      //  _gameManager.CheckAuth(axis);
    }
    [Command]
    private void AddGoaleToClients(string axis)
    {
        AddToOTherClients(axis);

    }
    [ClientRpc]
    private void AddToOTherClients(string axis)
    {


        StartCoroutine(RunReset());
    }

    IEnumerator RunReset()
    {

        yield return new WaitForSeconds(1f);
       _gameManager.GetCurrentGameBall().GetComponent<Rigidbody2D>().position = new Vector2(0, 2.5f);
        _gameManager._players.ForEach(x => x.ResetPos());
        var allgamemaanger = GameObject.FindObjectsOfType<GameManager>().ToList();
        allgamemaanger.ForEach(x => x._goal = false);
    }


    //[Command(requiresAuthority = false)]
    //public void AddGoalServer(int goal)
    //{

    //    AddGoal(1);
    //}


    //[ClientRpc(includeOwner = true)]
    //public void AddGoal(int goal)
    //{

    //    _goals += goal;
    //    _goalTxt.text = _goals.ToString();
    //    GameManager.Instance._goal = true;
    //    StartCoroutine(WaitGoal());
    //}
    //IEnumerator WaitGoal()
    //{
    //    Debug.Log("Waiting Goal");
    //    yield return new WaitForSeconds(0.5f);
    //    GameManager.Instance.GetCurrentGameBall().GetComponent<Rigidbody2D>().position = new Vector2(0, 2.5f);
    //    GameManager.Instance.Magnify();


    //}


}
