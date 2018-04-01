using System;
using System.Collections.Generic;
using System.IO;
using CsvExport.Common;
using CsvExport.DataAccess;
using CsvExport.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvExport
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Constants.ConnectoinString = @"C:\Users\bhaga\Dropbox\MoneyManger\Database\1.mmb";
                ExportData();

                Console.WriteLine("File exported successfully");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.ReadKey();
        }

        private static void ExportData()
        {
            const string filePath = @"C:\Users\bhaga\Dropbox\MoneyManger\Data\TransView.csv";

            var sqlQueries = GetSqlQuries();

            if (File.Exists(filePath)) File.Delete(filePath);

            TextWriter textWriter = new StreamWriter(filePath);

            var writer = new CsvWriter(textWriter);
            writer.Configuration.MemberTypes = MemberTypes.Properties;
            writer.WriteHeader(typeof(DataModel));
            writer.NextRecord();

            foreach (string sqlQuery in sqlQueries)
            {
                var data = ExportDataAccess.GetDataFromDatabase(sqlQuery);
                writer.WriteRecords(data);
                writer.Flush();
            }
            textWriter.Flush();
        }

        private static IEnumerable<string> GetSqlQuries()
        {
            return new[] {
                @"select T.TransId,A.AccountName,T.TransCode,T.Notes,C.CategName,SC.SubCategName,P.PayeeName,T.TransAmount,T.TransDate from CHECKINGACCOUNT_V1 T
left join CATEGORY_V1 C on T.CategId=C.CategId
left join SUBCATEGORY_V1 SC on T.SubCategId=SC.SubCategID
left join PAYEE_V1 P on P.PayeeId=T.PayeeId
left join ACCOUNTLIST_V1 A on T.AccountId=A.AccountId
where transcode!='Transfer'",

                @"select T.TransId,A.AccountName,'Withdrawal' TRANSCODE,T.Notes,C.CategName,SC.SubCategName,P.PayeeName,T.TransAmount,T.TransDate from CHECKINGACCOUNT_V1 T
left join CATEGORY_V1 C on T.CategId=C.CategId
left join SUBCATEGORY_V1 SC on T.SubCategId=SC.SubCategID
left join PAYEE_V1 P on P.PayeeId=T.PayeeId
left join ACCOUNTLIST_V1 A on T.AccountId=A.AccountId
where transcode='Transfer'",

                @"select T.TransId+'.1' TRANSID,A.AccountName,'Deposit' TRANSCODE,T.Notes,C.CategName,SC.SubCategName,P.PayeeName,T.TransAmount,T.TransDate from CHECKINGACCOUNT_V1 T
left join CATEGORY_V1 C on T.CategId=C.CategId
left join SUBCATEGORY_V1 SC on T.SubCategId=SC.SubCategID
left join PAYEE_V1 P on P.PayeeId=T.PayeeId
left join ACCOUNTLIST_V1 A on T.ToAccountId=A.AccountId
where transcode='Transfer'",

                @"select AccountName TRANSID,AccountName,'Deposit' TransCode,Notes,'Initial Balance' CATEGNAME,'' SUBCATEGNAME,'' PAYEENAME,initialBal  TRANSAMOUNT, '2015-10-10' TRANSDATE
from ACCOUNTLIST_V1 where INITIALBAL>0"
            };
        }
    }
}