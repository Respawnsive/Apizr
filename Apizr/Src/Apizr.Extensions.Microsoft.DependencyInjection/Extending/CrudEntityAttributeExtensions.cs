using Apizr.Requesting;
using System;
using Apizr.Mapping;

namespace Apizr.Extending
{
    internal static class CrudEntityAttributeExtensions
    {
        internal static CrudEntityAttribute ToCrudEntityAttribute(this MappedCrudEntityAttribute mappedAttribute, Type modelEntityType)
        {
            var attribute = mappedAttribute as CrudEntityAttribute;
            attribute.MappedEntityType = modelEntityType;
            return attribute;
        }
    }
}
