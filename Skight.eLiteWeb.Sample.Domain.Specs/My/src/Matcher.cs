namespace Skight.eLiteWeb.Sample.Domain.Specs.My.src
{
    public class Matcher
    {
        private int determine_by;

        public Matcher(int determineBy)
        {
            determine_by = determineBy;
        }

        public bool is_divisible(int num)
        {
            return num%determine_by == 0;
        }

        public bool is_appear_in(int num)
        {
            return num.ToString().Contains(determine_by.ToString());
        }

        public bool is_match(int num)
        {
            return is_divisible(num) || is_appear_in(num);
        }
    }
}