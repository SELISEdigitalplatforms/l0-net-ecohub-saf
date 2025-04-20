using System;

namespace SeliseBlocks.Ecohub.Saf;

internal static class SafOfferNlpiEventMapper
{
    public static SafOfferNlpiEncryptedEvent MapToSafOfferNlpiEncryptedEvent(this SafOfferNlpiEvent eventPayload)
    {
        if (eventPayload == null) throw new ArgumentNullException(nameof(eventPayload));

        return new SafOfferNlpiEncryptedEvent
        {
            Id = eventPayload.Id,
            Source = eventPayload.Source,
            Specversion = eventPayload.Specversion,
            Type = eventPayload.Type,
            DataContentType = eventPayload.DataContentType,
            DataSchema = eventPayload.DataSchema,
            Subject = eventPayload.Subject,
            Time = eventPayload.Time,
            LicenceKey = eventPayload.LicenceKey,
            UserAgent = eventPayload.UserAgent,
            EventReceiver = eventPayload.EventReceiver,
            EventSender = eventPayload.EventSender,
            ProcessId = eventPayload.ProcessId,
            ProcessStatus = eventPayload.ProcessStatus,
            SubProcessName = eventPayload.SubProcessName,
            ProcessName = eventPayload.ProcessName,
            SubProcessStatus = eventPayload.SubProcessStatus
        };
    }

    public static SafOfferNlpiEvent MapToSafOfferNlpiEvent(this SafOfferNlpiEncryptedEvent eventPayload)
    {
        if (eventPayload == null) throw new ArgumentNullException(nameof(eventPayload));

        return new SafOfferNlpiEvent
        {
            Id = eventPayload.Id,
            Source = eventPayload.Source,
            Specversion = eventPayload.Specversion,
            Type = eventPayload.Type,
            DataContentType = eventPayload.DataContentType,
            DataSchema = eventPayload.DataSchema,
            Subject = eventPayload.Subject,
            Time = eventPayload.Time,
            LicenceKey = eventPayload.LicenceKey,
            UserAgent = eventPayload.UserAgent,
            EventReceiver = eventPayload.EventReceiver,
            EventSender = eventPayload.EventSender,
            ProcessId = eventPayload.ProcessId,
            ProcessStatus = eventPayload.ProcessStatus,
            SubProcessName = eventPayload.SubProcessName,
            ProcessName = eventPayload.ProcessName,
            SubProcessStatus = eventPayload.SubProcessStatus
        };
    }
}
