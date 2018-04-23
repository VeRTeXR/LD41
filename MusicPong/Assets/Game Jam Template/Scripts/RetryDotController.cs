using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RetryDotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private Image _image;
    private bool _isPointerStillInside;
    public UiGameOverController UiGameOverController;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = new Color(0.99f, 0.50f,0.29f);
        UiGameOverController.RetryScoreText();
        _isPointerStillInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = Color.white;
        UiGameOverController.ResetScoreText();
        _isPointerStillInside = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isPointerStillInside)
        {
            Manager.Instance.StartMenu.GetComponent<StartOptions>().Retry();
//            Manager.Instance.Restart();
        }

    }
}
