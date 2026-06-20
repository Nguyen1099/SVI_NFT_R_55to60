using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SVI_NFT_R
{
    public class CCsvFile
    {
        /// <summary>
        /// Csv 파일 -> DataTable
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="isFirstRowHeader"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromCsv(string strPath, bool isFirstRowHeader)
        {
            DataTable objDataTable = new DataTable();

            // 헤더 유무
            string strHeader = null;
            if (true == isFirstRowHeader)
            {
                strHeader = "Yes";
            }
            else
            {
                strHeader = "No";
            }
            try
            {
                // 이름 제외한 폴더 경로
                string strPathOnly = Path.GetDirectoryName(strPath);
                // 파일 이름
                string strFileName = Path.GetFileName(strPath);
                // 전체 얻어옴
                string strQuery = string.Format("select * from [{0}]", strFileName);
                // CSV 파일 -> DataTable 변환
                using (OleDbConnection objConnection = new OleDbConnection(
                    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPathOnly +
                    ";Extended Properties=\"Text;HDR=" + strHeader + "\""))
                using (OleDbCommand objCommand = new OleDbCommand(strQuery, objConnection))
                using (OleDbDataAdapter objAdapter = new OleDbDataAdapter(objCommand))
                {
                    objDataTable.Locale = CultureInfo.CurrentCulture;
                    objAdapter.Fill(objDataTable);
                }
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }

            return objDataTable;
        }

        /// <summary>
        /// DataTable -> Csv 파일
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="objDataTable"></param>
        public void SetDataTableToCsv(string strPath, DataTable objDataTable)
        {
            try
            {
                FileStream objFileStream = new FileStream(strPath, FileMode.Create, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter(objFileStream, Encoding.UTF8);
                // 컬럼 구분자 , 로 나눔
                string strLine = string.Join(",", objDataTable.Columns.Cast<object>());
                objStreamWriter.WriteLine(strLine);
                // row 구분자 , 로 나눔
                for (int iLoopRow = 0; iLoopRow < objDataTable.Rows.Count; iLoopRow++)
                {
                    strLine = string.Join(",", objDataTable.Rows[iLoopRow].ItemArray.Cast<object>());
                    // 개행 문자 제거 mcr에 개행 문자 붙음..
                    strLine = strLine.Replace("\r", "");
                    strLine = strLine.Replace("\n", "");
                    objStreamWriter.WriteLine(strLine);
                }
                objStreamWriter.Close();
                objFileStream.Close();
            }
            catch (Exception ex)
            {
                LogWrite.Exception(ex);
            }
        }
    }
}