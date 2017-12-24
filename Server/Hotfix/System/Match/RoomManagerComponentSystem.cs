using System.Linq;
using System.Collections.Generic;
using Model;

namespace Hotfix
{
    public static class RoomManagerComponentSystem
    {
        public static void Add(this RoomManagerComponent self, Room room)
        {
            self.Rooms.Add(room.Id, room);
            self.IdleRooms.Enqueue(room);
        }

        public static void Recycle(this RoomManagerComponent self, long id)
        {
            Room room = self.ReadyRooms[id];
            self.ReadyRooms.Remove(room.Id);
            self.IdleRooms.Enqueue(room);
        }

        public static Room Get(this RoomManagerComponent self, long id)
        {
            Room room;
            self.Rooms.TryGetValue(id, out room);
            return room;
        }

        public static Room GetReadyRoom(this RoomManagerComponent self)
        {
            return self.ReadyRooms.Where(p => p.Value.Count < 3).FirstOrDefault().Value;
        }

        public static Room GetIdleRoom(this RoomManagerComponent self)
        {
            if (self.IdleRoomCount > 0)
            {
                Room room = self.IdleRooms.Dequeue();
                self.ReadyRooms.Add(room.Id, room);
                return room;
            }
            else
            {
                return null;
            }
        }

        public static void RoomStartGame(this RoomManagerComponent self, long id)
        {
            Room room = self.ReadyRooms[id];
            self.ReadyRooms.Remove(id);
            self.GameRooms.Add(room.Id, room);
        }

        public static void RoomEndGame(this RoomManagerComponent self, long id)
        {
            Room room = self.GameRooms[id];
            self.GameRooms.Remove(id);
            self.ReadyRooms.Add(room.Id, room);
        }
    }
}
