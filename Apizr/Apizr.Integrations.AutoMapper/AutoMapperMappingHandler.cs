using System;
using Apizr.Mapping;
using AutoMapper;

[assembly: Apizr.Preserve]
namespace Apizr
{
    public class AutoMapperMappingHandler : IMappingHandler
    {
        private readonly IMapper _mapper;

        public AutoMapperMappingHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source) => _mapper.Map<TDestination>(source);

        public TDestination Map<TSource, TDestination>(TSource source) => _mapper.Map<TSource, TDestination>(source);

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => _mapper.Map(source, destination);

        public object Map(object source, Type sourceType, Type destinationType) => _mapper.Map(source, sourceType, destinationType);

        public object Map(object source, object destination, Type sourceType, Type destinationType) => _mapper.Map(source, destination, sourceType, destinationType);
    }
}
