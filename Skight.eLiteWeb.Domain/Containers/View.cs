namespace Skight.eLiteWeb.Domain.Containers
{
    public class View
    {
        public View(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static implicit operator string(View view)
        {
            return view.ToString();
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(View other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (View)) return false;
            return Equals((View) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}