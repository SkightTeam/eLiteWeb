using System;
using System.Text.RegularExpressions;

namespace Skight.eLiteWeb.Domain
{
    public enum Gender
    {
        Female,
        Male
    }
    public class SocialID
    {
        private static Verifier verifier = new Verifier();
        private static String BIRTH_DATE_FORMAT = "yyyyMMdd";
        private static int CARD_NUMBER_LENGTH = 18;
        private static Regex SOCIAL_NUMBER_PATTERN = new Regex(@"^[0-9]{17}[0-9X]$");

        public SocialID(String cardNumber)
        {
            validate(cardNumber);
            CardNumber= cardNumber;
            extract();
        }

        private void validate(string cardNumber)
        {
            if (!SOCIAL_NUMBER_PATTERN.IsMatch(cardNumber))
                throw new ApplicationException("Card Number has wrong charactor(s).");

            if (cardNumber[CARD_NUMBER_LENGTH - 1] != verifier.verify(cardNumber))
                throw new ApplicationException("Card Number verified code is not match.");
        }
        void extract()
        {
            AddressCode = CardNumber.Substring(0, 6);
            Gender = ((int) CardNumber[CARD_NUMBER_LENGTH - 2])%2 == 0 ? Gender.Female : Gender.Male;
            BirthDate = extract_birth_date();
        }
        public DateTime extract_birth_date()
        {
            try
            {
                return DateTime.ParseExact(CardNumber.Substring(6, 8), BIRTH_DATE_FORMAT, null);
            }
            catch (Exception e)
            {
                throw new ApplicationException("身份证的出生日期无效");
            }
        }

        public string CardNumber { get;private set; }
        public string AddressCode { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; private set; }
    }
}