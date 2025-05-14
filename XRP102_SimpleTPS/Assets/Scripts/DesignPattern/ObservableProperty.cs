using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    // 속성(체력, 마나) 하나 하나 관리하기 위해 따로 만들어 준 것
    public class ObservableProperty<T>
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            // 새로 받은 값이 이전과 같다면 바로 return
            set
            {
                if (_value.Equals(value)) return;

                // 그 다음 value 대입
                _value = value;

                // 값이 변경되면 알림
                Notify();
            }
        }

        // 해당 value가 교체됐을 때, 실행
        private UnityEvent<T> _onValueChanged = new();

        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        // UnityAction을 받아서 추가해주는 형태로 구현
        public void Subscribe(UnityAction<T> action)
        {
            _onValueChanged.AddListener(action);
        }

        public void Unsubscribe(UnityAction<T> action)
        {
            _onValueChanged.RemoveListener(action);
        }

        public void UnsubscribeAll()
        {
            _onValueChanged.RemoveAllListeners();
        }

        private void Notify()
        {
            _onValueChanged?.Invoke(Value);
        }
    }
}
