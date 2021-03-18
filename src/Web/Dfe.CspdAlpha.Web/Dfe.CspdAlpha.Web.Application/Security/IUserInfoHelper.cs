using System.Security.Claims;
using System.Threading.Tasks;

namespace Dfe.Rscd.Web.Application.Security
{
    public interface IUserInfoHelper
    {
        UserInfo MapPrincipalToUserInfo(ClaimsPrincipal principal);

        Task<ClaimsPrincipal> HydrateUserClaimsAsync(ClaimsPrincipal principal);
    }
}
