﻿namespace Pixeval.Download.MacroParser
{
    public interface ISeekable<out T>
    {
        void Seek(int pos);

        T Peek();

        void Advance();

        void Advance(int n);

        T[] GetWindow();

        void AdvanceMarker();

        void ResetForward();

        void Return();
    }
}