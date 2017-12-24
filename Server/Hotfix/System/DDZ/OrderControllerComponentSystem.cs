﻿using System;
using Model;

namespace Hotfix
{
    public static class OrderControllerComponentSystem
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="self"></param>
        public static void Init(this OrderControllerComponent self, long id)
        {
            self.FirstAuthority = new System.Collections.Generic.KeyValuePair<long, bool>(id, false);
            self.Biggest = 0;
            self.CurrentAuthority = id;
            self.SelectLordIndex = 1;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="self"></param>
        public static void Start(this OrderControllerComponent self, long id)
        {
            self.Biggest = id;
            self.CurrentAuthority = id;
        }

        /// <summary>
        /// 轮转
        /// </summary>
        /// <param name="self"></param>
        public static void Turn(this OrderControllerComponent self)
        {
            Room room = self.GetEntity<Room>();
            Gamer[] gamers = room.GetAll();
            int index = Array.FindIndex(gamers, (gamer) => self.CurrentAuthority == gamer.Id);
            index++;
            if (index == gamers.Length)
            {
                index = 0;
            }
            self.CurrentAuthority = gamers[index].Id;
        }
    }
}
