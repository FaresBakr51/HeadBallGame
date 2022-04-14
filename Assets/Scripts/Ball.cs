using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
public class Ball : NetworkBehaviour
{
    private GameManage _gammanage;
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
                    var player = GameObject.FindGameObjectsWithTag("Player").ToList();
                    var right =  player.FirstOrDefault(x => x.gameObject.transform.localRotation.eulerAngles.y == 0);
                    var script = right.GetComponent<PlayerController>();
                    script.AddGoal("right");
                    //var rightplayer = GameManager.Instance._players.FirstOrDefault(x => x.gameObject.transform.localRotation.eulerAngles.y == 0);
                    //rightplayer.AddGoal("right");
               
                }
            }
        }
        if (collision.gameObject.CompareTag("goalright"))
        {
            if (GameModes._PvPMode)
            {
                if (!GameManager.Instance.IsGameOver() && !GameManager.Instance.Goal())
                {
                    var player = GameObject.FindGameObjectsWithTag("Player").ToList();
                    var right = player.FirstOrDefault(x => x.gameObject.transform.localRotation.eulerAngles.y > 20);
                    var script = right.GetComponent<PlayerController>();
                    script.AddGoal("left");


                    //var rightplayer = GameManager.Instance._players.FirstOrDefault(x => x.gameObject.transform.localRotation.eulerAngles.y > 20);
                    //rightplayer.AddGoal("left");


                }
            }
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
