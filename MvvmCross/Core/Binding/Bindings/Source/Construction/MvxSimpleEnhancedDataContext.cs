namespace MvvmCross.Binding.Bindings.Source.Construction
{
#warning TODO - this is in the wrong namespace (blame R#)
#warning TODO - should this be a struct... it's asking for problems...
    public class MvxSimpleEnhancedDataContext
        : IMvxEnhancedDataContext
    {
        public object Core { get; private set; }
        public object Parent { get; private set; }

        private MvxSimpleEnhancedDataContext()
        {
            
        }

        public static IMvxEnhancedDataContext CreateEmpty()
        {
            return new MvxSimpleEnhancedDataContext();
        }

        public static IMvxEnhancedDataContext Clone(IMvxEnhancedDataContext value)
        {
            return FromCoreAndParent(value?.Core, value?.Parent);
        }

        public static IMvxEnhancedDataContext FromCoreAndParent(object core, object parent)
        {
            return new MvxSimpleEnhancedDataContext()
            {
                Core = core,
                Parent = parent
            };
        }
    }
}