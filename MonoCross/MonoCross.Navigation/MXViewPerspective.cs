using System;

namespace MonoCross.Navigation
{
    public static class ViewPerspective
    {
        public const string Default = "";
        public const string Read = "GET";
        public const string Create = "POST";
        public const string Update = "PUT";
        public const string Delete = "DELETE";
    }

    /*
    public class MXViewPerspectiveOld : IComparable
    {
        public MXViewPerspectiveOld(Type modelType, string perspective)
        {
            this.Perspective = perspective;
            this.ModelType = modelType;
        }
        public string Perspective { get; set; }
        public Type ModelType { get; set; }

        public int CompareTo(object obj)
        {
            MXViewPerspectiveOld p =(MXViewPerspectiveOld)obj;
            return this.GetHashCode() == p.GetHashCode() ? 0 : -1;
        }
        public static bool operator ==(MXViewPerspectiveOld a, MXViewPerspectiveOld b)
        {
            return a.CompareTo(b) == 0;
        }
        public static bool operator !=(MXViewPerspectiveOld a, MXViewPerspectiveOld b)
        {
            return a.CompareTo(b) != 0;
        }
        public override bool Equals(object obj)
        {
            return this == (MXViewPerspectiveOld)obj;
        }
        public override int GetHashCode()
        {
            return this.ModelType.GetHashCode() ^ this.Perspective.GetHashCode();
        }
        
        public override string ToString()
        {
            return string.Format("Model \"{0}\" with perspective  \"{1}\"", ModelType, Perspective);
        }
    }

    public class MXViewPerspective
    {
        public string Perspective { get; private set; }
        public Type ViewModelType { get; private set; }

        public MXViewPerspective(Type viewModelType, string perspective)
        {
            ViewModelType = viewModelType;
            Perspective = perspective;
        }
    }

    public class MXViewPerspective<TViewModel> : MXViewPerspective
    {
        public MXViewPerspective(string perspective)
            : base(typeof(TViewModel), perspective)
        {
        }
    }
     */
}
