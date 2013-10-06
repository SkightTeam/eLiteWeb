using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Skight.eLiteWeb.Infrastructure.Persistent
{
    public class UserTypeAsString<DomainType>:IUserType
    {
        private static readonly SqlType[] SQL_TYPES = { NHibernateUtil.String.SqlType };
        public bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            object obj = NHibernateUtil.String.NullSafeGet(rs, names[0]);
            if (obj == null) return null;
            return (DomainType)obj;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value==null) {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            } else 
            {

                ((IDataParameter)cmd.Parameters[index]).Value = ((DomainType) value).ToString();
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public SqlType[] SqlTypes
        {
            get { return SQL_TYPES; }
        }

        public Type ReturnedType
        {
            get { return typeof (string); }
        }

        public bool IsMutable
        {
            get { return  false; }
        }
    }
}