using System;
using Apizr.Mapping;
using AutoMapper;

[assembly: Apizr.Preserve]
namespace Apizr
{
    /// <summary>
    /// AutoMapper mapping handler implementation
    /// </summary>
    public class AutoMapperMappingHandler : IMappingHandler
    {
        private readonly IMapper _mapper;

        public AutoMapperMappingHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <inheritdoc />
        public TDestination Map<TDestination>(object source) => _mapper.Map<TDestination>(source);

        /// <inheritdoc />
        public TDestination Map<TSource, TDestination>(TSource source) => _mapper.Map<TSource, TDestination>(source);

        /// <inheritdoc />
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => _mapper.Map(source, destination);

        /// <inheritdoc />
        public object Map(object source, Type sourceType, Type destinationType) => _mapper.Map(source, sourceType, destinationType);

        /// <inheritdoc />
        public object Map(object source, object destination, Type sourceType, Type destinationType) => _mapper.Map(source, destination, sourceType, destinationType);
    }
}
