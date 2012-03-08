namespace SimpleBindingDialog.Converters
{
    public class Converters
    {
        public readonly StringLengthValueConverter StringLength = new StringLengthValueConverter();
        public readonly StringReverseValueConverter StringReverse = new StringReverseValueConverter();
        public readonly FloatConverter Float = new FloatConverter();
        public readonly IntConverter Int = new IntConverter();
        public readonly IntToFloatConverter IntToFloat = new IntToFloatConverter();
    }
}