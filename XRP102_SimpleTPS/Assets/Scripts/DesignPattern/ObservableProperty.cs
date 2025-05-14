using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    // �Ӽ�(ü��, ����) �ϳ� �ϳ� �����ϱ� ���� ���� ����� �� ��
    public class ObservableProperty<T>
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            // ���� ���� ���� ������ ���ٸ� �ٷ� return
            set
            {
                if (_value.Equals(value)) return;

                // �� ���� value ����
                _value = value;

                // ���� ����Ǹ� �˸�
                Notify();
            }
        }

        // �ش� value�� ��ü���� ��, ����
        private UnityEvent<T> _onValueChanged = new();

        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        // UnityAction�� �޾Ƽ� �߰����ִ� ���·� ����
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
