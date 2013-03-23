using System;

namespace Cirrious.MvvmCross.Platform
{
    public interface IMvxAssociatedTypeFinder
    {
        Type FindAssociatedTypeOrNull(Type candidateType);
    }

    public interface IMvxAssociatedViewModelTypeFinder
        : IMvxAssociatedTypeFinder
    {
    }
}