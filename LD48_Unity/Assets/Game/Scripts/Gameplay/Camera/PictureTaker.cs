using System;
using DUCK.Tween;
using DUCK.Tween.Extensions;
using LD48.Core;
using LD48.Gameplay.UI;
using LD48.Save;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LD48.Gameplay.Camera
{
    public class PictureTaker : MonoBehaviour
    {
        public static PictureTaker Instance { get; private set; }
        public static bool IsActive { get; private set; }

        [SerializeField]
        private TextMeshProUGUI entityName;

        [SerializeField]
        private CanvasGroup cameraGroup;

        [SerializeField]
        private CanvasGroup blackScreen;

        [SerializeField]
        private UnityEngine.Camera pictureCamera;

        [SerializeField]
        RenderTexture renderTexture;

        [SerializeField]
        private RawImage rawImage;

        [SerializeField]
        private float fieldOfView = 30f;

        [SerializeField]
        private RenderTextureFormat renderTextureFormat;

        private float defaultFOV;
        private UnityEngine.Camera camera;
        private InputAction cameraAction;
        private InputAction captureAction;
        private SequencedAnimation activationAnimation;
        private SequencedAnimation deactivationAnimation;
        private SequencedAnimation captureAnimation;
        private bool isNewCapture;

        private void Start()
        {
            Instance = this;

            pictureCamera.targetTexture.height = Screen.height;
            pictureCamera.targetTexture.width = Screen.width;

            pictureCamera.enabled = false;
            rawImage.gameObject.SetActive(false);

            camera = UnityEngine.Camera.main;
            defaultFOV = camera.fieldOfView;

            pictureCamera.fieldOfView = fieldOfView;

            var map = new InputActionMap("Picture Taker");

            cameraAction = map.AddAction("Vertical");
            cameraAction.AddCompositeBinding("Dpad")
                .With("Up", "<Mouse>/rightButton")
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
            captureAnimation.Invoke(() =>
            {
                rawImage.gameObject.SetActive(true);
                EntityCaster.Instance.ForceDisplayName(true);
            });
            captureAnimation.Fade(blackScreen, 1f, 0f, 0.1f);
            captureAnimation.Wait(1.5f);
            captureAnimation.Invoke(() =>
            {
                rawImage.gameObject.SetActive(false);
                EntityCaster.Instance.ForceDisplayName(false);
                if (isNewCapture)
                {
                    DeactivateInstant();
                    Journal.Instance.Activate(EntityCaster.CurrentActiveEntity);
                }
                else
                {
                    Time.timeScale = 1f;
                }

            });
            cameraAction.performed += HandleActivate;
            cameraAction.Enable();

            captureAction.performed += HandleCapture;
            captureAction.Enable();
        }

        private void HandleCapture(InputAction.CallbackContext obj)
        {
            if (!IsActive || activationAnimation.IsPlaying || deactivationAnimation.IsPlaying || captureAnimation.IsPlaying) return;
            Capture();
        }

        private void HandleActivate(InputAction.CallbackContext obj)
        {
            if (activationAnimation.IsPlaying || deactivationAnimation.IsPlaying || captureAnimation.IsPlaying) return;

            if (!IsActive)
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
            if (GameCore.Instance.PlayerBusy) return;

            activationAnimation.Play();
            IsActive = true;
            GameCore.Instance.PlayerBusy = true;
        }

        private void Deactivate(Action onComplete = null)
        {
            deactivationAnimation.Play(() =>
            {
                IsActive = false;
                GameCore.Instance.PlayerBusy = false;
                onComplete?.Invoke();
            });
        }

        private void DeactivateInstant()
        {
            cameraGroup.alpha = 0f;
            blackScreen.alpha = 0f;
            camera.fieldOfView = defaultFOV;
            IsActive = false;
            GameCore.Instance.PlayerBusy = false;
        }

        private void Capture()
        {
            captureAnimation.Play();
        }

        public void TakeScreenshot()
        {
            pictureCamera.backgroundColor = camera.backgroundColor;

            var mRt = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, renderTextureFormat, RenderTextureReadWrite.sRGB) {antiAliasing = renderTexture.antiAliasing};

            var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
            pictureCamera.targetTexture = mRt;
            pictureCamera.Render();
            RenderTexture.active = mRt;

            tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
            tex.Apply();

            rawImage.texture = tex;

            isNewCapture = EntityCaster.CurrentActiveEntity != null && SaveData.Instance.SaveCapture(EntityCaster.CurrentActiveEntity, tex);

            pictureCamera.targetTexture = renderTexture;
            pictureCamera.Render();
            RenderTexture.active = renderTexture;

            DestroyImmediate(mRt);

            SaveData.Instance.SaveGame();

        }

        private void OnDestroy()
        {
            cameraAction.performed -= HandleActivate;
            captureAction.performed -= HandleCapture;
        }
    }
}
