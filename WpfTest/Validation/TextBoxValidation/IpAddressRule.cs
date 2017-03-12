using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Controls;

namespace WpfTest
{
    public class IpAddressRule : ValidationRuleBase
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string param = value as string;
            if (string.IsNullOrWhiteSpace(param))
            {
                return new ValidationResult(false, "请输入OBDH指令接收组地址 ");
            }
            IPAddress ip;
            bool get = IPAddress.TryParse(param, out ip);
            if (!get)
            {
                return new ValidationResult(false, "输入的不是IP地址 ");
            }

            byte[] arr = ip.GetAddressBytes();
            bool islegal = true;
            if ((arr[0] & 224) != 224 || arr[0] > 238)
            {
                islegal = false;
            }
            if (arr[0] == 224 && arr[1] == 0 && arr[2] < 2)
            {
                islegal = false;
            }
            if (!islegal)
            {
                return new ValidationResult(false, "输入IP地址不是组广播地址，请输入224.0.2.0～238.255.255.255间的地址");
            }
            return new ValidationResult(true, null);

        }
    }
}
