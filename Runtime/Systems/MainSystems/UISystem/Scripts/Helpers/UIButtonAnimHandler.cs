using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NilinhoGames.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonAnimHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Button button;

        // Inspector variables for customization
        [Header("Enable Animations")]
        [SerializeField] private bool enableScaleAnimation;
        [SerializeField] private bool enableColorAnimation;
        [SerializeField] private bool enableSpriteChange;
        [SerializeField] private bool enableSoundEffects;

        [Header("Scale Animation Settings")]
        [SerializeField] private Vector3 hoverScale;
        [SerializeField] private Vector3 pressedScale;
        [SerializeField] private float scaleAnimationDuration;

        [Header("Color Animation Settings")]
        [SerializeField] private Color hoverColor;
        [SerializeField] private Color pressedColor;
        [SerializeField] private float colorAnimationDuration;

        [Header("Sprite Settings")]
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private Sprite pressedSprite;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip pressSound;
        [SerializeField] private AudioSource audioSource;

        private Vector3 originalScale;
        private Color originalColor;
        private Image buttonImage;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonImage = GetComponent<Image>();
            originalScale = transform.localScale;
            originalColor = buttonImage.color;

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableScaleAnimation)
                AnimateScale(hoverScale);
            if (enableColorAnimation)
                AnimateColor(hoverColor);
            if (enableSpriteChange && hoverSprite != null)
                buttonImage.sprite = hoverSprite;
            if (enableSoundEffects && hoverSound != null)
                audioSource.PlayOneShot(hoverSound);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (enableScaleAnimation)
                AnimateScale(originalScale);
            if (enableColorAnimation)
                AnimateColor(originalColor);
            if (enableSpriteChange && normalSprite != null)
                buttonImage.sprite = normalSprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (enableScaleAnimation)
                AnimateScale(pressedScale);
            if (enableColorAnimation)
                AnimateColor(pressedColor);
            if (enableSpriteChange && pressedSprite != null)
                buttonImage.sprite = pressedSprite;
            if (enableSoundEffects && pressSound != null)
                audioSource.PlayOneShot(pressSound);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (enableScaleAnimation)
                AnimateScale(hoverScale);
            if (enableColorAnimation)
                AnimateColor(hoverColor);
            if (enableSpriteChange && hoverSprite != null)
                buttonImage.sprite = hoverSprite;
        }

        private async UniTask AnimateScale(Vector3 targetScale)
        {
            float timeElapsed = 0f;
            Vector3 initialScale = transform.localScale;

            while (timeElapsed < scaleAnimationDuration)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / scaleAnimationDuration);
                timeElapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            transform.localScale = targetScale;
        }

        private async UniTask AnimateColor(Color targetColor)
        {
            float timeElapsed = 0f;
            Color initialColor = buttonImage.color;

            while (timeElapsed < colorAnimationDuration)
            {
                buttonImage.color = Color.Lerp(initialColor, targetColor, timeElapsed / colorAnimationDuration);
                timeElapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            buttonImage.color = targetColor;
        }
    }
}
