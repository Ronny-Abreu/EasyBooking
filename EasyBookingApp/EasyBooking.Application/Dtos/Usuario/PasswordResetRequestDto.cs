﻿
namespace EasyBooking.Application.Dtos
{
    public class PasswordResetRequestDto
    {
        public string Email { get; set; }
        public int? UserId { get; set; }
    }

}
