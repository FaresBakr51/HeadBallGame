using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _mycanavas;
    [Header("MoveProp")]
    [SerializeField] private float _speed;
    private float horizontalAxis;
    private Rigidbody2D _rig;


    [Header("GameMange")]
    public Text _goalTxt;
    public int _goals;
    private GameManage _gamemanage;
   [SerializeField]  private Vector2 _mypos;

    [Header("ShotProp")]
    public bool _canShot;



    [Header("JumpProp")]

  
    [SerializeField] private bool _grounded;



    [SerializeField] private LayerMask _groundLAYER;
    [SerializeField] private Transform _checkGround;
    [SerializeField] private float _jumpForce;
    
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        if (!isLocalPlayer)
        {
            _mycanavas.SetActive(false);
        }
        _gamemanage = FindObjectOfType<GameManage>();
        _gamemanage._playersCount++;
        CheckMyProp();
        _mypos = this._rig.position;
    }
    [Client]
    public void ResetPos()
    {
        _rig.Sleep();
        this._rig.position = new Vector2(_mypos.x,_mypos.y +1);
    }
    private void CheckMyProp()
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
        if (GameManager.Instance.IsGameOver()) return;
        if (GameManager.Instance.Goal()) return;
        _rig.velocity = new Vector2(Time.deltaTime * _speed * horizontalAxis, _rig.velocity.y);
        _grounded = Physics2D.OverlapCircle(_checkGround.position, 0.2f, _groundLAYER);
    }
    public void Move(int val)
    {
        horizontalAxis = val;
    }
    public void StopMove()
    {

        horizontalAxis = 0;

    }
   [Client]
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
   
    [Client]
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

   
    public void Jump()
    {

        if (_grounded) { _rig.velocity = new Vector2(_rig.velocity.x, _jumpForce); }
    }

    [Client]
    public void AddGoal(int goal)
    {
        _goals += goal;
        _goalTxt.text = _goals.ToString();
    }
}
