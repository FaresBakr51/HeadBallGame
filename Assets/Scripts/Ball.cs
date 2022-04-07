using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
public class Ball : NetworkBehaviour
{
   
    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
                ActivePlayertoShot(collision.gameObject);
            
        }
        if (collision.gameObject.CompareTag("goalleft"))
        {
            if (GameModes._PvPMode)
            {
                if (!GameManager.Instance.IsGameOver() && !GameManager.Instance.Goal())
                {
                    var rightplayer = GameManager.Instance._players.FirstOrDefault(x => x.gameObject.transform.localRotation.eulerAngles.y == 0);
                    rightplayer.AddGoal(1);
                    PlayerGoal();
                }
            }
        }
        if (collision.gameObject.CompareTag("goalright"))
        {
            if (GameModes._PvPMode)
            {
                if (!GameManager.Instance.IsGameOver() && !GameManager.Instance.Goal())
                {
                    var rightplayer = GameManager.Instance._players.FirstOrDefault(x => x.gameObject.transform.localRotation.eulerAngles.y > 20);
                    rightplayer.AddGoal(1);
                    PlayerGoal();
                }
            }
        }
    }
    [Client]
    private void PlayerGoal()
    {
        if (!GameManager.Instance.Goal())
        {
            GameManager.Instance._goal = true;
           
        }
    }

    private void ActivePlayertoShot(GameObject pl)
    {
       var player = pl.GetComponent<PlayerController>();
        player._canShot = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            var player = collision.GetComponent<PlayerController>();
            player._canShot = false;
        }
    }
}
