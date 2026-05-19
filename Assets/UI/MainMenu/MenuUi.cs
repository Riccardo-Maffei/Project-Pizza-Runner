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

        // Buttons für die Tutorial-Themenauswahl
        private Button _generalInfoButton;
        private Button _dodgingObstaclesButton;
        private Button _coinsPizzasButton;
        private Button _puddleEffectButton;
        private Button _wineCoffeeButton;
        private Button _spiderButton;
        private Button _backToMainMenuButton; 

        private readonly Observable<bool> _tutorialOpen = new (false);
        private readonly Observable<bool> _modeSelectOpen = new (false);
        private readonly Observable<bool> _tutorialSelectOpen = new (false);

        private VisualElement _mainMenuContainer;
        private VisualElement _tutorialContainer;
        private VisualElement _modeSelectContainer;
        private VisualElement _tutorialSelectContainer;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Container zuweisen
            _mainMenuContainer = root.Q<VisualElement>("MainMenuContainer");
            _tutorialContainer = root.Q<VisualElement>("TutorialContainer");
            _modeSelectContainer = root.Q<VisualElement>("ModeSelectContainer"); 
            _tutorialSelectContainer = root.Q<VisualElement>("TutorialSelectContainer"); 

            // Standard-Buttons zuweisen
            _startGameButton = root.Q<Button>("StartGameButton");
            _exitGameButton = root.Q<Button>("ExitGameButton");
            _openTutorialButton = root.Q<Button>("OpenTutorialButton");
            _closeTutorialButton = root.Q<Button>("CloseTutorialButton");
            _storyModeButton = root.Q<Button>("StoryModeButton");
            _endlessModeButton = root.Q<Button>("EndlessModeButton");
            _closeModeSelectButton = root.Q<Button>("CloseModeSelectButton");

            // Tutorial-Auswahlbuttons via ID zuweisen
            _generalInfoButton = root.Q<Button>("GeneralInfoButton");
            _dodgingObstaclesButton = root.Q<Button>("DodgingObstaclesButton");
            _coinsPizzasButton = root.Q<Button>("CoinsPizzasButton");
            _puddleEffectButton = root.Q<Button>("PuddleEffectButton");
            _wineCoffeeButton = root.Q<Button>("WineCoffeeButton");
            _spiderButton = root.Q<Button>("SpiderButton");
            _backToMainMenuButton = root.Q<Button>("BackToMainMenuButton"); 

            // Klick-Events
            if (_openTutorialButton != null) _openTutorialButton.clicked += () => _tutorialSelectOpen.SetValue(true);
            
            if (_closeTutorialButton != null) {
                _closeTutorialButton.clicked += () => {
                    _tutorialOpen.SetValue(false);
                    _tutorialSelectOpen.SetValue(true); // Zurück zur Themenauswahl
                };
            }
            
            if (_startGameButton != null) _startGameButton.clicked += () => _modeSelectOpen.SetValue(true);
            if (_exitGameButton != null) _exitGameButton.clicked += GameHandler.EndGame;
            if (_closeModeSelectButton != null) _closeModeSelectButton.clicked += () => _modeSelectOpen.SetValue(false);
            if (_storyModeButton != null) _storyModeButton.clicked += OnStoryModeSelected;
            if (_endlessModeButton != null) _endlessModeButton.clicked += OnEndlessModeSelected;

            if (_backToMainMenuButton != null) _backToMainMenuButton.clicked += () => _tutorialSelectOpen.SetValue(false);
            if (_generalInfoButton != null) _generalInfoButton.clicked += () => OpenSpecificTutorial("GeneralInfo");
            if (_dodgingObstaclesButton != null) _dodgingObstaclesButton.clicked += () => OpenSpecificTutorial("DodgingObstacles");
            if (_coinsPizzasButton != null) _coinsPizzasButton.clicked += () => OpenSpecificTutorial("CoinsPizzas");
            if (_puddleEffectButton != null) _puddleEffectButton.clicked += () => OpenSpecificTutorial("PuddleEffect");
            if (_wineCoffeeButton != null) _wineCoffeeButton.clicked += () => OpenSpecificTutorial("WineCoffee");
            if (_spiderButton != null) _spiderButton.clicked += () => OpenSpecificTutorial("Spider");

            // --- Subscriptions für die Sichtbarkeit (Logik für das Ausblenden) ---

            // Wenn der eigentliche Tutorial-Text offen ist:
            _tutorialOpen.Subscribe(tutorialOpen => {
                if (_tutorialContainer != null) _tutorialContainer.style.display = tutorialOpen ? DisplayStyle.Flex : DisplayStyle.None;
                
                // WICHTIG: Wenn der Text offen ist, blenden wir die obere Box (Themenauswahl) AUS
                if (tutorialOpen && _tutorialSelectContainer != null) {
                    _tutorialSelectContainer.style.display = DisplayStyle.None;
                }
            });

            _modeSelectOpen.Subscribe(modeSelectOpen => {
                if (_mainMenuContainer != null) _mainMenuContainer.style.display = modeSelectOpen ? DisplayStyle.None : DisplayStyle.Flex;
                if (_modeSelectContainer != null) _modeSelectContainer.style.display = modeSelectOpen ? DisplayStyle.Flex : DisplayStyle.None;
            });

            _tutorialSelectOpen.Subscribe(tutorialSelectOpen => {
                // Hauptmenü weg, wenn Auswahl da ist
                if (_mainMenuContainer != null) _mainMenuContainer.style.display = tutorialSelectOpen ? DisplayStyle.None : DisplayStyle.Flex;
                
                // Nur anzeigen, wenn die Auswahl offen ist UND NICHT gerade der Text darüber liegt
                if (_tutorialSelectContainer != null) {
                    _tutorialSelectContainer.style.display = (tutorialSelectOpen && !_tutorialOpen.GetValue()) ? DisplayStyle.Flex : DisplayStyle.None;
                }
            });
        }

        private void OpenSpecificTutorial(string topicName)
        {
            if (topicName == "GeneralInfo")
            {
                _tutorialSelectOpen.SetValue(true); // Bleibt technisch offen (für ESC-Logik)
                _tutorialOpen.SetValue(true);        // Text schaltet sich darüber
                Debug.Log("Öffne Text-Tutorial: General Information");
            }
            else
            {
                Debug.Log("Tutorial für '" + topicName + "' ist noch nicht implementiert.");
            }
        }
        
        private void OnStoryModeSelected()
        {
            PlayerPrefs.SetInt("IsEndlessMode", 0); 
            GameHandler.LoadGameScene();
        }

        private void OnEndlessModeSelected()
        {
            PlayerPrefs.SetInt("IsEndlessMode", 1); 
            GameHandler.LoadGameScene();
        }

        protected void OnExitGameShortcut(InputValue value)
        {
            if (_tutorialOpen.GetValue())
            {
                _tutorialOpen.SetValue(false);
                _tutorialSelectOpen.SetValue(true); // Zurück zur Box-Auswahl
            }
            else if (_tutorialSelectOpen.GetValue())
            {
                _tutorialSelectOpen.SetValue(false); // Zurück zum Hauptmenü
            }
            else if (_modeSelectOpen.GetValue())
            {
                _modeSelectOpen.SetValue(false);
            }
            else 
            {
                StartCoroutine(ExitGameNextFrame());
            }
        }
        
        private void Update()
        {
            if (Keyboard.current == null) return;

            // --- 1. Shortcuts für das Hauptmenü (Wenn nichts anderes offen ist) ---
            if (!_modeSelectOpen.GetValue() && !_tutorialSelectOpen.GetValue() && !_tutorialOpen.GetValue())
            {
                // Taste 1 öffnet nun das Tutorial-Auswahlmenü
                if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
                {
                    _tutorialSelectOpen.SetValue(true);
                }
            }

            // --- 2. Shortcuts für das Start-Modus-Auswahlmenü ---
            if (_modeSelectOpen.GetValue())
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
                {
                    OnStoryModeSelected();
                }
                else if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
                {
                    OnEndlessModeSelected();
                }
            }

            // --- 3. Shortcuts für die Tutorial-Themenauswahl ---
            if (_tutorialSelectOpen.GetValue() && !_tutorialOpen.GetValue())
            {
                if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
                {
                    OpenSpecificTutorial("GeneralInfo");
                }
                else if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
                {
                    OpenSpecificTutorial("DodgingObstacles");
                }
                else if (Keyboard.current.digit3Key.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame)
                {
                    OpenSpecificTutorial("CoinsPizzas");
                }
                else if (Keyboard.current.digit4Key.wasPressedThisFrame || Keyboard.current.numpad4Key.wasPressedThisFrame)
                {
                    OpenSpecificTutorial("PuddleEffect");
                }
                else if (Keyboard.current.digit5Key.wasPressedThisFrame || Keyboard.current.numpad5Key.wasPressedThisFrame)
                {
                    OpenSpecificTutorial("WineCoffee");
                }
                else if (Keyboard.current.digit6Key.wasPressedThisFrame || Keyboard.current.numpad6Key.wasPressedThisFrame)
                {
                    OpenSpecificTutorial("Spider");
                }
            }
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