using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using Utils;


namespace UI.Level
{
    public class GameUI : MonoBehaviour
    {
        private Label _distanceField;
        private Label _hpField;
        private Label _timeField;
        private Label _coinsField;
        private Label _pizzasField;
        
        private VisualElement _lossMsgContainer;
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

            _distanceField = root.Q<Label>("DistanceField");
            _hpField = root.Q<Label>("HpField");
            _timeField = root.Q<Label>("TimeField");
            _coinsField = root.Q<Label>("CoinsField");
            _pizzasField = root.Q<Label>("PizzasField");
            
            _lossMsgContainer = root.Q<VisualElement>("LossMsgContainer");
            _victoryMsgContainer = root.Q<VisualElement>("VictoryMsgContainer");

            GameData.Reset();

            // Measurement units reached after x considering max speed of 10m/s
            _distanceObserver = newValue => _distanceField.text =
                // parsec (3.0857e16 m) — reached after ~97,924 years
                newValue >= 3.0857e15 ? (newValue / 3.0857e16).ToString("000000.00") + " pc"
                // light year (9.461e15 m) — reached after ~30,000 years
                : newValue >= 9.461e14 ? (newValue / 9.461e15).ToString("000000.00") + " ly"
                // astronomical unit (1.496e11 m) — reached after ~474 years
                : newValue >= 1.496e10 ? (newValue / 1.496e11).ToString("000000.00") + " AU"
                // kilometer — reached after ~80 seconds
                : newValue >= 800 ? (newValue / 1000).ToString("000000.00") + " km"
                // meter — start
                : newValue.ToString("000000.00") + "  m";

            _hpObserver = newValue =>
            {
                _hpField.text = string.Join(" ", Enumerable.Repeat("<3", newValue));
                _lossMsgContainer.style.display = newValue <= 0 ? DisplayStyle.Flex : DisplayStyle.None;
            };
            
            _timeObserver = newValue => _timeField.text = TimeSpan.FromSeconds(newValue).ToString(@"hh\:mm\:ss\.fff");
            _coinsObserver = newValue => _coinsField.text = newValue.ToString();
            _pizzasObserver = newValue => _pizzasField.text = newValue.ToString();
            _victoryObserver = newValue => _victoryMsgContainer.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;

            _distanceObserver(GameData.TotalDistance.GetValue());
            _hpObserver(GameData.Hp.GetValue());
            _timeObserver(GameData.CurrentTime.GetValue());
            _coinsObserver(GameData.Coins.GetValue());
            _pizzasObserver(GameData.Pizzas.GetValue());
            _victoryObserver(GameData.CrossedFinishLine.GetValue());

            GameData.TotalDistance.Subscribe(_distanceObserver);
            GameData.Hp.Subscribe(_hpObserver);
            GameData.CurrentTime.Subscribe(_timeObserver);
            GameData.Coins.Subscribe(_coinsObserver);
            GameData.Pizzas.Subscribe(_pizzasObserver);
            GameData.CrossedFinishLine.Subscribe(_victoryObserver);
        }

        private void OnDisable()
        {
            GameData.TotalDistance.Unsubscribe(_distanceObserver);
            GameData.Hp.Unsubscribe(_hpObserver);
            GameData.CurrentTime.Unsubscribe(_timeObserver);
            GameData.Coins.Unsubscribe(_coinsObserver);
            GameData.Pizzas.Unsubscribe(_pizzasObserver);
            GameData.CrossedFinishLine.Unsubscribe(_victoryObserver);
        }

        private void Update()
        {
            if (GameData.CrossedFinishLine.GetValue()) return;

            var currentLock = GameData.TimeLock.GetValue();
            var nextLock = currentLock - Time.deltaTime;

            if (nextLock < 0)
            {
                GameData.TimeLock.SetValue(0);
                GameData.CurrentTime.Increase(Math.Abs(nextLock));
            }
            else GameData.TimeLock.SetValue(nextLock);
        }
    }
}
