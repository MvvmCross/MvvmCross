#region Copyright

// <copyright file="AndroidDialogEnumHelper.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections.Generic;
using Android.Text;
using Android.Views.InputMethods;

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