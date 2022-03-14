using System;
using Mapr.Exceptions;

namespace Mapr;

/// <inheritdoc />
public class MapLocator : IMapLocator
{
    private readonly MapFactory _mapFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="MapLocator"/>
    /// </summary>
    /// <param name="mapFactory">A delegate for locating maps.</param>
    public MapLocator(MapFactory mapFactory)
    {
        _mapFactory = mapFactory;
    }

    /// <inheritdoc />
    public IMap<TSource, TDestination> LocateMapFor<TSource, TDestination>() =>
        LocateMap<IMap<TSource, TDestination>, TSource, TDestination>();

    /// <inheritdoc />
    public IComplexMap<TSource, TDestination> LocateComplexMapFor<TSource, TDestination>() =>
        LocateMap<IComplexMap<TSource, TDestination>, TSource, TDestination>();

    private TMap LocateMap<TMap, TSource, TDestination>() where TMap : IMap<TSource, TDestination>
    {
        var mapType = typeof(TMap);

        try
        {
            if (_mapFactory(mapType) is not TMap typeMap)
            {
                throw new MapNotFoundException(mapType);
            }

            return typeMap;
        }
        catch (Exception ex)
        {
            throw new MapLocatorException(mapType, ex);
        }
    }
}
