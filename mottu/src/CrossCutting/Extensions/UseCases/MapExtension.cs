using System;
using System.Linq;

namespace CrossCutting.Extensions.UseCases;

public static class MapExtension
{
    public static void Map<TSource, TDestination>(
        this TSource source,
        TDestination destination)
    {
        var sourceProperties = typeof(TSource).GetProperties();

        var destinationProperties = typeof(TDestination).GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty =
                destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name &&
                                                           p.PropertyType == sourceProperty.PropertyType);

            if (destinationProperty != null && destinationProperty.CanWrite)
            {
                var value = sourceProperty.GetValue(source);

                if (value != null && !(value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    if (value is DateTime dateTimeValue && dateTimeValue == DateTime.MinValue)
                        continue;

                    destinationProperty.SetValue(destination, value);
                }
            }
        }
    }
}