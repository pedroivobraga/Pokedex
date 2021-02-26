using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pokedex
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }


        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Source))
                return null;


            var source = ImageResourceExtension.Convert(Source);

            return source;

        }

        public static ImageSource Convert(string source)
        {


            var imageSource = ImageSource.FromResource(source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);


            if (source.ToLower().EndsWith(".svg"))
                return new SvgImageSource(imageSource, 0, 0, true);

            return imageSource;
        }
    }
}
