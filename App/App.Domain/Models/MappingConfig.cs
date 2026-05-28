using App.Domain.Models.Attributes;
using Mapster;
using System.Reflection;

namespace App.Domain.Models
{
    public static class MappingConfig
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {
            ApplyMappingsFromAssembly(config, typeof(MappingConfig).Assembly);
        }

        /// <summary>
        /// Auto mapping registration
        /// </summary>
        private static void ApplyMappingsFromAssembly(
            TypeAdapterConfig config,
            Assembly assembly)
        {
            var dtoTypes = assembly.GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericType &&
                    t.BaseType != null &&
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(BaseDto<>));

            foreach (var dtoType in dtoTypes)
            {
                var entityType = dtoType.BaseType!.GetGenericArguments()[0];

                // Create mapping
                var mapConfig = config.NewConfig(entityType, dtoType);

                // Reverse mapping
                var reverseMapConfig = config.NewConfig(dtoType, entityType);

                var entityPropertyNames = entityType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var dtoProperty in dtoType.GetProperties(
                             BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!entityPropertyNames.Contains(dtoProperty.Name))
                        continue;

                    if (dtoProperty
                        .GetCustomAttributes(typeof(IgnoreMappingAttribute), true)
                        .Any())
                    {
                        // Ignore entity -> dto
                        mapConfig.Ignore(dtoProperty.Name);

                        // Ignore dto -> entity
                        reverseMapConfig.Ignore(dtoProperty.Name);
                    }
                }
            }
        }
    }
}