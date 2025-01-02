using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Seed;

public static class SeedData
{
    public async static Task<WebApplication> Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            using var context = serviceProvider.GetRequiredService<DatabaseContext>();
            
            context.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var categoryRepository = serviceProvider.GetRequiredService<ICategoryRepository>();
            var courseRepository = serviceProvider.GetRequiredService<ICourseRepository>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            // Seed Roles
            if (!roleManager.Roles.Any())
            {
                if (!await roleManager.RoleExistsAsync("admin"))
                {
                    var adminRole = ApplicationRole.Create("admin");
                    await roleManager.CreateAsync(adminRole);
                }
                if (!await roleManager.RoleExistsAsync("user"))
                {
                    var userRole = ApplicationRole.Create("user");
                    await roleManager.CreateAsync(userRole);
                }
                if (!await roleManager.RoleExistsAsync("instructor"))
                {
                    var instructorRole = ApplicationRole.Create("instructor");
                    await roleManager.CreateAsync(instructorRole);
                }
            }

            // Seed Users
            if (!userManager.Users.Any())
            {
                var password = "Test*123";
                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    var admin = ApplicationUser.Create("System", "Administrator", "admin@admin.com", "admin");
                    admin.EmailConfirmed = true;
                    admin.Id = new Guid("7608d5cf-cbca-4730-a1ee-6cdfbe64c053");
                    var result = await userManager.CreateAsync(admin, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "admin");
                        await userManager.AddToRoleAsync(admin, "user");
                        await userManager.AddToRoleAsync(admin, "instructor");
                    }
                }
                if (await userManager.FindByEmailAsync("murat@test.com") == null)
                {
                    var user = ApplicationUser.Create("Murat", "Cinek", "murat@test.com", "muratcinek");
                    user.EmailConfirmed = true;
                    user.Id = new Guid("00859d34-c5c1-45f5-9573-b7ec668471ce");
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "user");
                    }
                }
                if (await userManager.FindByEmailAsync("fatih@test.com") == null)
                {
                    var instructor1 = ApplicationUser.Create("Fatih", "Çakıroğlu", "fatih@test.com", "instructor1");
                    instructor1.EmailConfirmed = true;
                    instructor1.Id = new Guid("2c54db2f-22bf-43b2-8ed0-296ee57b5500");
                    var result = await userManager.CreateAsync(instructor1, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(instructor1, "user");
                        await userManager.AddToRoleAsync(instructor1, "instructor");
                    }
                }
                if (await userManager.FindByEmailAsync("ahmet@test.com") == null)
                {
                    var instructor2 = ApplicationUser.Create("Ahmet", "Kaya", "ahmet@test.com", "instructor2");
                    instructor2.EmailConfirmed = true;
                    instructor2.Id = new Guid("cdd009df-15dc-4a0b-8cb3-4dc7a723f348");
                    var result = await userManager.CreateAsync(instructor2, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(instructor2, "user");
                        await userManager.AddToRoleAsync(instructor2, "instructor");
                    }
                }
            }

            // Seed Categories
            if (!categoryRepository.FindAll().Any())
            {
                var category1 = Category.Create("Software and Technology");
                category1.Id = new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2");
                var category2 = Category.Create("Business and Management");
                category2.Id = new Guid("b7b6d770-f6cf-4d39-808b-db39b954ab6c");
                var category3 = Category.Create("Personal Development");
                category3.Id = new Guid("7230827c-1d03-4afb-9c8a-fd7976557113");
                var category4 = Category.Create("Design");
                category4.Id = new Guid("885af5d0-bd08-4f56-8c30-76534ed8509e");
                var category5 = Category.Create("Language and Education");
                category5.Id = new Guid("16e47d6c-3f21-4203-b9bf-e34f7a812d1f");

                await categoryRepository.CreateAsync(category1);
                await categoryRepository.CreateAsync(category2);
                await categoryRepository.CreateAsync(category3);
                await categoryRepository.CreateAsync(category4);
                await categoryRepository.CreateAsync(category5);

                await unitOfWork.SaveChangesAsync();
            }

            // Seed Courses
            if (!courseRepository.FindAll().Any())
            {
                var course1 = Course.Create("Microservices with .Net", "Learn how to develop microservice architecture with .Net 7.", 270, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("2c54db2f-22bf-43b2-8ed0-296ee57b5500"));
                var course2 = Course.Create("Elasticsearch | Net Core", "Learn Elasticsearch in all its aspects.", 250, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("2c54db2f-22bf-43b2-8ed0-296ee57b5500"));
                var course3 = Course.Create("RabbitMQ | Net Core", "Learn RabbitMQ message queue system comprehensively and build efficient projects.", 250, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("2c54db2f-22bf-43b2-8ed0-296ee57b5500"));
                var course4 = Course.Create("Redis | Net Core", "Learn Redis (Distributed Cache) and In-Memory cache structure step by step from scratch.", 240, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("2c54db2f-22bf-43b2-8ed0-296ee57b5500"));
                var course5 = Course.Create("Asp.Net Core + Docker and Docker Compose", "Learn how to dockerize your Asp.Net Core projects step by step from scratch.", 400, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("2c54db2f-22bf-43b2-8ed0-296ee57b5500"));

                var course6 = Course.Create("Learn Web Dynamics with JavaScript and React", "Develop dynamic web applications with modern JavaScript and React.", 250, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("cdd009df-15dc-4a0b-8cb3-4dc7a723f348"));
                var course7 = Course.Create("C# From Beginner to Advanced", "The best resource to learn C# in detail and improve yourself.", 300, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("cdd009df-15dc-4a0b-8cb3-4dc7a723f348"));
                var course8 = Course.Create("Unity C#: Game Design from Beginner to Advanced", "Reshape creative mobile worlds: embark on a game development journey with Unity.", 300, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("cdd009df-15dc-4a0b-8cb3-4dc7a723f348"));
                var course9 = Course.Create("Arduino: From Beginner to Advanced", "Ready for robotic coding with Arduino through detailed explanations and practical projects?", 250, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("cdd009df-15dc-4a0b-8cb3-4dc7a723f348"));
                var course10 = Course.Create("Android App Development with JAVA", "Learn Android programming from scratch with Android Studio, Java, and SQLite.", 250, null, new Guid("ce6278d2-40e7-465b-a2ce-da2f0c6b05e2"), new Guid("cdd009df-15dc-4a0b-8cb3-4dc7a723f348"));

                var course11 = Course.Create("The English Master Course: English Grammar, English Speaking", "A Complete English Language Course: English Grammar, Speaking, Pronunciation, and Writing. British and American English.", 300, null, new Guid("16e47d6c-3f21-4203-b9bf-e34f7a812d1f"), new Guid("7608d5cf-cbca-4730-a1ee-6cdfbe64c053"));

                var course12 = Course.Create("UI/UX Design Essentials", "Master the fundamentals of user interface and user experience design to create visually stunning and user-friendly digital products.", 350, null, new Guid("885af5d0-bd08-4f56-8c30-76534ed8509e"), new Guid("7608d5cf-cbca-4730-a1ee-6cdfbe64c053"));

                var course13 = Course.Create("Personal Growth Mastery", "Learn essential skills to boost your confidence, improve time management, and achieve personal and professional success.", 400, null, new Guid("7230827c-1d03-4afb-9c8a-fd7976557113"), new Guid("7608d5cf-cbca-4730-a1ee-6cdfbe64c053"));

                var course14 = Course.Create("Effective Business Management Strategies", "Discover proven techniques and strategies to lead teams, manage projects, and drive organizational success.", 375, null, new Guid("b7b6d770-f6cf-4d39-808b-db39b954ab6c"), new Guid("7608d5cf-cbca-4730-a1ee-6cdfbe64c053"));

                await courseRepository.CreateAsync(course1);
                await courseRepository.CreateAsync(course2);
                await courseRepository.CreateAsync(course3);
                await courseRepository.CreateAsync(course4);
                await courseRepository.CreateAsync(course5);
                await courseRepository.CreateAsync(course6);
                await courseRepository.CreateAsync(course7);
                await courseRepository.CreateAsync(course8);
                await courseRepository.CreateAsync(course9);
                await courseRepository.CreateAsync(course10);
                await courseRepository.CreateAsync(course11);
                await courseRepository.CreateAsync(course12);
                await courseRepository.CreateAsync(course13);
                await courseRepository.CreateAsync(course14);
                await unitOfWork.SaveChangesAsync();
            }

            return app;
        }
    }
}
