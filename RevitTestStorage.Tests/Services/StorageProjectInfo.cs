using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;

namespace RevitTestStorage.Tests.Services
{
    public class StorageProjectInfo : StorageProjectInfoFactory<string>
    {
        public override Guid Guid => new Guid("1E832DE4-C3F8-4DAE-84AF-450BAF593AC4");
        public override string FieldName => "Text";
        public override string SchemaName => "StorageProjectInfo";
        public override string VendorId => "ricaun";
        public override AccessLevel ReadAccessLevel => AccessLevel.Vendor;
        public override AccessLevel WriteAccessLevel => AccessLevel.Vendor;
    }

    public class StorageProjectInfoVendor : StorageProjectInfoFactory<string>
    {
        public override Guid Guid => new Guid("1E832DE4-C3F8-4DAE-84AF-450BAF593AC5");
        public override string FieldName => "Vendor";
        public override string SchemaName => "StorageProjectInfoVendor";
        public override string VendorId => "Vendor";
        public override AccessLevel ReadAccessLevel => AccessLevel.Vendor;
        public override AccessLevel WriteAccessLevel => AccessLevel.Vendor;
    }

    public class StorageProjectInfoApplication : StorageProjectInfoFactory<string>
    {
        public override Guid Guid => new Guid("1E832DE4-C3F8-4DAE-84AF-450BAF593AC6");
        public override string FieldName => "Application";
        public override string SchemaName => "StorageProjectInfoApplication";
        public override string VendorId => "ricaun";
        public override Guid ApplicationGuid => new Guid("65f304b3-8efb-464d-b08a-12cfd61a1986"); // ricaun.RevitTest.Application
        public override AccessLevel ReadAccessLevel => AccessLevel.Application; // VendorId and ApplicationGuid are required
        public override AccessLevel WriteAccessLevel => AccessLevel.Application; // VendorId and ApplicationGuid are required
    }
}