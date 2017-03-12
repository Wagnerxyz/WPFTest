using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls
{
    public class ImageButton : Button
    {
        // Fields
        public static readonly DependencyProperty ImageHoverProperty = DependencyProperty.Register("ImageHover", typeof(ImageSource), typeof(ImageButton));
        public static readonly DependencyProperty ImageNormalProperty = DependencyProperty.Register("ImageNormal", typeof(ImageSource), typeof(ImageButton));
        public static readonly DependencyProperty ImagePressedProperty = DependencyProperty.Register("ImagePressed", typeof(ImageSource), typeof(ImageButton));

        // Methods
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        // Properties
        public ImageSource ImageHover
        {
            get
            {
                return (ImageSource)base.GetValue(ImageHoverProperty);
            }
            set
            {
                base.SetValue(ImageHoverProperty, value);
            }
        }

        public ImageSource ImageNormal
        {
            get
            {
                return (ImageSource)base.GetValue(ImageNormalProperty);
            }
            set
            {
                base.SetValue(ImageNormalProperty, value);
            }
        }

        public ImageSource ImagePressed
        {
            get
            {
                return (ImageSource)base.GetValue(ImagePressedProperty);
            }
            set
            {
                base.SetValue(ImagePressedProperty, value);
            }
        }
    }


    //public class ImageButton : Button
    //{
    //    static ImageButton()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
    //    }

    //    #region Dependency Properties

    //    public double ImageSize
    //    {
    //        get { return (double)GetValue(ImageSizeProperty); }
    //        set { SetValue(ImageSizeProperty, value); }
    //    }

    //    public static readonly DependencyProperty ImageSizeProperty =
    //        DependencyProperty.Register("ImageSize", typeof(double), typeof(ImageButton),
    //        new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.AffectsRender));

    //    public string NormalImage
    //    {
    //        get { return (string)GetValue(NormalImageProperty); }
    //        set { SetValue(NormalImageProperty, value); }
    //    }

    //    public static readonly DependencyProperty NormalImageProperty =
    //        DependencyProperty.Register("NormalImage", typeof(string), typeof(ImageButton),
    //        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

    //    public string HoverImage
    //    {
    //        get { return (string)GetValue(HoverImageProperty); }
    //        set { SetValue(HoverImageProperty, value); }
    //    }

    //    public static readonly DependencyProperty HoverImageProperty =
    //        DependencyProperty.Register("HoverImage", typeof(string), typeof(ImageButton),
    //        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

    //    public string PressedImage
    //    {
    //        get { return (string)GetValue(PressedImageProperty); }
    //        set { SetValue(PressedImageProperty, value); }
    //    }

    //    public static readonly DependencyProperty PressedImageProperty =
    //        DependencyProperty.Register("PressedImage", typeof(string), typeof(ImageButton),
    //        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

    //    public string DisabledImage
    //    {
    //        get { return (string)GetValue(DisabledImageProperty); }
    //        set { SetValue(DisabledImageProperty, value); }
    //    }

    //    public static readonly DependencyProperty DisabledImageProperty =
    //        DependencyProperty.Register("DisabledImage", typeof(string), typeof(ImageButton),
    //        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

    //    private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
    //    }

    //    public Visibility BorderVisibility
    //    {
    //        get { return (Visibility)GetValue(BorderVisibilityProperty); }
    //        set { SetValue(BorderVisibilityProperty, value); }
    //    }

    //    public static readonly DependencyProperty BorderVisibilityProperty =
    //        DependencyProperty.Register("BorderVisibility", typeof(Visibility), typeof(ImageButton),
    //        new FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender));

    //    #endregion
    //}


}
