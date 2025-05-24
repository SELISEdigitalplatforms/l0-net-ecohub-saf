using System;
using SeliseBlocks.Ecohub.Saf;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;


public class SafOfferNlpiEventMapperTests
{
    [Fact]
    public void MapToSafOfferNlpiEncryptedEvent_MapsAllProperties()
    {
        var eventPayload = new SafOfferNlpiEvent
        {
            Id = "id1",
            Source = "src",
            Specversion = "1.0",
            Type = "type",
            DataContentType = "ct",
            DataSchema = "schema",
            Subject = "subj",
            Time = "2024-01-01T00:00:00Z",
            LicenceKey = "lic",
            UserAgent = new SafUserAgent { Name = "n", Version = "v" },
            EventReceiver = new SafEventReceiver { Category = "catR", Id = "idR" },
            EventSender = new SafEventSender { Category = "catS", Id = "idS" },
            ProcessId = "pid",
            ProcessStatus = "pstat",
            SubProcessName = "spn",
            ProcessName = "pn",
            SubProcessStatus = "sps"
        };

        var result = eventPayload.MapToSafOfferNlpiEncryptedEvent();

        Assert.Equal(eventPayload.Id, result.Id);
        Assert.Equal(eventPayload.Source, result.Source);
        Assert.Equal(eventPayload.Specversion, result.Specversion);
        Assert.Equal(eventPayload.Type, result.Type);
        Assert.Equal(eventPayload.DataContentType, result.DataContentType);
        Assert.Equal(eventPayload.DataSchema, result.DataSchema);
        Assert.Equal(eventPayload.Subject, result.Subject);
        Assert.Equal(eventPayload.Time, result.Time);
        Assert.Equal(eventPayload.LicenceKey, result.LicenceKey);
        Assert.Equal(eventPayload.UserAgent, result.UserAgent);
        Assert.Equal(eventPayload.EventReceiver, result.EventReceiver);
        Assert.Equal(eventPayload.EventSender, result.EventSender);
        Assert.Equal(eventPayload.ProcessId, result.ProcessId);
        Assert.Equal(eventPayload.ProcessStatus, result.ProcessStatus);
        Assert.Equal(eventPayload.SubProcessName, result.SubProcessName);
        Assert.Equal(eventPayload.ProcessName, result.ProcessName);
        Assert.Equal(eventPayload.SubProcessStatus, result.SubProcessStatus);
    }

    [Fact]
    public void MapToSafKafkaOfferNlpiEncryptedEvent_MapsAllProperties()
    {
        var eventPayload = new SafOfferNlpiEvent
        {
            Id = "id1",
            Source = "src",
            Specversion = "1.0",
            Type = "type",
            DataContentType = "ct",
            DataSchema = "schema",
            Subject = "subj",
            Time = "2024-01-01T00:00:00Z",
            LicenceKey = "lic",
            UserAgent = new SafUserAgent { Name = "n", Version = "v" },
            EventReceiver = new SafEventReceiver { Category = "catR", Id = "idR" },
            EventSender = new SafEventSender { Category = "catS", Id = "idS" },
            ProcessId = "pid",
            ProcessStatus = "pstat",
            SubProcessName = "spn",
            ProcessName = "pn",
            SubProcessStatus = "sps"
        };

        var result = eventPayload.MapToSafKafkaOfferNlpiEncryptedEvent();

        Assert.Equal(eventPayload.Id, result.id);
        Assert.Equal(eventPayload.Source, result.source);
        Assert.Equal(eventPayload.Specversion, result.specversion);
        Assert.Equal(eventPayload.Type, result.type);
        Assert.Equal(eventPayload.DataContentType, result.datacontenttype);
        Assert.Equal(eventPayload.DataSchema, result.dataschema);
        Assert.Equal(eventPayload.Subject, result.subject);
        Assert.Equal(eventPayload.Time, result.time);
        Assert.Equal(eventPayload.LicenceKey, result.licenceKey);
        Assert.Equal(eventPayload.UserAgent.Name, result.userAgent.name);
        Assert.Equal(eventPayload.UserAgent.Version, result.userAgent.version);
        Assert.Equal(eventPayload.EventReceiver.Category, result.eventReceiver.category);
        Assert.Equal(eventPayload.EventReceiver.Id, result.eventReceiver.id);
        Assert.Equal(eventPayload.EventSender.Category, result.eventSender.category);
        Assert.Equal(eventPayload.EventSender.Id, result.eventSender.id);
        Assert.Equal(eventPayload.ProcessId, result.processId);
        Assert.Equal(eventPayload.ProcessStatus, result.processStatus);
        Assert.Equal(eventPayload.SubProcessName, result.subProcessName);
        Assert.Equal(eventPayload.ProcessName, result.processName);
        Assert.Equal(eventPayload.SubProcessStatus, result.subProcessStatus);
    }

    [Fact]
    public void MapToSafOfferNlpiEvent_MapsAllProperties()
    {
        var encryptedEvent = new SafOfferNlpiEncryptedEvent
        {
            Id = "id1",
            Source = "src",
            Specversion = "1.0",
            Type = "type",
            DataContentType = "ct",
            DataSchema = "schema",
            Subject = "subj",
            Time = "2024-01-01T00:00:00Z",
            LicenceKey = "lic",
            UserAgent = new SafUserAgent { Name = "n", Version = "v" },
            EventReceiver = new SafEventReceiver { Category = "catR", Id = "idR" },
            EventSender = new SafEventSender { Category = "catS", Id = "idS" },
            ProcessId = "pid",
            ProcessStatus = "pstat",
            SubProcessName = "spn",
            ProcessName = "pn",
            SubProcessStatus = "sps"
        };

        var result = encryptedEvent.MapToSafOfferNlpiEvent();

        Assert.Equal(encryptedEvent.Id, result.Id);
        Assert.Equal(encryptedEvent.Source, result.Source);
        Assert.Equal(encryptedEvent.Specversion, result.Specversion);
        Assert.Equal(encryptedEvent.Type, result.Type);
        Assert.Equal(encryptedEvent.DataContentType, result.DataContentType);
        Assert.Equal(encryptedEvent.DataSchema, result.DataSchema);
        Assert.Equal(encryptedEvent.Subject, result.Subject);
        Assert.Equal(encryptedEvent.Time, result.Time);
        Assert.Equal(encryptedEvent.LicenceKey, result.LicenceKey);
        Assert.Equal(encryptedEvent.UserAgent, result.UserAgent);
        Assert.Equal(encryptedEvent.EventReceiver, result.EventReceiver);
        Assert.Equal(encryptedEvent.EventSender, result.EventSender);
        Assert.Equal(encryptedEvent.ProcessId, result.ProcessId);
        Assert.Equal(encryptedEvent.ProcessStatus, result.ProcessStatus);
        Assert.Equal(encryptedEvent.SubProcessName, result.SubProcessName);
        Assert.Equal(encryptedEvent.ProcessName, result.ProcessName);
        Assert.Equal(encryptedEvent.SubProcessStatus, result.SubProcessStatus);
    }

    [Fact]
    public void MapToSafOfferNlpiEncryptedEvent_ThrowsOnNull()
    {
        SafOfferNlpiEvent nullEvent = null;
        Assert.Throws<ArgumentNullException>(() => nullEvent.MapToSafOfferNlpiEncryptedEvent());
    }

    [Fact]
    public void MapToSafKafkaOfferNlpiEncryptedEvent_ThrowsOnNull()
    {
        SafOfferNlpiEvent nullEvent = null;
        Assert.Throws<ArgumentNullException>(() => nullEvent.MapToSafKafkaOfferNlpiEncryptedEvent());
    }

    [Fact]
    public void MapToSafOfferNlpiEvent_ThrowsOnNull()
    {
        SafOfferNlpiEncryptedEvent nullEvent = null;
        Assert.Throws<ArgumentNullException>(() => nullEvent.MapToSafOfferNlpiEvent());
    }
}