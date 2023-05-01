namespace YagizAyer.Root.Scripts.Helpers
{
    public enum UnityNoParamEvents
    {
        Null = 0,
        Awake = 10,
        Start = 20,
        Update = 30,
        FixedUpdate = 40,
        LateUpdate = 50,
        OnEnable = 60,
        OnDisable = 70,
        OnDestroy = 80,
        OnBecameVisible = 90,
        OnBecameInvisible = 100,
    }
    
    public enum UnityTriggerEvents
    {
        Null = 0,
        OnTriggerEnter = 10,
        OnTriggerStay = 20,
        OnTriggerExit = 30,
    }
    
    public enum UnityCollisionEvents
    {
        Null = 0,
        OnCollisionEnter = 10,
        OnCollisionStay = 20,
        OnCollisionExit = 30,
    }
}