#region Copyright (c) Pixeval/Pixeval
// GPL v3 License
// 
// Pixeval/Pixeval
// Copyright (c) 2022 Pixeval/UIHelper.JustifiedLayout.cs
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
using System.Collections.Generic;
using System.Linq;

namespace Pixeval.Util.UI;

public static partial class UIHelper
{
    public static IEnumerable<IEnumerable<(int top, int width, int height)>> Normalize(this IEnumerable<LayoutItem> items, int spacing, int containerWidth)
    {
        var rows = items.GroupBy(i => i.Top)
            .Select(i => i.ToArray());
        foreach (var row in rows)
        {
            if (row.Length == 1 && row.Single() is { Top: var top, Height: var height })
            {
                yield return new[] { ((int) top, containerWidth, (int) height) };
                continue;
            }

            var totalWidth = containerWidth - spacing * (row.Length - 1);
            var availableSpace = totalWidth - row.SkipLast(1).Aggregate(0d, (acc, item) => acc + item.Width);
            var last = row.Last();
            var lastMetrics = ((int, int, int)) (last.Top, Math.Max(availableSpace, last.Width), last.Height);
            yield return row.SkipLast(1).Select(i => ((int, int, int)) (i.Top, i.Width, i.Height)).Append(lastMetrics);
        }
    }
}