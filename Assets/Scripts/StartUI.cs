using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessNetWork
{
    public class StartUI : MonoBehaviour
    {
        [SerializeField] private GameObject startUI;
        private Animator menuAnimator;
        public static StartUI Instance { get; set; }
        private void Awake()
        {
            Instance = this;
            menuAnimator = GetComponent<Animator>(); ;
        }
        public void OnClickLocal()
        {
            menuAnimator.SetTrigger("InGameMenu");
            // startUI.SetActive(false);
            BoardController.Instance.ShowChessBoard();
        }
        public void OnClickOnline() { menuAnimator.SetTrigger("OnlineMenu"); }
        public void OnClickOnlineHost() { menuAnimator.SetTrigger("HostMenu"); }
        public void OnClickOnlineConnect() { menuAnimator.SetTrigger("InGameMenu"); }
        public void OnClickBack() { menuAnimator.SetTrigger("StartMenu"); }
        public void OnClickHostBack() { menuAnimator.SetTrigger("OnlineMenu"); }
    }
}