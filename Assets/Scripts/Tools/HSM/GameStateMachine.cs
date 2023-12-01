using System.Collections.Generic;

namespace Tools.HSM
{
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class GameStateMachine : MonoBehaviour,IStateMachine
    {
        [SerializeField] List<GameObject> GamePanels = new List<GameObject>();

        public enum GameState
        {
            GameBoard = 0,
            StorePanel = 1,
            WinPanel = 2,
            LostPanel = 3
        }
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        private GameState _currentState;

        public void EnterState()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = "Game";
            if (currentScene.name != sceneName)
            {
                SceneManager.LoadScene(sceneName);
            }

            ChangeState(0);
        }

        public void ChangeState(int newState)
        {
            if (_currentState != (GameState)newState)
            {
                _currentState = (GameState)newState;
                OnGameStateChange();

            }
        }

        public void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public void ExitState()
        {
            throw new System.NotImplementedException();
        }
        

        private void OnGameStateChange()
        {
            for (int i = 0; i < GamePanels.Count; i++)
            {
                GamePanels[i].SetActive((int) _currentState == i);
            }
        }
        
        public void ChangeStateToGameBoard() => ChangeState(0);
        public void ChangeStateToStore() => ChangeState(1);
        public void ChangeStateToWin() => ChangeState(2);
        public void ChangeStateToLost() => ChangeState(3);
    }

}