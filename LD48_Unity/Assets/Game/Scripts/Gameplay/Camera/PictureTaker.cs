using Cysharp.Threading.Tasks;
using DUCK.Tween;
using DUCK.Tween.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD48.Gameplay.Camera
{
    public class PictureTaker : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup cameraGroup;

        [SerializeField]
        private CanvasGroup blackScreen;

        [SerializeField]
        private UnityEngine.Camera pictureCamera;

        [SerializeField]
        private float fieldOfView = 30f;

        private bool isActive;
        private float defaultFOV;
        private UnityEngine.Camera camera;
        private InputAction cameraAction;
        private InputAction captureAction;
        private SequencedAnimation activationAnimation;
        private SequencedAnimation deactivationAnimation;
        private SequencedAnimation captureAnimation;

        [SerializeField]
        private Texture2D texture;

        private void Start()
        {
            camera = UnityEngine.Camera.main;
            defaultFOV = camera.fieldOfView;

            pictureCamera.fieldOfView = fieldOfView;

            var map = new InputActionMap("Picture Taker");

            cameraAction = map.AddAction("Vertical Movement");
            cameraAction.AddCompositeBinding("Dpad")
                .With("Up", "<Keyboard>/tab")
                .With("Up", "<Gamepad>/buttonNorth");


            captureAction = map.AddAction("Capture", InputActionType.Button, "<Mouse>/leftButton");

            activationAnimation = new SequencedAnimation();
            activationAnimation.Fade(blackScreen, 0f,1f, 0.2f);
            activationAnimation.Invoke(()=>
            {
                camera.fieldOfView = fieldOfView;
            });
            activationAnimation.Fade(cameraGroup, 0f,1f, 0f);
            activationAnimation.Fade(blackScreen, 1f,0f, 0.2f);

            deactivationAnimation = new SequencedAnimation();
            deactivationAnimation.Fade(blackScreen, 0f,1f, 0.2f);

            deactivationAnimation.Invoke(()=>
            {
                camera.fieldOfView = defaultFOV;
            });
            deactivationAnimation.Fade(cameraGroup, 1f,0f, 0f);
            deactivationAnimation.Fade(blackScreen, 1f,0f, 0.2f);


            captureAnimation = new SequencedAnimation();
            captureAnimation.Fade(blackScreen, 0f, 1f, 0.1f);
            captureAnimation.Invoke(TakeScreenshot);
            captureAnimation.Fade(blackScreen, 1f, 0f, 0.1f);

            cameraAction.performed += HandleActivate;
            cameraAction.Enable();

            captureAction.performed += HandleCapture;
            captureAction.Enable();
        }

        private void HandleCapture(InputAction.CallbackContext obj)
        {
            Debug.Log("Cap");
            if (!isActive || activationAnimation.IsPlaying || deactivationAnimation.IsPlaying || captureAnimation.IsPlaying) return;
            Capture();
        }

        private void HandleActivate(InputAction.CallbackContext obj)
        {
            if (activationAnimation.IsPlaying || deactivationAnimation.IsPlaying || captureAnimation.IsPlaying) return;

            if (!isActive)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            activationAnimation.Play();
            isActive = true;
        }

        private void Deactivate()
        {
            deactivationAnimation.Play();
            isActive = false;
        }

        private void Capture()
        {
            captureAnimation.Play();
        }

        private async void TakeScreenshot()
        {
           pictureCamera.Render
        }

    }
}
