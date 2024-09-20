﻿using GloboTicket.Admin.Mobile.Models;
using GloboTicket.Admin.Mobile.Repositories;

namespace GloboTicket.Admin.Mobile.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public Task<List<EventModel>> GetEvents()
        => _eventRepository.GetEvents();

    public Task<EventModel?> GetEvent(Guid id)
        => _eventRepository.GetEvent(id);

    public Task<bool> UpdateStatus(Guid id, EventStatusModel status)
        => _eventRepository.UpdateStatus(id, status);

    public Task<bool> CreateEvent(EventModel model)
        => _eventRepository.CreateEvent(model);

    public Task<bool> EditEvent(EventModel model)
        => _eventRepository.EditEvent(model);

    public Task<bool> DeleteEvent(Guid id)
        => _eventRepository.DeleteEvent(id);
}
