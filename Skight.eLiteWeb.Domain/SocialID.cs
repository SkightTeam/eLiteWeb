using System;
using System.Text.RegularExpressions;

namespace Skight.eLiteWeb.Domain
{

    /**
     * 身份证号码,可以解析身份证号码的各个字段，以及验证身份证号码是否有效<br>
     * 身份证号码构成：6位地址编码+8位生日+3位顺序码+1位校验码
     * 
     * @author liuex
     * 
     */

    public class SocialID
    {
        /**
         * 完整的身份证号码
         */
        private string cardNumber;
        private DateTime? cacheBirthDate = null;
        private static Verifier verifier = new Verifier();
        private static String BIRTH_DATE_FORMAT = "yyyyMMdd";
        private static int CARD_NUMBER_LENGTH = 18;
        private static Regex SOCIAL_NUMBER_PATTERN = new Regex(@"^[0-9]{17}[0-9X]$");

        public SocialID(String cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                throw new ApplicationException("Card Number is empty");
            if (cardNumber.Length != CARD_NUMBER_LENGTH)
                throw new ApplicationException("Card Number Length is wrong.");
            if (!SOCIAL_NUMBER_PATTERN.IsMatch(cardNumber))
                throw new ApplicationException("Card Number has wrong charactor(s).");

            if (cardNumber[CARD_NUMBER_LENGTH - 1] != verifier.verify(cardNumber))
                throw new ApplicationException("Card Number verified code is not match.");
            this.cardNumber = cardNumber;
        }

        public String getCardNumber()
        {
            return cardNumber;
        }

        public String getAddressCode()
        {
            return this.cardNumber.Substring(0, 6);
        }

        public DateTime getBirthDate()
        {
            if (null == this.cacheBirthDate)
            {
                try
                {
                    this.cacheBirthDate = DateTime.ParseExact(getBirthDayPart(), BIRTH_DATE_FORMAT, null);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("身份证的出生日期无效");
                }
            }
            return cacheBirthDate.Value;
        }

        public bool isMale()
        {
            return 1 == this.getGenderCode();
        }

        public bool isFemal()
        {
            return false == this.isMale();
        }

        /**
         * 获取身份证的第17位，奇数为男性，偶数为女性
         * 
         * @return
         */

        private int getGenderCode()
        {
            //this.checkIfValid();
            char genderCode = this.cardNumber[CARD_NUMBER_LENGTH - 2];
            return (((int) (genderCode - '0')) & 0x1);
        }

        private String getBirthDayPart()
        {
            return this.cardNumber.Substring(6, 8);
        }


        
    }
}