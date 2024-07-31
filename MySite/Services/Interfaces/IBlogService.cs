﻿using MySite.Shared.Dtos;

namespace MySite.Services.Interfaces;

public interface IBlogService
{
    Task<List<BlogPostDto>?> GetAllBlogPostsAsync(int pageNumber);
    Task<BlogPostDto?> GetBlogPostAsync(string urlSlug);
}
