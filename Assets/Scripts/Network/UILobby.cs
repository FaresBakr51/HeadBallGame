using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace MirrorMatchMaking
{
    public class UILobby : MonoBehaviour
    {
        public static UILobby instance;

        [Header("Host Join")]
        [SerializeField] InputField joinMatchInput;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();
        [SerializeField] GameObject lobbyCanvas;
        [SerializeField] GameObject searchCanvas;
        bool searching = false;

        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] GameObject beginGameButton;

        GameObject localPlayerLobbyUI;

        void Start()
        {
            instance = this;
        }

        public void SetStartButtonActive(bool active)
        {
            beginGameButton.SetActive(active);
        }

        public void HostPublic()
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            PlayerCard.localPlayer.HostGame(true);
        }

        //public void HostPrivate()
        //{
        //    lobbySelectables.ForEach(x => x.interactable = false);
        //    PlayerCard.localPlayer.HostGame(false);
        //}

        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.SetActive(true);

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(PlayerCard.localPlayer);
                matchIDText.text = matchID;
            }
            else
            {
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void Join()
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            PlayerCard.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }

        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.SetActive(true);

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(PlayerCard.localPlayer);
                matchIDText.text = matchID;
            }
            else
            {
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void DisconnectGame()
        {
            if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                PlayerCard.localPlayer.DisconnectGame();

            lobbyCanvas.SetActive(false);
            lobbySelectables.ForEach(x => x.interactable = true);
        }

        public GameObject SpawnPlayerUIPrefab(PlayerCard player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);

            return newUIPlayer;
        }

        public void BeginGame()
        {
          PlayerCard.localPlayer.BeginGame();
        }

        public void SearchGame()
        {
            StartCoroutine(Searching());
        }

        public void CancelSearchGame()
        {
            searching = false;
        }

        public void SearchGameSuccess(bool success, string matchID)
        {
            if (success)
            {
                searchCanvas.SetActive(false);
                searching = false;
                JoinSuccess(success, matchID);
            }
        }

        IEnumerator Searching()
        {
            searchCanvas.SetActive(true);
            searching = true;

            float searchInterval = 1;
            float currentTime = 1;

            while (searching)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = searchInterval;
                    PlayerCard.localPlayer.SearchGame();
                }
                yield return null;
            }
            searchCanvas.SetActive(false);
        }

    }
}


