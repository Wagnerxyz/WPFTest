using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTest
{
    public static class TextBoxValidator
    {
        /// <summary>
        ///     TextBox Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty.RegisterAttached(
            "IsNumericOnly",
            typeof(bool),
            typeof(TextBoxValidator),
            new UIPropertyMetadata(false, OnIsNumericOnlyChanged));

        public static readonly DependencyProperty NumberPropertyProperty =
            DependencyProperty.RegisterAttached("NumberProperty", typeof(int), typeof(TextBoxValidator),
                new PropertyMetadata(0));

        public static readonly DependencyProperty MaxPropertyProperty =
            DependencyProperty.RegisterAttached("MaxProperty", typeof(int), typeof(TextBoxValidator),
                new PropertyMetadata(0));

        public static readonly DependencyProperty MinPropertyProperty =
            DependencyProperty.RegisterAttached("MinProperty", typeof(int), typeof(TextBoxValidator),
                new PropertyMetadata(0));


        public static readonly DependencyProperty RuleProperty = DependencyProperty.RegisterAttached(
            "Rule",
            typeof(ValidationRuleBase),
            typeof(TextBoxValidator),
            new UIPropertyMetadata(RuleValidate));

        public static readonly DependencyProperty LengthPropertyProperty =
            DependencyProperty.RegisterAttached("LengthProperty", typeof(int), typeof(TextBoxValidator));


        public static int GetNumber(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberPropertyProperty);
        }

        public static void SetNumber(DependencyObject obj, int value)
        {
            obj.SetValue(NumberPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for NumberProperty.  This enables animation, styling, binding, etc...


        public static int GetMax(DependencyObject obj)
        {
            return (int)obj.GetValue(MaxPropertyProperty);
        }

        public static void SetMax(DependencyObject obj, int value)
        {
            obj.SetValue(MaxPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for MaxProperty.  This enables animation, styling, binding, etc...


        public static int GetMin(DependencyObject obj)
        {
            return (int)obj.GetValue(MinPropertyProperty);
        }

        public static void SetMin(DependencyObject obj, int value)
        {
            obj.SetValue(MinPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for MinProperty.  This enables animation, styling, binding, etc...

        public static int GetLength(DependencyObject obj)
        {
            return (int)obj.GetValue(LengthPropertyProperty);
        }

        public static void SetLength(DependencyObject obj, int value)
        {
            obj.SetValue(LengthPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for LengthProperty.  This enables animation, styling, binding, etc...

        public static bool GetIsNumericOnly(DependencyObject d)
        {
            return (bool)d.GetValue(IsNumericOnlyProperty);
        }

        public static void SetIsNumericOnly(DependencyObject d, bool value)
        {
            d.SetValue(IsNumericOnlyProperty, value);
        }

        public static ValidationRuleBase GetRule(DependencyObject d)
        {
            return (ValidationRuleBase)d.GetValue(RuleProperty);
        }

        public static void SetRule(DependencyObject d, ValidationRuleBase value)
        {
            d.SetValue(RuleProperty, value);
        }

        private static void RuleValidate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            textBox.LostFocus += textBox_LostFocus;
        }

        private static void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as TextBox;
            ValidationRuleBase rule = GetRule(tb);
            rule.Max = GetMax(tb);
            rule.Min = GetMin(tb);
            rule.Number = GetNumber(tb);
            rule.Length = GetLength(tb);
            ValidationResult result = rule.Validate(tb.Text, null);
            if (!result.IsValid)
            {
                MessageBox.Show(result.ErrorContent.ToString(), "提示");
            }
        }


        private static void OnIsNumericOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isNumericOnly = (bool)e.NewValue;

            var textBox = (TextBox)d;

            if (isNumericOnly)
            {
                textBox.PreviewTextInput += BlockNonDigitCharacters;
                textBox.PreviewKeyDown += ReviewKeyDown;
            }
            else
            {
                textBox.PreviewTextInput -= BlockNonDigitCharacters;
                textBox.PreviewKeyDown -= ReviewKeyDown;
            }
        }

        private static void BlockNonDigitCharacters(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                {
                    e.Handled = true;
                }
            }
        }
        private static void ReviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                // Disallow the space key, which doesn't raise a PreviewTextInput event.
                e.Handled = true;
            }
        }
    }
}