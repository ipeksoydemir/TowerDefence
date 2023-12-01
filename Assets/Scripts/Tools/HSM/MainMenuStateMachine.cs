using System;

namespace Tools.HSM
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class MainMenuStateMachine : MonoBehaviour, IStateMachine
    {
        [SerializeField] List<GameObject> MainMenuPanels = new List<GameObject>();
        
        public enum MainMenuState
        {
            MainPanel = 0,
            LevelMapPanel = 1,
            SettingsPanel = 2
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        private MainMenuState _currentState;

        public void EnterState()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = "MainMenu";
            if (currentScene.name != sceneName)
            {
                SceneManager.LoadScene(sceneName);
            }

            _currentState = MainMenuState.MainPanel;
            OnMainMenuStateChange();
        }
        

        public void ChangeState(int newState)
        {
            if (_currentState != (MainMenuState) newState)
            {
                _currentState = (MainMenuState) newState;
                OnMainMenuStateChange();
            }
        }

        private void OnMainMenuStateChange()
        {
            for (int i = 0; i < MainMenuPanels.Count; i++)
            {
                MainMenuPanels[i].SetActive((int) _currentState == i);
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