using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
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

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }
   public GameObject GetCurrentGameBall()
    {
        var ball = GameObject.FindGameObjectWithTag("Ball");
        return ball;
    }
}
