using System;
using SeliseBlocks.Ecohub.Saf.Models.RequestModels;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for the ISafDriverService, which provides methods to interact with the SAF API.
/// </summary>
public interface ISafDriverService
{
    /// <summary>
    /// Asynchronously retrieves receiver information from the SAF API.
    /// This method sends a request to the SAF API to get information about receivers based on the provided request parameters.
    /// The request should include the necessary authentication details and any other required parameters.
    /// </summary>
    /// <param name="request">
    /// The request object containing the URL and body for obtaining receiver information.
    /// The body of the request should be of type SafReceiversRequestBody, which contains properties such as receiver ID, status, and other relevant information.
    /// The request URL should be the endpoint for obtaining receiver information from the SAF API.
    /// The bearer token should be the token obtained from the SAF API after successful authentication.
    /// </param>
    /// <returns>
    /// A Task of type SafReceiversResponse, which contains the receiver information retrieved from the SAF API.
    /// The method may throw exceptions if there are issues with the request or response, such as HttpRequestException or JsonException.
    /// </returns>
    Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request);


    /// <summary>
    /// Asynchronously retrieves the public key of a member from the SAF API.
    /// This method sends a request to the SAF API to get the public key of a member based on the provided IDP number.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication.
    /// This token is used to authorize the request to retrieve the member's public key.
    /// </param>
    /// <param name="idpNumber">
    /// The IDP number of the member whose public key is being requested.
    /// This number is used to identify the member in the SAF API.
    /// </param>
    /// <returns>
    /// A Task of type SafMemberPublicKeyResponse, which contains the public key of the member retrieved from the SAF API.
    /// The method may throw exceptions if there are issues with the request or response, such as HttpRequestException or JsonException.
    /// </returns>
    Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber);


    Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);

    Task<SafReceiveOfferNlpiEventResponse> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);




}
