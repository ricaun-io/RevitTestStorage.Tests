using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Linq;
using System.Collections.Generic;

// Based: https://github.com/ricaun-io/RevitAddin.StorageExample

namespace RevitTestStorage.Tests.Services
{
    public abstract class StorageFactory<FieldType> : StorageFactory<FieldType, Element>
    {

    }

    public abstract class StorageProjectInfoFactory<FieldType> : StorageFactory<FieldType, ProjectInfo>
    {
        public void Save(Document document, FieldType data)
        {
            ProjectInfo projectInfo = GetProjectInfo(document);
            Save(projectInfo, data);
        }

        public FieldType Load(Document document)
        {
            ProjectInfo projectInfo = GetProjectInfo(document);
            return Load(projectInfo);
        }

        public void Reset(Document document)
        {
            ProjectInfo projectInfo = GetProjectInfo(document);
            Reset(projectInfo);
        }

        public ProjectInfo GetProjectInfo(Document document) => Select(document).First();
        public override IEnumerable<ProjectInfo> Select(Document document)
        {
            return new[] { document.ProjectInformation };
        }
    }

    public abstract class StorageFactory<FieldType, TElement> where TElement : Element
    {
        #region Schema
        public abstract Guid Guid { get; }
        public abstract string SchemaName { get; }
        public abstract string VendorId { get; }
        public virtual string Documentation { get; } = string.Empty;
        public virtual AccessLevel ReadAccessLevel { get; } = AccessLevel.Public;
        public virtual AccessLevel WriteAccessLevel { get; } = AccessLevel.Public;
        public virtual Guid ApplicationGuid { get; } = default;
        public abstract string FieldName { get; }
        #endregion

        #region Save / Load / Reset
        public void Save(TElement element, FieldType data)
        {
            using (var entity = this.GetSchemaEntity(element))
            {
                entity.Set(this.FieldName, data);
                element.SetEntity(entity);
            }
        }
        public FieldType Load(TElement element)
        {
            using (var entity = this.GetSchemaEntity(element))
            {
                var storage = entity.Get<FieldType>(this.FieldName);
                return storage;
            }
        }

        public void Reset(TElement element)
        {
            using (var entity = this.GetSchemaEntity(element))
            {
                element.DeleteEntity(entity.Schema);
            }
        }

        #endregion

        #region Select
        public virtual IEnumerable<TElement> Select(Document document)
        {
            var filter = new ExtensibleStorageFilter(Guid);
            var elements = new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .OfClass(typeof(TElement))
                .Cast<TElement>();
            return elements;
        }
        #endregion

        #region Private
        private Entity GetSchemaEntity(Element element)
        {
            var schema = Schema.Lookup(Guid);

            if (schema == null)
            {
                schema = this.CreateSchema(Guid);
            }

            var entity = element.GetEntity(schema);
            if (entity.Schema == null)
            {
                entity = new Entity(schema);
            }

            return entity;
        }

        private Schema CreateSchema(Guid Guid)
        {
            SchemaBuilder builder = new SchemaBuilder(Guid);
            if (this.ApplicationGuid != default)
                builder.SetApplicationGUID(this.ApplicationGuid);

            builder.SetReadAccessLevel(this.ReadAccessLevel);
            builder.SetWriteAccessLevel(this.WriteAccessLevel);
            builder.SetSchemaName(this.SchemaName);
            builder.SetVendorId(this.VendorId);
            builder.SetDocumentation(this.Documentation);
            builder.AddSimpleField(this.FieldName, typeof(FieldType));
            return builder.Finish();
        }
        #endregion
    }
}