using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace ay.db
{
    public class MySqlServicesBase
    {
        public MySqlServicesBase()
        {
            AdminUserID = "f3dbcb9f-dd8e-48c0-bf48-aaaa35158243";
            RealUserTableName = "realuser";
            RealUserTablePrimaryKey = "Id";
            RealUserTableUserName = "user_name";
        }
        public string AdminUserID { get; set; }
        public string RealUserTableName { get; set; }
        public string RealUserTablePrimaryKey { get; set; }
        public string RealUserTableUserName { get; set; }
        public IDbConnection Con
        {
            get
            {
                return new MySqlConnection(AppConfig.Instance.MySqlConnectionString);
            }
        }
        public List<T> Page<T>(int pageIndex, int pageSize, string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select * from " + typeof(T).Name + " where 1=1 " + condition + " limit " + ((pageIndex - 1) * pageSize) + "," + pageSize, param).ToList();
            }
            return Con.Query<T>("select * from " + typeof(T).Name + " where 1=1 " + condition + " limit " + ((pageIndex - 1) * pageSize) + "," + pageSize).ToList();
        }
        public List<T> PageSql<T>(int pageIndex, int pageSize, string sql, object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>(sql + " limit " + ((pageIndex - 1) * pageSize) + "," + pageSize, param).ToList();
            }
            return Con.Query<T>(sql + " limit " + ((pageIndex - 1) * pageSize) + "," + pageSize).ToList();
        }
        public bool Exist(string sql, object param = null)
        {
            if (param != null)
            {
                return Con.Query(sql, param) == null ? false : true;
            }
            return Con.Query(sql) == null ? false : true;
        }
        public bool Exist<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select count(1) from " + typeof(T).Name + " where isdelete=0 " + condition, param) == null ? false : true;
            }
            return Con.Query<T>("select count(1) from " + typeof(T).Name + " where isdelete=0 " + condition) == null ? false : true;
        }
        public bool ExistIgnoreIsDeleted<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select count(1) from " + typeof(T).Name + " where 1=1 " + condition, param) == null ? false : true;
            }
            return Con.Query<T>("select count(1) from " + typeof(T).Name + " where 1=1 " + condition) == null ? false : true;
        }
        public int Count<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.ExecuteScalar<int>("select count(1) from " + typeof(T).Name + " where isdelete=0" + condition, param);
            }
            return Con.ExecuteScalar<int>("select count(1) from " + typeof(T).Name + " where isdelete=0 " + condition);
        }
        public int CountIgnoreIsDeleted<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.ExecuteScalar<int>("select count(1) from " + typeof(T).Name + " where 1=1 " + condition, param);
            }
            return Con.ExecuteScalar<int>("select count(1) from " + typeof(T).Name + " where 1=1 " + condition);
        }
        public T Get<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select * from " + typeof(T).Name + " where isdelete=0 " + condition + " limit 1;", param).FirstOrDefault();
            }
            return Con.Query<T>("select * from " + typeof(T).Name + " where isdelete=0 " + condition + " limit 1;").FirstOrDefault();
        }
        public T GetIgnoreIsDeleted<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select * from " + typeof(T).Name + " where 1=1 " + condition + " limit 1;", param).FirstOrDefault();
            }
            return Con.Query<T>("select * from " + typeof(T).Name + " where 1=1 " + condition + " limit 1;").FirstOrDefault();
        }
        public T GetById<T>(string id)
        {
            return Con.Query<T>("select top 1 * from " + typeof(T).Name + " where isdelete=0 And id='" + id + "' limit 1;").FirstOrDefault();
        }
        public List<T> Gets<T>(string condition = "", object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select * from " + typeof(T).Name + " where isdelete=0 " + condition, param).ToList();
            }
            return Con.Query<T>("select * from " + typeof(T).Name + " where isdelete=0 " + condition).ToList();
        }
        public List<T> GetsIgnoreIsDeleted<T>(string condition = "", object param = null)
        {
            if (param != null)
            {
                return Con.Query<T>("select * from " + typeof(T).Name + " where 1=1 " + condition, param).ToList();
            }
            return Con.Query<T>("select * from " + typeof(T).Name + " where 1=1 " + condition).ToList();
        }
        /// <summary>
        /// 任意增删改执行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null)
        {
            if (param != null)
            {
                return Con.Execute(sql, param);
            }
            return Con.Execute(sql);
        }
        public int Delete<T>(string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Execute("delete from " + typeof(T).Name + " where isdelete=0 " + condition, param);
            }
            return Con.Execute("delete from " + typeof(T).Name + " where isdelete=0 " + condition);
        }
        public int Update<T>(string set, string condition, object param = null)
        {
            if (param != null)
            {
                return Con.Execute("update " + typeof(T).Name + " " + set + " where isdelete=0 " + condition, param);
            }
            return Con.Execute("update " + typeof(T).Name + " " + set + " where isdelete=0 " + condition);
        }
        public int Insert<T>(string values, object param = null)
        {
            if (param != null)
            {
                return Con.Execute("insert into " + typeof(T).Name + " values(" + values + ")", param);
            }
            return Con.Execute("insert into " + typeof(T).Name + " values(" + values + ")");
        }

    }
}
