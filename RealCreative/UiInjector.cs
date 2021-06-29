using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace RealCreative
{
    public static class UiInjector
    {
        public static GameObject CreateSettingsButton(string text, UnityAction onClick)
        {
            var otherInput = OtherInput.Instance;
            if (otherInput == null) return null;

            var clone = CloneResume(otherInput);
            if (clone == null) return null;

            if (SetupButton(clone, text, onClick)) return clone;

            Object.Destroy(clone);
            return null;
        }

        private static GameObject CloneResume(OtherInput otherInput)
        {
            if (otherInput.pauseUi == null)
            {
                MelonLogger.Error("pauseUi not assigned");
                return null;
            }

            var pauseTransform = otherInput.pauseUi.transform;
            var originalChild = pauseTransform.GetChild(0);
            if (originalChild == null)
            {
                MelonLogger.Error("Failed to find children on pauseUi");
                return null;
            }

            return Object.Instantiate(originalChild.gameObject, pauseTransform.transform);
        }

        private static bool SetupButton(GameObject root, string text, UnityAction onClick)
        {
            root.name = text + "Btn";
            var textComponent = root.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent == null)
            {
                MelonLogger.Error("Failed to find text component for resume button");
                return false;
            }

            textComponent.text = text;

            // Doesn't need inChildren, just there for future-proofing
            var buttonComponent = root.GetComponentInChildren<Button>();
            if (buttonComponent == null)
            {
                MelonLogger.Error("Failed to find button component for resume button");
                return false;
            }

            // For some reason this doesn't work
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(onClick);
            return true;
        }
    }
}
