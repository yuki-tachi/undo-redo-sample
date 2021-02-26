using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CommonSlider))]
public class CommonSliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 各変数の型通りに表示する
        base.OnInspectorGUI();
    }

} // class RichButtonEditor
