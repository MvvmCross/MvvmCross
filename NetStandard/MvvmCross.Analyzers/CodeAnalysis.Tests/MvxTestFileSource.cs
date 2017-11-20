namespace MvvmCross.CodeAnalysis.Test
{
    public class MvxTestFileSource
    {
        public string Source { get; private set; }
        public MvxProjType ProjType { get; private set; }

        public MvxTestFileSource(string source, MvxProjType projType)
        {
            Source = source;
            ProjType = projType;
        }
    }
}
