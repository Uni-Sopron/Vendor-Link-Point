using Microsoft.EntityFrameworkCore;

namespace Vendor_Link_Point.Data
{
    public class VendorLinkPointContext : DbContext
    {
        public VendorLinkPointContext(DbContextOptions<VendorLinkPointContext> options) : base(options)
        {

        }

    }
}
