namespace Skight.eLiteWeb.Domain.Specs.Properties
{
    public class Verifier
    {
        private static char[] VERIFY_CODE =
            {
                '1', '0', 'X', '9', '8', '7',
                '6', '5', '4', '3', '2'
            };

        /**
         * 18位身份证中，各个数字的生成校验码时的权值
         */

        private static int[] VERIFY_CODE_WEIGHT =
            {
                7, 9, 10, 5, 8, 4, 2, 1,
                6, 3, 7, 9, 10, 5, 8, 4, 2
            };
        private static int CARD_NUMBER_LENGTH = 18;

        public char verify(string source)
        {
            /**
             * <li>校验码（第十八位数）：<br/>
             * <ul>
             * <li>十七位数字本体码加权求和公式 S = Sum(Ai * Wi), i = 0...16 ，先对前17位数字的权求和；
             * Ai:表示第i位置上的身份证号码数字值 Wi:表示第i位置上的加权因子 Wi: 7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4
             * 2；</li>
             * <li>计算模 Y = mod(S, 11)</li>
             * <li>通过模得到对应的校验码 Y: 0 1 2 3 4 5 6 7 8 9 10 校验码: 1 0 X 9 8 7 6 5 4 3 2</li>
             * </ul>
             * 
             * @param cardNumber
             * @return
             */

            int sum = 0;
            for (int i = 0; i < CARD_NUMBER_LENGTH - 1; i++)
            {
                char ch = source[i];
                sum += ((int) (ch - '0'))*VERIFY_CODE_WEIGHT[i];
            }
            return VERIFY_CODE[sum%11];
        }

    }
}