#region Copyright (c) Pixeval/Pixeval
// GPL v3 License
// 
// Pixeval/Pixeval
// Copyright (c) 2022 Pixeval/MicaBackground.cs
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using WinRT;

namespace Pixeval.Interop;

public class MicaBackground
{
    private readonly Window _window;
    private MicaController _micaController = new();
    private SystemBackdropConfiguration _backdropConfiguration = new();
    private readonly WindowsSystemDispatcherQueueHelper _dispatcherQueueHelper = new();

    public MicaBackground(Window window)
    {
        _window = window;
    }

    public bool TrySetMicaBackdrop()
    {
        if (MicaController.IsSupported())
        {
            _dispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
            _window.Activated += WindowOnActivated;
            _window.Closed += WindowOnClosed;
            _backdropConfiguration.IsInputActive = true;
            _backdropConfiguration.Theme = _window.Content switch
            {
                FrameworkElement { ActualTheme: ElementTheme.Dark } => SystemBackdropTheme.Dark,
                FrameworkElement { ActualTheme: ElementTheme.Light } => SystemBackdropTheme.Light,
                FrameworkElement { ActualTheme: ElementTheme.Default } => SystemBackdropTheme.Default,
                _ => throw new InvalidOperationException("Unknown theme")
            };

            _micaController.AddSystemBackdropTarget(_window.As<ICompositionSupportsSystemBackdrop>());
            _micaController.SetSystemBackdropConfiguration(_backdropConfiguration);
            return true;
        }

        return false;
    }

    private void WindowOnClosed(object sender, WindowEventArgs args)
    {
        _micaController.Dispose();
        _micaController = null!;
        _window.Activated -= WindowOnActivated;
        _backdropConfiguration = null!;
    }

    private void WindowOnActivated(object sender, WindowActivatedEventArgs args)
    {
        _backdropConfiguration.IsInputActive = args.WindowActivationState is not WindowActivationState.Deactivated;
    }
}