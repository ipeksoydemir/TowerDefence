using Tools.HSM;
using UnityEngine;
using UnityEngine.UI;

namespace ToweDefence.Scenes.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button levelMapButton;
        [SerializeField] private MainMenuStateMachine mainMenuStateMachine;
        void OnEnable()
        {
            levelMapButton.onClick.AddListener(ChangeStateToLevelMap);
        }
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                ChangeStateToLevelMap();
            }
        }

        public void ChangeStateToLevelMap() => mainMenuStateMachine.ChangeState(1);

    }

}