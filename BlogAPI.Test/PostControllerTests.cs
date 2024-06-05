using AutoMapper;
using BlogAPI.Controllers;
using BlogAPI.DTO;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BlogAPI.Test
{
    public class PostControllerTests
    {
        private readonly IMapper _mapper;
        private readonly BlogDbContext _context;
        private readonly Mock<ILogger<PostsController>> _logger;
        public PostControllerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _logger = new Mock<ILogger<PostsController>>();
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "BlogDB")
                .Options;

            _context = new BlogDbContext(options);
            _context.Database.EnsureCreated();
            _context.Database.EnsureDeleted();
        }



        [Fact]
        public async Task GetPosts_ReturnsOkResult_WithListOfPostDTOs()
        {       
            // Arrange
            var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1", CategoryId = 1, AuthorId = 1 },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2", CategoryId = 1, AuthorId = 2 }
        };
            _context.Posts.AddRange(posts);
            _context.SaveChanges();

            var controller = new PostsController(_context, _mapper, _logger.Object);

            // Act
            var result = await controller.GetPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var postDTOs = Assert.IsType<List<PostDTO>>(okResult.Value);
            Assert.Equal(2, postDTOs.Count);
        }

        [Fact]
        public async Task GetPostById_ReturnsOkResult_WithPostDTO()
        {           
            // Arrange
            var post = new Post { Id = 1, Title = "Post 1", Content = "Content 1", CategoryId = 1, AuthorId = 1 };
            _context.Posts.Add(post);
            _context.SaveChanges();

            var controller = new PostsController(_context, _mapper, _logger.Object);

            // Act
            var result = await controller.GetPost(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var postDTO = Assert.IsType<PostDTO>(okResult.Value);
            Assert.Equal(1, postDTO.Id);
        }

        [Fact]
        public async Task GetPostById_ReturnsNotFoundResult_WhenPostDoesNotExist()
        {          
            // Arrange
            var controller = new PostsController(_context, _mapper, _logger.Object);

            // Act
            var result = await controller.GetPost(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
