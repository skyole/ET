using System.Linq;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// 房间管理组件
    /// </summary>
    public class RoomManagerComponent : Component
    {
        /// <summary>
        /// 房间列表
        /// </summary>
        public readonly Dictionary<long, Room> Rooms = new Dictionary<long, Room>();

        /// <summary>
        /// 游戏中的房间列表
        /// </summary>
        public readonly Dictionary<long, Room> GameRooms = new Dictionary<long, Room>();

        /// <summary>
        /// 准备中的房间列表
        /// </summary>
        public readonly Dictionary<long, Room> ReadyRooms = new Dictionary<long, Room>();

        /// <summary>
        /// 房间轮询
        /// </summary>
        public readonly EQueue<Room> IdleRooms = new EQueue<Room>();

        /// <summary>
        /// 总房间数
        /// </summary>
        public int TotalCount { get { return this.Rooms.Count; } }

        /// <summary>
        /// 游戏中的房间数
        /// </summary>
        public int GameRoomCount { get { return GameRooms.Count; } }

        /// <summary>
        /// 准备中的房间数
        /// </summary>
        public int ReadyRoomCount { get { return ReadyRooms.Where(p => p.Value.Count < 3).Count(); } }

        /// <summary>
        /// 闲置的房间数
        /// </summary>
        public int IdleRoomCount { get { return IdleRooms.Count; } }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (this.Id == 0)
            {
                return;
            }

            base.Dispose();

            foreach (var room in this.Rooms.Values)
            {
                room.Dispose();
            }
        }
    }
}
