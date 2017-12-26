using Model;
using UnityEngine;

namespace Hotfix
{
    public class UIEndFactory
    {
        public static UI Create(Scene scene, UIType type, UI parent, bool isWin)
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{type}.unity3d");
            GameObject bundleGameObject = resourcesComponent.GetAsset<GameObject>($"{type}.unity3d", $"{type}");
            GameObject lobby = UnityEngine.Object.Instantiate(bundleGameObject);
            lobby.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.Create<UI, Scene, UI, GameObject>(scene, null, lobby);

            ui.AddComponent<UIEndComponent, bool>(isWin);
            return ui;
        }
    }
}
