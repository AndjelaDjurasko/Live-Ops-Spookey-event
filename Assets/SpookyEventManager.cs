using UnityEngine;
using UnityEngine.UIElements;

public class SpookyEventManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    [Header("Ubaci ovde sve svoje UXML ekrane redom")]
    [SerializeField] private VisualTreeAsset[] screens;
    private int currentIndex = 0;

    private VisualElement iconElement;
    private float rotationSpeed = 3f;
    private float maxAngle = 8f;

    private void Start()
    {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
        ShowCurrentScreen();
    }

    private void Update()
    {
        // 1. Deo za rotaciju ikonice
        if (iconElement != null)
        {
            float ugao = Mathf.Sin(Time.time * rotationSpeed) * maxAngle;
            iconElement.style.rotate = new StyleRotate(new Rotate(new Angle(ugao, AngleUnit.Degree)));
        }
    }

    private void ShowCurrentScreen()
    {
        if (screens == null || screens.Length == 0 || uiDocument == null) return;

        // Učitava trenutni ekran
        uiDocument.visualTreeAsset = screens[currentIndex];

        // Traži ikonicu na tom ekranu da bi je vrteli
        iconElement = uiDocument.rootVisualElement.Q<VisualElement>("Icon_");

        // Traži glavno dugme i kači mu akciju za prelazak na sledeći ekran
        Button actionButton = uiDocument.rootVisualElement.Q<Button>();
        if (actionButton != null)
        {
            actionButton.clicked -= OnNextScreenClicked; // sprečava duple klikove
            actionButton.clicked += OnNextScreenClicked;
        }
    }

    private void OnNextScreenClicked()
    {
        // Prebacuje na sledeći ekran u nizu (ide u krug)
        currentIndex = (currentIndex + 1) % screens.Length;
        ShowCurrentScreen();
    }
}

