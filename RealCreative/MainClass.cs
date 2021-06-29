using System.Xml.Serialization;
using MelonLoader;
using UnityEngine;

namespace RealCreative
{
    public class MainClass : MelonMod
    {
        private GameObject _generateButton;

        public override void OnUpdate()
        {
            CheckGenerateButton();
        }

        private void CheckGenerateButton()
        {
            if (_generateButton != null) return;

            _generateButton = UiInjector.CreateSettingsButton(
                "Generate ids",
                DataExtractor.GenerateAndSaveAndOpen
            );
        }
    }
}
