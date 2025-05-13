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

    public static SafOfferNlpiEncryptedKafkaEvent MapToSafKafkaOfferNlpiEncryptedEvent(this SafOfferNlpiEvent eventPayload)
    {
        if (eventPayload == null) throw new ArgumentNullException(nameof(eventPayload));

        return new SafOfferNlpiEncryptedKafkaEvent
        {
            id = eventPayload.Id,
            source = eventPayload.Source,
            specversion = eventPayload.Specversion,
            type = eventPayload.Type,
            datacontenttype = eventPayload.DataContentType,
            dataschema = eventPayload.DataSchema,
            subject = eventPayload.Subject,
            time = eventPayload.Time,
            licenceKey = eventPayload.LicenceKey,
            userAgent = new SafUserKafkaAgent
            {
                name = eventPayload.UserAgent.Name,
                version = eventPayload.UserAgent.Version
            },
            eventReceiver = new SafEventKafkaReceiver
            {
                category = eventPayload.EventReceiver.Category,
                id = eventPayload.EventReceiver.Id
            },
            eventSender = new SafEventKafkaSender
            {
                category = eventPayload.EventSender.Category,
                id = eventPayload.EventSender.Id
            },
            processId = eventPayload.ProcessId,
            processStatus = eventPayload.ProcessStatus,
            subProcessName = eventPayload.SubProcessName,
            processName = eventPayload.ProcessName,
            subProcessStatus = eventPayload.SubProcessStatus
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
