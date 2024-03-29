﻿namespace WPFFiler.Models.Tests
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WPFFiler.Models;

    [TestClass]
    public class ExFileTests
    {
        private readonly string emptyTextFileName0 = "emptyFile0.txt";
        private readonly string emptyDirectoryName = "emptyDirectory";

        /// <summary>
        /// 空白ファイル emptyFile0.txt,emptyFile1.txt の作成
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            File.WriteAllLines("emptyFile0.txt", new string[0]);
            File.WriteAllLines("emptyFile1.txt", new string[0]);
            Directory.CreateDirectory("emptyDirectory");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var f0 = new FileInfo("emptyFile0.txt");
            var f1 = new FileInfo("emptyFile1.tx");
            var d0 = new DirectoryInfo("emptyDirectory");

            if (File.Exists(f0.FullName))
            {
                f0.Delete();
            }

            if (File.Exists(f1.FullName))
            {
                f1.Delete();
            }

            if (Directory.Exists(d0.FullName))
            {
                d0.Delete();
            }
        }

        [TestMethod]
        public void ExistsTest()
        {
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

        [TestMethod]
        public void IsDirectoryTest()
        {
            // 存在するディレクトリ
            Assert.IsTrue(new ExFile(emptyDirectoryName).IsDirectory);

            // 存在するファイル
            Assert.IsFalse(new ExFile(emptyTextFileName0).IsDirectory);

            // 存在しないパス
            Assert.IsFalse(new ExFile("testFileName").IsDirectory);
        }

        [TestMethod]
        public void CreateFileTest()
        {
            File.Delete("notExistFile");

            ExFile f = new ExFile("notExistFile");
            Assert.IsFalse(f.Exists);
            Assert.IsNull(f.Content);

            f.CreateFile();
            Assert.IsTrue(f.Exists);
            Assert.IsFalse(f.IsDirectory);
            Assert.IsNotNull(f.Content);
        }

        [TestMethod]
        public void CreateDirectoryTest()
        {
            Directory.Delete("notExistDirectory");

            ExFile d = new ExFile("notExistDirectory");
            Assert.IsFalse(d.Exists);
            Assert.IsNull(d.Content);

            d.CreateDirectory();
            Assert.IsTrue(d.Exists);
            Assert.IsTrue(d.IsDirectory);
            Assert.IsNotNull(d.Content);
        }
    }
}