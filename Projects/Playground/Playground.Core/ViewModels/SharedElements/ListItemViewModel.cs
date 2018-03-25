// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Playground.Core.ViewModels
{
    public class ListItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public override string ToString()
            => Title;
    }
}
