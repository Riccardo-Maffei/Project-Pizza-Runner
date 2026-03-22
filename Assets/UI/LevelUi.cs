using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;

using utils;


namespace  UI
{
    public class GameUI : MonoBehaviour
    {
        private Label _distanceField;
        private Label _hpField;
        private Label _timeField;
        private Label _coinsField;
        private Label _pizzasField;

        private Action<double> _distanceObserver;
        private Action<int> _hpObserver;
        private Action<float> _timeObserver;
        private Action<int> _coinsObserver;
        private Action<int> _pizzasObserver;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            _distanceField = root.Q<Label>("DistanceField");
            _hpField = root.Q<Label>("HpField");
            _timeField = root.Q<Label>("TimeField");
            _coinsField = root.Q<Label>("CoinsField");
            _pizzasField = root.Q<Label>("PizzasField");

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

            _hpObserver = newValue => _hpField.text = string.Join(" ", Enumerable.Repeat("<3", newValue));
            _timeObserver = newValue => _timeField.text = TimeSpan.FromSeconds(newValue).ToString(@"hh\:mm\:ss\.fff");
            _coinsObserver = newValue => _coinsField.text = newValue.ToString();
            _pizzasObserver = newValue => _pizzasField.text = newValue.ToString();

            _distanceObserver(GameData.TotalDistance.GetValue());
            _hpObserver(GameData.Hp.GetValue());
            _timeObserver(GameData.CurrentTime.GetValue());
            _coinsObserver(GameData.Coins.GetValue());
            _pizzasObserver(GameData.Pizzas.GetValue());

            GameData.TotalDistance.Subscribe(_distanceObserver);
            GameData.Hp.Subscribe(_hpObserver);
            GameData.CurrentTime.Subscribe(_timeObserver);
            GameData.Coins.Subscribe(_coinsObserver);
            GameData.Pizzas.Subscribe(_pizzasObserver);
        }

        private void OnDisable()
        {
            GameData.TotalDistance.Unsubscribe(_distanceObserver);
            GameData.Hp.Unsubscribe(_hpObserver);
            GameData.CurrentTime.Unsubscribe(_timeObserver);
            GameData.Coins.Unsubscribe(_coinsObserver);
            GameData.Pizzas.Unsubscribe(_pizzasObserver);
        }

        private void Update()
        {
            GameData.CurrentTime.Increase(Time.deltaTime);
        }
    }
}
