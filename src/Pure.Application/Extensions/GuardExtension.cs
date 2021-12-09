using System;
using Ardalis.GuardClauses;

namespace Pure.Application.Extensions
{
    internal static class GuardExtension
    {
        /// <summary>
        /// Check if the given Guid match with the Guid in the dto.
        /// </summary>
        /// <param name="extensionsClause"></param>
        /// <param name="guid"></param>
        /// <param name="o"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotSameId(this IGuardClause extensionsClause, Guid guid, object o)
        {
            var dtoGuid = o.GetType().GetProperty("Id")?.GetValue(o);

            if (dtoGuid.ToString() != guid.ToString())
            {
                throw new ArgumentException("The ID don't match the one in the entity.");
            }
        }
    }
}