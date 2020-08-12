using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFFiler.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPFFiler.models.Tests {
    [TestClass()]
    public class ExFileTests {

        private readonly string emptyTextFileName0 = "emptyFile0.txt";
        private readonly string emptyTextFileName1 = "emptyFile1.txt";

        /// <summary>
        /// 空白ファイル emptyFile0.txt,emptyFile1.txt の作成
        /// </summary>
        [TestInitialize]
        public void TestInitialize() {
            File.WriteAllLines("emptyFile0.txt", new string[0]);
            File.WriteAllLines("emptyFile1.txt", new string[0]);
            Directory.CreateDirectory("emptyDirectory");
        }

        [TestCleanup]
        public void TestCleanup() {
            var f0 = new FileInfo("emptyFile0.txt");
            var f1 = new FileInfo("emptyFile1.tx");
            var d0 = new DirectoryInfo("emptyDirectory");

            if (File.Exists(f0.FullName)) {
                f0.Delete();
            }

            if (File.Exists(f1.FullName)) {
                f1.Delete();
            }

            if (Directory.Exists(d0.FullName)) {
                d0.Delete();
            }
        }

        [TestMethod()]
        public void ExFileTest() {
            FileInfo f0 = new FileInfo(emptyTextFileName0);
            ExFile f = new ExFile(f0.FullName);
        }
    }
}