using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GameManage : MonoBehaviour
{
    [Header("InGameProp")]
    public float _timer;
    [SerializeField] private Text _timertxt;
    [SerializeField] public int _playersCount;
    [SerializeField] private GameObject _ball;
    [SerializeField] public Text[] _scoreTxt;
    void Start()
    {
        _timer = 90;

    }
 
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
