using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    static JoyStick instance;
    Image backGround;
    Image stick;
    Vector2 touchPosition;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {

    }

    void Initialize()
    {
        backGround = GetComponent<Image>();
        stick = transform.GetChild(0).GetComponent<Image>();
        instance = this;
    }

    public static JoyStick GetInstance()
    {
        return instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backGround.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            touchPosition.x = touchPosition.x / backGround.rectTransform.sizeDelta.x;
            touchPosition.y = touchPosition.y / backGround.rectTransform.sizeDelta.y;

            // 음수, 양수
            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            // 최대 거리 제한
            if (touchPosition.magnitude > 0.5f)
                touchPosition = touchPosition.normalized * 0.5f;

            // 스틱 따라오기
            stick.rectTransform.anchoredPosition = new Vector2(
                touchPosition.x * backGround.rectTransform.sizeDelta.x/2, touchPosition.y * backGround.rectTransform.sizeDelta.y/2);

            // 실제 움직임 값은 무조건 1
            touchPosition = touchPosition.normalized;
        }
    }

    public void OnPointerUp(PointerEventData eventDakta)
    {
        stick.rectTransform.anchoredPosition = Vector2.zero;
        touchPosition = Vector2.zero;
    }

    public float GetHorizontal()
    {
        return touchPosition.x;
    }

    public float GetVertical()
    {
        return touchPosition.y;
    }
}
