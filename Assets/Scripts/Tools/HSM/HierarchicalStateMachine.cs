using System;

namespace Tools.HSM
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class HierarchicalStateMachine : MonoBehaviour, IStateMachine
    {
        public enum SceneState
        {
            MainMenu,
            Game
        }

        private SceneState _currentSceneState;
        [SerializeField]private MainMenuStateMachine mainMenuStateMachine;
        private GameStateMachine _gameStateMachine;

        void Start()
        {
            EnterState();
        }


        public void EnterState()
        {
            _gameStateMachine = new GameStateMachine();
            mainMenuStateMachine.EnterState();
        }

        public void ChangeState(int newState)
        {
            if (_currentSceneState != (SceneState) newState)
            {
                _currentSceneState = (SceneState) newState;
                SceneManager.LoadScene(_currentSceneState == SceneState.MainMenu ? "MainMenu" : "Game");
                if (_currentSceneState == SceneState.MainMenu)
                    mainMenuStateMachine.EnterState();
                else
                    _gameStateMachine.EnterState();
            }

        }

        public void UpdateState()
        {
            throw new NotImplementedException();
        }

        public void ExitState()
        {
            throw new NotImplementedException();
        }
    }

}