using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorMatchMaking
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] Text text;
        PlayerCard player;

        public void SetPlayer(PlayerCard player)
        {
            this.player = player;
            text.text = "Player " + player.playerIndex.ToString();
        }

    }
}
