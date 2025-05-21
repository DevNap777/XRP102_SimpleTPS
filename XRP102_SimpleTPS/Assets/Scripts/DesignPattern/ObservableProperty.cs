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
            // 새로 받은 값이 이전 값과 같다면 대입하지 않고 바로 return
            set
            {
                if (_value.Equals(value)) return;

                // 그 다음 value 대입
                _value = value;

                // 값이 변경되면 알림
                Notify();
            }
        }

        // 유니티 이벤트 사용
        // 해당 value가 교체됐을 때, 실행
        private UnityEvent<T> _onValueChanged = new();

        // 생성자에서 T value에 대한 것을 아무 값도 안들어 온다면 default
        public ObservableProperty(T value = default)
        {
            // 들어온다면 들어온 값으로 세팅
            _value = value;
        }

        // UnityAction을 받아서 추가해주는 형태로 구현
        // 구독
        public void Subscribe(UnityAction<T> action)
        {
            _onValueChanged.AddListener(action);
        }

        // 구독 해지
        public void Unsubscribe(UnityAction<T> action)
        {
            _onValueChanged.RemoveListener(action);
        }

        // 모든 구독 해지
        // 이번 게임에서는 쓰지 않을 것 같긴함.
        public void UnsubscribeAll()
        {
            _onValueChanged.RemoveAllListeners();
        }

        // 구독자에게 알림을 전송할 함수
        private void Notify()
        {
            // _onValueChanged가 null이 아니라면 Invoke
            _onValueChanged?.Invoke(Value);
        }
    }
}
