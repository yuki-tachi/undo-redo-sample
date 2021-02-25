using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEditor;

[CustomEditor( typeof( Slider ) )]
public class CommonSliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 各変数の型通りに表示する
        base.OnInspectorGUI();
    }

} // class RichButtonEditor

// IPointerはdownを必ず実装すること https://qiita.com/butislime/items/24610330812580cf5eb2
public class CommonSlider : Slider, IPointerDownHandler, IPointerUpHandler
{
    // インスペクタ上に表示させるためSerializableしてジェネリッククラスを作っている
    // https://takuplog.com/unity-arguments-unityevent/
    [Serializable]
    public class PointerUp : UnityEvent<float, PointerEventData> { };

    [SerializeField]
    public PointerUp OnPointerUpEvent = new PointerUp();

    [Serializable]
    public class PointerDown : UnityEvent<float, PointerEventData> { };

    [SerializeField]
    public PointerDown OnPointerDownEvent = new PointerDown();

    public void OnPointerDown(PointerEventData eventData)
    {
        this.OnPointerDownEvent?.Invoke(this.value, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.OnPointerUpEvent?.Invoke(this.value, eventData);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
