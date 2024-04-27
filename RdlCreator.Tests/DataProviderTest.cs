﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using RdlCreator;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace RdlCreator.Tests
{
    [TestFixture]
    public class DataProviderTest
    {
        string connectionString = "Data Source=sqlitetestdb2.db;";
        string dataProvider = "Microsoft.Data.Sqlite";

        [Test]
        public void TestMethodCsv()
        {
            var create = new RdlCreator.Create();

            var fyiReport = create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");
            var ms = new fyiReporting.RDL.MemoryStreamGen();
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ms, fyiReporting.RDL.OutputPresentationType.CSV);
            var text = ms.GetText();

            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(@"""DataProviderTest TestMethod1""
""CategoryID"",""CategoryName"",""Description""
1,""Beverages"",""Soft drinks, coffees, teas, beers, and ales""
2,""Condiments"",""Sweet and savory sauces, relishes, spreads, and seasonings""
3,""Confections"",""Desserts, candies, and sweet breads""
4,""Dairy Products"",""Cheeses""
5,""Grains/Cereals"",""Breads, crackers, pasta, and cereal""
6,""Meat/Poultry"",""Prepared meats""
7,""Produce"",""Dried fruit and bean curd""
8,""Seafood"",""Seaweed and fish""
""1 of 1""
"));
        }

        [Test]
        public void TestMethodExcelLegacy()
        {
            var create = new RdlCreator.Create();

            var fyiReport = create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodExcelLegacy.xls");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.ExcelTableOnly);

            Assert.That(System.IO.File.Exists(filepath), Is.True);
        }

        [Test]
        public void TestMethodExcel2007()
        {
            var create = new RdlCreator.Create();

            var fyiReport = create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodExcel.xlsx");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.Excel2007);

            Assert.That(System.IO.File.Exists(filepath), Is.True);
        }

        [Test]
        public void TestMethodExcel2007DataOnly()
        {
            var create = new RdlCreator.Create();

            var fyiReport = create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodExcelDataOnly.xlsx");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.Excel2007DataOnly);

            Assert.That(System.IO.File.Exists(filepath), Is.True);
        }

        [Test]
        public void TestMethodPdf()
        {
            var create = new RdlCreator.Create();

            var fyiReport = create.GenerateRdl(dataProvider,
                connectionString,
                "SELECT CategoryID, CategoryName, Description FROM Categories",
                pageHeaderText: "DataProviderTest TestMethod1");

            string filepath = System.IO.Path.Combine(Environment.CurrentDirectory, "TestMethodPdf.pdf");
            var ofs = new fyiReporting.RDL.OneFileStreamGen(filepath, true);
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ofs, fyiReporting.RDL.OutputPresentationType.PDF);

            Assert.That(System.IO.File.Exists(filepath), Is.True);
        }

        [Test]
        public void TestReportFromDataTable()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("CategoryID", typeof(int));
            dt.Columns.Add("CategoryName", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Rows.Add(1, "Beverages", "Soft drinks, coffees, teas, beers, and ales");
            dt.Rows.Add(2, "Condiments", "Sweet and savory sauces, relishes, spreads, and seasonings");
                dt.Rows.Add(3, "Confections", "Desserts, candies, and sweet breads");
            dt.Rows.Add(4, "Dairy Products", "Cheeses");
            dt.Rows.Add(5, "Grains/Cereals", "Breads, crackers, pasta, and cereal");
            dt.Rows.Add(6, "Meat/Poultry", "Prepared meats");
            dt.Rows.Add(7, "Produce", "Dried fruit and bean curd");
            dt.Rows.Add(8, "Seafood", "Seaweed and fish");

            var create = new RdlCreator.Create();
            var fyiReport = create.GenerateRdl(dt,
                pageHeaderText: "DataProviderTest TestMethod1");
            var ms = new fyiReporting.RDL.MemoryStreamGen();
            fyiReport.RunGetData(null);
            fyiReport.RunRender(ms, fyiReporting.RDL.OutputPresentationType.CSV);
            var text = ms.GetText();

            Assert.That(text, Is.Not.Null);
            Assert.That(NormalizeEOL(text), Is.EqualTo(@"""DataProviderTest TestMethod1""
""CategoryID"",""CategoryName"",""Description""
1,""Beverages"",""Soft drinks, coffees, teas, beers, and ales""
2,""Condiments"",""Sweet and savory sauces, relishes, spreads, and seasonings""
3,""Confections"",""Desserts, candies, and sweet breads""
4,""Dairy Products"",""Cheeses""
5,""Grains/Cereals"",""Breads, crackers, pasta, and cereal""
6,""Meat/Poultry"",""Prepared meats""
7,""Produce"",""Dried fruit and bean curd""
8,""Seafood"",""Seaweed and fish""
""1 of 1""
"));
        }

        private string NormalizeEOL(string input)
        {
            return Regex.Replace(input, @"\r\n|\n\r|\n|\r", "\r\n");
        }
    }
}