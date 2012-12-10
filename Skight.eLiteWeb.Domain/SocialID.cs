using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Skight.eLiteWeb.Domain.Specs.Properties
{
   
/**
 * 身份证号码,可以解析身份证号码的各个字段，以及验证身份证号码是否有效<br>
 * 身份证号码构成：6位地址编码+8位生日+3位顺序码+1位校验码
 * 
 * @author liuex
 * 
 */
public class SocialID {
	/**
	 * 完整的身份证号码
	 */
	private  string cardNumber;
	// 缓存身份证是否有效，因为验证有效性使用频繁且计算复杂
	private bool? cacheValidateResult = null;
	// 缓存出生日期，因为出生日期使用频繁且计算复杂
	private DateTime? cacheBirthDate = null;
    private static Verifier verifier=new Verifier();

    /**
	 * 如果是15位身份证号码，则自动转换为18位
	 * 
	 * @param cardNumber
	 */
	public SocialID(String cardNumber) 
    {
        if(string.IsNullOrEmpty(cardNumber))
            throw new ApplicationException("Card Number is empty");
        if(cardNumber.Length!=CARD_NUMBER_LENGTH)
            throw new ApplicationException("Card Number Length is wrong.");
        if (!SOCIAL_NUMBER_PATTERN.IsMatch(cardNumber))
            throw new ApplicationException("Card Number has wrong charactor(s).");

        if(cardNumber[CARD_NUMBER_LENGTH-1]!=verifier.verify(cardNumber))
            throw new ApplicationException("Card Number verified code is not match.");

		if (null != cardNumber) {
			cardNumber = cardNumber.Trim();
			if (OLD_CARD_NUMBER_LENGTH == cardNumber.Length) {
				cardNumber = contertToNewCardNumber(cardNumber);
			}
		}
		this.cardNumber = cardNumber;
	}

	public String getCardNumber() {
		return cardNumber;
	}

	public String getAddressCode() {
		return this.cardNumber.Substring(0, 6);
	}

	public DateTime getBirthDate() {
		if (null == this.cacheBirthDate) {
			try
			{
			    this.cacheBirthDate = DateTime.ParseExact(getBirthDayPart(), BIRTH_DATE_FORMAT, null); 
			} catch (Exception e) {
				throw new ApplicationException("身份证的出生日期无效");
			}
		}
	    return cacheBirthDate.Value;
	}

	public bool isMale() {
		return 1 == this.getGenderCode();
	}

	public bool isFemal() {
		return false == this.isMale();
	}

    /**
	 * 获取身份证的第17位，奇数为男性，偶数为女性
	 * 
	 * @return
	 */

    private int getGenderCode() {
        //this.checkIfValid();
        char genderCode = this.cardNumber[CARD_NUMBER_LENGTH - 2];
        return (((int) (genderCode - '0')) & 0x1);
    }

    private String getBirthDayPart() {
		return this.cardNumber.Substring(6, 8);
	}


    // 身份证号码中的出生日期的格式

    private static String BIRTH_DATE_FORMAT = "yyyyMMdd";

    // 身份证的最小出生日期,1900年1月1日

    private static DateTime MINIMAL_BIRTH_DATE = DateTime.MinValue;

    private static int CARD_NUMBER_LENGTH = 18;

    private static int OLD_CARD_NUMBER_LENGTH = 15;
    private static Regex SOCIAL_NUMBER_PATTERN =new Regex( @"^[0-9]{17}[0-9X]$");

    /**
	 * 18位身份证中最后一位校验码
	 */

    private static char[] VERIFY_CODE = { '1', '0', 'X', '9', '8', '7',
			'6', '5', '4', '3', '2' };

    /**
	 * 18位身份证中，各个数字的生成校验码时的权值
	 */

    private static int[] VERIFY_CODE_WEIGHT = { 7, 9, 10, 5, 8, 4, 2, 1,
			6, 3, 7, 9, 10, 5, 8, 4, 2 };

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

    private static char calculateVerifyCode(string cardNumber) {
		int sum = 0;
		for (int i = 0; i < CARD_NUMBER_LENGTH - 1; i++) {
			char ch = cardNumber[i];
			sum += ((int) (ch - '0')) * VERIFY_CODE_WEIGHT[i];
		}
		return VERIFY_CODE[sum % 11];
	}

    /**
	 * 把15位身份证号码转换到18位身份证号码<br>
	 * 15位身份证号码与18位身份证号码的区别为：<br>
	 * 1、15位身份证号码中，"出生年份"字段是2位，转换时需要补入"19"，表示20世纪<br>
	 * 2、15位身份证无最后一位校验码。18位身份证中，校验码根据根据前17位生成
	 * 
	 * @param cardNumber
	 * @return
	 */

    private static String contertToNewCardNumber(String oldCardNumber) {
		StringBuilder buf = new StringBuilder(CARD_NUMBER_LENGTH);
		buf.Append(oldCardNumber.Substring(0, 6));
		buf.Append("19");
		buf.Append(oldCardNumber.Substring(6));
		buf.Append(calculateVerifyCode(buf.ToString()));
		return buf.ToString();
	}
}
}