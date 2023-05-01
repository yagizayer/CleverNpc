namespace YagizAyer.Root.Scripts.EventHandling.BasicPassableData
{
    public class GenericPassableData<T> : IPassableData
    {
        public GenericPassableData(T value) => Value = value;
        public T Value { get; }
    }
}