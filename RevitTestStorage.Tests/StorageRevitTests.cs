using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using NUnit.Framework;
using RevitTestStorage.Tests.Services;
using RevitTestStorage.Tests.Utils;
using System;
using System.Linq;

namespace RevitTestStorage.Tests
{
    public class StorageRevitTests : OneTimeOpenDocumentTest
    {
        protected override string FileName => "Files\\ExtensibleStorage2021.rvt";
        [Test]
        public void Title()
        {
            Console.WriteLine(document.Title);
        }

        [TestCase("123")]
        [TestCase("Hello World")]
        public void SaveLoadReset_String(string value)
        {
            IStorageProjectInfo storageProjectInfo = new StorageProjectInfo();
            TestStorageProjectInfo(storageProjectInfo, value);
        }

        [TestCase("123")]
        [TestCase("Hello World")]
        public void SaveLoadReset_Vendor(string value)
        {
            IStorageProjectInfo storageProjectInfoVendor = new StorageProjectInfoVendor();
            TestStorageProjectInfo_Exception(storageProjectInfoVendor, value);
        }

        [TestCase("123")]
        [TestCase("Hello World")]
        public void SaveLoadReset_Application(string value)
        {
            IStorageProjectInfo storageProjectInfoApplication = new StorageProjectInfoApplication();
            TestStorageProjectInfo(storageProjectInfoApplication, value);
        }

        private void TestStorageProjectInfo(IStorageProjectInfo storageProjectInfo, string value)
        {
            using (Transaction transaction = new Transaction(document))
            {
                transaction.Start("SaveLoadReset");
                storageProjectInfo.Save(document, value);
                Assert.AreEqual(value, storageProjectInfo.Load(document));
                storageProjectInfo.Reset(document);
                Assert.AreEqual(string.Empty, storageProjectInfo.Load(document));
                transaction.Commit();
            }
        }

        private void TestStorageProjectInfo_Exception(IStorageProjectInfo storageProjectInfo, string value)
        {
            using (Transaction transaction = new Transaction(document))
            {
                transaction.Start("SaveLoadReset");
                Assert.Catch<Autodesk.Revit.Exceptions.ArgumentException>(() => storageProjectInfo.Save(document, value));
                Assert.Catch<Autodesk.Revit.Exceptions.ArgumentException>(() => storageProjectInfo.Load(document));
                transaction.Commit();
            }
        }


        [Test]
        public void Show_Schemas()
        {
            foreach (var schema in Schema.ListSchemas().OrderBy(e => e.VendorId))
            {
                Console.WriteLine(AsString(schema));
            }
        }

        //#if NET48
        //        [Test]
        //        public void Erase_Schemas()
        //        {
        //            foreach (var schema in Schema.ListSchemas())
        //            {
        //                try
        //                {
        //                    Schema.EraseSchemaAndAllEntities(schema, false);
        //                    Console.WriteLine(AsString(schema));
        //                }
        //                catch { }
        //            }   
        //            Assert.Ignore("EraseSchemaAndAllEntities does not work :(");
        //        }
        //#endif

        string AsString(Schema schema)
        {
            return $"Schema: {schema.SchemaName} - {schema.GUID} - {schema.VendorId}";
        }
    }
}
