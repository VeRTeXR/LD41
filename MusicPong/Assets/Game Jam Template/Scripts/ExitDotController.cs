using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitDotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private Image _image;
    private bool _isPointerStillInside;
    
    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = new Color(0.99f, 0.50f,0.29f);
        _isPointerStillInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = new Color(0.21f, 0.26f, 0.21f);
        _isPointerStillInside = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isPointerStillInside)
        {
           Application.Quit();
        }

    }
}
