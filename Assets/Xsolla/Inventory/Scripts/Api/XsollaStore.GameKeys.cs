﻿using System;
using Xsolla.Core;

namespace Xsolla.Store
{
	public partial class XsollaStore : MonoSingleton<XsollaStore>
	{
		private const string URL_GET_GAMES_LIST = BASE_STORE_API_URL + "/items/game?limit={1}&offset={2}";
		private const string URL_GET_GAMES_BY_GROUP = BASE_STORE_API_URL + "/items/game/group/{1}?limit={2}&offset={3}";
		private const string URL_GET_GAME_FOR_CATALOG = BASE_STORE_API_URL + "/items/game/sku/{1}";
		private const string URL_GET_GAME_KEY_CATALOG = BASE_STORE_API_URL + "/items/game/key/sku/{1}";
		private const string URL_GET_GAME_KEYS_BY_GROUP = BASE_STORE_API_URL + "/items/game/key/group/{1}?limit={2}&offset={3}";
		private const string URL_GET_USER_OWNED_GAMES = BASE_STORE_API_URL + "/entitlement?sandbox={1}&limit={2}&offset={3}";
		private const string URL_GET_DRM_LIST = BASE_STORE_API_URL + "/items/game/drm";
		private const string URL_REDEEM_GAME_CODE = BASE_STORE_API_URL + "/entitlement/redeem";

		/// <summary>
		/// Gets a games list for building a catalog.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/commerce-api/game-keys/catalog/get-games-list/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <param name="limit">Limit for the number of elements on the page.</param>
		/// <param name="offset">Number of the element from which the list is generated (the count starts from 0).</param>
		/// <param name="locale">Response language. Two-letter lowercase language code per ISO 639-1.</param>
		/// <param name="additionalFields">The list of additional fields. These fields will be in a response if you send them in a request. Available fields 'media_list', 'order', and 'long_description'.</param>
		/// <param name="country">Country used to calculate regional prices and restrictions for the catalog. Two-letter uppercase country code per ISO 3166-1 alpha-2. If you do not specify the country explicitly, it will be defined by the user IP address.</param>
		public void GetGamesList(string projectId, Action<GameItems> onSuccess, Action<Error> onError = null, int limit = 50, int offset = 0, string locale = null, string additionalFields = "long_description", string country = null)
		{
			var url = string.Format(URL_GET_GAMES_LIST, projectId, limit, offset);
			var localeParam = GetLocaleUrlParam(locale);
			var additionalFieldsParam = GetAdditionalFieldsParam(additionalFields);
			var countryParam = GetCountryUrlParam(country);
			url = ConcatUrlAndParams(url, localeParam, additionalFieldsParam, countryParam);

			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Gets a games list from the specified group for building a catalog.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/in-game-store-buy-button-api/game-keys/catalog/get-games-group/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="groupId">Group external ID.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <param name="limit">Limit for the number of elements on the page.</param>
		/// <param name="offset">Number of the element from which the list is generated (the count starts from 0).</param>
		/// <param name="locale">Response language. Two-letter lowercase language code per ISO 639-1.</param>
		/// <param name="additionalFields">The list of additional fields. These fields will be in a response if you send them in a request. Available fields 'media_list', 'order', and 'long_description'.</param>
		/// <param name="country">Country used to calculate regional prices and restrictions for the catalog. Two-letter uppercase country code per ISO 3166-1 alpha-2. If you do not specify the country explicitly, it will be defined by the user IP address.</param>
		public void GetGamesListBySpecifiedGroup(string projectId, string groupId, Action<GameItems> onSuccess, Action<Error> onError = null, int limit = 50, int offset = 0, string locale = null, string additionalFields = "long_description", string country = null)
		{
			var url = string.Format(URL_GET_GAMES_BY_GROUP, projectId, groupId, limit, offset);
			var localeParam = GetLocaleUrlParam(locale);
			var additionalFieldsParam = GetAdditionalFieldsParam(additionalFields);
			var countryParam = GetCountryUrlParam(country);
			url = ConcatUrlAndParams(url, localeParam, additionalFieldsParam, countryParam);

			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Gets a game for the catalog.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/in-game-store-buy-button-api/game-keys/catalog/get-game-by-sku/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="itemSku">Item SKU.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <param name="locale">Response language. Two-letter lowercase language code per ISO 639-1.</param>
		/// <param name="additionalFields">The list of additional fields. These fields will be in a response if you send them in a request. Available fields 'media_list', 'order', and 'long_description'.</param>
		/// <param name="country">Country used to calculate regional prices and restrictions for the catalog. Two-letter uppercase country code per ISO 3166-1 alpha-2. If you do not specify the country explicitly, it will be defined by the user IP address.</param>
		public void GetGameForCatalog(string projectId, string itemSku, Action<GameItem> onSuccess, Action<Error> onError = null, string locale = null, string additionalFields = "long_description", string country = null)
		{
			var url = string.Format(URL_GET_GAME_FOR_CATALOG, projectId, itemSku);
			var localeParam = GetLocaleUrlParam(locale);
			var additionalFieldsParam = GetAdditionalFieldsParam(additionalFields);
			var countryParam = GetCountryUrlParam(country);
			url = ConcatUrlAndParams(url, localeParam, additionalFieldsParam, countryParam);

			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Gets a game key for the catalog.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/in-game-store-buy-button-api/game-keys/catalog/get-game-key-by-sku/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="itemSku">Item SKU.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <param name="locale">Response language. Two-letter lowercase language code per ISO 639-1.</param>
		/// <param name="additionalFields">The list of additional fields. These fields will be in a response if you send them in a request. Available fields 'media_list', 'order', and 'long_description'.</param>
		/// <param name="country">Country used to calculate regional prices and restrictions for the catalog. Two-letter uppercase country code per ISO 3166-1 alpha-2. If you do not specify the country explicitly, it will be defined by the user IP address.</param>
		public void GetGameKeyForCatalog(string projectId, string itemSku, Action<GameKeyItems> onSuccess, Action<Error> onError = null, string locale = null, string additionalFields = "long_description", string country = null)
		{
			var url = string.Format(URL_GET_GAME_KEY_CATALOG, projectId, itemSku);
			var localeParam = GetLocaleUrlParam(locale);
			var additionalFieldsParam = GetAdditionalFieldsParam(additionalFields);
			var countryParam = GetCountryUrlParam(country);
			url = ConcatUrlAndParams(url, localeParam, additionalFieldsParam, countryParam);

			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Gets a game key list from the specified group for building a catalog.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/in-game-store-buy-button-api/game-keys/catalog/get-game-keys-group/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="groupId">Group external ID.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <param name="limit">Limit for the number of elements on the page.</param>
		/// <param name="offset">Number of the element from which the list is generated (the count starts from 0).</param>
		/// <param name="locale">Response language. Two-letter lowercase language code per ISO 639-1.</param>
		/// <param name="additionalFields">The list of additional fields. These fields will be in a response if you send them in a request. Available fields 'media_list', 'order', and 'long_description'.</param>
		/// <param name="country">Country used to calculate regional prices and restrictions for the catalog. Two-letter uppercase country code per ISO 3166-1 alpha-2. If you do not specify the country explicitly, it will be defined by the user IP address.</param>
		public void GetGameKeysListBySpecifiedGroup(string projectId, string groupId, Action<GameKeyItems> onSuccess, Action<Error> onError = null, int limit = 50, int offset = 0, string locale = null, string additionalFields = "long_description", string country = null)
		{
			var url = string.Format(URL_GET_GAME_KEYS_BY_GROUP, projectId, groupId, limit, offset);
			var localeParam = GetLocaleUrlParam(locale);
			var additionalFieldsParam = GetAdditionalFieldsParam(additionalFields);
			var countryParam = GetCountryUrlParam(country);
			url = ConcatUrlAndParams(url, localeParam, additionalFieldsParam, countryParam);

			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Get the list of games owned by the user. The response will contain an array of games owned by a particular user.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/in-game-store-buy-button-api/game-keys/entitlement/get-user-games/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="sandbox">What type of entitlements should be returned. If the parameter is set to true, the entitlements received by the user in the sandbox mode only are returned. If the parameter isn't passed or is set to false, the entitlements received by the user in the live mode only are returned."</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <param name="limit">Limit for the number of elements on the page.</param>
		/// <param name="offset">Number of the element from which the list is generated (the count starts from 0).</param>
		/// <param name="additionalFields">The list of additional fields. These fields will be in a response if you send them in a request. Available fields 'media_list', 'order', and 'long_description'.</param>
		public void GetOwnedGames(string projectId, bool sandbox, Action<GameOwnership> onSuccess, Action<Error> onError = null, int limit = 50, int offset = 0, string additionalFields = "long_description")
		{
			var sandboxFlag = sandbox ? "1" : "0";
			var url = string.Format(URL_GET_USER_OWNED_GAMES, projectId, sandboxFlag, limit, offset);
			var additionalFieldsParam = GetAdditionalFieldsParam(additionalFields);
			url = ConcatUrlAndParams(url, additionalFieldsParam);

			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Grants entitlement by a provided game code.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/in-game-store-buy-button-api/game-keys/catalog/get-drm-list/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void GetDrmList(string projectId, Action<DrmItems> onSuccess, Action<Error> onError = null)
		{
			var url = string.Format(URL_GET_DRM_LIST, projectId);
			WebRequestHelper.Instance.GetRequest(SdkType.Store, url, onSuccess, onError, Error.ItemsListErrors);
		}

		/// <summary>
		/// Grants entitlement by a provided game code.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/commerce-api/game-keys/entitlement/redeem-game-pin-code/"/>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="gameCode">Game code.</param>
		/// <param name="sandbox">Redeem game code in the sandbox mode. The option is available for those users who are specified in the list of company users.</param>
		/// <param name="onSuccess">Successful operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void RedeemGameCode(string projectId, string gameCode, bool sandbox, Action onSuccess, Action<Error> onError = null)
		{
			var url = string.Format(URL_REDEEM_GAME_CODE, projectId);
			var data = new RedeemGameCodeRequest(gameCode, sandbox);

			WebRequestHelper.Instance.PostRequest(SdkType.Login, url, data, WebRequestHeader.AuthHeader(Token.Instance), onSuccess, onError, Error.LoginErrors);
		}
	}
}