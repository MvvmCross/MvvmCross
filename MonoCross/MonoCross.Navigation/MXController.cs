using System;
using System.Collections.Generic;
using MonoCross.Navigation.ActionResults;

namespace MonoCross.Navigation
{
    public interface IMXController
    {
        Dictionary<string, string> Parameters { get; set; }
        String Uri { get; set; }
        IMXView View { get; set; }
        Type ModelType { get; }
        object GetModel();

        IMXActionResult Load(Dictionary<string, string> parameters);
        void RenderView();
    }

    public abstract class MXController<T> : IMXController
    {
        public string Uri { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }

        public virtual IMXView View { get; set; }

		public object GetModel() { return Model; }
        public abstract IMXActionResult Load(Dictionary<string, string> parameters);
        public virtual void RenderView() { if (View != null) View.Render(); }
    }
}
