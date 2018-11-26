using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Service_User
    {
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string UerName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Table_UserInfo");
            strSql.Append(" where UerName=@UerName ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UerName", SqlDbType.NVarChar,50)         };
            parameters[0].Value = UerName;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Table_UserInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Table_UserInfo(");
            strSql.Append("UerName,Password)");
            strSql.Append(" values (");
            strSql.Append("@UerName,@Password)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UerName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Password", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UerName;
            parameters[1].Value = model.Password;

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
        public bool Update(Table_UserInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Table_UserInfo set ");
            strSql.Append("Password=@Password");
            strSql.Append(" where Lsno=@Lsno");
            SqlParameter[] parameters = {
                    new SqlParameter("@Password", SqlDbType.NVarChar,50),
                    new SqlParameter("@UerName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Lsno", SqlDbType.Int,4)};
            parameters[0].Value = model.Password;
            parameters[1].Value = model.UerName;
            parameters[2].Value = model.Lsno;

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
        public bool Delete(int Lsno)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Table_UserInfo ");
            strSql.Append(" where Lsno=@Lsno");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lsno", SqlDbType.Int,4)
            };
            parameters[0].Value = Lsno;

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
        public bool Delete(string UerName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Table_UserInfo ");
            strSql.Append(" where UerName=@UerName ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UerName", SqlDbType.NVarChar,50)         };
            parameters[0].Value = UerName;

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
        public bool DeleteList(string Lsnolist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Table_UserInfo ");
            strSql.Append(" where Lsno in (" + Lsnolist + ")  ");
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
        public Table_UserInfo GetModel(int Lsno)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UerName,Password,Lsno from Table_UserInfo ");
            strSql.Append(" where Lsno=@Lsno");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lsno", SqlDbType.Int,4)
            };
            parameters[0].Value = Lsno;

            Table_UserInfo model = new Table_UserInfo();
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
        public Table_UserInfo GetModelByKey(string UerName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UerName,Password,Lsno from Table_UserInfo ");
            strSql.Append(" where UerName=@UerName ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UerName", SqlDbType.NVarChar,50)         };
            parameters[0].Value = UerName;

            Table_UserInfo model = new Table_UserInfo();
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
        public Table_UserInfo DataRowToModel(DataRow row)
        {
            Table_UserInfo model = new Table_UserInfo();
            if (row != null)
            {
                if (row["UerName"] != null)
                {
                    model.UerName = row["UerName"].ToString();
                }
                if (row["Password"] != null)
                {
                    model.Password = row["Password"].ToString();
                }
                if (row["Lsno"] != null && row["Lsno"].ToString() != "")
                {
                    model.Lsno = int.Parse(row["Lsno"].ToString());
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
            strSql.Append("select UerName,Password,Lsno ");
            strSql.Append(" FROM Table_UserInfo ");
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
            strSql.Append(" UerName,Password,Lsno ");
            strSql.Append(" FROM Table_UserInfo ");
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
            strSql.Append("select count(1) FROM Table_UserInfo ");
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
                strSql.Append("order by T.Lsno desc");
            }
            strSql.Append(")AS Row, T.*  from Table_UserInfo T ");
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
			parameters[0].Value = "Table_UserInfo";
			parameters[1].Value = "Lsno";
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

        public partial class Table_UserInfo
        {
            public Table_UserInfo()
            { }
            #region Model
            private string _uername;
            private string _password;
            private int _lsno;
            /// <summary>
            /// 
            /// </summary>
            public string UerName
            {
                set { _uername = value; }
                get { return _uername; }
            }
            /// <summary>
            /// 
            /// </summary>
            public string Password
            {
                set { _password = value; }
                get { return _password; }
            }
            /// <summary>
            /// 
            /// </summary>
            public int Lsno
            {
                set { _lsno = value; }
                get { return _lsno; }
            }
            #endregion Model

        }
    }
}
