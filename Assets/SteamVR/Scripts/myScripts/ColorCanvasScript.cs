using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ColorCanvasScript : MonoBehaviour, IColorCanvas
{

  private const int MAX_SLIDER_VALUE = 100;

  private List<Slider> _slider = new List<Slider>();
  private int _currentSelect;
  private Image _sampleImage;

  // Use this for initialization
  void Start()
  {
    _slider.Add(GameObject.Find("RedSlider").GetComponent<Slider>());
    _slider.Add(GameObject.Find("BlueSlider").GetComponent<Slider>());
    _slider.Add(GameObject.Find("GreenSlider").GetComponent<Slider>());
    _sampleImage = GameObject.Find("SampleImage").GetComponent<Image>();

    _currentSelect = 0;

    foreach (var item in _slider)
    {
      item.value = 100;
    }
    _slider[_currentSelect].transform.Find("Text").GetComponent<Text>().color = Color.red;

    _sampleImage.color = new Color(_slider[0].value / MAX_SLIDER_VALUE, _slider[1].value / MAX_SLIDER_VALUE, _slider[2].value / MAX_SLIDER_VALUE);
  }

  public void ChangeHead(int delta)
  {
    if (Math.Abs(delta) != 1) return;

    _slider[_currentSelect].transform.Find("Text").GetComponent<Text>().color = Color.black;

    _currentSelect += delta;

    if (_currentSelect > _slider.Count - 1) _currentSelect = 0;
    if (_currentSelect < 0) _currentSelect = _slider.Count - 1;

    _slider[_currentSelect].transform.Find("Text").GetComponent<Text>().color = Color.red;
  }

  public void ChangeValue(int delta)
  {
    _slider[_currentSelect].value += delta;
    _sampleImage.color = new Color(_slider[0].value / MAX_SLIDER_VALUE, _slider[1].value / MAX_SLIDER_VALUE, _slider[2].value / MAX_SLIDER_VALUE);
  }

  public Color GetColor()
  {
    return new Color(_slider[0].value / MAX_SLIDER_VALUE, _slider[1].value / MAX_SLIDER_VALUE, _slider[2].value / MAX_SLIDER_VALUE);
  }
}
