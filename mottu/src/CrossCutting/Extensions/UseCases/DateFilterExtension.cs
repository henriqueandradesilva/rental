using CrossCutting.Common.Dtos.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CrossCutting.Extensions.UseCases;

public static class DateFilterExtension<T> where T : class
{
    public static IQueryable<T> Filter(
       GenericSearchPaginationRequest genericSearchPaginationRequest,
       IQueryable<T> query)
    {
        if (genericSearchPaginationRequest.DataInicio.HasValue)
        {
            query = query.Where(x => EF.Property<DateTime>(x, "DateCreated") >= genericSearchPaginationRequest.DataInicio.Value ||
                                     EF.Property<DateTime>(x, "DateUpdated") >= genericSearchPaginationRequest.DataInicio.Value);
        }

        if (genericSearchPaginationRequest.DataFim.HasValue)
        {
            query = query.Where(x => EF.Property<DateTime>(x, "DateCreated") <= genericSearchPaginationRequest.DataFim.Value ||
                                     EF.Property<DateTime>(x, "DateUpdated") <= genericSearchPaginationRequest.DataFim.Value);
        }

        return query;
    }
}