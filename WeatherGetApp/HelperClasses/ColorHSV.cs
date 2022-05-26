namespace WeatherGetApp.HelperClasses
{
    internal struct ColorHSL
    {
        public float Hue { get; set; }
        public float Saturation { get; set; }
        public float Lightness { get; set; }

        public static ColorHSL ColorToHSL(System.Windows.Media.Color color)
        {
            System.Drawing.Color color1 = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

            return new()
            {
                Hue = color1.GetHue(),
                Saturation = color1.GetSaturation(),
                Lightness = color1.GetBrightness(),
            };
        }

        public static System.Windows.Media.Color HSLToRGB(ColorHSL hsl, byte alpha)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (hsl.Saturation == 0)
            {
                r = g = b = (byte)(hsl.Lightness * 255);
            }
            else
            {
                float v1, v2;
                float hue = (float)hsl.Hue / 360;

                v2 = (hsl.Lightness < 0.5) ? (hsl.Lightness * (1 + hsl.Saturation)) : ((hsl.Lightness + hsl.Saturation) - (hsl.Lightness * hsl.Saturation));
                v1 = 2 * hsl.Lightness - v2;

                r = (byte)(255 * HueToRGB(v1, v2, hue + (1.0f / 3)));
                g = (byte)(255 * HueToRGB(v1, v2, hue));
                b = (byte)(255 * HueToRGB(v1, v2, hue - (1.0f / 3)));
            }

            return System.Windows.Media.Color.FromArgb(alpha, r, g, b);
        }
        private static float HueToRGB(float v1, float v2, float vH)
        {
            if (vH < 0)
                vH += 1;

            if (vH > 1)
                vH -= 1;

            if ((6 * vH) < 1)
                return (v1 + (v2 - v1) * 6 * vH);

            if ((2 * vH) < 1)
                return v2;

            if ((3 * vH) < 2)
                return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

            return v1;
        }
    }
}
