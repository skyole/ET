namespace Model
{
    public sealed class Player : Entity
    {
        public long UserID { get; set; }

        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();
        }
    }
}