using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Validation;
using TaskManager.Tests.Mamas;
using Xunit;

namespace TaskManager.Tests.Integration.Categories
{
    public class CreatesCategories : IntegrationApiTestBase
    {
        [Fact]
        public async Task CreatesCategoryTest()
        {
            var context = Server.CreateDbContext();
            var mother = new CategoryMother("New Category");
            var content = CreateContent(mother.Category);

            var response = await SendPostRequest("/api/categories/create", content);

            var newCategory = await context.Categories.FirstOrDefaultAsync();
            newCategory.Should().BeEquivalentTo(new { Name = "New Category" });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendsDuplicateCategoryError()
        {
            var context = Server.CreateDbContext();
            await CreateCategory("CategoryOne", context);

            var category = new
            {
                Name = "CategoryOne"
            };

            var content = CreateContent(category);

            var response = await SendPostRequest("/api/categories/create", content);
            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Duplicate category error",
                "A category with name CategoryOne already exists."
            );
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SendsInvalidModelError()
        {
            var category = new
            {
                someKey = "Some property",
            };

            var content = CreateContent(category);
            var response = await SendPostRequest("/api/categories/create", content);

            var result = await GetJsonObject<ValidationResponse>(response);

            var expected = CreateExpectedResponse(
                "Validation failed",
                "The Name field is required."
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            result.Should().BeEquivalentTo(expected);
        }
    }
}