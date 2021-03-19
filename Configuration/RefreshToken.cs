using System;

namespace SSocial.Configuration
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }

        public string Token { get; set; }
         
        public Guid UserId { get; set; }

        public DateTime ExpiryOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime RevokedOn { get; set; }
        
        public string RevokedByIp { get; set; }
    }
}