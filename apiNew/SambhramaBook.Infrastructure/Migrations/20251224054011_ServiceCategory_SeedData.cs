using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SambhramaBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServiceCategory_SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO service_categories (code, display_name, description, icon_url, background_image_url, theme_color, display_order, is_active, created_at, updated_at)
                VALUES
                    ('photography', 'Photography', 'Professional wedding photographers capturing your special moments with artistic vision and technical expertise. From candid shots to traditional poses, we ensure every precious memory is beautifully preserved.', 
                     'https://images.unsplash.com/photo-1492684223066-81342ee5ff30?w=200&h=200&fit=crop', 
                     'https://images.unsplash.com/photo-1492684223066-81342ee5ff30?w=1200&h=600&fit=crop', 
                     '#4A90E2', 1, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    
                    ('catering', 'Catering', 'Delicious wedding catering services offering a wide variety of cuisines to delight your guests. From traditional South Indian delicacies to continental favorites, we create memorable dining experiences.', 
                     'https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=200&h=200&fit=crop', 
                     'https://images.unsplash.com/photo-1556910103-1c02745aae4d?w=1200&h=600&fit=crop', 
                     '#E74C3C', 2, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    
                    ('decoration', 'Decoration', 'Beautiful wedding decorations transforming your venue into a magical space. From elegant floral arrangements to stunning backdrops and lighting, we create the perfect ambiance for your celebration.', 
                     'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=200&h=200&fit=crop', 
                     'https://images.unsplash.com/photo-1519167758481-83f550bb49b3?w=1200&h=600&fit=crop', 
                     '#9B59B6', 3, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    
                    ('music', 'Music & DJ', 'Wedding music and DJ services to keep your celebration alive with the perfect soundtrack. From traditional music to modern beats, we ensure your guests dance the night away.', 
                     'https://images.unsplash.com/photo-1470225620780-dba8ba36b745?w=200&h=200&fit=crop', 
                     'https://images.unsplash.com/photo-1470225620780-dba8ba36b745?w=1200&h=600&fit=crop', 
                     '#F39C12', 4, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    
                    ('makeup', 'Makeup & Beauty', 'Professional makeup artists and beauty experts to make you look stunning on your special day. From bridal makeup to hair styling, we enhance your natural beauty with expert techniques.', 
                     'https://images.unsplash.com/photo-1522337360788-8b13dee7a37e?w=200&h=200&fit=crop', 
                     'https://images.unsplash.com/photo-1522337360788-8b13dee7a37e?w=1200&h=600&fit=crop', 
                     '#E91E63', 5, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
                    
                    ('videography', 'Videography', 'Wedding video services capturing your love story in cinematic style. From highlight reels to full-length documentaries, we preserve your special moments with professional videography and editing.', 
                     'https://images.unsplash.com/photo-1485846234645-a62644f84728?w=200&h=200&fit=crop', 
                     'https://images.unsplash.com/photo-1485846234645-a62644f84728?w=1200&h=600&fit=crop', 
                     '#1ABC9C', 6, true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "service_categories");
        }
    }
}
