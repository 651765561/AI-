using System;
using System.Text;

namespace AiWin
{
    public class Common
    {
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += " " + Convert.ToString(b[i], 16);
            }
            return result;
        }
        public static string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

     

        /// 字节数组转16进制字符串
        ///

        ///
        ///
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// 字节数组转16进制字符串第二种
        ///

        ///
        ///
        public static string ByteToHexStr2(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2")+" ";
                }
            }
            return returnStr;
        }
        ///

        /// 从汉字转换到16进制
        ///

        ///
        /// 编码,如"utf-8","gb2312"
        /// 是否每字符用空格分隔
        ///
        public static string ToHex(string s, Encoding encode, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                         //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = encode;
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", " ");
                }
            }
            return str.ToLower();
        }
  
        /// 从16进制转换成汉字
        ///
        public static string UnHex(string hex, Encoding charset)
        {
            hex = hex?.Replace(",", "") ?? throw new ArgumentNullException(nameof(hex));
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            hex = hex.Replace("\r", "");
            hex = hex.Replace("\0", "");
            if (hex.Length % 2 != 0)
            {
                hex += " ";//空格
            }
            // 需要将 hex 转换成 byte 数组。
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message.
                    throw new ArgumentException("hex is not a valid hex number!", nameof(hex));
                }
            }
            System.Text.Encoding chs = charset;
            return chs.GetString(bytes);
        }
        //字符串转换为16进制byte数组
        public static byte[] StrToHexByte(string data)
        {
            data = data.Replace(" ", "");
            if ((data.Length % 2) != 0)
            {
                data += " ";
            }
           // data = data.TrimEnd(' ');
            byte[] bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Convert.ToString(0xa,2)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBinString(string s)
        {
            Func<char, byte> toByte = (char1) => {
                if (char1 >= '0' && char1 <= '9')//数字
                    return (byte)(char1 - '0');
                if (char1 >= 'A' && char1 <= 'F')//大写字母，10以上的值
                    return (byte)(char1 - 'A' + 10);
                if (char1 >= 'a' && char1 <= 'f')//大写字母，10以上的值
                    return (byte)(char1 - 'a' + 10);
                throw new ArgumentOutOfRangeException();

            };

            Func<byte, string> toBin = (byte1) =>
              //string.Format("{0}{1}{2}{3}",
              //      byte1 >> 0 & 0x1,
              //      byte1 >> 1 & 0x1,
              //      byte1 >> 2 & 0x1,
              //      byte1 >> 3 & 0x1
              //  );

            $"{byte1 >> 0 & 0x1}{byte1 >> 1 & 0x1}{byte1 >> 2 & 0x1}{byte1 >> 3 & 0x1}";

            string result = "";
            foreach (var char1 in s)
            {
                var b = toByte(char1);
                result += toBin(b);
            }
            return result;
        }
        /// <summary>
        /// 截取指定字符串开始到结束
        /// </summary>
        /// <param name="txtStr"></param>
        /// <param name="firstStr"></param>
        /// <param name="secondStr"></param>
        /// <returns></returns>
        public static string GetStr(string txtStr, string firstStr, string secondStr)
        {
            if (firstStr.IndexOf(secondStr, 0, StringComparison.Ordinal) != -1)
                return "";
            int firstSite = txtStr.IndexOf(firstStr, 0, StringComparison.Ordinal);
            int secondSite = txtStr.IndexOf(secondStr, firstSite + 1, StringComparison.Ordinal);
            if (firstSite == -1 || secondSite == -1)
                return "";
            return txtStr.Substring(firstSite + firstStr.Length, secondSite - firstSite - firstStr.Length);
        }
    }
}
