namespace YagizAyer.Root.Scripts.EventHandling.BasicPassableData
{
    public class PassableDataBase<T> : IPassableData
    {
        public PassableDataBase(T value) => Value = value;
        public T Value { get; }
    }
}