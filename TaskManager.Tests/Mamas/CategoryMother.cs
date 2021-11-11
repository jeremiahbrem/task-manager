using TaskManager.Models.Domain.Categories;

namespace TaskManager.Tests.Mamas
{
    public class CategoryMother : IntegrationTestBase
    {
        private readonly string _name;
        public Category Category => CreateCategory();

        public CategoryMother(string name)
        {
            _name = name;
        }

        private Category CreateCategory()
        {
            return new Category
            {
                Name = _name
            };
        }
    }
}