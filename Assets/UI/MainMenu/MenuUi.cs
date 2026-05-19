using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

using Utils;

namespace UI.MainMenu
{
    public class MenuUI : MonoBehaviour
    {
        private Button _startGameButton;
        private Button _exitGameButton;

        private Button _openTutorialButton;
        private Button _closeTutorialButton;
        
        private Button _storyModeButton;
        private Button _endlessModeButton;
        private Button _closeModeSelectButton;

        private readonly Observable<bool> _tutorialOpen = new (false);
        private readonly Observable<bool> _modeSelectOpen = new (false);

        private Label _distanceField;
        private Label _hpField;
        private Label _timeField;
        private Label _coinsField;
        private Label _pizzasField;

        private VisualElement _victoryMsgContainer;

        private VisualElement _mainMenuContainer;
        private VisualElement _tutorialContainer;
        private VisualElement _modeSelectContainer;

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
            _modeSelectContainer = root.Q<VisualElement>("ModeSelectContainer"); // NEU
            
            _startGameButton = root.Q<Button>("StartGameButton");
            _exitGameButton = root.Q<Button>("ExitGameButton");
                
            _openTutorialButton = root.Q<Button>("OpenTutorialButton");
            _closeTutorialButton = root.Q<Button>("CloseTutorialButton");
            
            _storyModeButton = root.Q<Button>("StoryModeButton");
            _endlessModeButton = root.Q<Button>("EndlessModeButton");
            _closeModeSelectButton = root.Q<Button>("CloseModeSelectButton");
            
            _startGameButton.clicked += () => _modeSelectOpen.SetValue(true);
            _exitGameButton.clicked += GameHandler.EndGame;

            _openTutorialButton.clicked += () => _tutorialOpen.SetValue(true);
            _closeTutorialButton.clicked += () => _tutorialOpen.SetValue(false);
            
            _closeModeSelectButton.clicked += () => _modeSelectOpen.SetValue(false);
            _storyModeButton.clicked += OnStoryModeSelected;
            _endlessModeButton.clicked += OnEndlessModeSelected;
            
            _tutorialOpen.Subscribe(tutorialOpen => {
                _mainMenuContainer.style.display = tutorialOpen ? DisplayStyle.None : DisplayStyle.Flex;
                _tutorialContainer.style.display = tutorialOpen ? DisplayStyle.Flex : DisplayStyle.None;
            });
            
            _modeSelectOpen.Subscribe(modeSelectOpen => {
                _mainMenuContainer.style.display = modeSelectOpen ? DisplayStyle.None : DisplayStyle.Flex;
                _modeSelectContainer.style.display = modeSelectOpen ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }
        
        private void OnStoryModeSelected()
        {
            PlayerPrefs.SetInt("IsEndlessMode", 0); // 0 = Feste Levels
            GameHandler.LoadGameScene();
        }

        private void OnEndlessModeSelected()
        {
            PlayerPrefs.SetInt("IsEndlessMode", 1); // 1 = Unendliches Level
            GameHandler.LoadGameScene();
        }

        protected void OnStartGameShortcut(InputValue value)
        {
            if (!_tutorialOpen.GetValue() && !_modeSelectOpen.GetValue()) 
            {
                _modeSelectOpen.SetValue(true);
            }
        }

        protected void OnExitGameShortcut(InputValue value)
        {
            // esc to go back
            if (_modeSelectOpen.GetValue())
            {
                _modeSelectOpen.SetValue(false);
            }
            // esc to close tutorial
            else if (_tutorialOpen.GetValue())
            {
                _tutorialOpen.SetValue(false);
            }
            // esc to close game
            else 
            {
                StartCoroutine(ExitGameNextFrame());
            }
        }
        
        private void Update()
        {
            if (_modeSelectOpen.GetValue() && Keyboard.current != null)
            {
                // Taste 1 = Story Modus
                if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
                {
                    OnStoryModeSelected();
                }
                // Taste 2 = Endless Modus
                else if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
                {
                    OnEndlessModeSelected();
                }
            }
        }

        private static IEnumerator LoadGameNextFrame()
        {
            yield return null;
            GameHandler.LoadGameScene();
        }

        private static IEnumerator ExitGameNextFrame()
        {
            yield return null;
            GameHandler.EndGame();
        }

        private void OnDisable()
        {
            _exitGameButton.clicked -= GameHandler.EndGame;
            if (_storyModeButton != null) _storyModeButton.clicked -= OnStoryModeSelected;
            if (_endlessModeButton != null) _endlessModeButton.clicked -= OnEndlessModeSelected;
        }
    }
}