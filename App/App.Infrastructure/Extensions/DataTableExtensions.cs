
using App.Domain.Models.Request;
using App.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore; 
using System.Linq.Expressions;
using System.Reflection;

namespace App.Infrastructure.Extensions
{

    public static class DataTableExtensions
    {
        public static IQueryable<T> ApplyDataTableSorting<T>(this IQueryable<T> query, DataTableRequest request)
        {
            if (request == null || request.Order == null || request.Order.Count <= 0) return query;
            var column = request.Columns[request.Order[0].Column];
            if (!column.Orderable) return query;
            var propertyInfo = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == column.Data.ToLower());
            //var propertyInfo = typeof(T).GetProperty(column.Data);
            if (propertyInfo == null) return query;
            var parameter = Expression.Parameter(typeof(T));
            var propertyExpression = Expression.Property(parameter, propertyInfo);
            var orderByExpression = Expression.Lambda(propertyExpression, parameter);

            if (request.Order[0].Dir == "asc")
            {
                query = query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        "OrderBy",
                        new Type[] { typeof(T), propertyInfo.PropertyType },
                        query.Expression,
                        orderByExpression));
            }
            else
            {
                query = query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        "OrderByDescending",
                        new Type[] { typeof(T), propertyInfo.PropertyType },
                        query.Expression,
                        orderByExpression));
            }

            return query;
        }
        //public static IQueryable<T> ApplyDataTableSorting<T>(this IQueryable<T> query, DataTableRequest request)
        //{
        //    if (request != null && request.Order != null && request.Order.Count > 0)
        //    {
        //        var column = request.Columns[request.Order[0].Column];
        //        if (column.Orderable)
        //        {
        //            var propertyInfo = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == column.Data.ToLower());
        //            //var propertyInfo = typeof(T).GetProperty(column.Data);
        //            if (propertyInfo != null)
        //            {
        //                var parameter = Expression.Parameter(typeof(T));
        //                var propertyExpression = Expression.Property(parameter, propertyInfo);
        //                var orderByExpression = Expression.Lambda(propertyExpression, parameter);

        //                if (request.Order[0].Dir == "asc")
        //                {
        //                    query = query.Provider.CreateQuery<T>(
        //                        Expression.Call(
        //                            typeof(Queryable),
        //                            "OrderBy",
        //                            new Type[] { typeof(T), propertyInfo.PropertyType },
        //                            query.Expression,
        //                            orderByExpression));
        //                }
        //                else
        //                {
        //                    query = query.Provider.CreateQuery<T>(
        //                        Expression.Call(
        //                            typeof(Queryable),
        //                            "OrderByDescending",
        //                            new Type[] { typeof(T), propertyInfo.PropertyType },
        //                            query.Expression,
        //                            orderByExpression));
        //                }
        //            }
        //        }
        //    }

        //    return query;
        //}


        public static IQueryable<T> ApplyDataTablePaging<T>(this IQueryable<T> query, DataTableRequest request)
        {
            if (request != null && request.Length > 0)
            {
                return query.Skip(request.Start).Take(request.Length);
            }

            return query;
        }

        public static IQueryable<T> ApplyDataTableFilters<T>(
    this IQueryable<T> query,
    DataTableRequest request)
        {
            return ApplyDataTableFilters(query, request, enableColumnSearch: true);
        }

        public static IQueryable<T> ApplyDataTableFilters<T>(
            this IQueryable<T> query,
            DataTableRequest request,
            bool enableColumnSearch)
        {
            if (request == null)
                return query;

            return ApplyFiltersInternal(query, request, enableColumnSearch);
        }


        private static IQueryable<T> ApplyFiltersInternal<T>(
    IQueryable<T> query,
    DataTableRequest request,
    bool enableColumnSearch)
        {
            var predicate = PredicateBuilder.True<T>();

            var type = typeof(T);
            // ===============================
            // COLUMN SEARCH
            // ===============================
            predicate = columnSearch();

            // ===============================
            // GLOBAL SEARCH
            // ===============================
            predicate = GlobalSearch();

            return query.Where(predicate);


            Expression<Func<T, bool>> columnSearch()
            {
                if (!enableColumnSearch) return predicate;
                if (request.Columns == null) return predicate;
                foreach (var column in request.Columns)
                {
                    if (!column.Searchable || string.IsNullOrWhiteSpace(column.Search?.Value))
                        continue;

                    var propertyInfo = type.GetProperty(
                        column.Data,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                        continue;

                    var value = column.Search.Value.Trim();

                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        predicate = predicate.And(e =>
                            EF.Functions.Like(
                                EF.Property<string>(e, propertyInfo.Name),
                                $"%{value}%"));
                    }
                    else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                    {
                        if (int.TryParse(value, out var intValue))
                        {
                            predicate = predicate.And(e =>
                                EF.Property<int?>(e, propertyInfo.Name) == intValue);
                        }
                    }
                }

                return predicate;
            }

            Expression<Func<T, bool>> GlobalSearch()
            {
                if (string.IsNullOrWhiteSpace(request.Search?.Value)) return predicate;
                var globalValue = request.Search.Value.Trim();
                var globalPredicate = PredicateBuilder.False<T>();

                foreach (var column in request.Columns.Where(c => c.Searchable))
                {
                    var propertyInfo = type.GetProperty(
                        column.Data,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null || propertyInfo.PropertyType != typeof(string))
                        continue;

                    globalPredicate = globalPredicate.Or(e =>
                        EF.Functions.Like(
                            EF.Property<string>(e, propertyInfo.Name),
                            $"%{globalValue}%"));
                }

                predicate = predicate.And(globalPredicate);

                return predicate;
            }
        }
    }

    //public static class DataTableExtensions
    //{
    //    public static IQueryable<T> ApplyDataTableSorting<T>(this IQueryable<T> query, Domain.Models.Request.DataTableRequest request)
    //    {
    //        if (request == null || request.Order == null || request.Order.Count <= 0) return query;
    //        var column = request.Columns[request.Order[0].Column];
    //        if (!column.Orderable) return query;
    //        var propertyInfo = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == column.Data.ToLower());
    //        //var propertyInfo = typeof(T).GetProperty(column.Data);
    //        if (propertyInfo == null) return query;
    //        var parameter = Expression.Parameter(typeof(T));
    //        var propertyExpression = Expression.Property(parameter, propertyInfo);
    //        var orderByExpression = Expression.Lambda(propertyExpression, parameter);

    //        if (request.Order[0].Dir == "asc")
    //        {
    //            query = query.Provider.CreateQuery<T>(
    //                Expression.Call(
    //                    typeof(Queryable),
    //                    "OrderBy",
    //                    new Type[] { typeof(T), propertyInfo.PropertyType },
    //                    query.Expression,
    //                    orderByExpression));
    //        }
    //        else
    //        {
    //            query = query.Provider.CreateQuery<T>(
    //                Expression.Call(
    //                    typeof(Queryable),
    //                    "OrderByDescending",
    //                    new Type[] { typeof(T), propertyInfo.PropertyType },
    //                    query.Expression,
    //                    orderByExpression));
    //        }

    //        return query;
    //    }
    //    //public static IQueryable<T> ApplyDataTableSorting<T>(this IQueryable<T> query, DataTableRequest request)
    //    //{
    //    //    if (request != null && request.Order != null && request.Order.Count > 0)
    //    //    {
    //    //        var column = request.Columns[request.Order[0].Column];
    //    //        if (column.Orderable)
    //    //        {
    //    //            var propertyInfo = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == column.Data.ToLower());
    //    //            //var propertyInfo = typeof(T).GetProperty(column.Data);
    //    //            if (propertyInfo != null)
    //    //            {
    //    //                var parameter = Expression.Parameter(typeof(T));
    //    //                var propertyExpression = Expression.Property(parameter, propertyInfo);
    //    //                var orderByExpression = Expression.Lambda(propertyExpression, parameter);

    //    //                if (request.Order[0].Dir == "asc")
    //    //                {
    //    //                    query = query.Provider.CreateQuery<T>(
    //    //                        Expression.Call(
    //    //                            typeof(Queryable),
    //    //                            "OrderBy",
    //    //                            new Type[] { typeof(T), propertyInfo.PropertyType },
    //    //                            query.Expression,
    //    //                            orderByExpression));
    //    //                }
    //    //                else
    //    //                {
    //    //                    query = query.Provider.CreateQuery<T>(
    //    //                        Expression.Call(
    //    //                            typeof(Queryable),
    //    //                            "OrderByDescending",
    //    //                            new Type[] { typeof(T), propertyInfo.PropertyType },
    //    //                            query.Expression,
    //    //                            orderByExpression));
    //    //                }
    //    //            }
    //    //        }
    //    //    }

    //    //    return query;
    //    //}


    //    public static IQueryable<T> ApplyDataTablePaging<T>(this IQueryable<T> query, DataTableRequest request)
    //    {
    //        if (request != null && request.Length > 0)
    //        {
    //            return query.Skip(request.Start).Take(request.Length);
    //        }

    //        return query;
    //    }

    //    public static IQueryable<T> ApplyDataTableFilters<T>(
    //this IQueryable<T> query,
    //DataTableRequest request)
    //    {
    //        return ApplyDataTableFilters(query, request, enableColumnSearch: true);
    //    }

    //    public static IQueryable<T> ApplyDataTableFilters<T>(
    //        this IQueryable<T> query,
    //        DataTableRequest request,
    //        bool enableColumnSearch)
    //    {
    //        if (request == null)
    //            return query;

    //        return ApplyFiltersInternal(query, request, enableColumnSearch);
    //    }


    //    private static IQueryable<T> ApplyFiltersInternal<T>(
    //IQueryable<T> query,
    //DataTableRequest request,
    //bool enableColumnSearch)
    //    {
    //        var predicate = PredicateBuilder.True<T>();

    //        var type = typeof(T);
    //        // ===============================
    //        // COLUMN SEARCH
    //        // ===============================
    //        predicate = columnSearch();

    //        // ===============================
    //        // GLOBAL SEARCH
    //        // ===============================
    //        predicate = GlobalSearch();

    //        return query.Where(predicate);


    //        Expression<Func<T, bool>> columnSearch()
    //        {
    //            if (!enableColumnSearch) return predicate;
    //            if (request.Columns == null) return predicate;
    //            foreach (var column in request.Columns)
    //            {
    //                if (!column.Searchable || string.IsNullOrWhiteSpace(column.Search?.Value))
    //                    continue;

    //                var propertyInfo = type.GetProperty(
    //                    column.Data,
    //                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

    //                if (propertyInfo == null)
    //                    continue;

    //                var value = column.Search.Value.Trim();

    //                if (propertyInfo.PropertyType == typeof(string))
    //                {
    //                    predicate = predicate.And(e =>
    //                        EF.Functions.Like(
    //                            EF.Property<string>(e, propertyInfo.Name),
    //                            $"%{value}%"));
    //                }
    //                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
    //                {
    //                    if (int.TryParse(value, out var intValue))
    //                    {
    //                        predicate = predicate.And(e =>
    //                            EF.Property<int?>(e, propertyInfo.Name) == intValue);
    //                    }
    //                }
    //            }

    //            return predicate;
    //        }

    //        Expression<Func<T, bool>> GlobalSearch()
    //        {
    //            if (string.IsNullOrWhiteSpace(request.Search?.Value)) return predicate;
    //            var globalValue = request.Search.Value.Trim();
    //            var globalPredicate = PredicateBuilder.False<T>();

    //            foreach (var column in request.Columns.Where(c => c.Searchable))
    //            {
    //                var propertyInfo = type.GetProperty(
    //                    column.Data,
    //                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

    //                if (propertyInfo == null || propertyInfo.PropertyType != typeof(string))
    //                    continue;

    //                globalPredicate = globalPredicate.Or(e =>
    //                    EF.Functions.Like(
    //                        EF.Property<string>(e, propertyInfo.Name),
    //                        $"%{globalValue}%"));
    //            }

    //            predicate = predicate.And(globalPredicate);

    //            return predicate;
    //        }
    //    }
    //}

}
