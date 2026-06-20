using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SVI_NFT_R
{
    /// <summary>
    /// SQLite.dll을 직접 연결하는 인터페이스 클래스
    /// </summary>
    public class CSQLite
    {
        /// <summary>
        /// SQLite 쿼리 날리는 객체
        /// </summary>
        private SQLiteCommand m_objSQLiteCommand;
        /// <summary>
        /// sql 접속하기 위한 명령
        /// </summary>
        private string m_strConnection;
        /// <summary>
        /// sql 접속하려는 데이터베이스 경로
        /// </summary>
        private string _strDatabasePath;
        /// <summary>
        /// SQLite 접속 객체
        /// </summary>
        public SQLiteConnection SqliteDrive { get; private set; }
        public string m_strDatabasePath
        {
            get
            {
                return _strDatabasePath;
            }
        }
        /// <summary>
        /// sql 접속하려는 데이터베이스 이름
        /// </summary>
        private string _strDatabaseName;
        public string m_strDatabaseName
        {
            get
            {
                return _strDatabaseName;
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="strDatabaseFullPath"></param>
        /// <returns></returns>
        public CErrorReturn HLInitialize(string strDatabaseFullPath)
        {
            CErrorReturn objReturn = new CErrorReturn("CSQLite", "HLInitialize");

            do
            {
                // 이름 제외한 폴더 경로
                string strDatabasePathOnly = Path.GetDirectoryName(strDatabaseFullPath);
                // 파일 이름 (확장자를 포함)
                string strDatabaseExtendName = Path.GetFileName(strDatabaseFullPath);
                // 확장자를 제거한 파일 이름
                string strDatabaseName = Path.GetFileNameWithoutExtension(strDatabaseFullPath);
                // 데이터베이스 경로랑 이름만
                _strDatabasePath = strDatabasePathOnly;
                _strDatabaseName = strDatabaseName;
                // 데이퍼베이스 폴더 유무 체크
                if (false == Directory.Exists(strDatabasePathOnly))
                {
                    // 폴더가 없으면 생성
                    Directory.CreateDirectory(strDatabasePathOnly);
                }
                // 데이터베이스 파일 유무 체크
                if (false == File.Exists(strDatabaseFullPath))
                {
                    // db3 생성
                    SQLiteConnection.CreateFile(strDatabaseFullPath);
                }
                // 확장자 포함해서 연결
                m_strConnection = string.Format(@"Data Source={0}\{1}; Version=3; Journal Mode=WAL;", m_strDatabasePath, strDatabaseExtendName);

                objReturn.m_bError = false;
            } while (false);

            return objReturn;
        }

        /// <summary>
        /// 해제
        /// </summary>
        public void HLDeInitialize()
        {
        }

        /// <summary>
        /// 연결
        /// </summary>
        /// <returns></returns>
        public CErrorReturn HLConnect()
        {
            CErrorReturn objReturn = new CErrorReturn("CSQLite", "HLConnect");
            string strTrace = "";

            do
            {
                try
                {
                    SqliteDrive = new SQLiteConnection(m_strConnection);
                    m_objSQLiteCommand = new SQLiteCommand();
                    // 연결된 데이터베이스랑 커맨드 1:1 매칭
                    m_objSQLiteCommand.Connection = SqliteDrive;
                    // 이벤트 등록
                    SqliteDrive.Commit += new SQLiteCommitHandler(OnEventCommit);
                    SqliteDrive.RollBack += new EventHandler(OnEventRollback);
                    // sql 통신 열어줌
                    SqliteDrive.Open();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex);
                    objReturn.m_strErrorMessage = strTrace;
                    break;
                }

                objReturn.m_bError = false;
            } while (false);

            return objReturn;
        }

        /// <summary>
        /// 연결 해제
        /// </summary>
        public void HLDisconnect()
        {
            // sql 통신 닫아줌
            SqliteDrive.Close();
        }

        /// <summary>
        /// 실행( Insert, Update, Delete )
        /// 설명 : strQueryList : 데이터베이스에 실행할 쿼리 리스트 트랜잭션 이용하여 속도 향상
        /// </summary>
        /// <returns></returns>
        public SQLiteTransaction HLBeginTransaction()
        {
            SQLiteTransaction objTransaction = null;

            try
            {
                // 트랜잭션 시작
                objTransaction = SqliteDrive.BeginTransaction();
                //Trace.WriteLine( System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) + " Begin Transaction " );
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return objTransaction;
        }

        /// <summary>
        /// 실행( Insert, Update, Delete )
        /// 설명 : strQueryList : 데이터베이스에 실행할 쿼리 리스트 트랜잭션 이용하여 속도 향상
        /// </summary>
        /// <param name="objSQLiteTransaction"></param>
        public void HLCommit(SQLiteTransaction objSQLiteTransaction)
        {
            try
            {
                // 트랜잭션 COMMIT
                objSQLiteTransaction.Commit();
                //Trace.WriteLine( System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) + " Transaction Commit" );
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 실행( Insert, Update, Delete )
        /// 설명 : strQueryList : 데이터베이스에 실행할 쿼리 리스트 트랜잭션 이용하여 속도 향상
        /// </summary>
        /// <param name="objSQLiteTransaction"></param>
        public void HLRollback(SQLiteTransaction objSQLiteTransaction)
        {
            try
            {
                // 트랜잭션 ROLLBACK
                objSQLiteTransaction.Rollback();
                //Trace.WriteLine( System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) + " Transaction Rollback" );
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }

        /// <summary>
        /// 실행( Insert, Update, Delete )
        /// 설명 : strQuery : 데이터베이스에 실행할 쿼리
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public CErrorReturn HLExecute(string strQuery)
        {
            CErrorReturn objReturn = new CErrorReturn("CSQLite", "HLExecute");
            do
            {
                try
                {
                    // sql 쿼리문 넣어줌
                    m_objSQLiteCommand.CommandText = strQuery;
                    //Trace.WriteLine( System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) + " Query : " + strQuery );
                    // 연결에 대한 Transact-SQL 문을 실행하고 영향을 받는 행의 수를 반환합니다.
                    m_objSQLiteCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex, "Query:" + strQuery);
                    objReturn.m_strErrorMessage = ex.Message;
                    break;
                }

                objReturn.m_bError = false;
            } while (false);

            return objReturn;
        }

        /// <summary>
        /// 데이터베이스 데이터 불러오기( Select )
        /// 설명 : strQuery : 데이터베이스에 실행할 쿼리 / objDataTable : 메모리 내 데이터의 한 테이블
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="objDataTable"></param>
        /// <returns></returns>
        public CErrorReturn HLReload(string strQuery, ref DataTable objDataTable)
        {
            CErrorReturn objReturn = new CErrorReturn("CSQLite", "HLReload");
            do
            {
                try
                {
                    lock (this)
                    {
                        SQLiteDataAdapter objSQLiteDataAdapter = new SQLiteDataAdapter(strQuery, m_strConnection);
                        // 쿼리문 Trace
                        //Trace.WriteLine( System.DateTime.Now.ToString( "yyyy/MM/dd hh:mm:ss" ) + " Query : " + strQuery );
                        // 이름을 사용하여 지정된 범위에서 데이터 소스의 행과 일치하도록 행을 추가하거나 새로 고칩니다.
                        objSQLiteDataAdapter.Fill(objDataTable);
                    }
                }
                catch (Exception ex)
                {
                    LogWrite.Exception(ex, "Query:" + strQuery);
                    objReturn.m_strErrorMessage = ex.Message;
                    break;
                }

                objReturn.m_bError = false;
            } while (false);

            return objReturn;
        }

        /// <summary>
        /// 트랜잭션 완료 이벤트
        /// </summary>
        /// <param name="objSender"></param>
        /// <param name="e"></param>
        private void OnEventCommit(object objSender, EventArgs e)
        {
        }

        /// <summary>
        /// 트랜잭션 회복 이벤트
        /// </summary>
        /// <param name="objSender"></param>
        /// <param name="e"></param>
        private void OnEventRollback(object objSender, EventArgs e)
        {
        }
    }
}