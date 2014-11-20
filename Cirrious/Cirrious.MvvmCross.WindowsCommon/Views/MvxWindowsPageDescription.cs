using System;
using System.Runtime.Serialization;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    /// <summary>
    /// Describes a page in the page stack.  
    /// </summary>
    [DataContract]
    public class MvxWindowsPageDescription
    {
        private Type _type;

        internal MvxWindowsPageDescription() { } // for serialization
        public MvxWindowsPageDescription(Type pageType, object parameter)
        {
            _type = pageType;

            SerializationType = pageType.AssemblyQualifiedName;
            Parameter = parameter;
        }

        /// <summary>
        /// Gets a value indicating whether the page is instantiated. 
        /// </summary>
        public bool IsInstantiated
        {
            get { return Page != null; }
        }

        /// <summary>
        /// Gets the page type. 
        /// </summary>
        public Type Type
        {
            get
            {
                if (_type == null)
                    _type = Type.GetType(SerializationType);
                return _type;
            }
        }

        /// <summary>
        /// Gets or sets the page parameter. 
        /// </summary>
        public object Parameter { get; internal set; }

        /// <summary>
        /// Gets the page object or null if the page is not instantiated. 
        /// </summary>
        public MtPage Page { get; internal set; }

        [DataMember]
        internal string SerializationType { get; set; }

        [DataMember(Name = "Parameter")]
        internal object SerializationParameter
        {
            get
            {
                //if (Parameter == null)
                //    return null;
                //return DataContractSerialization.CanSerialize(Parameter.GetType()) ? Parameter : null;
                throw new NotImplementedException();
            }
            set { Parameter = value; }
        }

        internal MtPage GetPage(MvxWindowsFrame frame)
        {
            if (Page == null)
            {
                var page = Activator.CreateInstance(Type);
                if (!(page is MtPage))
                    throw new Exception("Base type is not a MtPage. Change the base type from Page to MtPage. ");

                Page = (MtPage)page;
                Page.Frame = frame;
            }

            return Page;
        }
    }
}