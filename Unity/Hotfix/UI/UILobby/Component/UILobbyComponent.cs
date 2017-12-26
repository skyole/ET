using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    [ObjectEvent]
    public class UILobbyComponentEvent : ObjectEvent<UILobbyComponent>, IAwake
    {
        public void Awake()
        {
            this.Get().Awake();
        }
    }

    public class UILobbyComponent: Component
    {
        public void Awake()
        {
            Init();
        }

        public async void OnStartMatch()
        {
            try
            {

            }
            catch (Exception e)
            {
                Log.Error(e.ToStr());
            }
        }

        private async void Init()
        {
           
        }
    }
}
