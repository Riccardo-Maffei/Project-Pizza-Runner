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
        private Label _distanceField;
        private Label _hpField;
        private Label _timeField;
        private Label _coinsField;
        private Label _pizzasField;
        private VisualElement _victoryMsgContainer;

        private Action<double> _distanceObserver;
        private Action<int> _hpObserver;
        private Action<float> _timeObserver;
        private Action<int> _coinsObserver;
        private Action<int> _pizzasObserver;
        private Action<bool> _victoryObserver;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            _startGameButton = root.Q<Button>("StartGameButton");
            _exitGameButton = root.Q<Button>("ExitGameButton");

            _startGameButton.clicked += GameHandler.LoadGameScene;
            _exitGameButton.clicked += GameHandler.EndGame;
        }
    }
}
