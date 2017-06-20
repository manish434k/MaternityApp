using System;

namespace Symlconnect.ViewModel.Media
{
    public struct Color
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public static Color FromHexString(string hexString)
        {
            Color newColor;

            hexString = hexString.Replace("#", string.Empty);

            if (hexString.Length == 6)
            {
                hexString = "FF" + hexString;
            }

            if (hexString.Length != 8)
            {
                throw new ArgumentOutOfRangeException(nameof(hexString), "Hex string must be 6 or 8 hex characters long with an optional # prefix.");
            }

            newColor.A = (byte)(Convert.ToUInt32(hexString.Substring(0, 2), 16));
            newColor.R = (byte)(Convert.ToUInt32(hexString.Substring(2, 2), 16));
            newColor.G = (byte)(Convert.ToUInt32(hexString.Substring(4, 2), 16));
            newColor.B = (byte)(Convert.ToUInt32(hexString.Substring(6, 2), 16));

            return newColor;
        }

        public string ToHexString()
        {
            return "#" + A.ToString("X2") + R.ToString("X2") + G.ToString("X2") + B.ToString("X2");
        }
    }
}