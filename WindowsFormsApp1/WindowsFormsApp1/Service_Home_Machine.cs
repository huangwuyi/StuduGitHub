using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Service_Home_Machine
    {
        [Serializable]
        public partial class Table_Home_Machine
        {
            public Table_Home_Machine()
            { }
            #region Model
            private int _lsno;
            private string _home;
            private string _ip;
            /// <summary>
            /// 
            /// </summary>
            public int lsno
            {
                set { _lsno = value; }
                get { return _lsno; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Home
            {
                set { _home = value; }
                get { return _home; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Ip
            {
                set { _ip = value; }
                get { return _ip; }
            }
            #endregion Model

        }

        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string Home, string Ip)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Table_Home_Machine");
            strSql.Append(" where Home=@Home and Ip=@Ip ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Home", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ip", SqlDbType.NVarChar,50)          };
            parameters[0].Value = Home;
            parameters[1].Value = Ip;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Table_Home_Machine model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Table_Home_Machine(");
            strSql.Append("Home,Ip)");
            strSql.Append(" values (");
            strSql.Append("@Home,@Ip)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Home", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ip", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Home;
            parameters[1].Value = model.Ip;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Table_Home_Machine model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Table_Home_Machine set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
            strSql.Append("lsno=@lsno,");
            strSql.Append("Home=@Home,");
            strSql.Append("Ip=@Ip");
            strSql.Append(" where lsno=@lsno");
            SqlParameter[] parameters = {
                    new SqlParameter("@lsno", SqlDbType.Int,4),
                    new SqlParameter("@Home", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ip", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.lsno;
            parameters[1].Value = model.Home;
            parameters[2].Value = model.Ip;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int lsno)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Table_Home_Machine ");
            strSql.Append(" where lsno=@lsno");
            SqlParameter[] parameters = {
                    new SqlParameter("@lsno", SqlDbType.Int,4)
            };
            parameters[0].Value = lsno;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string Home, string Ip)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Table_Home_Machine ");
            strSql.Append(" where Home=@Home and Ip=@Ip ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Home", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ip", SqlDbType.NVarChar,50)          };
            parameters[0].Value = Home;
            parameters[1].Value = Ip;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string lsnolist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Table_Home_Machine ");
            strSql.Append(" where lsno in (" + lsnolist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Table_Home_Machine GetModel(int lsno)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 lsno,Home,Ip from Table_Home_Machine ");
            strSql.Append(" where lsno=@lsno");
            SqlParameter[] parameters = {
                    new SqlParameter("@lsno", SqlDbType.Int,4)
            };
            parameters[0].Value = lsno;

            Table_Home_Machine model = new Table_Home_Machine();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Table_Home_Machine DataRowToModel(DataRow row)
        {
            Table_Home_Machine model = new Table_Home_Machine();
            if (row != null)
            {
                if (row["lsno"] != null && row["lsno"].ToString() != "")
                {
                    model.lsno = int.Parse(row["lsno"].ToString());
                }
                if (row["Home"] != null)
                {
                    model.Home = row["Home"].ToString();
                }
                if (row["Ip"] != null)
                {
                    model.Ip = row["Ip"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select lsno,Home,Ip ");
            strSql.Append(" FROM Table_Home_Machine ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" lsno,Home,Ip ");
            strSql.Append(" FROM Table_Home_Machine ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Table_Home_Machine ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.lsno desc");
            }
            strSql.Append(")AS Row, T.*  from Table_Home_Machine T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Table_Home_Machine";
			parameters[1].Value = "lsno";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
