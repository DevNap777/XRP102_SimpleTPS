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
            // ���� ���� ���� ���� ���� ���ٸ� �������� �ʰ� �ٷ� return
            set
            {
                if (_value.Equals(value)) return;

                // �� ���� value ����
                _value = value;

                // ���� ����Ǹ� �˸�
                Notify();
            }
        }

        // ����Ƽ �̺�Ʈ ���
        // �ش� value�� ��ü���� ��, ����
        private UnityEvent<T> _onValueChanged = new();

        // �����ڿ��� T value�� ���� ���� �ƹ� ���� �ȵ�� �´ٸ� default
        public ObservableProperty(T value = default)
        {
            // ���´ٸ� ���� ������ ����
            _value = value;
        }

        // UnityAction�� �޾Ƽ� �߰����ִ� ���·� ����
        // ����
        public void Subscribe(UnityAction<T> action)
        {
            _onValueChanged.AddListener(action);
        }

        // ���� ����
        public void Unsubscribe(UnityAction<T> action)
        {
            _onValueChanged.RemoveListener(action);
        }

        // ��� ���� ����
        // �̹� ���ӿ����� ���� ���� �� ������.
        public void UnsubscribeAll()
        {
            _onValueChanged.RemoveAllListeners();
        }

        // �����ڿ��� �˸��� ������ �Լ�
        private void Notify()
        {
            // _onValueChanged�� null�� �ƴ϶�� Invoke
            _onValueChanged?.Invoke(Value);
        }
    }
}
