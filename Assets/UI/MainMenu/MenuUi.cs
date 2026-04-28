using System;

using UnityEngine;
using UnityEngine.UIElements;

using Utils;


namespace UI.MainMenu
{
    public class MenuUI : MonoBehaviour
    {
        private Button _startGameButton;
        private Button _exitGameButton;

        private Button _openTutorialButton;
        private Button _closeTutorialButton;

        private readonly Observable<bool> _tutorialOpen = new (false);

        private Label _distanceField;
        private Label _hpField;
        private Label _timeField;
        private Label _coinsField;
        private Label _pizzasField;

        private VisualElement _victoryMsgContainer;

        private VisualElement _mainMenuContainer;
        private VisualElement _tutorialContainer;

        private Action<double> _distanceObserver;
        private Action<int> _hpObserver;
        private Action<float> _timeObserver;
        private Action<int> _coinsObserver;
        private Action<int> _pizzasObserver;
        private Action<bool> _victoryObserver;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            _mainMenuContainer = root.Q<VisualElement>("MainMenuContainer");
            _tutorialContainer = root.Q<VisualElement>("TutorialContainer");
            
            _startGameButton = root.Q<Button>("StartGameButton");
            _exitGameButton = root.Q<Button>("ExitGameButton");
                
            _openTutorialButton = root.Q<Button>("OpenTutorialButton");
            _closeTutorialButton = root.Q<Button>("CloseTutorialButton");

            _startGameButton.clicked += GameHandler.LoadGameScene;
            _exitGameButton.clicked += GameHandler.EndGame;

            _openTutorialButton.clicked += () => _tutorialOpen.SetValue(true);
            _closeTutorialButton.clicked += () => _tutorialOpen.SetValue(false);
            
            _tutorialOpen.Subscribe(tutorialOpen => {
                _mainMenuContainer.style.display = tutorialOpen ? DisplayStyle.None : DisplayStyle.Flex;
                _tutorialContainer.style.display = tutorialOpen ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }
    }
}
