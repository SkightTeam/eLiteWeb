namespace Skight.HelpCenter.Domain
{
    public struct Sentence
    {
        private string content;

        public Sentence(string content)
        {
            this.content = content;
        }

        public static implicit operator string(Sentence keyword)
        {
            return keyword.content;
        }

        public static implicit operator Sentence(string content)
        {
            return new Sentence(content);
        }

        public override string ToString()
        {
            return   content;
        }
    }
}