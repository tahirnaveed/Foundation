using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Routing;
using Foundation.Social;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.Models.Blocks;
using Foundation.Social.Repositories.ActivityStreams;
using Foundation.Social.Repositories.Common;
using Foundation.Social.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    /// <summary>
    /// The SubscriptionBlockController handles the rendering of the subscription block frontend view as well
    /// as the posting of new subscriptions.
    /// </summary>
    [TemplateDescriptor(Default = true)]
    public class SubscriptionBlockController : SocialBlockController<SubscriptionBlock>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPageSubscriptionRepository _subscriptionRepository;
        private readonly IPageRepository _pageRepository;

        private const string _actionSubscribe = "Subscribe";
        private const string _actionUnsubscribe = "Unsubscribe";
        private const string _submitSuccessMessage = "Your request was processed successfully!";
        private const string _messageKey = "SubscriptionBlock";
        private const string _errorMessage = "Error";
        private const string _successMessage = "Success";

        /// <summary>
        /// Constructor
        /// </summary>
        public SubscriptionBlockController(IUserRepository userRepository,
            IPageSubscriptionRepository pageSubscriptionRepository,
            IPageRepository pageRepository,
            IPageRouteHelper pageRouteHelper) : base(pageRouteHelper)
        {
            _userRepository = userRepository;
            _subscriptionRepository = pageSubscriptionRepository;
            _pageRepository = pageRepository;
        }

        /// <summary>
        /// Render the subscription block frontend view.
        /// </summary>
        /// <param name="currentContent">The current frontend block instance.</param>
        /// <returns>The action's result.</returns>
        [HttpGet]
        public override ActionResult Index(SubscriptionBlock currentContent)
        {
            // Create a subscription block view model to fill the frontend block view
            var blockViewModel = new SubscriptionBlockViewModel(currentContent, _pageRouteHelper.PageLink)
            {
                //get messages for view
                Messages = RetrieveMessages(_messageKey)
            };

            // Set Block View Model Properties
            SetBlockViewModelProperties(blockViewModel);

            // Render the frontend block view
            return PartialView("~/Features/Blocks/Views/SubscriptionBlock.cshtml", blockViewModel);
        }

        /// <summary>
        /// Subscribes the current user to the current page. It accepts a subscription form model,
        /// validates the form, stores the submitted subscription, and redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The subscription form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(SubscriptionFormViewModel formViewModel)
        {
            if (formViewModel is null)
            {
                throw new System.ArgumentNullException(nameof(formViewModel));
            }

            return HandleAction(_actionSubscribe, formViewModel);
        }

        /// <summary>
        /// Unsubscribes the current user from the current page. It accepts a subscription form model,
        /// validates the form, stores the submitted subscription, and redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The subscription form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unsubscribe(SubscriptionFormViewModel formViewModel)
        {
            if (formViewModel is null)
            {
                throw new System.ArgumentNullException(nameof(formViewModel));
            }

            return HandleAction(_actionUnsubscribe, formViewModel);
        }

        /// <summary>
        /// Handle subscribe/unsubscribe actions.
        /// </summary>
        /// <param name="actionName">The action.</param>
        /// <param name="formViewModel">The form view model.</param>
        /// <returns>The action result.</returns>
        private ActionResult HandleAction(string actionName, SubscriptionFormViewModel formViewModel)
        {
            var subscription = new PageSubscription
            {
                Subscriber = _userRepository.GetUserId(User),
                Target = _pageRepository.GetPageId(formViewModel.CurrentLink)
            };

            try
            {
                if (actionName == _actionSubscribe)
                {
                    _subscriptionRepository.Add(subscription);
                }
                else
                {
                    _subscriptionRepository.Remove(subscription);
                }
                AddMessage(_messageKey, new MessageViewModel(_submitSuccessMessage, _successMessage));
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(_messageKey, new MessageViewModel(ex.Message, _errorMessage));
            }

            return Redirect(UrlResolver.Current.GetUrl(formViewModel.CurrentLink));
        }

        /// <summary>
        /// Set any properties the block view model needs for the view to render properly.
        /// </summary>
        /// <param name="blockViewModel">The subscription block view model.</param>
        private void SetBlockViewModelProperties(SubscriptionBlockViewModel blockViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                blockViewModel.ShowSubscriptionForm = true;
                SetUserSubscribedToPage(blockViewModel);
            }
        }

        /// <summary>
        /// Set the block view  model property indicating whether the current user is subscribed to the current page.
        /// </summary>
        /// <param name="blockViewModel">The subscription block view model.</param>
        private void SetUserSubscribedToPage(SubscriptionBlockViewModel blockViewModel)
        {
            try
            {
                var filter = new PageSubscriptionFilter
                {
                    Subscriber = _userRepository.GetUserId(User),
                    Target = _pageRepository.GetPageId(blockViewModel.CurrentLink)
                };

                if (_subscriptionRepository.Exist(filter))
                {
                    blockViewModel.UserSubscribedToPage = true;
                }
                else
                {
                    blockViewModel.UserSubscribedToPage = false;
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.Messages.Add(new MessageViewModel(ex.Message, _errorMessage));
            }
        }
    }
}
