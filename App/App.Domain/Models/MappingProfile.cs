//using App.Domain.Models.Attributes;
//using AutoMapper; 
//using System.Reflection;
//using System.Reflection.Metadata; 

//namespace App.Domain.Models
//{

//    public class MappingProfile : Profile
//    {
//        public MappingProfile()
//        {
//            string name = typeof(MappingProfile).Assembly.FullName;
//            ApplyMappingsFromAssembly(typeof(MappingProfile).Assembly);
//        }

//        /// <summary>
//        /// auto mapping
//        /// </summary>
//        /// <param name="assembly"></param>
//        private void ApplyMappingsFromAssembly(Assembly assembly)
//        {
//            var dtoTypes = assembly.GetTypes()
//                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(BaseDto<>));

//            foreach (var dtoType in dtoTypes)
//            {
//                var entityType = dtoType.BaseType!.GetGenericArguments()[0];
//                var map = CreateMap(entityType, dtoType);
//                var reverseMap = map.ReverseMap();
//                var entityPropertyNames = entityType
//                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
//                    .Select(p => p.Name)
//                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

//                foreach (var dtoProperty in dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
//                {

//                    if (!entityPropertyNames.Contains(dtoProperty.Name))
//                        continue;

//                    if (dtoProperty.GetCustomAttributes(typeof(IgnoreMappingAttribute), true).Any())
//                    {
//                        map.ForMember(dtoProperty.Name, opt => opt.Ignore());
//                        reverseMap.ForMember(dtoProperty.Name, opt => opt.Ignore());
//                    }
//                }
//            }
//        }
//    }
//}
