using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : MonoBehaviour
{

    [Header("MoveProp")]
    [SerializeField] private float _speed;
    private float horizontalAxis;
    private Rigidbody2D _rig;



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

    }
    void FixedUpdate()
    {
       // if (!isLocalPlayer) return;
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

    public void NormalKick()
    {
        if (_canShot) {

           var ball =  GameManager.Instance.GetCurrentGameBall().GetComponent<Rigidbody2D>();
            ball.AddForce(new Vector2(-400,0));
        
        }
    }
    public void HighKick()
    {
        if (_canShot)
        {

            var ball = GameManager.Instance.GetCurrentGameBall().GetComponent<Rigidbody2D>();
            ball.AddForce(new Vector2(-400, 400));

        }
    }

    public void Jump()
    {

        if (_grounded) { _rig.velocity = new Vector2(_rig.velocity.x, _jumpForce); }
    }
}
