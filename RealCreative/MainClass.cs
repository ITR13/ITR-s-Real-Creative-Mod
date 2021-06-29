using MelonLoader;
using UnityEngine;

namespace RealCreative
{
    public class MainClass : MelonMod
    {
        private GameObject _tempTest;
        public override void OnUpdate()
        {
            if (_tempTest != null) return;
            _tempTest = UiInjector.CreateSettingsButton(
                "Test",
                () => { MelonLogger.Msg("Button was clicked"); }
            );

            if (_tempTest == null) return;
            MelonLogger.Msg("Created button");
        }
    }
}
