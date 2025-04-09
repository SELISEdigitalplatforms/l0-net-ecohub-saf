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

}
