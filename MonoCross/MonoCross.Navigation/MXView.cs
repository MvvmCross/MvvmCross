using System;

namespace MonoCross.Navigation
{
	public delegate void ModelEventHandler(object model);
	
    #region IMXView interface
    public interface IMXView
    {
        Type ModelType { get; }
        void SetModel(object model);

        void Render();
    }
    #endregion

    public abstract class MXView<T> : IMXView
    {
		public Type ModelType { get { return typeof(T); } }
        public virtual void SetModel(object model)
        {
            Model = (T)model;
        }

        public virtual void Render() { }
		
		public virtual T Model { get; set; }
    }
}
