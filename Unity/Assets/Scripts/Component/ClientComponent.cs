namespace Model
{
    [ObjectEvent]
    public class ClientComponentEvent : ObjectEvent<ClientComponent>, IAwake
    {
        public void Awake()
        {
            this.Get().Awake();
        }
    }

    public class ClientComponent : Component
    {
        public static ClientComponent Instance { get; private set; }

        public Player LocalPlayer { get; set; }

        public void Awake()
        {
            Instance = this;
        }
    }
}
