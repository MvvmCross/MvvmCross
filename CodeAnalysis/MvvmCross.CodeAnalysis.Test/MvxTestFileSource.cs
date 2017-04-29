namespace MvvmCross.CodeAnalysis.Test
{
    public class MvxTestFileSource
    {
        public MvxTestFileSource(string source, MvxProjType projType)
        {
            Source = source;
            ProjType = projType;
        }

        public string Source { get; }
        public MvxProjType ProjType { get; }
    }
}