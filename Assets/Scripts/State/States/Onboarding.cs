using System.Threading.Tasks;
using UnityEngine;

namespace State.States
{
    /// <summary>
    /// Starting State. All initial bootstrapping goes here.
    /// </summary>
    internal class Onboarding : State
    {
        public static Onboarding Instance { get; } = new Onboarding();

        private Onboarding() {}

        public override void OnEnter()
        {
            Debug.Log("[Onboarding] OnEnter");
            MockingPreloadingAssetsAsync();
        }

        public override void OnExit()
        {
            Debug.Log("[Onboarding] OnExit");
        }
        
        private static async Task MockingPreloadingAssetsAsync()
        {
            Debug.Log("[Onboarding] Checking DLC... (Mocking");
            await Task.Delay(2000);
            Debug.Log("[Onboarding] DLC Downloaded (Mocking");
            StateManager.SetState(MainMenu.Instance);
        }

        // For Mobile Devices, we could check Hardware compatibilities
        private void OnARUnsupported()
        {
            Debug.Log("[Onboarding] OnArUnsupported");
        }
        
    }
}
