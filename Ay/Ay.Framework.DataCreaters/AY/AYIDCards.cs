using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.Framework.DataCreaters.AY
{
    public class AyIDCards
    {
        private static readonly int[] CheckCodes = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
        private static readonly string ValidateCodes = "10X98765432";
        private static string _provinceCode;
        private static string _cityCode;
        private static string _countryCode;
        //private static string _birthday;
        private static string _policeStationCode;
        //private static bool _sex;
        //private static string _validateCode;
        //public bool Accuracy { get; set; }

        //public string Number { get; set; }

        //public AyIDCards(string idNumber)
        //{
        //    Parse(idNumber);
        //    Accuracy = Validate(idNumber);
        //}

        //private static void Parse(string idNumber)
        //{
        //    if (idNumber.Length != 18 && idNumber.Length != 15)
        //    {
        //        throw new Exception(string.Format("{0}长度错误。", idNumber));
        //    }
        //    _provinceCode = idNumber.Substring(0, 2);
        //    _cityCode = idNumber.Substring(2, 2);
        //    _countryCode = idNumber.Substring(4, 2);
        //    _birthday = idNumber.Substring(6, 8);
        //    _policeStationCode = idNumber.Substring(14, 2);
        //    _sex = Convert.ToInt32(idNumber.Substring(16, 1)) % 2 == 1;
        //    _validateCode = idNumber.Substring(17, 1);

        //}

        //private static bool Validate(string idNumber)
        //{
        //    bool accuracy = false;
        //    if (idNumber.Length == 18)
        //    {
        //        accuracy = string.Compare(GeneratorValidateCode(idNumber), _validateCode) == 0;
        //    }
        //    return accuracy;
        //}

        private static string GeneratorValidateCode(string idNumber)
        {
            string validateCode = string.Empty;
            if (idNumber.Length == 18)
            {
                int sum = 0;
                for (int i = 0; i <= 16; i++)
                {
                    sum += Convert.ToInt32(idNumber[i].ToString()) * Convert.ToInt32(CheckCodes[i]);
                }
                int validateCodeIndex = (sum % 11);
                validateCode = ValidateCodes[validateCodeIndex].ToString();
            }

            return validateCode;
        }

        public static string IDCardFromDate(string birthday, bool sex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_provinceCode);
            sb.Append(_cityCode);
            sb.Append(_countryCode);
            sb.Append(birthday);
            sb.Append(_policeStationCode);
            if (sex)
            {
                sb.Append("1");
            }
            else
            {
                sb.Append("2");
            }
            sb.Append(GeneratorValidateCode(sb.ToString() + "0"));

            return sb.ToString();
        }

        public static string IDCard()
        {
            System.Random rnd;
            string[] _crabodistrict = new string[] { "350201", "350202", "350203", "350204", "350205", "350206", "350211", "350205", "350213" };

            rnd = new Random(System.DateTime.Now.Millisecond);

            //PIN = District + Year(50-92) + Month(01-12) + Date(01-30) + Seq(001-600)
            string _pinCode = string.Format("{0}19{1}{2:00}{3:00}{4:000}", _crabodistrict[rnd.Next(0, 8)], rnd.Next(50, 92), rnd.Next(1, 12), rnd.Next(1, 30), rnd.Next(1, 600));
            #region Verify
            char[] _chrPinCode = _pinCode.ToCharArray();
            //校验码字符值
            char[] _chrVerify = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
            //i----表示号码字符从由至左包括校验码在内的位置序号；
            //ai----表示第i位置上的号码字符值；
            //Wi----示第i位置上的加权因子，其数值依据公式intWeight=2（n-1）(mod 11)计算得出。
            int[] _intWeight = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
            int _craboWeight = 0;
            for (int i = 0; i < 17; i++)//从1 到 17 位,18为要生成的验证码
            {
                _craboWeight = _craboWeight + Convert.ToUInt16(_chrPinCode[i].ToString()) * _intWeight[i];
            }
            _craboWeight = _craboWeight % 11;
            _pinCode += _chrVerify[_craboWeight];
            #endregion
            return _pinCode;
        }

    }


}
