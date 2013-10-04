namespace Skight.HelpCenter.Domain
{
    public struct Keyword
    {
        public bool Equals(Keyword other)
        {
            return string.Equals(content, other.content);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Keyword && Equals((Keyword) obj);
        }

        public override int GetHashCode()
        {
            return (content != null ? content.GetHashCode() : 0);
        }

        private string content;

        public Keyword(string content)
        {
            this.content = content;
        }

        public static implicit operator string(Keyword keyword)
        {
            return keyword.content;
        }

        public static implicit operator Keyword(string content)
        {
            return new Keyword(content);
        }

        public override string ToString()
        {
            return string.Format(content);
        }
    }
}