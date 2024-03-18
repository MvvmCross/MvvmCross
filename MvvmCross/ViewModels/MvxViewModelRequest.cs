// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Text;
using MvvmCross.Core;

namespace MvvmCross.ViewModels;

public class MvxViewModelRequest
{
    public MvxViewModelRequest()
    {
    }

    public MvxViewModelRequest(Type viewModelType)
    {
        ViewModelType = viewModelType;
    }

    public MvxViewModelRequest(Type viewModelType,
        IMvxBundle? parameterBundle,
        IMvxBundle? presentationBundle)
    {
        ViewModelType = viewModelType;
        ParameterValues = parameterBundle.SafeGetData();
        PresentationValues = presentationBundle.SafeGetData();
    }

    public Type? ViewModelType { get; set; }
    public IDictionary<string, string>? ParameterValues { get; set; }
    public IDictionary<string, string>? PresentationValues { get; set; }

    public static MvxViewModelRequest GetDefaultRequest(Type viewModelType)
    {
        return new MvxViewModelRequest(viewModelType, null, null);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"MvxViewModelRequest - ViewModelType: '{ViewModelType}'");
        if (ParameterValues != null)
        {
            sb.Append(
                $", ParameterValues: '{string.Join(", ", ParameterValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
        }

        if (PresentationValues != null)
        {
            sb.Append(
                $", PresentationValues: '{string.Join(", ", PresentationValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
        } 

        return sb.ToString();
    }
}

public class MvxViewModelRequest<TViewModel> : MvxViewModelRequest where TViewModel : IMvxViewModel
{
    public MvxViewModelRequest() : base(typeof(TViewModel))
    {
    }

    public MvxViewModelRequest(IMvxBundle? parameterBundle, IMvxBundle? presentationBundle)
        : base(typeof(TViewModel), parameterBundle, presentationBundle)
    {
    }

    public static MvxViewModelRequest GetDefaultRequest()
    {
        return GetDefaultRequest(typeof(TViewModel));
    }
}
