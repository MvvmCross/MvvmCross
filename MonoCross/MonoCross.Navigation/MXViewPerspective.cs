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
    public class MXViewPerspective : IComparable
    {
        public MXViewPerspective(Type modelType, string perspective)
        {
            this.Perspective = perspective;
            this.ModelType = modelType;
        }
        public string Perspective { get; set; }
        public Type ModelType { get; set; }

        public int CompareTo(object obj)
        {
            MXViewPerspective p =(MXViewPerspective)obj;
            return this.GetHashCode() == p.GetHashCode() ? 0 : -1;
        }
        public static bool operator ==(MXViewPerspective a, MXViewPerspective b)
        {
            return a.CompareTo(b) == 0;
        }
        public static bool operator !=(MXViewPerspective a, MXViewPerspective b)
        {
            return a.CompareTo(b) != 0;
        }
        public override bool Equals(object obj)
        {
            return this == (MXViewPerspective)obj;
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
}
