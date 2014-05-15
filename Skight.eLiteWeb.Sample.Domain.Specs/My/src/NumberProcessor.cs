namespace Skight.eLiteWeb.Sample.Domain.Specs.My.src
{
    public class NumberProcessor
    {
        private int assigned_number;
        private string replace;
        private Matcher matcher;

        public NumberProcessor(int assignedNumber, string replace, Matcher matcher)
        {
            assigned_number = assignedNumber;
            this.replace = replace;
            this.matcher = matcher;
        }

        public string process(int num)
        {
            return matcher.is_match(num) ? replace : string.Empty;
        }
    }
}