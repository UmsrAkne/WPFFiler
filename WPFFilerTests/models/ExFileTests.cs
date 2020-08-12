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
        private readonly string emptyDirectoryName = "emptyDirectory";

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

        [TestMethod()]
        public void ExistsTest() {
            // initialize で生成したファイルが存在するか
            ExFile f = new ExFile(emptyTextFileName0);
            Assert.IsTrue(f.Exists);

            // 存在しないファイルを指定した際、false が返って来るか
            ExFile notExistFile = new ExFile("notExistsFile");
            Assert.IsFalse(notExistFile.Exists);

            // 指定したパスがディレクトリであっても、存在すると判定されるか
            ExFile d = new ExFile(emptyDirectoryName);
            Assert.IsTrue(d.Exists);
        }

        [TestMethod()]
        public void IsDirectoryTest() {
            // 存在するディレクトリ
            Assert.IsTrue(new ExFile(emptyDirectoryName).IsDirectory);

            // 存在するファイル
            Assert.IsFalse(new ExFile(emptyTextFileName0).IsDirectory);

            // 存在しないパス
            Assert.IsFalse(new ExFile("testFileName").IsDirectory);
        }
    }
}