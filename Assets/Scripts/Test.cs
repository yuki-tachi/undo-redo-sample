using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// https://anis774.net/codevault/undoredomanager.html のunity版
namespace Test
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private CommonSlider slider = null;

        [SerializeField]
        private Text text = null;

        [SerializeField]
        private Button undoButton = null;

        [SerializeField]
        private Button redoButton = null;

        private float prevSliderValue = 0;
        private float sliderValue = 0;

        private UndoRedoManager undoRedoManager = new UndoRedoManager();

        // Start is called before the first frame update
        void Start()
        {
            this.text.text = $"{this.slider.value}";
            this.prevSliderValue = this.slider.value;

            this.slider.onValueChanged.AddListener((value) =>
            {
                // Debug.Log(value);
                this.text.text = $"{value}";
                // this.sliderValue = value;
            });

            this.undoButton.interactable = undoRedoManager.CanUndo;
            undoRedoManager.CanUndoChange += (sender, e) => {
                this.undoButton.interactable = undoRedoManager.CanUndo;
            };

            this.redoButton.interactable = undoRedoManager.CanRedo;
            undoRedoManager.CanRedoChange += (sender, e) => {
                this.redoButton.interactable = undoRedoManager.CanRedo;
            };
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnSliderUp(float value, PointerEventData eventData) {
            AddCommand command = new AddCommand(this.slider, this.text, value, this.prevSliderValue);
            undoRedoManager.Do(command);

            this.prevSliderValue = value;
        }

        public void OnUndoButtonClick()
        {
            undoRedoManager.Undo();
        }

        public void OnRedoButtonClick()
        {
            undoRedoManager.Redo();
        }
    }

    public class AddCommand : ICommand
    {
        private Slider slider;
        private Text text;
        private float value;
        private float prevValue;
        public AddCommand(Slider slider, Text text, float value, float prevValue)
        {
            this.slider = slider;
            this.text = text;
            this.value = value;
            this.prevValue = prevValue;
        }

        public void Do()
        {
            this.slider.value = value;

            this.text.text = $"{value}";
        }

        public void Redo()
        {
            this.Do();
        }

        public void Undo()
        {
            this.slider.value = prevValue;

            this.text.text = $"{prevValue}";
        }
    }

}