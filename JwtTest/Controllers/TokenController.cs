using JwtAuthentication.Server.Models;
using JwtTest.Models;
using JwtTest.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly UserContext _userContext; 
        private readonly ITokenService _tokenService;
 
     public TokenController(UserContext userContext, ITokenService tokenService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
           
        }

        [HttpPost]
        [Route("refresh")]

        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel == null)
                return BadRequest("Invalid Client Request");


            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principle = _tokenService.GetPrincipleFromExpiredToken(accessToken);
            var username = principle.Identity.Name;

            var user = _userContext.LoginModels.SingleOrDefault(u => u.UserName == username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Client Request");

            var newAccessToken = _tokenService.GenerateAccessToken(principle.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _userContext.SaveChanges();

            return Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
            });




        }

        [HttpPost,Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;

            var user = _userContext.LoginModels.SingleOrDefault(u => u.UserName == username);

            user.RefreshToken = null;

            _userContext.SaveChanges();

            return NoContent();
        }
    }
}
