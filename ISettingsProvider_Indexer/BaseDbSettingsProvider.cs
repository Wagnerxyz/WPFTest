using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ISettingsProvider_Indexer
{
    abstract class BaseDbSettingsProvider : ISettingProvider
    {
        abstract protected IDbConnection CreateDbConnection();

        #region ISettingsProvider 成员

        public string this[string name]
        {
            get
            {
                //虽然这里CreateDbConnection()没有实现，但是知道子类一定会实现
                //这里调用的也是子类实现的方法
                // 不用担心子类没有实现CreateDbConnection方法，因为能够调起来this[string name]这个方法
                //一定是通过一个子类的对象来调用的，既然有对象，一定不是一个抽象类，抽象类不能new
                using (IDbConnection conn = CreateDbConnection())
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from T_Settings where Name='" + name + "'";
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                            {
                                throw new Exception("没找到");
                            }
                            return reader.GetString(reader.GetOrdinal("Value"));
                        }
                    }
                }
            }
            set
            {
                if (NameExists(name))
                {
                    using (IDbConnection conn = CreateDbConnection())
                    {
                        conn.Open();
                        using (IDbCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "Update T_Settings set [Value]='" + value
                                + "' where [Name]='" + name + "'";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (IDbConnection conn = CreateDbConnection())
                    {
                        conn.Open();
                        using (IDbCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "Insert into T_Settings([Name],[Value]) values('" +
                                name + "','" + value + "')";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public string[] Names
        {
            get
            {
                List<string> list = new List<string>();
                //当前指向的对象类型在运行的时候是确定的（写代码的时候是不确定的）
                //如果当前this是SqlSettingsProvider，就不会调到AccessSettingsprovider
                //难点
                //写这个类的时候也不知道子类有哪些
                using (IDbConnection conn = CreateDbConnection())
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from T_Settings";
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString(reader.GetOrdinal("Name"));
                                list.Add(name);
                            }
                        }
                    }
                }
                return list.ToArray();
            }
        }

        public bool NameExists(string name)
        {
            using (IDbConnection conn = CreateDbConnection())
            {
                conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from T_Settings where Name='" + name + "'";
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }

        #endregion
    }
}
