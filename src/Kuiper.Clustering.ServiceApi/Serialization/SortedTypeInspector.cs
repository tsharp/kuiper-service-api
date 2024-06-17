using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Kuiper.Clustering.ServiceApi.Serialization
{
    public class SortedTypeInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector _innerTypeInspector;

        public SortedTypeInspector(ITypeInspector innerTypeInspector)
        {
            _innerTypeInspector = innerTypeInspector;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            return _innerTypeInspector
                .GetProperties(type, container)
                .OrderBy(x => x.GetCustomAttribute<DataMemberAttribute>()?.Order ?? int.MaxValue);
        }
    }

}
