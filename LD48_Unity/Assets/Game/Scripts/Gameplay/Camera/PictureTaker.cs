using Cysharp.Threading.Tasks;
using DUCK.Tween;
using DUCK.Tween.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        private RawImage rawImage;

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
            pictureCamera.targetTexture.height = Screen.height;
            pictureCamera.targetTexture.width = Screen.width;

            pictureCamera.enabled = false;
            rawImage.gameObject.SetActive(false);

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
            captureAnimation.Invoke(() => { Time.timeScale = 0f; });
            captureAnimation.Fade(blackScreen, 0f, 1f, 0.1f);
            captureAnimation.Invoke(TakeScreenshot);
            captureAnimation.Invoke(() => { rawImage.gameObject.SetActive(true); });
            captureAnimation.Fade(blackScreen, 1f, 0f, 0.1f);
            captureAnimation.Wait(2f);
            captureAnimation.Invoke(() => { Time.timeScale = 1f; });
            captureAnimation.Invoke(() => { rawImage.gameObject.SetActive(false); });

            cameraAction.performed += HandleActivate;
            cameraAction.Enable();

            captureAction.performed += HandleCapture;
            captureAction.Enable();
        }

        private void HandleCapture(InputAction.CallbackContext obj)
        {
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
            pictureCamera.enabled = true;
            pictureCamera.backgroundColor = camera.backgroundColor;
            await UniTask.NextFrame();

            rawImage.texture = ToTexture2D(pictureCamera.targetTexture);
        }

        private static Texture2D ToTexture2D(RenderTexture rTex)
        {
            var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, true);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}
