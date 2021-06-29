using System.Diagnostics;
using MelonLoader;
using UnityEngine;

namespace RealCreative
{
    public class MainClass : MelonMod
    {
        private GameObject _generateButton, _creativeSettings;

        public override void OnApplicationQuit()
        {
            ConfigWatcher.Unload();
        }

        public override void OnUpdate()
        {
            // Not in an active game
            if(OtherInput.Instance == null) return;

            CheckGenerateButton();
            if(!LocalClient.serverOwner) return;
            CheckOpenSettingsButton();
            ConfigWatcher.UpdateIfDirty();
            InputHandler.DoInput();
        }

        private void CheckGenerateButton()
        {
            if (_generateButton != null) return;

            _generateButton = UiInjector.CreatePauseMenuButton(
                "Generate ids",
                DataExtractor.GenerateAndSaveAndOpen
            );

            _generateButton.transform.SetSiblingIndex(2);
        }

        private void CheckOpenSettingsButton()
        {
            if (_creativeSettings != null) return;

            _creativeSettings = UiInjector.CreatePauseMenuButton(
                "Creative",
                () => Process.Start(ConfigWatcher.FullPath)
            );

            _creativeSettings.transform.SetSiblingIndex(_creativeSettings.transform.parent.childCount - 2);
        }
    }
}
