using System.Linq;
using System.Collections.Generic;

namespace Hotfix
{
    public class GamerComponent : Component
    {
        private readonly Dictionary<long, Gamer> gamers = new Dictionary<long, Gamer>();

        public Gamer LocalGamer { get; set; }

        public void Add(Gamer gamer)
        {
            this.gamers.Add(gamer.Id, gamer);
        }

        public void Replace(long id, Gamer newGamer)
        {
            this.gamers.Remove(id);
            this.gamers.Add(newGamer.Id, newGamer);
        }

        public Gamer Get(long id)
        {
            Gamer gamer;
            this.gamers.TryGetValue(id, out gamer);
            return gamer;
        }

        public Gamer[] GetAll()
        {
            return this.gamers.Values.ToArray();
        }

        public void Remove(long id)
        {
            Gamer gamer = Get(id);
            this.gamers.Remove(id);
            gamer?.Dispose();
        }

        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();

            foreach (var gamer in this.gamers.Values)
            {
                gamer.Dispose();
            }
        }
    }
}
