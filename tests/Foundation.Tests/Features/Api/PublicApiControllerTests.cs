using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using FluentAssertions;
using Foundation.Cms.Identity;
using Foundation.Cms.Personalization;
using Foundation.Cms.Services;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Features.Api;
using Foundation.Infrastructure.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Foundation.Tests.Features.Api
{
    public class PublicApiControllerTests
    {
        [Fact]
        public void Signout_WhenRedirectingToAction()
        {
            var result = _subject.SignOut();

            _customerService.Verify(_ => _.SignOut(), Times.Once);
            _trackingCookieService.Verify(_ => _.SetTrackingCookie(It.IsAny<string>()), Times.Once);
            
            result.Should().BeOfType<RedirectToRouteResult>().Which.RouteValues["action"].Should().Be("Index");
        }

        [Fact]
        public async Task Login_WhenUserDoesNotExistAsync()
        {
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserEmailStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<SiteUser>(null));
            });
            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            var result = await _subject.LoginAsync("mark", It.IsAny<string>());
            
            _customerService.Verify(s => s.SignOut(), Times.Once);
            result.Should().BeOfType<EmptyResult>();
        }

        [Fact]
        public async Task Login_WhenUserDoesExistAsync()
        {
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserEmailStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(new SiteUser
                {
                    Username = "mark"
                }));
            });
            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.LoginAsync("mark", "http://foundation");

            
            result.Should().BeOfType<RedirectResult>().Which.Url.Should().Be("http://foundation");
        }

        [Fact]
        public async Task RegisterAccount_WhenModelStateisInvalidAsync()
        {
            var viewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };

            _subject.ModelState.AddModelError("test", "not valid");

            var result = await _subject.RegisterAccountAsync(viewModel);

            _customerService.Verify(s => s.CreateUserAsync(It.IsAny<SiteUser>()), Times.Never);
            result.Should().BeEquivalentTo(new JsonResult()
            {
                Data = new
                {
                    success = false,
                    errors = new string[] { "not valid" }
                }
            });
        }

        [Fact]
        public async Task RegisterAccount_WhenSuccessAsync()
        {
            var identityResult = GetIdentityResult();

            _customerService.Setup(x => x.CreateUserAsync(It.IsAny<SiteUser>()))
                .Returns(Task.FromResult(new IdentityContactResult
                {
                    IdentityResult = identityResult,
                    FoundationContact = FoundationContact.New()
                }));

            var viewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };

            var result = await _subject.RegisterAccountAsync(viewModel);

            _customerService.Verify(s => s.CreateUserAsync(It.IsAny<SiteUser>()), Times.Once);
            _addressBookService.Verify(x => x.Save(It.IsAny<AddressModel>(), It.IsAny<FoundationContact>()), Times.Once);
            result.Should().BeOfType<EmptyResult>();
        }

        

        [Fact]
        public async Task RegisterAccount_WhenFailureAsync()
        {
            _customerService.Setup(x => x.CreateUserAsync(It.IsAny<SiteUser>()))
                .Returns(Task.FromResult(new IdentityContactResult
                {
                    IdentityResult = new IdentityResult("error"),
                    FoundationContact = FoundationContact.New()
                }));

            var viewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };

            var result = await _subject.RegisterAccountAsync(viewModel);

            _customerService.Verify(s => s.CreateUserAsync(It.IsAny<SiteUser>()), Times.Once);
            _addressBookService.Verify(x => x.Save(It.IsAny<AddressModel>(), It.IsAny<FoundationContact>()), Times.Never);
            result.Should().BeEquivalentTo(new JsonResult()
            {
                Data = new
                {
                    success = false,
                    errors = new string[] { "error" }
                }
            });
        }

        [Fact]
        public async Task InternalLogin_WhenModelStateisInvalidAsync()
        {
            _httpRequestBaseMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://mark"));
            _subject.ModelState.AddModelError("test", "not valid");

            var result = await _subject.InternalLoginAsync(new LoginViewModel());

            result.Should().BeEquivalentTo(new JsonResult()
            {
                Data = new
                {
                    success = false,
                    errors = new string[] { "not valid" }
                }
            });
        }

        [Fact]
        public async Task InternalLogin_WhenPasswodFailureAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };

            _httpRequestBaseMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://mark.com?returnUrl=/"));
            _customerService.Setup(x => x.GetSiteUser(It.IsAny<string>())).Returns(siteUser);
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                userPasswordStore.Setup(x => x.GetPasswordHashAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(new PasswordHasher().HashPassword("passwords")));
                var lockoutStore = store.As<IUserLockoutStore<SiteUser, string>>();
                lockoutStore.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(false));
                lockoutStore.Setup(x => x.GetAccessFailedCountAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(0));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            _localizationService.Setup(x => x.GetStringByCulture(It.IsAny<string>(), It.IsAny<FallbackBehaviors>(), It.IsAny<string>(), It.IsAny<CultureInfo>())).Returns("You have entered wrong username or password");
            var result = await _subject.InternalLoginAsync(new LoginViewModel
            {
                Password = "password",
                RememberMe = true,
            });

            result.Should().BeEquivalentTo(new JsonResult()
            {
                Data = new
                {
                    success = false,
                    errors = new[] { "You have entered wrong username or password" }
                }
            });
        }

        [Fact]
        public async Task InternalLogin_WhenLockedOutAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };

            _httpRequestBaseMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://mark.com?returnUrl=/"));
            _customerService.Setup(x => x.GetSiteUser(It.IsAny<string>())).Returns(siteUser);
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                userPasswordStore.Setup(x => x.GetPasswordHashAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(new PasswordHasher().HashPassword("password")));
                var lockoutStore = store.As<IUserLockoutStore<SiteUser, string>>();
                lockoutStore.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(true));
                lockoutStore.Setup(x => x.GetLockoutEndDateAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(DateTimeOffset.UtcNow.AddDays(30)));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            Func<Task<ActionResult>> action = async ()=> await _subject.InternalLoginAsync(new LoginViewModel
            {
                Password = "password",
                RememberMe = true,
            });

            action.Should().Throw<Exception>().WithMessage("Account is locked out."); ;
        }

        [Fact]
        public async Task InternalLogin_WhenSuccessAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };

            _httpRequestBaseMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://mark.com?returnUrl=/"));
            _customerService.Setup(x => x.GetSiteUser(It.IsAny<string>())).Returns(siteUser);
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                userPasswordStore.Setup(x => x.GetPasswordHashAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(new PasswordHasher().HashPassword("password")));
                var lockoutStore = store.As<IUserLockoutStore<SiteUser, string>>();
                lockoutStore.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(false));
                lockoutStore.Setup(x => x.GetAccessFailedCountAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(0));
                var twoFactorStore = store.As<IUserTwoFactorStore<SiteUser, string>>();
                twoFactorStore.Setup(x => x.GetTwoFactorEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(false));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.InternalLoginAsync(new LoginViewModel
            {
                Password = "password",
                RememberMe = true,
            });

            result.Should().BeEquivalentTo(new JsonResult()
            {
                Data = new
                {
                    success = true,
                }
            });
        }

        [Fact]
        public async Task InternalLogin_WhenUserIsNullAsync()
        {
            _httpRequestBaseMock.Setup(x => x.UrlReferrer).Returns(new Uri("http://mark.com?returnUrl=/"));
            _customerService.Setup(x => x.GetSiteUser(It.IsAny<string>())).Returns<SiteUser>(null);
            _localizationService.Setup(x => x.GetStringByCulture(It.IsAny<string>(), It.IsAny<FallbackBehaviors>(), It.IsAny<string>(), It.IsAny<CultureInfo>())).Returns("You have entered wrong username or password");
            var result = await _subject.InternalLoginAsync(new LoginViewModel
            {
                Password = "password",
                RememberMe = true,
            });

            result.Should().BeEquivalentTo(new JsonResult()
            {
                Data = new
                {
                    success = false,
                    errors = new[] { "You have entered wrong username or password" }
                }
            });
        }

        [Fact]
        public void ExternalLogin_WhenSuccess()
        {
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(x => x.Action(It.IsAny<string>(), It.IsAny<object>())).Returns("/publicapi/ExternalLoginCallback?returnUrl=/");
            _subject.Url = urlHelper.Object;
            var result = _subject.ExternalLogin("mark", "/");
            result.Should().BeEquivalentTo(new ChallengeResult("mark", "/publicapi/ExternalLoginCallback?returnUrl=/"));
        }

        [Fact]
        public async Task ExternalLoginCallback_WhenLoginInfoIsNullAsync()
        {
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult<ExternalLoginInfo>(null));
            var result = await _subject.ExternalLoginCallbackAsync("/");

            result.Should().BeEquivalentTo(new RedirectResult("/"));
        }

        [Fact]
        public async Task ExternalLoginCallback_WhenSuccessAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult<ExternalLoginInfo>(null));
            var userManager = Setup_UserManager((store) =>
            {
                var loginStore = store.As<IUserLoginStore<SiteUser,string>>();
                loginStore.Setup(x => x.FindAsync(It.IsAny<UserLoginInfo>())).Returns(Task.FromResult(siteUser));
                var lockoutStore = store.As<IUserLockoutStore<SiteUser, string>>();
                lockoutStore.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(false));
                lockoutStore.Setup(x => x.GetAccessFailedCountAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(0));
                var twoFactorStore = store.As<IUserTwoFactorStore<SiteUser, string>>();
                twoFactorStore.Setup(x => x.GetTwoFactorEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(false));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.ExternalLoginCallbackAsync("/");
            result.Should().BeEquivalentTo(new RedirectResult("/"));
        }

        [Fact]
        public async Task ExternalLoginCallback_WhenLockedOutAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult(new ExternalLoginInfo()));
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                var loginStore = store.As<IUserLoginStore<SiteUser, string>>();
                loginStore.Setup(x => x.FindAsync(It.IsAny<UserLoginInfo>())).Returns(Task.FromResult(siteUser));
                var lockoutStore = store.As<IUserLockoutStore<SiteUser, string>>();
                lockoutStore.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(true));
                lockoutStore.Setup(x => x.GetLockoutEndDateAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(DateTimeOffset.UtcNow.AddDays(30)));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.ExternalLoginCallbackAsync("/");
            result.Should().BeEquivalentTo(new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "Lockout",
                controller = "Login"
            })));
        }

        [Fact]
        public async Task ExternalLoginCallback_WhenRequiresVerificationAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };

            _authenticationManger.Setup(x => x.AuthenticateAsync(It.IsAny<string>())).Returns(Task.FromResult(new AuthenticateResult(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, "test@email.com") }), new AuthenticationProperties(), new AuthenticationDescription())));
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult(new ExternalLoginInfo()));
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                var loginStore = store.As<IUserLoginStore<SiteUser, string>>();
                loginStore.Setup(x => x.FindAsync(It.IsAny<UserLoginInfo>())).Returns(Task.FromResult(siteUser));
                var lockoutStore = store.As<IUserLockoutStore<SiteUser, string>>();
                lockoutStore.Setup(x => x.GetLockoutEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(false));
                lockoutStore.Setup(x => x.GetAccessFailedCountAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(0));
                var twoFactorStore = store.As<IUserTwoFactorStore<SiteUser, string>>();
                twoFactorStore.Setup(x => x.GetTwoFactorEnabledAsync(It.IsAny<SiteUser>())).Returns(Task.FromResult(true));
            });

            var twoFactorMock = new Mock<IUserTokenProvider<SiteUser, string>>();
            twoFactorMock.Setup(x => x.IsValidProviderForUserAsync(It.IsAny<UserManager<SiteUser, string>>(), It.IsAny<SiteUser>())).Returns(Task.FromResult(true));
            userManager.TwoFactorProviders.Add("test", twoFactorMock.Object);
            
            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.ExternalLoginCallbackAsync("/");
            result.Should().BeEquivalentTo(new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "SendCode",
                controller = "Login",
                ReturnUrl = "/",
                RememberMe = false
            })));
        }

        [Fact]
        public async Task ExternalLoginCallback_WhenAccountDoesNotExistAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };

            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult(new ExternalLoginInfo { Login = new UserLoginInfo("test", "test")}));
            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                var loginStore = store.As<IUserLoginStore<SiteUser, string>>();
                loginStore.Setup(x => x.FindAsync(It.IsAny<UserLoginInfo>())).Returns(Task.FromResult<SiteUser>(null));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
                .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.ExternalLoginCallbackAsync("/");
            var viewData = new ViewDataDictionary(new ExternalLoginConfirmationViewModel { ReturnUrl = "/" });
            viewData.Add("ReturnUrl", "/");
            viewData.Add("LoginProvider", "test");
            result.Should().BeEquivalentTo(new ViewResult
            {
                ViewName = "ExternalLoginConfirmation",
                ViewData = viewData
            });
        }

        [Fact]
        public async Task ExternalLoginConfirmation_WhenUserIsAuthenticatedAsync()
        {
            _httpContextBase.Setup(x => x.User).Returns(new GenericPrincipal(new GenericIdentity("test@email.com"), new[] { "admin" }));
            var result = await _subject.ExternalLoginConfirmationAsync(null);
            result.Should().BeEquivalentTo(new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "Index",
                controller = "Manage",
            })));
        }

        [Fact]
        public async Task ExternalLoginConfirmation_WhenModelStateIsInvalidAsync()
        {
            _httpContextBase.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));
            _subject.ModelState.AddModelError("test", "not valid");
            var result = await _subject.ExternalLoginConfirmationAsync(new ExternalLoginConfirmationViewModel());
            result.Should().BeEquivalentTo(new ViewResult
            {
                ViewData = new ViewDataDictionary(new ExternalLoginConfirmationViewModel())
            });
        }

        [Fact]
        public async Task ExternalLoginConfirmation_WhenExternalLoginIsNullAsync()
        {
            _httpContextBase.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult<ExternalLoginInfo>(null));
            var result = await _subject.ExternalLoginConfirmationAsync(null);
            result.Should().BeEquivalentTo(new ViewResult
            {
                ViewName = "ExternalLoginFailure",
            });
        }

        [Fact]
        public async Task ExternalLoginConfirmation_WhenSuccessAsync()
        {
            var siteUser = new SiteUser
            {
                Username = "test@email.com",
                Email = "test@email.com"
            };

            _httpContextBase.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult(new ExternalLoginInfo
            {
                Login = new UserLoginInfo("test", "test"),
                ExternalIdentity = new ClaimsIdentity(new List<Claim>() 
                {
                    new Claim(ClaimTypes.Email, "test@email.com"),
                    new Claim(ClaimTypes.Name, "test user")
                }, DefaultAuthenticationTypes.ApplicationCookie)
            }));
            
            var identityResult = GetIdentityResult();

            _customerService.Setup(x => x.CreateUserAsync(It.IsAny<SiteUser>()))
                .Returns(Task.FromResult(new IdentityContactResult
                {
                    IdentityResult = identityResult,
                    FoundationContact = FoundationContact.New()
                }));

            var userManager = Setup_UserManager((store) =>
            {
                var userPasswordStore = store.As<IUserPasswordStore<SiteUser>>();
                userPasswordStore.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(siteUser));
                var loginStore = store.As<IUserLoginStore<SiteUser, string>>();
                loginStore.Setup(x => x.FindAsync(It.IsAny<UserLoginInfo>())).Returns(Task.FromResult<SiteUser>(null));
            });

            _customerService.Setup(_ => _.UserManager)
                .Returns(() => Setup_ServiceAccessors(userManager));

            _customerService.Setup(_ => _.SignInManager)
               .Returns(() => Setup_ServiceAccessors(Setup_SignInManager(userManager)));

            var result = await _subject.ExternalLoginConfirmationAsync(new ExternalLoginConfirmationViewModel() {  ReturnUrl = "/" });
            result.Should().BeEquivalentTo(new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "Index",
                node = ContentReference.StartPage,
            })));
        }

        [Fact]
        public async Task ExternalLoginConfirmation_WhenFailureAsync()
        {
            _httpContextBase.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));
            _customerService.Setup(x => x.GetExternalLoginInfoAsync()).Returns(Task.FromResult(new ExternalLoginInfo
            {
                Login = new UserLoginInfo("test", "test"),
                ExternalIdentity = new ClaimsIdentity(new List<Claim>() 
                {
                    new Claim(ClaimTypes.Email, "test@email.com"),
                    new Claim(ClaimTypes.Name, "test user")
                }, DefaultAuthenticationTypes.ApplicationCookie)
            }));

            _customerService.Setup(x => x.CreateUserAsync(It.IsAny<SiteUser>()))
                .Returns(Task.FromResult(new IdentityContactResult
                {
                    IdentityResult = new IdentityResult("error"),
                    FoundationContact = FoundationContact.New()
                }));
            
            var result = await _subject.ExternalLoginConfirmationAsync(new ExternalLoginConfirmationViewModel());
            _subject.ModelState.Count.Should().Be(1);
        }

        [Fact]
        public async Task TrackHeroBlock_WhenSuccess()
        {
            var result = await _subject.TrackHeroBlockAsync("id","block", "page");
            result.Should().BeEquivalentTo(new ContentResult()
            {
               Content = "block"
            });
        }

        [Fact]
        public async Task TrackVideoBlock_WhenSuccess()
        {
            var result = await _subject.TrackVideoBlockAsync("id", "block", "page");
            result.Should().BeEquivalentTo(new ContentResult()
            {
                Content = "block"
            });
        }

        public PublicApiControllerTests()
        {
            _addressBookService = new Mock<IAddressBookService>();
            _campaignService = new Mock<ICampaignService>();
            _cmsTrackingService = new Mock<ICmsTrackingService>();
            _contentLoader = new Mock<IContentLoader>();
            _customerService = new Mock<ICustomerService>();
            _httpContextBase = new Mock<HttpContextBase>();
            _localizationService = new Mock<LocalizationService>(null);
            _urlResolver = new Mock<IUrlResolver>();
            _trackingCookieService = new Mock<ITrackingCookieService>();
            _authenticationManger = new Mock<IAuthenticationManager>();
            _httpRequestBaseMock = new Mock<HttpRequestBase>();
            _httpContextBase.SetupGet(x => x.Request).Returns(_httpRequestBaseMock.Object);

            _subject = new PublicApiController(_localizationService.Object,
                _contentLoader.Object,
                _addressBookService.Object,
                _customerService.Object,
                _campaignService.Object,
                _urlResolver.Object,
                _cmsTrackingService.Object,
                _httpContextBase.Object,
                _trackingCookieService.Object);
            _subject.ControllerContext = new ControllerContext(_httpContextBase.Object, new RouteData(), _subject);
        }

        private PublicApiController _subject;
        private Mock<IContentLoader> _contentLoader;
        private Mock<IAddressBookService> _addressBookService;
        private Mock<LocalizationService> _localizationService;
        private Mock<ICustomerService> _customerService;
        private Mock<ICampaignService> _campaignService;
        private Mock<IUrlResolver> _urlResolver;
        private Mock<ICmsTrackingService> _cmsTrackingService;
        private Mock<HttpContextBase> _httpContextBase;
        private Mock<ITrackingCookieService> _trackingCookieService;
        private Mock<HttpRequestBase> _httpRequestBaseMock;
        private Mock<IAuthenticationManager> _authenticationManger;
        private ServiceAccessor<T> Setup_ServiceAccessors<T>(T value) => new ServiceAccessor<T>(() => value);

        private ApplicationUserManager<SiteUser> Setup_UserManager(Action<Mock<IUserStore<SiteUser>>> configureStore = null)
        {
            var userStore = new Mock<IUserStore<SiteUser>>();
            configureStore?.Invoke(userStore);
            return new ApplicationUserManager<SiteUser>(userStore.Object);
        }

        private ApplicationSignInManager<SiteUser> Setup_SignInManager(ApplicationUserManager<SiteUser> userManager) => new ApplicationSignInManager<SiteUser>(userManager, _authenticationManger.Object, new ApplicationOptions());

        private static IdentityResult GetIdentityResult()
        {
            var identityResult = new IdentityResult();
            typeof(IdentityResult).GetProperty("Succeeded").SetValue(identityResult, true, null);
            return identityResult;
        }
    }
}
