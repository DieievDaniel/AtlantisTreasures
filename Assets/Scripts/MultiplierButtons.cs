using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons; // Массив кнопок
    public Sprite[] sprites; // Массив спрайтов

    // Массив, где будут храниться предыдущие спрайты кнопок
    public Sprite[] previousSprites;

    // Вызывается при старте игры
    void Start()
    {
        // Инициализируем массив предыдущих спрайтов
        previousSprites = new Sprite[buttons.Length];

        // Запоминаем начальные спрайты кнопок
        for (int i = 0; i < buttons.Length; i++)
        {
            previousSprites[i] = buttons[i].image.sprite;
        }
    }

    // Вызывается при нажатии на кнопку
    public void OnButtonClick(int buttonIndex)
    {
        // Проверяем, чтобы индекс находился в пределах массива спрайтов и кнопок
        if (buttonIndex >= 0 && buttonIndex < buttons.Length && buttonIndex < sprites.Length)
        {
            if (buttons[buttonIndex].image.sprite == sprites[buttonIndex])
            {
                // Если текущий спрайт уже изменен, возвращаем предыдущий спрайт
                buttons[buttonIndex].image.sprite = previousSprites[buttonIndex];
            }
            else
            {
                // Сохраняем текущий спрайт как предыдущий и меняем на новый спрайт
                previousSprites[buttonIndex] = buttons[buttonIndex].image.sprite;
                buttons[buttonIndex].image.sprite = sprites[buttonIndex];
            }

            // Приводим размер кнопки к размеру нативного спрайта
            buttons[buttonIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(
                buttons[buttonIndex].image.sprite.rect.width,
                buttons[buttonIndex].image.sprite.rect.height);
        }
        else
        {
            Debug.LogWarning("Invalid button index or sprite index.");
        }
    }

    // Метод для возвращения спрайта кнопки на базовый (предыдущий)
    public void RestorePreviousSprite(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < buttons.Length && buttonIndex < previousSprites.Length)
        {
            buttons[buttonIndex].image.sprite = previousSprites[buttonIndex];
            // Приводим размер кнопки к размеру нативного спрайта
            buttons[buttonIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(
                buttons[buttonIndex].image.sprite.rect.width,
                buttons[buttonIndex].image.sprite.rect.height);
        }
        else
        {
            Debug.LogWarning("Invalid button index or sprite index.");
        }
    }
}