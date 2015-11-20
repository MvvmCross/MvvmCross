// AndroidDialogEnumHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Text;
using Android.Views.InputMethods;
using System.Collections.Generic;

namespace CrossUI.Droid.Dialog.Enums
{
    public static class AndroidDialogEnumHelper
    {
        public static readonly Dictionary<UIKeyboardType, InputTypes> KeyboardTypeMap = new Dictionary
            <UIKeyboardType, InputTypes>
            {
                {UIKeyboardType.Default, InputTypes.ClassText},
                {
                    UIKeyboardType.DecimalPad,
                    InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned
                },
                {UIKeyboardType.NumberPad, InputTypes.ClassNumber},
                {UIKeyboardType.PhonePad, InputTypes.ClassPhone},
                {UIKeyboardType.NamePhonePad, InputTypes.TextVariationPersonName | InputTypes.ClassText},
                {UIKeyboardType.ASCIICapable, InputTypes.TextVariationVisiblePassword | InputTypes.ClassText},
                {UIKeyboardType.NumbersAndPunctuation, InputTypes.TextVariationVisiblePassword | InputTypes.ClassText},
                {UIKeyboardType.EmailAddress, InputTypes.TextVariationEmailAddress | InputTypes.ClassText},
                {UIKeyboardType.NoAutoCorrect, InputTypes.TextFlagNoSuggestions | InputTypes.ClassText},
            };

        public static readonly Dictionary<UIReturnKeyType, ImeAction> ReturnKeyTypeMap = new Dictionary
            <UIReturnKeyType, ImeAction>
            {
                {UIReturnKeyType.Default, ImeAction.Unspecified},
                {UIReturnKeyType.Done, ImeAction.Done},
                {UIReturnKeyType.Go, ImeAction.Go},
                {UIReturnKeyType.Next, ImeAction.Next},
                {UIReturnKeyType.Search, ImeAction.Search},
                {UIReturnKeyType.Send, ImeAction.Send},
            };

        public static ImeAction ImeActionFromUIReturnKeyType(this UIReturnKeyType returnKeyType)
        {
            return ReturnKeyTypeMap.ContainsKey(returnKeyType) ? ReturnKeyTypeMap[returnKeyType] : ImeAction.Unspecified;
        }

        public static InputTypes InputTypesFromUIKeyboardType(this UIKeyboardType keyboardType)
        {
            return KeyboardTypeMap.ContainsKey(keyboardType) ? KeyboardTypeMap[keyboardType] : InputTypes.ClassText;
        }
    }
}