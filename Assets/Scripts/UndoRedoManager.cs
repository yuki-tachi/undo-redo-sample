using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

public interface ICommand {
    void Do();
    void Undo();
    void Redo();
}

public class UndoRedoManager
{

    private Stack<ICommand> undo = new Stack<ICommand>();
    private Stack<ICommand> redo = new Stack<ICommand>();
    private bool canUndo = false;
    private bool canRedo = false;

    public void Do(ICommand command) {
        this.undo.Push(command);
        this.CanUndo = this.undo.Count > 0;
        command.Do();

        this.redo.Clear();
        this.CanRedo = this.redo.Count > 0;
    }

    /// <summary>
    /// 操作を実行し、かつその内容を履歴に追加します。直接引数を渡してコマンドを生成させるver
    /// </summary>
    /// <param name="doMethod">操作を行なうメソッド</param>
    /// <param name="doParamater">doMethodに必要な引数</param>
    /// <param name="undoMethod">操作を行なう前の状態に戻すためのメソッド</param>
    /// <param name="undoParamater">undoMethodに必要な引数</param>
    public void Do(Delegate doMethod, object[] doParamater, Delegate undoMethod, object[] undoParamater){
        var command = new Command(doMethod, doParamater, undoMethod, undoParamater);

        this.Do(command);
    }

    public void Undo() {
        if(this.undo.Count >= 1){
            ICommand command = this.undo.Pop();
            this.CanUndo = this.undo.Count > 0;

            command.Undo();

            this.redo.Push(command);
            this.CanRedo = this.redo.Count > 0;
        }
    }

    public void Redo (){
        if(this.redo.Count >= 1) {
            ICommand command = this.redo.Pop();
            this.CanRedo  = this.redo.Count > 0;

            command.Redo();

            this.undo.Push(command);
            this.CanUndo = this.undo.Count > 0;
        }
    }


    /// <summary>
    /// Undo出来るかどうかを返します。
    /// </summary>
    public bool CanUndo {
        private set {
            if(this.canUndo != value) {
                this.canUndo = value;

                if(this.CanUndoChange != null) {
                    this.CanUndoChange(this, EventArgs.Empty);
                }
            }
        }
        get {
            return this.canUndo;
        }
    }

    /// <summary>
    /// Redo出来るかどうかを返します。
    /// </summary>
    public bool CanRedo {
        private set {
            if(this.canRedo != value) {
                this.canRedo = value;

                if(this.CanRedoChange != null) {
                    this.CanRedoChange(this, EventArgs.Empty);
                }
            }
        }
        get {
            return this.canRedo;
        }
    }

    /// <summary>
    /// Undo出来るかどうかの状態が変化すると発生します。
    /// </summary>
    public event EventHandler CanUndoChange;

    /// <summary>
    /// Redo出来るかどうかの状態が変化すると発生します。
    /// </summary>
    public event EventHandler CanRedoChange;

    private class Command:ICommand {
        private Delegate doMethod;
        private Delegate undoMethod;
        private object[] doParamater;
        private object[] undoParamater;

        public Command(Delegate doMethod, object[] doParamater, Delegate undoMethod, object[] undoParamater) {
            this.doMethod = doMethod;
            this.doParamater = doParamater;
            this.undoMethod = undoMethod;
            this.undoParamater = undoParamater;
        }

        #region ICommand メンバ

        public void Do() {
            doMethod.DynamicInvoke(doParamater);
        }

        public void Undo() {
            undoMethod.DynamicInvoke(undoParamater);
        }

        public void Redo() {
            doMethod.DynamicInvoke(doParamater);
        }

        #endregion
    }
}
