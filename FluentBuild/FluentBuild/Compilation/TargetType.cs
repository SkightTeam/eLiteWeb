namespace FluentBuild.Compilation
{
    ///<summary>
    /// Picks the Target Type
    ///</summary>
    public class TargetType
    {
        internal readonly string _compiler;
        private Target _target;

        internal TargetType(string compiler)
        {
            _compiler = compiler;
        }

        ///<summary>
        /// Pick the target
        ///</summary>
        public Target Target
        {
            get
            {
                _target = new Target(_compiler);
                return _target;
            }
        }
    }
}