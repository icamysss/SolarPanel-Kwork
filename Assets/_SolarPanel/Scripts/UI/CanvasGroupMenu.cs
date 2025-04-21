using UnityEngine;

namespace _SolarPanel.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class CanvasGroupMenu : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void Show(bool show = true)
        {
            UIManager.Show(canvasGroup, show);
        }
    }
}